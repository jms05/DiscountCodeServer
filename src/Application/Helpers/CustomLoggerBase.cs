using System.Threading.Channels;

namespace JMS.Application.Helpers;

public abstract class CustomLoggerBase : ICustomLogger
{
    private readonly Channel<string> _logChannel = Channel.CreateUnbounded<string>();
    private readonly Task _worker;
    protected bool _disposed = false;

    protected CustomLoggerBase()
    {
        _worker = Task.Run(ProcessQueueAsync);
    }

    public void Log(string message)
    {
        if (!_disposed)
            _logChannel.Writer.TryWrite($"{DateTime.UtcNow:O} {message}");
    }

    public void Log(string message,  Exception ex)
    {
        if (!_disposed)
            _logChannel.Writer.TryWrite($"{DateTime.UtcNow:O} {message} {ex.Message} {ex.StackTrace}");
    }

    abstract protected Task ProcessLog(string msg);

    private async Task ProcessQueueAsync()
    {
        await foreach (var msg in _logChannel.Reader.ReadAllAsync())
        {
            try
            {
                await ProcessLog(msg);
            }
            catch { /* swallow or handle exceptions */ }
        }
    }


    public void Dispose()
    {
        _disposed = true;
        _logChannel.Writer.Complete();
        _worker.Wait();
    }
}
