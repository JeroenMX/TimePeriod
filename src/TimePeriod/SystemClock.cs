// -- FILE ------------------------------------------------------------------
// name       : SystemClock.cs
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
	public class SystemClock : IClock
	{		
		internal SystemClock()
		{ }
		
		public DateTime Now => DateTime.Now;
	}
}
