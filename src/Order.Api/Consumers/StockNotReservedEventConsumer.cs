using Events;
using MassTransit;
using Order.Api.Application.Orders.Fail;

namespace Order.Api.Consumers;

public class StockNotReservedEventConsumer(Handler handler) : IConsumer<StockNotReservedEvent>
{
    public Task Consume(ConsumeContext<StockNotReservedEvent> context)
    {
        handler.Handle(new Command(context.Message.OrderId, context.Message.Message));

        return Task.CompletedTask;
    }
}
