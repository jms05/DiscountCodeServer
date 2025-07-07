using JMS.Application.UseCases.DiscountCodes.Add;
using JMS.Application.UseCases.DiscountCodes.Edit;
using JMS.Application.UseCases.DiscountCodes.Get;
using JMS.Application.UseCases.DiscountCodes.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[ApiController]
[Route("[controller]")]
public sealed class DiscountCodeController : ControllerBase
{
    private readonly IMediator _mediator;

    public DiscountCodeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ListDiscountCodeResponse>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> List(
        [FromQuery] IEnumerable<string>? names,
        [FromQuery] bool includeUsed = false)
    {
        var result = await _mediator.Send(new ListDiscountCodeQuery(Names: names, IncludeUsed: includeUsed));

        return result.Any() ? Ok(result) : NoContent();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GetDiscountCodeResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(
        [FromBody] AddCodesCommand addCodesCommand
        )
    {
        var appRespose = await _mediator.Send(new AddDiscountCodeCommand(addCodesCommand.Count, addCodesCommand.Lenght));
        return Created($"/discountcode/", appRespose);
    }


    [HttpPut("{code}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put(
        [FromRoute] string code)
    {
        var result = await _mediator.Send(new EditDiscountCodeCommand(code,true));
        return result is null ? NotFound() : Ok(result.SuccessUsed ? 0 : 1);
    }

}
public sealed record AddCodesCommand(int Count, int Lenght)
{
   
}
