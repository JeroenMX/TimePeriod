// -- FILE ------------------------------------------------------------------
// name       : TimePeriodCollection.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using TimePeriod.Enums;
using TimePeriod.Interfaces;

namespace TimePeriod
{
    public class TimePeriodCollection : ITimePeriodCollection
    {
        private readonly List<ITimePeriod> _periods = new List<ITimePeriod>();

        public bool IsReadOnly { get; } = false;
        public int Count => _periods.Count;
        public ITimePeriod this[int index]
        {
            get => _periods[index];
            set => _periods[index] = value;
        }
        public bool IsAnytime => !HasStart && !HasEnd;
        public bool IsMoment => Duration == TimeSpan.Zero;
        public bool HasStart => Start != TimeSpec.MinPeriodDate;


        public TimePeriodCollection()
        { }

        public TimePeriodCollection(IEnumerable<ITimePeriod> timePeriods) 
            : this()
        {
            if (timePeriods == null)
            {
                throw new ArgumentNullException("timePeriods");
            }
            AddAll(timePeriods);
        }


        public DateTime Start
        {
            get
            {
                DateTime? start = GetStart();
                return start.HasValue ? start.Value : TimeSpec.MinPeriodDate;
            }
            set
            {
                if (Count == 0)
                {
                    return;
                }
                Move(value - Start);
            }
        }

        public bool HasEnd => End != TimeSpec.MaxPeriodDate;

        public DateTime End
        {
            get
            {
                DateTime? end = GetEnd();
                return end.HasValue ? end.Value : TimeSpec.MaxPeriodDate;
            }
            set
            {
                if (Count == 0)
                {
                    return;
                }
                Move(value - End);
            }
        }

        public TimeSpan Duration
        {
            get
            {
                TimeSpan? duration = GetDuration();
                return duration.HasValue ? duration.Value : TimeSpec.MaxPeriodDuration;
            }
        }

        public virtual TimeSpan TotalDuration
        {
            get
            {
                TimeSpan duration = TimeSpan.Zero;
                foreach (ITimePeriod timePeriod in _periods)
                {
                    duration = duration.Add(timePeriod.Duration);
                }
                return duration;
            }
        }

        public string DurationDescription
        {
            get { return TimeFormatter.Instance.GetDuration(Duration, DurationFormatType.Detailed); }
        }

        public virtual TimeSpan GetDuration(IDurationProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            return provider.GetDuration(Start, End);
        }

        public virtual TimeSpan GetTotalDuration(IDurationProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            TimeSpan duration = TimeSpan.Zero;
            foreach (ITimePeriod timePeriod in _periods)
            {
                duration = duration.Add(timePeriod.GetDuration(provider));
            }
            return duration;
        }

        {
            throw new InvalidOperationException();
        }

        {
            return GetEnumerator();
        }

        {
            return _periods.GetEnumerator();
        }

