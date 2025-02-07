using AutoMapper;
using Contracts;
using RestaurantService.DTOs;
using RestaurantService.Entities;

namespace RestaurantService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.Logo.Url))
            .ForMember(dest => dest.BannerUrl, opt => opt.MapFrom(src => src.Banner.Url));
        CreateMap<FoodCategory, FoodCategoryDto>();
        CreateMap<FoodItem, FoodItemDto>();
        CreateMap<Variation, VariationDto>();
        CreateMap<VariationOption, VariationOptionDto>();
        CreateMap<CreateAndUpdateRestaurantDto, Restaurant>();
        CreateMap<RestaurantDto, RestaurantCreated>();
        CreateMap<Restaurant, RestaurantUpdated>();
    }
}
