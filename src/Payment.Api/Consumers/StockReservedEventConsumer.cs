using Events;
using MassTransit;
using Payment.Api.Application.Payments.Pay;

namespace Payment.Api.Consumers;

public class StockReservedEventConsumer(
    Handler handler,
    IPublishEndpoint publishEndpoint,
    ISendEndpointProvider sendEndpointProvider) : IConsumer<StockReservedEvent>
{
    public async Task Consume(ConsumeContext<StockReservedEvent> context)
    {
        var result = handler.Handle(new Command(
            new(
                context.Message.CreditCard.Number,
                context.Message.CreditCard.Name,
                context.Message.CreditCard.Surname,
                context.Message.CreditCard.Month,
                context.Message.CreditCard.Year)));

        if (result.Failed)
        {
            await publishEndpoint.Publish(new PaymentFailedEvent(
                context.Message.OrderId,
                context.Message.Lines,
                result.Message));
        }
        else
        {
            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:PaymentCompletedEvent"));
            await sendEndpoint.Send(new PaymentCompletedEvent(context.Message.OrderId));
        }
    }
}
