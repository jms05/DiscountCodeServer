namespace JMS.Domain.Abstraction;

public abstract class EntityBase
{
    private readonly DateTime _createdDate = DateTime.Now;
    private readonly DateTime? _updatedDate = null;

    public Guid Id { get; }
    public DateTime CreatedDate => _createdDate;
    public DateTime? UpdatedDate => _updatedDate;
}
