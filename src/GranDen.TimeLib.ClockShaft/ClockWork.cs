using Microsoft.Win32.SafeHandles;
using System;

namespace GranDen.TimeLib.ClockShaft
{
    /// <summary>
    /// Init Shaft parameter delegate function type
    /// </summary>
    /// <param name="shaftInstance"></param>
    public delegate IShaft ConfigShaftDelegate(IShaft shaftInstance);

    /// <summary>
    /// Library global access &amp; control point
    /// </summary>
    public static class ClockWork
    {
        private static ConfigShaftDelegate _configShaftDelegate;

        /// <summary>
        /// Shaft configuration function
        /// </summary>
        public static ConfigShaftDelegate ShaftConfigurationFunc
        {
            get
            {
                return _configShaftDelegate;
            }
            set
            {
                _configShaftDelegate = value;
                Shaft.ReAssignLazyInstance(value);
            }
        }

        /// <summary>
        /// Reset Shaft drift value
        /// </summary>
        public static void Reset()
        {
            Shaft.ReAssignLazyInstance(instance => instance);
        }

        /// <summary>
        /// Mimic property to act like <c>DateTime</c> 
        /// </summary>
        public static IDateTime DateTime { get => Shaft.SingletonInstance; }
        
        /// <summary>
        /// Mimic property to act like <c>DateTimeOffset</c>
        /// </summary>
        public static IDateTimeOffset DateTimeOffset { get => Shaft.SingletonInstance; }
    }

    #region Singleton Shaft class

    internal class Shaft : IShaft, IDateTime, IDateTimeOffset
    {
        protected static Lazy<Shaft> LazyInstance = new Lazy<Shaft>(GenerateShaftFactory(ClockWork.ShaftConfigurationFunc));

        public static bool IsCreated()
        {
            return LazyInstance.IsValueCreated;
        }

        protected internal static void ReAssignLazyInstance(ConfigShaftDelegate initializeDelegate)
        {
            LazyInstance = new Lazy<Shaft>(GenerateShaftFactory(initializeDelegate));
        }

        public static Shaft SingletonInstance { get => LazyInstance.Value; }

        private Shaft()
        {
        }

        public DateTime Now
        {
            get
            {
                if (ShiftTimeSpan.HasValue)
                {
                    return Backward.HasValue && Backward.Value
                        ? DateTime.Now.Subtract(ShiftTimeSpan.Value)
                        : DateTime.Now.Add(ShiftTimeSpan.Value);
                }

                return DateTime.Now;
            }
        }

        DateTimeOffset IDateTimeOffset.Now
        {
            get
            {
                
                if (ShiftTimeSpan.HasValue)
                {
                    return Backward.HasValue && Backward.Value
                        ? DateTimeOffset.Now.Subtract(ShiftTimeSpan.Value)
                        : DateTimeOffset.Now.Add(ShiftTimeSpan.Value);
                }

                return DateTimeOffset.Now; 
            }
        }


        public DateTime UtcNow
        {
            get
            {
                if (ShiftTimeSpan.HasValue)
                {
                    return Backward.HasValue && Backward.Value
                        ? DateTime.UtcNow.Subtract(ShiftTimeSpan.Value)
                        : DateTime.UtcNow.Add(ShiftTimeSpan.Value);
                }

                return DateTime.UtcNow;
            }
        }

        DateTimeOffset IDateTimeOffset.UtcNow
        {
            get
            {
                if (ShiftTimeSpan.HasValue)
                {
                    return Backward.HasValue && Backward.Value
                        ? DateTimeOffset.UtcNow.Subtract(ShiftTimeSpan.Value)
                        : DateTimeOffset.UtcNow.Add(ShiftTimeSpan.Value);
                }

                return DateTimeOffset.UtcNow;
            }
        }


        public bool? Backward { get; set; }

        public TimeSpan? ShiftTimeSpan { get; set; }

        private static Func<Shaft> GenerateShaftFactory(ConfigShaftDelegate shaftDelegate)
        {
            var instance = new Shaft();
            Func<Shaft> fac;

            if (shaftDelegate != null)
            {
                fac = () =>
                {
                    var ret = shaftDelegate(instance);
                    return (Shaft)ret;
                };
            }
            else
            {
                fac = () => instance;
            }

            return fac;
        }
    }

    #endregion
}