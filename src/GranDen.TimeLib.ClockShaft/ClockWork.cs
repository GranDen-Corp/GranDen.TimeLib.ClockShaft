using Microsoft.Win32.SafeHandles;
using System;

namespace GranDen.TimeLib.ClockShaft
{
    public delegate IShaft InitShaftDelegate(IShaft shaftInstance);

    public static class ClockWork
    {
        private static InitShaftDelegate _initShaftDelegate;

        public static InitShaftDelegate Initializer
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

        public static IDateTime DateTime { get => Shaft.Instance; }
    }

    #region Singleton Shaft class

    internal class Shaft : IShaft, IDateTime
    {
        protected internal static Lazy<Shaft> LazyInstance = new Lazy<Shaft>(GenerateShaftFactory(ClockWork.Initializer));

        public static bool IsCreated()
        {
            return LazyInstance.IsValueCreated;
        }

        protected internal static void ReAssignLazyInstance(InitShaftDelegate initializeDelegate)
        {
            LazyInstance = new Lazy<Shaft>(GenerateShaftFactory(initializeDelegate));
        }

        public static Shaft Instance { get => LazyInstance.Value; }

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