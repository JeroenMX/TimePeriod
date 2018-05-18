// -- FILE ------------------------------------------------------------------
// name       : DateDiff.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.03.19
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using System.Globalization;
using TimePeriod.Enums;
using TimePeriod.Interfaces;

namespace TimePeriod
{

    public sealed class DateDiff
    {
        // cached values
        private int? _years;
        private int? _quarters;
        private int? _months;
        private int? _weeks;
        private int? _elapsedYears;
        private int? _elapsedMonths;
        private int? _elapsedDays;
        private int? _elapsedHours;
        private int? _elapsedMinutes;
        private int? _elapsedSeconds;

        public DateDiff(DateTime date)
            : this(date, SafeCurrentInfo.Calendar, SafeCurrentInfo.FirstDayOfWeek)
        { }

        public DateDiff(DateTime date, Calendar calendar, DayOfWeek firstDayOfWeek,
            YearMonth yearBaseMonth = TimeSpec.CalendarYearStartMonth)
            : this(date, ClockProxy.Clock.Now, calendar, firstDayOfWeek, yearBaseMonth)
        { }

        public DateDiff(DateTime date1, DateTime date2)
            : this(date1, date2, SafeCurrentInfo.Calendar, SafeCurrentInfo.FirstDayOfWeek)
        { }

        public DateDiff(DateTime date1, DateTime date2, Calendar calendar,
            DayOfWeek firstDayOfWeek, YearMonth yearBaseMonth = TimeSpec.CalendarYearStartMonth)
        {
            if (calendar == null)
            {
                throw new ArgumentNullException("calendar");
            }
            this.Calendar = calendar;
            this.YearBaseMonth = yearBaseMonth;
            this.FirstDayOfWeek = firstDayOfWeek;
            this.Date1 = date1;
            this.Date2 = date2;
            Difference = date2.Subtract(date1);
        }

        public DateDiff(TimeSpan difference)
            : this(ClockProxy.Clock.Now, difference, SafeCurrentInfo.Calendar, SafeCurrentInfo.FirstDayOfWeek)
        { }

        public DateDiff(TimeSpan difference, Calendar calendar,
                DayOfWeek firstDayOfWeek, YearMonth yearBaseMonth = TimeSpec.CalendarYearStartMonth) :
            this(ClockProxy.Clock.Now, difference, calendar, firstDayOfWeek, yearBaseMonth)
        { }

        public DateDiff(DateTime date1, TimeSpan difference) :
            this(date1, difference, SafeCurrentInfo.Calendar, SafeCurrentInfo.FirstDayOfWeek)
        { }

        public DateDiff(DateTime date1, TimeSpan difference, Calendar calendar,
                DayOfWeek firstDayOfWeek, YearMonth yearBaseMonth = TimeSpec.CalendarYearStartMonth)
        {
            if (calendar == null)
            {
                throw new ArgumentNullException("calendar");
            }
            this.Calendar = calendar;
            this.YearBaseMonth = yearBaseMonth;
            this.FirstDayOfWeek = firstDayOfWeek;
            this.Date1 = date1;
            Date2 = date1.Add(difference);
            this.Difference = difference;
        }

        public static DateTimeFormatInfo SafeCurrentInfo => (DateTimeFormatInfo.CurrentInfo ?? DateTimeFormatInfo.InvariantInfo);
        public Calendar Calendar { get; }

        public YearMonth YearBaseMonth { get; }

        public DayOfWeek FirstDayOfWeek { get; }

        public DateTime Date1 { get; }

        public DateTime Date2 { get; }

        public TimeSpan Difference { get; }

        public bool IsEmpty => Difference == TimeSpan.Zero;
        private int Year1 => Calendar.GetYear(Date1);
        private int Year2 => Calendar.GetYear(Date2);

        public int Years
        {
            get
            {
                if (!_years.HasValue)
                {
                    _years = CalcYears();
                }
                return _years.Value;
            }
        }

        public int ElapsedYears
        {
            get
            {
                if (!_elapsedYears.HasValue)
                {
                    _elapsedYears = Years;
                }
                return _elapsedYears.Value;
            }
        }

