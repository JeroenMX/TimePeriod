// -- FILE ------------------------------------------------------------------
// name       : QuarterTest.cs
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
	
	public sealed class QuarterTest : TestUnitBase
	{

        
        [Trait("Category", "Quarter")]
        [Fact]
		public void InitValuesTest()
		{
			DateTime now = ClockProxy.Clock.Now;
			DateTime firstQuarter = new DateTime( now.Year, 1, 1 );
			DateTime secondQuarter = firstQuarter.AddMonths( TimeSpec.MonthsPerQuarter );
			Quarter quarter = new Quarter( now.Year, YearQuarter.First, TimeCalendar.NewEmptyOffset() );

			Assert.Equal( quarter.Start.Year, firstQuarter.Year );
			Assert.Equal( quarter.Start.Month, firstQuarter.Month );
			Assert.Equal( quarter.Start.Day, firstQuarter.Day );
			Assert.Equal(0, quarter.Start.Hour);
			Assert.Equal(0, quarter.Start.Minute);
			Assert.Equal(0, quarter.Start.Second);
			Assert.Equal(0, quarter.Start.Millisecond);

			Assert.Equal( quarter.End.Year, secondQuarter.Year );
			Assert.Equal( quarter.End.Month, secondQuarter.Month );
			Assert.Equal( quarter.End.Day, secondQuarter.Day );
			Assert.Equal(0, quarter.End.Hour);
			Assert.Equal(0, quarter.End.Minute);
			Assert.Equal(0, quarter.End.Second);
			Assert.Equal(0, quarter.End.Millisecond);
		} // InitValuesTest

        
        [Trait("Category", "Quarter")]
        [Fact]
		public void DefaultCalendarTest()
		{
			DateTime yearStart = new DateTime( ClockProxy.Clock.Now.Year, 1, 1 );
			foreach ( YearQuarter yearQuarter in Enum.GetValues( typeof( YearQuarter ) ) )
			{
				int offset = (int)yearQuarter - 1;
				Quarter quarter = new Quarter( yearStart.AddMonths( TimeSpec.MonthsPerQuarter * offset ) );
				Assert.Equal(YearMonth.January, quarter.YearBaseMonth);
				Assert.Equal<int>( quarter.BaseYear, yearStart.Year );
				Assert.Equal<DateTime>( quarter.Start, yearStart.AddMonths( TimeSpec.MonthsPerQuarter * offset ).Add( quarter.Calendar.StartOffset ) );
				Assert.Equal<DateTime>( quarter.End, yearStart.AddMonths( TimeSpec.MonthsPerQuarter * ( offset + 1 ) ).Add( quarter.Calendar.EndOffset ) );
			}
		} // DefaultCalendarTest

        
        [Trait("Category", "Quarter")]
        [Fact]
		public void MomentTest()
		{
			DateTime now = ClockProxy.Clock.Now;
			TimeCalendar calendar = TimeCalendar.New( YearMonth.April );

			Assert.Equal(YearQuarter.First, new Quarter( new DateTime( now.Year, 1, 1 ) ).YearQuarter);
			Assert.Equal(YearQuarter.First, new Quarter( new DateTime( now.Year, 3, 30 ) ).YearQuarter);
			Assert.Equal(YearQuarter.Second, new Quarter( new DateTime( now.Year, 4, 1 ) ).YearQuarter);
			Assert.Equal(YearQuarter.Second, new Quarter( new DateTime( now.Year, 6, 30 ) ).YearQuarter);
			Assert.Equal(YearQuarter.Third, new Quarter( new DateTime( now.Year, 7, 1 ) ).YearQuarter);
			Assert.Equal(YearQuarter.Third, new Quarter( new DateTime( now.Year, 9, 30 ) ).YearQuarter);
			Assert.Equal(YearQuarter.Fourth, new Quarter( new DateTime( now.Year, 10, 1 ) ).YearQuarter);
			Assert.Equal(YearQuarter.Fourth, new Quarter( new DateTime( now.Year, 12, 31 ) ).YearQuarter);

			Assert.Equal(YearQuarter.First, new Quarter( new DateTime( now.Year, 4, 1 ), calendar ).YearQuarter);
			Assert.Equal(YearQuarter.First, new Quarter( new DateTime( now.Year, 6, 30 ), calendar ).YearQuarter);
			Assert.Equal(YearQuarter.Second, new Quarter( new DateTime( now.Year, 7, 1 ), calendar ).YearQuarter);
			Assert.Equal(YearQuarter.Second, new Quarter( new DateTime( now.Year, 9, 30 ), calendar ).YearQuarter);
			Assert.Equal(YearQuarter.Third, new Quarter( new DateTime( now.Year, 10, 1 ), calendar ).YearQuarter);
			Assert.Equal(YearQuarter.Third, new Quarter( new DateTime( now.Year, 12, 31 ), calendar ).YearQuarter);
			Assert.Equal(YearQuarter.Fourth, new Quarter( new DateTime( now.Year, 1, 1 ), calendar ).YearQuarter);
			Assert.Equal(YearQuarter.Fourth, new Quarter( new DateTime( now.Year, 3, 30 ), calendar ).YearQuarter);
		} // MomentTest

        
        [Trait("Category", "Quarter")]
        [Fact]
		public void YearTest()
		{
			int currentYear = ClockProxy.Clock.Now.Year;
			Assert.Equal<int>( new Quarter( currentYear, YearQuarter.Fourth, TimeCalendar.New( YearMonth.April ) ).BaseYear, currentYear );
			Assert.Equal(2006, new Quarter( 2006, YearQuarter.Fourth ).BaseYear);
		} // YearTest

        
        [Trait("Category", "Quarter")]
        [Fact]
		public void YearQuarterTest()
		{
			int currentYear = ClockProxy.Clock.Now.Year;
			Assert.Equal(YearQuarter.Third, new Quarter( currentYear, YearQuarter.Third, TimeCalendar.New( YearMonth.April ) ).YearQuarter);
			Assert.Equal(YearQuarter.Third, new Quarter( currentYear, YearQuarter.Third ).YearQuarter);
		} // YearQuarterTest

        
        [Trait("Category", "Quarter")]
        [Fact]
		public void CurrentQuarterTest()
		{
			int currentYear = ClockProxy.Clock.Now.Year;
			Quarter quarter = new Quarter( currentYear, YearQuarter.First );
			Assert.Equal<int>( quarter.BaseYear, currentYear );
		} // CurrentQuarterTest

        
        [Trait("Category", "Quarter")]
        [Fact]
		public void StartMonthTest()
		{
			int currentYear = ClockProxy.Clock.Now.Year;

			Assert.Equal(YearMonth.January, new Quarter( currentYear, YearQuarter.First, TimeCalendar.New( YearMonth.January ) ).StartMonth);
			Assert.Equal(YearMonth.February, new Quarter( currentYear, YearQuarter.First, TimeCalendar.New( YearMonth.February ) ).StartMonth);
			Assert.Equal(YearMonth.March, new Quarter( currentYear, YearQuarter.First, TimeCalendar.New( YearMonth.March ) ).StartMonth);
			Assert.Equal(YearMonth.April, new Quarter( currentYear, YearQuarter.First, TimeCalendar.New( YearMonth.April ) ).StartMonth);
			Assert.Equal(YearMonth.May, new Quarter( currentYear, YearQuarter.First, TimeCalendar.New( YearMonth.May ) ).StartMonth);
			Assert.Equal(YearMonth.June, new Quarter( currentYear, YearQuarter.First, TimeCalendar.New( YearMonth.June ) ).StartMonth);
			Assert.Equal(YearMonth.July, new Quarter( currentYear, YearQuarter.First, TimeCalendar.New( YearMonth.July ) ).StartMonth);
			Assert.Equal(YearMonth.August, new Quarter( currentYear, YearQuarter.First, TimeCalendar.New( YearMonth.August ) ).StartMonth);
			Assert.Equal(YearMonth.September, new Quarter( currentYear, YearQuarter.First, TimeCalendar.New( YearMonth.September ) ).StartMonth);
			Assert.Equal(YearMonth.October, new Quarter( currentYear, YearQuarter.First, TimeCalendar.New( YearMonth.October ) ).StartMonth);
			Assert.Equal(YearMonth.November, new Quarter( currentYear, YearQuarter.First, TimeCalendar.New( YearMonth.November ) ).StartMonth);
			Assert.Equal(YearMonth.December, new Quarter( currentYear, YearQuarter.First, TimeCalendar.New( YearMonth.December ) ).StartMonth);

			Assert.Equal(YearMonth.April, new Quarter( currentYear, YearQuarter.Second, TimeCalendar.New( YearMonth.January ) ).StartMonth);
			Assert.Equal(YearMonth.May, new Quarter( currentYear, YearQuarter.Second, TimeCalendar.New( YearMonth.February ) ).StartMonth);
			Assert.Equal(YearMonth.June, new Quarter( currentYear, YearQuarter.Second, TimeCalendar.New( YearMonth.March ) ).StartMonth);
			Assert.Equal(YearMonth.July, new Quarter( currentYear, YearQuarter.Second, TimeCalendar.New( YearMonth.April ) ).StartMonth);
			Assert.Equal(YearMonth.August, new Quarter( currentYear, YearQuarter.Second, TimeCalendar.New( YearMonth.May ) ).StartMonth);
			Assert.Equal(YearMonth.September, new Quarter( currentYear, YearQuarter.Second, TimeCalendar.New( YearMonth.June ) ).StartMonth);
			Assert.Equal(YearMonth.October, new Quarter( currentYear, YearQuarter.Second, TimeCalendar.New( YearMonth.July ) ).StartMonth);
			Assert.Equal(YearMonth.November, new Quarter( currentYear, YearQuarter.Second, TimeCalendar.New( YearMonth.August ) ).StartMonth);
			Assert.Equal(YearMonth.December, new Quarter( currentYear, YearQuarter.Second, TimeCalendar.New( YearMonth.September ) ).StartMonth);
			Assert.Equal(YearMonth.January, new Quarter( currentYear, YearQuarter.Second, TimeCalendar.New( YearMonth.October ) ).StartMonth);
			Assert.Equal(YearMonth.February, new Quarter( currentYear, YearQuarter.Second, TimeCalendar.New( YearMonth.November ) ).StartMonth);
			Assert.Equal(YearMonth.March, new Quarter( currentYear, YearQuarter.Second, TimeCalendar.New( YearMonth.December ) ).StartMonth);

			Assert.Equal(YearMonth.July, new Quarter( currentYear, YearQuarter.Third, TimeCalendar.New( YearMonth.January ) ).StartMonth);
			Assert.Equal(YearMonth.August, new Quarter( currentYear, YearQuarter.Third, TimeCalendar.New( YearMonth.February ) ).StartMonth);
			Assert.Equal(YearMonth.September, new Quarter( currentYear, YearQuarter.Third, TimeCalendar.New( YearMonth.March ) ).StartMonth);
			Assert.Equal(YearMonth.October, new Quarter( currentYear, YearQuarter.Third, TimeCalendar.New( YearMonth.April ) ).StartMonth);
			Assert.Equal(YearMonth.November, new Quarter( currentYear, YearQuarter.Third, TimeCalendar.New( YearMonth.May ) ).StartMonth);
			Assert.Equal(YearMonth.December, new Quarter( currentYear, YearQuarter.Third, TimeCalendar.New( YearMonth.June ) ).StartMonth);
			Assert.Equal(YearMonth.January, new Quarter( currentYear, YearQuarter.Third, TimeCalendar.New( YearMonth.July ) ).StartMonth);
			Assert.Equal(YearMonth.February, new Quarter( currentYear, YearQuarter.Third, TimeCalendar.New( YearMonth.August ) ).StartMonth);
			Assert.Equal(YearMonth.March, new Quarter( currentYear, YearQuarter.Third, TimeCalendar.New( YearMonth.September ) ).StartMonth);
			Assert.Equal(YearMonth.April, new Quarter( currentYear, YearQuarter.Third, TimeCalendar.New( YearMonth.October ) ).StartMonth);
			Assert.Equal(YearMonth.May, new Quarter( currentYear, YearQuarter.Third, TimeCalendar.New( YearMonth.November ) ).StartMonth);
			Assert.Equal(YearMonth.June, new Quarter( currentYear, YearQuarter.Third, TimeCalendar.New( YearMonth.December ) ).StartMonth);

			Assert.Equal(YearMonth.October, new Quarter( currentYear, YearQuarter.Fourth, TimeCalendar.New( YearMonth.January ) ).StartMonth);
			Assert.Equal(YearMonth.November, new Quarter( currentYear, YearQuarter.Fourth, TimeCalendar.New( YearMonth.February ) ).StartMonth);
			Assert.Equal(YearMonth.December, new Quarter( currentYear, YearQuarter.Fourth, TimeCalendar.New( YearMonth.March ) ).StartMonth);
			Assert.Equal(YearMonth.January, new Quarter( currentYear, YearQuarter.Fourth, TimeCalendar.New( YearMonth.April ) ).StartMonth);
			Assert.Equal(YearMonth.February, new Quarter( currentYear, YearQuarter.Fourth, TimeCalendar.New( YearMonth.May ) ).StartMonth);
			Assert.Equal(YearMonth.March, new Quarter( currentYear, YearQuarter.Fourth, TimeCalendar.New( YearMonth.June ) ).StartMonth);
			Assert.Equal(YearMonth.April, new Quarter( currentYear, YearQuarter.Fourth, TimeCalendar.New( YearMonth.July ) ).StartMonth);
			Assert.Equal(YearMonth.May, new Quarter( currentYear, YearQuarter.Fourth, TimeCalendar.New( YearMonth.August ) ).StartMonth);
			Assert.Equal(YearMonth.June, new Quarter( currentYear, YearQuarter.Fourth, TimeCalendar.New( YearMonth.September ) ).StartMonth);
			Assert.Equal(YearMonth.July, new Quarter( currentYear, YearQuarter.Fourth, TimeCalendar.New( YearMonth.October ) ).StartMonth);
			Assert.Equal(YearMonth.August, new Quarter( currentYear, YearQuarter.Fourth, TimeCalendar.New( YearMonth.November ) ).StartMonth);
			Assert.Equal(YearMonth.September, new Quarter( currentYear, YearQuarter.Fourth, TimeCalendar.New( YearMonth.December ) ).StartMonth);
		} // StartMonthTest

        
        [Trait("Category", "Quarter")]
        [Fact]
		public void CalendarYearQuarterTest()
		{
			int currentYear = ClockProxy.Clock.Now.Year;
			TimeCalendar calendar = TimeCalendar.New( TimeSpan.Zero, TimeSpan.Zero );

			Quarter current = new Quarter( currentYear, YearQuarter.First );
			Assert.Equal( current.GetNextQuarter().GetNextQuarter().GetNextQuarter().GetNextQuarter().YearQuarter, current.YearQuarter );
			Assert.Equal( current.GetPreviousQuarter().GetPreviousQuarter().GetPreviousQuarter().GetPreviousQuarter().YearQuarter, current.YearQuarter );

			Quarter q1 = new Quarter( currentYear, YearQuarter.First, calendar );
			Assert.True( q1.IsReadOnly );
			Assert.Equal(YearQuarter.First, q1.YearQuarter);
			Assert.Equal<DateTime>( q1.Start, new DateTime( currentYear, TimeSpec.FirstQuarterMonthIndex, 1 ) );
			Assert.Equal<DateTime>( q1.End, new DateTime( currentYear, TimeSpec.SecondQuarterMonthIndex, 1 ) );

			Quarter q2 = new Quarter( currentYear, YearQuarter.Second, calendar );
			Assert.True( q2.IsReadOnly );
			Assert.Equal(YearQuarter.Second, q2.YearQuarter);
			Assert.Equal<DateTime>( q2.Start, new DateTime( currentYear, TimeSpec.SecondQuarterMonthIndex, 1 ) );
			Assert.Equal<DateTime>( q2.End, new DateTime( currentYear, TimeSpec.ThirdQuarterMonthIndex, 1 ) );

			Quarter q3 = new Quarter( currentYear, YearQuarter.Third, calendar );
			Assert.True( q3.IsReadOnly );
			Assert.Equal(YearQuarter.Third, q3.YearQuarter);
			Assert.Equal<DateTime>( q3.Start, new DateTime( currentYear, TimeSpec.ThirdQuarterMonthIndex, 1 ) );
			Assert.Equal<DateTime>( q3.End, new DateTime( currentYear, TimeSpec.FourthQuarterMonthIndex, 1 ) );

			Quarter q4 = new Quarter( currentYear, YearQuarter.Fourth, calendar );
			Assert.True( q4.IsReadOnly );
			Assert.Equal(YearQuarter.Fourth, q4.YearQuarter);
			Assert.Equal<DateTime>( q4.Start, new DateTime( currentYear, TimeSpec.FourthQuarterMonthIndex, 1 ) );
			Assert.Equal<DateTime>( q4.End, new DateTime( currentYear + 1, TimeSpec.FirstQuarterMonthIndex, 1 ) );
		} // CalendarYearQuarterTest

        
        [Trait("Category", "Quarter")]
        [Fact]
		public void QuarterMomentTest()
		{
			Quarter quarter = new Quarter( 2008, YearQuarter.First, TimeCalendar.New( TimeSpan.Zero, TimeSpan.Zero ) );
			Assert.True( quarter.IsReadOnly );
			Assert.Equal(2008, quarter.BaseYear);
			Assert.Equal(YearQuarter.First, quarter.YearQuarter);
			Assert.Equal<DateTime>( quarter.Start, new DateTime( 2008, TimeSpec.FirstQuarterMonthIndex, 1 ) );
			Assert.Equal<DateTime>( quarter.End, new DateTime( 2008, TimeSpec.SecondQuarterMonthIndex, 1 ) );

			Quarter previous = quarter.GetPreviousQuarter();
			Assert.Equal(2007, previous.BaseYear);
			Assert.Equal(YearQuarter.Fourth, previous.YearQuarter);
			Assert.Equal<DateTime>( previous.Start, new DateTime( 2007, TimeSpec.FourthQuarterMonthIndex, 1 ) );
			Assert.Equal<DateTime>( previous.End, new DateTime( 2008, TimeSpec.FirstQuarterMonthIndex, 1 ) );

			Quarter next = quarter.GetNextQuarter();
			Assert.Equal(2008, next.BaseYear);
			Assert.Equal(YearQuarter.Second, next.YearQuarter);
			Assert.Equal<DateTime>( next.Start, new DateTime( 2008, TimeSpec.SecondQuarterMonthIndex, 1 ) );
			Assert.Equal<DateTime>( next.End, new DateTime( 2008, TimeSpec.ThirdQuarterMonthIndex, 1 ) );
		} // QuarterMomentTest

        
        [Trait("Category", "Quarter")]
        [Fact]
		public void IsCalendarQuarterTest()
		{
			int currentYear = ClockProxy.Clock.Now.Year;

			foreach ( YearQuarter yearQuarter in Enum.GetValues( typeof( YearQuarter ) ) )
			{
				Assert.True( new Quarter( currentYear, yearQuarter, TimeCalendar.New( YearMonth.January ) ).IsCalendarQuarter );
				Assert.False( new Quarter( currentYear, yearQuarter, TimeCalendar.New( YearMonth.February ) ).IsCalendarQuarter );
				Assert.False( new Quarter( currentYear, yearQuarter, TimeCalendar.New( YearMonth.March ) ).IsCalendarQuarter );
				Assert.True( new Quarter( currentYear, yearQuarter, TimeCalendar.New( YearMonth.April ) ).IsCalendarQuarter );
				Assert.False( new Quarter( currentYear, yearQuarter, TimeCalendar.New( YearMonth.May ) ).IsCalendarQuarter );
				Assert.False( new Quarter( currentYear, yearQuarter, TimeCalendar.New( YearMonth.June ) ).IsCalendarQuarter );
				Assert.True( new Quarter( currentYear, yearQuarter, TimeCalendar.New( YearMonth.July ) ).IsCalendarQuarter );
				Assert.False( new Quarter( currentYear, yearQuarter, TimeCalendar.New( YearMonth.August ) ).IsCalendarQuarter );
				Assert.False( new Quarter( currentYear, yearQuarter, TimeCalendar.New( YearMonth.September ) ).IsCalendarQuarter );
				Assert.True( new Quarter( currentYear, yearQuarter, TimeCalendar.New( YearMonth.October ) ).IsCalendarQuarter );
				Assert.False( new Quarter( currentYear, yearQuarter, TimeCalendar.New( YearMonth.November ) ).IsCalendarQuarter );
				Assert.False( new Quarter( currentYear, yearQuarter, TimeCalendar.New( YearMonth.December ) ).IsCalendarQuarter );
			}
		} // IsCalendarQuarterTest

        
        [Trait("Category", "Quarter")]
        [Fact]
		public void MultipleCalendarYearsTest()
		{
			DateTime now = ClockProxy.Clock.Now;

			Assert.False( new Quarter( now.Year, YearQuarter.First, TimeCalendar.New( YearMonth.January ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.First, TimeCalendar.New( YearMonth.February ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.First, TimeCalendar.New( YearMonth.March ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.First, TimeCalendar.New( YearMonth.April ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.First, TimeCalendar.New( YearMonth.May ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.First, TimeCalendar.New( YearMonth.June ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.First, TimeCalendar.New( YearMonth.July ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.First, TimeCalendar.New( YearMonth.August ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.First, TimeCalendar.New( YearMonth.September ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.First, TimeCalendar.New( YearMonth.October ) ).MultipleCalendarYears );
			Assert.True( new Quarter( now.Year, YearQuarter.First, TimeCalendar.New( YearMonth.November ) ).MultipleCalendarYears );
			Assert.True( new Quarter( now.Year, YearQuarter.First, TimeCalendar.New( YearMonth.December ) ).MultipleCalendarYears );

			Assert.False( new Quarter( now.Year, YearQuarter.Second, TimeCalendar.New( YearMonth.January ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Second, TimeCalendar.New( YearMonth.February ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Second, TimeCalendar.New( YearMonth.March ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Second, TimeCalendar.New( YearMonth.April ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Second, TimeCalendar.New( YearMonth.May ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Second, TimeCalendar.New( YearMonth.June ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Second, TimeCalendar.New( YearMonth.July ) ).MultipleCalendarYears );
			Assert.True( new Quarter( now.Year, YearQuarter.Second, TimeCalendar.New( YearMonth.August ) ).MultipleCalendarYears );
			Assert.True( new Quarter( now.Year, YearQuarter.Second, TimeCalendar.New( YearMonth.September ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Second, TimeCalendar.New( YearMonth.October ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Second, TimeCalendar.New( YearMonth.November ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Second, TimeCalendar.New( YearMonth.December ) ).MultipleCalendarYears );

			Assert.False( new Quarter( now.Year, YearQuarter.Third, TimeCalendar.New( YearMonth.January ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Third, TimeCalendar.New( YearMonth.February ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Third, TimeCalendar.New( YearMonth.March ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Third, TimeCalendar.New( YearMonth.April ) ).MultipleCalendarYears );
			Assert.True( new Quarter( now.Year, YearQuarter.Third, TimeCalendar.New( YearMonth.May ) ).MultipleCalendarYears );
			Assert.True( new Quarter( now.Year, YearQuarter.Third, TimeCalendar.New( YearMonth.June ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Third, TimeCalendar.New( YearMonth.July ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Third, TimeCalendar.New( YearMonth.August ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Third, TimeCalendar.New( YearMonth.September ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Third, TimeCalendar.New( YearMonth.October ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Third, TimeCalendar.New( YearMonth.November ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Third, TimeCalendar.New( YearMonth.December ) ).MultipleCalendarYears );

			Assert.False( new Quarter( now.Year, YearQuarter.Fourth, TimeCalendar.New( YearMonth.January ) ).MultipleCalendarYears );
			Assert.True( new Quarter( now.Year, YearQuarter.Fourth, TimeCalendar.New( YearMonth.February ) ).MultipleCalendarYears );
			Assert.True( new Quarter( now.Year, YearQuarter.Fourth, TimeCalendar.New( YearMonth.March ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Fourth, TimeCalendar.New( YearMonth.April ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Fourth, TimeCalendar.New( YearMonth.May ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Fourth, TimeCalendar.New( YearMonth.June ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Fourth, TimeCalendar.New( YearMonth.July ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Fourth, TimeCalendar.New( YearMonth.August ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Fourth, TimeCalendar.New( YearMonth.September ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Fourth, TimeCalendar.New( YearMonth.October ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Fourth, TimeCalendar.New( YearMonth.November ) ).MultipleCalendarYears );
			Assert.False( new Quarter( now.Year, YearQuarter.Fourth, TimeCalendar.New( YearMonth.December ) ).MultipleCalendarYears );
		} // MultipleCalendarYearsTest

        
        [Trait("Category", "Quarter")]
        [Fact]
		public void DefaultQuarterTest()
		{
			int currentYear = ClockProxy.Clock.Now.Year;
			const YearMonth yearStartMonth = YearMonth.April;
			TimeCalendar calendar = TimeCalendar.New( TimeSpan.Zero, TimeSpan.Zero, yearStartMonth );

			Quarter q1 = new Quarter( currentYear, YearQuarter.First, calendar );
			Assert.True( q1.IsReadOnly );
			Assert.Equal( q1.YearBaseMonth, yearStartMonth );
			Assert.Equal(YearQuarter.First, q1.YearQuarter);
			Assert.Equal<int>( q1.BaseYear, currentYear );
			Assert.Equal<DateTime>( q1.Start, new DateTime( currentYear, 4, 1 ) );
			Assert.Equal<DateTime>( q1.End, new DateTime( currentYear, 7, 1 ) );

			Quarter q2 = new Quarter( currentYear, YearQuarter.Second, calendar );
			Assert.True( q2.IsReadOnly );
			Assert.Equal( q2.YearBaseMonth, yearStartMonth );
			Assert.Equal(YearQuarter.Second, q2.YearQuarter);
			Assert.Equal<int>( q2.BaseYear, currentYear );
			Assert.Equal<DateTime>( q2.Start, new DateTime( currentYear, 7, 1 ) );
			Assert.Equal<DateTime>( q2.End, new DateTime( currentYear, 10, 1 ) );

			Quarter q3 = new Quarter( currentYear, YearQuarter.Third, calendar );
			Assert.True( q3.IsReadOnly );
			Assert.Equal( q3.YearBaseMonth, yearStartMonth );
			Assert.Equal(YearQuarter.Third, q3.YearQuarter);
			Assert.Equal<int>( q3.BaseYear, currentYear );
			Assert.Equal<DateTime>( q3.Start, new DateTime( currentYear, 10, 1 ) );
			Assert.Equal<DateTime>( q3.End, new DateTime( currentYear + 1, 1, 1 ) );

			Quarter q4 = new Quarter( currentYear, YearQuarter.Fourth, calendar );
			Assert.True( q4.IsReadOnly );
			Assert.Equal( q4.YearBaseMonth, yearStartMonth );
			Assert.Equal(YearQuarter.Fourth, q4.YearQuarter);
			Assert.Equal<int>( q4.BaseYear, currentYear );
			Assert.Equal<DateTime>( q4.Start, new DateTime( currentYear + 1, 1, 1 ) );
			Assert.Equal<DateTime>( q4.End, new DateTime( currentYear + 1, 4, 1 ) );
		} // DefaultQuarterTest

        
        [Trait("Category", "Quarter")]
        [Fact]
		public void GetMonthsTest()
		{
			Quarter quarter = new Quarter( ClockProxy.Clock.Now.Year, YearQuarter.First, TimeCalendar.New( YearMonth.October ) );

			ITimePeriodCollection months = quarter.GetMonths();
			Assert.NotNull(months);

			int index = 0;
			foreach ( Month month in months )
			{
				Assert.Equal( month.Start, quarter.Start.AddMonths( index ) );
				Assert.Equal( month.End, month.Calendar.MapEnd( month.Start.AddMonths( 1 ) ) );
				index++;
			}
			Assert.Equal( index, TimeSpec.MonthsPerQuarter );
		} // GetMonthsTest

        
        [Trait("Category", "Quarter")]
        [Fact]
		public void AddQuartersTest()
		{
			int currentYear = ClockProxy.Clock.Now.Year;
			const YearMonth yearStartMonth = YearMonth.April;
			TimeCalendar calendar = TimeCalendar.New( TimeSpan.Zero, TimeSpan.Zero, yearStartMonth );

			DateTime calendarStartDate = new DateTime( currentYear, 4, 1 );
			Quarter calendarQuarter = new Quarter( currentYear, YearQuarter.First, calendar );

			Assert.Equal( calendarQuarter.AddQuarters( 0 ), calendarQuarter );

			Quarter prevQ1 = calendarQuarter.AddQuarters( -1 );
			Assert.Equal(YearQuarter.Fourth, prevQ1.YearQuarter);
			Assert.Equal<int>( prevQ1.BaseYear, currentYear - 1 );
			Assert.Equal<DateTime>( prevQ1.Start, calendarStartDate.AddMonths( -3 ) );
			Assert.Equal<DateTime>( prevQ1.End, calendarStartDate );

			Quarter prevQ2 = calendarQuarter.AddQuarters( -2 );
			Assert.Equal(YearQuarter.Third, prevQ2.YearQuarter);
			Assert.Equal<int>( prevQ2.BaseYear, currentYear - 1 );
			Assert.Equal<DateTime>( prevQ2.Start, calendarStartDate.AddMonths( -6 ) );
			Assert.Equal<DateTime>( prevQ2.End, calendarStartDate.AddMonths( -3 ) );

			Quarter prevQ3 = calendarQuarter.AddQuarters( -3 );
			Assert.Equal(YearQuarter.Second, prevQ3.YearQuarter);
			Assert.Equal<int>( prevQ3.BaseYear, currentYear - 1 );
			Assert.Equal<DateTime>( prevQ3.Start, calendarStartDate.AddMonths( -9 ) );
			Assert.Equal<DateTime>( prevQ3.End, calendarStartDate.AddMonths( -6 ) );

			Quarter prevQ4 = calendarQuarter.AddQuarters( -4 );
			Assert.Equal(YearQuarter.First, prevQ4.YearQuarter);
			Assert.Equal<int>( prevQ4.BaseYear, currentYear - 1 );
			Assert.Equal<DateTime>( prevQ4.Start, calendarStartDate.AddMonths( -12 ) );
			Assert.Equal<DateTime>( prevQ4.End, calendarStartDate.AddMonths( -9 ) );

			Quarter prevQ5 = calendarQuarter.AddQuarters( -5 );
			Assert.Equal(YearQuarter.Fourth, prevQ5.YearQuarter);
			Assert.Equal<int>( prevQ5.BaseYear, currentYear - 2 );
			Assert.Equal<DateTime>( prevQ5.Start, calendarStartDate.AddMonths( -15 ) );
			Assert.Equal<DateTime>( prevQ5.End, calendarStartDate.AddMonths( -12 ) );

			Quarter futureQ1 = calendarQuarter.AddQuarters( 1 );
			Assert.Equal(YearQuarter.Second, futureQ1.YearQuarter);
			Assert.Equal<int>( futureQ1.BaseYear, currentYear );
			Assert.Equal<DateTime>( futureQ1.Start, calendarStartDate.AddMonths( 3 ) );
			Assert.Equal<DateTime>( futureQ1.End, calendarStartDate.AddMonths( 6 ) );

			Quarter futureQ2 = calendarQuarter.AddQuarters( 2 );
			Assert.Equal(YearQuarter.Third, futureQ2.YearQuarter);
			Assert.Equal<int>( futureQ2.BaseYear, currentYear );
			Assert.Equal<DateTime>( futureQ2.Start, calendarStartDate.AddMonths( 6 ) );
			Assert.Equal<DateTime>( futureQ2.End, calendarStartDate.AddMonths( 9 ) );

			Quarter futureQ3 = calendarQuarter.AddQuarters( 3 );
			Assert.Equal(YearQuarter.Fourth, futureQ3.YearQuarter);
			Assert.Equal<int>( futureQ3.BaseYear, currentYear );
			Assert.Equal<DateTime>( futureQ3.Start, calendarStartDate.AddMonths( 9 ) );
			Assert.Equal<DateTime>( futureQ3.End, calendarStartDate.AddMonths( 12 ) );

			Quarter futureQ4 = calendarQuarter.AddQuarters( 4 );
			Assert.Equal(YearQuarter.First, futureQ4.YearQuarter);
			Assert.Equal<int>( futureQ4.BaseYear, currentYear + 1 );
			Assert.Equal<DateTime>( futureQ4.Start, calendarStartDate.AddMonths( 12 ) );
			Assert.Equal<DateTime>( futureQ4.End, calendarStartDate.AddMonths( 15 ) );

			Quarter futureQ5 = calendarQuarter.AddQuarters( 5 );
			Assert.Equal(YearQuarter.Second, futureQ5.YearQuarter);
			Assert.Equal<int>( futureQ5.BaseYear, currentYear + 1 );
			Assert.Equal<DateTime>( futureQ5.Start, calendarStartDate.AddMonths( 15 ) );
			Assert.Equal<DateTime>( futureQ5.End, calendarStartDate.AddMonths( 18 ) );
		} // AddQuartersTest

		
		private ITimeCalendar GetFiscalYearCalendar( FiscalYearAlignment yearAlignment )
		{
			return new TimeCalendar(
				new TimeCalendarConfig
				{
					YearType = YearType.FiscalYear,
					YearBaseMonth = YearMonth.September,
					FiscalFirstDayOfYear = DayOfWeek.Sunday,
					FiscalYearAlignment = yearAlignment,
					FiscalQuarterGrouping = FiscalQuarterGrouping.FourFourFiveWeeks
				} );
		} // GetFiscalYearCalendar

        
        // http://en.wikipedia.org/wiki/4-4-5_Calendar
        [Trait("Category", "Quarter")]
        [Fact]
		public void FiscalYearGetMonthsTest()
		{
			Quarter quarter = new Quarter( 2006, YearQuarter.First, GetFiscalYearCalendar( FiscalYearAlignment.LastDay ) );
			ITimePeriodCollection months = quarter.GetMonths();
			Assert.NotNull(months);
			Assert.Equal( months.Count, TimeSpec.MonthsPerQuarter );

			Assert.Equal<DateTime>( months[ 0 ].Start, new DateTime( 2006, 8, 27 ) );
			for ( int i = 0; i < months.Count; i++ )
			{
				Assert.Equal( months[ i ].Duration.Subtract( TimeCalendar.DefaultEndOffset ).Days,
					( i + 1 ) % 3 == 0 ? TimeSpec.FiscalDaysPerLongMonth : TimeSpec.FiscalDaysPerShortMonth );
			}
			Assert.Equal( months[ TimeSpec.MonthsPerQuarter - 1 ].End, quarter.End );
		} // FiscalYearGetMonthsTest

	} // class QuarterTest

} // namespace Itenso.TimePeriodTests
// -- EOF -------------------------------------------------------------------
