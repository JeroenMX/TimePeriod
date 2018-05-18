// -- FILE ------------------------------------------------------------------
// name       : TimeGapCalculator.cs
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
    public class TimeGapCalculator<T> where T : ITimePeriod, new()
    {
        public TimeGapCalculator()
            : this(null)
        { }

        public TimeGapCalculator(ITimePeriodMapper periodMapper)
        {
            this.PeriodMapper = periodMapper;
        }

        public ITimePeriodMapper PeriodMapper { get; }

        public virtual ITimePeriodCollection GetGaps(ITimePeriodContainer periods, ITimePeriod limits = null)
        {
            if (periods == null)
            {
                throw new ArgumentNullException("periods");
            }
            TimeLine<T> timeLine = new TimeLine<T>(periods, limits, PeriodMapper);
            return timeLine.CalculateGaps();
        }
    }
}
