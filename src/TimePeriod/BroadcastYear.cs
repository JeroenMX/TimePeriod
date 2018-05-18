// -- FILE ------------------------------------------------------------------
// name       : BroadcastYear.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2013.09.27
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2013 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using System.Globalization;
using TimePeriod.Enums;
using TimePeriod.Interfaces;

namespace TimePeriod
{
    public sealed class BroadcastYear : CalendarTimeRange
    {
        public int Year { get; }

        public BroadcastYear()
            : this(new TimeCalendar())
        { }

        public BroadcastYear(ITimeCalendar calendar)
            : this(ClockProxy.Clock.Now, calendar)
        { }

        public BroadcastYear(DateTime moment)
            : this(moment, new TimeCalendar())
        { }

        public BroadcastYear(DateTime moment, ITimeCalendar calendar) 
            : this(GetYearOf(moment), calendar)
        { }

        public BroadcastYear(int year) 
            : this(year, new TimeCalendar())
        { }

        public BroadcastYear(int year, ITimeCalendar calendar) 
            : base(GetPeriodOf(year), calendar)
        {
            Year = year;
        }        

        public ITimePeriodCollection GetWeeks()
        {
            TimePeriodCollection weeks = new TimePeriodCollection();
            int weekCount = BroadcastCalendarTool.GetWeeksOfYear(Year);
            for (int week = 1; week <= weekCount; week++)
            {
                weeks.Add(new BroadcastWeek(Year, week));
            }
            return weeks;
        }

        public ITimePeriodCollection GetMonths()
        {
            TimePeriodCollection months = new TimePeriodCollection();
            for (int month = 1; month <= TimeSpec.MonthsPerYear; month++)
            {
                months.Add(new BroadcastMonth(Year, (YearMonth)month));
            }
            return months;
        }

        public BroadcastYear GetPreviousYear()
        {
            return AddYears(-1);
        }

        public BroadcastYear GetNextYear()
        {
            return AddYears(1);
        }

        public BroadcastYear AddYears(int count)
        {
            return new BroadcastYear(Year + count, Calendar);
        }

        protected override string Format(ITimeFormatter formatter)
        {
            return formatter.GetCalendarPeriod(Year.ToString(CultureInfo.InvariantCulture),
                formatter.GetShortDate(Start), formatter.GetShortDate(End), Duration);
        }

        private static int GetYearOf(DateTime moment)
        {
            int year;
            BroadcastCalendarTool.GetYearOf(moment, out year);
            return year;
        }

        private static ITimeRange GetPeriodOf(int year)
        {
            return new CalendarTimeRange(
                BroadcastCalendarTool.GetStartOfYear(year),
                BroadcastCalendarTool.GetStartOfYear(year + 1));
        }
    }
}
