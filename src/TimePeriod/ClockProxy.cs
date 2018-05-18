// -- FILE ------------------------------------------------------------------
// name       : ClockProxy.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using TimePeriod.Interfaces;

namespace TimePeriod
{

	public static class ClockProxy
	{
	    private static readonly object Mutex = new object();
	    private static volatile IClock _clock;

        public static IClock Clock
		{
			get
			{
				if ( _clock == null )
				{
					lock ( Mutex )
					{
						if ( _clock == null )
						{
							_clock = new SystemClock();
						}
					}
				}
				return _clock;
			}
			set
			{
				lock ( Mutex )
				{
					_clock = value;
				}
			}
		}	
	}
}
