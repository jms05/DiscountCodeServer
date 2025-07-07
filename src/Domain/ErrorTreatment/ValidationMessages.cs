namespace JMS.Domain.ErrorTreatment;

public static class ValidationMessages
{
    public static readonly (string Code, string Message) V0001_CodeLenghtBetween7And8AtoZ0to9 = (
        "V0001", "Code Lenght Must Be Between 7 and 8 A-Z, 0-9 ");

    public static readonly (string Code, string Message) V0002_CodesToGenerateAreBetween0And2000 = (
        "V0002", "Only can generate 1-2k codes ");
}
