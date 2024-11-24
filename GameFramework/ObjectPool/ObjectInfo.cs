using System;
using System.Runtime.InteropServices;

namespace GameFramework
{
    /// <summary>
    /// 对象信息。
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public readonly struct ObjectInfo
    {
        /// <summary>
        /// 获取对象名称。
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// 获取对象是否被加锁。
        /// </summary>
        public readonly bool Locked;

        /// <summary>
        /// 获取对象自定义释放检查标记。
        /// </summary>
        public readonly bool CustomCanReleaseFlag;

        /// <summary>
        /// 获取对象的优先级。
        /// </summary>
        public readonly int Priority;

        /// <summary>
        /// 获取对象上次使用时间。
        /// </summary>
        public readonly DateTime LastUseTime;

        /// <summary>
        /// 获取对象是否正在使用。
        /// </summary>
        public readonly bool IsInUse => SpawnCount > 0;

        /// <summary>
        /// 获取对象的获取计数。
        /// </summary>
        public readonly int SpawnCount;

        /// <summary>
        /// 初始化对象信息的新实例。
        /// </summary>
        /// <param name="name">对象名称。</param>
        /// <param name="locked">对象是否被加锁。</param>
        /// <param name="customCanReleaseFlag">对象自定义释放检查标记。</param>
        /// <param name="priority">对象的优先级。</param>
        /// <param name="lastUseTime">对象上次使用时间。</param>
        /// <param name="spawnCount">对象的获取计数。</param>
        public ObjectInfo(string name, bool locked, bool customCanReleaseFlag, int priority, DateTime lastUseTime, int spawnCount)
        {
            Name = name;
            Locked = locked;
            CustomCanReleaseFlag = customCanReleaseFlag;
            Priority = priority;
            LastUseTime = lastUseTime;
            SpawnCount = spawnCount;
        }
    }
}