using Events;
using MassTransit;
using Order.Api.Application.Orders.Success;

namespace Order.Api.Consumers;

public class PaymentCompletedEventConsumer(Handler handler) : IConsumer<PaymentCompletedEvent>
{
    public Task Consume(ConsumeContext<PaymentCompletedEvent> context)
    {
        handler.Handle(new Command(context.Message.OrderId));

        return Task.CompletedTask;
    }
}
