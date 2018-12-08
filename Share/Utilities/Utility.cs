using GeoAPI.Geometries;
using NetTopologySuite;
using System;
using System.Collections.Generic;
using Share.Enums;
using System.Linq;
using System.IO;
using System.Reflection;

namespace Share.Utilities
{
    public class Utility
    {
        public static string GetCacheCode(int index, string CountryCode, string Code, string Name)
        {
            if (index <= 1 || index >= 6 || string.IsNullOrWhiteSpace(CountryCode) || string.IsNullOrWhiteSpace(Code) || string.IsNullOrWhiteSpace(Name))
                return null;
            switch ((TerritoryTypeEnum)index)
            {
                case TerritoryTypeEnum.State:
                    return string.Format("{0}2", CountryCode);
                case TerritoryTypeEnum.CountyOrProvince:
                    return string.Format("{0}3:{1}_{2}", CountryCode, Code, Name);
                case TerritoryTypeEnum.Community:
                    return string.Format("{0}4:{1}_{2}", CountryCode, Code, Name);
                case TerritoryTypeEnum.Place:
                    return string.Format("{0}5:{1}_{2}", CountryCode, Code, Name);
                default:
                    throw new Exception(nameof(GetCacheCode));
            }
        }

        public static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
        {
            {".txt",  "text/plain"},
            {".pdf",  "application/pdf"},
            {".doc",  "application/vnd.ms-word"},
            {".docx", "application/vnd.ms-word"},
            {".xls",  "application/vnd.ms-excel"},
            {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {".png",  "image/png"},
            {".jpg",  "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".gif",  "image/gif"},
            {".csv",  "text/csv"},
            { ".zip", "application/x-rar-compressed"},
            { ".json", " application/json"},
            { ".htm", "text/html"},
            { ".html", "text/html"}
        };
        }

        /// <summary>
        /// Casting from parameter to parameter  {  var first = new { Id = 1, Name = "Bob" }; var second = new { Id = 0, Name = "" }; second = utility.utility.Cast(second, first); }
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="to">future upcoming anonymous object, could be empty</param>
        /// <param name="from">original with data</param>
        /// <returns>anonymous to type object</returns>
        public static T Cast<T>(T to, T from)
        {
            return (T)from;
        }

        //10 million didn’t create a duplicate
        public static string GenerateStringId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        //https://madskristensen.net/blog/generate-unique-strings-and-numbers-in-c/
        public static long GenerateLongId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        public static T Parse<T>(string input)
        {
            return (T)Enum.Parse(typeof(T), input, true);
        }

        //https://www.latlong.net/convert-address-to-lat-long.html
        //public static IPoint CreatePoint(double longitude, double lattitude)
        //{
        //    //var r = new NetTopologySuite.IO.WKTReader { DefaultSRID = 4326, HandleOrdinates = GeoAPI.Geometries.Ordinates.XY };

        //    //Location = LocationManager.GeometryFactory.CreatePoint(new Coordinate(rnd.NextDouble() * 90.0, rnd.NextDouble() * 90.0))
        //    //https://github.com/cryptograch/backend/blob/c9f2666d909f577d9d98d41133b7ab08f1cab6b2/Taxi/Helpers/Location.cs
        //    //Longitude and Latitude [https://docs.microsoft.com/en-us/ef/core/modeling/spatial]
        //    //Coordinates in NTS are in terms of X and Y values. To represent longitude and latitude, use X for longitude and Y for latitude.Note that this is backwards from the latitude, longitude format in which you typically see these values.
        //    // glendale ca, Latitude and longitude coordinates are: 34.142509, -118.255074.
        //    //if (longitude == 0)
        //    //    longitude = -118.255074;
        //    //if (lattitude == 0)
        //    //    lattitude = 34.142509;
        //    // verify later : very imp: https://github.com/Hinaar/KutyApp/blob/4564904bbb4397d66a7375461eb4df337aa8bc58/KutyApp.Services.Environment.Bll/Mapping/KutyAppServiceProfile.cs
        //    return NtsGeometryServices.Instance.CreateGeometryFactory(4326).CreatePoint(new Coordinate(longitude, lattitude));
        //}

        //public static IPoint CreatePoint(string longitude, string latitude)
        //{
        //    double longi;
        //    if (!double.TryParse(longitude, out longi))
        //        longi = 1.0;
        //    double lati;
        //    if (!double.TryParse(longitude, out lati))
        //        lati = 1.0;
        //    return NtsGeometryServices.Instance.CreateGeometryFactory(4326).CreatePoint(new Coordinate(longi, lati));
        //}

        public static IPoint CreatePointOrDefault(string longitude, string latitude)
        {
            Coordinate c = new Coordinate(1.0, 1.0);
            double longi; double lati;
            if (!string.IsNullOrWhiteSpace(longitude) && !string.IsNullOrWhiteSpace(latitude) && double.TryParse(longitude, out longi) && double.TryParse(longitude, out lati))
                c = new Coordinate(longi, lati);
            //Point p = new Point
            return NtsGeometryServices.Instance.CreateGeometryFactory(4326).CreatePoint(c);
        }
        public static IPoint CreatePoint(string longitude, string latitude)
        {
            double longi; double lati;
            if (double.TryParse(longitude, out longi) && double.TryParse(longitude, out lati))
                return NtsGeometryServices.Instance.CreateGeometryFactory(4326).CreatePoint(new Coordinate(longi, lati));
            return null;
        }

        public static bool IsValidCountryCallingCode(int callingCode)
        {
            
            int[] codes = new int[] {   1, 7, 20, 27, 28, 30, 31, 32, 33, 34, 36, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49,
                                        51, 52, 53, 54, 55, 56, 57, 58, 60, 61, 62, 63, 64, 65, 66, 81, 82, 84, 86, 90, 91, 92, 93, 94, 95, 98,
                                        210, 211, 212, 213, 214, 215, 216, 217, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230,
                                        231, 232, 233, 234, 235, 236, 237, 238, 239, 240, 241, 242, 243, 244, 245, 246, 247, 248, 249, 250,
                                        251, 252, 253, 254, 255, 256, 257, 258, 259, 260, 261, 262, 263, 264, 265, 266, 267, 268, 269, 290,
                                        291, 292, 293, 294, 295, 296, 297, 298, 299, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 370,
                                        371, 373, 374, 375, 377, 380, 381, 385, 387, 389, 500, 501, 502, 503, 504, 505, 506, 507, 508, 509, 590,
                                        591, 592, 593, 594, 595, 596, 597, 598, 599, 670, 671, 672, 673, 674, 675, 676, 677, 678, 679, 680,
                                        681, 682, 683, 684, 685, 686, 687, 688, 689, 690, 691, 692, 808, 809, 850, 852, 853, 855, 856,
                                        871, 872, 873, 874, 880, 886, 960, 961, 962, 963, 964, 965, 966, 967, 968, 969, 971, 972, 973, 974, 975, 976, 977, 993, 994, 995 };
            if (callingCode < codes.Min() || callingCode > codes.Max())
                return false;
            return codes.Any(i => i == callingCode);
        }

        public static long? GetLongNumberFromString(string numberString)
        {
            if (!string.IsNullOrWhiteSpace(numberString))
            {
                long number;
                if (long.TryParse(numberString, out number))
                    return number;
            }
            return null;
        }

        public static short? GetShortNumberFromString(string numberString)
        {
            if (!string.IsNullOrWhiteSpace(numberString))
            {
                short number;
                if (short.TryParse(numberString, out number))
                    return number;
            }
            return null;
        }

        public static DateTime GetCacheExpireDateTime(string CacheExpireDays)
        {
            double inMemoryCacheExpireDays;
            if (double.TryParse(CacheExpireDays, out inMemoryCacheExpireDays))
                return DateTime.UtcNow.AddDays(inMemoryCacheExpireDays);
            else
                throw new Exception(nameof(CacheExpireDays));
        }


        //var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        //var path = Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location).LocalPath);
        // success in test proj and failed in web proj, Ad: Directory.GetCurrentDirectory()
        public static string HappyPath(string happyPath)
        {
            if (happyPath.Contains(','))
                happyPath = string.Join<string>(Path.DirectorySeparatorChar, happyPath.Split(',', StringSplitOptions.RemoveEmptyEntries));
            return Path.Combine(Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location).LocalPath), happyPath);
        }

        //-180.0 to 180.0.
        public static bool IsValidLongitude(string longitude)
        {
            if (string.IsNullOrWhiteSpace(longitude))
                return true;
            double l;
            if (double.TryParse(longitude, out l))
            {
                if (l < -180 || l > 180)
                    return false;
                return true;
            }
            else
                return false;
        }

        //-90.0 to 90.0.
        public static bool IsValidLatitude(string latitude)
        {
            if (string.IsNullOrWhiteSpace(latitude))
                return true;
            double l;
            if (double.TryParse(latitude, out l))
            {
                if (l < -90 || l > 90)
                    return false;
                return true;
            }
            else
                return false;
        }

        public static double ConvertToDoubleFromString(string value)
        {
            double d = 0;
            double.TryParse(value, out d);
            return d;
        }
    }
}

