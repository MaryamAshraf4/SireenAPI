using AutoMapper;
using Sireen.Application.DTOs.Amenities;
using Sireen.Application.DTOs.Rooms;
using Sireen.Domain.Enums;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Mapping
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomDto>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.ID))
            .ForMember(dest => dest.RoomType,
                opt => opt.MapFrom(src => src.RoomType.ToString()))
            .ForMember(dest => dest.RoomStatus,
                opt => opt.MapFrom(src => src.RoomStatus.ToString()))
            .ForMember(dest => dest.RoomImages,
                opt => opt.MapFrom(src =>
                    src.RoomImages.Select(i => i.ImageUrl)));

            CreateMap<Room, DisplayRoomDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.RoomType,
                    opt => opt.MapFrom(src => src.RoomType.ToString()))
                .ForMember(dest => dest.RoomStatus,
                    opt => opt.MapFrom(src => src.RoomStatus.ToString()))
                .ForMember(dest => dest.RoomImages,
                    opt => opt.MapFrom(src =>
                        src.RoomImages.Select(i => i.ImageUrl)));

            CreateMap<CreateRoomDto, Room>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.RoomStatus, opt => opt.MapFrom(_ => RoomStatus.Available));

            CreateMap<UpdateRoomDto, Room>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}
