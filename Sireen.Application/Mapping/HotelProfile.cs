using AutoMapper;
using Sireen.Application.DTOs.Hotels;
using Sireen.Application.DTOs.Rooms;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Mapping
{
    public class HotelProfile : Profile
    {
        public HotelProfile()
        {
            CreateMap<CreateHotelDto, Hotel>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false));

            CreateMap<UpdateHotelDto, Hotel>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); 

            CreateMap<Hotel, HotelDto>()
                .ForMember(dest => dest.HotelImages, opt => opt.MapFrom(src => src.HotelImages.Select(img => img.ImageUrl)));

            CreateMap<Hotel, DisplayHotelDto>()
                .ForMember(dest => dest.HotelImages,opt => opt.MapFrom(src => src.HotelImages.Select(img => img.ImageUrl)))
                .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms));

            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.RoomType.ToString()))
                .ForMember(dest => dest.RoomStatus, opt => opt.MapFrom(src => src.RoomStatus.ToString()))
                .ForMember(dest => dest.RoomImages, opt => opt.MapFrom(src => src.RoomImages.Select(img => img.ImageUrl)));
        }
    }
}
