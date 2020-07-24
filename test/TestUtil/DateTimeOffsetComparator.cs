using System;
using System.Collections.Generic;

namespace TestUtil
{
    public class DateTimeOffsetComparator : IEqualityComparer<DateTimeOffset>
    {
        private readonly TimeSpan _fraction;

        public DateTimeOffsetComparator(double fractionMilliseconds)
        {
            _fraction = TimeSpan.FromMilliseconds(fractionMilliseconds);
        }

        public bool Equals(DateTimeOffset expected, DateTimeOffset actual)
        {
            var compareResult = expected.CompareTo(actual);

            if (compareResult == 0) { return true; }

            var diff = compareResult > 0 ? expected.Subtract(actual) : actual.Subtract(expected);

            return diff <= _fraction;
        }

        public int GetHashCode(DateTimeOffset obj)
        {
            return obj.GetHashCode();
        }
    }
}