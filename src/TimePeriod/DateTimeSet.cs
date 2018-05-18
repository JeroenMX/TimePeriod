// -- FILE ------------------------------------------------------------------
// name       : DateTimeSet.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
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
    public class DateTimeSet : IDateTimeSet
    {
        private readonly List<DateTime> _moments = new List<DateTime>();

        public DateTime this[int index] => _moments[index];
        public DateTime? Min => !IsEmpty ? _moments[0] : (DateTime?)null;
        public DateTime? Max => !IsEmpty ? _moments[Count - 1] : (DateTime?)null;

        public DateTimeSet()
        { }

        public DateTimeSet(IEnumerable<DateTime> moments)
        {
            AddAll(moments);
        }

        public TimeSpan? Duration
        {
            get
            {
                DateTime? min = Min;
                DateTime? max = Max;
                return min.HasValue && max.HasValue ? max.Value - min.Value : (TimeSpan?)null;
            }
        }

        public bool IsEmpty => Count == 0;
        
        public bool IsMoment
        {
            get
            {
                TimeSpan? duration = Duration;
                return duration.HasValue && duration.Value == TimeSpan.Zero;
            }
        }

        public bool IsAnytime
        {
            get
            {
                DateTime? min = Min;
                DateTime? max = Max;
                return
                    min.HasValue && min.Value == DateTime.MinValue &&
                    max.HasValue && max.Value == DateTime.MaxValue;
            }
        }

        public int Count => _moments.Count;
        bool ICollection<DateTime>.IsReadOnly => false;
        
        public int IndexOf(DateTime moment)
        {
            return _moments.IndexOf(moment);
        }
        
        public DateTime? FindPrevious(DateTime moment)
        {
            if (IsEmpty)
            {
                return null;
            }

            for (int i = Count - 1; i >= 0; i--)
            {
                if (_moments[i] < moment)
                {
                    return _moments[i];
                }
            }

            return null;
        }

        public DateTime? FindNext(DateTime moment)
        {
            if (IsEmpty)
            {
                return null;
            }

            for (int i = 0; i < Count; i++)
            {
                if (_moments[i] > moment)
                {
                    return _moments[i];
                }
            }

            return null;
        }

        public IEnumerator<DateTime> GetEnumerator()
        {
            return _moments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Add(DateTime moment)
        {
            if (Contains(moment))
            {
                return false;
            }

            for (int i = 0; i < Count; i++)
            {
                if (_moments[i] <= moment)
                {
                    continue;
                }
                _moments.Insert(i, moment);
                return true;
            }

            _moments.Add(moment);
            return true;
        }

        void ICollection<DateTime>.Add(DateTime moment)
        {
            Add(moment);
        }

        public void AddAll(IEnumerable<DateTime> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            foreach (DateTime item in items)
            {
                Add(item);
            }
        }

        public IList<TimeSpan> GetDurations(int startIndex, int count)
        {
            if (startIndex >= Count - 1)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            int endIndex = startIndex + count - 1;
            if (endIndex >= Count - 1)
            {
                endIndex = Count - 2;
            }

            List<TimeSpan> durations = new List<TimeSpan>();
            if (endIndex >= startIndex)
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    durations.Add(this[i + 1] - this[i]);
                }
            }
            return durations;
        }
        
        public void Clear()
        {
            _moments.Clear();
        }
        
        public bool Contains(DateTime moment)
        {
            return _moments.Contains(moment);
        }
        
        public void CopyTo(DateTime[] array, int arrayIndex)
        {
            _moments.CopyTo(array, arrayIndex);
        }
        
        public bool Remove(DateTime moment)
        {
            return _moments.Remove(moment);
        }
        
        public override bool Equals(object obj)
        {
            return _moments.Equals(obj);
        }
        
        public override int GetHashCode()
        {
            return _moments.GetHashCode();
        }
        
        public override string ToString()
        {
            DateTime? min = Min;
            DateTime? max = Max;
            TimeSpan? duration = Duration;
            if (!min.HasValue || !max.HasValue || !duration.HasValue)
            {
                return TimeFormatter.Instance.GetCollection(Count);
            }

            return TimeFormatter.Instance.GetCollectionPeriod(Count, min.Value, max.Value, duration.Value);
        }
               
    }
}
