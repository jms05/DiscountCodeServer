using JMS.Domain.Models.DiscountCodes.Repository;
using MediatR;

namespace JMS.Application.UseCases.DiscountCodes.Edit;

public sealed class EditDiscountCodeHandler : IRequestHandler<EditDiscountCodeCommand, EditDiscountCodeResponse?>
{
    private readonly IGetDiscountCode _getDiscountCode;
    private readonly IUpdateDiscountCode _updateDiscountCode;

    public EditDiscountCodeHandler(IGetDiscountCode getDiscountCode, IUpdateDiscountCode updateDiscountCode)
    {
        _getDiscountCode = getDiscountCode;
        _updateDiscountCode = updateDiscountCode;
    }
    public async Task<EditDiscountCodeResponse?> Handle(EditDiscountCodeCommand request, CancellationToken cancellationToken)
    {
        var discountCode = await _getDiscountCode.ExecuteAsync(request.Code, true, cancellationToken);

        if (discountCode is null)
            return null;

        if (discountCode.Used)
        {
            return new EditDiscountCodeResponse(discountCode.Code, false);
        }
        request.MergeWith(discountCode);

        var updated = await _updateDiscountCode.ExecuteAsync(discountCode, cancellationToken);

        return new EditDiscountCodeResponse(discountCode.Code, updated);
    }
}