using Microsoft.Win32.SafeHandles;
using System;

namespace GranDen.TimeLib.ClockShaft
{
    public delegate IShaft InitShaftDelegate(IShaft shaftInstance);

    public static class ClockWork
    {
        private static InitShaftDelegate _initShaftDelegate;

        public static InitShaftDelegate ShaftInitializer
        {
            get
            {
                return _initShaftDelegate;
            }
            set
            {
                _initShaftDelegate = value;
                Shaft.ReAssignLazyInstance(value);
            }
        }

        public static void Reset()
        {
            Shaft.ReAssignLazyInstance(instance => instance);
        }

        public static IDateTime DateTime { get => Shaft.SingletonInstance; }
        
        public static IDateTimeOffset DateTimeOffset { get => Shaft.SingletonInstance; }
    }

    #region Singleton Shaft class

    internal class Shaft : IShaft, IDateTime, IDateTimeOffset
    {
        protected static Lazy<Shaft> LazyInstance = new Lazy<Shaft>(GenerateShaftFactory(ClockWork.ShaftInitializer));

        public static bool IsCreated()
        {
            return LazyInstance.IsValueCreated;
        }

        protected internal static void ReAssignLazyInstance(InitShaftDelegate initializeDelegate)
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

        private static Func<Shaft> GenerateShaftFactory(InitShaftDelegate shaftDelegate)
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