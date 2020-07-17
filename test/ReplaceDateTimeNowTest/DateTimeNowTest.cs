using GranDen.TimeLib.ClockShaft;
using System;
using Xunit;

namespace ReplaceDateTimeNowTest
{
    public class DateTimeNowTest
    {
        public DateTimeNowTest()
        {
            ClockWork.Reset();
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
        public void ClockWorkSetEarly1Hour()
        {
            //Arrange
            TimeSpan oneHourSpan = new TimeSpan(1, 0, 0);

            ClockWork.ShaftInitializer = instance =>
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
        public void ClockworkSetLate1Hour()
        {
            //Arrange
            TimeSpan oneHourSpan = new TimeSpan(1, 0, 0);

            ClockWork.ShaftInitializer = shaft =>
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
    }
}