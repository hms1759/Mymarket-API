using MarketMe.Share.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketMe.Share.Extensions
{
    public static class DateExtensions
    {
        public static DateTime GetDateUtcNow(this DateTime now)
        {
            return DateTime.Now;
        }

        public static DateTime FindNextDate(this DateTime startDate, int interval)
        {
            DateTime today = DateTime.Today;
            do
            {
                startDate = startDate.AddMonths(interval);
            } while (startDate <= today);
            return startDate;
        }

        public static DateTime ToInvariantDateTime(this String value, String format)
        {
            DateTimeFormatInfo dtfi = DateTimeFormatInfo.InvariantInfo;
            var result = DateTime.TryParseExact(value, format, dtfi, DateTimeStyles.None, out DateTime newValue);
            return newValue;
        }

        public static string ToDateString(this DateTime dt, String format)
        {
            return dt.ToString(format, DateTimeFormatInfo.InvariantInfo);
        }

        public static bool TryParseToDateTime(this DateViewModel date, out DateTime dateTime)
        {
            dateTime = new DateTime();
            try
            {
                dateTime = new DateTime(date.Year, date.Month, date.Day);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DateTime ParseToDateTime(this DateViewModel date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        public static DateViewModel ConvertToDateViewModel(this DateTime dateTime)
        {
            return new DateViewModel(dateTime.Year, dateTime.Month, dateTime.Day);
        }
    }
}
