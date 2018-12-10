using System;
using System.Collections.Generic;
using Services.Commmon;
using Share.Models.Common;
using Microsoft.Extensions.Configuration;
using Share.Utilities;
using Newtonsoft.Json;
using System.Linq;
using Share.Extensions;

namespace Services.Common
{
    public class JsonDataService : IJsonDataService
    {
        private readonly IConfiguration _configuration;
        private readonly IFileRead _fileReadService;
        private readonly ICacheService _cacheService;

        public JsonDataService(IConfiguration configuration, ICacheService cacheService, IFileRead fileReadService)
        {
            _configuration = configuration;
            _fileReadService = fileReadService;
            _cacheService = cacheService;
        }

        public List<Country> GetCountries()
        {
            List<Country> countries = _cacheService.Get<List<Country>>(nameof(GetCountries));
            if (countries == null)
            {
                string json = _fileReadService.ReadFile(_configuration["FolderPathForCountryJson"]);
                Countries jsonCountry = JsonConvert.DeserializeObject<Countries>(json);
                jsonCountry = _cacheService.GetOrAdd<Countries>(nameof(GetCountries), () => jsonCountry, _configuration["CacheExpireDays"].ConvertToCacheExpireDateTime());
                if (jsonCountry == null)
                    throw new Exception(nameof(jsonCountry.Country));
                countries = jsonCountry.Country;
            }
            return countries;
        }

        public LookUp GetLookUpBy()
        {
            LookUp lookUp = _cacheService.Get<LookUp>(nameof(GetLookUpBy));
            if (lookUp == null)
            {
                string json = _fileReadService.ReadFile(_configuration["FolderPathForLookUpJson"]);
                lookUp = JsonConvert.DeserializeObject<LookUp>(json);
                IsValidLookUp(lookUp);
                lookUp = _cacheService.GetOrAdd<LookUp>(nameof(GetLookUpBy), () => lookUp, _configuration["CacheExpireDays"].ConvertToCacheExpireDateTime());
                IsValidLookUp(lookUp);
            }
            return lookUp;
        }

        private void IsValidLookUp(LookUp lookUp)
        {
            if (lookUp == null ||
                    lookUp.CategoryOptionsBy == null || lookUp.CategoryOptionsBy.Count == 0 ||
                    lookUp.ConditionOptionsBy == null || lookUp.ConditionOptionsBy.Count == 0 ||
                    lookUp.MileOptionsBy == null || lookUp.MileOptionsBy.Count == 0 ||
                lookUp.SortOptionsBy == null || lookUp.SortOptionsBy.Count == 0)
            throw new Exception(nameof(LookUp));
        }

        public List<KeyValueDescription> GetCategories() => GetLookUpBy().CategoryOptionsBy;
        public bool IsValidCategory(int categoryId) => GetCategories().Any(c => c.Key == categoryId);
        public KeyValueDescription GetCategoryOrDefault(byte cId) => this.GetCategories().FirstOrDefault(o => o.Key == cId) ?? this.GetCategories().First();

        public List<KeyValueDescription> GetConditions() => GetLookUpBy().ConditionOptionsBy;
        public bool IsValidCondition(int conditionId) => GetConditions().Any(c => c.Key == conditionId);
        public KeyValueDescription GetConditionOrDefault(byte cId) => this.GetConditions().FirstOrDefault(o => o.Key == cId) ?? this.GetConditions().First();

        public bool IsValidCallingCode(int callingCode) => GetCountries().Any(c => c.CountryCallingCode == callingCode);
        public bool IsValidCountryCode(string countryCode) => GetCountries().Any(c => c.CountryCode == countryCode);
        public bool IsValidCurrencyCode(string currencyCode) => GetCountries().Any(c => c.CurrencyCode == currencyCode);

        public List<KeyValueDescription> GetMileOptionsBy() => GetLookUpBy().MileOptionsBy;
        public bool IsValidMileOption(int mileOptionId) => GetMileOptionsBy().Any(c => c.Key == mileOptionId);
        public KeyValueDescription GetMileOptionById(int mileOptionId) => GetMileOptionsBy().FirstOrDefault(c => c.Key == mileOptionId);

        public List<KeyValueDescription> GetSortOptionsBy() => GetLookUpBy().SortOptionsBy;
        public bool IsValidSortOption(int sortOptionId) => GetSortOptionsBy().Any(c => c.Key == sortOptionId);
        public KeyValueDescription GetSortOptionByIdOrDefault(byte sortOptionId) => this.GetSortOptionsBy().FirstOrDefault(o => o.Key == sortOptionId) ?? this.GetSortOptionsBy().First();
    }

    public interface IJsonDataService
    {
        LookUp GetLookUpBy();
        List<Country> GetCountries();

        List<KeyValueDescription> GetCategories();
        bool IsValidCategory(int categoryId);
        KeyValueDescription GetCategoryOrDefault(byte cId);

        List<KeyValueDescription> GetConditions();
        KeyValueDescription GetConditionOrDefault(byte conditionId);
        bool IsValidCondition(int conditionId);

        bool IsValidCallingCode(int callingCode);
        bool IsValidCountryCode(string countryCode);
        bool IsValidCurrencyCode(string currencyCode);

        List<KeyValueDescription> GetMileOptionsBy();
        bool IsValidMileOption(int mileOptionId);
        KeyValueDescription GetMileOptionById(int mileOptionId);

        List<KeyValueDescription> GetSortOptionsBy();
        bool IsValidSortOption(int sortOptionId);
        KeyValueDescription GetSortOptionByIdOrDefault(byte sortOptionId);
    }
}
