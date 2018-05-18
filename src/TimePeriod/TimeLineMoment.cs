// -- FILE ------------------------------------------------------------------
// name       : TimeLineMoment.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.03.31
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using TimePeriod.Interfaces;

namespace TimePeriod
{	
	public class TimeLineMoment : ITimeLineMoment
	{		
		public TimeLineMoment( DateTime moment )
		{
			this.Moment = moment;
		}
		
		public DateTime Moment { get; }
	    public int BalanceCount => StartCount - EndCount;
        public int StartCount { get; private set; }

        public int EndCount { get; private set; }

        public bool IsEmpty => StartCount == 0 && EndCount == 0;

	    public void AddStart()
		{
			StartCount++;
		}
		
		public void RemoveStart()
		{
			if ( StartCount == 0 )
			{
				throw new InvalidOperationException();
			}
			StartCount--;
		}
		
		public void AddEnd()
		{
			EndCount++;
		}
		
		public void RemoveEnd()
		{
			if ( EndCount == 0 )
			{
				throw new InvalidOperationException();
			}
			EndCount--;
		}
		
		public override string ToString()
		{
			return string.Format( "{0} [{1}/{2}]", Moment, StartCount, EndCount );
		}
    }
}

