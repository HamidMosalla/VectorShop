using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorShop.Helpers
{

    public static class PersianDateTime
    {
        public static string Now()
        {
            PersianCalendar PerCal = new PersianCalendar();

            return string.Format("{0:0000}/{1:00}/{2:00} {3}"
                , PerCal.GetYear(DateTime.Now)
                , PerCal.GetMonth(DateTime.Now)
                , PerCal.GetDayOfMonth(DateTime.Now)
                , DateTime.Now.ToString("hh:mm:ss tt", new CultureInfo("fa-IR")));
        }
        public static string ToShortDateString()
        {
            PersianCalendar PerCal = new PersianCalendar();

            return string.Format("{0:0000}/{1:00}/{2:00}"
                , PerCal.GetYear(DateTime.Now)
                , PerCal.GetMonth(DateTime.Now)
                , PerCal.GetDayOfMonth(DateTime.Now));

        }
        public static string ToLongDateString()
        {
            PersianCalendar PerCal = new PersianCalendar();

            return GetDayOfWeekName() + " - " + PerCal.GetDayOfMonth(DateTime.Now) + " " + GetMonthName() + " - " +
                   PerCal.GetYear(DateTime.Now);
        }
        public static string GetDayOfWeekName()
        {
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return "شنبه";
                case DayOfWeek.Sunday:
                    return "يکشنبه";
                case DayOfWeek.Monday:
                    return "دوشنبه";
                case DayOfWeek.Tuesday:
                    return "سه‏ شنبه";
                case DayOfWeek.Wednesday:
                    return "چهارشنبه";
                case DayOfWeek.Thursday:
                    return "پنجشنبه";
                case DayOfWeek.Friday:
                    return "جمعه";
                default:
                    return "";
            }
        }
        public static string GetDayOfWeekName(DateTime date)
        {
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    return "شنبه";
                case DayOfWeek.Sunday:
                    return "يکشنبه";
                case DayOfWeek.Monday:
                    return "دوشنبه";
                case DayOfWeek.Tuesday:
                    return "سه‏ شنبه";
                case DayOfWeek.Wednesday:
                    return "چهارشنبه";
                case DayOfWeek.Thursday:
                    return "پنجشنبه";
                case DayOfWeek.Friday:
                    return "جمعه";
                default:
                    return "";
            }
        }
        public static string GetMonthName()
        {
            PersianCalendar PerCal = new PersianCalendar();

            switch (PerCal.GetMonth(DateTime.Now))
            {
                case 1:
                    return "فروردین";
                case 2:
                    return "اردیبهشت";
                case 3:
                    return "خرداد";
                case 4:
                    return "تیر‏";
                case 5:
                    return "مرداد";
                case 6:
                    return "شهریور";
                case 7:
                    return "مهر";
                case 8:
                    return "آبان";
                case 9:
                    return "آذر";
                case 10:
                    return "دي";
                case 11:
                    return "بهمن";
                case 12:
                    return "اسفند";
                default:
                    return "";
            }

        }
        public static string GetMonthName(DateTime date)
        {
            PersianCalendar PerCal = new PersianCalendar();

            switch (PerCal.GetMonth(date))
            {
                case 1:
                    return "فروردین";
                case 2:
                    return "اردیبهشت";
                case 3:
                    return "خرداد";
                case 4:
                    return "تیر‏";
                case 5:
                    return "مرداد";
                case 6:
                    return "شهریور";
                case 7:
                    return "مهر";
                case 8:
                    return "آبان";
                case 9:
                    return "آذر";
                case 10:
                    return "دي";
                case 11:
                    return "بهمن";
                case 12:
                    return "اسفند";
                default:
                    return "";
            }

        }
        public static string GregorianDateToPersian(int year, int month, int day)
        {
            DateTime dt = new DateTime(year, month, day);
            PersianCalendar PerCal = new PersianCalendar();

            try
            {
                return string.Format("{0:0000}/{1:00}/{2:00}"
                , PerCal.GetYear(dt)
                , PerCal.GetMonth(dt)
                , PerCal.GetDayOfMonth(dt));
            }
            catch
            {
                throw new FormatException("The input string must be in year/month/day format with digits of 0000/00/00.");
            }
        }
        public static string GregorianDateToPersian(string date)
        {
            DateTime dt = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture);

            PersianCalendar PerCal = new PersianCalendar();

            try
            {
                return string.Format("{0:0000}/{1:00}/{2:00}"
                , PerCal.GetYear(dt)
                , PerCal.GetMonth(dt)
                , PerCal.GetDayOfMonth(dt));
            }
            catch
            {
                throw new FormatException("The input string must be in year/month/day format with digits of 0000/00/00.");
            }
        }
        public static string GregorianDateToPersian(DateTime date)
        {
            var PerCal = new PersianCalendar();

            try
            {
                return string.Format("{0:0000}/{1:00}/{2:00}"
                , PerCal.GetYear(date)
                , PerCal.GetMonth(date)
                , PerCal.GetDayOfMonth(date));
            }
            catch
            {
                throw new FormatException("The input string must be in year/month/day format with digits of 0000/00/00.");
            }
        }
        public static string GregorianDateToPersianLong(DateTime date)
        {
            var PerCal = new PersianCalendar();

            try
            {
                return GetDayOfWeekName(date) + " - " + PerCal.GetDayOfMonth(date) + " " + GetMonthName(date) + " - " +
                   PerCal.GetYear(date);
            }
            catch
            {
                throw new FormatException("The input string must be in year/month/day format with digits of 0000/00/00.");
            }
        }
        public static DateTime PersianDateToGregorian(int year, int month, int day)
        {
            PersianCalendar PerCal = new PersianCalendar();

            try
            {
                DateTime date = PerCal.ToDateTime(year, month, day, 0, 0, 0, 0, PersianCalendar.PersianEra);
                return date;
            }
            catch
            {
                throw new FormatException("The input string must be in year/month/day format with digits of 0000/00/00.");
            }
        }
    }

}
