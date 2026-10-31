namespace scoutly.Abstraction;

public interface IHandler<TCommand, TCommandResult>
{
    Task<TCommandResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface ICommand<T>
{
    
}

public interface ICommand
{
    
}