        public int Quarters
        {
            get
            {
                if (!_quarters.HasValue)
                {
                    _quarters = CalcQuarters();
                }
                return _quarters.Value;
            }
        }

        private int Month1 => Calendar.GetMonth(Date1);
        private int Month2 => Calendar.GetMonth(Date2);

        public int Months
        {
            get
            {
                if (!_months.HasValue)
                {
                    _months = CalcMonths();
                }
                return _months.Value;
            }
        }
        
        public int ElapsedMonths
        {
            get
            {
                if (!_elapsedMonths.HasValue)
                {
                    _elapsedMonths = Months - (ElapsedYears * TimeSpec.MonthsPerYear);
                }
                return _elapsedMonths.Value;
            }
        }

        public int Weeks
        {
            get
            {
                if (!_weeks.HasValue)
                {
                    _weeks = CalcWeeks();
                }
                return _weeks.Value;
            }
        }

        public int Days => (int)Math.Round(Round(Difference.TotalDays));
        public int Weekdays => ((int)Math.Round(Round(Difference.TotalDays))) / TimeSpec.DaysPerWeek;

        public int ElapsedDays
        {
            get
            {
                if (!_elapsedDays.HasValue)
                {
                    DateTime compareDate = Date1.AddYears(ElapsedYears).AddMonths(ElapsedMonths);
                    _elapsedDays = (int)Date2.Subtract(compareDate).TotalDays;
                }
                return _elapsedDays.Value;
            }
        }

        public int Hours => (int)Math.Round(Round(Difference.TotalHours));
        public int ElapsedHours
        {
            get
            {
                if (!_elapsedHours.HasValue)
                {
                    DateTime compareDate = Date1.AddYears(ElapsedYears).AddMonths(ElapsedMonths).AddDays(ElapsedDays);
                    _elapsedHours = (int)Date2.Subtract(compareDate).TotalHours;
                }
                return _elapsedHours.Value;
            }
        }
        
        public int Minutes => (int)Math.Round(Round(Difference.TotalMinutes));
        
        public int ElapsedMinutes
        {
            get
            {
                if (!_elapsedMinutes.HasValue)
                {
                    DateTime compareDate = Date1.AddYears(
                        ElapsedYears).AddMonths(ElapsedMonths).AddDays(ElapsedDays).AddHours(ElapsedHours);
                    _elapsedMinutes = (int)Date2.Subtract(compareDate).TotalMinutes;
                }
                return _elapsedMinutes.Value;
            }
        }
        
        public int Seconds => (int)Math.Round(Round(Difference.TotalSeconds));
        
        public int ElapsedSeconds
        {
            get
            {
                if (!_elapsedSeconds.HasValue)
                {
                    DateTime compareDate = Date1.AddYears(
                        ElapsedYears).AddMonths(
                        ElapsedMonths).AddDays(
                        ElapsedDays).AddHours(
                        ElapsedHours).AddMinutes(
                        ElapsedMinutes);
                    _elapsedSeconds = (int)Date2.Subtract(compareDate).TotalSeconds;
                }
                return _elapsedSeconds.Value;
            }
        }
        
        public string GetDescription(int precision = int.MaxValue, ITimeFormatter formatter = null)
        {
            if (precision < 1)
            {
                throw new ArgumentOutOfRangeException("precision");
            }

            formatter = formatter ?? TimeFormatter.Instance;

            int[] elapsedItems = new int[6];
            elapsedItems[0] = ElapsedYears;
            elapsedItems[1] = ElapsedMonths;
            elapsedItems[2] = ElapsedDays;
            elapsedItems[3] = ElapsedHours;
            elapsedItems[4] = ElapsedMinutes;
            elapsedItems[5] = ElapsedSeconds;

            if (precision <= elapsedItems.Length - 1)
            {
                for (int i = precision; i < elapsedItems.Length; i++)
                {
                    elapsedItems[i] = 0;
                }
            }

            return formatter.GetDuration(
                elapsedItems[0],
                elapsedItems[1],
                elapsedItems[2],
                elapsedItems[3],
                elapsedItems[4],
                elapsedItems[5]);
        }

