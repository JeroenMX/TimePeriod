// -- FILE ------------------------------------------------------------------
// name       : Days.cs
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

    public sealed class Days : DayTimeRange
    {

        public Days(DateTime moment, int count)
            : this(moment, count, new TimeCalendar())
        { }

        public Days(DateTime moment, int count, ITimeCalendar calendar) 
            : this(calendar.GetYear(moment), calendar.GetMonth(moment), calendar.GetDayOfMonth(moment), count, calendar)
        { }

        public Days(int startYear, int startMonth, int startDay, int dayCount) 
            : this(startYear, startMonth, startDay, dayCount, new TimeCalendar())
        { }

        public Days(int startYear, int startMonth, int startDay, int dayCount, ITimeCalendar calendar) 
            : base(startYear, startMonth, startDay, dayCount, calendar)
        { }

        public ITimePeriodCollection GetDays()
        {
            TimePeriodCollection days = new TimePeriodCollection();
            DateTime startDay = new DateTime(StartYear, StartMonth, StartDay);
            for (int i = 0; i < DayCount; i++)
            {
                days.Add(new Day(startDay.AddDays(i), Calendar));
            }
            return days;
        }

        protected override string Format(ITimeFormatter formatter)
        {
            return formatter.GetCalendarPeriod(StartDayName, EndDayName,
                formatter.GetShortDate(Start), formatter.GetShortDate(End), Duration);
        }
    }
}
