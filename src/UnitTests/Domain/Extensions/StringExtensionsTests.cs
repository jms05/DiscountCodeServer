using FluentAssertions;
using Xunit;

namespace UnitTests.Domain.Extensions;
public sealed class StringExtensionsTests
{

    [Theory]
    [Trait("Domain", "Extensions")]
    [InlineData("HelloWord", "hello_word")]
    [InlineData("", "")]
    [InlineData(null, null)]
    public void When_Called_ShouldSnakeCaseString(string? originalString, string? expectedResult)
    {
        //Act //Assert
        originalString?.ToSnakeCase().Should().Be(expectedResult);
    }
}
