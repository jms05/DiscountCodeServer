using ConsoleInteface.Helpers;
using JMS.Application.UseCases.DiscountCodes.Add;
using JMS.Application.UseCases.DiscountCodes.Edit;
using JMS.Plugins.EntityFramework;
using MediatR;

Console.WriteLine("Welcome To the ConsoleAPP RightNow The only Purpose is to seed Data!!!");
Console.WriteLine("Press 1 to Seed new Data");
Console.WriteLine("Press 2 to Use Code");
Console.WriteLine("Any Other to Cancel");
var input = Console.ReadLine();
switch (input)
{
    case "1":
        await ConsoleHelpers.Seed();
        break;
    case "2":
        await ConsoleHelpers.Use();
        break;
    default:
        Console.WriteLine("Nothing to be Done");
        break;
}
Console.WriteLine("Thanks ");
return 0;

static class ConsoleHelpers
{
    private static readonly DatabaseOptions _clientDbOptions = new()
    {
        DatabaseName = "jms-local",
        Host = "localhost",
        Port = "5432",
        User = "postgres",
        Password = "postgres",
        LogSensitiveData = true
    };
    private static readonly IMediator _mediator = EnvHelper.Build(_clientDbOptions);


    public static async Task Use()
    { 
        Console.WriteLine("Please Enter the Code you want to use");
        var code = Console.ReadLine();
        var result = await _mediator.Send(new EditDiscountCodeCommand(code!, true));
        Console.WriteLine($"Result:{result?.SuccessUsed ?? false}");

    }

    public static async Task Seed()
    {
        var pendigTasks = new List<Task>();

        Console.WriteLine("Creating Codes");

        var rnd = new Random(Guid.NewGuid().GetHashCode());

        var totalIterations = rnd.Next(10, 20);

        for (int i = 0; i < totalIterations; i++)
        {
            var totalCodes = rnd.Next(1, 2000);
            var lenght = rnd.Next(7, 8);
            pendigTasks.Add(_mediator.Send(new AddDiscountCodeCommand(totalCodes, lenght)));
        }

        await Task.WhenAll(pendigTasks);

        Console.WriteLine("Seeded");

    }




}

