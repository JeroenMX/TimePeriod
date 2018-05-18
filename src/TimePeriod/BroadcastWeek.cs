// -- FILE ------------------------------------------------------------------
// name       : BroadcastWeek.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2013.09.27
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2013 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using TimePeriod.Interfaces;

namespace TimePeriod
{
    public sealed class BroadcastWeek : CalendarTimeRange
    {
        public int Week { get; }

        public int Year { get; }


        public BroadcastWeek() 
            : this(new TimeCalendar())
        { }

        public BroadcastWeek(ITimeCalendar calendar) 
            : this(ClockProxy.Clock.Now, calendar)
        { } 

        public BroadcastWeek(DateTime moment) 
            : this(moment, new TimeCalendar())
        { }

        public BroadcastWeek(DateTime moment, ITimeCalendar calendar) 
            : this(GetYearOf(moment), GetWeekOf(moment), calendar)
        { }

        public BroadcastWeek(int year, int week) 
            : this(year, week, new TimeCalendar())
        { }

        public BroadcastWeek(int year, int week, ITimeCalendar calendar) 
            : base(GetPeriodOf(year, week), calendar)
        {
            Year = year;
            Week = week;
        }

        public ITimePeriodCollection GetDays()
        {
            TimePeriodCollection days = new TimePeriodCollection();
            DateTime moment = Start.Date;
            while (moment <= End.Date)
            {
                days.Add(new Day(moment.Date, Calendar));
                moment = moment.AddDays(1);
            }
            return days;
        }

        public BroadcastWeek GetPreviousWeek()
        {
            return AddWeeks(-1);
        }

        public BroadcastWeek GetNextWeek()
        {
            return AddWeeks(1);
        }

        public BroadcastWeek AddWeeks(int weeks)
        {
            return new BroadcastWeek(Start.AddDays(weeks * TimeSpec.DaysPerWeek), Calendar);
        }

        protected override string Format(ITimeFormatter formatter)
        {
            return formatter.GetCalendarPeriod(string.Format("{0}/{1}", Year, Week),
                formatter.GetShortDate(Start), formatter.GetShortDate(End), Duration);
        }

        private static int GetYearOf(DateTime moment)
        {
            int year;
            BroadcastCalendarTool.GetYearOf(moment, out year);
            return year;
        }

        private static int GetWeekOf(DateTime moment)
        {
            int year;
            int week;
            BroadcastCalendarTool.GetWeekOf(moment, out year, out week);
            return week;
        }

        private static ITimeRange GetPeriodOf(int year, int week)
        {
            DateTime start = BroadcastCalendarTool.GetStartOfWeek(year, week);
            return new CalendarTimeRange(start, start.AddDays(TimeSpec.DaysPerWeek));
        }
    }
}
