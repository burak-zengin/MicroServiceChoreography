namespace Order.Api.Application.Orders.Create;

public record Command(Order Order, CreditCard CreditCard);