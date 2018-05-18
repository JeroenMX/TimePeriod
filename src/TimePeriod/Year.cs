// -- FILE ------------------------------------------------------------------
// name       : Year.cs
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
    public sealed class Year : YearTimeRange
    {
        public Year()
            : this(new TimeCalendar())
        { }

        public Year(ITimeCalendar calendar)
            : this(ClockProxy.Clock.Now, calendar)
        { }

        public Year(DateTime moment) 
            : this(moment, new TimeCalendar())
        { }

        public Year(DateTime moment, ITimeCalendar calendar) 
            : this(TimeTool.GetYearOf(calendar.YearBaseMonth, calendar.GetYear(moment), calendar.GetMonth(moment)), calendar)
        { }

        public Year(int year) 
            : this(year, new TimeCalendar())
        { }

        public Year(int year, ITimeCalendar calendar) 
            : base(year, 1, calendar)
        { }

        public int YearValue => Calendar.GetYear(BaseYear, (int)YearBaseMonth);
        public string YearName => StartYearName;
        public bool IsCalendarYear => YearBaseMonth == TimeSpec.CalendarYearStartMonth;    
        public Year GetPreviousYear()
        {
            return AddYears(-1);
        }
        public Year GetNextYear()
        {
            return AddYears(1);
        }
        public Year AddYears(int count)
        {
            DateTime startDate = new DateTime(BaseYear, (int)YearBaseMonth, 1);
            return new Year(startDate.AddYears(count), Calendar);
        }

        protected override string Format(ITimeFormatter formatter)
        {
            return formatter.GetCalendarPeriod(YearName,
                formatter.GetShortDate(Start), formatter.GetShortDate(End), Duration);
        }
    }
}

