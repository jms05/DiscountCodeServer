using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.EntityFrameworkCore;

public static class ModelBuilderExtensions
{
    [ExcludeFromCodeCoverage(Justification = "Helper Code")]
    public static void SetColumnNamesSnakeCase(this ModelBuilder modelBuilder)
    {
        if (modelBuilder == null)
        {
            throw new ArgumentNullException(nameof(modelBuilder));
        }

        foreach (IMutableEntityType item in from entity in modelBuilder.Model.GetEntityTypes()
                                            where entity.BaseType == null
                                            select entity)
        {
            if (string.IsNullOrWhiteSpace(item.GetViewName()))
            {
                string input = item.GetTableName().Singularize();
                item.SetTableName(input.ToSnakeCase());
            }
        }

        foreach (IMutableProperty item2 in modelBuilder.Model.GetEntityTypes().SelectMany((IMutableEntityType entity) => entity.GetProperties()))
        {
            StoreObjectIdentifier storeObject = StoreObjectIdentifier.Create(item2.DeclaringEntityType, StoreObjectType.Table).GetValueOrDefault();
            item2.SetColumnName(item2.GetColumnName(in storeObject)!.ToSnakeCase());
        }

        foreach (IMutableKey item3 in modelBuilder.Model.GetEntityTypes().SelectMany((IMutableEntityType entity) => entity.GetKeys()))
        {
            item3.SetName(item3.GetName()!.ToSnakeCase());
        }

        foreach (IMutableForeignKey item4 in modelBuilder.Model.GetEntityTypes().SelectMany((IMutableEntityType entity) => entity.GetForeignKeys()))
        {
            item4.SetConstraintName(item4.GetConstraintName()!.ToSnakeCase());
        }

        foreach (IMutableIndex item5 in modelBuilder.Model.GetEntityTypes().SelectMany((IMutableEntityType entity) => entity.GetIndexes()))
        {
            item5.SetDatabaseName(item5.GetDatabaseName().ToSnakeCase());
        }
    }
}