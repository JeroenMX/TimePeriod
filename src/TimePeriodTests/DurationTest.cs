// -- FILE ------------------------------------------------------------------
// name       : DurationTest.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using System.Globalization;
using TimePeriod.Enums;
using Xunit;

namespace TimePeriod.Tests
{
	public sealed class DurationTest : TestUnitBase
	{

        
        [Trait("Category", "Duration")]
        [Fact]
		public void YearTest()
		{
			int currentYear = ClockProxy.Clock.Now.Year;
			Calendar calendar = DateDiff.SafeCurrentInfo.Calendar;

			Assert.Equal<TimeSpan>( Duration.Year( currentYear ), new TimeSpan( calendar.GetDaysInYear( currentYear ), 0, 0, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Year( currentYear + 1 ), new TimeSpan( calendar.GetDaysInYear( currentYear + 1 ), 0, 0, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Year( currentYear - 1 ), new TimeSpan( calendar.GetDaysInYear( currentYear - 1 ), 0, 0, 0 ) );

			Assert.Equal<TimeSpan>( Duration.Year( calendar, currentYear ), new TimeSpan( calendar.GetDaysInYear( currentYear ), 0, 0, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Year( calendar, currentYear + 1 ), new TimeSpan( calendar.GetDaysInYear( currentYear + 1 ), 0, 0, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Year( calendar, currentYear - 1 ), new TimeSpan( calendar.GetDaysInYear( currentYear - 1 ), 0, 0, 0 ) );
		} // YearTest

        
        [Trait("Category", "Duration")]
        [Fact]
		public void HalfyearTest()
		{
			int currentYear = ClockProxy.Clock.Now.Year;
			Calendar calendar = DateDiff.SafeCurrentInfo.Calendar;

			foreach ( YearHalfyear yearHalfyear in Enum.GetValues( typeof( YearHalfyear ) ) )
			{
				YearMonth[] halfyearMonths = TimeTool.GetMonthsOfHalfyear( yearHalfyear );
				TimeSpan duration = TimeSpan.Zero;
				foreach ( YearMonth halfyearMonth in halfyearMonths )
				{
					int monthDays = calendar.GetDaysInMonth( currentYear, (int)halfyearMonth );
					duration = duration.Add( new TimeSpan( monthDays, 0, 0, 0 ) );
				}

				Assert.Equal<TimeSpan>( Duration.Halfyear( currentYear, yearHalfyear ), duration );
				Assert.Equal<TimeSpan>( Duration.Halfyear( calendar, currentYear, yearHalfyear ), duration );
			}
		} // HalfyearTest

        
        [Trait("Category", "Duration")]
        [Fact]
		public void QuarterTest()
		{
			int currentYear = ClockProxy.Clock.Now.Year;
			Calendar calendar = DateDiff.SafeCurrentInfo.Calendar;

			foreach ( YearQuarter yearQuarter in Enum.GetValues( typeof( YearQuarter ) ) )
			{
				YearMonth[] quarterMonths = TimeTool.GetMonthsOfQuarter( yearQuarter );
				TimeSpan duration = TimeSpan.Zero;
				foreach ( YearMonth quarterMonth in quarterMonths )
				{
					int monthDays = calendar.GetDaysInMonth( currentYear, (int)quarterMonth );
					duration = duration.Add( new TimeSpan( monthDays, 0, 0, 0 ) );
				}

				Assert.Equal<TimeSpan>( Duration.Quarter( currentYear, yearQuarter ), duration );
				Assert.Equal<TimeSpan>( Duration.Quarter( calendar, currentYear, yearQuarter ), duration );
			}
		} // QuarterTest

        
        [Trait("Category", "Duration")]
        [Fact]
		public void MonthTest()
		{
			DateTime now = ClockProxy.Clock.Now;
			int currentYear = now.Year;
			Calendar calendar = DateDiff.SafeCurrentInfo.Calendar;

			foreach ( YearMonth yearMonth in Enum.GetValues( typeof( YearMonth ) ) )
			{
				Assert.Equal<TimeSpan>( Duration.Month( currentYear, yearMonth ), new TimeSpan( calendar.GetDaysInMonth( currentYear, (int)yearMonth ), 0, 0, 0 ) );
				Assert.Equal<TimeSpan>( Duration.Month( calendar, currentYear, yearMonth ), new TimeSpan( calendar.GetDaysInMonth( currentYear, (int)yearMonth ), 0, 0, 0 ) );
			}
		} // MonthTest

        
        [Trait("Category", "Duration")]
        [Fact]
		public void WeekTest()
		{
			Assert.Equal<TimeSpan>( Duration.Week, new TimeSpan( TimeSpec.DaysPerWeek * 1, 0, 0, 0 ) );

			Assert.Equal<TimeSpan>( Duration.Weeks( 0 ), TimeSpan.Zero );
			Assert.Equal<TimeSpan>( Duration.Weeks( 1 ), new TimeSpan( TimeSpec.DaysPerWeek * 1, 0, 0, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Weeks( 2 ), new TimeSpan( TimeSpec.DaysPerWeek * 2, 0, 0, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Weeks( -1 ), new TimeSpan( TimeSpec.DaysPerWeek * -1, 0, 0, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Weeks( -2 ), new TimeSpan( TimeSpec.DaysPerWeek * -2, 0, 0, 0 ) );
		} // WeekTest

        
        [Trait("Category", "Duration")]
        [Fact]
		public void DayTest()
		{
			Assert.Equal<TimeSpan>( Duration.Day, new TimeSpan( 1, 0, 0, 0 ) );

			Assert.Equal<TimeSpan>( Duration.Days( 0 ), TimeSpan.Zero );
			Assert.Equal<TimeSpan>( Duration.Days( 1 ), new TimeSpan( 1, 0, 0, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Days( 2 ), new TimeSpan( 2, 0, 0, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Days( -1 ), new TimeSpan( -1, 0, 0, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Days( -2 ), new TimeSpan( -2, 0, 0, 0 ) );

			Assert.Equal<TimeSpan>( Duration.Days( 1, 23 ), new TimeSpan( 1, 23, 0, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Days( 1, 23, 22 ), new TimeSpan( 1, 23, 22, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Days( 1, 23, 22, 18 ), new TimeSpan( 1, 23, 22, 18 ) );
			Assert.Equal<TimeSpan>( Duration.Days( 1, 23, 22, 18, 875 ), new TimeSpan( 1, 23, 22, 18, 875 ) );
		} // DayTest

        
        [Trait("Category", "Duration")]
        [Fact]
		public void HourTest()
		{
			Assert.Equal<TimeSpan>( Duration.Hour, new TimeSpan( 1, 0, 0 ) );

			Assert.Equal<TimeSpan>( Duration.Hours( 0 ), TimeSpan.Zero );
			Assert.Equal<TimeSpan>( Duration.Hours( 1 ), new TimeSpan( 1, 0, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Hours( 2 ), new TimeSpan( 2, 0, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Hours( -1 ), new TimeSpan( -1, 0, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Hours( -2 ), new TimeSpan( -2, 0, 0 ) );

			Assert.Equal<TimeSpan>( Duration.Hours( 23 ), new TimeSpan( 0, 23, 0, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Hours( 23, 22 ), new TimeSpan( 0, 23, 22, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Hours( 23, 22, 18 ), new TimeSpan( 0, 23, 22, 18 ) );
			Assert.Equal<TimeSpan>( Duration.Hours( 23, 22, 18, 875 ), new TimeSpan( 0, 23, 22, 18, 875 ) );
		} // HourTest

        
        [Trait("Category", "Duration")]
        [Fact]
		public void MinuteTest()
		{
			Assert.Equal<TimeSpan>( Duration.Minute, new TimeSpan( 0, 1, 0 ) );

			Assert.Equal<TimeSpan>( Duration.Minutes( 0 ), TimeSpan.Zero );
			Assert.Equal<TimeSpan>( Duration.Minutes( 1 ), new TimeSpan( 0, 1, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Minutes( 2 ), new TimeSpan( 0, 2, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Minutes( -1 ), new TimeSpan( 0, -1, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Minutes( -2 ), new TimeSpan( 0, -2, 0 ) );

			Assert.Equal<TimeSpan>( Duration.Minutes( 22 ), new TimeSpan( 0, 0, 22, 0 ) );
			Assert.Equal<TimeSpan>( Duration.Minutes( 22, 18 ), new TimeSpan( 0, 0, 22, 18 ) );
			Assert.Equal<TimeSpan>( Duration.Minutes( 22, 18, 875 ), new TimeSpan( 0, 0, 22, 18, 875 ) );
		} // MinuteTest

        
        [Trait("Category", "Duration")]
        [Fact]
		public void SecondTest()
		{
			Assert.Equal<TimeSpan>( Duration.Second, new TimeSpan( 0, 0, 1 ) );

			Assert.Equal<TimeSpan>( Duration.Seconds( 0 ), TimeSpan.Zero );
			Assert.Equal<TimeSpan>( Duration.Seconds( 1 ), new TimeSpan( 0, 0, 1 ) );
			Assert.Equal<TimeSpan>( Duration.Seconds( 2 ), new TimeSpan( 0, 0, 2 ) );
			Assert.Equal<TimeSpan>( Duration.Seconds( -1 ), new TimeSpan( 0, 0, -1 ) );
			Assert.Equal<TimeSpan>( Duration.Seconds( -2 ), new TimeSpan( 0, 0, -2 ) );

			Assert.Equal<TimeSpan>( Duration.Seconds( 18 ), new TimeSpan( 0, 0, 0, 18 ) );
			Assert.Equal<TimeSpan>( Duration.Seconds( 18, 875 ), new TimeSpan( 0, 0, 0, 18, 875 ) );
		} // SecondTest

        
        [Trait("Category", "Duration")]
        [Fact]
		public void MillisecondTest()
		{
			Assert.Equal<TimeSpan>( Duration.Millisecond, new TimeSpan( 0, 0, 0, 0, 1 ) );

			Assert.Equal<TimeSpan>( Duration.Milliseconds( 0 ), TimeSpan.Zero );
			Assert.Equal<TimeSpan>( Duration.Milliseconds( 1 ), new TimeSpan( 0, 0, 0, 0, 1 ) );
			Assert.Equal<TimeSpan>( Duration.Milliseconds( 2 ), new TimeSpan( 0, 0, 0, 0, 2 ) );
			Assert.Equal<TimeSpan>( Duration.Milliseconds( -1 ), new TimeSpan( 0, 0, 0, 0, -1 ) );
			Assert.Equal<TimeSpan>( Duration.Milliseconds( -2 ), new TimeSpan( 0, 0, 0, 0, -2 ) );
		} // MillisecondTest

	} // class DurationTest

} // namespace Itenso.TimePeriodTests
// -- EOF -------------------------------------------------------------------
