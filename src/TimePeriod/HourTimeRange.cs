// -- FILE ------------------------------------------------------------------
// name       : HourTimeRange.cs
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
	public abstract class HourTimeRange : CalendarTimeRange
	{		
		protected HourTimeRange( int startYear, int startMonth, int startDay, int startHour, int hourCount ) 
            : this( startYear, startMonth, startDay, startHour, hourCount, new TimeCalendar() )
		{ }
		
		protected HourTimeRange( int startYear, int startMonth, int startDay, int startHour, int hourCount, ITimeCalendar calendar ) :
			base( GetPeriodOf( startYear, startMonth, startDay, startHour, hourCount ), calendar )
		{
			this.startHour = new DateTime( startYear, startMonth, startDay, startHour, 0, 0 );
			this.hourCount = hourCount;
			endHour = this.startHour.AddHours( hourCount );
		}
		
		public int StartYear
		{
			get { return startHour.Year; }
		}
		
		public int StartMonth
		{
			get { return startHour.Month; }
		}
		
		public int StartDay
		{
			get { return startHour.Day; }
		}
		
		public int StartHour
		{
			get { return startHour.Hour; }
		}
		
		public int EndYear
		{
			get { return endHour.Year; }
		}
		
		public int EndMonth
		{
			get { return endHour.Month; }
		}
		
		public int EndDay
		{
			get { return endHour.Day; }
		}
		
		public int EndHour
		{
			get { return endHour.Hour; }
		}
		
		public int HourCount
		{
			get { return hourCount; }
		}
		
		public ITimePeriodCollection GetMinutes()
		{
			TimePeriodCollection minutes = new TimePeriodCollection();
			for ( int hour = 0; hour < hourCount; hour++ )
			{
				DateTime curHour = startHour.AddHours( hour );
				for ( int minute = 0; minute < TimeSpec.MinutesPerHour; minute++ )
				{
					minutes.Add( new Minute( curHour.AddMinutes( minute ), Calendar ) );
				}
			}
			return minutes;
		}
		
		protected override bool IsEqual( object obj )
		{
			return base.IsEqual( obj ) && HasSameData( obj as HourTimeRange );
		}
		
		private bool HasSameData( HourTimeRange comp )
		{
			return startHour == comp.startHour && hourCount == comp.hourCount && endHour == comp.endHour;
		}
		
		protected override int ComputeHashCode()
		{
			return HashTool.ComputeHashCode( base.ComputeHashCode(), startHour, hourCount, endHour );
		}
		
		private static TimeRange GetPeriodOf( int year, int month, int day, int hour, int hourCount )
		{
			if ( hourCount < 1 )
			{
				throw new ArgumentOutOfRangeException( "hourCount" );
			}
			DateTime start = new DateTime( year, month, day, hour, 0, 0 );
			DateTime end = start.AddHours( hourCount );
			return new TimeRange( start, end );
		}
		
		// members
		private readonly DateTime startHour;
		private readonly int hourCount;
		private readonly DateTime endHour; // cache
	}
}
