// -- FILE ------------------------------------------------------------------
// name       : CalendarVisitorFilter.cs
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
    public class CalendarVisitorFilter : ICalendarVisitorFilter
    {
        private readonly TimePeriodCollection _excludePeriods = new TimePeriodCollection();
        private readonly List<int> _years = new List<int>();
        private readonly List<YearMonth> _months = new List<YearMonth>();
        private readonly List<int> _days = new List<int>();
        private readonly List<DayOfWeek> _weekDays = new List<DayOfWeek>();
        private readonly List<int> _hours = new List<int>();        

        public ITimePeriodCollection ExcludePeriods => _excludePeriods;
        public IList<int> Years => _years;
        public IList<YearMonth> Months => _months;
        public IList<int> Days => _days;
        public IList<DayOfWeek> WeekDays => _weekDays;
        public IList<int> Hours => _hours;

        public virtual void Clear()
        {
            _years.Clear();
            _months.Clear();
            _days.Clear();
            _weekDays.Clear();
            _hours.Clear();
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
    }
}
