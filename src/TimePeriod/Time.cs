// -- FILE ------------------------------------------------------------------
// name       : Time.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.04.10
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;

namespace TimePeriod
{
    public struct Time : IComparable, IComparable<Time>, IEquatable<Time>
    {

        public Time(DateTime dateTime)
        {
            duration = dateTime.TimeOfDay;
        }

        public Time(TimeSpan duration) 
            : this(Math.Abs(duration.Hours), Math.Abs(duration.Minutes),
                        Math.Abs(duration.Seconds), Math.Abs(duration.Milliseconds))
        { }

        public Time(int hour = 0, int minute = 0, int second = 0, int millisecond = 0)
        {
            if (hour < 0 || hour > TimeSpec.HoursPerDay)
            {
                throw new ArgumentOutOfRangeException("hour");
            }
            if (hour == TimeSpec.HoursPerDay)
            {
                if (minute > 0)
                {
                    throw new ArgumentOutOfRangeException("minute");
                }
                if (second > 0)
                {
                    throw new ArgumentOutOfRangeException("second");
                }
                if (millisecond > 0)
                {
                    throw new ArgumentOutOfRangeException("millisecond");
                }
            }
            if (minute < 0 || minute >= TimeSpec.MinutesPerHour)
            {
                throw new ArgumentOutOfRangeException("minute");
            }
            if (second < 0 || second >= TimeSpec.SecondsPerMinute)
            {
                throw new ArgumentOutOfRangeException("second");
            }
            if (millisecond < 0 || millisecond >= TimeSpec.MillisecondsPerSecond)
            {
                throw new ArgumentOutOfRangeException("millisecond");
            }
            duration = new TimeSpan(0, hour, minute, second, millisecond);
        }

        public int Hour => duration.Hours;
        public int Minute => duration.Minutes;
        public int Second => duration.Seconds;
        public int Millisecond => duration.Milliseconds;
        public TimeSpan Duration => duration;
        public bool IsZero => duration.Equals(TimeSpan.Zero);
        public bool IsFullDay => (int)duration.TotalHours == TimeSpec.HoursPerDay;
        public bool IsFullDayOrZero => IsFullDay || IsZero;
        public long Ticks => duration.Ticks;
        public double TotalHours => duration.TotalHours;
        public double TotalMinutes => duration.TotalMinutes;
        public double TotalSeconds => duration.TotalSeconds;
        public double TotalMilliseconds => duration.TotalMilliseconds;

        public int CompareTo(Time other)
        {
            return duration.CompareTo(other.duration);
        }

        public int CompareTo(object obj)
        {
            return duration.CompareTo(((Time)obj).duration);
        }

        public bool Equals(Time other)
        {
            return duration.Equals(other.duration);
        }

        public override string ToString()
        {
            return ((int)TotalHours).ToString("00") + ":" + Minute.ToString("00") +
                ":" + Second.ToString("00") + "." + Millisecond.ToString("000");
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return Equals((Time)obj);
        }

        public override int GetHashCode()
        {
            return HashTool.ComputeHashCode(GetType().GetHashCode(), duration);
        }

        public static TimeSpan operator -(Time time1, Time time2)
        {
            return (time1 - time2.duration).duration;
        }

        public static Time operator -(Time time, TimeSpan duration)
        {
            if (Equals(duration, TimeSpan.Zero))
            {
                return time;
            }
            DateTime day = duration > TimeSpan.Zero ? DateTime.MaxValue.Date : DateTime.MinValue.Date;
            return new Time(time.ToDateTime(day).Subtract(duration));
        }

        public static TimeSpan operator +(Time time1, Time time2)
        {
            return (time1 + time2.duration).duration;
        }

        public static Time operator +(Time time, TimeSpan duration)
        {
            if (Equals(duration, TimeSpan.Zero))
            {
                return time;
            }
            DateTime day = duration > TimeSpan.Zero ? DateTime.MinValue : DateTime.MaxValue;
            return new Time(time.ToDateTime(day).Add(duration));
        }

        public static bool operator <(Time time1, Time time2)
        {
            return time1.duration < time2.duration;
        }

        public static bool operator <=(Time time1, Time time2)
        {
            return time1.duration <= time2.duration;
        }

        public static bool operator ==(Time left, Time right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Time left, Time right)
        {
            return !Equals(left, right);
        }

        public static bool operator >(Time time1, Time time2)
        {
            return time1.duration > time2.duration;
        }

        public static bool operator >=(Time time1, Time time2)
        {
            return time1.duration >= time2.duration;
        }

        public DateTime ToDateTime(Date date)
        {
            return ToDateTime(date.DateTime);
        }

        public DateTime ToDateTime(DateTime dateTime)
        {
            return ToDateTime(dateTime, this);
        }

        public static DateTime ToDateTime(Date date, Time time)
        {
            return ToDateTime(date.DateTime, time);
        }

        public static DateTime ToDateTime(DateTime dateTime, Time time)
        {
            return dateTime.Date.Add(time.Duration);
        }

        // members
        private readonly TimeSpan duration;
    }
}
// -- EOF -------------------------------------------------------------------
