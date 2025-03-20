using System;
using AutoMapper;
using Contracts;
using RatingService.Models;

namespace RatingService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<OrderPlaced, FoodOrder>();
    }
}
