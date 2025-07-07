using JMS.Domain.Abstraction;
using JMS.Domain.ErrorTreatment;
using System.Text.RegularExpressions;

namespace JMS.Domain.Models.DiscountCodes;

public class DiscountCode : EntityBase
{
    private readonly static string CodeRegex = "^[A-Z0-9]{7,8}$";
    public static readonly char[] AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
    public DiscountCode(string code)
    {
        if (!Regex.IsMatch(code, CodeRegex))
            throw new ValidationException(ValidationMessages.V0001_CodeLenghtBetween7And8AtoZ0to9);
        Code = code;
    }

    public string Code { get; }
    public bool Used { get; set; }
}