        public override string ToString()
        {
            return GetDescription();
        }
        
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            DateDiff comp = (DateDiff)obj;
            return Calendar == comp.Calendar &&
                YearBaseMonth == comp.YearBaseMonth &&
                FirstDayOfWeek == comp.FirstDayOfWeek &&
                Date1 == comp.Date1 &&
                Date2 == comp.Date2 &&
                Difference == comp.Difference;
        }

        public override int GetHashCode()
        {
            return HashTool.ComputeHashCode(GetType().GetHashCode(),
                Calendar,
                YearBaseMonth,
                FirstDayOfWeek,
                Date1,
                Date2,
                Difference);
        }

        private static double Round(double number)
        {
            if (number >= 0.0)
            {
                return Math.Floor(number);
            }
            return -Math.Floor(-number);
        }

        private int CalcYears()
        {
            if (TimeCompare.IsSameMonth(Date1, Date2))
            {
                return 0;
            }

            int compareDay = Date2.Day;
            int compareDaysPerMonth = Calendar.GetDaysInMonth(Year1, Month2);
            if (compareDay > compareDaysPerMonth)
            {
                compareDay = compareDaysPerMonth;
            }
            DateTime compareDate = new DateTime(Year1, Month2, compareDay,
                Date2.Hour, Date2.Minute, Date2.Second, Date2.Millisecond);
            if (Date2 > Date1)
            {
                if (compareDate < Date1)
                {
                    compareDate = compareDate.AddYears(1);
                }
            }
            else
            {
                if (compareDate > Date1)
                {
                    compareDate = compareDate.AddYears(-1);
                }
            }
            return Year2 - Calendar.GetYear(compareDate);
        }

        private int CalcQuarters()
        {
            if (TimeCompare.IsSameMonth(Date1, Date2))
            {
                return 0;
            }

            int year1 = TimeTool.GetYearOf(YearBaseMonth, Year1, Month1);
            YearQuarter quarter1 = TimeTool.GetQuarterOfMonth(YearBaseMonth, (YearMonth)Month1);

            int year2 = TimeTool.GetYearOf(YearBaseMonth, Year2, Month2);
            YearQuarter quarter2 = TimeTool.GetQuarterOfMonth(YearBaseMonth, (YearMonth)Month2);

            return
                ((year2 * TimeSpec.QuartersPerYear) + quarter2) -
                ((year1 * TimeSpec.QuartersPerYear) + quarter1);
        }

        private int CalcMonths()
        {
            if (TimeCompare.IsSameDay(Date1, Date2))
            {
                return 0;
            }

            int compareDay = Date2.Day;
            int compareDaysPerMonth = Calendar.GetDaysInMonth(Year1, Month1);
            if (compareDay > compareDaysPerMonth)
            {
                compareDay = compareDaysPerMonth;
            }

            DateTime compareDate = new DateTime(Year1, Month1, compareDay,
                Date2.Hour, Date2.Minute, Date2.Second, Date2.Millisecond);
            if (Date2 > Date1)
            {
                if (compareDate < Date1)
                {
                    compareDate = compareDate.AddMonths(1);
                }
            }
            else
            {
                if (compareDate > Date1)
                {
                    compareDate = compareDate.AddMonths(-1);
                }
            }
            return
            ((Year2 * TimeSpec.MonthsPerYear) + Month2) -
            ((Calendar.GetYear(compareDate) * TimeSpec.MonthsPerYear) + Calendar.GetMonth(compareDate));
        }

        private int CalcWeeks()
        {
            if (TimeCompare.IsSameDay(Date1, Date2))
            {
                return 0;
            }

            DateTime week1 = TimeTool.GetStartOfWeek(Date1, FirstDayOfWeek);
            DateTime week2 = TimeTool.GetStartOfWeek(Date2, FirstDayOfWeek);
            if (week1.Equals(week2))
            {
                return 0;
            }

            return (int)(week2.Subtract(week1).TotalDays / TimeSpec.DaysPerWeek);
        }
        
    }
}
