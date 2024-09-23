namespace Events;

public record PaymentFailedEvent(int OrderId, List<Line> Lines, string Message);