using MassTransit;
using Stock.Api.Application.Stocks.Reserve;
using Stock.Api.Consumers;
using Stock.Api.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StockDbContext>();
builder.Services.AddScoped<Handler>();
builder.Services.AddScoped<Stock.Api.Application.Stocks.Increase.Handler>();
builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<OrderCreatedEventConsumer>();
    configure.AddConsumer<PaymentFailedEventConsumer>();

    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host("rabbitmq", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });

        configurator.ReceiveEndpoint("Stock.PaymentFailedEvent", e =>
        {
            e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
        });

        configurator.ConfigureEndpoints(context);
    });
});

var app = builder.Build();
app.UseHttpsRedirection();
app.Run();