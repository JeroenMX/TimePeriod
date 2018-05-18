// -- FILE ------------------------------------------------------------------
// name       : TimePeriodSubtractor.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.09.30
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using TimePeriod.Interfaces;

namespace TimePeriod
{
    public class TimePeriodSubtractor<T> where T : ITimePeriod, new()
    {
        private readonly TimePeriodCombiner<T> _timePeriodCombiner;
        private readonly TimeGapCalculator<T> _timeGapCalculator;
        private readonly TimePeriodIntersector<T> _timePeriodIntersector;

        public TimePeriodSubtractor() 
            : this(null)
        { }

        public TimePeriodSubtractor(ITimePeriodMapper periodMapper)
        {
            this.PeriodMapper = periodMapper;
            _timePeriodCombiner = new TimePeriodCombiner<T>(periodMapper);
            _timeGapCalculator = new TimeGapCalculator<T>(periodMapper);
            _timePeriodIntersector = new TimePeriodIntersector<T>(periodMapper);
        }
        public ITimePeriodMapper PeriodMapper { get; }

        public virtual ITimePeriodCollection SubtractPeriods(ITimePeriodContainer sourcePeriods, ITimePeriodCollection subtractingPeriods,
            bool combinePeriods = true)
        {
            if (sourcePeriods == null)
            {
                throw new ArgumentNullException("sourcePeriods");
            }
            if (subtractingPeriods == null)
            {
                throw new ArgumentNullException("subtractingPeriods");
            }

            if (sourcePeriods.Count == 0)
            {
                return new TimePeriodCollection();
            }

            if (subtractingPeriods.Count == 0 && !combinePeriods)
            {
                return new TimePeriodCollection(sourcePeriods);
            }

            // combined source periods
            sourcePeriods = _timePeriodCombiner.CombinePeriods(sourcePeriods);

            // combined subtracting periods
            if (subtractingPeriods.Count == 0)
            {
                return new TimePeriodCollection(sourcePeriods);
            }
            subtractingPeriods = _timePeriodCombiner.CombinePeriods(subtractingPeriods);

            // invert subtracting periods
            sourcePeriods.AddAll(_timeGapCalculator.GetGaps(subtractingPeriods, new TimeRange(sourcePeriods.Start, sourcePeriods.End)));

            return _timePeriodIntersector.IntersectPeriods(sourcePeriods, combinePeriods);
        }
    }
}

