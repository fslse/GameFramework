using System;
using System.Runtime.InteropServices;

namespace GameFramework
{
    /// <summary>
    /// 内存池信息。
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    public readonly struct MemoryPoolInfo
    {
        /// <summary>
        /// 获取内存池类型。
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// 获取未使用内存对象数量。
        /// </summary>
        public readonly int UnusedMemoryCount;

        /// <summary>
        /// 获取正在使用内存对象数量。
        /// </summary>
        public readonly int UsingMemoryCount;

        /// <summary>
        /// 获取获取内存对象数量。
        /// </summary>
        public readonly int AcquireMemoryCount;

        /// <summary>
        /// 获取归还内存对象数量。
        /// </summary>
        public readonly int ReleaseMemoryCount;

        /// <summary>
        /// 获取增加内存对象数量。
        /// </summary>
        public readonly int AddMemoryCount;

        /// <summary>
        /// 获取移除内存对象数量。
        /// </summary>
        public readonly int RemoveMemoryCount;

        /// <summary>
        /// 初始化内存池信息的新实例。
        /// </summary>
        /// <param name="type">内存池类型。</param>
        /// <param name="unusedMemoryCount">未使用内存对象数量。</param>
        /// <param name="usingMemoryCount">正在使用内存对象数量。</param>
        /// <param name="acquireMemoryCount">获取内存对象数量。</param>
        /// <param name="releaseMemoryCount">归还内存对象数量。</param>
        /// <param name="addMemoryCount">增加内存对象数量。</param>
        /// <param name="removeMemoryCount">移除内存对象数量。</param>
        public MemoryPoolInfo(Type type, int unusedMemoryCount, int usingMemoryCount, int acquireMemoryCount, int releaseMemoryCount, int addMemoryCount, int removeMemoryCount)
        {
            Type = type;
            UnusedMemoryCount = unusedMemoryCount;
            UsingMemoryCount = usingMemoryCount;
            AcquireMemoryCount = acquireMemoryCount;
            ReleaseMemoryCount = releaseMemoryCount;
            AddMemoryCount = addMemoryCount;
            RemoveMemoryCount = removeMemoryCount;
        }
    }
}