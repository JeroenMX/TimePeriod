// -- FILE ------------------------------------------------------------------
// name       : DayHourRange.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.08.21
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using System;

namespace TimePeriod
{
    public class DayHourRange : HourRange
    {
        public DayHourRange(DayOfWeek day, int hour)
            : base(hour, hour)
        {
            this.Day = day;
        }

        public DayHourRange(DayOfWeek day, int startHour, int endHour) 
            : base(new Time(startHour), new Time(endHour))
        {
            this.Day = day;
        }

        public DayHourRange(DayOfWeek day, Time start, Time end) 
            : base(start, end)
        {
            this.Day = day;
        }

        public DayOfWeek Day { get; }

        public override string ToString()
        {
            return Day + ": " + base.ToString();
        }
    }
}
