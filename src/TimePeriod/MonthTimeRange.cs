// -- FILE ------------------------------------------------------------------
// name       : MonthTimeRange.cs
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

    public abstract class MonthTimeRange : CalendarTimeRange
    {
        private readonly int _startYear;
        private readonly int _endYear; // cache
        private readonly YearMonth _endMonth; // cache

        protected MonthTimeRange(int startYear, YearMonth startMonth, int monthCounth)
            : this(startYear, startMonth, monthCounth, new TimeCalendar())
        { }

        protected MonthTimeRange(int startYear, YearMonth startMonth, int monthCount, ITimeCalendar calendar)
            : base(GetPeriodOf(calendar, startYear, startMonth, monthCount), calendar)
        {
            this._startYear = startYear;
            this.StartMonth = startMonth;
            this.MonthCount = monthCount;
            TimeTool.AddMonth(startYear, startMonth, monthCount - 1, out _endYear, out _endMonth);
        }

        public int StartYear => Calendar.GetYear(_startYear, (int)StartMonth);
        public int EndYear => Calendar.GetYear(_endYear, (int)_endMonth);
        public YearMonth StartMonth { get; }

        public YearMonth EndMonth => _endMonth;
        public int MonthCount { get; }

        public string StartMonthName => Calendar.GetMonthName((int)StartMonth);
        public string StartMonthOfYearName => Calendar.GetMonthOfYearName(StartYear, (int)StartMonth);
        public string EndMonthName => Calendar.GetMonthName((int)EndMonth);
        public string EndMonthOfYearName => Calendar.GetMonthOfYearName(EndYear, (int)EndMonth);
        public ITimePeriodCollection GetDays()
        {
            TimePeriodCollection days = new TimePeriodCollection();
            DateTime startDate = GetStartOfMonth(Calendar, _startYear, StartMonth);
            for (int month = 0; month < MonthCount; month++)
            {
                DateTime monthStart = startDate.AddMonths(month);
                int daysOfMonth = TimeTool.GetDaysInMonth(monthStart.Year, monthStart.Month);
                for (int day = 0; day < daysOfMonth; day++)
                {
                    days.Add(new Day(monthStart.AddDays(day), Calendar));
                }
            }
            return days;
        }

        protected override bool IsEqual(object obj)
        {
            return base.IsEqual(obj) && HasSameData(obj as MonthTimeRange);
        }

        private bool HasSameData(MonthTimeRange comp)
        {
            return
                _startYear == comp._startYear &&
                StartMonth == comp.StartMonth &&
                MonthCount == comp.MonthCount &&
                _endYear == comp._endYear &&
                _endMonth == comp._endMonth;
        }

        protected override int ComputeHashCode()
        {
            return HashTool.ComputeHashCode(base.ComputeHashCode(), _startYear, StartMonth, MonthCount, _endYear, _endMonth);
        }

        private static DateTime GetStartOfMonth(ITimeCalendar calendar, int year, YearMonth month)
        {
            DateTime startOfMonth;
            if (calendar.YearType == YearType.FiscalYear)
            {
                startOfMonth = FiscalCalendarTool.GetStartOfMonth(
                    year, month, calendar.YearBaseMonth, calendar.FiscalFirstDayOfYear, calendar.FiscalYearAlignment, calendar.FiscalQuarterGrouping);
            }
            else
            {
                startOfMonth = new DateTime(year, (int)month, 1);
            }
            return startOfMonth;
        }

        private static TimeRange GetPeriodOf(ITimeCalendar calendar, int startYear, YearMonth startMonth, int monthCount)
        {
            if (monthCount < 1)
            {
                throw new ArgumentOutOfRangeException("monthCount");
            }
            DateTime start = GetStartOfMonth(calendar, startYear, startMonth);
            DateTime end;
            if (calendar.YearType == YearType.FiscalYear)
            {
                int endYear;
                YearMonth endMonth;
                TimeTool.AddMonth(startYear, startMonth, monthCount, out endYear, out endMonth);
                end = GetStartOfMonth(calendar, endYear, endMonth);
            }
            else
            {
                end = start.AddMonths(monthCount);
            }
            return new TimeRange(start, end);
        }                
    }
}
