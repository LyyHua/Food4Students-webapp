using System;
using AutoMapper;
using Contracts;
using SearchService.Models;

namespace SearchService.RequestHelpers;

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
    }
}
