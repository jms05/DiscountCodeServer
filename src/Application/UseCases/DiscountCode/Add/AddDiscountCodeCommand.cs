using MediatR;

namespace JMS.Application.UseCases.DiscountCodes.Add;
public sealed record AddDiscountCodeCommand(int Count, int Lenght) : IRequest<AddDiscountCodeResponse>;
