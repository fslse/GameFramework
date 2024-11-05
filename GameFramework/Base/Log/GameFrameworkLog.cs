namespace GameFramework
{
    /// <summary>
    /// 游戏框架日志类。
    /// </summary>
    internal static partial class GameFrameworkLog
    {
        private static ILogHelper _logHelper;

        /// <summary>
        /// 设置游戏框架日志辅助器。
        /// </summary>
        /// <param name="logHelper">要设置的游戏框架日志辅助器。</param>
        public static void SetLogHelper(ILogHelper logHelper)
        {
            _logHelper = logHelper;
        }

        /// <summary>
        /// 打印调试级别日志。
        /// </summary>
        /// <param name="message">日志内容。</param>
        public static void LogDebug(object message)
        {
            _logHelper.Log(EGameFrameworkLogLevel.Debug, message.ToString());
        }

        /// <summary>
        /// 打印信息级别日志。
        /// </summary>
        /// <param name="message">日志内容。</param>
        public static void LogInfo(object message)
        {
            _logHelper.Log(EGameFrameworkLogLevel.Info, message.ToString());
        }

        /// <summary>
        /// 打印警告级别日志。
        /// </summary>
        /// <param name="message">日志内容。</param>
        public static void LogWarning(object message)
        {
            _logHelper.Log(EGameFrameworkLogLevel.Warning, message.ToString());
        }

        /// <summary>
        /// 打印错误级别日志。
        /// </summary>
        /// <param name="message">日志内容。</param>
        public static void LogError(object message)
        {
            _logHelper.Log(EGameFrameworkLogLevel.Error, message.ToString());
        }

        /// <summary>
        /// 打印严重错误级别日志。
        /// </summary>
        /// <param name="message">日志内容。</param>
        public static void LogFatal(object message)
        {
            _logHelper.Log(EGameFrameworkLogLevel.Fatal, message.ToString());
        }
    }
}