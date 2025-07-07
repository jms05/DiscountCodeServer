using FluentAssertions;
using JMS.Application.UseCases.DiscountCodes.Edit;
using JMS.Domain.Models.DiscountCodes;
using JMS.Domain.Models.DiscountCodes.Repository;
using Moq;
using Xunit;

namespace UnitTests.Application.UseCases.DiscountCodes;
public class EditDiscountCodeHandlerTests
{
    [Fact]
    [Trait("Application", "UseCases")]
    public async Task When_UpdateingExistingUnusedDiscountCode_ShouldCallRepositoryGet_Update_ReturningUpdatedDiscountCodesd()
    {
        // Arrange
        var getDiscountCodeMock = new Mock<IGetDiscountCode>();
        var updateDiscountCodeMock = new Mock<IUpdateDiscountCode>();

        getDiscountCodeMock.Setup(v => v.ExecuteAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
          .Returns(Task.FromResult(new DiscountCode("12345678"))!);

        updateDiscountCodeMock.Setup(v => v.ExecuteAsync(It.IsAny<DiscountCode>(), It.IsAny<CancellationToken>()))
          .Returns(Task.FromResult(true));

        var handler = new EditDiscountCodeHandler(getDiscountCodeMock.Object, updateDiscountCodeMock.Object);

        // Act
        var result = await handler.Handle(new EditDiscountCodeCommand("12345678", true), CancellationToken.None);

        // Assert
        getDiscountCodeMock.Verify(v => v.ExecuteAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        updateDiscountCodeMock.Verify(v => v.ExecuteAsync(It.IsAny<DiscountCode>(), It.IsAny<CancellationToken>()), Times.Once);
        result?.SuccessUsed.Should().BeTrue();
    }

    [Fact]
    [Trait("Application", "UseCases")]
    public async Task When_EditingNotExistingDiscountCode_ShouldCallOnlyCallGetRepo()
    {

        // Arrange
        var getDiscountCodeMock = new Mock<IGetDiscountCode>();
        var updateDiscountCodeMock = new Mock<IUpdateDiscountCode>();

        DiscountCode? emptyDiscount = null;

        getDiscountCodeMock.Setup(v => v.ExecuteAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
          .Returns(Task.FromResult(emptyDiscount));

        var handler = new EditDiscountCodeHandler(getDiscountCodeMock.Object, updateDiscountCodeMock.Object);

        // Act
        var result = await handler.Handle(new EditDiscountCodeCommand("12345678", true), CancellationToken.None);

        // Assert
        getDiscountCodeMock.Verify(v => v.ExecuteAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        updateDiscountCodeMock.Verify(v => v.ExecuteAsync(It.IsAny<DiscountCode>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Should().BeNull();
    }


    [Fact]
    [Trait("Application", "UseCases")]
    public async Task When_EditingUsedDiscountCode_ShouldCallOnlyCallGetRepo()
    {

        // Arrange
        var getDiscountCodeMock = new Mock<IGetDiscountCode>();
        var updateDiscountCodeMock = new Mock<IUpdateDiscountCode>();


        getDiscountCodeMock.Setup(v => v.ExecuteAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
          .Returns(Task.FromResult(new DiscountCode("12345678") { Used= true})!);

        var handler = new EditDiscountCodeHandler(getDiscountCodeMock.Object, updateDiscountCodeMock.Object);

        // Act
        var result = await handler.Handle(new EditDiscountCodeCommand("12345678", true), CancellationToken.None);

        // Assert
        getDiscountCodeMock.Verify(v => v.ExecuteAsync(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        updateDiscountCodeMock.Verify(v => v.ExecuteAsync(It.IsAny<DiscountCode>(), It.IsAny<CancellationToken>()), Times.Never);
        result?.SuccessUsed.Should().BeFalse();
    }
}
