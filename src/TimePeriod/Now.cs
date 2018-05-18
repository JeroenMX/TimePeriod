// -- FILE ------------------------------------------------------------------
// name       : Now.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------
using System;
using System.Globalization;
using TimePeriod.Enums;

namespace TimePeriod
{
	
	public static class Now
	{
		#region Year
		
		public static DateTime CalendarYear => Year( TimeSpec.CalendarYearStartMonth );

	    public static DateTime Year( YearMonth yearStartMonth )
		{
			DateTime now = ClockProxy.Clock.Now;
			int startMonth = (int)yearStartMonth;
			int monthOffset = now.Month - startMonth;
			int year = monthOffset < 0 ? now.Year - 1 : now.Year;
			return new DateTime( year, startMonth, 1 );
		}
		#endregion
		#region Halfyear
		
		public static DateTime CalendarHalfyear => Halfyear( TimeSpec.CalendarYearStartMonth );

	    public static DateTime Halfyear( YearMonth yearStartMonth )
		{
			DateTime now = ClockProxy.Clock.Now;
			int year = now.Year;
			if ( now.Month - (int)yearStartMonth < 0 )
			{
				year--;
			}
			YearHalfyear halfyear = TimeTool.GetHalfyearOfMonth( yearStartMonth, (YearMonth)now.Month );
			int months = ( (int)halfyear - 1 ) * TimeSpec.MonthsPerHalfyear;
			return new DateTime( year, (int)yearStartMonth, 1 ).AddMonths( months );
		}
		#endregion
		#region Quarter
		
		public static DateTime CalendarQuarter => Quarter( TimeSpec.CalendarYearStartMonth );

	    public static DateTime Quarter( YearMonth yearStartMonth )
		{
			DateTime now = ClockProxy.Clock.Now;
			int year = now.Year;
			if ( now.Month - (int)yearStartMonth < 0 )
			{
				year--;
			}
			YearQuarter quarter = TimeTool.GetQuarterOfMonth( yearStartMonth, (YearMonth)now.Month );
			int months = ( (int)quarter - 1 ) * TimeSpec.MonthsPerQuarter;
			return new DateTime( year, (int)yearStartMonth, 1 ).AddMonths( months );
		}
		#endregion
		#region Month
		
		public static DateTime Month => TimeTrim.Day( ClockProxy.Clock.Now );

	    public static YearMonth YearMonth => (YearMonth)ClockProxy.Clock.Now.Month;

	    #endregion
		#region Week
		
		public static DateTime Week()
		{
			return DateTimeFormatInfo.CurrentInfo == null ? DateTime.Now : Week( DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek );
		}
		
		public static DateTime Week( DayOfWeek firstDayOfWeek )
		{
			DateTime currentDay = ClockProxy.Clock.Now;
			while ( currentDay.DayOfWeek != firstDayOfWeek )
			{
				currentDay = currentDay.AddDays( -1 );
			}
			return new DateTime( currentDay.Year, currentDay.Month, currentDay.Day );
		}
		#endregion
		#region Day
		
		public static DateTime Today => ClockProxy.Clock.Now.Date;

	    #endregion
		#region Hour
		
		public static DateTime Hour => TimeTrim.Minute( ClockProxy.Clock.Now );

	    #endregion
		#region Minute
		
		public static DateTime Minute => TimeTrim.Second( ClockProxy.Clock.Now );

	    #endregion
		#region Second
		
		public static DateTime Second => TimeTrim.Millisecond( ClockProxy.Clock.Now );

	    #endregion
	}
}
