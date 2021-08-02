using System;

namespace TaskOne.Models
{
    class DateTimeRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateTimeRange(DateTime start, DateTime end)
        {
            this.Start = start;
            this.End = end;
        }

        public bool Intersects(DateTimeRange test)
        {
            if (this.Start > this.End || test.Start > test.End)
                throw new InvalidOperationException();

            if (this.Start == this.End || test.Start == test.End)
                return false;

            if (this.Start == test.Start || this.End == test.End)
                return true;

            if (this.Start < test.Start)
            {
                if (this.End > test.Start && this.End < test.End)
                    return true;

                if (this.End > test.End)
                    return true;
            }
            else
            {
                if (test.End > this.Start && test.End < this.End)
                    return true;

                if (test.End > this.End)
                    return true;
            }

            return false;
        }
    }
}
