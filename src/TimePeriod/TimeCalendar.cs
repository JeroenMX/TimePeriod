// -- FILE ------------------------------------------------------------------
// name       : TimeCalendar.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using System.Globalization;
using TimePeriod.Enums;
using TimePeriod.Interfaces;

namespace TimePeriod
{
    public class TimeCalendar : ITimeCalendar
    {
        private readonly CalendarNameType _dayNameType;
        private readonly CalendarNameType _monthNameType;
        public static readonly TimeSpan DefaultStartOffset = TimeSpec.NoDuration;
        public static readonly TimeSpan DefaultEndOffset = TimeSpec.MinNegativeDuration;

        public TimeCalendar()
            : this(new TimeCalendarConfig())
        { }

        public TimeCalendar(TimeCalendarConfig config)
        {
            if (config.StartOffset < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("config");
            }
            if (config.EndOffset > TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("config");
            }

            Culture = config.Culture ?? CultureInfo.CurrentCulture;
            YearType = config.YearType.HasValue ? config.YearType.Value : YearType.SystemYear;
            StartOffset = config.StartOffset.HasValue ? config.StartOffset.Value : DefaultStartOffset;
            EndOffset = config.EndOffset.HasValue ? config.EndOffset.Value : DefaultEndOffset;
            YearBaseMonth = config.YearBaseMonth.HasValue ? config.YearBaseMonth.Value : TimeSpec.CalendarYearStartMonth;
            FiscalYearBaseMonth = config.FiscalYearBaseMonth.HasValue ? config.FiscalYearBaseMonth.Value : TimeSpec.FiscalYearBaseMonth;
            FiscalFirstDayOfYear = config.FiscalFirstDayOfYear.HasValue ? config.FiscalFirstDayOfYear.Value : DayOfWeek.Sunday;
            FiscalYearAlignment = config.FiscalYearAlignment.HasValue ? config.FiscalYearAlignment.Value : FiscalYearAlignment.None;
            FiscalQuarterGrouping = config.FiscalQuarterGrouping.HasValue ? config.FiscalQuarterGrouping.Value : FiscalQuarterGrouping.FourFourFiveWeeks;
            YearWeekType = config.YearWeekType.HasValue ? config.YearWeekType.Value : YearWeekType.Calendar;
            _dayNameType = config.DayNameType.HasValue ? config.DayNameType.Value : CalendarNameType.Full;
            _monthNameType = config.MonthNameType.HasValue ? config.MonthNameType.Value : CalendarNameType.Full;
        }

        public CultureInfo Culture { get; }
        public YearType YearType { get; }
        public TimeSpan StartOffset { get; }

        public TimeSpan EndOffset { get; }

        public YearMonth YearBaseMonth { get; }

        public YearMonth FiscalYearBaseMonth { get; }

        public DayOfWeek FiscalFirstDayOfYear { get; }

        public FiscalYearAlignment FiscalYearAlignment { get; }

        public FiscalQuarterGrouping FiscalQuarterGrouping { get; }

        public virtual DayOfWeek FirstDayOfWeek => Culture.DateTimeFormat.FirstDayOfWeek;
        public YearWeekType YearWeekType { get; }

        public static TimeCalendar New(CultureInfo culture)
        {
            return new TimeCalendar(new TimeCalendarConfig
            {
                Culture = culture
            });
        }

        public static TimeCalendar New(YearMonth yearBaseMonth)
        {
            return new TimeCalendar(new TimeCalendarConfig
            {
                YearBaseMonth = yearBaseMonth
            });
        }

        public static TimeCalendar New(TimeSpan startOffset, TimeSpan endOffset)
        {
            return new TimeCalendar(new TimeCalendarConfig
            {
                StartOffset = startOffset,
                EndOffset = endOffset
            });
        }

