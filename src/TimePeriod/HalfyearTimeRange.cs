// -- FILE ------------------------------------------------------------------
// name       : HalfyearTimeRange.cs
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
    public abstract class HalfyearTimeRange : CalendarTimeRange
    {

        protected HalfyearTimeRange(int startYear, YearHalfyear startHalfyear, int halfyearCount, ITimeCalendar calendar)
            : base(GetPeriodOf(calendar, startYear, startHalfyear, halfyearCount), calendar)
        {
            this.startYear = startYear;
            this.startHalfyear = startHalfyear;
            this.halfyearCount = halfyearCount;
            TimeTool.AddHalfyear(startYear, startHalfyear, halfyearCount - 1, out endYear, out endHalfyear);
        }

        public override int BaseYear
        {
            get { return startYear; }
        }

        public int StartYear
        {
            get { return Calendar.GetYear(startYear, (int)Calendar.YearBaseMonth); }
        }

        public int EndYear
        {
            get { return Calendar.GetYear(endYear, (int)Calendar.YearBaseMonth); }
        }

        public YearHalfyear StartHalfyear
        {
            get { return startHalfyear; }
        }

        public YearHalfyear EndHalfyear
        {
            get { return endHalfyear; }
        }

        public int HalfyearCount
        {
            get { return halfyearCount; }
        }

        public string StartHalfyearName
        {
            get { return Calendar.GetHalfyearName(StartHalfyear); }
        }

        public string StartHalfyearOfYearName
        {
            get { return Calendar.GetHalfyearOfYearName(StartYear, StartHalfyear); }
        }

        public string EndHalfyearName
        {
            get { return Calendar.GetHalfyearName(EndHalfyear); }
        }

        public string EndHalfyearOfYearName
        {
            get { return Calendar.GetHalfyearOfYearName(EndYear, EndHalfyear); }
        }

        public ITimePeriodCollection GetQuarters()
        {
            TimePeriodCollection quarters = new TimePeriodCollection();
            YearQuarter startQuarter = StartHalfyear == YearHalfyear.First ? YearQuarter.First : YearQuarter.Third;
            for (int i = 0; i < halfyearCount; i++)
            {
                for (int quarter = 0; quarter < TimeSpec.QuartersPerHalfyear; quarter++)
                {
                    int year;
                    YearQuarter yearQuarter;
                    TimeTool.AddQuarter(startYear, startQuarter, (i * TimeSpec.QuartersPerHalfyear) + quarter, out year, out yearQuarter);
                    quarters.Add(new Quarter(year, yearQuarter, Calendar));
                }
            }
            return quarters;
        }

        public ITimePeriodCollection GetMonths()
        {
            TimePeriodCollection months = new TimePeriodCollection();
            YearMonth startMonth = YearBaseMonth;
            if (StartHalfyear == YearHalfyear.Second)
            {
                int year;
                TimeTool.AddMonth(startYear, startMonth, TimeSpec.MonthsPerHalfyear, out year, out startMonth);
            }
            for (int i = 0; i < halfyearCount; i++)
            {
                for (int month = 0; month < TimeSpec.MonthsPerHalfyear; month++)
                {
                    int year;
                    YearMonth yearMonth;
                    TimeTool.AddMonth(startYear, startMonth, (i * TimeSpec.MonthsPerHalfyear) + month, out year, out yearMonth);
                    months.Add(new Month(year, yearMonth, Calendar));
                }
            }
            return months;
        }

        protected override bool IsEqual(object obj)
        {
            return base.IsEqual(obj) && HasSameData(obj as HalfyearTimeRange);
        }

        private bool HasSameData(HalfyearTimeRange comp)
        {
            return
                startYear == comp.startYear &&
                startHalfyear == comp.startHalfyear &&
                halfyearCount == comp.halfyearCount &&
                endYear == comp.endYear &&
                endHalfyear == comp.endHalfyear;
        }

        protected override int ComputeHashCode()
        {
            return HashTool.ComputeHashCode(base.ComputeHashCode(), startYear, startHalfyear, halfyearCount, endYear, endHalfyear);
        }

        private static DateTime GetStartOfHalfyear(ITimeCalendar calendar, int year, YearHalfyear halfyear)
        {
            DateTime startOfHalfyear;
            switch (calendar.YearType)
            {
                case YearType.FiscalYear:
                    startOfHalfyear = FiscalCalendarTool.GetStartOfHalfyear(year, halfyear,
                        calendar.YearBaseMonth, calendar.FiscalFirstDayOfYear, calendar.FiscalYearAlignment);
                    break;
                default:
                    DateTime yearStart = new DateTime(year, (int)calendar.YearBaseMonth, 1);
                    startOfHalfyear = yearStart.AddMonths(((int)halfyear - 1) * TimeSpec.MonthsPerHalfyear);
                    break;
            }
            return startOfHalfyear;
        }

        private static TimeRange GetPeriodOf(ITimeCalendar calendar, int startYear, YearHalfyear startHalfyear, int halfyearCount)
        {
            if (halfyearCount < 1)
            {
                throw new ArgumentOutOfRangeException("halfyearCount");
            }
            DateTime start = GetStartOfHalfyear(calendar, startYear, startHalfyear);
            int endYear;
            YearHalfyear endHalfyear;
            TimeTool.AddHalfyear(startYear, startHalfyear, halfyearCount, out endYear, out endHalfyear);
            DateTime end = GetStartOfHalfyear(calendar, endYear, endHalfyear);
            return new TimeRange(start, end);
        }

        // members
        private readonly int startYear;
        private readonly YearHalfyear startHalfyear;
        private readonly int halfyearCount;
        private readonly int endYear; // cache
        private readonly YearHalfyear endHalfyear; // cache
    }
}
// -- EOF -------------------------------------------------------------------
