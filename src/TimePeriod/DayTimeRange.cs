// -- FILE ------------------------------------------------------------------
// name       : DayTimeRange.cs
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

    public abstract class DayTimeRange : CalendarTimeRange
    {
        private readonly DateTime _startDay;
        private readonly DateTime _endDay; // cache

        protected DayTimeRange(int startYear, int startMonth, int startDay, int dayCount)
            : this(startYear, startMonth, startDay, dayCount, new TimeCalendar())
        { }

        protected DayTimeRange(int startYear, int startMonth, int startDay, int dayCount, ITimeCalendar calendar) 
            : base(GetPeriodOf(startYear, startMonth, startDay, dayCount), calendar)
        {
            this._startDay = new DateTime(startYear, startMonth, startDay);
            this.DayCount = dayCount;
            _endDay = calendar.MapEnd(this._startDay.AddDays(dayCount));
        }

        public int StartYear => _startDay.Year;

        public int StartMonth => _startDay.Month;

        public int StartDay => _startDay.Day;

        public int EndYear => _endDay.Year;

        public int EndMonth => _endDay.Month;

        public int EndDay => _endDay.Day;

        public int DayCount { get; }

        public DayOfWeek StartDayOfWeek => Calendar.GetDayOfWeek(_startDay);

        public string StartDayName => Calendar.GetDayName(StartDayOfWeek);

        public DayOfWeek EndDayOfWeek => Calendar.GetDayOfWeek(_endDay);

        public string EndDayName => Calendar.GetDayName(EndDayOfWeek);

        public ITimePeriodCollection GetHours()
        {
            TimePeriodCollection hours = new TimePeriodCollection();
            DateTime startDate = _startDay;
            for (int day = 0; day < DayCount; day++)
            {
                DateTime curDay = startDate.AddDays(day);
                for (int hour = 0; hour < TimeSpec.HoursPerDay; hour++)
                {
                    hours.Add(new Hour(curDay.AddHours(hour), Calendar));
                }
            }
            return hours;
        }

        protected override bool IsEqual(object obj)
        {
            return base.IsEqual(obj) && HasSameData(obj as DayTimeRange);
        }

        private bool HasSameData(DayTimeRange comp)
        {
            return
                _startDay == comp._startDay &&
                DayCount == comp.DayCount &&
                _endDay == comp._endDay;
        }

        protected override int ComputeHashCode()
        {
            return HashTool.ComputeHashCode(base.ComputeHashCode(), _startDay, DayCount, _endDay);
        }

        private static TimeRange GetPeriodOf(int year, int month, int day, int dayCount)
        {
            if (dayCount < 1)
            {
                throw new ArgumentOutOfRangeException("dayCount");
            }
            DateTime start = new DateTime(year, month, day);
            DateTime end = start.AddDays(dayCount);
            return new TimeRange(start, end);
        }                
    }
}
