using Events;
using MassTransit;
using Order.Api.Persistence;
using Results;

namespace Order.Api.Application.Orders.Create;

public class Handler(OrderDbContext context)
{
    public async Task<Result<int>> HandleAsync(Command command)
    {
        var order = new Domain.Models.Order
        {
            Status = Domain.Models.OrderStatus.Awaiting,
            Lines = command.Order.Lines.Select(l => new Domain.Models.Line
            {
                Barcode = l.Barcode,
                Quantity = l.Quantity
            }).ToList()
        };

        context.Add(order);
        context.SaveChanges();

        return new Result<int>
        {
            Data = order.Id
        };
    }
}
