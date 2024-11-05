namespace GameFramework
{
    /// <summary>
    /// 内存池对象接口。
    /// </summary>
    public interface IMemory
    {
        /// <summary>
        /// 清理内存对象回收入池。
        /// </summary>
        void Clear();
    }
}