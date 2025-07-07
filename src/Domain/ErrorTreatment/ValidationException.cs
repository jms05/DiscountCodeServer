namespace JMS.Domain.ErrorTreatment;
public class ValidationException : Exception
{
    public string ErrorCode { get; private set; }

    public ValidationException((string Code, string Message) validationError, Exception? innerException = null)
        : base(validationError.Message, innerException)
    {
        ErrorCode = validationError.Code;
    }
}
