// -- FILE ------------------------------------------------------------------
// name       : TimeBlock.cs
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
    public class TimeBlock : ITimeBlock
    {

        public static readonly TimeBlock Anytime = new TimeBlock(true);

        public TimeBlock()
            : this(TimeSpec.MinPeriodDate, TimeSpec.MaxPeriodDate)
        { }
        internal TimeBlock(bool isReadOnly = false) :
            this(TimeSpec.MinPeriodDate, TimeSpec.MaxPeriodDate, isReadOnly)
        { }
        public TimeBlock(DateTime moment, bool isReadOnly = false) 
            : this(moment, TimeSpec.MinPeriodDuration, isReadOnly)
        { }
        public TimeBlock(DateTime start, DateTime end, bool isReadOnly = false)
        {
            if (start <= end)
            {
                this.start = start;
                this.end = end;
            }
            else
            {
                this.end = start;
                this.start = end;
            }
            duration = this.end - this.start;
            this.isReadOnly = isReadOnly;
        }

        public TimeBlock(DateTime start, TimeSpan duration, bool isReadOnly = false)
        {
            if (duration < TimeSpec.MinPeriodDuration)
            {
                throw new ArgumentOutOfRangeException("duration");
            }
            this.start = start;
            this.duration = duration;
            end = start.Add(duration);
            this.isReadOnly = isReadOnly;
        }

        public TimeBlock(TimeSpan duration, DateTime end, bool isReadOnly = false)
        {
            if (duration < TimeSpec.MinPeriodDuration)
            {
                throw new ArgumentOutOfRangeException("duration");
            }
            this.end = end;
            this.duration = duration;
            start = end.Subtract(duration);
            this.isReadOnly = isReadOnly;
        }

        public TimeBlock(ITimePeriod copy)
        {
            if (copy == null)
            {
                throw new ArgumentNullException("copy");
            }
            start = copy.Start;
            end = copy.End;
            duration = copy.Duration;
            isReadOnly = copy.IsReadOnly;
        }

        protected TimeBlock(ITimePeriod copy, bool isReadOnly)
        {
            if (copy == null)
            {
                throw new ArgumentNullException("copy");
            }
            start = copy.Start;
            end = copy.End;
            duration = copy.Duration;
            this.isReadOnly = isReadOnly;
        }

        public bool IsReadOnly => isReadOnly;
        public bool IsAnytime => !HasStart && !HasEnd;

        public bool IsMoment => start.Equals(end);

        public bool HasStart => start != TimeSpec.MinPeriodDate;

        public DateTime Start
        {
            get => start;
            set
            {
                CheckModification();
                start = value;
                end = start.Add(duration);
            }
        }

        public bool HasEnd => end != TimeSpec.MaxPeriodDate;

        public DateTime End
        {
            get => end;
            set
            {
                CheckModification();
                end = value;
                start = end.Subtract(duration);
            }
        }

        public TimeSpan Duration
        {
            get => duration;
            set => DurationFromStart(value);
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
                start = newStart;
                end = newEnd;
            }
            else
            {
                end = newStart;
                start = newEnd;
            }
            duration = end - start;
        }

        public virtual void Setup(DateTime newStart, TimeSpan newDuration)
        {
            CheckModification();
            if (newDuration < TimeSpec.MinPeriodDuration)
            {
                throw new ArgumentOutOfRangeException("newDuration");
            }
            start = newStart;
            duration = newDuration;
            end = start.Add(duration);
        }

        public ITimeBlock Copy()
        {
            return Copy(TimeSpan.Zero);
        }

        public virtual ITimeBlock Copy(TimeSpan offset)
        {
            return new TimeBlock(start.Add(offset), end.Add(offset), IsReadOnly);
        }

        public virtual void Move(TimeSpan offset)
        {
            CheckModification();
            if (offset == TimeSpan.Zero)
            {
                return;
            }
            start = start.Add(offset);
            end = end.Add(offset);
        }

        public ITimeBlock GetPreviousPeriod()
        {
            return GetPreviousPeriod(TimeSpan.Zero);
        }

        public virtual ITimeBlock GetPreviousPeriod(TimeSpan offset)
        {
            return new TimeBlock(Duration, Start.Add(offset), IsReadOnly);
        }

        public ITimeBlock GetNextPeriod()
        {
            return GetNextPeriod(TimeSpan.Zero);
        }

        public virtual ITimeBlock GetNextPeriod(TimeSpan offset)
        {
            return new TimeBlock(End.Add(offset), Duration, IsReadOnly);
        }

        public virtual void DurationFromStart(TimeSpan newDuration)
        {
            if (newDuration < TimeSpec.MinPeriodDuration)
            {
                throw new ArgumentOutOfRangeException("newDuration");
            }
            CheckModification();
            duration = newDuration;
            end = start.Add(newDuration);
        }

        public virtual void DurationFromEnd(TimeSpan newDuration)
        {
            if (newDuration < TimeSpec.MinPeriodDuration)
            {
                throw new ArgumentOutOfRangeException("newDuration");
            }
            CheckModification();
            duration = newDuration;
            start = end.Subtract(newDuration);
        }

        public virtual bool IsSamePeriod(ITimePeriod test)
        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }
            return start == test.Start && end == test.End;
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

        public virtual ITimeBlock GetIntersection(ITimePeriod period)
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
            return new TimeBlock(
                periodStart > start ? periodStart : start,
                periodEnd < end ? periodEnd : end,
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
            start = TimeSpec.MinPeriodDate;
            duration = TimeSpec.MaxPeriodDuration;
            end = TimeSpec.MaxPeriodDate;
        }

        public string GetDescription(ITimeFormatter formatter = null)
        {
            return Format(formatter ?? TimeFormatter.Instance);
        }

        protected virtual string Format(ITimeFormatter formatter)
        {
            return formatter.GetPeriod(start, end, duration);
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
            return HasSameData(obj as TimeBlock);
        }

        private bool HasSameData(TimeBlock comp)
        {
            return start == comp.start && end == comp.end && isReadOnly == comp.isReadOnly;
        }

        public sealed override int GetHashCode()
        {
            return HashTool.AddHashCode(GetType().GetHashCode(), ComputeHashCode());
        }

        protected virtual int ComputeHashCode()
        {
            return HashTool.ComputeHashCode(isReadOnly, start, end, duration);
        }

        protected void CheckModification()
        {
            if (IsReadOnly)
            {
                throw new NotSupportedException("TimeBlock is read-only");
            }
        }

        // members
        private readonly bool isReadOnly;
        private DateTime start;
        private TimeSpan duration;
        private DateTime end;  // cache
    }

}
// -- EOF -------------------------------------------------------------------
