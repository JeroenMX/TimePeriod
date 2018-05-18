// -- FILE ------------------------------------------------------------------
// name       : CalendarPeriodCollectorFilter.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System.Collections.Generic;
using TimePeriod.Interfaces;

namespace TimePeriod
{
	public class CalendarPeriodCollectorFilter : CalendarVisitorFilter, ICalendarPeriodCollectorFilter
	{
	    private readonly List<MonthRange> _collectingMonths = new List<MonthRange>();
	    private readonly List<DayRange> _collectingDays = new List<DayRange>();
	    private readonly List<HourRange> _collectingHours = new List<HourRange>();
	    private readonly List<DayHourRange> _collectingDayHours = new List<DayHourRange>();

	    public IList<MonthRange> CollectingMonths => _collectingMonths;
	    public IList<DayRange> CollectingDays => _collectingDays;
	    public IList<HourRange> CollectingHours => _collectingHours;
	    public IList<DayHourRange> CollectingDayHours => _collectingDayHours;

        public override void Clear()
		{
			base.Clear();
			_collectingMonths.Clear();
			_collectingDays.Clear();
			_collectingHours.Clear();
		}
	}
}
