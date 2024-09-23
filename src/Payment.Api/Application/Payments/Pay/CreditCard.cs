namespace Payment.Api.Application.Payments.Pay;

public record CreditCard(string Number, string Name, string Surname, int Month, int Year);