// -- FILE ------------------------------------------------------------------
// name       : TimePeriodReversComparer.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2013.10.26
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using TimePeriod.Interfaces;

namespace TimePeriod
{

    public sealed class TimePeriodReversComparer : ITimePeriodComparer
    {
        public TimePeriodReversComparer(ITimePeriodComparer baseComparer)
        {
            this.BaseComparer = baseComparer;
        }

        public ITimePeriodComparer BaseComparer { get; }

        public int Compare(ITimePeriod left, ITimePeriod right)
        {
            return -BaseComparer.Compare(left, right);
        }
    }
}
