using MassTransit;
using NotificationService.Consumers;
using NotificationService.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<OrderPlacedConsumer>();
    x.AddConsumersFromNamespaceContaining<OrderFinishedConsumer>();
    x.AddConsumersFromNamespaceContaining<OrderStatusUpdatedConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("nt", false));

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", h =>
        {
            h.Username(builder.Configuration.GetValue("RabbitMQ:Username", "guest")!);
            h.Password(builder.Configuration.GetValue("RabbitMQ:Password", "guest")!);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<NotificationHub>("/notifications");

app.Run();
