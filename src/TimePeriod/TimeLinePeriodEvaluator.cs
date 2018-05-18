// -- FILE ------------------------------------------------------------------
// name       : TimeLinePeriodEvaluator.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2012.09.04
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using TimePeriod.Interfaces;

namespace TimePeriod
{

    public abstract class TimeLinePeriodEvaluator
    {
        protected TimeLinePeriodEvaluator(ITimePeriodContainer periods, ITimePeriodMapper periodMapper = null)
        {
            if (periods == null)
            {
                throw new ArgumentNullException("periods");
            }

            this.Periods = periods;
            this.PeriodMapper = periodMapper;
        }
        public ITimePeriodContainer Periods { get; }

        public ITimePeriodMapper PeriodMapper { get; }

        protected virtual bool IgnoreEmptyPeriods => false;

        protected virtual void StartEvaluation()
        {
            if (Periods.Count > 0)
            {
                TimeLineMomentCollection timeLineMoments = new TimeLineMomentCollection();
                timeLineMoments.AddAll(Periods);
                if (timeLineMoments.Count > 1)
                {
                    int periodCount = 0;
                    for (int i = 0; i < timeLineMoments.Count - 1; i++)
                    {
                        ITimeLineMoment start = timeLineMoments[i];
                        ITimeLineMoment end = timeLineMoments[i + 1];

                        if (i == 0)
                        {
                            periodCount += start.StartCount;
                            periodCount -= start.EndCount;
                        }

                        ITimePeriod period = new TimeRange(MapPeriodStart(start.Moment), MapPeriodEnd(end.Moment));
                        if (!(IgnoreEmptyPeriods && period.IsMoment))
                        {
                            if (EvaluatePeriod(period, periodCount) == false)
                            {
                                break;
                            }
                        }

                        periodCount += end.StartCount;
                        periodCount -= end.EndCount;
                    }
                }
            }
        }

        protected abstract bool EvaluatePeriod(ITimePeriod period, int periodCount);
        private DateTime MapPeriodStart(DateTime start)
        {
            return PeriodMapper != null ? PeriodMapper.UnmapStart(start) : start;
        }

        private DateTime MapPeriodEnd(DateTime end)
        {
            return PeriodMapper != null ? PeriodMapper.UnmapEnd(end) : end;
        }
    }}
