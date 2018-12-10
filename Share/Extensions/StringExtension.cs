using Share.Utilities;
using System;
using System.IO;

namespace Share.Extensions
{
    public static partial class StringExtension
    {
        public static double ConvertToDouble(this string str)
        {
            double result;
            if(double.TryParse(str, out result))
                return result;
            throw new Exception(nameof(ConvertToDouble));
        }

        public static double ConvertToDoubleOrZero(this string str)
        {
            double d = 0;
            double.TryParse(str, out d);
            return d;
        }

        public static bool IsValidDouble(this string str)
        {
            return ConvertToDoubleOrZero(str) > 0.0;
        }

        public static bool IsValidLocation(this string longitude, string latitude)
        {
            if (string.IsNullOrWhiteSpace(longitude) || string.IsNullOrWhiteSpace(latitude))
                return false;

            double longitude1;
            if (double.TryParse(longitude, out longitude1))
            {
                if (longitude1 < -180 || longitude1 > 180)
                    return false;
            }
            else
                return false;

            double latitude1;
            if (double.TryParse(latitude, out latitude1))
            {
                if (latitude1 < -90 || latitude1 > 90)
                    return false;
            }
            else
                return false;
            return true;
        }

        public static int ConvertToInt(this string str)
        {
            int result;
            if (int.TryParse(str, out result))
                return result;
            throw new Exception(nameof(ConvertToInt));
        }

        public static byte ConvertToByte(this string str)
        {
            byte result;
            if (byte.TryParse(str, out result))
                return result;
            throw new Exception(nameof(ConvertToByte));
        }

        public static DateTime ConvertToCacheExpireDateTime(this string CacheExpireDays)
        {
            double inMemoryCacheExpireDays;
            if (double.TryParse(CacheExpireDays, out inMemoryCacheExpireDays))
                return DateTime.UtcNow.AddDays(inMemoryCacheExpireDays);
            throw new Exception(nameof(CacheExpireDays));
        }

        public static short? ConvertToShortOrDefault(this string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                short number;
                if (short.TryParse(str, out number))
                    return number;
            }
            return default(short?);
        }

        public static long? ConvertToLongOrDefault(this string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                long number;
                if (long.TryParse(str, out number))
                    return number;
            }
            return default(long?);
        }

        //var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        //var path = Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location).LocalPath);
        // success in test proj and failed in web proj, Ad: Directory.GetCurrentDirectory()
        public static string ConvertToHappyPath(this string happyPath)
        {
            if (happyPath.Contains(','))
                happyPath = string.Join<string>(Path.DirectorySeparatorChar, happyPath.Split(',', StringSplitOptions.RemoveEmptyEntries));
            return Path.Combine(Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location).LocalPath), happyPath);
        }
    }
}
