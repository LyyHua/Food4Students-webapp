using System;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class OrderStatusUpdatedConsumer(IHubContext<NotificationHub> hubContext) : IConsumer<OrderStatusUpdated>
{
    public async Task Consume(ConsumeContext<OrderStatusUpdated> context)
    {
        Console.WriteLine("==> Order status updated message received: " + context.Message.Id);

        await hubContext.Clients.All.SendAsync("OrderStatusUpdated", context.Message);
    }
}
