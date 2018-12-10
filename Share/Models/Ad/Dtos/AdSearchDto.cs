using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

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
        public string MapLatitude { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int? PageCount { get; set; }
        public bool IsValidPageCount { get; set; }
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
        public Point MapLocation { get; set; }
        public bool IsValidLocation { get; set; }
        public double Miles { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        #endregion
    }
}
