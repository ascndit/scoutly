using scoutly.Startup;

try
{
    await WebApplication
        .CreateBuilder(args)
        .ConfigureServices()
        .Build()
        .Configure()
        .RunAsync();
}
catch (Exception ex)
    when (ex is not HostAbortedException)
{
    Console.Error.WriteLine(ex.Message);
    throw;
}