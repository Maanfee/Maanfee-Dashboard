using System;
using System.Globalization;

namespace Maanfee.Web.Core
{
    public static class DateConverter
    {
        public static DateTime CreatePersianDate(int Year, int Month, int Day)
        {
            return new PersianCalendar().ToDateTime(Year, Month, Day, 0, 0, 0, 0);
        }

        public static DateTime CreateGregorianDate(int Year, int Month, int Day)
        {
            return new DateTime(Year, Month, Day, 0, 0, 0, 0);
        }

        public static DateTime PersianToGregorian(DateTime Date)
        {
            return CreateGregorianDate(Date.Year, Date.Month, Date.Day);
        }

        public static DateTime GregorianToPersian(DateTime Date)
        {
            string StringDate = Date.ToString("yyyy/MM/dd");

            DateTime dt = DateTime.ParseExact(StringDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            PersianCalendar pc = new PersianCalendar();

            return CreatePersianDate(pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt));
        }

    }
}
