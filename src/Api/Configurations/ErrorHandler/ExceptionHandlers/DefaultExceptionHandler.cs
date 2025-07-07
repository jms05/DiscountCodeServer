using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JMS.Api.Configurations.ErrorHandler.ExceptionHandlers;

public class DefaultExceptionHandler : IExceptionHandler
{
    public DefaultExceptionHandler() { }

    protected ProblemDetails GetDetails(Exception exception, HttpContext context)
    {
        return new ProblemDetails
        {
            Instance = (string)context.Request.Path,
            Status = 500,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
        };
    }

    public virtual bool CanHandle(Exception ex) => true;

    public virtual Task HandleAsync(Exception exception, HttpContext context)
    {
        string errorCode = Guid.NewGuid().ToString();
        if (context.Response.HasStarted || !CanHandle(exception))
        {
            return Task.CompletedTask;
        }

        return WriteResponse(exception, context, errorCode);
    }

    protected virtual object GetExceptionDetails(Exception exception)
    {
        string text = exception.ToString();
        return text.Split(Environment.NewLine);
    }

    private Task WriteResponse(Exception exception, HttpContext context, string errorCode)
    {
        ProblemDetails details = GetDetails(exception, context);
        EnrichDetails(exception, context, errorCode, details);
        context.Response.StatusCode = details.Status ?? 500;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(JsonSerializer.Serialize(details));
    }

    private void EnrichDetails(Exception exception, HttpContext context, string errorCode, ProblemDetails problemDetails)
    {
        problemDetails.Extensions.Add("errorCode", errorCode);
        problemDetails.Extensions.Add("exception", GetExceptionDetails(exception));
        problemDetails.Extensions.Add("exceptionData", exception.Data);
        problemDetails.Extensions.Add("headers", context.Request.Headers.Select((h) => string.Join(": ", h.Key, (string?)h.Value)));
        problemDetails.Extensions.Add("cookies", context.Request.Cookies.Select((h) => string.Join(": ", h.Key, h.Value)));

    }

}
