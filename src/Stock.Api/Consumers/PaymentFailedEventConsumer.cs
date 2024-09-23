using Events;
using MassTransit;
using Stock.Api.Application.Stocks.Increase;

namespace Stock.Api.Consumers;

public class PaymentFailedEventConsumer(Handler handler) : IConsumer<PaymentFailedEvent>
{
    public Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        handler.Handle(new Command(
            context
            .Message
            .Lines
            .Select(l => new Application.Stocks.Line(
                l.Barcode, 
                l.Quantity))
            .ToList()));

        return Task.CompletedTask;
    }
}
