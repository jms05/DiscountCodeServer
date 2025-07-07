
using JMS.Application.Helpers;
using JMS.Domain.Models.DiscountCodes.Repository;
using MediatR;

namespace JMS.Application.UseCases.DiscountCodes.Get;
public sealed class GetDiscountCodeHandler : IRequestHandler<GetDiscountCodeQuery, GetDiscountCodeResponse?>
{
    private readonly IGetDiscountCode _getDiscountCode;
    private readonly ICustomLogger _logger;

    public GetDiscountCodeHandler(IGetDiscountCode getDiscountCode, ICustomLogger logger)
    {
        _getDiscountCode = getDiscountCode;
        _logger = logger;
    }
    public async Task<GetDiscountCodeResponse?> Handle(GetDiscountCodeQuery request, CancellationToken cancellationToken)
    {
        var discountCode = await _getDiscountCode.ExecuteAsync(request.DiscountCode, false, cancellationToken);
        _logger.Log($"Getting Discount Code with: {request.DiscountCode}");
        GetDiscountCodeResponse? result = null;
        if (discountCode is not null)
        {
            result = new GetDiscountCodeResponse(discountCode);
        }
        else
        {
            _logger.Log("Discount Code not found");
        }

            return result;
    }
}
