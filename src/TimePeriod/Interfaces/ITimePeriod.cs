// -- FILE ------------------------------------------------------------------
// name       : ITimePeriod.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using TimePeriod.Enums;

namespace TimePeriod.Interfaces
{
    public interface ITimePeriod
    {
        bool HasStart { get; }
        DateTime Start { get; }
        bool HasEnd { get; }
        DateTime End { get; }
        TimeSpan Duration { get; }
        string DurationDescription { get; }
        bool IsMoment { get; }
        bool IsAnytime { get; }
        bool IsReadOnly { get; }
        TimeSpan GetDuration(IDurationProvider provider);
        void Setup(DateTime newStart, DateTime newEnd);
        bool IsSamePeriod(ITimePeriod test);
        bool HasInside(DateTime test);
        bool HasInside(ITimePeriod test);
        bool IntersectsWith(ITimePeriod test);
        bool OverlapsWith(ITimePeriod test);
        PeriodRelation GetRelation(ITimePeriod test);
        int CompareTo(ITimePeriod other, ITimePeriodComparer comparer);
        string GetDescription(ITimeFormatter formatter);
    }
}
