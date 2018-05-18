// -- FILE ------------------------------------------------------------------
// name       : Date.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.08.24
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;

namespace TimePeriod
{
	public struct Date : IComparable, IComparable<Date>, IEquatable<Date>
	{
		public Date( DateTime date )
		{
			this.DateTime = date.Date;
		}

		public Date( int year, int month = 1, int day = 1 )
		{
			if ( year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year )
			{
				throw new ArgumentOutOfRangeException( "year" );
			}
			if ( month <= 0 || month > TimeSpec.MonthsPerYear )
			{
				throw new ArgumentOutOfRangeException( "month" );
			}
			if ( day <= 0 || day > TimeSpec.MaxDaysPerMonth )
			{
				throw new ArgumentOutOfRangeException( "day" );
			}
			DateTime = new DateTime( year, month, day );
		}

		public int Year => DateTime.Year;
		public int Month => DateTime.Month;
		public int Day => DateTime.Day;
        public DateTime DateTime { get; }

        public int CompareTo( Date other )
		{
			return DateTime.CompareTo( other.DateTime );
		}

		public int CompareTo( object obj )
		{
			return DateTime.CompareTo( ((Date)obj).DateTime );
		}

		public bool Equals( Date other )
		{
			return DateTime.Equals( other.DateTime );
		}

		public override string ToString()
		{
			return DateTime.ToString( "d" ); // only the date part
		}

		public override bool Equals( object obj )
		{
			if ( obj == null || GetType() != obj.GetType() )
			{
				return false;
			}

			return Equals( (Date)obj );
		}

		public override int GetHashCode()
		{
			return HashTool.ComputeHashCode( GetType().GetHashCode(), DateTime );
		}

		public static TimeSpan operator -( Date date1, Date date2 )
		{
			return date1.DateTime - date2.DateTime;
		}

		public static Date operator -( Date date, TimeSpan duration )
		{
			return new Date( date.DateTime - duration );
		}

		public static Date operator +( Date date, TimeSpan duration )
		{
			return new Date( date.DateTime + duration );
		}

		public static bool operator <( Date date1, Date date2 )
		{
			return date1.DateTime < date2.DateTime;
		}

		public static bool operator <=( Date date1, Date date2 )
		{
			return date1.DateTime <= date2.DateTime;
		}

		public static bool operator ==( Date left, Date right )
		{
			return Equals( left, right );
		}

		public static bool operator !=( Date left, Date right )
		{
			return !Equals( left, right );
		}

		public static bool operator >( Date date1, Date date2 )
		{
			return date1.DateTime > date2.DateTime;
		}

		public static bool operator >=( Date date1, Date date2 )
		{
			return date1.DateTime >= date2.DateTime;
		}

		public DateTime ToDateTime( Time time )
		{
			return ToDateTime( this, time );
		}

		public DateTime ToDateTime( int hour, int minute = 0, int second = 0, int millisecond = 0 )
		{
			return ToDateTime( this, hour, minute, second, millisecond );
		}

		public static DateTime ToDateTime( Date date, Time time )
		{
			return date.DateTime.Add( time.Duration );
		}

		public static DateTime ToDateTime( Date date, int hour, int minute = 0, int second = 0, int millisecond = 0 )
		{
			return new DateTime( date.Year, date.Month, date.Day, hour, minute, second, millisecond );
		}
    }
}
