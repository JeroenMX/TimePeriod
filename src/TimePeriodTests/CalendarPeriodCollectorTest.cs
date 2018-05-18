// -- FILE ------------------------------------------------------------------
// name       : CalendarPeriodCollectorTest.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using TimePeriod.Enums;
using Xunit;

namespace TimePeriod.Tests
{

	
	public sealed class CalendarPeriodCollectorTest : TestUnitBase
	{

        
        [Trait("Category", "CalendarPeriodCollector")]
        [Fact]
		public void CollectYearsTest()
		{
			CalendarPeriodCollectorFilter filter = new CalendarPeriodCollectorFilter();
			filter.Years.Add( 2006 );
			filter.Years.Add( 2007 );
			filter.Years.Add( 2012 );

			CalendarTimeRange testPeriod = new CalendarTimeRange( new DateTime( 2001, 1, 1 ), new DateTime( 2019, 12, 31 ) );

			CalendarPeriodCollector collector = new CalendarPeriodCollector( filter, testPeriod );
			collector.CollectYears();

			Assert.Equal(3, collector.Periods.Count);
			Assert.True( collector.Periods[ 0 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2006, 1, 1 ), new DateTime( 2007, 1, 1 ) ) ) );
			Assert.True( collector.Periods[ 1 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2007, 1, 1 ), new DateTime( 2008, 1, 1 ) ) ) );
			Assert.True( collector.Periods[ 2 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2012, 1, 1 ), new DateTime( 2013, 1, 1 ) ) ) );
		} // CollectYearsTest

        
        [Trait("Category", "CalendarPeriodCollector")]
        [Fact]
		public void CollectMonthsTest()
		{
			CalendarPeriodCollectorFilter filter = new CalendarPeriodCollectorFilter();
			filter.Months.Add( YearMonth.January );

			CalendarTimeRange testPeriod = new CalendarTimeRange( new DateTime( 2010, 1, 1 ), new DateTime( 2011, 12, 31 ) );

			CalendarPeriodCollector collector = new CalendarPeriodCollector( filter, testPeriod );
			collector.CollectMonths();

			Assert.Equal(2, collector.Periods.Count);
			Assert.True( collector.Periods[ 0 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2010, 1, 1 ), new DateTime( 2010, 2, 1 ) ) ) );
			Assert.True( collector.Periods[ 1 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2011, 1, 1 ), new DateTime( 2011, 2, 1 ) ) ) );
		} // CollectMonthsTest

        
        [Trait("Category", "CalendarPeriodCollector")]
        [Fact]
		public void CollectDaysTest()
		{
			CalendarPeriodCollectorFilter filter = new CalendarPeriodCollectorFilter();
			filter.Months.Add( YearMonth.January );
			filter.WeekDays.Add( DayOfWeek.Friday );

			CalendarTimeRange testPeriod = new CalendarTimeRange( new DateTime( 2010, 1, 1 ), new DateTime( 2011, 12, 31 ) );

			CalendarPeriodCollector collector = new CalendarPeriodCollector( filter, testPeriod );
			collector.CollectDays();

			Assert.Equal(9, collector.Periods.Count);
			Assert.True( collector.Periods[ 0 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2010, 1, 01 ), new DateTime( 2010, 1, 02 ) ) ) );
			Assert.True( collector.Periods[ 1 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2010, 1, 08 ), new DateTime( 2010, 1, 09 ) ) ) );
			Assert.True( collector.Periods[ 2 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2010, 1, 15 ), new DateTime( 2010, 1, 16 ) ) ) );
			Assert.True( collector.Periods[ 3 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2010, 1, 22 ), new DateTime( 2010, 1, 23 ) ) ) );
			Assert.True( collector.Periods[ 4 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2010, 1, 29 ), new DateTime( 2010, 1, 30 ) ) ) );
			Assert.True( collector.Periods[ 5 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2011, 1, 07 ), new DateTime( 2011, 1, 08 ) ) ) );
			Assert.True( collector.Periods[ 6 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2011, 1, 14 ), new DateTime( 2011, 1, 15 ) ) ) );
			Assert.True( collector.Periods[ 7 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2011, 1, 21 ), new DateTime( 2011, 1, 22 ) ) ) );
			Assert.True( collector.Periods[ 8 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2011, 1, 28 ), new DateTime( 2011, 1, 29 ) ) ) );
		} // CollectDaysTest

        
        [Trait("Category", "CalendarPeriodCollector")]
        [Fact]
		public void CollectHoursTest()
		{
			CalendarPeriodCollectorFilter filter = new CalendarPeriodCollectorFilter();
			filter.Months.Add( YearMonth.January );
			filter.WeekDays.Add( DayOfWeek.Friday );
			filter.CollectingHours.Add( new HourRange( 8, 18 ) );

			CalendarTimeRange testPeriod = new CalendarTimeRange( new DateTime( 2010, 1, 1 ), new DateTime( 2011, 12, 31 ) );

			CalendarPeriodCollector collector = new CalendarPeriodCollector( filter, testPeriod );
			collector.CollectHours();

			Assert.Equal(9, collector.Periods.Count);
			Assert.True( collector.Periods[ 0 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2010, 1, 01, 8, 0, 0 ), new DateTime( 2010, 1, 01, 18, 0, 0 ) ) ) );
			Assert.True( collector.Periods[ 1 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2010, 1, 08, 8, 0, 0 ), new DateTime( 2010, 1, 08, 18, 0, 0 ) ) ) );
			Assert.True( collector.Periods[ 2 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2010, 1, 15, 8, 0, 0 ), new DateTime( 2010, 1, 15, 18, 0, 0 ) ) ) );
			Assert.True( collector.Periods[ 3 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2010, 1, 22, 8, 0, 0 ), new DateTime( 2010, 1, 22, 18, 0, 0 ) ) ) );
			Assert.True( collector.Periods[ 4 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2010, 1, 29, 8, 0, 0 ), new DateTime( 2010, 1, 29, 18, 0, 0 ) ) ) );
			Assert.True( collector.Periods[ 5 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2011, 1, 07, 8, 0, 0 ), new DateTime( 2011, 1, 07, 18, 0, 0 ) ) ) );
			Assert.True( collector.Periods[ 6 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2011, 1, 14, 8, 0, 0 ), new DateTime( 2011, 1, 14, 18, 0, 0 ) ) ) );
			Assert.True( collector.Periods[ 7 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2011, 1, 21, 8, 0, 0 ), new DateTime( 2011, 1, 21, 18, 0, 0 ) ) ) );
			Assert.True( collector.Periods[ 8 ].IsSamePeriod( new CalendarTimeRange( new DateTime( 2011, 1, 28, 8, 0, 0 ), new DateTime( 2011, 1, 28, 18, 0, 0 ) ) ) );
		} // CollectHoursTest

        
        [Trait("Category", "CalendarPeriodCollector")]
        [Fact]
		public void Collect24HoursTest()
		{
			CalendarPeriodCollectorFilter filter = new CalendarPeriodCollectorFilter();
			filter.Months.Add( YearMonth.January );
			filter.WeekDays.Add( DayOfWeek.Friday );
			filter.CollectingHours.Add( new HourRange( 8, 24 ) );

			TimeRange testPeriod = new TimeRange( new DateTime( 2010, 1, 1 ), new DateTime( 2012, 1, 1 ) );

			TimeCalendar calendar = new TimeCalendar( new TimeCalendarConfig { EndOffset = TimeSpan.Zero } );
			CalendarPeriodCollector collector = new CalendarPeriodCollector( filter, testPeriod, SeekDirection.Forward, calendar );
			collector.CollectHours();

			Assert.Equal(9, collector.Periods.Count);
			Assert.True( collector.Periods[ 0 ].IsSamePeriod( new TimeRange( new DateTime( 2010, 1, 01, 8, 0, 0 ), new DateTime( 2010, 1, 02 ) ) ) );
			Assert.True( collector.Periods[ 1 ].IsSamePeriod( new TimeRange( new DateTime( 2010, 1, 08, 8, 0, 0 ), new DateTime( 2010, 1, 09 ) ) ) );
			Assert.True( collector.Periods[ 2 ].IsSamePeriod( new TimeRange( new DateTime( 2010, 1, 15, 8, 0, 0 ), new DateTime( 2010, 1, 16 ) ) ) );
			Assert.True( collector.Periods[ 3 ].IsSamePeriod( new TimeRange( new DateTime( 2010, 1, 22, 8, 0, 0 ), new DateTime( 2010, 1, 23 ) ) ) );
			Assert.True( collector.Periods[ 4 ].IsSamePeriod( new TimeRange( new DateTime( 2010, 1, 29, 8, 0, 0 ), new DateTime( 2010, 1, 30 ) ) ) );
			Assert.True( collector.Periods[ 5 ].IsSamePeriod( new TimeRange( new DateTime( 2011, 1, 07, 8, 0, 0 ), new DateTime( 2011, 1, 08 ) ) ) );
			Assert.True( collector.Periods[ 6 ].IsSamePeriod( new TimeRange( new DateTime( 2011, 1, 14, 8, 0, 0 ), new DateTime( 2011, 1, 15 ) ) ) );
			Assert.True( collector.Periods[ 7 ].IsSamePeriod( new TimeRange( new DateTime( 2011, 1, 21, 8, 0, 0 ), new DateTime( 2011, 1, 22 ) ) ) );
			Assert.True( collector.Periods[ 8 ].IsSamePeriod( new TimeRange( new DateTime( 2011, 1, 28, 8, 0, 0 ), new DateTime( 2011, 1, 29 ) ) ) );
		} // Collect24HoursTest

        
        [Trait("Category", "CalendarPeriodCollector")]
        [Fact]
		public void CollectAllDayHoursTest()
		{
			CalendarPeriodCollectorFilter filter = new CalendarPeriodCollectorFilter();
			filter.Months.Add( YearMonth.January );
			filter.WeekDays.Add( DayOfWeek.Friday );
			filter.CollectingHours.Add( new HourRange( 0, 24 ) );

			TimeRange testPeriod = new TimeRange( new DateTime( 2010, 1, 1 ), new DateTime( 2012, 1, 1 ) );

			TimeCalendar calendar = new TimeCalendar( new TimeCalendarConfig { EndOffset = TimeSpan.Zero } );
			CalendarPeriodCollector collector = new CalendarPeriodCollector( filter, testPeriod, SeekDirection.Forward, calendar );
			collector.CollectHours();

			Assert.Equal(9, collector.Periods.Count);
			Assert.True( collector.Periods[ 0 ].IsSamePeriod( new TimeRange( new DateTime( 2010, 1, 01 ), new DateTime( 2010, 1, 02 ) ) ) );
			Assert.True( collector.Periods[ 1 ].IsSamePeriod( new TimeRange( new DateTime( 2010, 1, 08 ), new DateTime( 2010, 1, 09 ) ) ) );
			Assert.True( collector.Periods[ 2 ].IsSamePeriod( new TimeRange( new DateTime( 2010, 1, 15 ), new DateTime( 2010, 1, 16 ) ) ) );
			Assert.True( collector.Periods[ 3 ].IsSamePeriod( new TimeRange( new DateTime( 2010, 1, 22 ), new DateTime( 2010, 1, 23 ) ) ) );
			Assert.True( collector.Periods[ 4 ].IsSamePeriod( new TimeRange( new DateTime( 2010, 1, 29 ), new DateTime( 2010, 1, 30 ) ) ) );
			Assert.True( collector.Periods[ 5 ].IsSamePeriod( new TimeRange( new DateTime( 2011, 1, 07 ), new DateTime( 2011, 1, 08 ) ) ) );
			Assert.True( collector.Periods[ 6 ].IsSamePeriod( new TimeRange( new DateTime( 2011, 1, 14 ), new DateTime( 2011, 1, 15 ) ) ) );
			Assert.True( collector.Periods[ 7 ].IsSamePeriod( new TimeRange( new DateTime( 2011, 1, 21 ), new DateTime( 2011, 1, 22 ) ) ) );
			Assert.True( collector.Periods[ 8 ].IsSamePeriod( new TimeRange( new DateTime( 2011, 1, 28 ), new DateTime( 2011, 1, 29 ) ) ) );
		} // CollectAllDayHoursTest

        
        [Trait("Category", "CalendarPeriodCollector")]
        [Fact]
		public void CollectExcludePeriodTest()
		{
			const int workingDays2011 = 365 - 2 - ( 51 * 2 ) - 1;
			const int workingDaysMarch2011 = 31 - 8; // total days - weekend days

			Year year2011 = new Year( 2011 );

			CalendarPeriodCollectorFilter filter1 = new CalendarPeriodCollectorFilter();
			filter1.AddWorkingWeekDays();
			CalendarPeriodCollector collector1 = new CalendarPeriodCollector( filter1, year2011 );
			collector1.CollectDays();
			Assert.Equal( collector1.Periods.Count, workingDays2011 );

			// exclude month
			CalendarPeriodCollectorFilter filter2 = new CalendarPeriodCollectorFilter();
			filter2.AddWorkingWeekDays();
			filter2.ExcludePeriods.Add( new Month( 2011, YearMonth.March ) );
			CalendarPeriodCollector collector2 = new CalendarPeriodCollector( filter2, year2011 );
			collector2.CollectDays();
			Assert.Equal( collector2.Periods.Count, workingDays2011 - workingDaysMarch2011 );

			// exclude weeks (holidays)
			CalendarPeriodCollectorFilter filter3 = new CalendarPeriodCollectorFilter();
			filter3.AddWorkingWeekDays();
			filter3.ExcludePeriods.Add( new Month( 2011, YearMonth.March ) );
			filter3.ExcludePeriods.Add( new Weeks( 2011, 26, 2 ) );
			CalendarPeriodCollector collector3 = new CalendarPeriodCollector( filter3, year2011 );
			collector3.CollectDays();
			Assert.Equal( collector3.Periods.Count, workingDays2011 - workingDaysMarch2011 - 10 );
		} // CollectExcludePeriodTest

        
        [Trait("Category", "CalendarPeriodCollector")]
        [Fact]
        public void CollectHoursMissingLastPeriodTest()
        {
            CalendarPeriodCollectorFilter filter = new CalendarPeriodCollectorFilter();
            filter.Months.Add(YearMonth.September);
            filter.WeekDays.Add(DayOfWeek.Monday);
            filter.WeekDays.Add(DayOfWeek.Tuesday);
            filter.WeekDays.Add(DayOfWeek.Wednesday);
            filter.WeekDays.Add(DayOfWeek.Thursday);
            filter.WeekDays.Add(DayOfWeek.Friday);
            filter.CollectingHours.Add(new HourRange(9, 17)); // working hours
            filter.ExcludePeriods.Add(new TimeBlock(new DateTime(2015, 9, 15, 00, 0, 0), new DateTime(2015, 9, 16, 0, 0, 0)));

            CalendarTimeRange testPeriod = new CalendarTimeRange(new DateTime(2015, 9, 14, 9, 0, 0), new DateTime(2015, 9, 17, 18, 0, 0));
            CalendarPeriodCollector collector = new CalendarPeriodCollector(filter, testPeriod);
            collector.CollectHours();
            Assert.Equal(3, collector.Periods.Count);
        } // CollectHoursMissingLastPeriodTest

    } // class CalendarPeriodCollectorTest

} // namespace Itenso.TimePeriodTests
// -- EOF -------------------------------------------------------------------
