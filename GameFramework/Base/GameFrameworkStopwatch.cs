using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameFramework
{
    /// <summary>
    /// 游戏框架计时器。
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public readonly struct GameFrameworkStopwatch
    {
        private static readonly double _timestampToTicks = TimeSpan.TicksPerSecond / (double)Stopwatch.Frequency;

        private readonly long _startTimestamp;

        private GameFrameworkStopwatch(long startTimestamp)
        {
            _startTimestamp = startTimestamp;
        }

        /// <summary>
        /// 初始化游戏框架计时器的新实例。
        /// </summary>
        /// <returns></returns>
        public static GameFrameworkStopwatch StartNew() => new(Stopwatch.GetTimestamp());

        /// <summary>
        /// 获取游戏框架计时器是否无效。
        /// </summary>
        public bool IsInvalid => _startTimestamp == 0;

        /// <summary>
        /// A read-only TimeSpan representing the total elapsed time measured by the current instance.
        /// </summary>
        public TimeSpan Elapsed => TimeSpan.FromTicks(ElapsedTicks);

        /// <summary>
        /// Gets the total elapsed time measured by the current instance, in seconds.
        /// </summary>
        /// <remarks></remarks>
        public long ElapsedTicks
        {
            get
            {
                if (_startTimestamp == 0)
                {
                    throw new InvalidOperationException("Detected invalid initialization(use 'default'), only to create from StartNew().");
                }

                var delta = Stopwatch.GetTimestamp() - _startTimestamp;
                return (long)(delta * _timestampToTicks);
            }
        }
    }
}