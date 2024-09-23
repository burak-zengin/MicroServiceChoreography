using MassTransit;
using Results;

namespace Payment.Api.Application.Payments.Pay;

public class Handler
{
    public Result Handle(Command command)
    {
        if (command.CreditCard.Number.StartsWith("1234"))
        {
            return new Result
            {
                Failed = true,
                Message = "Insufficient funds."
            };
        }

        return new Result();
    }
}
