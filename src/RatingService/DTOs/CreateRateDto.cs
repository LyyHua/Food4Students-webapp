using System;

namespace RatingService.DTOs;

public class CreateRateDto
{
    public string Comment { get; set; }
    public int Stars { get; set; }
}
