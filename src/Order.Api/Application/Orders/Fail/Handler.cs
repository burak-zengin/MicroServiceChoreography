using Order.Api.Persistence;

namespace Order.Api.Application.Orders.Fail;

public class Handler(OrderDbContext context)
{
    public void Handle(Command command)
    {
        var order = context.Orders.FirstOrDefault(o => o.Id == command.Id);
        order.Status = Domain.Models.OrderStatus.Fail;
        order.Description = command.Message;

        context.SaveChanges();
    }
}