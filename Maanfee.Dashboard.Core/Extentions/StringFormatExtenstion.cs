using System;
using System.Linq;

namespace Maanfee.Dashboard.Core
{
    public static class StringFormatExtenstion
    {
        public static string ToMoneyString(this object input)
        {
            return string.Format("{0:#,##0.##}", input); 
        }

        public static string ToDateString(this object input)
        {
            return string.Format("{0:yyyy/MM/dd}", input);
        }

        public static string ToTimeString(this object input)
        {
            return string.Format("{0:HH:mm}", input);
        }

        public static double ToCrypto(this decimal input)
        {
            return ((double)Math.Round(input, 8));
        }

        public static double ToCrypto(this double input)
        {
            return ((double)Math.Round(input, 8));
        }

        public static double ToCrypto(this float input)
        {
            return ((double)Math.Round(input, 8));
        }

        public static TimeSpan ToCurrectTime(this int Time)
        {
            try
            {
                string time = Time.ToString();

                if (time.Count() == 3)
                {
                    time = time.Insert(0, "0");
                }

                int Hour = int.Parse(time.Substring(0, 2));
                int Minute = int.Parse(time.Substring(2, 2));

                return new TimeSpan(Hour, Minute, 0);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public static DateTime? ToCurrectDate(this string Date, string Spliter = "/")
        {
            try
            {
                string CurrectDate = Date;
                if (CurrectDate.Count() < 10)
                {
                    var DateArray = CurrectDate.Split(Spliter);
                    string Year = DateArray[0];
                    string Month = DateArray[1];
                    string Day = DateArray[2];
                    if (DateArray[0].Count() < 4)
                    {
                        Year = $"13{DateArray[0]}";
                    }
                    if (DateArray[1].Count() < 2)
                    {
                        Month = $"0{DateArray[1]}";
                    }
                    if (DateArray[2].Count() < 2)
                    {
                        Day = $"0{DateArray[2]}";
                    }
                    CurrectDate = $"{Year}/{Month}/{Day}";
                }

                return Convert.ToDateTime(CurrectDate);
            }
            catch
            {
                return null;
            }
        }

        public static string ToBooleanString(this bool input)
        {
            if(input == true)
            {
                return "دارد";
            }
            else
            {
                return "ندارد";
            }
        }

    }
}
