
namespace JMS.Domain.Models.DiscountCodes.Repository;
public interface IListDiscountCode
{
    Task<IEnumerable<DiscountCode>> ExecuteAsync(ListDiscountCodeFilter filter, bool trackChanges = false, CancellationToken cancellationToken=default);
}

public sealed record ListDiscountCodeFilter(
        IEnumerable<string>? Codes= null,
        bool IncludeUsed= false)
{
}
