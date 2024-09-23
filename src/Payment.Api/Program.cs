using MassTransit;
using Payment.Api.Application.Payments.Pay;
using Payment.Api.Consumers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<Handler>();
builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<StockReservedEventConsumer>();

    configure.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host("rabbitmq", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });

        configurator.ConfigureEndpoints(context);
    });
});

var app = builder.Build();
app.UseHttpsRedirection();
app.Run();