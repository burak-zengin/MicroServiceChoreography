using Events;
using MassTransit;
using Order.Api.Application.Orders.Fail;

namespace Order.Api.Consumers;

public class PaymentFailedEventConsumer(Handler handler) : IConsumer<PaymentFailedEvent>
{
    public Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        handler.Handle(new Command(context.Message.OrderId, context.Message.Message));

        return Task.CompletedTask;
    }
}
