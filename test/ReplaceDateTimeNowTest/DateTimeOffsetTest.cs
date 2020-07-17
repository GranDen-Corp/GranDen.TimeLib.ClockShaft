﻿using System;
using System.Collections.Generic;
using GranDen.TimeLib.ClockShaft;
using Xunit;

namespace ReplaceDateTimeNowTest
{
    public class DateTimeOffsetTest
    {
        public DateTimeOffsetTest()
        {
            ClockWork.Reset();
        } 
        
        
        [Fact]
        public void ClockWorkInitializerNotSet_ActLikeNormalDateTimeNow()
        {
            //Arrange
            var now = DateTimeOffset.Now;

            //Act
            var shaftNow = ClockWork.DateTimeOffset.Now;

            //Assert
            Assert.Equal(now, shaftNow, new DateTimeOffsetComparator(10.0) );
        }

        [Fact]
        public void ClockWorkSetEarly1Hour()
        {
            //Arrange
            TimeSpan oneHourSpan = new TimeSpan(1, 0, 0);

            ClockWork.ShaftConfigurationFunc = instance =>
            {
                instance.Backward = true;
                instance.ShiftTimeSpan = oneHourSpan;
                return instance;
            };

            //Act
            var now = DateTimeOffset.Now;
            var shaftNow = ClockWork.DateTimeOffset.Now;

            //Assert
            Assert.Equal(now, shaftNow.Add(oneHourSpan), new DateTimeOffsetComparator(10.0));
        }

    }

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

            if (compareResult == 0) { return true;}

            var diff = compareResult > 0 ? expected.Subtract(actual) : actual.Subtract(expected);

            return diff <= _fraction;
        }

        public int GetHashCode(DateTimeOffset obj)
        {
            return obj.GetHashCode();
        }
    }
}