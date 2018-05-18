// -- FILE ------------------------------------------------------------------
// name       : HourRange.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.02.18
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

namespace TimePeriod
{
    public class HourRange
    {
        public HourRange(int hour) 
            : this(hour, hour)
        { }

        public HourRange(int startHour, int endHour) 
            : this(new Time(startHour), new Time(endHour))
        { }

        public HourRange(Time start, Time end)
        {
            if (start.Ticks <= end.Ticks)
            {
                this.start = start;
                this.end = end;
            }
            else
            {
                this.end = start;
                this.start = end;
            }
        }

        public Time Start
        {
            get { return start; }
        }

        public Time End
        {
            get { return end; }
        }

        public bool IsMoment
        {
            get { return start.Equals(end); }
        }

        public override string ToString()
        {
            return start + " - " + end;
        }

        // members
        private readonly Time start;
        private readonly Time end;
    }
}
