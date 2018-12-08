using AutoMapper;
using Share.Models.Article.Entities;
using Share.Models.Article.Dtos;

// https://github.com/heymdeel/CarHelp/blob/683de14e157527472cde266f597930daf5b4cfe7/src/CarHelp.ApplicationLayer/Models/MappingProfileAppLayer.cs
namespace Share._3rdParty
{
    public class ArticleAutoMapperProfile : Profile
    {
        public ArticleAutoMapperProfile()
        {
            //CreateMap<CreateOrderInput, Order>()
            //    .ForMember(o => o.Location, s => s.MapFrom(x => new Point(new Coordinate(x.Location.Longitude, x.Location.Latitude)) { SRID = 4326 }));

            //https://github.com/Hinaar/KutyApp/blob/4564904bbb4397d66a7375461eb4df337aa8bc58/KutyApp.Services.Environment.Bll/Mapping/LocationResolver.cs
            //CreateMap<AddOrEditPoiDto, Poi>().ForMember(p => p.Location, m => m.ResolveUsing<LocationResolver>());
            //    public class LocationResolver : IValueResolver<AddOrEditPoiDto, Poi, IPoint>
            //{
            //    private ILocationManager LocationManager { get; }

            //    public LocationResolver(ILocationManager locationManager)
            //    {
            //        LocationManager = locationManager;
            //    }

            //    public IPoint Resolve(AddOrEditPoiDto source, Poi destination, IPoint destMember, ResolutionContext context) =>
            //        LocationManager.GeometryFactory.CreatePoint(new Coordinate(source.Latitude, source.Longitude));
            //}


            CreateMap<ArticleLicenseDto, ArticleLicense>().ForMember(dest => dest.Article, opt => opt.Ignore()).ReverseMap();
            CreateMap<ArticleCommitDto, ArticleCommit>().ForMember(dest => dest.Article, opt => opt.Ignore()).ReverseMap();
            CreateMap<ArticleCommentDto, ArticleComment>().ForMember(dest => dest.Article, opt => opt.Ignore()).ReverseMap();

            CreateMap<ArticleDto, Share.Models.Article.Entities.Article>()
                .ForMember(dest => dest.ArticleLicense, opt => opt.MapFrom(src => src.ArticleLicenseDto))
                .ForMember(dest => dest.ArticleCommits, opt => opt.MapFrom(src => src.ArticleCommitDtos))
                .ForMember(dest => dest.ArticleComments, opt => opt.MapFrom(src => src.ArticleCommentDtos));

            CreateMap<Share.Models.Article.Entities.Article, ArticleDto>()
                .ForMember(dest => dest.ArticleLicenseDto, opt => opt.MapFrom(src => src.ArticleLicense))
                .ForMember(dest => dest.ArticleCommitDtos, opt => opt.MapFrom(src => src.ArticleCommits))
                .ForMember(dest => dest.ArticleCommentDtos, opt => opt.MapFrom(src => src.ArticleComments))
                .ForMember(dest => dest.GoogleStorageArticleFileDto, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDateTimeString, opt => opt.Ignore());
        }
    }
}
