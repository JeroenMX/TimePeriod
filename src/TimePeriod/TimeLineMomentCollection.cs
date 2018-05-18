// -- FILE ------------------------------------------------------------------
// name       : TimeLineMomentCollection.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.03.31
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using TimePeriod.Interfaces;

namespace TimePeriod
{

    public class TimeLineMomentCollection : ITimeLineMomentCollection
    {
        private readonly List<ITimeLineMoment> _timeLineMoments = new List<ITimeLineMoment>();
        private readonly Dictionary<DateTime, ITimeLineMoment> _timeLineMomentLookup = new Dictionary<DateTime, ITimeLineMoment>();

        public TimeLineMomentCollection()
        {
        }

        public TimeLineMomentCollection(IEnumerable<ITimePeriod> periods)
        {
            AddAll(periods);
        }

        public int Count => _timeLineMoments.Count;
        public bool IsEmpty => Count == 0;
        public ITimeLineMoment Min => !IsEmpty ? _timeLineMoments[0] : null;
        public ITimeLineMoment Max => !IsEmpty ? _timeLineMoments[Count - 1] : null;
        public ITimeLineMoment this[int index] => _timeLineMoments[index];
        public ITimeLineMoment this[DateTime moment] => _timeLineMomentLookup[moment];
        protected IList<ITimeLineMoment> Moments => _timeLineMoments;

        public void Clear()
        {
            _timeLineMoments.Clear();
            _timeLineMomentLookup.Clear();
        }

        public void Add(ITimePeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException("period");
            }
            AddStart(period.Start);
            AddEnd(period.End);
            Sort();
        }

        public void AddAll(IEnumerable<ITimePeriod> periods)
        {
            if (periods == null)
            {
                throw new ArgumentNullException("periods");
            }

            foreach (ITimePeriod period in periods)
            {
                AddStart(period.Start);
                AddEnd(period.End);
            }
            Sort();
        }

        public void Remove(ITimePeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException("period");
            }

            RemoveStart(period.Start);
            RemoveEnd(period.End);
            Sort();
        }

        public ITimeLineMoment Find(DateTime moment)
        {
            ITimeLineMoment timeLineMoment = null;
            if (Count > 0)
            {
                _timeLineMomentLookup.TryGetValue(moment, out timeLineMoment);
            }
            return timeLineMoment;
        }

        public bool Contains(DateTime moment)
        {
            return _timeLineMomentLookup.ContainsKey(moment);
        }

        public bool HasOverlaps()
        {
            bool hasOverlaps = false;

            if (Count > 1)
            {
                bool start = true;
                foreach (ITimeLineMoment timeLineMoment in _timeLineMoments)
                {
                    int startCount = timeLineMoment.StartCount;
                    int endCount = timeLineMoment.EndCount;
                    if (start)
                    {
                        if (startCount != 1 || endCount > 1)
                        {
                            hasOverlaps = true;
                            break;
                        }
                    }
                    else // end
                    {
                        if (startCount > 1 || endCount != 1)
                        {
                            hasOverlaps = true;
                            break;
                        }
                    }
                    start = (endCount - startCount) > 0;
                }
            }

            return hasOverlaps;
        }

        public bool HasGaps()
        {
            bool hasGaps = false;

            if (Count > 1)
            {
                int momentCount = 0;
                for (int index = 0; index < _timeLineMoments.Count; index++)
                {
                    ITimeLineMoment timeLineMoment = _timeLineMoments[index];
                    momentCount += timeLineMoment.StartCount;
                    momentCount -= timeLineMoment.EndCount;
                    if (momentCount == 0 && index > 0 && index < _timeLineMoments.Count - 1)
                    {
                        hasGaps = true;
                        break;
                    }
                }
            }

            return hasGaps;
        }

        public IEnumerator<ITimeLineMoment> GetEnumerator()
        {
            return _timeLineMoments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected virtual void AddStart(DateTime moment)
        {
            ITimeLineMoment timeLineMoment = Find(moment);
            if (timeLineMoment == null)
            {
                timeLineMoment = new TimeLineMoment(moment);
                _timeLineMoments.Add(timeLineMoment);
                _timeLineMomentLookup.Add(moment, timeLineMoment);
            }
            timeLineMoment.AddStart();
        }

        protected virtual void AddEnd(DateTime moment)
        {
            ITimeLineMoment timeLineMoment = Find(moment);
            if (timeLineMoment == null)
            {
                timeLineMoment = new TimeLineMoment(moment);
                _timeLineMoments.Add(timeLineMoment);
                _timeLineMomentLookup.Add(moment, timeLineMoment);
            }
            timeLineMoment.AddEnd();
        }

        protected virtual void RemoveStart(DateTime moment)
        {
            ITimeLineMoment timeLineMoment = Find(moment);
            if (timeLineMoment == null)
            {
                throw new InvalidOperationException();
            }

            timeLineMoment.RemoveStart();
            if (timeLineMoment.IsEmpty)
            {
                _timeLineMoments.Remove(timeLineMoment);
                _timeLineMomentLookup.Remove(moment);
            }
        }

        protected virtual void RemoveEnd(DateTime moment)
        {
            ITimeLineMoment timeLineMoment = Find(moment);
            if (timeLineMoment == null)
            {
                throw new InvalidOperationException();
            }

            timeLineMoment.RemoveEnd();
            if (timeLineMoment.IsEmpty)
            {
                _timeLineMoments.Remove(timeLineMoment);
                _timeLineMomentLookup.Remove(moment);
            }
        }

        protected virtual void Sort()
        {
            _timeLineMoments.Sort((left, right) => left.Moment.CompareTo(right.Moment));
        }
    }}
