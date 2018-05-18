// -- FILE ------------------------------------------------------------------
// name       : CalendarDateDiff.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.09.15
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
    public class CalendarDateDiff
    {
        private readonly CalendarPeriodCollectorFilter _collectorFilter = new CalendarPeriodCollectorFilter();

        public IList<DayOfWeek> WeekDays => _collectorFilter.WeekDays;
        public IList<HourRange> WorkingHours => _collectorFilter.CollectingHours;
        public IList<DayHourRange> WorkingDayHours => _collectorFilter.CollectingDayHours;
        public ITimeCalendar Calendar { get; }
        public IDurationProvider DurationProvider { get; }

        public CalendarDateDiff() 
            : this(new TimeCalendar(new TimeCalendarConfig { EndOffset = TimeSpan.Zero }), new DurationProvider())
        { }

        public CalendarDateDiff(ITimeCalendar calendar, IDurationProvider durationProvider)
        {
            if (calendar == null)
            {
                throw new ArgumentNullException("calendar");
            }
            if (durationProvider == null)
            {
                throw new ArgumentNullException("durationProvider");
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
            this.DurationProvider = durationProvider;
        }

        
        public void AddWorkingWeekDays()
        {
            WeekDays.Add(DayOfWeek.Monday);
            WeekDays.Add(DayOfWeek.Tuesday);
            WeekDays.Add(DayOfWeek.Wednesday);
            WeekDays.Add(DayOfWeek.Thursday);
            WeekDays.Add(DayOfWeek.Friday);
        }


        
        public void AddWeekendWeekDays()
        {
            WeekDays.Add(DayOfWeek.Saturday);
            WeekDays.Add(DayOfWeek.Sunday);
        }


        
        public TimeSpan Difference(DateTime date)
        {
            return Difference(date, ClockProxy.Clock.Now);
        }


        
        public TimeSpan Difference(DateTime date1, DateTime date2)
        {
            if (date1.Equals(date2))
            {
                return TimeSpan.Zero;
            }
            if (_collectorFilter.WeekDays.Count == 0 && _collectorFilter.CollectingHours.Count == 0 && _collectorFilter.CollectingDayHours.Count == 0)
            {
                return new DateDiff(date1, date2, Calendar.Culture.Calendar, Calendar.FirstDayOfWeek, Calendar.YearBaseMonth).Difference;
            }

            TimeRange differenceRange = new TimeRange(date1, date2);

            CalendarPeriodCollector collector = new CalendarPeriodCollector(
                _collectorFilter, new TimeRange(differenceRange.Start.Date, differenceRange.End.Date.AddDays(1)), SeekDirection.Forward, Calendar);
            collector.CollectHours();

            // calculate gaps
            TimeGapCalculator<TimeRange> gapCalculator = new TimeGapCalculator<TimeRange>(Calendar);
            ITimePeriodCollection gaps = gapCalculator.GetGaps(collector.Periods, differenceRange);

            // calculate difference (sum gap durations)
            TimeSpan difference = gaps.GetTotalDuration(DurationProvider);
            return date1 < date2 ? difference : difference.Negate();
        }        
    }
}
