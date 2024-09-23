using Order.Api.Persistence;
using Results;

namespace Order.Api.Application.Orders.Success;

public class Handler(OrderDbContext context)
{
    public void Handle(Command command)
    {
        var order = context.Orders.FirstOrDefault(o => o.Id == command.Id);
        order.Status = Domain.Models.OrderStatus.Success;

        context.SaveChanges();
    }
}