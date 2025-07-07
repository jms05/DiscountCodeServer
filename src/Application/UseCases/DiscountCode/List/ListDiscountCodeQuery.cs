using JMS.Domain.Models.DiscountCodes.Repository;
using MediatR;

namespace JMS.Application.UseCases.DiscountCodes.List;

public sealed record ListDiscountCodeQuery(IEnumerable<string>? Names = null, bool IncludeUsed = false) : IRequest<IEnumerable<ListDiscountCodeResponse>>
{
    public ListDiscountCodeFilter ToListDiscountCodeFilter() => new(Names, IncludeUsed);
}