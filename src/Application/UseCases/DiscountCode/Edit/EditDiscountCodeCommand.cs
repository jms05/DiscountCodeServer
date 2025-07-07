using JMS.Application.UseCases.DiscountCodes.Get;
using JMS.Domain.Models.DiscountCodes;
using MediatR;

namespace JMS.Application.UseCases.DiscountCodes.Edit;

public sealed record EditDiscountCodeCommand(string Code, bool Used) : IRequest<EditDiscountCodeResponse>
{
    public void MergeWith(DiscountCode discountCode)
    {
        discountCode.Used = Used;
    }
}
