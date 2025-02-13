using AutoMapper;
using Contracts;
using RestaurantService.DTOs;
using RestaurantService.Entities;

namespace RestaurantService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Restaurant, RestaurantDto>();
        CreateMap<FoodCategory, FoodCategoryDto>();
        CreateMap<FoodItem, FoodItemDto>();
        CreateMap<Variation, VariationDto>();
        CreateMap<VariationOption, VariationOptionDto>();
        CreateMap<CreateRestaurantDto, Restaurant>();
        CreateMap<RestaurantDto, RestaurantCreated>();
        CreateMap<Restaurant, RestaurantUpdated>();
        CreateMap<CreateFoodCategoryDto, FoodCategory>();
        CreateMap<CreateFoodItemDto, FoodItem>();
        CreateMap<CreateVariationDto, Variation>();
        CreateMap<CreateVariationOptionDto, VariationOption>();
        CreateMap<FoodCategoryDto, FoodCategory>();
        CreateMap<FoodItemDto, FoodItem>();
        CreateMap<VariationDto, Variation>();
        CreateMap<VariationOptionDto, VariationOption>();

        CreateMap<FoodCategoryDto, FoodCategoryUpdated>();
        CreateMap<FoodItemDto, FoodItemUpdated>();
        CreateMap<VariationDto, VariationUpdated>();
        CreateMap<VariationOptionDto, VariationOptionUpdated>();

        CreateMap<RestaurantDto, MenuUpdated>();
        CreateMap<Restaurant, MenuUpdated>();

        CreateMap<FoodCategory, FoodCategoryUpdated>();
        CreateMap<FoodItem, FoodItemUpdated>();
        CreateMap<Variation, VariationUpdated>();
        CreateMap<VariationOption, VariationOptionUpdated>();
    }
}
