// -- FILE ------------------------------------------------------------------
// name       : MonthsTest.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using TimePeriod.Enums;
using Xunit;

namespace TimePeriod.Tests
{

	public sealed class MonthsTest : TestUnitBase
	{

        
        [Trait("Category", "Months")]
        [Fact]
		public void SingleMonthsTest()
		{
			const int startYear = 2004;
			const YearMonth startMonth = YearMonth.June;
			Months months = new Months( startYear, startMonth, 1 );

			Assert.Equal(1, months.MonthCount);
			Assert.Equal( months.StartMonth, startMonth );
			Assert.Equal<int>( months.StartYear, startYear );
			Assert.Equal<int>( months.EndYear, startYear );
			Assert.Equal(YearMonth.June, months.EndMonth);
			Assert.Equal(1, months.GetMonths().Count);
			Assert.True( months.GetMonths()[ 0 ].IsSamePeriod( new Month( 2004, YearMonth.June ) ) );
		} // SingleMonthsTest

        
        [Trait("Category", "Months")]
        [Fact]
		public void CalendarMonthsTest()
		{
			const int startYear = 2004;
			const YearMonth startMonth = YearMonth.November;
			const int monthCount = 5;
			Months months = new Months( startYear, startMonth, monthCount );

			Assert.Equal<int>( months.MonthCount, monthCount );
			Assert.Equal( months.StartMonth, startMonth );
			Assert.Equal<int>( months.StartYear, startYear );
			Assert.Equal(2005, months.EndYear);
			Assert.Equal(YearMonth.March, months.EndMonth);
			Assert.Equal( months.GetMonths().Count, monthCount );
			Assert.True( months.GetMonths()[ 0 ].IsSamePeriod( new Month( 2004, YearMonth.November ) ) );
			Assert.True( months.GetMonths()[ 1 ].IsSamePeriod( new Month( 2004, YearMonth.December ) ) );
			Assert.True( months.GetMonths()[ 2 ].IsSamePeriod( new Month( 2005, YearMonth.January ) ) );
			Assert.True( months.GetMonths()[ 3 ].IsSamePeriod( new Month( 2005, YearMonth.February ) ) );
			Assert.True( months.GetMonths()[ 4 ].IsSamePeriod( new Month( 2005, YearMonth.March ) ) );
		} // CalendarMonthsTest

	} // class MonthsTest

} // namespace Itenso.TimePeriodTests
// -- EOF -------------------------------------------------------------------
