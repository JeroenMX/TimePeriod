// -- FILE ------------------------------------------------------------------
// name       : TimeFormatter.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Text;
using TimePeriod.Enums;
using TimePeriod.Interfaces;

namespace TimePeriod
{
    public class TimeFormatter : ITimeFormatter
    {
        private static readonly object Mutex = new object();
        private static volatile TimeFormatter _instance;

        public TimeFormatter() :
            this(CultureInfo.CurrentCulture)
        { }

        public TimeFormatter(CultureInfo culture = null,
            string contextSeparator = "; ", string startEndSeparator = " - ",
            string durationSeparator = " | ",
            string dateTimeFormat = null,
            string shortDateFormat = null,
            string longTimeFormat = null,
            string shortTimeFormat = null,
            DurationFormatType durationType = DurationFormatType.Compact,
            bool useDurationSeconds = false,
            bool useIsoIntervalNotation = false,
            string durationItemSeparator = " ",
            string durationLastItemSeparator = " ",
            string durationValueSeparator = " ",
            string intervalStartClosed = "[",
            string intervalStartOpen = "(",
            string intervalStartOpenIso = "]",
            string intervalEndClosed = "]",
            string intervalEndOpen = ")",
            string intervalEndOpenIso = "[")
        {
            if (culture == null)
            {
                culture = CultureInfo.CurrentCulture;
            }
            this.Culture = culture;
            ListSeparator = culture.TextInfo.ListSeparator;
            this.ContextSeparator = contextSeparator;
            this.StartEndSeparator = startEndSeparator;
            this.DurationSeparator = durationSeparator;
            this.DurationItemSeparator = durationItemSeparator;
            this.DurationLastItemSeparator = durationLastItemSeparator;
            this.DurationValueSeparator = durationValueSeparator;
            this.IntervalStartClosed = intervalStartClosed;
            this.IntervalStartOpen = intervalStartOpen;
            this.IntervalStartOpenIso = intervalStartOpenIso;
            this.IntervalEndClosed = intervalEndClosed;
            this.IntervalEndOpen = intervalEndOpen;
            this.IntervalEndOpenIso = intervalEndOpenIso;
            this.DateTimeFormat = dateTimeFormat;
            this.ShortDateFormat = shortDateFormat;
            this.LongTimeFormat = longTimeFormat;
            this.ShortTimeFormat = shortTimeFormat;
            this.DurationType = durationType;
            this.UseDurationSeconds = useDurationSeconds;
            this.UseIsoIntervalNotation = useIsoIntervalNotation;
        }

