using Grpc.Net.Client;
using JMS.GrpcApi;



using var channel = GrpcChannel.ForAddress("http://localhost:5204");
var client = new DiscountCodeService.DiscountCodeServiceClient(channel);

Console.WriteLine("gRPC client- Discount Code Service\n");

while (true)
{
    Console.WriteLine("Options:");
    Console.WriteLine("1. Add codes");
    Console.WriteLine("2. List codes");
    Console.WriteLine("3. Use codes");
    Console.WriteLine("0. Exit");
    Console.Write("Option: ");

    var option = Console.ReadLine();

    try
    {
        switch (option)
        {
            case "1":
                await AddDiscountCodes(client);
                break;
            case "2":
                await ListDiscountCodes(client);
                break;
            case "3":
                await UpdateDiscountCode(client);
                break;
            case "0":
                Console.WriteLine("Exiting...");
                return;
            default:
                Console.WriteLine("Invalid Input!");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

async Task AddDiscountCodes(DiscountCodeService.DiscountCodeServiceClient client)
{
    Console.Write("Codes to Generate: ");
    if (!int.TryParse(Console.ReadLine(), out int count))
    {
        Console.WriteLine("Invalid Input aborting!");
        return;
    }

    Console.Write("Code Lenght? ");
    if (!int.TryParse(Console.ReadLine(), out int length))
    {
        Console.WriteLine("Invalid Input aborting!");
        return;
    }


    Console.Write("How many Request in Paralel: ");
    var input = Console.ReadLine();
    var number = int.TryParse(input, out var n) ? n : 1;


    var tasks = Enumerable.Range(1, number).Select(async i =>
    {
        var request = new AddDiscountCodesRequest
        {
            Count = count,
            Length = length
        };

        try
        {
            var response = await client.AddDiscountCodesAsync(request);
            Console.WriteLine($"Success: {response.Success}");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Request {i}] Exception: {ex.Message}");
        }
    }).ToArray();

    await Task.WhenAll(tasks);
}

async Task ListDiscountCodes(DiscountCodeService.DiscountCodeServiceClient client)
{
    Console.Write("Filtrs (comma separated, Enter for Everything): ");
    var filtersInput = Console.ReadLine();

    var request = new ListDiscountCodesRequest();

    if (!string.IsNullOrWhiteSpace(filtersInput))
    {
        var filters = filtersInput.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                 .Select(f => f.Trim());
        request.Filter.AddRange(filters);
    }

    var response = await client.ListDiscountCodesAsync(request);

    Console.WriteLine($"\nFound {response.Codes.Count} codes:");

    foreach (var code in response.Codes)
    {
        Console.WriteLine($"{code.Code}\tUsed: {code.Used}");
    }
}


async Task UpdateDiscountCode(DiscountCodeService.DiscountCodeServiceClient client)
{
    Console.Write("Insert Code to use: ");
    var code = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(code))
    {
        Console.WriteLine("Empty code, aborting");
        return;
    }

    Console.Write("How many Request in Paralel: ");
    var input = Console.ReadLine();
    var number = int.TryParse(input, out var n) ? n : 1;


    var tasks = Enumerable.Range(1, number).Select(async i =>
    {
        var request = new UpdateDiscountCodeRequest { Code = code };

        try
        {
            var response = await client.UpdateDiscountCodeAsync(request);
            var resultByte = response.Code.ToByteArray()[0];

            string message = resultByte switch
            {
                0 => "Code updated successfully!",
                1 => "Code not found!",
                2 => "Failed to update code!",
                _ => $"Unknown response: {resultByte}"
            };

            Console.WriteLine($"[Request {i}] {message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Request {i}] Exception: {ex.Message}");
        }
    }).ToArray();

    await Task.WhenAll(tasks);
}
