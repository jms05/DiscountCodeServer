using AutoFixture;
using JMS.Domain.Models.DiscountCodes;
using JMS.Plugins.EntityFramework.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace UnitTests.Helpers;
internal static class TestHelper
{
    public static ApplicationDbContext NewApplicationDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: "TestDb" + Guid.NewGuid().ToString().Replace("-", string.Empty))
           .Options;

        return new ApplicationDbContext(options);
    }

    public static Mock<IMediator> GetControllerMediator<TRequest, TResponse>(int responseCount, Fixture? customFixture = null)
        where TRequest : IRequest<IEnumerable<TResponse>>
    {
        var mediatorMock = new Mock<IMediator>();

        var items = (customFixture ?? TestHelper.NewFixture()).CreateMany<TResponse>(responseCount).ToList();

        mediatorMock
            .Setup(s => s.Send(It.IsAny<TRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(items);

        return mediatorMock;
    }

    public static Mock<IMediator> GetControllerMediator<TRequest, TResponse>(out TResponse? expectedResponse, bool found = true, Fixture? customFixture = null)
      where TRequest : IRequest<TResponse?>
    {
        var mediatorMock = new Mock<IMediator>();

        expectedResponse = !found || typeof(TResponse) == typeof(Unit)
                ? default
                : (customFixture ?? TestHelper.NewFixture()).Create<TResponse>();

        mediatorMock
            .Setup(s => s.Send(It.IsAny<TRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        return mediatorMock;
    }

    public static Fixture NewFixture()
    {
        var fixture = new Fixture();

        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        fixture.Customize<DiscountCode>(ct => ct.FromFactory<DateTime>((a) =>
        {
            return new DiscountCode(fixture.Create<string>())
            {
                Used =  fixture.Create<bool>()
            };
        })
        .OmitAutoProperties());

        return fixture;
    }
}
