using JMS.Api.Configurations.ErrorHandler.ExceptionHandlers;

namespace JMS.Api.Configurations.ErrorHandler;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly List<IExceptionHandler> _handlers = new List<IExceptionHandler>();

    public ErrorHandlerMiddleware(
        RequestDelegate next)
    {
        _next = next;
        RegisterExceptionHandlers();
    }

    protected void RegisterExceptionHandlers()
    {
        // Custom registrations
        RegisterHandler<ValidationExceptionHandler>();
    }

    private Task HandleException(HttpContext context, Exception ex)
    {
        foreach (IExceptionHandler handler in _handlers)
        {
            if (handler.CanHandle(ex))
            {
                return handler.HandleAsync(ex, context);
            }
        }

        return new DefaultExceptionHandler().HandleAsync(ex, context);
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    protected void RegisterHandler<T>()
    {
        _handlers.Add(Activator.CreateInstance(typeof(T)) as IExceptionHandler ?? throw new ArgumentException(typeof(T).ToString()));
    }
}
