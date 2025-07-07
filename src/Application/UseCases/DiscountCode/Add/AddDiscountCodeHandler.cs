using JMS.Application.Helpers;
using JMS.Application.UseCases.DiscountCodes.Add;
using JMS.Application.UseCases.RandomGenerator;
using JMS.Domain.Models.DiscountCodes;
using JMS.Domain.Models.DiscountCodes.Repository;
using MediatR;

namespace JMS.Application.UseCases.DiscountCodes.Get;

public sealed class AddDiscountCodeHandler : IRequestHandler<AddDiscountCodeCommand, AddDiscountCodeResponse?>
{
    private readonly IDiscountCodeGenerator _codeGenerator;
    private readonly IListDiscountCode _listDiscountCode;
    private readonly IAddDiscountCode _addDiscountCode;
    private readonly ICustomLogger _logger;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    public AddDiscountCodeHandler(IDiscountCodeGenerator codeGenerator, IListDiscountCode listDiscountCode, IAddDiscountCode addDiscountCode, ICustomLogger logger)
    {
        this._codeGenerator = codeGenerator;
        this._listDiscountCode = listDiscountCode;
        this._addDiscountCode = addDiscountCode;
        this._logger = logger;
    }
    public async Task<AddDiscountCodeResponse?> Handle(AddDiscountCodeCommand request, CancellationToken cancellationToken)
    {
        // This logic can also be executed with Queue to process one request at a time, or hope for the best and retry if we got a DB fail due a repeated code inseted between the check and save (it would be faster since we would not be waiting for one request to be completed)
        var allCodes = new HashSet<string>();
        var codesToGenerate = request.Count;
        _logger.Log($"Waiting For Semaphore for {request.Count}");
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            _logger.Log("Got Semaphore");

            while (allCodes.Count < request.Count)
            {
                var newCodes = _codeGenerator.GenerateCodes(codesToGenerate, request.Lenght)
                    .Except(allCodes);

                var existing = (await _listDiscountCode
                    .ExecuteAsync(new ListDiscountCodeFilter(newCodes), false, cancellationToken))
                    .Select(c => c.Code);

                var unique = newCodes.Except(existing);
                foreach (var code in unique)
                    allCodes.Add(code);

                codesToGenerate = request.Count - allCodes.Count;
            }

            var allDiscountCodes = allCodes.Select(c => new DiscountCode(c));
            await _addDiscountCode.ExecuteAsync(allDiscountCodes, cancellationToken);
            return new AddDiscountCodeResponse(allCodes);
        }
        finally
        {
            _semaphore.Release();
            _logger.Log("Semaphore Released");

        }
    }
}