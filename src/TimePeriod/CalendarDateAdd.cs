// -- FILE ------------------------------------------------------------------
// name       : CalendarDateAdd.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.04.04
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using TimePeriod.Enums;
using TimePeriod.Interfaces;

namespace TimePeriod
{
    public class CalendarDateAdd : DateAdd
    {
        private readonly List<DayOfWeek> _weekDays = new List<DayOfWeek>();
        private readonly List<HourRange> _workingHours = new List<HourRange>();
        private readonly List<DayHourRange> _workingDayHours = new List<DayHourRange>();

        public IList<DayOfWeek> WeekDays => _weekDays;
        public IList<HourRange> WorkingHours => _workingHours;
        public IList<DayHourRange> WorkingDayHours => _workingDayHours;
        public ITimeCalendar Calendar { get; }
        public new ITimePeriodCollection IncludePeriods => throw new NotSupportedException();

        public CalendarDateAdd()
            : this(new TimeCalendar(new TimeCalendarConfig { EndOffset = TimeSpan.Zero }))
        { }

        public CalendarDateAdd(ITimeCalendar calendar)
        {
            if (calendar == null)
            {
                throw new ArgumentNullException("calendar");
            }
            if (calendar.StartOffset != TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("calendar", "start offset");
            }
            if (calendar.EndOffset != TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("calendar", "end offset");
            }
            this.Calendar = calendar;
        }

        public void AddWorkingWeekDays()
        {
            _weekDays.Add(DayOfWeek.Monday);
            _weekDays.Add(DayOfWeek.Tuesday);
            _weekDays.Add(DayOfWeek.Wednesday);
            _weekDays.Add(DayOfWeek.Thursday);
            _weekDays.Add(DayOfWeek.Friday);
        }

        public void AddWeekendWeekDays()
        {
            _weekDays.Add(DayOfWeek.Saturday);
            _weekDays.Add(DayOfWeek.Sunday);
        }

        public override DateTime? Subtract(DateTime start, TimeSpan offset, SeekBoundaryMode seekBoundaryMode = SeekBoundaryMode.Next)
        {
            if (_weekDays.Count == 0 && ExcludePeriods.Count == 0 && _workingHours.Count == 0)
            {
                return start.Subtract(offset);
            }

            return offset < TimeSpan.Zero ?
                CalculateEnd(start, offset.Negate(), SeekDirection.Forward, seekBoundaryMode) :
                CalculateEnd(start, offset, SeekDirection.Backward, seekBoundaryMode);
        }

        public override DateTime? Add(DateTime start, TimeSpan offset, SeekBoundaryMode seekBoundaryMode = SeekBoundaryMode.Next)
        {
            if (_weekDays.Count == 0 && ExcludePeriods.Count == 0 && _workingHours.Count == 0)
            {
                return start.Add(offset);
            }

            return offset < TimeSpan.Zero ?
                CalculateEnd(start, offset.Negate(), SeekDirection.Backward, seekBoundaryMode) :
                CalculateEnd(start, offset, SeekDirection.Forward, seekBoundaryMode);
        }

        protected DateTime? CalculateEnd(DateTime start, TimeSpan offset,
            SeekDirection seekDirection, SeekBoundaryMode seekBoundaryMode)
        {
            if (offset < TimeSpan.Zero)
            {
                throw new InvalidOperationException("time span must be positive");
            }

            DateTime? endDate = null;
            DateTime moment = start;
            TimeSpan? remaining = offset;
            Week week = new Week(start, Calendar);

            // search end date, iteraring week by week
            while (week != null)
            {
                base.IncludePeriods.Clear();
                base.IncludePeriods.AddAll(GetAvailableWeekPeriods(week));

                endDate = CalculateEnd(moment, remaining.Value, seekDirection, seekBoundaryMode, out remaining);
                if (endDate != null || !remaining.HasValue)
                {
                    break;
                }

                switch (seekDirection)
                {
                    case SeekDirection.Forward:
                        week = FindNextWeek(week);
                        if (week != null)
                        {
                            moment = week.Start;
                        }
                        break;
                    case SeekDirection.Backward:
                        week = FindPreviousWeek(week);
                        if (week != null)
                        {
                            moment = week.End;
                        }
                        break;
                }
            }

            return endDate;
        }

        private Week FindNextWeek(Week current)
        {
            if (ExcludePeriods.Count == 0)
            {
                return current.GetNextWeek();
            }

            TimeRange limits = new TimeRange(current.End.AddTicks(1), DateTime.MaxValue);
            TimeGapCalculator<TimeRange> gapCalculator = new TimeGapCalculator<TimeRange>(Calendar);
            ITimePeriodCollection remainingPeriods = gapCalculator.GetGaps(ExcludePeriods, limits);
            return remainingPeriods.Count > 0 ? new Week(remainingPeriods[0].Start) : null;
        }

        private Week FindPreviousWeek(Week current)
        {
            if (ExcludePeriods.Count == 0)
            {
                return current.GetPreviousWeek();
            }

            TimeRange limits = new TimeRange(DateTime.MinValue, current.Start.AddTicks(-1));
            TimeGapCalculator<TimeRange> gapCalculator = new TimeGapCalculator<TimeRange>(Calendar);
            ITimePeriodCollection remainingPeriods = gapCalculator.GetGaps(ExcludePeriods, limits);
            return remainingPeriods.Count > 0 ? new Week(remainingPeriods[remainingPeriods.Count - 1].End) : null;
        }

        protected virtual IEnumerable<ITimePeriod> GetAvailableWeekPeriods(Week week)
        {
            if (_weekDays.Count == 0 && _workingHours.Count == 0 && WorkingDayHours.Count == 0)
            {
                return new TimePeriodCollection { week };
            }

            CalendarPeriodCollectorFilter filter = new CalendarPeriodCollectorFilter();

            // days
            foreach (DayOfWeek weekDay in _weekDays)
            {
                filter.WeekDays.Add(weekDay);
            }

            // hours
            foreach (HourRange workingHour in _workingHours)
            {
                filter.CollectingHours.Add(workingHour);
            }

            // day hours
            foreach (DayHourRange workingDayHour in _workingDayHours)
            {
                filter.CollectingDayHours.Add(workingDayHour);
            }

            CalendarPeriodCollector weekCollector =
                new CalendarPeriodCollector(filter, week, SeekDirection.Forward, Calendar);
            weekCollector.CollectHours();
            return weekCollector.Periods;
        }
    }
}
