namespace JMS.Domain.Models.DiscountCodes.Repository;
public interface IGetDiscountCode
{
    Task<DiscountCode?> ExecuteAsync(string discountCode, bool trackCkanges, CancellationToken cancellationToken);
}
