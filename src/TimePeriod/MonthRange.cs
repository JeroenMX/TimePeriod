// -- FILE ------------------------------------------------------------------
// name       : MonthRange.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using TimePeriod.Enums;

namespace TimePeriod
{
    public struct MonthRange
    {
        public MonthRange(YearMonth month) 
            : this(month, month)
        { }

        public MonthRange(YearMonth min, YearMonth max)
        {
            if (max < min)
            {
                throw new ArgumentOutOfRangeException("max");
            }
            this.Min = min;
            this.Max = max;
        }

        public YearMonth Min { get; }
        public YearMonth Max { get; }

        public bool IsSingleMonth => Min == Max;

        public bool HasInside(YearMonth test)
        {
            return test >= Min && test <= Max;
        }
    }
}
