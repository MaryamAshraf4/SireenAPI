using AutoMapper;
using Sireen.API.DTOs.RoomImages;
using Sireen.Application.DTOs.RoomImages;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Mapping
{
    public class RoomImageProfile : Profile
    {
        public RoomImageProfile()
        {
            CreateMap<RoomImageUploadDto, RoomImage>();
            CreateMap<RoomImage, RoomImageDto>();
        }
    }
}
