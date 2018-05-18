// -- FILE ------------------------------------------------------------------
// name       : Day.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------
using System;
using TimePeriod.Interfaces;

namespace TimePeriod
{
    public sealed class Day : DayTimeRange
    {
        public Day()
            : this(new TimeCalendar())
        { }
        public Day(DateTime moment)
            : this(moment, new TimeCalendar())
        { }
        public Day(ITimeCalendar calendar)
            : this(ClockProxy.Clock.Now, calendar)
        { }

        public Day(DateTime moment, ITimeCalendar calendar)
             : this(calendar.GetYear(moment), calendar.GetMonth(moment), calendar.GetDayOfMonth(moment), calendar)
        { }

        public Day(int year, int month) 
            : this(year, month, new TimeCalendar())
        { }

        public Day(int year, int month, ITimeCalendar calendar) 
            : this(year, month, 1, calendar)
        { }

        public Day(int year, int month, int day) 
            : this(year, month, day, new TimeCalendar())
        { }

        public Day(int year, int month, int day, ITimeCalendar calendar) 
            : base(year, month, day, 1, calendar)
        { }

        public int Year => StartYear;
        public int Month => StartMonth;
        public int DayValue => StartDay;
        public DayOfWeek DayOfWeek => StartDayOfWeek;
        public string DayName => StartDayName;

        public Day GetPreviousDay()
        {
            return AddDays(-1);
        }
        public Day GetNextDay()
        {
            return AddDays(1);
        }
        public Day AddDays(int days)
        {
            DateTime startDay = new DateTime(StartYear, StartMonth, StartDay);
            return new Day(startDay.AddDays(days), Calendar);
        }
        protected override string Format(ITimeFormatter formatter)
        {
            return formatter.GetCalendarPeriod(DayName,
                formatter.GetShortDate(Start), formatter.GetShortDate(End), Duration);
        }
    }
}
