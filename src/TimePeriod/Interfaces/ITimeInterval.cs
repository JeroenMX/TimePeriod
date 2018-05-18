// -- FILE ------------------------------------------------------------------
// name       : ITimeInterval.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.05.06
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using TimePeriod.Enums;

namespace TimePeriod.Interfaces
{
    public interface ITimeInterval : ITimePeriod
    {
        bool IsStartOpen { get; }
        bool IsEndOpen { get; }
        bool IsOpen { get; }
        bool IsStartClosed { get; }
        bool IsEndClosed { get; }
        bool IsClosed { get; }
        bool IsEmpty { get; }
        bool IsDegenerate { get; }
        bool IsIntervalEnabled { get; }
        DateTime StartInterval { get; set; }
        DateTime EndInterval { get; set; }
        IntervalEdge StartEdge { get; set; }
        IntervalEdge EndEdge { get; set; }
        void Move(TimeSpan offset);
        void ExpandStartTo(DateTime moment);
        void ExpandEndTo(DateTime moment);
        void ExpandTo(DateTime moment);
        void ExpandTo(ITimePeriod period);
        void ShrinkStartTo(DateTime moment);
        void ShrinkEndTo(DateTime moment);
        void ShrinkTo(ITimePeriod period);
        ITimeInterval Copy(TimeSpan offset);
    }
}