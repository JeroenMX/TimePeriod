// -- FILE ------------------------------------------------------------------
// name       : Years.cs
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
    public sealed class Years : YearTimeRange
    {
        public Years(DateTime moment, int count)
            : this(moment, count, new TimeCalendar())
        { }

        public Years(DateTime moment, int count, ITimeCalendar calendar)
            : this(TimeTool.GetYearOf(calendar.YearBaseMonth, calendar.GetYear(moment), calendar.GetMonth(moment)), count, calendar)
        { }

        public Years(int year, int count) 
            : this(year, count, new TimeCalendar())
        {  }

        public Years(int year, int count, ITimeCalendar calendar) 
            : base(year, count, calendar)
        { }

        public ITimePeriodCollection GetYears()
        {
            TimePeriodCollection years = new TimePeriodCollection();
            for (int i = 0; i < YearCount; i++)
            {
                years.Add(new Year(BaseYear + i, Calendar));
            }
            return years;
        }

        protected override string Format(ITimeFormatter formatter)
        {
            return formatter.GetCalendarPeriod(StartYearName, EndYearName,
                formatter.GetShortDate(Start), formatter.GetShortDate(End), Duration);
        }
    }
}
