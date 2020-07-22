﻿using System;
using System.Collections.Generic;
using GranDen.TimeLib.ClockShaft;
using Xunit;

namespace ClockShaftTest
{
    [Collection("Test should not parallel on different test classes")]
    public class DateTimeOffsetTest
    {
        public DateTimeOffsetTest()
        {
            if (ClockWork.ShaftInitialized)
            {
                ClockWork.Reset();
            }
        }


        [Fact]
        public void ClockWorkInitializerNotSet_ActLikeNormalDateTimeOffsetNow()
        {
            //Arrange
            var now = DateTimeOffset.Now;

            //Act
            var shaftNow = ClockWork.DateTimeOffset.Now;

            //Assert
            Assert.Equal(now, shaftNow, new DateTimeOffsetComparator(10.0));
        }

        [Fact]
        public void ClockWorkSetNow_1HourEarlier()
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

        [Fact]
        public void ClockWorkSetNow_1HourLater()
        {
            //Arrange
            TimeSpan oneHourSpan = new TimeSpan(1, 0, 0);

            ClockWork.ShaftConfigurationFunc = instance =>
            {
                instance.Backward = false;
                instance.ShiftTimeSpan = oneHourSpan;
                return instance;
            };

            //Act
            var now = DateTimeOffset.Now;
            var shaftNow = ClockWork.DateTimeOffset.Now;

            //Assert
            Assert.Equal(now, shaftNow.Subtract(oneHourSpan), new DateTimeOffsetComparator(10.0));
        }

        [Fact]
        public void ClockWorkInitializerNotSet_ActLikeNormalDateTimeOffsetUtcNow()
        {
            //Arrange
            var utcNow = DateTimeOffset.UtcNow;

            //Act
            var shaftUtcNow = ClockWork.DateTimeOffset.UtcNow;

            //Assert
            Assert.Equal(utcNow, shaftUtcNow, new DateTimeOffsetComparator(10.0)); 
        }
        
        [Fact]
        public void ClockWorkSetUtcNow_1HourEarlier()
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
            var now = DateTimeOffset.UtcNow;
            var shaftNow = ClockWork.DateTimeOffset.UtcNow;

            //Assert
            Assert.Equal(now, shaftNow.Add(oneHourSpan), new DateTimeOffsetComparator(10.0));
        }
        
        [Fact]
        public void ClockWorkSetUtcNow_1HourLater()
        {
            //Arrange
            TimeSpan oneHourSpan = new TimeSpan(1, 0, 0);

            ClockWork.ShaftConfigurationFunc = instance =>
            {
                instance.Backward = false;
                instance.ShiftTimeSpan = oneHourSpan;
                return instance;
            };

            //Act
            var now = DateTimeOffset.UtcNow;
            var shaftNow = ClockWork.DateTimeOffset.UtcNow;

            //Assert
            Assert.Equal(now, shaftNow.Subtract(oneHourSpan), new DateTimeOffsetComparator(10.0));
        }
    }

    #region DateTimeOffset comparator

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

    #endregion
}