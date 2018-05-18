// -- FILE ------------------------------------------------------------------
// name       : YearTimeRange.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using TimePeriod.Enums;
using TimePeriod.Interfaces;

namespace TimePeriod
{	
	public abstract class YearTimeRange : CalendarTimeRange
	{
	    private readonly int _startYear;
	    private readonly int _endYear; // cache

	    public int YearCount { get; }
	    public override int BaseYear => _startYear;
	    public int StartYear => Calendar.GetYear(_startYear, (int)YearBaseMonth);
	    public int EndYear => Calendar.GetYear(_endYear, (int)YearBaseMonth);
	    public string StartYearName => Calendar.GetYearName(StartYear);
	    public string EndYearName => Calendar.GetYearName(StartYear + YearCount - 1);


        protected YearTimeRange( int startYear, int yearCount, ITimeCalendar calendar ) :
			base( GetPeriodOf( calendar, startYear, yearCount ), calendar )
		{
			this._startYear = startYear;
			this.YearCount = yearCount;
			_endYear = End.Year;
		}        

	    public ITimePeriodCollection GetHalfyears()
		{
			TimePeriodCollection halfyears = new TimePeriodCollection();
			for ( int i = 0; i < YearCount; i++ )
			{
				for ( int halfyear = 0; halfyear < TimeSpec.HalfyearsPerYear; halfyear++ )
				{
					int year;
					YearHalfyear yearHalfyear;
					TimeTool.AddHalfyear( _startYear, YearHalfyear.First, ( i * TimeSpec.HalfyearsPerYear ) + halfyear, out year, out yearHalfyear );
					halfyears.Add( new Halfyear( year, yearHalfyear, Calendar ) );
				}
			}
			return halfyears;
		}
		
		public ITimePeriodCollection GetQuarters()
		{
			TimePeriodCollection quarters = new TimePeriodCollection();
			for ( int i = 0; i < YearCount; i++ )
			{
				for ( int quarter = 0; quarter < TimeSpec.QuartersPerYear; quarter++ )
				{
					int year;
					YearQuarter yearQuarter;
					TimeTool.AddQuarter( _startYear, YearQuarter.First, ( i * TimeSpec.QuartersPerYear ) + quarter, out year, out yearQuarter );
					quarters.Add( new Quarter( year, yearQuarter, Calendar ) );
				}
			}
			return quarters;
		}
		
		public ITimePeriodCollection GetMonths()
		{
			TimePeriodCollection months = new TimePeriodCollection();
			for ( int i = 0; i < YearCount; i++ )
			{
				for ( int month = 0; month < TimeSpec.MonthsPerYear; month++ )
				{
					int year;
					YearMonth yearMonth;
					TimeTool.AddMonth( _startYear, YearBaseMonth, ( i * TimeSpec.MonthsPerYear ) + month, out year, out yearMonth );
					months.Add( new Month( year, yearMonth, Calendar ) );
				}
			}
			return months;
		}
		
		protected override bool IsEqual( object obj )
		{
			return base.IsEqual( obj ) && HasSameData( obj as YearTimeRange );
		}
		
		private bool HasSameData( YearTimeRange comp )
		{
			return
				_startYear == comp._startYear &&
				_endYear == comp._endYear &&
				YearCount == comp.YearCount;
		}
		
		protected override int ComputeHashCode()
		{
			return HashTool.ComputeHashCode( base.ComputeHashCode(), _startYear, _startYear, YearCount );
		}
		
		private static DateTime GetStartOfYear( ITimeCalendar calendar, int year )
		{
			DateTime startOfYear;

			switch ( calendar.YearType )
			{
				case YearType.FiscalYear:
					startOfYear = FiscalCalendarTool.GetStartOfYear( year, calendar.YearBaseMonth,
						calendar.FiscalFirstDayOfYear, calendar.FiscalYearAlignment );
					break;
				default:
					startOfYear = new DateTime( year, (int)calendar.YearBaseMonth, 1 );
					break;
			}
			return startOfYear;
		}
		
		private static TimeRange GetPeriodOf( ITimeCalendar calendar, int year, int yearCount )
		{
			if ( yearCount < 1 )
			{
				throw new ArgumentOutOfRangeException( "yearCount" );
			}

			DateTime start = GetStartOfYear( calendar, year );
			DateTime end = GetStartOfYear( calendar, year + yearCount );
			return new TimeRange( start, end );
		}		
	}
}

