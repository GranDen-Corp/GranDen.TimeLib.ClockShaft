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
        public void ClockWorkSetEarly1hou()
        {
            //Arrange
            ClockWork.Initializer = instance => {
                instance.Backward = true;
                instance.ShiftTimeSpan = new TimeSpan(1, 0, 0);
                return instance;
            };

            //Arrange
            var now = DateTime.Now;
         
            var shaftNow = ClockWork.DateTime.Now;

            //Assert
            Assert.Equal(now, shaftNow.Add(new TimeSpan(1, 0, 0)), TimeSpan.FromMilliseconds(10.0));
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
