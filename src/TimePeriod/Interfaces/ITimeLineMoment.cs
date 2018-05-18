// -- FILE ------------------------------------------------------------------
// name       : ITimeLineMoment.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.03.31
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;

namespace TimePeriod.Interfaces
{
    public interface ITimeLineMoment
    {
        DateTime Moment { get; }
        int BalanceCount { get; }
        int StartCount { get; }
        int EndCount { get; }
        bool IsEmpty { get; }
        void AddStart();
        void RemoveStart();
        void AddEnd();
        void RemoveEnd();
    }
}
