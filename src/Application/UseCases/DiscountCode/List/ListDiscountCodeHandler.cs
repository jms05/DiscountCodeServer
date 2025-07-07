using JMS.Application.Helpers;
using JMS.Domain.Models.DiscountCodes.Repository;
using MediatR;

namespace JMS.Application.UseCases.DiscountCodes.List;

public sealed class ListDiscountCodeHandler : IRequestHandler<ListDiscountCodeQuery, IEnumerable<ListDiscountCodeResponse>?>
{

    private readonly IListDiscountCode _listDiscountCode;
    private readonly ICustomLogger _logger;

    public ListDiscountCodeHandler(IListDiscountCode listDiscountCode, ICustomLogger logger)
    {;
        _listDiscountCode = listDiscountCode;
        _logger = logger;
    }

    public async Task<IEnumerable<ListDiscountCodeResponse>?> Handle(ListDiscountCodeQuery request, CancellationToken cancellationToken)
    {
        _logger.Log($"Getting Discount Codes with: {request.Names} (IncludeUsed: {request.IncludeUsed})");
        var result = await _listDiscountCode.ExecuteAsync(request.ToListDiscountCodeFilter(),false,cancellationToken);

        return result?.Select(d => new ListDiscountCodeResponse(d));
    }
}
