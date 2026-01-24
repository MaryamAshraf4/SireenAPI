using AutoMapper;
using Sireen.API.DTOs.HotelImages;
using Sireen.Application.DTOs.HotelImages;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Mapping
{
    public class HotelImageProfile : Profile
    {
        public HotelImageProfile()
        {
            CreateMap<HotelImageUploadDto, HotelImage>();
            CreateMap<HotelImage, HotelImageDto>();
        }
    }
}
