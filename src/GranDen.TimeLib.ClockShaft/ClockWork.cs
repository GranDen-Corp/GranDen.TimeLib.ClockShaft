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
        /// Reset Clock shaft drift value
        /// </summary>
        public static void Reset()
        {
            Shaft.ReAssignLazyInstance(Shaft.DefaultConfigShaftDelegate);
        }

        /// <summary>
        /// Mimic property to act like <c>DateTime</c> 
        /// </summary>
        public static IDateTime DateTime { get => Shaft.SingletonInstance; }

        /// <summary>
        /// Mimic property to act like <c>DateTimeOffset</c>
        /// </summary>
        public static IDateTimeOffset DateTimeOffset { get => Shaft.SingletonInstance; }

        /// <summary>
        /// Get to know if <c>Shaft</c> lazy singleton instance is created.
        /// </summary>
        public static bool ShaftInitialized { get => Shaft.IsCreated(); }
    }

    #region Singleton Shaft class

    internal class Shaft : IShaft, IDateTime, IDateTimeOffset
    {
        private static Lazy<Shaft> _lazyInstance = new Lazy<Shaft>(GenerateShaftFactory(ClockWork.ShaftConfigurationFunc));

        public static readonly ConfigShaftDelegate DefaultConfigShaftDelegate = instance => instance;

        public static bool IsCreated()
        {
            return _lazyInstance.IsValueCreated;
        }

        protected internal static void ReAssignLazyInstance(ConfigShaftDelegate initializeDelegate)
        {
            _lazyInstance = new Lazy<Shaft>(GenerateShaftFactory(initializeDelegate));
        }

        public static Shaft SingletonInstance { get => _lazyInstance.Value; }

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

            Shaft LazyShaftInitFunc()
            {
                if (shaftDelegate == null) { shaftDelegate = DefaultConfigShaftDelegate; }

                var ret = shaftDelegate(instance);
                return (Shaft)ret;
            }

            return LazyShaftInitFunc;
        }
    }

    #endregion
}