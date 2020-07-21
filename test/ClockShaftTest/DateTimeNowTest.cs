using GranDen.TimeLib.ClockShaft;
using System;
using Xunit;

namespace ClockShaftTest
{
    [Collection("Test should not parallel on different test classes")]
    public class DateTimeNowTest
    {
        public DateTimeNowTest()
        {
            if (ClockWork.ShaftInitialized)
            {
                ClockWork.Reset();
            }
        }

        [Fact]
        public void ClockWorkInitializerNotSet_ActLikeNormalDateTimeNow()
        {
            //Arrange
            var now = DateTime.Now;

            //Act
            var shaftNow = ClockWork.DateTime.Now;

            //Assert
            Assert.Equal(now, shaftNow, TimeSpan.FromMilliseconds(10.0));
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
            var now = DateTime.Now;
            var shaftNow = ClockWork.DateTime.Now;

            //Assert
            Assert.Equal(now, shaftNow.Add(oneHourSpan), TimeSpan.FromMilliseconds(10.0));
        }

        [Fact]
        public void ClockworkSetNow_1HourLater()
        {
            //Arrange
            TimeSpan oneHourSpan = new TimeSpan(1, 0, 0);

            ClockWork.ShaftConfigurationFunc = shaft =>
            {
                shaft.ShiftTimeSpan = oneHourSpan;
                return shaft;
            };

            //Act
            var now = DateTime.Now;
            var shaftNow = ClockWork.DateTime.Now;

            //Assert
            Assert.Equal(now, shaftNow.Subtract(oneHourSpan), TimeSpan.FromMilliseconds(10.0));
        }

        [Fact]
        public void ClockWorkInitializerNotSet_ActLikeNormalDateTimeUtcNow()
        {
            //Arrange
            var utcNow = DateTime.UtcNow;

            //Act
            var shaftUtcNow = ClockWork.DateTime.UtcNow;

            //Assert
            Assert.Equal(utcNow, shaftUtcNow, TimeSpan.FromMilliseconds(10.0));
        }

        [Fact]
        public void ClockWorkSetUtc_1HourEarlier()
        {
            
            //Arrange
            TimeSpan oneHourSpan = new TimeSpan(1, 0, 0);

            ClockWork.ShaftConfigurationFunc = shaft =>
            {
                shaft.Backward = true;
                shaft.ShiftTimeSpan = oneHourSpan;
                return shaft;
            };

            //Act
            var now = DateTime.UtcNow;
            var shaftNow = ClockWork.DateTime.UtcNow;

            //Assert
            Assert.Equal(now, shaftNow.Add(oneHourSpan), TimeSpan.FromMilliseconds(10.0));
        }
    }
}