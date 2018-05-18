// -- FILE ------------------------------------------------------------------
// name       : InvalidMomentException.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2013.11.03
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------
using System;
namespace TimePeriod
{
    public class InvalidMomentException : Exception
    {
        public InvalidMomentException(DateTime moment)
        {
            this.moment = moment;
        }

        public InvalidMomentException(DateTime moment, string message) 
            : base(message)
        {
            this.moment = moment;
        }

        public InvalidMomentException(DateTime moment, Exception cause) 
            : base(cause.Message, cause)
        {
            this.moment = moment;
        }

        public InvalidMomentException(DateTime moment, string message, Exception cause) 
            : base(message, cause)
        {
            this.moment = moment;
        }

        public DateTime Moment
        {
            get { return moment; }
        }

        // members
        private readonly DateTime moment;
    }
}
