namespace JMS.Domain.Models.DiscountCodes.Repository;
public interface IDeleteDiscountCode
{
    Task ExecuteAsync(DiscountCode discountCode, CancellationToken cancellationToken);
}
