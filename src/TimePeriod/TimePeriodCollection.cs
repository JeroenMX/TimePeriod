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
        public virtual void Setup(DateTime newStart, DateTime newEnd)
        {
            throw new InvalidOperationException();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<ITimePeriod> GetEnumerator()
        {
            return _periods.GetEnumerator();
        }
        public virtual void Move(TimeSpan delta)
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
        public virtual void SortBy(ITimePeriodComparer comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            _periods.Sort(comparer);
        }
        public virtual void SortReverseBy(ITimePeriodComparer comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            SortBy(new TimePeriodReversComparer(comparer));
        }
        public virtual void SortByStart(ListSortDirection sortDirection = ListSortDirection.Ascending)
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
        public virtual void SortByEnd(ListSortDirection sortDirection = ListSortDirection.Ascending)
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
        public virtual void SortByDuration(ListSortDirection sortDirection = ListSortDirection.Ascending)
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
        public virtual bool HasInsidePeriods(ITimePeriod test)
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
        public virtual ITimePeriodCollection InsidePeriods(ITimePeriod test)
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
        public virtual bool HasOverlaps()
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
        public virtual bool HasGaps()
        {
            bool hasGaps = false;
            if (Count > 1)
            {
                hasGaps = new TimeLineMomentCollection(this).HasGaps();
            }

            return hasGaps;
        }
        public virtual bool HasOverlapPeriods(ITimePeriod test)
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
        public virtual ITimePeriodCollection OverlapPeriods(ITimePeriod test)
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
        public virtual bool HasIntersectionPeriods(DateTime test)
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
        public virtual ITimePeriodCollection IntersectionPeriods(DateTime test)
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
        public virtual bool HasIntersectionPeriods(ITimePeriod test)
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
        public virtual ITimePeriodCollection IntersectionPeriods(ITimePeriod test)
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
        public virtual ITimePeriodCollection RelationPeriods(ITimePeriod test, PeriodRelation relation)
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
        public virtual void Add(ITimePeriod item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            _periods.Add(item);
        }
        public bool ContainsPeriod(ITimePeriod test)
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
        public void AddAll(IEnumerable<ITimePeriod> timePeriods)
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
        public virtual void Insert(int index, ITimePeriod item)
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
        public virtual bool Contains(ITimePeriod item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return _periods.Contains(item);
        }
        public virtual int IndexOf(ITimePeriod item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return _periods.IndexOf(item);
        }
        public virtual void CopyTo(ITimePeriod[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            _periods.CopyTo(array, arrayIndex);
        }
        public virtual void Clear()
        {
            _periods.Clear();
        }
        public virtual bool Remove(ITimePeriod item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return _periods.Remove(item);
        }
        public virtual void RemoveAt(int index)
        {
            _periods.RemoveAt(index);
        }
        public virtual bool IsSamePeriod(ITimePeriod test)
        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }
            return Start == test.Start && End == test.End;
        }
        public virtual bool HasInside(DateTime test)
        {
            return TimePeriodCalc.HasInside(this, test);
        }
        public virtual bool HasInside(ITimePeriod test)
        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }
            return TimePeriodCalc.HasInside(this, test);
        }
        public virtual bool IntersectsWith(ITimePeriod test)
        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }
            return TimePeriodCalc.IntersectsWith(this, test);
        }
        public virtual bool OverlapsWith(ITimePeriod test)
        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }
            return TimePeriodCalc.OverlapsWith(this, test);
        }
        public virtual PeriodRelation GetRelation(ITimePeriod test)
        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }
            return TimePeriodCalc.GetRelation(this, test);
        }
        public virtual int CompareTo(ITimePeriod other, ITimePeriodComparer comparer)
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
        public string GetDescription(ITimeFormatter formatter = null)
        {
            return Format(formatter ?? TimeFormatter.Instance);
        }
        protected virtual string Format(ITimeFormatter formatter)
        {
            return formatter.GetCollectionPeriod(Count, Start, End, Duration);
        }
        public override string ToString()
        {
            return GetDescription();
        }
        public sealed override bool Equals(object obj)
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
        protected virtual bool IsEqual(object obj)
        {
            return HasSameData(obj as TimePeriodCollection);
        }
        private bool HasSameData(IList<ITimePeriod> comp)
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
        public sealed override int GetHashCode()
        {
            return HashTool.AddHashCode(GetType().GetHashCode(), ComputeHashCode());
        }
        protected virtual int ComputeHashCode()
        {
            return HashTool.ComputeHashCode(_periods);
        }
        protected virtual DateTime? GetStart()
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
        protected virtual DateTime? GetEnd()
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
        protected virtual void GetStartEnd(out DateTime? start, out DateTime? end)
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
        protected virtual TimeSpan? GetDuration()
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
