using System.Text.Json;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantService.Data;
using RestaurantService.DTOs;
using RestaurantService.Entities;

namespace RestaurantService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantsController(RestaurantDbContext context,
    IMapper mapper, IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants(string date)
    {
        var query = context.Restaurants.AsQueryable();

        if (!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.CreatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        return await query.ProjectTo<RestaurantDto>(mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RestaurantDto>> GetRestaurantById(Guid id)
    {
        var restaurant = await context.Restaurants
            .FirstOrDefaultAsync(x => x.Id == id);

        if (restaurant == null) return NotFound();

        return mapper.Map<RestaurantDto>(restaurant);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Restaurant>> CreateRestaurant(CreateRestaurantDto restaurantDto)
    {
        var restaurant = mapper.Map<Restaurant>(restaurantDto);

        restaurant.Owner = User.Identity.Name;

        context.Restaurants.Add(restaurant);

        var newRestaurant = mapper.Map<RestaurantDto>(restaurant);

        await publishEndpoint.Publish(mapper.Map<RestaurantCreated>(newRestaurant));

        var result = await context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not save changes to the database");

        return CreatedAtAction(nameof(GetRestaurantById), new { id = restaurant.Id }, restaurant);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateRestaurant(Guid id, CreateRestaurantDto restaurantDto)
    {
        var restaurant = await context.Restaurants.FindAsync(id);

        if (restaurant == null) return NotFound();

        if (restaurant.Owner != User.Identity.Name) return Forbid();

        restaurant.Name = restaurantDto.Name ?? restaurant.Name;
        restaurant.Description = restaurantDto.Description ?? restaurant.Description;
        restaurant.Address = restaurantDto.Address ?? restaurant.Address;
        restaurant.PhoneNumber = restaurantDto.PhoneNumber ?? restaurant.PhoneNumber;
        restaurant.LogoUrl = restaurantDto.LogoUrl ?? restaurant.LogoUrl;
        restaurant.BannerUrl = restaurantDto.BannerUrl ?? restaurant.BannerUrl;

        await publishEndpoint.Publish(mapper.Map<RestaurantUpdated>(restaurant));

        var result = await context.SaveChangesAsync() > 0;

        if (result) return Ok();

        return BadRequest("Could not save changes to the database");
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRestaurant(Guid id)
    {
        var restaurant = await context.Restaurants.FindAsync(id);

        if (restaurant == null) return NotFound();

        if (restaurant.Owner != User.Identity.Name) return Forbid();

        context.Restaurants.Remove(restaurant);

        await publishEndpoint.Publish<RestaurantDeleted>(new { Id = id.ToString() });

        var result = await context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not update DB");

        return Ok();
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<ActionResult> ApproveRestaurant(Guid id)
    {
        var restaurant = await context.Restaurants.FindAsync(id);

        if (restaurant == null) return NotFound();

        if (User.Identity.Name != "LyHua") return Forbid();

        restaurant.Status = Status.Approved;

        var updatedRestaurantStatus = new StatusUpdated
        {
            Id = restaurant.Id.ToString(),
            Status = restaurant.Status.ToString()
        };

        await publishEndpoint.Publish(updatedRestaurantStatus);

        var result = await context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not update DB");

        return Ok();
    }

    [HttpGet("{id}/menu")]
    public async Task<ActionResult<List<FoodCategoryDto>>> GetMenu(Guid id)
    {
        var restaurant = await context.Restaurants
            .Include(r => r.FoodCategories)
                .ThenInclude(fc => fc.FoodItems)
                    .ThenInclude(fi => fi.Variations)
                        .ThenInclude(v => v.VariationOptions)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (restaurant == null) return NotFound();

        return mapper.Map<List<FoodCategoryDto>>(restaurant.FoodCategories);
    }

    [Authorize]
    [HttpPut("{id}/menu")]
    public async Task<ActionResult<RestaurantDto>> UpdateMenu(Guid id, List<FoodCategoryDto> dtoCategories)
    {
        // Fetch the restaurant and its categories/items/variations
        var restaurant = await context.Restaurants
            .Include(r => r.FoodCategories)
                .ThenInclude(fc => fc.FoodItems)
                    .ThenInclude(fi => fi.Variations)
                        .ThenInclude(v => v.VariationOptions)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (restaurant == null)
            return NotFound();
        if (restaurant.Owner != User.Identity.Name)
            return Forbid();

        // Remove FoodCategories missing in the DTO
        var dtoCategoryIds = dtoCategories
            .Where(c => c.Id != Guid.Empty)
            .Select(c => c.Id)
            .ToList();
        var categoriesToRemove = restaurant.FoodCategories
            .Where(c => !dtoCategoryIds.Contains(c.Id))
            .ToList();
        foreach (var cat in categoriesToRemove)
            restaurant.FoodCategories.Remove(cat);

        // Process each Category
        foreach (var dtoCat in dtoCategories)
        {
            FoodCategory existingCategory;
            if (dtoCat.Id != Guid.Empty)
            {
                // Try to find matching category
                existingCategory = restaurant.FoodCategories
                    .FirstOrDefault(c => c.Id == dtoCat.Id);

                if (existingCategory != null)
                {
                    // Update existing category
                    existingCategory.Name = dtoCat.Name;

                    // Remove FoodItems not in DTO
                    var dtoItemIds = dtoCat.FoodItems
                        .Where(fi => fi.Id != Guid.Empty)
                        .Select(fi => fi.Id)
                        .ToList();
                    var itemsToRemove = existingCategory.FoodItems
                        .Where(fi => !dtoItemIds.Contains(fi.Id))
                        .ToList();
                    foreach (var oldItem in itemsToRemove)
                    {
                        existingCategory.FoodItems.Remove(oldItem);
                    }

                    // Process each item in that category
                    foreach (var itemDto in dtoCat.FoodItems)
                    {
                        FoodItem existingItem;
                        if (itemDto.Id != Guid.Empty)
                        {
                            // Update or add by item ID
                            existingItem = existingCategory.FoodItems
                                .FirstOrDefault(fi => fi.Id == itemDto.Id);

                            if (existingItem != null)
                            {
                                // Update existing item
                                existingItem.Name = itemDto.Name;
                                existingItem.Description = itemDto.Description;
                                existingItem.PhotoUrl = itemDto.PhotoUrl;
                                existingItem.BasePrice = itemDto.BasePrice;

                                // Remove variations not in DTO
                                var dtoVariationIds = itemDto.Variations
                                    .Where(v => v.Id != Guid.Empty)
                                    .Select(v => v.Id)
                                    .ToList();
                                var variationsToRemove = existingItem.Variations
                                    .Where(v => !dtoVariationIds.Contains(v.Id))
                                    .ToList();
                                foreach (var oldVar in variationsToRemove)
                                {
                                    existingItem.Variations.Remove(oldVar);
                                }

                                // Add or update Variation
                                foreach (var varDto in itemDto.Variations)
                                {
                                    Variation existingVar;

                                    // If Variation ID is set, try to find it
                                    if (varDto.Id != Guid.Empty)
                                    {
                                        existingVar = existingItem.Variations
                                            .FirstOrDefault(v => v.Id == varDto.Id);

                                        if (existingVar != null)
                                        {
                                            // Update Variation
                                            existingVar.Name = varDto.Name;
                                            existingVar.MinSelect = varDto.MinSelect;
                                            existingVar.MaxSelect = varDto.MaxSelect;
                                        }
                                        else
                                        {
                                            // Variation ID given but not found: treat as new
                                            existingVar = mapper.Map<Variation>(varDto);
                                            existingVar.Id = varDto.Id; // keep the DTO’s ID if desired
                                            existingItem.Variations.Add(existingVar);
                                        }
                                    }
                                    else
                                    {
                                        // Brand-new Variation
                                        existingVar = mapper.Map<Variation>(varDto);
                                        existingItem.Variations.Add(existingVar);
                                    }

                                    // Ensure Variation is tracked and has a valid Guid
                                    if (context.Entry(existingVar).State == EntityState.Detached
                                        && existingVar.Id == Guid.Empty)
                                    {
                                        existingVar.Id = Guid.NewGuid();
                                        context.Variations.Add(existingVar);
                                    }

                                    // VariationOptions: remove those not listed
                                    var dtoOptIds = varDto.VariationOptions
                                        .Where(o => o.Id != Guid.Empty)
                                        .Select(o => o.Id)
                                        .ToList();
                                    var oldOpts = existingVar.VariationOptions
                                        .Where(o => !dtoOptIds.Contains(o.Id))
                                        .ToList();
                                    foreach (var deadOpt in oldOpts)
                                    {
                                        existingVar.VariationOptions.Remove(deadOpt);
                                    }

                                    // Add or update VariationOptions
                                    foreach (var optDto in varDto.VariationOptions)
                                    {
                                        if (optDto.Id != Guid.Empty)
                                        {
                                            var existingOpt = existingVar.VariationOptions
                                                .FirstOrDefault(o => o.Id == optDto.Id);
                                            if (existingOpt != null)
                                            {
                                                // Update existing VariationOption
                                                existingOpt.Name = optDto.Name;
                                                existingOpt.PriceAdjustment = optDto.PriceAdjustment;
                                            }
                                            else
                                            {
                                                // VariationOption ID specified but not found; add new
                                                var newOpt = mapper.Map<VariationOption>(optDto);

                                                // Keep the original ID if you’d like
                                                newOpt.Id = optDto.Id;

                                                // If Variation is still missing a valid ID, fix it
                                                if (existingVar.Id == Guid.Empty)
                                                {
                                                    existingVar.Id = Guid.NewGuid();
                                                    context.Variations.Add(existingVar);
                                                }
                                                newOpt.VariationId = existingVar.Id;
                                                existingVar.VariationOptions.Add(newOpt);
                                            }
                                        }
                                        else
                                        {
                                            // Brand-new VariationOption
                                            var newOpt = mapper.Map<VariationOption>(optDto);
                                            if (existingVar.Id == Guid.Empty)
                                            {
                                                existingVar.Id = Guid.NewGuid();
                                                context.Variations.Add(existingVar);
                                            }
                                            newOpt.VariationId = existingVar.Id;
                                            existingVar.VariationOptions.Add(newOpt);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // ID set, but no item found in DB: treat as new
                                var newFoodItem = mapper.Map<FoodItem>(itemDto);
                                existingCategory.FoodItems.Add(newFoodItem);
                            }
                        }
                        else
                        {
                            // Brand-new FoodItem
                            var newFoodItem = mapper.Map<FoodItem>(itemDto);
                            existingCategory.FoodItems.Add(newFoodItem);
                        }
                    }
                }
                else
                {
                    // Category ID was specified but not found: treat as new
                    var newCategory = mapper.Map<FoodCategory>(dtoCat);
                    newCategory.Id = dtoCat.Id; // keep the incoming ID if you want
                    restaurant.FoodCategories.Add(newCategory);
                }
            }
            else
            {
                // Brand-new Category
                var newCategory = mapper.Map<FoodCategory>(dtoCat);
                restaurant.FoodCategories.Add(newCategory);
            }
        }

        // Save changes
        var result = await context.SaveChangesAsync() > 0;
        if (!result)
            return BadRequest("Could not save changes to the database");

        restaurant.Owner = User.Identity.Name;

        await publishEndpoint.Publish(mapper.Map<MenuUpdated>(restaurant));

        var result2 = await context.SaveChangesAsync() > 0;
        if (!result2)
            return BadRequest("Could not save changes to the database");

        return Ok(mapper.Map<RestaurantDto>(restaurant));
    }
}