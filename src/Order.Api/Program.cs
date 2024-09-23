using Events;
using MassTransit;
using Order.Api.Application.Orders.Create;
using Order.Api.Consumers;
using Order.Api.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<OrderDbContext>();
builder.Services.AddScoped<Handler>();
builder.Services.AddScoped<Order.Api.Application.Orders.Fail.Handler>();
builder.Services.AddScoped<Order.Api.Application.Orders.Success.Handler>();
builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<PaymentCompletedEventConsumer>();
    configure.AddConsumer<PaymentFailedEventConsumer>();
    configure.AddConsumer<StockNotReservedEventConsumer>();

    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host("rabbitmq", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });

        configurator.ReceiveEndpoint("Order.PaymentFailedEvent", e =>
        {
            e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
        });

        configurator.ReceiveEndpoint("Order.StockNotReservedEvent", e =>
        {
            e.ConfigureConsumer<StockNotReservedEventConsumer>(context);
        });

        configurator.ConfigureEndpoints(context);
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapPost("/", async (
    Handler handler,
    Command command,
    ISendEndpointProvider sendEndpointProvider) =>
{
    var result = await handler.HandleAsync(command);

    if (result.Failed == false)
    {
        var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:OrderCreatedEvent"));
        await sendEndpoint.Send(new OrderCreatedEvent(
            result.Data,
            command.Order.Lines.Select(l => new Events.Line(l.Barcode, l.Quantity)).ToList(),
            new Events.CreditCard(
                command.CreditCard.Number,
                command.CreditCard.Name,
                command.CreditCard.Surname,
                command.CreditCard.Month,
                command.CreditCard.Year)));
    }

    return result;
});
app.UseSwagger();
app.UseSwaggerUI();
app.Run();