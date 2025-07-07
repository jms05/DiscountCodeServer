using JMS.Domain.ErrorTreatment;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JMS.Api.Configurations.ErrorHandler.ExceptionHandlers;

public class ValidationExceptionHandler : IExceptionHandler
{
    public ValidationExceptionHandler() { }

    protected ProblemDetails GetDetails(ValidationException exception, HttpContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "One or more validation errors occurred.",
            Instance = context.Request.Path,
            Status = StatusCodes.Status400BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
        };

        problemDetails.Extensions.Add("messageCode", exception.ErrorCode);

        return problemDetails;
    }

    public virtual bool CanHandle(Exception ex)
    {
        return ex is ValidationException;
    }

    public virtual Task HandleAsync(Exception exception, HttpContext context)
    {
        string errorCode = Guid.NewGuid().ToString();
        if (context.Response.HasStarted || !CanHandle(exception))
        {
            return Task.CompletedTask;
        }

        return WriteResponse(exception as ValidationException ?? throw new ArgumentException("cannot handle", exception), context, errorCode);
    }

    protected virtual object GetExceptionDetails(Exception exception)
    {
        string text = exception.ToString();
        return text.Split(Environment.NewLine);
    }

    private Task WriteResponse(ValidationException exception, HttpContext context, string errorCode)
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
