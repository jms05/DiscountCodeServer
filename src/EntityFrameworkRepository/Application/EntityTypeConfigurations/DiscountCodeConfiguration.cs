using JMS.Domain.Models.DiscountCodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JMS.Plugins.EntityFramework.Application.EntityTypeConfigurations;

internal sealed class DiscountCodeConfiguration : EntityConfigurationBase<DiscountCode>
{
    public override void Configure(EntityTypeBuilder<DiscountCode> builder)
    {
        builder.HasIndex(i => i.Code)
            .IsUnique();
        builder.Property(x => x.Code)
            .HasMaxLength(8);
        builder.Property(x => x.UpdatedDate)
            .IsConcurrencyToken();

        base.Configure(builder);
    }
}
