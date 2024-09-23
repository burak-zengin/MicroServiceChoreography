using Events;
using MassTransit;
using Stock.Api.Application.Stocks.Reserve;

namespace Stock.Api.Consumers;

public class OrderCreatedEventConsumer(
    Handler handler,
    IPublishEndpoint publishEndpoint,
    ISendEndpointProvider sendEndpointProvider) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var result = await handler.Handle(new Command(
            context
                .Message
                .Lines
                .Select(
                    l => new Application.Stocks.Line(
                        l.Barcode,
                        l.Quantity))
                .ToList()));

        if (result.Failed)
        {
            await publishEndpoint.Publish(
                new StockNotReservedEvent(
                    context.Message.OrderId,
                    result.Message));
        }
        else
        {
            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:StockReservedEvent"));
            await sendEndpoint.Send(new StockReservedEvent(
                context.Message.OrderId,
                context.Message.CreditCard,
                context.Message.Lines));
        }
    }
}
