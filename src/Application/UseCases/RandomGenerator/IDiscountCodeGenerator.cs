using JMS.Domain.ErrorTreatment;
using JMS.Domain.Models.DiscountCodes;

namespace JMS.Application.UseCases.RandomGenerator;

public interface IDiscountCodeGenerator
{
    IEnumerable<string> GenerateCodes(int count, int length);
}

public class DiscountCodeGenerator : IDiscountCodeGenerator
{
    // GUID used for unique random seed generation, not ticks because can be seeded again in a malicious way 
    private static readonly ThreadLocal<Random> _random = new(() => new Random(Guid.NewGuid().GetHashCode()));
    private Random Random => _random.Value!;

    public DiscountCodeGenerator()
    {
        
    }

    public IEnumerable<string> GenerateCodes(int count, int length)
    {
        if (count > 2000  || count<=0)
            throw new ValidationException(ValidationMessages.V0002_CodesToGenerateAreBetween0And2000);

        var results = new HashSet<string>();

        while (results.Count < count)
        {
            var code = new string(Enumerable.Range(0, length)
                .Select(_ => DiscountCode.AllowedChars[Random.Next(DiscountCode.AllowedChars.Length)])
                .ToArray());

            if (results.Contains(code))
                continue;

            results.Add(code);
        }

        return results;
    }
}