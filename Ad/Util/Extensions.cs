using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Share.Models.Ad.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Share.Constants;
using Share.Utilities;
using Services.Common;
using Share.Models.Common;
using Share.Enums;
using GeoAPI.Geometries;

namespace Ad.Util
{
    public static class Extensions
    {
        public static IEnumerable<string> Errors(this ModelStateDictionary ModelState)
        {
            return ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        }

        public static void Defaults(this AdDto model, IConfiguration _configuration)
        {
            byte adDefaultDisplayActiveDays;
            if (byte.TryParse(_configuration["AdDefaultDisplayActiveDays"], out adDefaultDisplayActiveDays))
                model.AdDisplayDays = adDefaultDisplayActiveDays;

            double Longitude;
            if (double.TryParse(_configuration["DefaultLongitude"], out Longitude))
                model.AddressLongitude = "1.0";

            double Lattitude;
            if (double.TryParse(_configuration["DefaultLattitude"], out Lattitude))
                model.AddressLatitude = "1.0";

            model.AdId = DateTime.UtcNow.Ticks.ToString();
            model.AttachedAssetsInCloudStorageId = Guid.NewGuid();
            model.CreatedDateTime = model.UpdatedDateTime = DateTime.UtcNow;
            model.IsDeleted = model.IsActivated = model.IsPublished = false;
        }

        public static void Values(this GoogleStorageAdFileDto fileModel, IConfiguration _configuration, Guid attachedAssetsInCloudStorageGuId)
        {
            string googleStorageBucketName = _configuration["AdBucketNameInGoogleCloudStorage"];
            string htmlFileName = _configuration["AdHtmlTemplateFileNameWithExt"];
            string GoogleStorageFileExtType = Path.GetExtension(htmlFileName);

            fileModel.CacheExpiryDateTimeForHtmlTemplate = Utility.GetCacheExpireDateTime(_configuration["CacheExpireDays"]);
            fileModel.HtmlFileTemplateFullPathWithExt = Path.Combine(Directory.GetCurrentDirectory(), htmlFileName);
            fileModel.GoogleStorageBucketName = googleStorageBucketName;
            fileModel.CACHE_KEY = Constants.AD_HTML_FILE_TEMPLATE;
            fileModel.GoogleStorageObjectNameWithExt = string.Format("{0}{1}", attachedAssetsInCloudStorageGuId, GoogleStorageFileExtType);
            fileModel.ContentType = Utility.GetMimeTypes()[GoogleStorageFileExtType];
        }

        public static KeyValuePair<bool,string> IsValidSearchInputs(this AdSearchDto options, IJsonDataService _jsonDataService)
        {
            List<string> errors = new List<string>();

            if (!string.IsNullOrEmpty(options.SearchText))
            {
                options.IsValidSearchText = true;
                options.SearchText = options.SearchText.Trim().ToLower();
            }

            options.IsValidCategory = true;
            options.ConditionId = _jsonDataService.GetCategoryOrDefault(options.CategoryId).Key;

            options.IsValidCondition = true;
            options.ConditionId = _jsonDataService.GetConditionOrDefault(options.ConditionId).Key;

            if (_jsonDataService.IsValidCountryCode(options.CountryCode))
            {
                options.IsValidCountryCode = true;
                options.CountryCode = options.CountryCode.Trim().ToUpper();
            }

            if (_jsonDataService.IsValidCurrencyCode(options.CurrencyCode))
            {
                options.IsValidCurrencyCode = true;
                options.CurrencyCode = options.CurrencyCode.Trim().ToUpper();
            }

            if (!string.IsNullOrEmpty(options.CityName))
            {
                options.IsValidCityName = true;
                options.CityName = options.CityName.Trim().ToLower();
            }

            if (!string.IsNullOrEmpty(options.ZipCode))
            {
                options.IsValidZipCode = true;
                options.ZipCode = options.ZipCode.Trim().ToLower();
            }

            if (!string.IsNullOrWhiteSpace(options.MinPrice) && !string.IsNullOrWhiteSpace(options.MaxPrice))
            {
                options.ItemCostMin = Utility.ConvertToDoubleFromString(options.MinPrice);
                options.ItemCostMax = Utility.ConvertToDoubleFromString(options.MaxPrice);
                if (options.ItemCostMin >= 0 && options.ItemCostMax >= 0 && options.ItemCostMin <= options.ItemCostMax)
                    options.IsValidPrice = true;
            }
            else if (!string.IsNullOrWhiteSpace(options.MinPrice) && string.IsNullOrWhiteSpace(options.MaxPrice))
            {
                options.ItemCostMin = Utility.ConvertToDoubleFromString(options.MinPrice);
                if (options.ItemCostMin > 0)
                    options.IsValidMinPrice = true;
            }
            else if (string.IsNullOrWhiteSpace(options.MinPrice) && !string.IsNullOrWhiteSpace(options.MaxPrice))
            {
                options.ItemCostMax = Utility.ConvertToDoubleFromString(options.MaxPrice);
                if (options.ItemCostMax > 0)
                    options.IsValidMaxPrice = true;
            }

            KeyValueDescription mileOption = _jsonDataService.GetMileOptionById(options.MileOptionsId);
            if (mileOption != null)
            {
                options.IsValidMileOption = true;
                if (options.MileOptionsId == byte.MaxValue)
                    options.Miles = double.MaxValue;
                else
                    options.Miles = options.MileOptionsId;
            }

            options.IsValidSortOption = true;
            options.SortOptionsId = _jsonDataService.GetSortOptionByIdOrDefault(options.SortOptionsId).Key;

            if (!string.IsNullOrWhiteSpace(options.MapLattitude)
                && !string.IsNullOrWhiteSpace(options.MapLongitude)
                && Utility.IsValidLatitude(options.MapLattitude)
                && Utility.IsValidLongitude(options.MapLongitude))
            {
                IPoint point = Utility.CreatePoint(longitude: options.MapLongitude, latitude: options.MapLattitude);
                if (point != null)
                {
                    options.IsValidLocation = true;
                    options.MapLocation = point;
                }
            }

            return new KeyValuePair<bool, string>(errors.Count > 0, string.Join(Path.PathSeparator, errors));
        }
    }
}
