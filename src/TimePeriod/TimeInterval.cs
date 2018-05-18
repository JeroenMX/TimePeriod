// -- FILE ------------------------------------------------------------------
// name       : TimeInterval.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.05.06
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using TimePeriod.Enums;
using TimePeriod.Interfaces;

namespace TimePeriod
{
    public class TimeInterval : ITimeInterval
    {
        private bool _isIntervalEnabled = true;
        private DateTime _startInterval;
        private DateTime _endInterval;
        private IntervalEdge _startEdge;
        private IntervalEdge _endEdge;

        public static readonly TimeInterval Anytime = new TimeInterval(
            TimeSpec.MinPeriodDate, TimeSpec.MaxPeriodDate, IntervalEdge.Closed, IntervalEdge.Closed, false, true);

        public TimeInterval()
            : this(TimeSpec.MinPeriodDate, TimeSpec.MaxPeriodDate)
        { }

        public TimeInterval(DateTime moment,
            IntervalEdge startEdge = IntervalEdge.Closed, IntervalEdge endEdge = IntervalEdge.Closed,
            bool isIntervalEnabled = true, bool isReadOnly = false) 
            : this(moment, moment, startEdge, endEdge, isIntervalEnabled, isReadOnly)
        { }

        public TimeInterval(DateTime startInterval, DateTime endInterval,
            IntervalEdge startEdge = IntervalEdge.Closed, IntervalEdge endEdge = IntervalEdge.Closed,
            bool isIntervalEnabled = true, bool isReadOnly = false)
        {
            if (startInterval <= endInterval)
            {
                this._startInterval = startInterval;
                this._endInterval = endInterval;
            }
            else
            {
                this._endInterval = startInterval;
                this._startInterval = endInterval;
            }

            this._startEdge = startEdge;
            this._endEdge = endEdge;

            this._isIntervalEnabled = isIntervalEnabled;
            this.IsReadOnly = isReadOnly;
        }

        public TimeInterval(ITimePeriod copy)
        {
            if (copy == null)
            {
                throw new ArgumentNullException("copy");
            }
            ITimeInterval timeInterval = copy as ITimeInterval;
            if (timeInterval != null)
            {
                _startInterval = timeInterval.StartInterval;
                _endInterval = timeInterval.EndInterval;
                _startEdge = timeInterval.StartEdge;
                _endEdge = timeInterval.EndEdge;
                _isIntervalEnabled = timeInterval.IsIntervalEnabled;
            }
            else
            {
                _startInterval = copy.Start;
                _endInterval = copy.End;
            }

            IsReadOnly = copy.IsReadOnly;
        }

        protected TimeInterval(ITimePeriod copy, bool isReadOnly)
        {
            if (copy == null)
            {
                throw new ArgumentNullException("copy");
            }
            ITimeInterval timeInterval = copy as ITimeInterval;
            if (timeInterval != null)
            {
                _startInterval = timeInterval.StartInterval;
                _endInterval = timeInterval.EndInterval;
                _startEdge = timeInterval.StartEdge;
                _endEdge = timeInterval.EndEdge;
                _isIntervalEnabled = timeInterval.IsIntervalEnabled;
            }
            else
            {
                _startInterval = copy.Start;
                _endInterval = copy.End;
            }
            this.IsReadOnly = isReadOnly;
        }

        public bool IsReadOnly { get; }

        public bool IsAnytime => !HasStart && !HasEnd;
        public bool IsMoment => _startInterval.Equals(_endInterval);
        public bool IsStartOpen => _startEdge == IntervalEdge.Open;
        public bool IsEndOpen => _endEdge == IntervalEdge.Open;
        public bool IsOpen => IsStartOpen && IsEndOpen;
        public bool IsStartClosed => _startEdge == IntervalEdge.Closed;
        public bool IsEndClosed => _endEdge == IntervalEdge.Closed;
        public bool IsClosed => IsStartClosed && IsEndClosed;
        public bool IsEmpty => IsMoment && !IsClosed;
        public bool IsDegenerate => IsMoment && IsClosed;

        public bool IsIntervalEnabled
        {
            get => _isIntervalEnabled;
            set
            {
                CheckModification();
                _isIntervalEnabled = value;
            }
        }

        public bool HasStart => !(_startInterval == TimeSpec.MinPeriodDate && _startEdge == IntervalEdge.Closed);

        public DateTime StartInterval
        {
            get => _startInterval;
            set
            {
                CheckModification();
                if (value > _endInterval)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                _startInterval = value;
            }
        }

        public DateTime Start
        {
            get
            {
                if (_isIntervalEnabled && _startEdge == IntervalEdge.Open)
                {
                    return _startInterval.AddTicks(1);
                }
                return _startInterval;
            }
        }

        public IntervalEdge StartEdge
        {
            get { return _startEdge; }
            set
            {
                CheckModification();
                _startEdge = value;
            }
        }

        public bool HasEnd => !(_endInterval == TimeSpec.MaxPeriodDate && _endEdge == IntervalEdge.Closed);

        public DateTime EndInterval
        {
            get => _endInterval;
            set
            {
                CheckModification();
                if (value < _startInterval)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                _endInterval = value;
            }
        }

        public DateTime End
        {
            get
            {
                if (_isIntervalEnabled && _endEdge == IntervalEdge.Open)
                {
                    return _endInterval.AddTicks(-1);
                }
                return _endInterval;
            }
        }

