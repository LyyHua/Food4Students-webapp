using System;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class OrderRejectedConsumer(IHubContext<NotificationHub> hubContext) : IConsumer<OrderRejected>
{
    public async Task Consume(ConsumeContext<OrderRejected> context)
    {
        Console.WriteLine("==> Order rejected message received");

        await hubContext.Clients.All.SendAsync("OrderRejected", context.Message);
    }
}