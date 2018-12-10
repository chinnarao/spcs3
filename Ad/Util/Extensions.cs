using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Share.Models.Ad.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Services.Common;
using Share.Models.Common;
using Share.Extensions;
using NetTopologySuite.Geometries;

namespace Ad.Util
{
    public static class Extensions
    {
        public static IEnumerable<string> Errors(this ModelStateDictionary ModelState)
        {
            return ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        }

        public static KeyValuePair<bool, string> IsValidCreateAdInputs(this AdDto dto, IConfiguration _configuration, IJsonDataService _jsonDataService)
        {
            List<string> errors = new List<string>();

            dto.AdId = DateTime.UtcNow.Ticks.ToString();
            dto.AdDisplayDays = _configuration["AdDefaultDisplayActiveDays"].ConvertToByte();
            dto.AttachedAssetsInCloudStorageId = Guid.NewGuid();
            dto.CreatedDateTime = dto.UpdatedDateTime = DateTime.UtcNow;
            dto.IsDeleted = false;
            dto.IsActivated = dto.IsPublished = true;

            if (string.IsNullOrWhiteSpace(_configuration["FolderPathForGoogleHtmlTemplate"]))
                errors.Add("FolderPathForGoogleHtmlTemplate");
            if (string.IsNullOrWhiteSpace(_configuration["AdBucketNameInGoogleCloudStorage"]))
                errors.Add("AdBucketNameInGoogleCloudStorage");
            if (string.IsNullOrWhiteSpace(_configuration["CacheExpireDays"]))
                errors.Add("CacheExpireDays");
            
            if (!_jsonDataService.IsValidCategory(dto.AdCategoryId))
                errors.Add(nameof(dto.AdCategoryId));
            if (!_jsonDataService.IsValidCategory(dto.ItemConditionId))
                errors.Add(nameof(dto.ItemConditionId));
            //if (!_jsonDataService.IsValidCallingCode(int.Parse(dto.UserPhoneCountryCode)))
            //    errors.Add(nameof(dto.UserPhoneCountryCode));

            if (dto.AddressLongitude.IsValidLocation(dto.AddressLatitude))
                dto.IsValidLocation = true;
            if (dto.IsValidLocation)
            {
                dto.Longitude = dto.AddressLongitude.ConvertToDouble();
                dto.Latitude = dto.AddressLatitude.ConvertToDouble();
            }

            return new KeyValuePair<bool, string>(errors.Count > 0, string.Join(Path.PathSeparator, errors));
        }

        public static KeyValuePair<bool,string> IsValidSearchInputs(this AdSearchDto options, IConfiguration _configuration, IJsonDataService _jsonDataService)
        {
            List<string> errors = new List<string>();

            #region All General
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
            #endregion

            #region Price
            if (!string.IsNullOrWhiteSpace(options.MinPrice) && !string.IsNullOrWhiteSpace(options.MaxPrice))
            {
                options.ItemCostMin = options.MinPrice.ConvertToDoubleOrZero();
                options.ItemCostMax = options.MaxPrice.ConvertToDoubleOrZero();
                if (options.ItemCostMin >= 0 && options.ItemCostMax >= 0 && options.ItemCostMin <= options.ItemCostMax)
                    options.IsValidPrice = true;
            }
            else if (!string.IsNullOrWhiteSpace(options.MinPrice) && string.IsNullOrWhiteSpace(options.MaxPrice))
            {
                options.ItemCostMin = options.MinPrice.ConvertToDoubleOrZero();
                if (options.ItemCostMin > 0)
                    options.IsValidMinPrice = true;
            }
            else if (string.IsNullOrWhiteSpace(options.MinPrice) && !string.IsNullOrWhiteSpace(options.MaxPrice))
            {
                options.ItemCostMax = options.MaxPrice.ConvertToDoubleOrZero();
                if (options.ItemCostMax > 0)
                    options.IsValidMaxPrice = true;
            }
            #endregion

            #region Mile Option
            KeyValueDescription mileOption = _jsonDataService.GetMileOptionById(options.MileOptionsId);
            if (mileOption != null)
            {
                options.IsValidMileOption = true;
                if (options.MileOptionsId == byte.MaxValue)
                    options.Miles = double.MaxValue;
                else
                    options.Miles = options.MileOptionsId;
            }
            #endregion

            #region Location
            if (options.MapLongitude.IsValidLocation(options.MapLatitude))
                options.IsValidLocation = true;

            if (options.IsValidLocation)
            {
                options.Longitude = options.MapLongitude.ConvertToDouble();
                options.Latitude = options.MapLatitude.ConvertToDouble();
                options.MapLocation = new Point(options.Longitude, options.Latitude) { SRID = 4326 };
            }
            #endregion

            #region Pagination
            if (options.PageSize <= 0)
                options.PageSize = _configuration["DefaultItemsCount"].ConvertToInt();
            if (options.PageCount.HasValue && options.PageCount.Value > 0)
            {
                options.IsValidPageCount = true;
            }
            else
            {
                options.IsValidPageCount = false;
                options.PageCount = default(int?);
            }
            if (options.Page < 1)
                options.Page = 1;
            else if (options.IsValidPageCount && options.PageCount.Value < options.Page)
                options.Page = options.PageCount.Value;
            #endregion

            #region Sorting
            options.IsValidSortOption = true;
            options.SortOptionsId = _jsonDataService.GetSortOptionByIdOrDefault(options.SortOptionsId).Key;
            #endregion

            return new KeyValuePair<bool, string>(errors.Count > 0, string.Join(Path.PathSeparator, errors));
        }
    }
}
