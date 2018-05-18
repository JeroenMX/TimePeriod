// -- FILE ------------------------------------------------------------------
// name       : TimePeriodCombiner.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.03.22
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using TimePeriod.Interfaces;

namespace TimePeriod
{
    public class TimePeriodCombiner<T>
        where T : ITimePeriod, new()
    {

        public TimePeriodCombiner()
            : this(null)
        { }

        public TimePeriodCombiner(ITimePeriodMapper periodMapper)
        {
            this.PeriodMapper = periodMapper;
        }

        public ITimePeriodMapper PeriodMapper { get; }

        public virtual ITimePeriodCollection CombinePeriods(ITimePeriodContainer periods)
        {
            if (periods == null)
            {
                throw new ArgumentNullException("periods");
            }
            TimeLine<T> timeLine = new TimeLine<T>(periods, PeriodMapper);
            return timeLine.CombinePeriods();
        }
    }
}
