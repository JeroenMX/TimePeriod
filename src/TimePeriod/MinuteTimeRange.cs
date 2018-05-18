// -- FILE ------------------------------------------------------------------
// name       : MinuteTimeRange.cs
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

    public abstract class MinuteTimeRange : CalendarTimeRange
    {
        private readonly DateTime _startMinute;
        private readonly DateTime _endMinute; // cache

        protected MinuteTimeRange(int startYear, int startMonth, int startDay, int startHour, int startMinute, int minuteCount)
            : this(startYear, startMonth, startDay, startHour, startMinute, minuteCount, new TimeCalendar())
        { }

        protected MinuteTimeRange(int startYear, int startMonth, int startDay, int startHour, int startMinute, int minuteCount, ITimeCalendar calendar)
            : base(GetPeriodOf(startYear, startMonth, startDay, startHour, startMinute, minuteCount), calendar)
        {
            this._startMinute = new DateTime(startYear, startMonth, startDay, startHour, startMinute, 0);
            this.MinuteCount = minuteCount;
            _endMinute = this._startMinute.AddMinutes(minuteCount);
        }

        public int StartYear => _startMinute.Year;
        public int StartMonth => _startMinute.Month;
        public int StartDay => _startMinute.Day;
        public int StartHour => _startMinute.Hour;
        public int StartMinute => _startMinute.Minute;
        public int EndYear => _endMinute.Year;
        public int EndMonth => _endMinute.Month;
        public int EndDay => _endMinute.Day;
        public int EndHour => _endMinute.Hour;
        public int EndMinute => _endMinute.Minute;
        public int MinuteCount { get; }

        protected override bool IsEqual(object obj)
        {
            return base.IsEqual(obj) && HasSameData(obj as MinuteTimeRange);
        }

        private bool HasSameData(MinuteTimeRange comp)
        {
            return _startMinute == comp._startMinute && MinuteCount == comp.MinuteCount && _endMinute == comp._endMinute;
        }

        protected override int ComputeHashCode()
        {
            return HashTool.ComputeHashCode(base.ComputeHashCode(), _startMinute, MinuteCount, _endMinute);
        }

        private static TimeRange GetPeriodOf(int year, int month, int day, int hour, int minute, int minuteCount)
        {
            if (minuteCount < 1)
            {
                throw new ArgumentOutOfRangeException("minuteCount");
            }
            DateTime start = new DateTime(year, month, day, hour, minute, 0);
            DateTime end = start.AddMinutes(minuteCount);
            return new TimeRange(start, end);
        }
    }
}
