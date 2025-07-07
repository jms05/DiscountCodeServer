using FluentAssertions;
using JMS.Application.Helpers;
using JMS.Application.UseCases.DiscountCodes.List;
using JMS.Domain.Models.DiscountCodes;
using JMS.Domain.Models.DiscountCodes.Repository;
using Moq;
using UnitTests.Helpers;
using Xunit;

namespace UnitTests.Application.UseCases.DiscountCodes;
public class ListDiscountCodeHandlerTests
{
    [Fact]
    [Trait("Application", "UseCases")]
    public async Task When_NoDiscountCodesOnRepo_ShouldReturnNull()
    {
        // Arrange
        var listDiscountCodeMock = new Mock<IListDiscountCode>();

        listDiscountCodeMock.Setup(v => v.ExecuteAsync(It.IsAny<ListDiscountCodeFilter>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(Enumerable.Empty<DiscountCode>()));

        var handler = new ListDiscountCodeHandler(listDiscountCodeMock.Object, new Mock<ICustomLogger>().Object);

        // Act
        var result = await handler.Handle(new ListDiscountCodeQuery(), CancellationToken.None);

        // Assert
        result.Should().BeNullOrEmpty();
    }

    [Fact]
    [Trait("Application", "UseCases")]
    public async Task When_SomethingOnRepo_ShouldReturnAllFromRepo()
    {
        // Arrange
        IEnumerable<DiscountCode> mockedResult = new List<DiscountCode>()
        {
            new DiscountCode("12345678"),
            new DiscountCode("12345679")
        };

        var listDiscountCodeMock = new Mock<IListDiscountCode>();

        listDiscountCodeMock.Setup(v => v.ExecuteAsync(It.IsAny<ListDiscountCodeFilter>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(mockedResult));

        var handler = new ListDiscountCodeHandler(listDiscountCodeMock.Object, new Mock<ICustomLogger>().Object);

        // Act
        var result = await handler.Handle(new ListDiscountCodeQuery(), CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.Code.Equals("12345678"));
        result.Should().Contain(t => t.Code.Equals("12345679"));
    }
}
