using System.Runtime.InteropServices;

namespace GameFramework
{
    /// <summary>
    /// 任务信息。
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public readonly struct TaskInfo
    {
        /// <summary>
        /// 获取任务的序列编号。
        /// </summary>
        public readonly int SerialId;

        /// <summary>
        /// 获取任务的标签。
        /// </summary>
        public readonly string Tag;

        /// <summary>
        /// 获取任务的优先级。
        /// </summary>
        public readonly int Priority;

        /// <summary>
        /// 获取任务的用户自定义数据。
        /// </summary>
        public readonly object UserData;

        /// <summary>
        /// 获取任务状态。
        /// </summary>
        public readonly ETaskStatus Status;

        /// <summary>
        /// 获取任务描述。
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// 初始化任务信息的新实例。
        /// </summary>
        /// <param name="serialId">任务的序列编号。</param>
        /// <param name="tag">任务的标签。</param>
        /// <param name="priority">任务的优先级。</param>
        /// <param name="userData">任务的用户自定义数据。</param>
        /// <param name="status">任务状态。</param>
        /// <param name="description">任务描述。</param>
        public TaskInfo(int serialId, string tag, int priority, object userData, ETaskStatus status, string description)
        {
            SerialId = serialId;
            Tag = tag;
            Priority = priority;
            UserData = userData;
            Status = status;
            Description = description;
        }
    }
}