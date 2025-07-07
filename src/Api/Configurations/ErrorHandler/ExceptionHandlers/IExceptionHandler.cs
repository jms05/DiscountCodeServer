namespace JMS.Api.Configurations.ErrorHandler.ExceptionHandlers;

public interface IExceptionHandler
{
    bool CanHandle(Exception ex);

    Task HandleAsync(Exception exception, HttpContext context);
}
