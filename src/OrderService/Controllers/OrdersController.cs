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
    public async Task<ActionResult<FoodOrder>> PlaceOrder(CreateOrderDto createOrderDto)
    {
        var restaurant = await DB.Find<Restaurant>().OneAsync(createOrderDto.RestaurantId);

        if (restaurant is null) return NotFound("Restaurant not found");

        if (restaurant.Status != "Approved" && restaurant.Status != "Open")
            return BadRequest("Cannot place order for unapproved restaurant.");

        var order = mapper.Map<FoodOrder>(createOrderDto);
        order.Name ??= User.Identity.Name;
        order.RestaurantId = createOrderDto.RestaurantId;
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
            .Match(x => x.Name == User.Identity.Name)
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
    // I will uncomment this if i EVER allow the user to delete their order
    /*
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(string id)
        {
            var order = await DB.Find<FoodOrder>()
                .MatchID(id)
                .ExecuteFirstAsync();

            if (order is null) return NotFound();

            if (order.Name != User.Identity.Name)
                return Unauthorized();

            if (order.OrderStatus != OrderStatus.Pending)
                return BadRequest("Cannot delete order that is not pending.");

            await DB.DeleteAsync<FoodOrder>(id);

            await publishEndpoint.Publish(mapper.Map<OrderDeleted>(order));

            return NoContent();
        }
    */
    [Authorize]
    [HttpPatch("{id}/accept")]
    public async Task<ActionResult> AcceptOrder(string id)
    {
        var order = await DB.Find<FoodOrder>()
            .MatchID(id)
            .ExecuteFirstAsync();

        var restaurant = await DB.Find<Restaurant>().OneAsync(order.RestaurantId);

        if (order is null) return NotFound();

        if (restaurant.Owner != User.Identity.Name)
            return Unauthorized();

        if (order.OrderStatus != OrderStatus.Pending)
            return BadRequest("Cannot update status of order that is not pending.");

        order.OrderStatus = OrderStatus.Accepted;

        await DB.SaveAsync(order);

        await publishEndpoint.Publish(mapper.Map<OrderAccepted>(order));

        return NoContent();
    }

    [Authorize]
    [HttpPatch("{id}/reject")]
    public async Task<ActionResult> RejectOrder(string id)
    {
        var order = await DB.Find<FoodOrder>()
            .MatchID(id)
            .ExecuteFirstAsync();

        var restaurant = await DB.Find<Restaurant>().OneAsync(order.RestaurantId);

        if (order is null) return NotFound();

        if (restaurant.Owner != User.Identity.Name)
            return Unauthorized();

        if (order.OrderStatus != OrderStatus.Pending)
            return BadRequest("Cannot update status of order that is not pending.");

        order.OrderStatus = OrderStatus.Rejected;

        await DB.SaveAsync(order);

        await publishEndpoint.Publish(mapper.Map<OrderRejected>(order));

        return NoContent();
    }

    [Authorize]
    [HttpPatch("{id}/deliver")]
    public async Task<ActionResult> DeliverOrder(string id)
    {
        var order = await DB.Find<FoodOrder>()
            .MatchID(id)
            .ExecuteFirstAsync();

        var restaurant = await DB.Find<Restaurant>().OneAsync(order.RestaurantId);

        if (order is null) return NotFound();

        if (restaurant.Owner != User.Identity.Name)
            return Unauthorized();

        if (order.OrderStatus != OrderStatus.Accepted)
            return BadRequest("Cannot update status of order that is not accepted.");

        order.OrderStatus = OrderStatus.Delivering;

        await DB.SaveAsync(order);

        await publishEndpoint.Publish(mapper.Map<OrderDelivering>(order));

        return NoContent();
    }

    [Authorize]
    [HttpPatch("{id}/complete")]
    public async Task<ActionResult> FinishOrder(string id)
    {
        var order = await DB.Find<FoodOrder>()
            .MatchID(id)
            .ExecuteFirstAsync();

        var restaurant = await DB.Find<Restaurant>().OneAsync(order.RestaurantId);

        if (order is null) return NotFound();

        if (restaurant.Owner != User.Identity.Name)
            return Unauthorized();
        
        // Some day in the future. Order can only be set to finished by the user
        // Or after 20 days count down from the day the order was placed it can be set to finished

        if (order.OrderStatus != OrderStatus.Delivering)
            return BadRequest("Cannot update status of order that is not delivering.");

        order.OrderStatus = OrderStatus.Finished;

        await DB.SaveAsync(order);

        await publishEndpoint.Publish(mapper.Map<OrderFinished>(order));

        return NoContent();
    }
}