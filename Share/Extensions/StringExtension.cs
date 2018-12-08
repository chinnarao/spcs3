namespace Share.Extensions
{
    public static partial class StringExtension
    {
        public static double StringToDouble(this string str)
        {
            double result;
            double.TryParse(str, out result);
            return result;
        }
    }
}
