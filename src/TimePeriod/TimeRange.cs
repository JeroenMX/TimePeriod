// -- FILE ------------------------------------------------------------------
// name       : TimeRange.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using TimePeriod.Enums;
using TimePeriod.Interfaces;

namespace TimePeriod
{
    public class TimeRange : ITimeRange
    {
        private DateTime _start;
        private DateTime _end;
        public static readonly TimeRange Anytime = new TimeRange(true);

        public bool IsReadOnly { get; }

        public bool IsAnytime => !HasStart && !HasEnd;
        public bool IsMoment => _start.Equals(_end);
        public bool HasStart => _start != TimeSpec.MinPeriodDate;

        public TimeRange()
            : this(TimeSpec.MinPeriodDate, TimeSpec.MaxPeriodDate)
        { }

        internal TimeRange(bool isReadOnly = false) 
            : this(TimeSpec.MinPeriodDate, TimeSpec.MaxPeriodDate, isReadOnly)
        { }

        public TimeRange(DateTime moment, bool isReadOnly = false) 
            : this(moment, moment, isReadOnly)
        { }

        public TimeRange(DateTime start, DateTime end, bool isReadOnly = false)
        {
            if (start <= end)
            {
                this._start = start;
                this._end = end;
            }
            else
            {
                this._end = start;
                this._start = end;
            }
            this.IsReadOnly = isReadOnly;
        }

        public TimeRange(DateTime start, TimeSpan duration, bool isReadOnly = false)
        {
            if (duration >= TimeSpan.Zero)
            {
                this._start = start;
                _end = start.Add(duration);
            }
            else
            {
                this._start = start.Add(duration);
                _end = start;
            }
            this.IsReadOnly = isReadOnly;
        }

        public TimeRange(ITimePeriod copy)
        {
            if (copy == null)
            {
                throw new ArgumentNullException("copy");
            }
            _start = copy.Start;
            _end = copy.End;
            IsReadOnly = copy.IsReadOnly;
        }

        protected TimeRange(ITimePeriod copy, bool isReadOnly)
        {
            if (copy == null)
            {
                throw new ArgumentNullException("copy");
            }
            _start = copy.Start;
            _end = copy.End;
            this.IsReadOnly = isReadOnly;
        }

        public DateTime Start
        {
            get { return _start; }
            set
            {
                CheckModification();
                if (value > _end)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                _start = value;
            }
        }

        public bool HasEnd => _end != TimeSpec.MaxPeriodDate;

        public DateTime End
        {
            get { return _end; }
            set
            {
                CheckModification();
                if (value < _start)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                _end = value;
            }
        }

        public TimeSpan Duration
        {
            get { return _end.Subtract(_start); }
            set
            {
                CheckModification();
                _end = _start.Add(value);
            }
        }

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
            CheckModification();
            if (newStart <= newEnd)
            {
                _start = newStart;
                _end = newEnd;
            }
            else
            {
                _end = newStart;
                _start = newEnd;
            }
        }

        public virtual bool IsSamePeriod(ITimePeriod test)
        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }
            return _start == test.Start && _end == test.End;
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

        public ITimeRange Copy()
        {
            return Copy(TimeSpan.Zero);
        }

        public virtual ITimeRange Copy(TimeSpan offset)
        {
            return new TimeRange(_start.Add(offset), _end.Add(offset), IsReadOnly);
        }

        public virtual void Move(TimeSpan offset)
        {
            CheckModification();
            if (offset == TimeSpan.Zero)
            {
                return;
            }
            _start = _start.Add(offset);
            _end = _end.Add(offset);
        }

        public virtual void ExpandStartTo(DateTime moment)
        {
            CheckModification();
            if (_start > moment)
            {
                _start = moment;
            }
        }

        public virtual void ExpandEndTo(DateTime moment)
        {
            CheckModification();
            if (_end < moment)
            {
                _end = moment;
            }
        }

        public void ExpandTo(DateTime moment)
        {
            ExpandStartTo(moment);
            ExpandEndTo(moment);
        }

        public void ExpandTo(ITimePeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException("period");
            }
            ExpandStartTo(period.Start);
            ExpandEndTo(period.End);
        }

        public virtual void ShrinkStartTo(DateTime moment)
        {
            CheckModification();
            if (HasInside(moment) && _start < moment)
            {
                _start = moment;
            }
        }

        public virtual void ShrinkEndTo(DateTime moment)
        {
            CheckModification();
            if (HasInside(moment) && _end > moment)
            {
                _end = moment;
            }
        }

        public void ShrinkTo(ITimePeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException("period");
            }
            ShrinkStartTo(period.Start);
            ShrinkEndTo(period.End);
        }

        public virtual bool IntersectsWith(ITimePeriod test)
        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }
            return TimePeriodCalc.IntersectsWith(this, test);
        }

        public virtual ITimeRange GetIntersection(ITimePeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException("period");
            }
            if (!IntersectsWith(period))
            {
                return null;
            }
            DateTime periodStart = period.Start;
            DateTime periodEnd = period.End;
            return new TimeRange(
                periodStart > _start ? periodStart : _start,
                periodEnd < _end ? periodEnd : _end,
                IsReadOnly);
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

        public virtual void Reset()
        {
            CheckModification();
            _start = TimeSpec.MinPeriodDate;
            _end = TimeSpec.MaxPeriodDate;
        }

        public string GetDescription(ITimeFormatter formatter = null)
        {
            return Format(formatter ?? TimeFormatter.Instance);
        }

        protected virtual string Format(ITimeFormatter formatter)
        {
            return formatter.GetPeriod(_start, _end, Duration);
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
            return HasSameData(obj as TimeRange);
        }

        private bool HasSameData(TimeRange comp)
        {
            return _start == comp._start && _end == comp._end && IsReadOnly == comp.IsReadOnly;
        }

        public sealed override int GetHashCode()
        {
            return HashTool.AddHashCode(GetType().GetHashCode(), ComputeHashCode());
        }

        protected virtual int ComputeHashCode()
        {
            return HashTool.ComputeHashCode(IsReadOnly, _start, _end);
        }

        protected void CheckModification()
        {
            if (IsReadOnly)
            {
                throw new NotSupportedException("TimeRange is read-only");
            }
        }
    }
}
