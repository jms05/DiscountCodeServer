namespace JMS.Domain.Models.DiscountCodes.Repository;
public interface IUpdateDiscountCode
{
    Task<bool> ExecuteAsync(DiscountCode discountCode, CancellationToken cancellationToken);
}
