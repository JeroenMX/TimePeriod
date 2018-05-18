// -- FILE ------------------------------------------------------------------
// name       : Hour.cs
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
    public sealed class Hour : HourTimeRange
    {
        public Hour()
            : this(new TimeCalendar())
        { }

        public Hour(ITimeCalendar calendar)
            : this(ClockProxy.Clock.Now, calendar)
        { }

        public Hour(DateTime moment)
            : this(moment, new TimeCalendar())
        { }

        public Hour(DateTime moment, ITimeCalendar calendar)
            : this(calendar.GetYear(moment), calendar.GetMonth(moment),
                calendar.GetDayOfMonth(moment), calendar.GetHour(moment), calendar)
        { }

        public Hour(int year, int month, int day, int hour)
            : this(year, month, day, hour, new TimeCalendar())
        { }

        public Hour(int year, int month, int day, int hour, ITimeCalendar calendar)
            : base(year, month, day, hour, 1, calendar)
        { }

        public int Year
        {
            get { return StartYear; }
        }

        public int Month
        {
            get { return StartMonth; }
        }

        public int Day
        {
            get { return StartDay; }
        }

        public int HourValue
        {
            get { return StartHour; }
        }

        public Hour GetPreviousHour()
        {
            return AddHours(-1);
        }

        public Hour GetNextHour()
        {
            return AddHours(1);
        }

        public Hour AddHours(int hours)
        {
            DateTime startHour = new DateTime(StartYear, StartMonth, StartDay, StartHour, 0, 0);
            return new Hour(startHour.AddHours(hours), Calendar);
        }

        protected override string Format(ITimeFormatter formatter)
        {
            return formatter.GetCalendarPeriod(formatter.GetShortDate(Start),
                formatter.GetShortTime(Start), formatter.GetShortTime(End), Duration);
        }
    }
}
