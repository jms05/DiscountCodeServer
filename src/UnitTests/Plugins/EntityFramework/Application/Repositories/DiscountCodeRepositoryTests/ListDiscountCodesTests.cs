using EntityFrameworkRepository.Application.Repositories;
using FluentAssertions;
using JMS.Application.Helpers;
using JMS.Domain.Models.DiscountCodes;
using JMS.Domain.Models.DiscountCodes.Repository;
using Moq;
using UnitTests.Helpers;
using Xunit;

namespace UnitTests.Plugins.EntityFramework.Application.Repositories.DiscountCodeRepositoryTests;
public class ListDiscountCodesTests
{
    [Theory]
    [InlineData("12345678", 1)]
    [InlineData("12345679", 0)]
    [InlineData(null, 2)]
    [Trait("Plugin.EntityFramework", "DiscountCodes")]
    public async Task WhenFilteringByCode_ShoudOnlyGiveMatchinOnes(string? code, int counter)
    {
        // Arrange
        var context = TestHelper.NewApplicationDbContext();
        context.DiscountCodes.Add(new DiscountCode("12345678"));
        context.DiscountCodes.Add(new DiscountCode("87654321"));

        await context.SaveChangesAsync();

        IListDiscountCode repo = new DiscountCodeRepository(context, new Mock<ICustomLogger>().Object);

        var filter = new List<string>();
        if (code != null)
        {
            filter.Add(code);
        }

        // Act
        var result = await repo.ExecuteAsync(new ListDiscountCodeFilter(filter));

        // Assert
        result.Count().Should().Be(counter);
    }

}
