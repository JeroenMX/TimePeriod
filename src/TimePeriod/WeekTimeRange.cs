// -- FILE ------------------------------------------------------------------
// name       : WeekTimeRange.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using TimePeriod.Interfaces;

namespace TimePeriod
{	
	public abstract class WeekTimeRange : CalendarTimeRange
	{
	    private readonly int _year;
	    private readonly int _startWeek;

        public int Year => _year;
        public int WeekCount { get; }

        public int StartWeek => _startWeek;
	    public int EndWeek => _startWeek + WeekCount - 1;
	    public string StartWeekOfYearName => Calendar.GetWeekOfYearName(Year, StartWeek);
	    public string EndWeekOfYearName => Calendar.GetWeekOfYearName(Year, EndWeek);

        protected WeekTimeRange( int year, int startWeek, int weekCount ) 
            : this( year, startWeek, weekCount, new TimeCalendar() )
		{ }
		
		protected WeekTimeRange( int year, int startWeek, int weekCount, ITimeCalendar calendar ) 
            : base( GetPeriodOf( year, startWeek, weekCount, calendar ), calendar )
		{
			this._year = year;
			this._startWeek = startWeek;
			this.WeekCount = weekCount;
		}
		
		protected WeekTimeRange( DateTime moment, int weekCount ) 
            : this( moment, weekCount, new TimeCalendar() )
		{ }
		
		protected WeekTimeRange( DateTime moment, int weekCount, ITimeCalendar calendar ) 
            : base( GetPeriodOf( moment, weekCount, calendar ), calendar )
		{
			TimeTool.GetWeekOfYear( moment, calendar.Culture, calendar.YearWeekType, out _year, out _startWeek );
			this.WeekCount = weekCount;
		}
		
	    public DateTime GetStartOfWeek( int weekIndex )
		{
			if ( weekIndex < 0 || weekIndex >= WeekCount )
			{
				throw new ArgumentOutOfRangeException( "weekIndex" );
			}

			DateTime startDate = TimeTool.GetStartOfYearWeek( _year, _startWeek, Calendar.Culture, Calendar.YearWeekType );
			return startDate.AddDays( weekIndex * TimeSpec.DaysPerWeek );
		}
		
		public ITimePeriodCollection GetDays()
		{
			TimePeriodCollection days = new TimePeriodCollection();
			DateTime startDate = TimeTool.GetStartOfYearWeek( _year, _startWeek, Calendar.Culture, Calendar.YearWeekType );
			int dayCount = WeekCount * TimeSpec.DaysPerWeek;
			for ( int i = 0; i < dayCount; i++ )
			{
				days.Add( new Day( startDate.AddDays( i ), Calendar ) );
			}
			return days;
		}
		
		protected override bool IsEqual( object obj )
		{
			return base.IsEqual( obj ) && HasSameData( obj as WeekTimeRange );
		}
		
		private bool HasSameData( WeekTimeRange comp )
		{
			return _year == comp._year && _startWeek == comp._startWeek && WeekCount == comp.WeekCount;
		}
		
		protected override int ComputeHashCode()
		{
			return HashTool.ComputeHashCode( base.ComputeHashCode(), _year, _startWeek, WeekCount );
		}
		
		private static TimeRange GetPeriodOf( DateTime moment, int weekCount, ITimeCalendar calendar )
		{
			int year;
			int weekOfYear;
			TimeTool.GetWeekOfYear( moment, calendar.Culture, calendar.YearWeekType, out year, out weekOfYear );
			return GetPeriodOf( year, weekOfYear, weekCount, calendar );
		}
		
		private static TimeRange GetPeriodOf( int year, int weekOfYear, int weekCount, ITimeCalendar calendar )
		{
			if ( weekCount < 1 )
			{
				throw new ArgumentOutOfRangeException( "weekCount" );
			}

			DateTime start = TimeTool.GetStartOfYearWeek( year, weekOfYear, calendar.Culture, calendar.YearWeekType );
			DateTime end = start.AddDays( weekCount * TimeSpec.DaysPerWeek );
			return new TimeRange( start, end );
		}		
    }
}

