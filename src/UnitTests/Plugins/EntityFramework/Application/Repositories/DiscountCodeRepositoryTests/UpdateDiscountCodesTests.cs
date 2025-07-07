using EntityFrameworkRepository.Application.Repositories;
using FluentAssertions;
using JMS.Application.Helpers;
using JMS.Domain.Models.DiscountCodes;
using JMS.Domain.Models.DiscountCodes.Repository;
using Moq;
using UnitTests.Helpers;
using Xunit;

namespace UnitTests.Plugins.EntityFramework.Application.Repositories.DiscountCodeRepositoryTests;
public class UpdateDiscountCodesTests
{

    [Fact]
    [Trait("Plugin.EntityFramework", "DiscountCode")]
    public async Task When_DiscountCodeReceived_ShouldBeUpdated()
    {
        // Arrange
        using var context = TestHelper.NewApplicationDbContext();

        var code = new DiscountCode("12345678")
        {
            Used = false,
        };
        context.DiscountCodes.Add(code);
        await context.SaveChangesAsync();

        IUpdateDiscountCode repo = new DiscountCodeRepository(context, new Mock<ICustomLogger>().Object);

        code.Used = true;

        // Act
        await repo.ExecuteAsync(code, CancellationToken.None);
        var updatedCode = context.DiscountCodes.Single();

        // Assert
        updatedCode.Used.Should().BeTrue();
    }
}
