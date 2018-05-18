// -- FILE ------------------------------------------------------------------
// name       : Strings.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Resources;
using TimePeriod.Enums;

namespace TimePeriod
{	
	internal sealed class Strings
	{
	    private static readonly ResourceManager Inst = NewInst(typeof(Strings));

	    #region Year

        public static string SystemYearName( int year )
		{
			return Format( Inst.GetString( "SystemYearName" ), year );
		}
		
		public static string CalendarYearName( int year )
		{
			return Format( Inst.GetString( "CalendarYearName" ), year );
		}
		
		
		public static string FiscalYearName( int year )
		{
			return Format( Inst.GetString( "FiscalYearName" ), year );
		}
		
		
		public static string SchoolYearName( int year )
		{
			return Format( Inst.GetString( "SchoolYearName" ), year );
		}
		#endregion
		#region Halfyear
		
		public static string SystemHalfyearName( YearHalfyear yearHalfyear )
		{
			return Format( Inst.GetString( "SystemHalfyearName" ), (int)yearHalfyear );
		}
		
		public static string CalendarHalfyearName( YearHalfyear yearHalfyear )
		{
			return Format( Inst.GetString( "CalendarHalfyearName" ), (int)yearHalfyear );
		}
		
		
		public static string FiscalHalfyearName( YearHalfyear yearHalfyear )
		{
			return Format( Inst.GetString( "FiscalHalfyearName" ), (int)yearHalfyear );
		}
		
		
		public static string SchoolHalfyearName( YearHalfyear yearHalfyear )
		{
			return Format( Inst.GetString( "SchoolHalfyearName" ), (int)yearHalfyear );
		}
		
		public static string SystemHalfyearOfYearName( YearHalfyear yearHalfyear, int year )
		{
			return Format( Inst.GetString( "SystemHalfyearOfYearName" ), (int)yearHalfyear, year );
		}
		
		public static string CalendarHalfyearOfYearName( YearHalfyear yearHalfyear, int year )
		{
			return Format( Inst.GetString( "CalendarHalfyearOfYearName" ), (int)yearHalfyear, year );
		}
		
		
		public static string FiscalHalfyearOfYearName( YearHalfyear yearHalfyear, int year )
		{
			return Format( Inst.GetString( "FiscalHalfyearOfYearName" ), (int)yearHalfyear, year );
		}
		
		
		public static string SchoolHalfyearOfYearName( YearHalfyear yearHalfyear, int year )
		{
			return Format( Inst.GetString( "SchoolHalfyearOfYearName" ), (int)yearHalfyear, year );
		}
		#endregion
		#region Quarter
		
		public static string SystemQuarterName( YearQuarter yearQuarter )
		{
			return Format( Inst.GetString( "SystemQuarterName" ), (int)yearQuarter );
		}
		
		public static string CalendarQuarterName( YearQuarter yearQuarter )
		{
			return Format( Inst.GetString( "CalendarQuarterName" ), (int)yearQuarter );
		}
		
		
		public static string FiscalQuarterName( YearQuarter yearQuarter )
		{
			return Format( Inst.GetString( "FiscalQuarterName" ), (int)yearQuarter );
		}
		
		
		public static string SchoolQuarterName( YearQuarter yearQuarter )
		{
			return Format( Inst.GetString( "SchoolQuarterName" ), (int)yearQuarter );
		}
		
		public static string SystemQuarterOfYearName( YearQuarter yearQuarter, int year )
		{
			return Format( Inst.GetString( "SystemQuarterOfYearName" ), (int)yearQuarter, year );
		}
		
		public static string CalendarQuarterOfYearName( YearQuarter yearQuarter, int year )
		{
			return Format( Inst.GetString( "CalendarQuarterOfYearName" ), (int)yearQuarter, year );
		}
		
		
		public static string FiscalQuarterOfYearName( YearQuarter yearQuarter, int year )
		{
			return Format( Inst.GetString( "FiscalQuarterOfYearName" ), (int)yearQuarter, year );
		}
		
		
		public static string SchoolQuarterOfYearName( YearQuarter yearQuarter, int year )
		{
			return Format( Inst.GetString( "SchoolQuarterOfYearName" ), (int)yearQuarter, year );
		}
		#endregion
		#region Month
		
		public static string MonthOfYearName( string monthName, string yearName )
		{
			return Format( Inst.GetString( "MonthOfYearName" ), monthName, yearName );
		}
		#endregion
		#region Wek
		
		public static string WeekOfYearName( int weekOfYear, string yearName )
		{
			return Format( Inst.GetString( "WeekOfYearName" ), weekOfYear, yearName );
		}
		#endregion
		#region Time Formatter
		
		public static string TimeSpanYears => Inst.GetString( "TimeSpanYears" );
	    public static string TimeSpanYear => Inst.GetString( "TimeSpanYear" );
	    public static string TimeSpanMonths => Inst.GetString( "TimeSpanMonths" );
	    public static string TimeSpanMonth => Inst.GetString( "TimeSpanMonth" );
	    public static string TimeSpanWeeks => Inst.GetString( "TimeSpanWeeks" );
	    public static string TimeSpanWeek => Inst.GetString( "TimeSpanWeek" );
	    public static string TimeSpanDays => Inst.GetString( "TimeSpanDays" );
	    public static string TimeSpanDay => Inst.GetString( "TimeSpanDay" );
	    public static string TimeSpanHours => Inst.GetString( "TimeSpanHours" );
	    public static string TimeSpanHour => Inst.GetString( "TimeSpanHour" );
	    public static string TimeSpanMinutes => Inst.GetString( "TimeSpanMinutes" );
	    public static string TimeSpanMinute => Inst.GetString( "TimeSpanMinute" );
	    public static string TimeSpanSeconds => Inst.GetString( "TimeSpanSeconds" );
	    public static string TimeSpanSecond => Inst.GetString( "TimeSpanSecond" );

	    #endregion
		
		private static string Format( string format, params object[] args )
		{
			return string.Format( CultureInfo.InvariantCulture, format, args );
		}
		
		private static ResourceManager NewInst( Type singletonType )
		{
			if ( singletonType == null )
			{
				throw new ArgumentNullException( "singletonType" );
			}
			if ( singletonType.FullName == null )
			{
				throw new InvalidOperationException();
			}
            return new ResourceManager(singletonType);
        }
                       
	}
}
