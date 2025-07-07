namespace JMS.Application.Helpers.Implementations;

public class AsyncFileLogger : CustomLoggerBase
{

    private readonly string _filePath;

    public AsyncFileLogger(string? filePath)
    {
        _filePath = filePath ?? $"{DateTime.UtcNow:O}_Log.txt";

    }

    protected override async Task ProcessLog(string msg)
    {
        await File.AppendAllTextAsync(_filePath, msg + Environment.NewLine);
    }
}
