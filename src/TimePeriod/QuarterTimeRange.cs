// -- FILE ------------------------------------------------------------------
// name       : QuarterTimeRange.cs
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
    public abstract class QuarterTimeRange : CalendarTimeRange
    {
        private readonly int _startYear;
        private readonly int _endYear; // cache
        private readonly YearQuarter _endQuarter; // cache
        protected QuarterTimeRange(int startYear, YearQuarter startQuarter, int quarterCount) 
            : this(startYear, startQuarter, quarterCount, new TimeCalendar())
        { }

        protected QuarterTimeRange(int startYear, YearQuarter startQuarter, int quarterCount, ITimeCalendar calendar) :
            base(GetPeriodOf(calendar, startYear, startQuarter, quarterCount), calendar)
        {
            this._startYear = startYear;
            this.StartQuarter = startQuarter;
            this.QuarterCount = quarterCount;
            TimeTool.AddQuarter(startYear, startQuarter, quarterCount - 1, out _endYear, out _endQuarter);
        }

        public override int BaseYear => _startYear;
        public int StartYear => Calendar.GetYear(_startYear, (int)Calendar.YearBaseMonth);
        public int EndYear => Calendar.GetYear(_endYear, (int)Calendar.YearBaseMonth);
        public YearQuarter StartQuarter { get; }

        public YearQuarter EndQuarter => _endQuarter;
        public int QuarterCount { get; }

        public string StartQuarterName => Calendar.GetQuarterName(StartQuarter);    
        public string StartQuarterOfYearName => Calendar.GetQuarterOfYearName(StartYear, StartQuarter);    
        public string EndQuarterName => Calendar.GetQuarterName(EndQuarter);
        public string EndQuarterOfYearName => Calendar.GetQuarterOfYearName(EndYear, EndQuarter);

        public ITimePeriodCollection GetMonths()
        {
            TimePeriodCollection months = new TimePeriodCollection();
            for (int i = 0; i < QuarterCount; i++)
            {
                for (int month = 0; month < TimeSpec.MonthsPerQuarter; month++)
                {
                    int year;
                    YearMonth yearMonth;
                    TimeTool.AddMonth(_startYear, YearBaseMonth, (i * TimeSpec.MonthsPerQuarter) + month, out year, out yearMonth);
                    months.Add(new Month(year, yearMonth, Calendar));
                }
            }
            return months;
        }

        protected override bool IsEqual(object obj)
        {
            return base.IsEqual(obj) && HasSameData(obj as QuarterTimeRange);
        }

        private bool HasSameData(QuarterTimeRange comp)
        {
            return
                _startYear == comp._startYear &&
                StartQuarter == comp.StartQuarter &&
                QuarterCount == comp.QuarterCount &&
                _endYear == comp._endYear &&
                _endQuarter == comp._endQuarter;
        }

        protected override int ComputeHashCode()
        {
            return HashTool.ComputeHashCode(base.ComputeHashCode(), _startYear, StartQuarter, QuarterCount, _endYear, _endQuarter);
        }

        private static DateTime GetStartOfQuarter(ITimeCalendar calendar, int year, YearQuarter quarter)
        {
            DateTime startOfQuarter;
            switch (calendar.YearType)
            {
                case YearType.FiscalYear:
                    startOfQuarter = FiscalCalendarTool.GetStartOfQuarter(year, quarter,
                        calendar.YearBaseMonth, calendar.FiscalFirstDayOfYear, calendar.FiscalYearAlignment);
                    break;
                default:
                    DateTime yearStart = new DateTime(year, (int)calendar.YearBaseMonth, 1);
                    startOfQuarter = yearStart.AddMonths(((int)quarter - 1) * TimeSpec.MonthsPerQuarter);
                    break;
            }
            return startOfQuarter;
        }

        private static TimeRange GetPeriodOf(ITimeCalendar calendar, int startYear, YearQuarter startQuarter, int quarterCount)
        {
            if (quarterCount < 1)
            {
                throw new ArgumentOutOfRangeException("quarterCount");
            }
            DateTime start = GetStartOfQuarter(calendar, startYear, startQuarter);
            int endYear;
            YearQuarter endQuarter;
            TimeTool.AddQuarter(startYear, startQuarter, quarterCount, out endYear, out endQuarter);
            DateTime end = GetStartOfQuarter(calendar, endYear, endQuarter);
            return new TimeRange(start, end);
        }
    }
}
