using System;
using GranDen.TimeLib.ClockShaft;
using TestUtil;
using Xunit;

namespace ClockShaftTests
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
}