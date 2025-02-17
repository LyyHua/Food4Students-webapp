using System;
using AutoMapper;
using Contracts;
using OrderService.DTOs;
using OrderService.Models;

namespace OrderService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RestaurantCreated, Restaurant>();
        CreateMap<RestaurantUpdated, Restaurant>();
        CreateMap<FoodCategoryUpdated, FoodCategory>();
        CreateMap<FoodItemUpdated, FoodItem>();
        CreateMap<VariationUpdated, Variation>();
        CreateMap<VariationOptionUpdated, VariationOption>();
        CreateMap<MenuUpdated, Restaurant>();
        CreateMap<CreateOrderDto, FoodOrder>();
        CreateMap<CreateOrderItemDto, OrderItem>();
        CreateMap<FoodOrder, OrderPlaced>();
        CreateMap<FoodOrder, OrderStatusUpdated>();
        CreateMap<OrderItem, OrderItemUpdated>();
    }
}
