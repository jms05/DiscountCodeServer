namespace JMS.Application.Helpers.Implementations;

public class AsyncConsoleLogger : CustomLoggerBase
{


    public AsyncConsoleLogger()
    {

    }

    protected override Task ProcessLog(string msg)
    {
        Console.WriteLine(msg);
        return Task.CompletedTask;
    }


}
