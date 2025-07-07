using JMS.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JMS.Plugins.EntityFramework.Application.EntityTypeConfigurations;
internal abstract class EntityConfigurationBase<T> : IEntityTypeConfiguration<T> where T : EntityBase
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(p => p.CreatedDate).UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);
        builder.Property(p => p.UpdatedDate).UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);
    }
}