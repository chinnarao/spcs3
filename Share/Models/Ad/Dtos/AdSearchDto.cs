using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GeoAPI.Geometries;

namespace Share.Models.Ad.Dtos
{
    public class AdSearchDto
    {
        #region All Input Parameters
        public string SearchText { get; set; }

        public byte CategoryId { get; set; }
        public byte ConditionId { get; set; }

        public byte SortOptionsId { get; set; }
        public byte MileOptionsId { get; set; }
        
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }

        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }

        public string MapAddress { get; set; } 
        public string MapLongitude { get; set; }
        public string MapLattitude { get; set; }
        #endregion
        
        #region these are all not inputs only inside custom properties
        public bool IsValidCategory { get; set; }
        public bool IsValidCondition { get; set; }
        public bool IsValidMileOption { get; set; }
        public bool IsValidSortOption { get; set; }
        public bool IsValidCountryCode { get; set; }
        public bool IsValidCurrencyCode { get; set; }
        public bool IsValidMinPrice { get; set; }
        public bool IsValidMaxPrice { get; set; }
        public bool IsValidPrice { get; set; }
        public bool IsValidCityName { get; set; }
        public bool IsValidZipCode { get; set; }
        public bool IsValidSearchText { get; set; }
        public double ItemCostMin { get; set; }
        public double ItemCostMax { get; set; }
        //public string SortOptionsFromJsonFile { get; set; }
        //public string MileOptionsNameFromJsonFile { get; set; }
        public IGeometry MapLocation { get; set; }
        public bool IsValidLocation { get; set; }
        public double Miles { get; set; }
        #endregion

        public int PageNumber { get; set; }
        public int DefaultPageSize { get; set; } = 10;//how many records should display in the screen
        public int SearchResultCount { get; private set; }

        public string StateWithComma { get; set; }

        //public Expression<Func<Models.Ad.Entities.Ad, bool>> CreatePredicate()
        //{
        //    var predicate = PredicateBuilder.True<Models.Ad.Entities.Ad>();
        //    if (IsValidCategoryId)
        //        predicate = predicate.And(exp => exp.AdCategoryId == CategoryId);
        //    if (IsValidConditionId)
        //        predicate = predicate.And(exp => exp.ItemConditionId == ConditionId);
        //    if (IsValidCountryCode)
        //        predicate = predicate.And(exp => exp.AddressCountryCode == CountryCode);
        //    if (IsValidCurrencyCode)
        //        predicate = predicate.And(exp => exp.ItemCurrencyCode == CurrencyCode);
        //    if (IsValidCityName)
        //        predicate = predicate.And(exp => exp.AddressCity.ToLower() == CityName);
        //    if (IsValidZipCode)
        //        predicate = predicate.And(exp => exp.AddressZipCode.ToLower() == ZipCode);
        //    if (IsValidMinPrice)
        //        predicate = predicate.And(exp => exp.ItemCost >= MinPrice);
        //    if (IsValidMaxPrice)
        //        predicate = predicate.And(exp => exp.ItemCost <= MaxPrice);
        //    if (IsValidSearchText)
        //        predicate = predicate.And(exp => exp.AdContent.ToLower() == SearchText);
        //    return predicate;
        //}
    }
}
