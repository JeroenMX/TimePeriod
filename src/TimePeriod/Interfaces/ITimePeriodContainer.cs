// -- FILE ------------------------------------------------------------------
// name       : ITimePeriodContainer.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace TimePeriod.Interfaces
{
    public interface ITimePeriodContainer : IList<ITimePeriod>, ITimePeriod
    {
        new bool IsReadOnly { get; }
        bool ContainsPeriod(ITimePeriod test);
        void AddAll(IEnumerable<ITimePeriod> periods);
        void Move(TimeSpan delta);
    }
}
