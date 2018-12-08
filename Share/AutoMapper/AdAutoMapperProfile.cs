using AutoMapper;
using Share.Models.Ad.Dtos;

namespace Share.AutoMapper
{
    public class AdAutoMapperProfile : Profile
    {
        public AdAutoMapperProfile()
        {
            CreateMap<Share.Models.Ad.Entities.Ad, AdDto>()
                .ReverseMap()
                .ForMember(dest => dest.AddressLocation, opt => opt.Ignore());
            //.ForMember(dest => dest.UserPhoneCountryCode, opt => opt.MapFrom(src => short.Parse(strin.isnullorempty(src.UserPhoneCountryCode) ? src.UserPhoneCountryCode : null )))
        }
    }
}
