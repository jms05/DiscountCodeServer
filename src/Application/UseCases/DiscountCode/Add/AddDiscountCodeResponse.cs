using JMS.Domain.Models.DiscountCodes;

namespace JMS.Application.UseCases.DiscountCodes.Add;

public sealed record AddDiscountCodeResponse(IEnumerable<string> GeneratedCodes)
{
    public AddDiscountCodeResponse(IEnumerable<DiscountCode> codes) : this(codes.Select(c => c.Code)) { }
}