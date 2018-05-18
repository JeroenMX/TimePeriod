// -- FILE ------------------------------------------------------------------
// name       : CalendarPeriodCollector.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
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
    public class CalendarPeriodCollector : CalendarVisitor<CalendarPeriodCollectorFilter, CalendarPeriodCollectorContext>
    {
        public CalendarPeriodCollector(CalendarPeriodCollectorFilter filter, ITimePeriod limits,
            SeekDirection seekDirection = SeekDirection.Forward, ITimeCalendar calendar = null)
            : base(filter, limits, seekDirection, calendar)
        { }

        public ITimePeriodCollection Periods { get; } = new TimePeriodCollection();

        public void CollectYears()
        {
            StartPeriodVisit(new CalendarPeriodCollectorContext { Scope = CalendarPeriodCollectorContext.CollectType.Year });
        }

        public void CollectMonths()
        {
            StartPeriodVisit(new CalendarPeriodCollectorContext { Scope = CalendarPeriodCollectorContext.CollectType.Month });
        }

        public void CollectDays()
        {
            StartPeriodVisit(new CalendarPeriodCollectorContext { Scope = CalendarPeriodCollectorContext.CollectType.Day });
        }

        public void CollectHours()
        {
            StartPeriodVisit(new CalendarPeriodCollectorContext { Scope = CalendarPeriodCollectorContext.CollectType.Hour });
        }

        public override string ToString()
        {
            return Periods.ToString();
        }

        protected override bool EnterYears(Years years, CalendarPeriodCollectorContext context)
        {
            return
                context.Scope == CalendarPeriodCollectorContext.CollectType.Month ||
                context.Scope == CalendarPeriodCollectorContext.CollectType.Day ||
                context.Scope == CalendarPeriodCollectorContext.CollectType.Hour;
        }

        protected override bool EnterMonths(Year year, CalendarPeriodCollectorContext context)
        {
            return
                context.Scope == CalendarPeriodCollectorContext.CollectType.Day ||
                context.Scope == CalendarPeriodCollectorContext.CollectType.Hour;
        }

        protected override bool EnterDays(Month month, CalendarPeriodCollectorContext context)
        {
            return
                context.Scope == CalendarPeriodCollectorContext.CollectType.Hour;
        }

        protected override bool EnterHours(Day day, CalendarPeriodCollectorContext context)
        {
            return false;
        }

        protected override bool OnVisitYears(Years years, CalendarPeriodCollectorContext context)
        {
            if (context.Scope != CalendarPeriodCollectorContext.CollectType.Year)
            {
                return true; // continue
            }

            foreach (Year year in years.GetYears())
            {
                if (IsMatchingYear(year, context) && CheckLimits(year))
                {
                    Periods.Add(year);
                }
            }
            return false; // abort
        }

        protected override bool OnVisitYear(Year year, CalendarPeriodCollectorContext context)
        {
            if (context.Scope != CalendarPeriodCollectorContext.CollectType.Month)
            {
                return true; // continue
            }

            // all months
            if (Filter.CollectingMonths.Count == 0)
            {
                foreach (Month month in year.GetMonths())
                {
                    if (IsMatchingMonth(month, context) && CheckLimits(month))
                    {
                        Periods.Add(month);
                    }
                }
            }
            else // custom months
            {
                foreach (MonthRange collectingMonth in Filter.CollectingMonths)
                {
                    if (collectingMonth.IsSingleMonth)
                    {
                        Month month = new Month(year.YearValue, collectingMonth.Min, year.Calendar);
                        if (IsMatchingMonth(month, context) && CheckLimits(month))
                        {
                            Periods.Add(month);
                        }
                    }
                    else
                    {
                        Months months = new Months(year.YearValue, collectingMonth.Min,
                            collectingMonth.Max - collectingMonth.Min, year.Calendar);
                        bool isMatching = true;
                        foreach (Month month in months.GetMonths())
                        {
                            if (IsMatchingMonth(month, context))
                            {
                                continue;
                            }
                            isMatching = false;
                            break;
                        }
                        if (isMatching && CheckLimits(months))
                        {
                            Periods.Add(months);
                        }
                    }
                }
            }

            return false; // abort
        }

        protected override bool OnVisitMonth(Month month, CalendarPeriodCollectorContext context)
        {
            if (context.Scope != CalendarPeriodCollectorContext.CollectType.Day)
            {
                return true; // continue
            }

            if (Filter.CollectingDays.Count == 0)
            {
                foreach (Day day in month.GetDays())
                {
                    if (IsMatchingDay(day, context) && CheckLimits(day))
                    {
                        Periods.Add(day);
                    }
                }
            }
            else // custom days
            {
                foreach (DayRange collectingDay in Filter.CollectingDays)
                {
                    if (collectingDay.IsSingleDay)
                    {
                        Day day = new Day(month.Year, month.MonthValue, collectingDay.Min, month.Calendar);
                        if (IsMatchingDay(day, context) && CheckLimits(day))
                        {
                            Periods.Add(day);
                        }
                    }
                    else
                    {
                        Days days = new Days(month.Year, month.MonthValue, collectingDay.Min, collectingDay.Max - collectingDay.Min, month.Calendar);
                        bool isMatching = true;
                        foreach (Day day in days.GetDays())
                        {
                            if (IsMatchingDay(day, context))
                            {
                                continue;
                            }
                            isMatching = false;
                            break;
                        }
                        if (isMatching && CheckLimits(days))
                        {
                            Periods.Add(days);
                        }
                    }
                }
            }

            return false; // abort
        }

        protected override bool OnVisitDay(Day day, CalendarPeriodCollectorContext context)
        {
            if (context.Scope != CalendarPeriodCollectorContext.CollectType.Hour)
            {
                return true; // continue
            }

            if (Filter.CollectingHours.Count == 0 && Filter.CollectingDayHours.Count == 0)
            {
                foreach (Hour hour in day.GetHours())
                {
                    if (IsMatchingHour(hour, context) && CheckLimits(hour))
                    {
                        Periods.Add(hour);
                    }
                }
            }
            else
            {
                if (IsMatchingDay(day, context))
                {
                    List<HourRange> collectingHours = new List<HourRange>(Filter.CollectingHours);
                    foreach (DayHourRange collectingDayHour in Filter.CollectingDayHours)
                    {
                        if (collectingDayHour.Day == day.DayOfWeek)
                        {
                            collectingHours.Add(collectingDayHour);
                        }
                    }
                    foreach (HourRange collectingHour in collectingHours)
                    {
                        DateTime start = collectingHour.Start.ToDateTime(day.Start);
                        DateTime end = collectingHour.End.ToDateTime(day.Start);
                        CalendarTimeRange hours = new CalendarTimeRange(start, end, day.Calendar);
                        if (CheckExcludePeriods(hours) && CheckLimits(hours))
                        {
                            Periods.Add(hours);
                        }
                    }
                }
            }

            return false; // abort
        }
    }
}
