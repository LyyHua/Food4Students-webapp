using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using OrderService.DTOs;
using OrderService.Models;

namespace OrderService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(IMapper mapper, IPublishEndpoint publishEndpoint) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<FoodOrder>> PlaceOrder(string restaurantId, CreateOrderDto createOrderDto)
    {
        var restaurant = await DB.Find<Restaurant>().OneAsync(restaurantId);

        if (restaurant is null) return NotFound("Restaurant not found");

        if (restaurant.Status == "Pending")
            return BadRequest("Cannot place order for pending restaurant.");

        if (restaurant.Status != "Open")
            return BadRequest("Cannot place order for closed restaurant.");

        if (restaurant.Owner == User.Identity.Name)
            return BadRequest("Cannot place order for own restaurant.");

        var order = mapper.Map<FoodOrder>(createOrderDto);
        order.Name ??= User.Identity.Name;
        order.RestaurantId = restaurantId;
        order.Orderer = User.Identity.Name;
        order.OrderItems = [];

        // Process each order item in the DTO.
        foreach (var itemDto in createOrderDto.OrderItems)
        {
            // Look up the food item from the restaurant's embedded food categories.
            var foodItem = restaurant.FoodCategories?
                .SelectMany(fc => fc.FoodItems)
                .FirstOrDefault(fi => fi.Id == itemDto.FoodItemId);

            if (foodItem is null)
                return BadRequest($"FoodItem with ID {itemDto.FoodItemId} not found in restaurant.");

            // Start with the base price.
            int finalPrice = foodItem.BasePrice;

            // Process selected variations (if any)
            if (itemDto.SelectedVariations != null && itemDto.SelectedVariations.Count != 0)
            {
                foreach (var variationSelection in itemDto.SelectedVariations)
                {
                    // Find the corresponding variation in the food item.
                    var variation = foodItem.Variations?
                        .FirstOrDefault(v => v.Id == variationSelection.VariationId);
                    if (variation == null)
                        return BadRequest($"Variation with ID {variationSelection.VariationId} not found for FoodItem {foodItem.Name}.");

                    // For this variation, accumulate the price adjustment of each selected option.
                    if (variationSelection.VariationOptionIds != null && variationSelection.VariationOptionIds.Count != 0)
                    {
                        foreach (var optionId in variationSelection.VariationOptionIds)
                        {
                            var option = variation.VariationOptions?
                                .FirstOrDefault(o => o.Id == optionId);
                            if (option == null)
                                return BadRequest($"VariationOption with ID {optionId} not found in Variation {variation.Name}.");

                            finalPrice += option.PriceAdjustment;
                        }
                    }
                }
            }

            // Create and add the order item with the calculated price.
            var orderItem = new OrderItem
            {
                FoodName = foodItem.Name,
                FoodDescription = foodItem.Description,
                Price = finalPrice, // final calculated price
                Quantity = itemDto.Quantity,
                FoodItemPhotoUrl = foodItem.PhotoUrl,
                FoodItemId = foodItem.Id,
                // Optionally, create a string of all selected variation option names.
                Variations = itemDto.SelectedVariations != null
                    ? string.Join(", ",
                        itemDto.SelectedVariations.Select(vs =>
                        {
                            var variation = foodItem.Variations?
                                .FirstOrDefault(v => v.Id == vs.VariationId);

                            if (variation == null) return null;

                            // Collect all selected option names.
                            var optionNames = vs.VariationOptionIds.Select(optionId =>
                                variation.VariationOptions?
                                    .FirstOrDefault(o => o.Id == optionId)?.Name
                            ).Where(x => x != null);

                            // Return "VariationName: option1 option2 ..."
                            return $"{variation.Name}: {string.Join(" ", optionNames)}";
                        })
                        .Where(x => x != null)
                    )
                    : null
            };

            order.OrderItems.Add(orderItem);
        }

        // Calculate the total price.
        order.TotalPrice = order.OrderItems.Sum(oi => oi.Price * oi.Quantity);

        // Insert the order into the database.
        await DB.SaveAsync(order);

        await publishEndpoint.Publish(mapper.Map<OrderPlaced>(order));

        return CreatedAtAction(nameof(GetOrderById), new { id = order.ID }, order);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<FoodOrder>> GetOrderById(string id)
    {
        var order = await DB.Find<FoodOrder>()
            .MatchID(id)
            .ExecuteFirstAsync();

        if (order is null) return NotFound();

        return Ok(order);
    }

    [Authorize]
    [HttpGet("users")]
    public async Task<ActionResult<List<FoodOrder>>> GetUserOrders()
    {
        var orders = await DB.Find<FoodOrder>()
            .Match(x => x.Orderer == User.Identity.Name)
            .Sort(x => x.Descending(y => y.CreatedAt))
            .ExecuteAsync();

        return Ok(orders);
    }

    [Authorize]
    [HttpGet("restaurants/{restaurantId}")]
    public async Task<ActionResult<List<FoodOrder>>> GetRestaurantOrders(string restaurantId)
    {
        var orders = await DB.Find<FoodOrder>()
            .Match(x => x.RestaurantId == restaurantId)
            .Sort(x => x.Descending(y => y.CreatedAt))
            .ExecuteAsync();

        return Ok(orders);
    }

    [Authorize]
    [HttpPatch]
    public async Task<ActionResult> ChangeOrderStatus(string id, string newStatus)
    {
        var order = await DB.Find<FoodOrder>()
            .MatchID(id)
            .ExecuteFirstAsync();
        if (order is null) return NotFound();

        var restaurant = await DB.Find<Restaurant>().OneAsync(order.RestaurantId);
        if (restaurant is null) return NotFound();

        if (restaurant.Owner != User.Identity.Name && order.Orderer != User.Identity.Name)
            return Unauthorized();

        if (!Enum.TryParse<OrderStatus>(newStatus, true, out var desiredStatus))
            return BadRequest("Invalid order status.");

        bool isOwner = restaurant.Owner == User.Identity.Name;
        bool isOrderer = order.Orderer == User.Identity.Name;

        if (isOwner)
        {
            switch (desiredStatus)
            {
                case OrderStatus.Accepted:
                    if (order.OrderStatus != OrderStatus.Pending)
                        return BadRequest("You can only accept a pending order.");
                    order.OrderStatus = OrderStatus.Accepted;
                    break;
                case OrderStatus.Delivering:
                    if (order.OrderStatus != OrderStatus.Accepted)
                        return BadRequest("You can only deliver an accepted order.");
                    order.OrderStatus = OrderStatus.Delivering;
                    break;
                default:
                    return BadRequest("This transition is not supported for the owner.");
            }
        }
        else if (isOrderer)
        {
            switch (desiredStatus)
            {
                case OrderStatus.Finished:
                    if (order.OrderStatus != OrderStatus.Delivering)
                        return BadRequest("You can only finish a delivering order.");
                    order.OrderStatus = OrderStatus.Finished;
                    break;
                case OrderStatus.Rejected:
                    if (order.OrderStatus != OrderStatus.Pending)
                        return BadRequest("You can only reject a pending order.");
                    order.OrderStatus = OrderStatus.Rejected;
                    break;
                default:
                    return BadRequest("This transition is not supported for the orderer.");
            }
        }
        else
            return Unauthorized();

        await DB.SaveAsync(order);
        await publishEndpoint.Publish(new OrderStatusUpdated
        {
            Id = order.ID,
            Status = order.OrderStatus.ToString()

        });

        return NoContent();
    }
}