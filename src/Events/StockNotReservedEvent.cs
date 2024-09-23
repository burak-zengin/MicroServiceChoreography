namespace Events;

public record StockNotReservedEvent(int OrderId, string Message);
