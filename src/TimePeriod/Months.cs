// -- FILE ------------------------------------------------------------------
// name       : Months.cs
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

    public sealed class Months : MonthTimeRange
    {

        public Months(DateTime moment, YearMonth startMonth, int count)
            : this(moment, startMonth, count, new TimeCalendar())
        { }

        public Months(DateTime moment, YearMonth startMonth, int count, ITimeCalendar calendar)
            : this(calendar.GetYear(moment), startMonth, count, calendar)
        { }

        public Months(int startYear, YearMonth startMonth, int monthCounth)
            : this(startYear, startMonth, monthCounth, new TimeCalendar())
        { }

        public Months(int startYear, YearMonth startMonth, int monthCount, ITimeCalendar calendar) 
            : base(startYear, startMonth, monthCount, calendar)
        { }

        public ITimePeriodCollection GetMonths()
        {
            TimePeriodCollection months = new TimePeriodCollection();
            for (int i = 0; i < MonthCount; i++)
            {
                int year;
                YearMonth month;
                TimeTool.AddMonth(StartYear, StartMonth, i, out year, out month);
                months.Add(new Month(year, month, Calendar));
            }
            return months;
        }

        protected override string Format(ITimeFormatter formatter)
        {
            return formatter.GetCalendarPeriod(StartMonthOfYearName, EndMonthOfYearName,
                formatter.GetShortDate(Start), formatter.GetShortDate(End), Duration);
        }
    }
}
