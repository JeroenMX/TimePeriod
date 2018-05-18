// -- FILE ------------------------------------------------------------------
// name       : BusinessCaseTest.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using TimePeriod.Enums;
using TimePeriod.Interfaces;
using Xunit;

namespace TimePeriod.Tests
{
	
	public sealed class BusinessCaseTest : TestUnitBase
	{

        
        [Trait("Category", "BusinessCase")]
        [Fact]
		public void TimeRangeCalendarTimeRangeTest()
		{
			DateTime now = ClockProxy.Clock.Now;
			TimeRange fiveSeconds = new TimeRange(
				new DateTime( now.Year, now.Month, now.Day, now.Hour, now.Minute, 15, 0 ),
				new DateTime( now.Year, now.Month, now.Day, now.Hour, now.Minute, 20, 0 ) );
			Assert.True( new Year( now ).HasInside( fiveSeconds ) );
			Assert.True( new Quarter( now ).HasInside( fiveSeconds ) );
			Assert.True( new Month( now ).HasInside( fiveSeconds ) );
			// fo nto test the week: can be outside of the current days
			//Assert.True( new Week( now ).HasInside( fiveSeconds ) );
			Assert.True( new Day( now ).HasInside( fiveSeconds ) );
			Assert.True( new Hour( now ).HasInside( fiveSeconds ) );
			Assert.True( new Minute( now ).HasInside( fiveSeconds ) );

			TimeRange anytime = new TimeRange();
			Assert.False( new Year().HasInside( anytime ) );
			Assert.False( new Quarter().HasInside( anytime ) );
			Assert.False( new Month().HasInside( anytime ) );
			Assert.False( new Week().HasInside( anytime ) );
			Assert.False( new Day().HasInside( anytime ) );
			Assert.False( new Hour().HasInside( anytime ) );
			Assert.False( new Minute().HasInside( anytime ) );
		} // TimeRangeCalendarTimeRangeTest

        
        [Trait("Category", "BusinessCase")]
        [Fact]
		public void FiscalYearTest()
		{
			DateTime testDate = new DateTime( 2008, 11, 18 );
			Year year = new Year( testDate, TimeCalendar.New( YearMonth.October ) );

			Assert.Equal(YearMonth.October, year.YearBaseMonth);
			Assert.Equal(2008, year.BaseYear);

			// start & end
			Assert.Equal( year.Start.Year, testDate.Year );
			Assert.Equal(10, year.Start.Month);
			Assert.Equal(1, year.Start.Day);
			Assert.Equal( year.End.Year, testDate.Year + 1 );
			Assert.Equal(9, year.End.Month);
			Assert.Equal(30, year.End.Day);

			// half years
			ITimePeriodCollection halfyears = year.GetHalfyears();
			foreach ( Halfyear halfyear in halfyears )
			{
				switch ( halfyear.YearHalfyear )
				{
					case YearHalfyear.First:
						Assert.Equal( halfyear.Start, year.Start );
						Assert.Equal( halfyear.Start.Year, testDate.Year );
						Assert.Equal(10, halfyear.Start.Month);
						Assert.Equal(1, halfyear.Start.Day);
						Assert.Equal( halfyear.End.Year, testDate.Year + 1 );
						Assert.Equal(3, halfyear.End.Month);
						Assert.Equal(31, halfyear.End.Day);
						break;
					case YearHalfyear.Second:
						Assert.Equal( halfyear.End, year.End );
						Assert.Equal( halfyear.Start.Year, testDate.Year + 1 );
						Assert.Equal(4, halfyear.Start.Month);
						Assert.Equal(1, halfyear.Start.Day);
						Assert.Equal( halfyear.End.Year, testDate.Year + 1 );
						Assert.Equal(9, halfyear.End.Month);
						Assert.Equal(30, halfyear.End.Day);
						break;
				}
			}

			// half years
			ITimePeriodCollection quarters = year.GetQuarters();
			foreach ( Quarter quarter in quarters )
			{
				switch ( quarter.YearQuarter )
				{
					case YearQuarter.First:
						Assert.Equal( quarter.Start, year.Start );
						Assert.Equal( quarter.Start.Year, testDate.Year );
						Assert.Equal(10, quarter.Start.Month);
						Assert.Equal(1, quarter.Start.Day);
						Assert.Equal( quarter.End.Year, testDate.Year );
						Assert.Equal(12, quarter.End.Month);
						Assert.Equal(31, quarter.End.Day);
						break;
					case YearQuarter.Second:
						Assert.Equal( quarter.Start.Year, testDate.Year + 1 );
						Assert.Equal(1, quarter.Start.Month);
						Assert.Equal(1, quarter.Start.Day);
						Assert.Equal( quarter.End.Year, testDate.Year + 1 );
						Assert.Equal(3, quarter.End.Month);
						Assert.Equal(31, quarter.End.Day);
						break;
					case YearQuarter.Third:
						Assert.Equal( quarter.Start.Year, testDate.Year + 1 );
						Assert.Equal(4, quarter.Start.Month);
						Assert.Equal(1, quarter.Start.Day);
						Assert.Equal( quarter.End.Year, testDate.Year + 1 );
						Assert.Equal(6, quarter.End.Month);
						Assert.Equal(30, quarter.End.Day);
						break;
					case YearQuarter.Fourth:
						Assert.Equal( quarter.End, year.End );
						Assert.Equal( quarter.Start.Year, testDate.Year + 1 );
						Assert.Equal(7, quarter.Start.Month);
						Assert.Equal(1, quarter.Start.Day);
						Assert.Equal( quarter.End.Year, testDate.Year + 1 );
						Assert.Equal(9, quarter.End.Month);
						Assert.Equal(30, quarter.End.Day);
						break;
				}
			}

			// months
			ITimePeriodCollection months = year.GetMonths();
			int monthIndex = 0;
			foreach ( Month month in months )
			{
				switch ( monthIndex )
				{
					case 0:
						Assert.Equal( month.Start, year.Start );
						break;
					case TimeSpec.MonthsPerYear - 1:
						Assert.Equal( month.End, year.End );
						break;
				}

				DateTime startDate = new DateTime( year.BaseYear, year.Start.Month, 1 ).AddMonths( monthIndex );
				Assert.Equal( month.Start.Year, startDate.Year );
				Assert.Equal( month.Start.Month, startDate.Month );
				Assert.Equal( month.Start.Day, startDate.Day );
				Assert.Equal( month.End.Year, startDate.Year );
				Assert.Equal( month.End.Month, startDate.Month );

				monthIndex++;
			}
		} // FiscalYearTest

        
        [Trait("Category", "BusinessCase")]
        [Fact]
		public void CalendarQuarterOfYearTest()
		{
			int currentYear = ClockProxy.Clock.Now.Year;

			TimeCalendar timeCalendar = TimeCalendar.New( YearMonth.October );
			Year calendarYear = new Year( currentYear, timeCalendar );
			Assert.Equal(YearMonth.October, calendarYear.YearBaseMonth);
			Assert.Equal<int>( calendarYear.BaseYear, currentYear );
			Assert.Equal<DateTime>( calendarYear.Start, new DateTime( currentYear, 10, 1 ) );
			Assert.Equal( calendarYear.End, calendarYear.Calendar.MapEnd( calendarYear.Start.AddYears( 1 ) ) );

			// Q1
			Quarter q1 = new Quarter( calendarYear.BaseYear, YearQuarter.First, timeCalendar );
			Assert.Equal( q1.YearBaseMonth, calendarYear.YearBaseMonth );
			Assert.Equal( q1.BaseYear, calendarYear.BaseYear );
			Assert.Equal<DateTime>( q1.Start, new DateTime( currentYear, 10, 1 ) );
			Assert.Equal( q1.End, q1.Calendar.MapEnd( q1.Start.AddMonths( 3 ) ) );

			// Q2
			Quarter q2 = new Quarter( calendarYear.BaseYear, YearQuarter.Second, timeCalendar );
			Assert.Equal( q2.YearBaseMonth, calendarYear.YearBaseMonth );
			Assert.Equal( q2.BaseYear, calendarYear.BaseYear );
			Assert.Equal<DateTime>( q2.Start, new DateTime( currentYear + 1, 1, 1 ) );
			Assert.Equal( q2.End, q2.Calendar.MapEnd( q2.Start.AddMonths( 3 ) ) );

			// Q3
			Quarter q3 = new Quarter( calendarYear.BaseYear, YearQuarter.Third, timeCalendar );
			Assert.Equal( q3.YearBaseMonth, calendarYear.YearBaseMonth );
			Assert.Equal( q3.BaseYear, calendarYear.BaseYear );
			Assert.Equal<DateTime>( q3.Start, new DateTime( currentYear + 1, 4, 1 ) );
			Assert.Equal( q3.End, q3.Calendar.MapEnd( q3.Start.AddMonths( 3 ) ) );

			// Q4
			Quarter q4 = new Quarter( calendarYear.BaseYear, YearQuarter.Fourth, timeCalendar );
			Assert.Equal( q4.YearBaseMonth, calendarYear.YearBaseMonth );
			Assert.Equal( q4.BaseYear, calendarYear.BaseYear );
			Assert.Equal<DateTime>( q4.Start, new DateTime( currentYear + 1, 7, 1 ) );
			Assert.Equal( q4.End, q4.Calendar.MapEnd( q4.Start.AddMonths( 3 ) ) );

		} // CalendarQuarterOfYearTest

	} // class BusinessCaseTest

} // namespace Itenso.TimePeriodTests
// -- EOF -------------------------------------------------------------------