        public static TimeCalendar New(TimeSpan startOffset, TimeSpan endOffset, YearMonth yearBaseMonth)
        {
            return new TimeCalendar(new TimeCalendarConfig
            {
                StartOffset = startOffset,
                EndOffset = endOffset,
                YearBaseMonth = yearBaseMonth,
            });
        }

        public static TimeCalendar New(CultureInfo culture, TimeSpan startOffset, TimeSpan endOffset)
        {
            return new TimeCalendar(new TimeCalendarConfig
            {
                Culture = culture,
                StartOffset = startOffset,
                EndOffset = endOffset
            });
        }

        public static TimeCalendar New(CultureInfo culture, YearMonth yearBaseMonth, YearWeekType yearWeekType)
        {
            return new TimeCalendar(new TimeCalendarConfig
            {
                Culture = culture,
                YearBaseMonth = yearBaseMonth,
                YearWeekType = yearWeekType
            });
        }

        public static TimeCalendar NewEmptyOffset()
        {
            return new TimeCalendar(new TimeCalendarConfig
            {
                StartOffset = TimeSpan.Zero,
                EndOffset = TimeSpan.Zero
            });
        }

        public virtual DateTime MapStart(DateTime moment)
        {
            return moment.Add(StartOffset);
        }

        public virtual DateTime MapEnd(DateTime moment)
        {
            return moment.Add(EndOffset);
        }

        public virtual DateTime UnmapStart(DateTime moment)
        {
            return moment.Subtract(StartOffset);
        }

        public virtual DateTime UnmapEnd(DateTime moment)
        {
            return moment.Subtract(EndOffset);
        }

        public virtual int GetYear(DateTime time)
        {
            return Culture.Calendar.GetYear(time);
        }

        public virtual int GetMonth(DateTime time)
        {
            return Culture.Calendar.GetMonth(time);
        }

        public virtual int GetHour(DateTime time)
        {
            return Culture.Calendar.GetHour(time);
        }

        public virtual int GetMinute(DateTime time)
        {
            return Culture.Calendar.GetMinute(time);
        }

        public virtual int GetDayOfMonth(DateTime time)
        {
            return Culture.Calendar.GetDayOfMonth(time);
        }

        public virtual DayOfWeek GetDayOfWeek(DateTime time)
        {
            return Culture.Calendar.GetDayOfWeek(time);
        }

        public virtual int GetDaysInMonth(int year, int month)
        {
            return Culture.Calendar.GetDaysInMonth(year, month);
        }

        public int GetYear(int year, int month)
        {
            if (YearType == YearType.FiscalYear)
            {
                year = FiscalCalendarTool.GetYear(year, (YearMonth)month, YearBaseMonth, FiscalYearBaseMonth);
            }
            return year;
        }

        public virtual string GetYearName(int year)
        {
            switch (YearType)
            {
                case YearType.CalendarYear:
                    return Strings.CalendarYearName(year);
                case YearType.FiscalYear:
                    return Strings.FiscalYearName(year);
                case YearType.SchoolYear:
                    return Strings.SchoolYearName(year);
                default:
                    return Strings.SystemYearName(year);
            }
        }

        public virtual string GetHalfyearName(YearHalfyear yearHalfyear)
        {
            switch (YearType)
            {
                case YearType.CalendarYear:
                    return Strings.CalendarHalfyearName(yearHalfyear);
                case YearType.FiscalYear:
                    return Strings.FiscalHalfyearName(yearHalfyear);
                case YearType.SchoolYear:
                    return Strings.SchoolHalfyearName(yearHalfyear);
                default:
                    return Strings.SystemHalfyearName(yearHalfyear);
            }
        }

        public virtual string GetHalfyearOfYearName(int year, YearHalfyear yearHalfyear)
        {
            switch (YearType)
            {
                case YearType.CalendarYear:
                    return Strings.CalendarHalfyearOfYearName(yearHalfyear, year);
                case YearType.FiscalYear:
                    return Strings.FiscalHalfyearOfYearName(yearHalfyear, year);
                case YearType.SchoolYear:
                    return Strings.SchoolHalfyearOfYearName(yearHalfyear, year);
                default:
                    return Strings.SystemHalfyearOfYearName(yearHalfyear, year);
            }
        }

