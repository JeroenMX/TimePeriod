// -- FILE ------------------------------------------------------------------
// name       : TimeTrimTest.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using Xunit;

namespace TimePeriod.Tests
{	
	
	public sealed class TimeTrimTest : TestUnitBase
	{

        
        [Trait("Category", "TimeTrim")]
        [Fact]
		public void MonthTest()
		{
			Assert.Equal<DateTime>( TimeTrim.Month( testDate ), new DateTime( testDate.Year, 1, 1 ) );
			Assert.Equal<DateTime>( TimeTrim.Month( testDate, 6 ), new DateTime( testDate.Year, 6, 1 ) );
			Assert.Equal<DateTime>( TimeTrim.Month( testDate, 6, 5 ), new DateTime( testDate.Year, 6, 5, 0, 0, 0 ) );
			Assert.Equal<DateTime>( TimeTrim.Month( testDate, 6, 5, 4 ), new DateTime( testDate.Year, 6, 5, 4, 0, 0 ) );
			Assert.Equal<DateTime>( TimeTrim.Month( testDate, 6, 5, 4, 23 ), new DateTime( testDate.Year, 6, 5, 4, 23, 0 ) );
			Assert.Equal<DateTime>( TimeTrim.Month( testDate, 6, 5, 4, 23, 55 ), new DateTime( testDate.Year, 6, 5, 4, 23, 55 ) );
			Assert.Equal<DateTime>( TimeTrim.Month( testDate, 6, 5, 4, 23, 55, 128 ), new DateTime( testDate.Year, 6, 5, 4, 23, 55, 128 ) );
		} // MonthTest

        
        [Trait("Category", "TimeTrim")]
        [Fact]
		public void DayTest()
		{
			Assert.Equal<DateTime>( TimeTrim.Day( testDate ), new DateTime( testDate.Year, testDate.Month, 1 ) );
			Assert.Equal<DateTime>( TimeTrim.Day( testDate, 5 ), new DateTime( testDate.Year, testDate.Month, 5, 0, 0, 0 ) );
			Assert.Equal<DateTime>( TimeTrim.Day( testDate, 5, 4 ), new DateTime( testDate.Year, testDate.Month, 5, 4, 0, 0 ) );
			Assert.Equal<DateTime>( TimeTrim.Day( testDate, 5, 4, 23 ), new DateTime( testDate.Year, testDate.Month, 5, 4, 23, 0 ) );
			Assert.Equal<DateTime>( TimeTrim.Day( testDate, 5, 4, 23, 55 ), new DateTime( testDate.Year, testDate.Month, 5, 4, 23, 55 ) );
			Assert.Equal<DateTime>( TimeTrim.Day( testDate, 5, 4, 23, 55, 128 ), new DateTime( testDate.Year, testDate.Month, 5, 4, 23, 55, 128 ) );
		} // DayTest

        
        [Trait("Category", "TimeTrim")]
        [Fact]
		public void HourTest()
		{
			Assert.Equal<DateTime>( TimeTrim.Hour( testDate ), new DateTime( testDate.Year, testDate.Month, testDate.Day ) );
			Assert.Equal<DateTime>( TimeTrim.Hour( testDate, 4 ), new DateTime( testDate.Year, testDate.Month, testDate.Day, 4, 0, 0 ) );
			Assert.Equal<DateTime>( TimeTrim.Hour( testDate, 4, 23 ), new DateTime( testDate.Year, testDate.Month, testDate.Day, 4, 23, 0 ) );
			Assert.Equal<DateTime>( TimeTrim.Hour( testDate, 4, 23, 55 ), new DateTime( testDate.Year, testDate.Month, testDate.Day, 4, 23, 55 ) );
			Assert.Equal<DateTime>( TimeTrim.Hour( testDate, 4, 23, 55, 128 ), new DateTime( testDate.Year, testDate.Month, testDate.Day, 4, 23, 55, 128 ) );
		} // HourTest

        
        [Trait("Category", "TimeTrim")]
        [Fact]
		public void MinuteTest()
		{
			Assert.Equal<DateTime>( TimeTrim.Minute( testDate ), new DateTime( testDate.Year, testDate.Month, testDate.Day, testDate.Hour, 0, 0 ) );
			Assert.Equal<DateTime>( TimeTrim.Minute( testDate, 23 ), new DateTime( testDate.Year, testDate.Month, testDate.Day, testDate.Hour, 23, 0 ) );
			Assert.Equal<DateTime>( TimeTrim.Minute( testDate, 23, 55 ), new DateTime( testDate.Year, testDate.Month, testDate.Day, testDate.Hour, 23, 55 ) );
			Assert.Equal<DateTime>( TimeTrim.Minute( testDate, 23, 55, 128 ), new DateTime( testDate.Year, testDate.Month, testDate.Day, testDate.Hour, 23, 55, 128 ) );
		} // MinuteTest

        
        [Trait("Category", "TimeTrim")]
        [Fact]
		public void SecondTest()
		{
			Assert.Equal<DateTime>( TimeTrim.Second( testDate ), new DateTime( testDate.Year, testDate.Month, testDate.Day, testDate.Hour, testDate.Minute, 0 ) );
			Assert.Equal<DateTime>( TimeTrim.Second( testDate, 55 ), new DateTime( testDate.Year, testDate.Month, testDate.Day, testDate.Hour, testDate.Minute, 55 ) );
			Assert.Equal<DateTime>( TimeTrim.Second( testDate, 55, 128 ), new DateTime( testDate.Year, testDate.Month, testDate.Day, testDate.Hour, testDate.Minute, 55, 128 ) );
		} // SecondTest

        
        [Trait("Category", "TimeTrim")]
        [Fact]
		public void MillisecondTest()
		{
			Assert.Equal<DateTime>( TimeTrim.Millisecond( testDate ), new DateTime( testDate.Year, testDate.Month, testDate.Day, testDate.Hour, testDate.Minute, testDate.Second ) );
			Assert.Equal<DateTime>( TimeTrim.Millisecond( testDate, 128 ), new DateTime( testDate.Year, testDate.Month, testDate.Day, testDate.Hour, testDate.Minute, testDate.Second, 128 ) );
		} // MillisecondTest

		
		// members
		private DateTime testDate = new DateTime( 2000, 10, 2, 13, 45, 53, 673 );

	} // class TimeTrimTest

} // namespace Itenso.TimePeriodTests
// -- EOF -------------------------------------------------------------------
