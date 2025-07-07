using EntityFrameworkRepository.Application.Repositories;
using FluentAssertions;
using JMS.Application.Helpers;
using JMS.Domain.Models.DiscountCodes;
using JMS.Domain.Models.DiscountCodes.Repository;
using Moq;
using UnitTests.Helpers;
using Xunit;

namespace UnitTests.Plugins.EntityFramework.Application.Repositories.DiscountCodeRepositoryTests;
public class AddDiscountCodesTests
{

    [Fact]
    [Trait("Plugin.EntityFramework", "DiscountCode")]
    public void When_DiscountCodedReceived_ShouldAddDiscountCode()
    {
        // Arrange
        using var context = TestHelper.NewApplicationDbContext();

        var discountCode = new DiscountCode("12345678");

        IAddDiscountCode repo = new DiscountCodeRepository(context, new Mock<ICustomLogger>().Object);

        // Pre assert
        context.DiscountCodes.Should().HaveCount(0);

        // Act
        repo.ExecuteAsync(discountCode, CancellationToken.None);

        // Assert
        context.DiscountCodes.Should().HaveCount(1);
        context.DiscountCodes.First().Should().Be(discountCode);
    }
}
