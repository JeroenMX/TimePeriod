// -- FILE ------------------------------------------------------------------
// name       : Quarter.cs
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

    public sealed class Quarter : QuarterTimeRange
    {

        public Quarter()
             : this(new TimeCalendar())
        { }

        public Quarter(ITimeCalendar calendar)
            : this(ClockProxy.Clock.Now, calendar)
        { }

        public Quarter(DateTime moment)
            : this(moment, new TimeCalendar())
        { }

        public Quarter(DateTime moment, ITimeCalendar calendar)
            : this(TimeTool.GetYearOf(calendar.YearBaseMonth, calendar.GetYear(moment), calendar.GetMonth(moment)),
                TimeTool.GetQuarterOfMonth(calendar.YearBaseMonth, (YearMonth)moment.Month), calendar)
        { }

        public Quarter(int baseYear, YearQuarter yearQuarter)
             : this(baseYear, yearQuarter, new TimeCalendar())
        { }

        public Quarter(int baseYear, YearQuarter yearQuarter, ITimeCalendar calendar) 
            : base(baseYear, yearQuarter, 1, calendar)
        { }

        public int Year
        {
            get
            {
                int year;
                YearMonth month;
                int monthCount = ((int)StartQuarter - 1) * TimeSpec.MonthsPerQuarter;
                TimeTool.AddMonth(BaseYear, Calendar.YearBaseMonth, monthCount, out year, out month);
                return Calendar.GetYear(year, (int)month);
            }
        }

        public YearMonth StartMonth
        {
            get
            {
                int year;
                YearMonth month;
                int monthCount = ((int)StartQuarter - 1) * TimeSpec.MonthsPerQuarter;
                TimeTool.AddMonth(BaseYear, Calendar.YearBaseMonth, monthCount, out year, out month);
                return month;
            }
        }

        public YearQuarter YearQuarter => StartQuarter;
        public string QuarterName => StartQuarterName;
        public string QuarterOfYearName => StartQuarterOfYearName;
        public bool IsCalendarQuarter => ((int)YearBaseMonth - 1) % TimeSpec.MonthsPerQuarter == 0;

        public bool MultipleCalendarYears
        {
            get
            {
                if (IsCalendarQuarter)
                {
                    return false;
                }
                int startYear;
                YearMonth month;
                int monthCount = ((int)StartQuarter - 1) * TimeSpec.MonthsPerQuarter;
                TimeTool.AddMonth(BaseYear, YearBaseMonth, monthCount, out startYear, out month);
                int endYear;
                monthCount += TimeSpec.MonthsPerQuarter;
                TimeTool.AddMonth(BaseYear, YearBaseMonth, monthCount, out endYear, out month);
                return startYear != endYear;
            }
        }

        public Quarter GetPreviousQuarter()
        {
            return AddQuarters(-1);
        }

        public Quarter GetNextQuarter()
        {
            return AddQuarters(1);
        }

        public Quarter AddQuarters(int count)
        {
            int year;
            YearQuarter quarter;
            TimeTool.AddQuarter(BaseYear, StartQuarter, count, out year, out quarter);
            return new Quarter(year, quarter, Calendar);
        }

        protected override string Format(ITimeFormatter formatter)
        {
            return formatter.GetCalendarPeriod(QuarterOfYearName,
                formatter.GetShortDate(Start), formatter.GetShortDate(End), Duration);
        }
    }
}
