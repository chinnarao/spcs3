using AutoMapper;
using Share.Models.Ad.Dtos;

namespace Share._3rdParty
{
    public class AdAutoMapperProfile : Profile
    {
        public AdAutoMapperProfile()
        {
            CreateMap<Share.Models.Ad.Entities.Ad, AdDto>()
                .ReverseMap()
                .ForMember(dest => dest.AddressLocation, opt => opt.Ignore());
        }
    }
}
