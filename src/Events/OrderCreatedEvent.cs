namespace Events;

public record OrderCreatedEvent(int OrderId, List<Line> Lines, CreditCard CreditCard);