using AutoMapper;
using Sireen.Application.DTOs.Amenities;
using Sireen.Application.DTOs.Ratings;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Mapping
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<CreateRatingDto, Rating>();
            CreateMap<Rating, HotelRatingDto>();
            CreateMap<Rating, ClientRatingDto>();
        }
    }
}