        public virtual string GetQuarterName(YearQuarter yearQuarter)
        {
            switch (YearType)
            {
                case YearType.CalendarYear:
                    return Strings.CalendarQuarterName(yearQuarter);
                case YearType.FiscalYear:
                    return Strings.FiscalQuarterName(yearQuarter);
                case YearType.SchoolYear:
                    return Strings.SchoolQuarterName(yearQuarter);
                default:
                    return Strings.SystemQuarterName(yearQuarter);
            }
        }

        public virtual string GetQuarterOfYearName(int year, YearQuarter yearQuarter)
        {
            switch (YearType)
            {
                case YearType.CalendarYear:
                    return Strings.CalendarQuarterOfYearName(yearQuarter, year);
                case YearType.FiscalYear:
                    return Strings.FiscalQuarterOfYearName(yearQuarter, year);
                case YearType.SchoolYear:
                    return Strings.SchoolQuarterOfYearName(yearQuarter, year);
                default:
                    return Strings.SystemQuarterOfYearName(yearQuarter, year);
            }
        }

        public virtual string GetMonthName(int month)
        {
            switch (_monthNameType)
            {
                case CalendarNameType.Abbreviated:
                    return Culture.DateTimeFormat.GetAbbreviatedMonthName(month);
                default:
                    return Culture.DateTimeFormat.GetMonthName(month);
            }
        }

        public virtual string GetMonthOfYearName(int year, int month)
        {
            return Strings.MonthOfYearName(GetMonthName(month), GetYearName(year));
        }

        public virtual string GetWeekOfYearName(int year, int weekOfYear)
        {
            return Strings.WeekOfYearName(weekOfYear, GetYearName(year));
        }

        public virtual string GetDayName(DayOfWeek dayOfWeek)
        {
            switch (_dayNameType)
            {
                case CalendarNameType.Abbreviated:
                    return Culture.DateTimeFormat.GetAbbreviatedDayName(dayOfWeek);
                default:
                    return Culture.DateTimeFormat.GetDayName(dayOfWeek);
            }
        }

        public virtual int GetWeekOfYear(DateTime time)
        {
            int year;
            int weekOfYear;
            TimeTool.GetWeekOfYear(time, Culture, YearWeekType, out year, out weekOfYear);
            return weekOfYear;
        }
        
        public virtual DateTime GetStartOfYearWeek(int year, int weekOfYear)
        {
            return TimeTool.GetStartOfYearWeek(year, weekOfYear, Culture, YearWeekType);
        }
        
        public sealed override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return IsEqual(obj);
        }
        
        protected virtual bool IsEqual(object obj)
        {
            return HasSameData(obj as TimeCalendar);
        }
        
        private bool HasSameData(TimeCalendar comp)
        {
            return Culture.Equals(comp.Culture) &&
                StartOffset == comp.StartOffset &&
                EndOffset == comp.EndOffset &&
                YearBaseMonth == comp.YearBaseMonth &&
                FiscalYearBaseMonth == comp.FiscalYearBaseMonth &&
                YearWeekType == comp.YearWeekType &&
                _dayNameType == comp._dayNameType &&
                _monthNameType == comp._monthNameType;
        }       

        public sealed override int GetHashCode()
        {
            return HashTool.AddHashCode(GetType().GetHashCode(), ComputeHashCode());
        }
        
        protected virtual int ComputeHashCode()
        {
            return HashTool.ComputeHashCode(
                Culture,
                StartOffset,
                EndOffset,
                YearBaseMonth,
                FiscalYearBaseMonth,
                FiscalFirstDayOfYear,
                FiscalYearAlignment,
                FiscalQuarterGrouping,
                YearWeekType,
                _dayNameType,
                _monthNameType);
        }
    }
}
