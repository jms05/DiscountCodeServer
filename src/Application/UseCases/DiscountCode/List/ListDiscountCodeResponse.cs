using JMS.Domain.Models.DiscountCodes;

namespace JMS.Application.UseCases.DiscountCodes.List;
public sealed record ListDiscountCodeResponse(Guid Id, string Code, bool Used)
{
    public ListDiscountCodeResponse(DiscountCode discountCode) : this(discountCode.Id, discountCode.Code, discountCode.Used) { }
}
