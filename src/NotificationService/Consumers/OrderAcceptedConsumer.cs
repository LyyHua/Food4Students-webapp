using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class OrderAcceptedConsumer(IHubContext<NotificationHub> hubContext) : IConsumer<OrderAccepted>
{
    public async Task Consume(ConsumeContext<OrderAccepted> context)
    {
        Console.WriteLine("==> Order accepted message received");

        await hubContext.Clients.All.SendAsync("OrderAccepted", context.Message);
    }
}
