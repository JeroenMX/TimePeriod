// -- FILE ------------------------------------------------------------------
// name       : ITimeRange.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2012.03.07
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// --------------------------------------------------------------------------

using TimePeriod.Interfaces;

namespace TimePeriodDemo.Player
{

	// ------------------------------------------------------------------------
	public interface IPlayTimeRange : ITimePeriod
	{

		// ----------------------------------------------------------------------
		PlayDirection Direction { get; set; }

	} // interface ITimeRange

} // namespace Itenso.TimePeriodDemo.Player
// -- EOF -------------------------------------------------------------------
