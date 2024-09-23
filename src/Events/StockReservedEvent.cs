namespace Events;

public record StockReservedEvent(int OrderId, CreditCard CreditCard, List<Line> Lines);
