// -- FILE ------------------------------------------------------------------
// name       : DaySeeker.cs
// project    : Itenso Time Period
// created    : Jani Giannoudis - 2011.03.21
// language   : C# 4.0
// environment: .NET 2.0
// copyright  : (c) 2011-2012 by Itenso GmbH, Switzerland
// ------------------------------------------------------------------------

using TimePeriod.Enums;
using TimePeriod.Interfaces;

namespace TimePeriod
{
    public class DaySeeker : CalendarVisitor<CalendarVisitorFilter, DaySeekerContext>
    {
        public DaySeeker(SeekDirection seekDirection = SeekDirection.Forward, ITimeCalendar calendar = null)
            : this(new CalendarVisitorFilter(), seekDirection, calendar)
        { }

        public DaySeeker(CalendarVisitorFilter filter, SeekDirection seekDirection = SeekDirection.Forward, ITimeCalendar calendar = null) 
            : base(filter, TimeRange.Anytime, seekDirection, calendar)
        { }

        public ITimePeriodCollection Periods { get; } = new TimePeriodCollection();

        public Day FindDay(Day start, int offset)
        {
            if (offset == 0)
            {
                return start;
            }
            DaySeekerContext context = new DaySeekerContext(start, offset);
            SeekDirection visitDirection = SeekDirection;
            // reverse seek direction
            if (offset < 0)
            {
                visitDirection = (visitDirection == SeekDirection.Forward) ?
                    SeekDirection.Backward :
                    SeekDirection.Forward;
            }
            StartDayVisit(start, context, visitDirection);
            return context.FoundDay;
        }

        protected override bool EnterYears(Years years, DaySeekerContext context)
        {
            return !context.IsFinished;
        }

        protected override bool EnterMonths(Year year, DaySeekerContext context)
        {
            return !context.IsFinished;
        }

        protected override bool EnterDays(Month month, DaySeekerContext context)
        {
            return !context.IsFinished;
        }

        protected override bool EnterHours(Day day, DaySeekerContext context)
        {
            return !context.IsFinished;
        }

        protected override bool OnVisitDay(Day day, DaySeekerContext context)
        {
            if (context.IsFinished)
            {
                return false;
            }
            if (day.IsSamePeriod(context.StartDay))
            {
                return true; // ignore the start day
            }
            if (IsMatchingDay(day, context) == false)
            {
                return true;
            }
            if (CheckLimits(day) == false)
            {
                return true;
            }
            context.ProcessDay(day);
            return !context.IsFinished; // abort condition
        }
    }
}
