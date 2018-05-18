// -- FILE ------------------------------------------------------------------
// name       : Month.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------
using System;
using TimePeriod.Enums;
using TimePeriod.Interfaces;

namespace TimePeriod
{

    public sealed class Month : MonthTimeRange
    {

        public Month()
            : this(new TimeCalendar())
        { }

        public Month(ITimeCalendar calendar) 
            : this(ClockProxy.Clock.Now, calendar)
        { }

        public Month(DateTime moment) 
            : this(moment, new TimeCalendar())
        { }

        public Month(DateTime moment, ITimeCalendar calendar) 
            : this(calendar.GetYear(moment), (YearMonth)calendar.GetMonth(moment), calendar)
        { }

        public Month(int year, YearMonth yearMonth) 
            : this(year, yearMonth, new TimeCalendar())
        { }

        public Month(int year, YearMonth yearMonth, ITimeCalendar calendar) 
            : base(year, yearMonth, 1, calendar)
        { }

        public int Year => StartYear;
        public YearMonth YearMonth => StartMonth;
        public int MonthValue => (int)StartMonth;
        public string MonthName => StartMonthName;
        public string MonthOfYearName => StartMonthOfYearName;
        public int DaysInMonth => TimeTool.GetDaysInMonth(StartYear, (int)StartMonth);

        public Month GetPreviousMonth()
        {
            return AddMonths(-1);
        }

        public Month GetNextMonth()
        {
            return AddMonths(1);
        }

        public Month AddMonths(int months)
        {
            DateTime startDate = new DateTime(StartYear, (int)StartMonth, 1);
            return new Month(startDate.AddMonths(months), Calendar);
        }

        protected override string Format(ITimeFormatter formatter)
        {
            return formatter.GetCalendarPeriod(MonthOfYearName,
                formatter.GetShortDate(Start), formatter.GetShortDate(End), Duration);
        }
    }
}