        {
            if (delta == TimeSpan.Zero)
            {
                return;
            }

            foreach (ITimePeriod timePeriod in _periods)
            {
                DateTime start = timePeriod.Start + delta;
                timePeriod.Setup(start, start.Add(timePeriod.Duration));
            }
        }

        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            _periods.Sort(comparer);
        }

        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            SortBy(new TimePeriodReversComparer(comparer));
        }

        {
            switch (sortDirection)
            {
                case ListSortDirection.Ascending:
                    SortBy(TimePeriodStartComparer.Comparer);
                    break;
                case ListSortDirection.Descending:
                    SortBy(TimePeriodStartComparer.ReverseComparer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("sortDirection");
            }
        }

        {
            switch (sortDirection)
            {
                case ListSortDirection.Ascending:
                    SortBy(TimePeriodEndComparer.Comparer);
                    break;
                case ListSortDirection.Descending:
                    SortBy(TimePeriodEndComparer.ReverseComparer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("sortDirection");
            }
        }

        {
            switch (sortDirection)
            {
                case ListSortDirection.Ascending:
                    SortBy(TimePeriodDurationComparer.Comparer);
                    break;
                case ListSortDirection.Descending:
                    SortBy(TimePeriodDurationComparer.ReverseComparer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("sortDirection");
            }
        }

        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }

            foreach (ITimePeriod period in _periods)
            {
                if (test.HasInside(period))
                {
                    return true;
                }
            }

            return false;
        }

        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }

            TimePeriodCollection insidePeriods = new TimePeriodCollection();

            foreach (ITimePeriod period in _periods)
            {
                if (test.HasInside(period))
                {
                    insidePeriods.Add(period);
                }
            }

            return insidePeriods;
        }

        {
            bool hasOverlaps = false;
            if (Count == 2)
            {
                hasOverlaps = this[0].OverlapsWith(this[1]);
            }
            else if (Count > 2)
            {
                hasOverlaps = new TimeLineMomentCollection(this).HasOverlaps();
            }

            return hasOverlaps;
        }

        {
            bool hasGaps = false;
            if (Count > 1)
            {
                hasGaps = new TimeLineMomentCollection(this).HasGaps();
            }

            return hasGaps;
        }

        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }

            foreach (ITimePeriod period in _periods)
            {
                if (test.OverlapsWith(period))
                {
                    return true;
                }
            }

            return false;
        }

        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }

            TimePeriodCollection overlapPeriods = new TimePeriodCollection();

            foreach (ITimePeriod period in _periods)
            {
                if (test.OverlapsWith(period))
                {
                    overlapPeriods.Add(period);
                }
            }

            return overlapPeriods;
        }

        {
            foreach (ITimePeriod period in _periods)
            {
                if (period.HasInside(test))
                {
                    return true;
                }
            }

            return false;
        }

        {
            TimePeriodCollection intersectionPeriods = new TimePeriodCollection();

            foreach (ITimePeriod period in _periods)
            {
                if (period.HasInside(test))
                {
                    intersectionPeriods.Add(period);
                }
            }

            return intersectionPeriods;
        }

        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }

            foreach (ITimePeriod period in _periods)
            {
                if (period.IntersectsWith(test))
                {
                    return true;
                }
            }

            return false;
        }

        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }

            TimePeriodCollection intersectionPeriods = new TimePeriodCollection();

            foreach (ITimePeriod period in _periods)
            {
                if (test.IntersectsWith(period))
                {
                    intersectionPeriods.Add(period);
                }
            }

            return intersectionPeriods;
        }

        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }

            TimePeriodCollection relationPeriods = new TimePeriodCollection();

            foreach (ITimePeriod period in _periods)
            {
                if (test.GetRelation(period) == relation)
                {
                    relationPeriods.Add(period);
                }
            }

            return relationPeriods;
        }

        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            _periods.Add(item);
        }

        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }

            foreach (ITimePeriod period in _periods)
            {
                if (period.IsSamePeriod(test))
                {
                    return true;
                }
            }
            return false;
        }

        {
            if (timePeriods == null)
            {
                throw new ArgumentNullException("timePeriods");
            }

            foreach (ITimePeriod period in timePeriods)
            {
                Add(period);
            }
        }

        {
            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            _periods.Insert(index, item);
        }

        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return _periods.Contains(item);
        }

        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return _periods.IndexOf(item);
        }

        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            _periods.CopyTo(array, arrayIndex);
        }

        {
            _periods.Clear();
        }

        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return _periods.Remove(item);
        }

        {
            _periods.RemoveAt(index);
        }

        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }
            return Start == test.Start && End == test.End;
        }

        {
            return TimePeriodCalc.HasInside(this, test);
        }

        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }
            return TimePeriodCalc.HasInside(this, test);
        }

        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }
            return TimePeriodCalc.IntersectsWith(this, test);
        }

        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }
            return TimePeriodCalc.OverlapsWith(this, test);
        }

        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }
            return TimePeriodCalc.GetRelation(this, test);
        }

        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            return comparer.Compare(this, other);
        }

        {
            return Format(formatter ?? TimeFormatter.Instance);
        }

        {
            return formatter.GetCollectionPeriod(Count, Start, End, Duration);
        }

        {
            return GetDescription();
        }

        {
            if (obj == this)
            {
                return true;
            }
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return IsEqual(obj);
        }

        {
            return HasSameData(obj as TimePeriodCollection);
        }

        {
            if (Count != comp.Count)
            {
                return false;
            }

            for (int i = 0; i < Count; i++)
            {
                if (!this[i].Equals(comp[i]))
                {
                    return false;
                }
            }

            return true;
        }

        {
            return HashTool.AddHashCode(GetType().GetHashCode(), ComputeHashCode());
        }

        {
            return HashTool.ComputeHashCode(_periods);
        }

        {
            if (Count == 0)
            {
                return null;
            }

            DateTime start = TimeSpec.MaxPeriodDate;

            foreach (ITimePeriod timePeriod in _periods)
            {
                if (timePeriod.Start < start)
                {
                    start = timePeriod.Start;
                }
            }

            return start;
        }

        {
            if (Count == 0)
            {
                return null;
            }

            DateTime end = TimeSpec.MinPeriodDate;

            foreach (ITimePeriod timePeriod in _periods)
            {
                if (timePeriod.End > end)
                {
                    end = timePeriod.End;
                }
            }

            return end;
        }

        {
            if (Count == 0)
            {
                start = null;
                end = null;
                return;
            }

            start = TimeSpec.MaxPeriodDate;
            end = TimeSpec.MinPeriodDate;

            foreach (ITimePeriod timePeriod in _periods)
            {
                if (timePeriod.Start < start)
                {
                    start = timePeriod.Start;
                }
                if (timePeriod.End > end)
                {
                    end = timePeriod.End;
                }
            }
        }

        {
            DateTime? start;
            DateTime? end;

            GetStartEnd(out start, out end);

            if (start == null || end == null)
            {
                return null;
            }

            return end.Value - start.Value;
        }                
    }
}