        public static TimeFormatter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Mutex)
                    {
                        if (_instance == null)
                        {
                            _instance = new TimeFormatter();
                        }
                    }
                }
                return _instance;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                lock (Mutex)
                {
                    _instance = value;
                }
            }
        }

        public CultureInfo Culture { get; }
        public string ListSeparator { get; }
        public string ContextSeparator { get; }

        public string StartEndSeparator { get; }

        public string DurationSeparator { get; }
        public string DurationItemSeparator { get; }
        public string DurationLastItemSeparator { get; }
        public string DurationValueSeparator { get; }
        public string IntervalStartClosed { get; }
        public string IntervalStartOpen { get; }

        public string IntervalStartOpenIso { get; }
        public string IntervalEndClosed { get; }
        public string IntervalEndOpen { get; }
        public string IntervalEndOpenIso { get; }
        public string DateTimeFormat { get; }

        public string ShortDateFormat { get; }

        public string LongTimeFormat { get; }

        public string ShortTimeFormat { get; }

        public DurationFormatType DurationType { get; }
        public bool UseDurationSeconds { get; }
        public bool UseIsoIntervalNotation { get; }

        #region Collection

        public virtual string GetCollection(int count)
        {
            return string.Format("Count = {0}", count);
        }

        public virtual string GetCollectionPeriod(int count, DateTime start, DateTime end, TimeSpan duration)
        {
            return string.Format("{0}{1} {2}", GetCollection(count), ListSeparator, GetPeriod(start, end, duration));
        }

        #endregion

        #region DateTime

        public string GetDateTime(DateTime dateTime)
        {
            return !string.IsNullOrEmpty(DateTimeFormat) ? dateTime.ToString(DateTimeFormat) : dateTime.ToString(Culture);
        }

        public string GetShortDate(DateTime dateTime)
        {
            return !string.IsNullOrEmpty(ShortDateFormat) ? dateTime.ToString(ShortDateFormat) : dateTime.ToString("d");
        }

        public string GetLongTime(DateTime dateTime)
        {
            return !string.IsNullOrEmpty(LongTimeFormat) ? dateTime.ToString(LongTimeFormat) : dateTime.ToString("T");
        }

        public string GetShortTime(DateTime dateTime)
        {
            return !string.IsNullOrEmpty(ShortTimeFormat) ? dateTime.ToString(ShortTimeFormat) : dateTime.ToString("t");
        }

        #endregion

        #region Duration

        public string GetPeriod(DateTime start, DateTime end)
        {
            return GetPeriod(start, end, end - start);
        }

        public string GetDuration(TimeSpan timeSpan)
        {
            return GetDuration(timeSpan, DurationType);
        }

        public string GetDuration(TimeSpan timeSpan, DurationFormatType durationFormatType)
        {
            switch (durationFormatType)
            {
                case DurationFormatType.Detailed:
                    int days = (int)timeSpan.TotalDays;
                    int hours = timeSpan.Hours;
                    int minutes = timeSpan.Minutes;
                    int seconds = UseDurationSeconds ? timeSpan.Seconds : 0;
                    return GetDuration(0, 0, days, hours, minutes, seconds);
                default:
                    return UseDurationSeconds ?
                        string.Format("{0}.{1:00}:{2:00}:{3:00}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds) :
                        string.Format("{0}.{1:00}:{2:00}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes);
            }
        }

        public virtual string GetDuration(int years, int months, int days, int hours, int minutes, int seconds)
        {
            StringBuilder sb = new StringBuilder();

            // years(s)
            if (years != 0)
            {
                sb.Append(years);
                sb.Append(DurationValueSeparator);
                sb.Append(years == 1 ? Strings.TimeSpanYear : Strings.TimeSpanYears);
            }

            // month(s)
            if (months != 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(days == 0 && hours == 0 && minutes == 0 && seconds == 0 ? DurationLastItemSeparator : DurationItemSeparator);
                }
                sb.Append(months);
                sb.Append(DurationValueSeparator);
                sb.Append(months == 1 ? Strings.TimeSpanMonth : Strings.TimeSpanMonths);
            }

            // day(s)
            if (days != 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(hours == 0 && minutes == 0 && seconds == 0 ? DurationLastItemSeparator : DurationItemSeparator);
                }
                sb.Append(days);
                sb.Append(DurationValueSeparator);
                sb.Append(days == 1 ? Strings.TimeSpanDay : Strings.TimeSpanDays);
            }

            // hour(s)
            if (hours != 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(minutes == 0 && seconds == 0 ? DurationLastItemSeparator : DurationItemSeparator);
                }
                sb.Append(hours);
                sb.Append(DurationValueSeparator);
                sb.Append(hours == 1 ? Strings.TimeSpanHour : Strings.TimeSpanHours);
            }

            // minute(s)
            if (minutes != 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(seconds == 0 ? DurationLastItemSeparator : DurationItemSeparator);
                }
                sb.Append(minutes);
                sb.Append(DurationValueSeparator);
                sb.Append(minutes == 1 ? Strings.TimeSpanMinute : Strings.TimeSpanMinutes);
            }

            // second(s)
            if (seconds != 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(DurationLastItemSeparator);
                }
                sb.Append(seconds);
                sb.Append(DurationValueSeparator);
                sb.Append(seconds == 1 ? Strings.TimeSpanSecond : Strings.TimeSpanSeconds);
            }

            return sb.ToString();
        }

        #endregion

        #region Period

        public virtual string GetPeriod(DateTime start, DateTime end, TimeSpan duration)
        {
            if (end < start)
            {
                throw new ArgumentOutOfRangeException("end");
            }

            bool startHasTimeOfDay = TimeTool.HasTimeOfDay(start);

            // no duration - schow start date (optionally with the time part)
            if (duration == TimeSpec.MinPeriodDuration)
            {
                return startHasTimeOfDay ? GetDateTime(start) : GetShortDate(start);
            }

            // within one day: show full start, end time and suration
            if (TimeCompare.IsSameDay(start, end))
            {
                return GetDateTime(start) + StartEndSeparator + GetLongTime(end) + DurationSeparator + GetDuration(duration);
            }

            // show start date, end date and duration (optionally with the time part)
            bool endHasTimeOfDay = TimeTool.HasTimeOfDay(end);
            bool hasTimeOfDays = startHasTimeOfDay || endHasTimeOfDay;
            string startPart = hasTimeOfDays ? GetDateTime(start) : GetShortDate(start);
            string endPart = hasTimeOfDays ? GetDateTime(end) : GetShortDate(end);
            return startPart + StartEndSeparator + endPart + DurationSeparator + GetDuration(duration);
        }

        public string GetCalendarPeriod(string start, string end, TimeSpan duration)
        {
            string timePeriod = start.Equals(end) ? start : start + StartEndSeparator + end;
            return timePeriod + DurationSeparator + GetDuration(duration);
        }

        public string GetCalendarPeriod(string context, string start, string end, TimeSpan duration)
        {
            string timePeriod = start.Equals(end) ? start : start + StartEndSeparator + end;
            return context + ContextSeparator + timePeriod + DurationSeparator + GetDuration(duration);
        }

        public string GetCalendarPeriod(string startContext, string endContext, string start, string end, TimeSpan duration)
        {
            string contextPeriod = startContext.Equals(endContext) ? startContext : startContext + StartEndSeparator + endContext;
            string timePeriod = start.Equals(end) ? start : start + StartEndSeparator + end;
            return contextPeriod + ContextSeparator + timePeriod + DurationSeparator + GetDuration(duration);
        }

        #endregion

        #region Interval

        public string GetInterval(DateTime start, DateTime end,
            IntervalEdge startEdge, IntervalEdge endEdge, TimeSpan duration)
        {
            if (end < start)
            {
                throw new ArgumentOutOfRangeException("end");
            }

            StringBuilder sb = new StringBuilder();

            // interval start
            switch (startEdge)
            {
                case IntervalEdge.Closed:
                    sb.Append(IntervalStartClosed);
                    break;
                case IntervalEdge.Open:
                    sb.Append(UseIsoIntervalNotation ? IntervalStartOpenIso : IntervalStartOpen);
                    break;
            }

            bool addDuration = true;
            bool startHasTimeOfDay = TimeTool.HasTimeOfDay(start);

            // no duration - schow start date (optionally with the time part)
            if (duration == TimeSpec.MinPeriodDuration)
            {
                sb.Append(startHasTimeOfDay ? GetDateTime(start) : GetShortDate(start));
                addDuration = false;
            }
            // within one day: show full start, end time and suration
            else if (TimeCompare.IsSameDay(start, end))
            {
                sb.Append(GetDateTime(start));
                sb.Append(StartEndSeparator);
                sb.Append(GetLongTime(end));
            }
            else
            {
                bool endHasTimeOfDay = TimeTool.HasTimeOfDay(start);
                bool hasTimeOfDays = startHasTimeOfDay || endHasTimeOfDay;
                if (hasTimeOfDays)
                {
                    sb.Append(GetDateTime(start));
                    sb.Append(StartEndSeparator);
                    sb.Append(GetDateTime(end));
                }
                else
                {
                    sb.Append(GetShortDate(start));
                    sb.Append(StartEndSeparator);
                    sb.Append(GetShortDate(end));
                }
            }

            // interval end
            switch (endEdge)
            {
                case IntervalEdge.Closed:
                    sb.Append(IntervalEndClosed);
                    break;
                case IntervalEdge.Open:
                    sb.Append(UseIsoIntervalNotation ? IntervalEndOpenIso : IntervalEndOpen);
                    break;
            }

            // duration
            if (addDuration)
            {
                sb.Append(DurationSeparator);
                sb.Append(GetDuration(duration));
            }

            return sb.ToString();
        }

        #endregion
    }
}

