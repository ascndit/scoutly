namespace scoutly.Abstraction;

using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using scoutly.Abstraction;

public interface IDispatcher
{
    Task<TResult> SendAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default);
}

public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> Methods = new();

    public Task<TResult> SendAsync<TResult>(
        ICommand<TResult> command,
        CancellationToken cancellationToken = default)
    {
        var commandType = command.GetType();

        var method = Methods.GetOrAdd(commandType, static type =>
        {
            return typeof(Dispatcher)
                .GetMethod(nameof(DispatchInternal), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(type, typeof(TResult));
        });

        return (Task<TResult>)method.Invoke(this, [command, cancellationToken])!;
    }

    private Task<TResult> DispatchInternal<TCommand, TResult>(
        TCommand command,
        CancellationToken cancellationToken)
        where TCommand : ICommand<TResult>
    {
        var handler = serviceProvider.GetRequiredService<IHandler<TCommand, TResult>>();

        return handler.HandleAsync(command, cancellationToken);
    }
}