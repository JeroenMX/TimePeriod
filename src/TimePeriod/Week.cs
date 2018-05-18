// -- FILE ------------------------------------------------------------------
// name       : Week.cs
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
    public sealed class Week : WeekTimeRange
    {
        public int WeekOfYear => StartWeek;
        public string WeekOfYearName => StartWeekOfYearName;
        public DateTime FirstDayOfWeek => Start;
        public DateTime LastDayOfWeek => FirstDayOfWeek.AddDays(TimeSpec.DaysPerWeek - 1);
        public bool MultipleCalendarYears => FirstDayOfWeek.Year != LastDayOfWeek.Year;

        public Week()
            : this(new TimeCalendar())
        { }

        public Week(ITimeCalendar calendar)
             : this(ClockProxy.Clock.Now, calendar)
        { }

        public Week(DateTime moment)
            : this(moment, new TimeCalendar())
        { }

        public Week(DateTime moment, ITimeCalendar calendar) 
            : base(moment, 1, calendar)
        { }

        public Week(int year, int weekOfYear) 
            : this(year, weekOfYear, new TimeCalendar())
        { }

        public Week(int year, int weekOfYear, ITimeCalendar calendar) 
            : base(year, weekOfYear, 1, calendar)
        { }

        public Week GetPreviousWeek()
        {
            return AddWeeks(-1);
        }

        public Week GetNextWeek()
        {
            return AddWeeks(1);
        }

        public Week AddWeeks(int weeks)
        {
            DateTime startDate = TimeTool.GetStartOfYearWeek(Year, StartWeek, Calendar.Culture, Calendar.YearWeekType);
            return new Week(startDate.AddDays(weeks * TimeSpec.DaysPerWeek), Calendar);
        }

        protected override string Format(ITimeFormatter formatter)
        {
            return formatter.GetCalendarPeriod(WeekOfYearName,
                formatter.GetShortDate(Start), formatter.GetShortDate(End), Duration);
        }    
    }
}
