using AutoMapper;
using DbContexts.Ad;
using Repository;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Share.Models.Ad.Dtos;
using System.Data.SqlClient;
using System.Linq;

namespace Services.Ad
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IRepository<Share.Models.Ad.Entities.Ad, AdDbContext> _adRepository;

        public UserService(IMapper mapper, IRepository<Share.Models.Ad.Entities.Ad, AdDbContext> adRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _mapper = mapper;
            _adRepository = adRepository;
        }

        public bool Activate(long adId)
        {
            int count = 0;
            using (_adRepository.Context)
            {
                var rawSqlString = "UPDATE [Ad].[dbo].[Ad] SET [IsActivated] = 1, [ActivatedDateTime] = GETDATE() WHERE [AdId] = @AdId"; //636800074153093334
                SqlParameter[] parameters = { new SqlParameter("@AdId", adId) };
                count = _adRepository.ExecuteSqlCommand(rawSqlString, parameters);
            }
            return count > 0;
        }

        public bool DeActivate(long adId)
        {
            int count = 0;
            using (_adRepository.Context)
            {
                var rawSqlString = "UPDATE [Ad].[dbo].[Ad] SET [IsActivated] = 0, [ActivatedDateTime] = GETDATE() WHERE [AdId] = @AdId";
                SqlParameter[] parameters = { new SqlParameter("@AdId", adId) };
                count = _adRepository.ExecuteSqlCommand(rawSqlString, parameters);
            }
            return count > 0;
        }

        public bool Delete(long adId)
        {
            int count = 0;
            using (_adRepository.Context)
            {
                var rawSqlString = "UPDATE [Ad].[dbo].[Ad] SET [IsDeleted] = 1 , [DeletedDateTime] = GETDATE() WHERE [AdId] = @AdId";
                SqlParameter[] parameters = { new SqlParameter("@AdId", adId) };
                count = _adRepository.ExecuteSqlCommand(rawSqlString, parameters);
            }
            return count > 0;
        }

        public List<AdDto> GetAds(string userIdOrEmail, string userSocialProviderName)
        {
            List<Share.Models.Ad.Entities.Ad>  ads = _adRepository.By(a => a.UserIdOrEmail == userIdOrEmail && a.UserSocialProviderName == userSocialProviderName).ToList();
            List<AdDto> adDtos = _mapper.Map<List<AdDto>>(ads);
            return adDtos;
        }
    }

    public interface IUserService
    {
        List<AdDto> GetAds(string userIdOrEmail, string userSocialProviderName);
        bool Activate(long adId);
        bool DeActivate(long adId);
        bool Delete(long adId);
    }
}
