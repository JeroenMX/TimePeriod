// -- FILE ------------------------------------------------------------------
// name       : CalendarVisitor.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.03.21
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using TimePeriod.Enums;
using TimePeriod.Interfaces;

namespace TimePeriod
{
    public abstract class CalendarVisitor<TFilter, TContext>
        where TFilter : class, ICalendarVisitorFilter
        where TContext : class, ICalendarVisitorContext
    {
        protected CalendarVisitor(TFilter filter, ITimePeriod limits,
            SeekDirection seekDirection = SeekDirection.Forward, ITimeCalendar calendar = null)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }
            if (limits == null)
            {
                throw new ArgumentNullException("limits");
            }

            this.Filter = filter;
            this.Limits = limits;
            this.SeekDirection = seekDirection;
            this.Calendar = calendar;
        }

        public TFilter Filter { get; }

        public ITimePeriod Limits { get; }

        public SeekDirection SeekDirection { get; }

        public ITimeCalendar Calendar { get; }

        protected void StartPeriodVisit(TContext context = null)
        {
            StartPeriodVisit(Limits, context);
        }

        protected void StartPeriodVisit(ITimePeriod period, TContext context = null)
        {
            if (period == null)
            {
                throw new ArgumentNullException("period");
            }
            if (period.IsMoment)
            {
                return;
            }

            OnVisitStart();

            Years years = Calendar != null ?
                new Years(period.Start.Year, period.End.Year - period.Start.Year + 1, Calendar) :
                new Years(period.Start.Year, period.End.Year - period.Start.Year + 1);
            if (OnVisitYears(years, context) && EnterYears(years, context))
            {
                ITimePeriodCollection yearsToVisit = years.GetYears();
                if (SeekDirection == SeekDirection.Backward)
                {
                    yearsToVisit.SortByEnd();
                }
                foreach (Year year in yearsToVisit)
                {
                    if (!year.OverlapsWith(period) || OnVisitYear(year, context) == false)
                    {
                        continue;
                    }

                    // months
                    if (EnterMonths(year, context) == false)
                    {
                        continue;
                    }
                    ITimePeriodCollection monthsToVisit = year.GetMonths();
                    if (SeekDirection == SeekDirection.Backward)
                    {
                        monthsToVisit.SortByEnd();
                    }
                    foreach (Month month in monthsToVisit)
                    {
                        if (!month.OverlapsWith(period) || OnVisitMonth(month, context) == false)
                        {
                            continue;
                        }

                        // days
                        if (EnterDays(month, context) == false)
                        {
                            continue;
                        }
                        ITimePeriodCollection daysToVisit = month.GetDays();
                        if (SeekDirection == SeekDirection.Backward)
                        {
                            daysToVisit.SortByEnd();
                        }
                        foreach (Day day in daysToVisit)
                        {
                            if (!day.OverlapsWith(period) || OnVisitDay(day, context) == false)
                            {
                                continue;
                            }

                            // hours
                            if (EnterHours(day, context) == false)
                            {
                                continue;
                            }
                            ITimePeriodCollection hoursToVisit = day.GetHours();
                            if (SeekDirection == SeekDirection.Backward)
                            {
                                hoursToVisit.SortByEnd();
                            }
                            foreach (Hour hour in hoursToVisit)
                            {
                                if (!hour.OverlapsWith(period) || OnVisitHour(hour, context) == false)
                                {
                                    // ReSharper disable RedundantJumpStatement
                                    continue;
                                    // ReSharper restore RedundantJumpStatement
                                }
                            }
                        }
                    }
                }
            }

            OnVisitEnd();
        }

        protected Year StartYearVisit(Year year, TContext context = null, SeekDirection? visitDirection = null)
        {
            if (year == null)
            {
                throw new ArgumentNullException("year");
            }

            if (visitDirection == null)
            {
                visitDirection = SeekDirection;
            }

            OnVisitStart();

            // iteration limits
            Year lastVisited = null;
            DateTime minStart = DateTime.MinValue;
            DateTime maxEnd = DateTime.MaxValue.AddYears(-1);
            while (year.Start > minStart && year.End < maxEnd)
            {
                if (OnVisitYear(year, context) == false)
                {
                    lastVisited = year;
                    break;
                }
                switch (visitDirection)
                {
                    case SeekDirection.Forward:
                        year = year.GetNextYear();
                        break;
                    case SeekDirection.Backward:
                        year = year.GetPreviousYear();
                        break;
                }
            }

            OnVisitEnd();

            return lastVisited;
        }

        protected Month StartMonthVisit(Month month, TContext context = null, SeekDirection? visitDirection = null)
        {
            if (month == null)
            {
                throw new ArgumentNullException("month");
            }

            if (visitDirection == null)
            {
                visitDirection = SeekDirection;
            }

            OnVisitStart();

            // iteration limits
            Month lastVisited = null;
            DateTime minStart = DateTime.MinValue;
            DateTime maxEnd = DateTime.MaxValue.AddMonths(-1);
            while (month.Start > minStart && month.End < maxEnd)
            {
                if (OnVisitMonth(month, context) == false)
                {
                    lastVisited = month;
                    break;
                }
                switch (visitDirection)
                {
                    case SeekDirection.Forward:
                        month = month.GetNextMonth();
                        break;
                    case SeekDirection.Backward:
                        month = month.GetPreviousMonth();
                        break;
                }
            }

            OnVisitEnd();

            return lastVisited;
        }

        protected Day StartDayVisit(Day day, TContext context = null, SeekDirection? visitDirection = null)
        {
            if (day == null)
            {
                throw new ArgumentNullException("day");
            }

            if (visitDirection == null)
            {
                visitDirection = SeekDirection;
            }

            OnVisitStart();

            // iteration limits
            Day lastVisited = null;
            DateTime minStart = DateTime.MinValue;
            DateTime maxEnd = DateTime.MaxValue.AddDays(-1);
            while (day.Start > minStart && day.End < maxEnd)
            {
                if (OnVisitDay(day, context) == false)
                {
                    lastVisited = day;
                    break;
                }
                switch (visitDirection)
                {
                    case SeekDirection.Forward:
                        day = day.GetNextDay();
                        break;
                    case SeekDirection.Backward:
                        day = day.GetPreviousDay();
                        break;
                }
            }

            OnVisitEnd();

            return lastVisited;
        }

        protected Hour StartHourVisit(Hour hour, TContext context = null, SeekDirection? visitDirection = null)
        {
            if (hour == null)
            {
                throw new ArgumentNullException("hour");
            }

            if (visitDirection == null)
            {
                visitDirection = SeekDirection;
            }

            OnVisitStart();

            // iteration limits
            Hour lastVisited = null;
            DateTime minStart = DateTime.MinValue;
            DateTime maxEnd = DateTime.MaxValue.AddHours(-1);
            while (hour.Start > minStart && hour.End < maxEnd)
            {
                if (OnVisitHour(hour, context) == false)
                {
                    lastVisited = hour;
                    break;
                }
                switch (visitDirection)
                {
                    case SeekDirection.Forward:
                        hour = hour.GetNextHour();
                        break;
                    case SeekDirection.Backward:
                        hour = hour.GetPreviousHour();
                        break;
                }
            }

            OnVisitEnd();

            return lastVisited;
        }

        protected virtual void OnVisitStart()
        { }

        protected virtual bool CheckLimits(ITimePeriod test)
        {
            return Limits.HasInside(test);
        }

        protected virtual bool CheckExcludePeriods(ITimePeriod test)
        {
            if (Filter.ExcludePeriods.Count == 0)
            {
                return true;
            }
            return Filter.ExcludePeriods.OverlapPeriods(test).Count == 0;
        }

        protected virtual bool EnterYears(Years years, TContext context)
        {
            return true;
        }

        protected virtual bool EnterMonths(Year year, TContext context)
        {
            return true;
        }

        protected virtual bool EnterDays(Month month, TContext context)
        {
            return true;
        }

        protected virtual bool EnterHours(Day day, TContext context)
        {
            return true;
        }

        protected virtual bool OnVisitYears(Years years, TContext context)
        {
            return true;
        }

        protected virtual bool OnVisitYear(Year year, TContext context)
        {
            return true;
        }

        protected virtual bool OnVisitMonth(Month month, TContext context)
        {
            return true;
        }

        protected virtual bool OnVisitDay(Day day, TContext context)
        {
            return true;
        }

        protected virtual bool OnVisitHour(Hour hour, TContext context)
        {
            return true;
        }

        protected virtual bool IsMatchingYear(Year year, TContext context)
        {
            // year filter
            if (Filter.Years.Count > 0 && !Filter.Years.Contains(year.YearValue))
            {
                return false;
            }

            return CheckExcludePeriods(year);
        }

        protected virtual bool IsMatchingMonth(Month month, TContext context)
        {
            // year filter
            if (Filter.Years.Count > 0 && !Filter.Years.Contains(month.Year))
            {
                return false;
            }

            // month filter
            if (Filter.Months.Count > 0 && !Filter.Months.Contains(month.YearMonth))
            {
                return false;
            }

            return CheckExcludePeriods(month);
        }

        protected virtual bool IsMatchingDay(Day day, TContext context)
        {
            // year filter
            if (Filter.Years.Count > 0 && !Filter.Years.Contains(day.Year))
            {
                return false;
            }

            // month filter
            if (Filter.Months.Count > 0 && !Filter.Months.Contains((YearMonth)day.Month))
            {
                return false;
            }

            // day filter
            if (Filter.Days.Count > 0 && !Filter.Days.Contains(day.DayValue))
            {
                return false;
            }

            // weekday filter
            if (Filter.WeekDays.Count > 0 && !Filter.WeekDays.Contains(day.DayOfWeek))
            {
                return false;
            }

            return CheckExcludePeriods(day);
        }

        protected virtual bool IsMatchingHour(Hour hour, TContext context)
        {
            // year filter
            if (Filter.Years.Count > 0 && !Filter.Years.Contains(hour.Year))
            {
                return false;
            }

            // month filter
            if (Filter.Months.Count > 0 && !Filter.Months.Contains((YearMonth)hour.Month))
            {
                return false;
            }

            // day filter
            if (Filter.Days.Count > 0 && !Filter.Days.Contains(hour.Day))
            {
                return false;
            }

            // weekday filter
            if (Filter.WeekDays.Count > 0 && !Filter.WeekDays.Contains(hour.Start.DayOfWeek))
            {
                return false;
            }

            // hour filter
            if (Filter.Hours.Count > 0 && !Filter.Hours.Contains(hour.HourValue))
            {
                return false;
            }

            return CheckExcludePeriods(hour);
        }

        protected virtual void OnVisitEnd()
        { }
    }
}
