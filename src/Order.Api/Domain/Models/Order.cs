namespace Order.Api.Domain.Models;

public class Order
{
    public int Id { get; set; }

    public OrderStatus Status { get; set; }

    public string? Description { get; set; }

    public List<Line> Lines { get; set; }
}
