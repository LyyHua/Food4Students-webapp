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
        CreateMap<CreateAndUpdateRestaurantDto, Restaurant>();
        CreateMap<Restaurant, CreateAndUpdateRestaurantDto>();
        CreateMap<Restaurant, RestaurantCreatedAndUpdated>();
        CreateMap<RestaurantDto, RestaurantCreatedAndUpdated>();
    }
}
