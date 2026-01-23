using AutoMapper;
using Sireen.Application.DTOs.Amenities;
using Sireen.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sireen.Application.Mapping
{
    public class AmenityProfile : Profile
    {
        public AmenityProfile() {
            CreateMap<Amenity, AmenityDto>();

            CreateMap<CreateAmenityDto,Amenity>();

            CreateMap<Amenity, DisplayAmenityDto>()
               .ForMember(dest => dest.IsFree, opt => opt.MapFrom(src => true));

            CreateMap<UpdateAmenityDto, Amenity>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));       
        }
    }
}
