using System;
using AutoMapper;
using Share.Models.Ad.Dtos;
using DbContexts.Ad;
using Repository;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Share.Extensions;
using Services.Commmon;
using Microsoft.Extensions.Configuration;
using Services.Common;
using Share.Enums;

namespace Services.Ad
{
    public class AdSearchService : IAdSearchService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IRepository<Share.Models.Ad.Entities.Ad, AdDbContext> _adRepository;
        private readonly IJsonDataService _jsonDataService;

        public AdSearchService(ILogger<AdService> logger, IMapper mapper, ICacheService cacheService, 
            IRepository<Share.Models.Ad.Entities.Ad, AdDbContext> adRepository,
            IConfiguration configuration, IJsonDataService jsonDataService)
        {
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _cacheService = cacheService;
            _adRepository = adRepository;
            _jsonDataService = jsonDataService;
        }
        //https://dzone.com/articles/using-the-angular-material-paginator-with-aspnet-c
        // https://github.com/dncuug/X.PagedList
        //https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/sort-filter-page?view=aspnetcore-2.1
        //https://docs.microsoft.com/en-us/sql/relational-databases/search/query-with-full-text-search?view=sql-server-2017
        //https://github.com/uber-asido/backend/blob/e32bf1ddabe500002d835228993707503449e06c/src/Uber.Module.Search.EFCore/Store/SearchItemStore.cs
        //https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql/blob/42c335ceac6d93d1c0487ef45fc992810c07fd9d/upstream/EFCore.Upstream.FunctionalTests/Query/DbFunctionsMySqlTest.cs
        public dynamic SearchAds(AdSearchDto options)
        {
            IQueryable<Share.Models.Ad.Entities.Ad> query = _adRepository.Entities.AsNoTracking().Where(w => w.IsPublished && w.IsActivated && !w.IsDeleted);

            // implemented and chosen FreeText from 4 options: 1.FreeText 2.Contains 3.ContainsTable 4.FreeTextTable
            // figure out later: SqlServerDbFunctionsExtensions
            if (options.IsValidSearchText)
            {
                //query = query.Where(ft => EF.Functions.FreeText(ft.AdContent, options.SearchText));
                query = query.Where(ft => EF.Functions.FreeText(ft.AdTitle, options.SearchText));
            }

            string connection = "Server=localhost;Database=Ad;Trusted_Connection=True;";
            var optionBuilder = new DbContextOptionsBuilder<AdDbContext>().UseSqlServer(connection, ya => ya.UseNetTopologySuite());
            AdDbContext context = new AdDbContext(optionBuilder.Options);

            IQueryable<Share.Models.Ad.Entities.Ad> query12 = context.Ads.Where(w => w.IsPublished && w.IsActivated && !w.IsDeleted);
            var sfsfs = query12.ToList();

            IQueryable<Share.Models.Ad.Entities.Ad> query1 = context.Ads.Where(ft => EF.Functions.Contains(ft.AdTitle, "title", 1033));
            var aaa = query1.ToList();
            //query1 = query1.Where(ft => EF.Functions.FreeText(ft.AdTitle, "title", 1033));  // 0: newtral, 1033: english

            string sql = query1.ToSql<Share.Models.Ad.Entities.Ad>();


            if (options.IsValidCategory)
                query = query.Where(q => q.AdCategoryId == options.CategoryId);
            if (options.IsValidCondition)
                query = query.Where(q => q.ItemConditionId == options.ConditionId);
            if (options.IsValidCountryCode)
                query = query.Where(q => q.AddressCountryCode.Trim().ToUpper() == options.CountryCode);
            if (options.IsValidCurrencyCode)
                query = query.Where(q => q.ItemCurrencyCode.Trim().ToUpper() == options.CurrencyCode);
            if (options.IsValidCityName)
                query = query.Where(q => q.AddressCity.Trim().ToLower() == options.CityName);
            if (options.IsValidZipCode)
                query = query.Where(q => q.AddressZipCode.Trim().ToLower() == options.ZipCode);

            if (options.IsValidPrice)
                query = query.Where(q => q.ItemCost >= options.ItemCostMin && q.ItemCost <= options.ItemCostMax);
            else if (options.IsValidMinPrice)
                query = query.Where(q => q.ItemCost >= options.ItemCostMin);
            else if (options.IsValidMinPrice)
                query = query.Where(q => q.ItemCost <= options.ItemCostMax);


            if (options.IsValidSortOption)
            {
                switch ((SortOptionsBy)options.SortOptionsId)
                {
                    case SortOptionsBy.ClosestFirst:
                        // handled below in same function , ref : line #73
                        break;
                    case SortOptionsBy.NewestFirst:
                        query = query.OrderByDescending(o => o.UpdatedDateTime);
                        break;
                    case SortOptionsBy.PriceHighToLow:
                        query = query.OrderByDescending(o => o.ItemCost);
                        break;
                    case SortOptionsBy.PriceLowToHigh:
                        query = query.OrderBy(o => o.ItemCost);
                        break;
                    default:
                        break;
                }
            }

            if ((SortOptionsBy)options.SortOptionsId == SortOptionsBy.ClosestFirst && options.IsValidLocation)
            {
                if (options.IsValidMileOption)
                {
                    if ((MileOptionsBy)options.SortOptionsId == MileOptionsBy.Maximum)
                        query = query.OrderBy(o => o.AddressLocation.Distance(options.MapLocation));
                    else
                        query = query.OrderBy(o => o.AddressLocation.Distance(options.MapLocation) < options.Miles);
                }
                else
                    query = query.OrderBy(o => o.AddressLocation.Distance(options.MapLocation));
            }
            else if (options.IsValidMileOption && options.IsValidLocation)
            {
                if ((MileOptionsBy)options.SortOptionsId == MileOptionsBy.Maximum)
                    query = query.OrderBy(o => o.AddressLocation.Distance(options.MapLocation));
                else
                    query = query.OrderBy(o => o.AddressLocation.Distance(options.MapLocation) < options.Miles);
            }

            List<Share.Models.Ad.Entities.Ad> a = query.ToList();
            //paging
            query = query.Take(options.DefaultPageSize);

            // select columns:
            List<AdDto> adDtos =  query.Select(s => new AdDto()
            {
                AdId = s.AdId.ToString(),
                AdTitle = s.AdTitle,
                UpdatedDateTimeString = s.UpdatedDateTime.TimeAgo(),
                UserIdOrEmail = s.UserIdOrEmail,
            }).ToList<AdDto>();

            

            return new { records = adDtos, options = options };
        }
    }

    public interface IAdSearchService
    {
        dynamic SearchAds(AdSearchDto options);
    }
}
