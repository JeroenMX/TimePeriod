// -- FILE ------------------------------------------------------------------
// name       : DayRange.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------
using System;
namespace TimePeriod
{
    public struct DayRange
    {
        public DayRange(int day) 
            : this(day, day)
        { }

        public DayRange(int min, int max)
        {
            if (min < 1 || min > TimeSpec.MaxDaysPerMonth)
            {
                throw new ArgumentOutOfRangeException("min");
            }
            if (max < min || max > TimeSpec.MaxDaysPerMonth)
            {
                throw new ArgumentOutOfRangeException("max");
            }
            this.Min = min;
            this.Max = max;
        }

        public int Min { get; }

        public int Max { get; }

        public bool IsSingleDay => Min == Max;

        public bool HasInside(int test)
        {
            return test >= Min && test <= Max;
        }
    }
}

