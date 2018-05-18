// -- FILE ------------------------------------------------------------------
// name       : CalendarTimeRange.cs
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

    public class CalendarTimeRange : TimeRange, ICalendarTimeRange
    {
        public CalendarTimeRange(DateTime start, DateTime end)
            : this(start, end, new TimeCalendar())
        { }

        public CalendarTimeRange(DateTime start, DateTime end, ITimeCalendar calendar)
            : this(new TimeRange(start, end), calendar)
        { }

        public CalendarTimeRange(DateTime start, TimeSpan duration)
            : this(start, duration, new TimeCalendar())
        { }

        public CalendarTimeRange(DateTime start, TimeSpan duration, ITimeCalendar calendar)
            : this(new TimeRange(start, duration), calendar)
        { }

        public CalendarTimeRange(ITimePeriod period) 
            : this(period, new TimeCalendar())
        { }

        public CalendarTimeRange(ITimePeriod period, ITimeCalendar calendar) 
            : base(ToCalendarTimeRange(period, calendar), true)
        {
            Calendar = calendar;
        }

        public ITimeCalendar Calendar { get; }

        public YearMonth YearBaseMonth => Calendar.YearBaseMonth;
        public virtual int BaseYear => Start.Year;
        public DateTime FirstMonthStart => new DateTime(Start.Year, Start.Month, 1);
        public DateTime LastMonthStart => new DateTime(End.Year, End.Month, 1);
        public DateTime FirstDayStart => new DateTime(Start.Year, Start.Month, Start.Day);
        public DateTime LastDayStart => new DateTime(End.Year, End.Month, End.Day);
        public DateTime FirstHourStart => new DateTime(Start.Year, Start.Month, Start.Day, Start.Hour, 0, 0);
        public DateTime LastHourStart => new DateTime(End.Year, End.Month, End.Day, End.Hour, 0, 0);
        public DateTime FirstMinuteStart => new DateTime(Start.Year, Start.Month, Start.Day, Start.Hour, Start.Minute, 0, 0);
        public DateTime LastMinuteStart => new DateTime(End.Year, End.Month, End.Day, End.Hour, End.Minute, 0, 0);

        public override ITimeRange Copy(TimeSpan offset)
        {
            throw new NotSupportedException();
        }

        protected override string Format(ITimeFormatter formatter)
        {
            return formatter.GetCalendarPeriod(formatter.GetDateTime(Start), formatter.GetDateTime(End), Duration);
        }

        protected override bool IsEqual(object obj)
        {
            return base.IsEqual(obj) && HasSameData(obj as CalendarTimeRange);
        }

        protected override int ComputeHashCode()
        {
            return HashTool.ComputeHashCode(base.ComputeHashCode(), Calendar);
        }

        private bool HasSameData(CalendarTimeRange comp)
        {
            return Calendar.Equals(comp.Calendar);
        }

        private static TimeRange ToCalendarTimeRange(ITimePeriod period, ITimePeriodMapper mapper)
        {
            DateTime mappedStart = mapper.MapStart(period.Start);
            DateTime mappedEnd = mapper.MapEnd(period.End);
            if (mappedEnd <= mappedStart)
            {
                throw new NotSupportedException();
            }
            return new TimeRange(mappedStart, mappedEnd);
        }
    }
}
