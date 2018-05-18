// -- FILE ------------------------------------------------------------------
// name       : Minute.cs
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

    public sealed class Minute : MinuteTimeRange
    {

        public Minute() 
            : this(new TimeCalendar())
        { }

        public Minute(ITimeCalendar calendar) 
            : this(ClockProxy.Clock.Now, calendar)
        { }

        public Minute(DateTime moment) 
            : this(moment, new TimeCalendar())
        { }

        public Minute(DateTime moment, ITimeCalendar calendar) 
            : this(calendar.GetYear(moment), calendar.GetMonth(moment), calendar.GetDayOfMonth(moment),
            calendar.GetHour(moment), calendar.GetMinute(moment), calendar)
        { }

        public Minute(int year, int month, int day, int hour, int minute) 
            : this(year, month, day, hour, minute, new TimeCalendar())
        { }

        public Minute(int year, int month, int day, int hour, int minute, ITimeCalendar calendar) 
            : base(year, month, day, hour, minute, 1, calendar)
        { }

        public int Year => StartYear;
        public int Month => StartMonth;    
        public int Day => StartDay;
        public int Hour => StartHour;
        public int MinuteValue => StartMinute;

        public Minute GetPreviousMinute()
        {
            return AddMinutes(-1);
        }

        public Minute GetNextMinute()
        {
            return AddMinutes(1);
        }

        public Minute AddMinutes(int minutes)
        {
            DateTime startMinute = new DateTime(StartYear, StartMonth, StartDay, StartHour, StartMinute, 0);
            return new Minute(startMinute.AddMinutes(minutes), Calendar);
        }

        protected override string Format(ITimeFormatter formatter)
        {
            return formatter.GetCalendarPeriod(formatter.GetShortDate(Start),
                formatter.GetShortTime(Start), formatter.GetShortTime(End), Duration);
        }
    }
}
