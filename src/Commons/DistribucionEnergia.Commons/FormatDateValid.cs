using System.Globalization;

namespace DistribucionEnergia.Commons
{
    public static class FormatDateValid
    {
        public static bool isFormatDateValid(string date)
        {
            DateTime result;
            return DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }
    }
}
