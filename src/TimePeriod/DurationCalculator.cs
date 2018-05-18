// -- FILE ------------------------------------------------------------------
// name       : DurationCalculator.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2013.10.27
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2013 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------
using System;
using TimePeriod.Enums;
using TimePeriod.Interfaces;

namespace TimePeriod
{

    public class DurationCalculator
    {
        private readonly CalendarPeriodCollectorFilter _filter = new CalendarPeriodCollectorFilter();
        private readonly TimePeriodCollection _includePeriods = new TimePeriodCollection();
        private readonly TimePeriodCollection _excludePeriods = new TimePeriodCollection();

        public DurationCalculator() 
            : this(new DurationProvider())
        { }

        public DurationCalculator(IDurationProvider durationProvider)
        {
            if (durationProvider == null)
            {
                throw new ArgumentNullException("durationProvider");
            }
            this.DurationProvider = durationProvider;
        }

        public IDurationProvider DurationProvider { get; }

        public ITimePeriodCollection IncludePeriods => _includePeriods;
        public ITimePeriodCollection ExcludePeriods => _excludePeriods;

        public void Hours(Time start, Time end)
        {
            Hours(new HourRange(start, end));
        }

        public void Hours(params HourRange[] hours)
        {
            _filter.CollectingHours.Clear();
            foreach (HourRange hour in hours)
            {
                _filter.CollectingHours.Add(hour);
            }
        }

        public void DayHours(DayOfWeek dayOfWeek, Time start, Time end)
        {
            DayHours(new DayHourRange(dayOfWeek, start, end));
        }

        public void DayHours(params DayHourRange[] dayHours)
        {
            _filter.CollectingDayHours.Clear();
            foreach (DayHourRange dayHour in dayHours)
            {
                _filter.CollectingDayHours.Add(dayHour);
            }
        }

        public void WeekDays(params DayOfWeek[] weekDays)
        {
            _filter.WeekDays.Clear();
            foreach (DayOfWeek weekDay in weekDays)
            {
                _filter.WeekDays.Add(weekDay);
            }
        }

        public void WorkingWeekDays()
        {
            _filter.WeekDays.Clear();
            _filter.AddWorkingWeekDays();
        }

        public void WeekendWeekDays()
        {
            _filter.WeekDays.Clear();
            _filter.AddWeekendWeekDays();
        }

        public TimeSpan CalcDuration(ITimeRange period)
        {
            if (period == null)
            {
                throw new ArgumentNullException("period");
            }
            return DoCalcDuration(period.Start, period.End);
        }

        public TimeSpan CalcDuration(DateTime start, DateTime end)
        {
            return DoCalcDuration(start, end);
        }

        public TimeSpan CalcDayllightDuration(ITimeRange period, TimeZoneInfo timeZone)
        {
            if (period == null)
            {
                throw new ArgumentNullException("period");
            }
            if (timeZone == null)
            {
                timeZone = TimeZoneInfo.Local;
            }
            return DoCalcDuration(period.Start, period.End, timeZone);
        }

        public TimeSpan CalcDayllightDuration(DateTime start, DateTime end, TimeZoneInfo timeZone)
        {
            if (timeZone == null)
            {
                timeZone = TimeZoneInfo.Local;
            }
            return DoCalcDuration(start, end, timeZone);
        }

        protected virtual TimeSpan DoCalcDuration(DateTime start, DateTime end, TimeZoneInfo timeZone = null)
        {
            if (start.Equals(end))
            {
                return TimeSpan.Zero;
            }
            // test range
            TimeRange testRange = new TimeRange(start, end);
            // search range
            DateTime searchStart = new Day(testRange.Start).Start;
            DateTime serachEnd = new Day(testRange.End).GetNextDay().Start;
            TimeRange searchPeriod = new TimeRange(searchStart, serachEnd);
            // exclude periods
            _filter.ExcludePeriods.Clear();
            _filter.ExcludePeriods.AddAll(_excludePeriods);
            // collect hours
            TimeCalendar calendar = new TimeCalendar(new TimeCalendarConfig { EndOffset = TimeSpan.Zero });
            CalendarPeriodCollector collector = new CalendarPeriodCollector(_filter, searchPeriod, SeekDirection.Forward, calendar);
            collector.CollectHours();
            TimeSpan duration = TimeSpan.Zero;
            collector.Periods.AddAll(_includePeriods);
            foreach (ICalendarTimeRange period in collector.Periods)
            {
                // get the intersection of the test-range and the day hours
                ITimePeriod intersection = testRange.GetIntersection(period);
                if (intersection == null)
                {
                    continue;
                }
                duration = duration.Add(DurationProvider.GetDuration(intersection.Start, intersection.End));
            }
            return start < end ? duration : duration.Negate();
        }                
    }
}
