namespace JMS.Domain.Models.DiscountCodes.Repository;
public interface IAddDiscountCode
{
    Task ExecuteAsync(DiscountCode discountCode, CancellationToken cancellationToken);
    Task ExecuteAsync(IEnumerable<DiscountCode> discountCode, CancellationToken cancellationToken);
}
