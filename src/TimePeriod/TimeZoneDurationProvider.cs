// -- FILE ------------------------------------------------------------------
// name       : TimeZoneDurationProvider.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2013.11.03
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;

namespace TimePeriod
{	
	public class TimeZoneDurationProvider : DurationProvider
	{
        public TimeZoneInfo TimeZone { get; }

        public Func<TimeZoneInfo, DateTime, TimeSpan[], DateTime> AmbigiousMoment { get; }

        public Func<TimeZoneInfo, DateTime, DateTime> InvalidMoment { get; }

        public TimeZoneDurationProvider( TimeZoneInfo timeZone = null,
			Func<TimeZoneInfo, DateTime, TimeSpan[], DateTime> ambigiousMoment = null,
			Func<TimeZoneInfo, DateTime, DateTime> invalidMoment = null )
		{
			if ( timeZone == null )
			{
				timeZone = TimeZoneInfo.Local;
			}

			this.TimeZone = timeZone;
			this.AmbigiousMoment = ambigiousMoment;
			this.InvalidMoment = invalidMoment;
		}   
		
	    public override TimeSpan GetDuration( DateTime start, DateTime end )
		{
			if ( TimeZone.SupportsDaylightSavingTime )
			{
				// start
				if ( TimeZone.IsAmbiguousTime( start ) )
				{
					start = OnAmbiguousMoment( start );
				}
				else if ( TimeZone.IsInvalidTime( start ) )
				{
					start = OnInvalidMoment( start );
				}

				// end
				if ( TimeZone.IsAmbiguousTime( end ) )
				{
					end = OnAmbiguousMoment( end );
				}
				else if ( TimeZone.IsInvalidTime( end ) )
				{
					end = OnInvalidMoment( end );
				}
			}

			return base.GetDuration( start, end );
		}
		
		protected virtual DateTime OnAmbiguousMoment( DateTime moment )
		{
			if ( AmbigiousMoment == null )
			{
				throw new AmbiguousMomentException( moment );
			}
			return AmbigiousMoment( TimeZone, moment, TimeZone.GetAmbiguousTimeOffsets( moment ) );
		}
		
		protected virtual DateTime OnInvalidMoment( DateTime moment )
		{
			if ( InvalidMoment == null )
			{
				throw new InvalidMomentException( moment );
			}
			return InvalidMoment( TimeZone, moment );
		}
    }
}

