// -- FILE ------------------------------------------------------------------
// name       : TimePeriodChain.cs
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

    public class TimePeriodChain : ITimePeriodChain
    {
        private readonly List<ITimePeriod> _periods = new List<ITimePeriod>();

        public TimePeriodChain()
        { }

        public TimePeriodChain(IEnumerable<ITimePeriod> timePeriods)
        {
            if (timePeriods == null)
            {
                throw new ArgumentNullException("timePeriods");
            }
            AddAll(timePeriods);
        }
        public bool IsReadOnly => false;
        public ITimePeriod First => _periods.Count > 0 ? _periods[0] : null;
        public ITimePeriod Last => _periods.Count > 0 ? _periods[_periods.Count - 1] : null;
        public int Count => _periods.Count;

        public ITimePeriod this[int index]
        {
            get => _periods[index];
            set
            {
                RemoveAt(index);
                Insert(index, value);
            }
        }
        public bool IsAnytime => !HasStart && !HasEnd;
        public bool IsMoment => Count != 0 && First.Start.Equals(Last.End);
        public bool HasStart => Start != TimeSpec.MinPeriodDate;

        public DateTime Start
        {
            get { return Count > 0 ? First.Start : TimeSpec.MinPeriodDate; }
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
            get { return Count > 0 ? Last.End : TimeSpec.MaxPeriodDate; }
            set
            {
                if (Count == 0)
                {
                    return;
                }
                Move(value - End);
            }
        }
        public TimeSpan Duration => End - Start;

        public string DurationDescription => TimeFormatter.Instance.GetDuration(Duration, DurationFormatType.Detailed);

        public virtual TimeSpan GetDuration(IDurationProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            return provider.GetDuration(Start, End);
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
        public virtual void Add(ITimePeriod item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            CheckReadOnlyItem(item);

            ITimePeriod last = Last;
            if (last != null)
            {
                CheckSpaceAfter(last.End, item.Duration);
                item.Setup(last.End, last.End.Add(item.Duration));
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
        public virtual void Insert(int index, ITimePeriod period)
        {
            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (period == null)
            {
                throw new ArgumentNullException("period");
            }
            CheckReadOnlyItem(period);

            TimeSpan itemDuration = period.Duration;

            ITimePeriod previous = null;
            ITimePeriod next = null;
            if (Count > 0)
            {
                if (index > 0)
                {
                    previous = this[index - 1];
                    CheckSpaceAfter(End, itemDuration);
                }
                if (index < Count - 1)
                {
                    next = this[index];
                    CheckSpaceBefore(Start, itemDuration);
                }
            }

            _periods.Insert(index, period);

            // adjust time periods after the inserted item
            if (previous != null)
            {
                period.Setup(previous.End, previous.End + period.Duration);
                for (int i = index + 1; i < Count; i++)
                {
                    DateTime previousStart = this[i].Start.Add(itemDuration);
                    this[i].Setup(previousStart, previousStart.Add(this[i].Duration));
                }
            }

            // adjust time periods before the inserted item
            if (next == null)
            {
                return;
            }
            DateTime nextStart = next.Start.Subtract(itemDuration);
            period.Setup(nextStart, nextStart.Add(period.Duration));
            for (int i = 0; i < index - 1; i++)
            {
                nextStart = this[i].Start.Subtract(itemDuration);
                this[i].Setup(nextStart, nextStart.Add(this[i].Duration));
            }
        }
        public virtual bool Contains(ITimePeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException("period");
            }
            return _periods.Contains(period);
        }
        public virtual int IndexOf(ITimePeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException("period");
            }
            return _periods.IndexOf(period);
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
        public virtual bool Remove(ITimePeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException("period");
            }

            TimeSpan itemDuration = period.Duration;
            int index = IndexOf(period);

            ITimePeriod next = null;
            if (itemDuration > TimeSpan.Zero && Count > 1 && index > 0 && index < Count - 1) // between
            {
                next = this[index];
            }

            bool removed = _periods.Remove(period);
            if (removed && next != null) // fill the gap
            {
                for (int i = index; i < Count; i++)
                {
                    DateTime start = this[i].Start.Subtract(itemDuration);
                    this[i].Setup(start, start.Add(this[i].Duration));
                }
            }

            return removed;
        }
        public virtual void RemoveAt(int index)
        {
            Remove(this[index]);
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
            return HasSameData(obj as TimePeriodChain);
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
        protected void CheckSpaceBefore(DateTime moment, TimeSpan duration)
        {
            bool hasSpace = moment != TimeSpec.MinPeriodDate;
            if (hasSpace)
            {
                TimeSpan remaining = moment - TimeSpec.MinPeriodDate;
                hasSpace = duration <= remaining;
            }

            if (!hasSpace)
            {
                throw new InvalidOperationException("duration " + duration + " out of range ");
            }
        }
        protected void CheckSpaceAfter(DateTime moment, TimeSpan duration)
        {
            bool hasSpace = moment != TimeSpec.MaxPeriodDate;
            if (hasSpace)
            {
                TimeSpan remaining = TimeSpec.MaxPeriodDate - moment;
                hasSpace = duration <= remaining;
            }

            if (!hasSpace)
            {
                throw new InvalidOperationException("duration " + duration + " out of range");
            }
        }
        protected void CheckReadOnlyItem(ITimePeriod timePeriod)
        {
            if (timePeriod.IsReadOnly)
            {
                throw new NotSupportedException("TimePeriod is read-only");
            }
        }

    }}
