using Google.Protobuf;
using Grpc.Core;
using JMS.Application.UseCases.DiscountCodes.Add;
using JMS.Application.UseCases.DiscountCodes.Edit;
using JMS.Application.UseCases.DiscountCodes.List;
using MediatR;

namespace JMS.GrpcApi.Services;
public class DiscountCodeServiceImpl : DiscountCodeService.DiscountCodeServiceBase
{
    private readonly IMediator _mediator;

    public DiscountCodeServiceImpl(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<AddDiscountCodesResponse> AddDiscountCodes(AddDiscountCodesRequest request, ServerCallContext context)
    {
        var appResponse = await _mediator.Send(new AddDiscountCodeCommand(request.Count, request.Length));

        var response = new AddDiscountCodesResponse { Success = request.Count == appResponse.GeneratedCodes.Count() };

        return response;
    }

    public override async Task<ListDiscountCodesResponse> ListDiscountCodes(ListDiscountCodesRequest request, ServerCallContext context)
    {
        var appResponse = await _mediator.Send(new ListDiscountCodeQuery(request.Filter, false));

        var response = new ListDiscountCodesResponse { Codes = { appResponse.Select(c => new DiscountCode() { Code = c.Code, Used = c.Used }) } };

        return response;
    }

    
    public override async Task<UpdateDiscountCodeResponse> UpdateDiscountCode(UpdateDiscountCodeRequest request, ServerCallContext context)
    {
        
        var appResponse = await _mediator.Send(new EditDiscountCodeCommand(request.Code, true));

        byte responseByte = 0;
        if (!appResponse?.SuccessUsed ?? false)
            responseByte = 2;

        if (appResponse== null)
             responseByte = 1;
        
        return new UpdateDiscountCodeResponse { Code = ByteString.CopyFrom(responseByte) };
    }
}
