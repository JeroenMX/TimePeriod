// -- FILE ------------------------------------------------------------------
// name       : AmbiguousMomentException.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2013.11.03
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;

namespace TimePeriod
{
    public class AmbiguousMomentException : Exception
    {
        public DateTime Moment { get; }

        public AmbiguousMomentException(DateTime moment, string message, Exception cause) :
            base(message, cause)
        {
            this.Moment = moment;
        }

        public AmbiguousMomentException(DateTime moment)
        {
            this.Moment = moment;
        }

        public AmbiguousMomentException(DateTime moment, string message) :
            base(message)
        {
            this.Moment = moment;
        }

        public AmbiguousMomentException(DateTime moment, Exception cause) :
            base(cause.Message, cause)
        {
            this.Moment = moment;
        }
    }
}