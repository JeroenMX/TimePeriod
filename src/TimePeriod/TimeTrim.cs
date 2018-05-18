// -- FILE ------------------------------------------------------------------
// name       : TimeTrim.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;

namespace TimePeriod
{	
	public static class TimeTrim
	{
		public static DateTime Month( DateTime dateTime, int month = 1, int day = 1,
			int hour = 0, int minute = 0, int second = 0, int millisecond = 0 )
		{
			return new DateTime( dateTime.Year, month, day, hour, minute, second, millisecond );
		}
		
		public static DateTime Day( DateTime dateTime, int day = 1,
			int hour = 0, int minute = 0, int second = 0, int millisecond = 0 )
		{
			return new DateTime( dateTime.Year, dateTime.Month, day, hour, minute, second, millisecond );
		}        
		
		public static DateTime Hour( DateTime dateTime, int hour = 0,
			int minute = 0, int second = 0, int millisecond = 0 )
		{
			return new DateTime( dateTime.Year, dateTime.Month, dateTime.Day, hour, minute, second, millisecond );
		}
		
		public static DateTime Minute( DateTime dateTime, int minute = 0, int second = 0, int millisecond = 0 )
		{
			return new DateTime( dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, minute, second, millisecond );
		}
		
		public static DateTime Second( DateTime dateTime, int second = 0, int millisecond = 0 )
		{
			return new DateTime( dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, second, millisecond );
		}
		
		public static DateTime Millisecond( DateTime dateTime, int millisecond = 0 )
		{
			return new DateTime( dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, millisecond );
		}
	}
}
