using System;
using System.IO;
using AutoMapper;
using Share.Models.Ad.Dtos;
using DbContexts.Ad;
using Repository;
using System.Text;
using Services.Commmon;
using Share._3rdParty;
using Microsoft.Extensions.Configuration;
using Share.Extensions;
using NetTopologySuite.Geometries;
using Share.Utilities;

namespace Services.Ad
{
    public class AdCreateService : IAdCreateService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileRead _fileReadService;
        private readonly IMapper _mapper;
        private readonly IGoogleStorage _googleStorage;
        private readonly IRepository<Share.Models.Ad.Entities.Ad, AdDbContext> _adRepository;

        public AdCreateService(IMapper mapper, IFileRead fileReadService, IGoogleStorage googleStorage,
            IRepository<Share.Models.Ad.Entities.Ad, AdDbContext> adRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _mapper = mapper;
            _fileReadService = fileReadService;
            _googleStorage = googleStorage;
            _adRepository = adRepository;
        }

        // NOTE: transaction has to implement or not , has to think more required.
        public AdDto CreateAd(AdDto dto)
        {
            #region Ad
            Share.Models.Ad.Entities.Ad ad = _mapper.Map<Share.Models.Ad.Entities.Ad>(dto);
            ad.UserPhoneNumber = dto.UserPhoneNumber.ConvertToLongOrDefault();
            ad.UserPhoneCountryCode = dto.UserPhoneCountryCode.ConvertToShortOrDefault();
            int SRID = _configuration["SRID"].ConvertToInt();
            ad.AddressLocation = dto.IsValidLocation ? new Point(dto.Longitude, dto.Latitude) { SRID = SRID } : new Point(0.0, 0.0) { SRID = SRID };
            RepositoryResult result = _adRepository.Create(ad);
            if (!result.Succeeded)
                throw new Exception(string.Join(Path.PathSeparator, result.Errors));
            #endregion

            #region Google
            string content = _fileReadService.ReadFile(_configuration["FolderPathForGoogleHtmlTemplate"]);
            content = content.ToParseLiquidRender(dto);
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            string Dothtml = Path.GetExtension(_configuration["FolderPathForGoogleHtmlTemplate"]);

            var bucketName = _configuration["AdBucketNameInGoogleCloudStorage"];
            var objectName = string.Format("{0}{1}", dto.AttachedAssetsInCloudStorageId.Value, Dothtml);
            var contentType = Utility.GetMimeTypes()[Dothtml];
            _googleStorage.UploadObject(bucketName, stream, objectName, contentType);
            #endregion

            return dto;
        }
    }

    public interface IAdCreateService
    {
        AdDto CreateAd(AdDto adDto);
    }
}
