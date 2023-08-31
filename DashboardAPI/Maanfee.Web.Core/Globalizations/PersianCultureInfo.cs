using System;
using System.Collections.Generic;
using System.Globalization;

namespace Maanfee.Web.Core
{
	public class PersianCultureInfo
	{
		#region - Date Converter -  

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

		#endregion

		#region - Date Spec -

		public static IEnumerable<Month> GetMonth()
		{
			return new List<Month>()
			{
				 new (1, "فروردین"),
				 new (2, "اردیبهشت"),
				 new (3, "خرداد"),
				 new (4, "تیر"),
				 new (5, "مرداد"),
				 new (6, "شهریور"),
				 new (7, "مهر"),
				 new (8, "آبان"),
				 new (9, "آذر"),
				 new (10, "دی"),
				 new (11, "بهمن"),
				 new (12, "اسفند"),
		   };
		}

		public record Month(int MonthNumber, string MonthName);

		public static int GetCurrentYear()
		{
			return new PersianCalendar().GetYear(DateTime.Now);
		}

		public static DateTime GetFirstDayOfCurrentYear()
		{
			return PersianToGregorian(CreatePersianDate(GetCurrentYear(), 1, 1));
		}

		public static DateTime GetLastDayOfCurrentYear()
		{
			return GetFirstDayOfCurrentYear().AddYears(1).AddDays(-1);
		}

		public static DateTime GetFirstDayOfYear(int Year)
		{
			return PersianToGregorian(CreatePersianDate(Year, 1, 1));
		}

		public static DateTime GetLastDayOfYear(int Year)
		{
			return PersianToGregorian(CreatePersianDate(Year, 12, GetDaysInMonth(Year, 12)));
		}

		// ***********************************************

		public static DateTime GetFirstDayOfMonth(int Year, int Month)
		{
			return PersianToGregorian(CreatePersianDate(Year, Month, 1));
		}

		public static DateTime GetLastDayOfMonth(int Year, int Month)
		{
			return PersianToGregorian(CreatePersianDate(Year, Month, GetDaysInMonth(Year, Month)));
		}

		// ***********************************************

		public static int GetDaysInMonth(int Year, int Month)
		{
			return new PersianCalendar().GetDaysInMonth(Year, Month, PersianCalendar.PersianEra);
		}

		#endregion

	}
}
