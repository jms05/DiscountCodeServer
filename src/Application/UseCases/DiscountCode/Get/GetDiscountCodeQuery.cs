using MediatR;

namespace JMS.Application.UseCases.DiscountCodes.Get;
public sealed record GetDiscountCodeQuery(string DiscountCode) : IRequest<GetDiscountCodeResponse>;