        public IntervalEdge EndEdge
        {
            get { return _endEdge; }
            set
            {
                CheckModification();
                _endEdge = value;
            }
        }

        public TimeSpan Duration => _endInterval.Subtract(_startInterval);

        public string DurationDescription => TimeFormatter.Instance.GetDuration(Duration, DurationFormatType.Detailed);
    
        public virtual TimeSpan GetDuration(IDurationProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            return provider.GetDuration(Start, End);
        }

        public virtual void Setup(DateTime newStartInterval, DateTime newEndInterval)
        {
            CheckModification();
            if (newStartInterval <= newEndInterval)
            {
                _startInterval = newStartInterval;
                _endInterval = newEndInterval;
            }
            else
            {
                _endInterval = newStartInterval;
                _startInterval = newEndInterval;
            }
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
        
        public ITimeInterval Copy()
        {
            return Copy(TimeSpan.Zero);
        }
        
        public virtual ITimeInterval Copy(TimeSpan offset)
        {
            return new TimeInterval(
                _startInterval.Add(offset),
                _endInterval.Add(offset),
                _startEdge,
                _endEdge,
                IsIntervalEnabled,
                IsReadOnly);
        }
        
        public virtual void Move(TimeSpan offset)
        {
            CheckModification();
            if (offset == TimeSpan.Zero)
            {
                return;
            }
            _startInterval = _startInterval.Add(offset);
            _endInterval = _endInterval.Add(offset);
        }
        
        public virtual void ExpandStartTo(DateTime moment)
        {
            CheckModification();
            if (_startInterval > moment)
            {
                _startInterval = moment;
            }
        }
        
        public virtual void ExpandEndTo(DateTime moment)
        {
            CheckModification();
            if (_endInterval < moment)
            {
                _endInterval = moment;
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
            ITimeInterval timeInterval = period as ITimeInterval;
            if (timeInterval != null)
            {
                ExpandStartTo(timeInterval.StartInterval);
                ExpandEndTo(timeInterval.EndInterval);
            }
            else
            {
                ExpandStartTo(period.Start);
                ExpandEndTo(period.End);
            }
        }
        
        public virtual void ShrinkStartTo(DateTime moment)
        {
            CheckModification();
            if (HasInside(moment) && _startInterval < moment)
            {
                _startInterval = moment;
            }
        }
        
        public virtual void ShrinkEndTo(DateTime moment)
        {
            CheckModification();
            if (HasInside(moment) && _endInterval > moment)
            {
                _endInterval = moment;
            }
        }
        
        public void ShrinkTo(ITimePeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException("period");
            }
            ITimeInterval timeInterval = period as ITimeInterval;
            if (timeInterval != null)
            {
                ShrinkStartTo(timeInterval.StartInterval);
                ShrinkEndTo(timeInterval.EndInterval);
            }
            else
            {
                ShrinkStartTo(period.Start);
                ShrinkEndTo(period.End);
            }
            ShrinkStartTo(period.Start);
        }
        
        public virtual bool IntersectsWith(ITimePeriod test)
        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }
            return TimePeriodCalc.IntersectsWith(this, test);
        }
        
        public virtual ITimeInterval GetIntersection(ITimePeriod period)
        {
            if (period == null)
            {
                throw new ArgumentNullException("period");
            }
            if (!IntersectsWith(period))
            {
                return null;
            }
            DateTime start = Start;
            DateTime end = End;
            DateTime periodStart = period.Start;
            DateTime periodEnd = period.End;
            return new TimeInterval(
                periodStart > start ? periodStart : start,
                periodEnd < end ? periodEnd : end,
                IntervalEdge.Closed,
                IntervalEdge.Closed,
                IsIntervalEnabled,
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
            _isIntervalEnabled = true;
            _startInterval = TimeSpec.MinPeriodDate;
            _endInterval = TimeSpec.MaxPeriodDate;
            _startEdge = IntervalEdge.Closed;
            _endEdge = IntervalEdge.Closed;
        }
        
        public string GetDescription(ITimeFormatter formatter = null)
        {
            return Format(formatter ?? TimeFormatter.Instance);
        }
        
        protected virtual string Format(ITimeFormatter formatter)
        {
            return formatter.GetInterval(_startInterval, _endInterval, _startEdge, _endEdge, Duration);
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
            return HasSameData(obj as TimeInterval);
        }
        
        private bool HasSameData(TimeInterval comp)
        {
            return
                IsReadOnly == comp.IsReadOnly &&
                _isIntervalEnabled == comp._isIntervalEnabled &&
                _startInterval == comp._startInterval &&
                _endInterval == comp._endInterval &&
                _startEdge == comp._startEdge &&
                _endEdge == comp._endEdge;
        }
        
        public sealed override int GetHashCode()
        {
            return HashTool.AddHashCode(GetType().GetHashCode(), ComputeHashCode());
        }
        
        protected virtual int ComputeHashCode()
        {
            return HashTool.ComputeHashCode(
                IsReadOnly,
                _isIntervalEnabled,
                _startInterval,
                _endInterval,
                _startEdge,
                _endEdge);
        }
        
        protected void CheckModification()
        {
            if (IsReadOnly)
            {
                throw new NotSupportedException("TimeInterval is read-only");
            }
        }        
    }
}

