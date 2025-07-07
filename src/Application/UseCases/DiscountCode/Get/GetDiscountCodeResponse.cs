using JMS.Domain.Models.DiscountCodes;

namespace JMS.Application.UseCases.DiscountCodes.Get;
public sealed record GetDiscountCodeResponse(Guid Id, string Code, bool Used)
{
    public GetDiscountCodeResponse(DiscountCode discountCode) : this(discountCode.Id, discountCode.Code, discountCode.Used) { }
}


