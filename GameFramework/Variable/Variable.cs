using System;

namespace GameFramework
{
    /// <summary>
    /// 变量。
    /// </summary>
    public abstract class Variable : IMemory
    {
        /// <summary>
        /// 初始化变量的新实例。
        /// </summary>
        protected Variable()
        {
        }

        /// <summary>
        /// 获取变量类型。
        /// </summary>
        public abstract Type Type { get; }

        /// <summary>
        /// 获取变量值。
        /// </summary>
        /// <returns>变量值。</returns>
        public abstract object GetValue();

        /// <summary>
        /// 设置变量值。
        /// </summary>
        /// <param name="value">变量值。</param>
        public abstract void SetValue(object value);

        /// <summary>
        /// 清理变量值。
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// 从 System.Boolean 到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(bool value)
        {
            VarBoolean varValue = MemoryPool.Acquire<VarBoolean>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Byte 到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(byte value)
        {
            VarByte varValue = MemoryPool.Acquire<VarByte>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Byte 数组到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(byte[] value)
        {
            VarByteArray varValue = MemoryPool.Acquire<VarByteArray>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Char 到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(char value)
        {
            VarChar varValue = MemoryPool.Acquire<VarChar>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Char 数组到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(char[] value)
        {
            VarCharArray varValue = MemoryPool.Acquire<VarCharArray>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.DateTime 到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(DateTime value)
        {
            VarDateTime varValue = MemoryPool.Acquire<VarDateTime>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Decimal 到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(decimal value)
        {
            VarDecimal varValue = MemoryPool.Acquire<VarDecimal>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Double 到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(double value)
        {
            VarDouble varValue = MemoryPool.Acquire<VarDouble>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Int16 到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(short value)
        {
            VarInt16 varValue = MemoryPool.Acquire<VarInt16>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Int32 到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(int value)
        {
            VarInt32 varValue = MemoryPool.Acquire<VarInt32>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Int64 到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(long value)
        {
            VarInt64 varValue = MemoryPool.Acquire<VarInt64>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.SByte 到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(sbyte value)
        {
            VarSByte varValue = MemoryPool.Acquire<VarSByte>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Single 到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(float value)
        {
            VarSingle varValue = MemoryPool.Acquire<VarSingle>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.String 到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(string value)
        {
            VarString varValue = MemoryPool.Acquire<VarString>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.UInt16 到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(ushort value)
        {
            VarUInt16 varValue = MemoryPool.Acquire<VarUInt16>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.UInt32 到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(uint value)
        {
            VarUInt32 varValue = MemoryPool.Acquire<VarUInt32>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.UInt64 到 GameFramework.Variable 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Variable(ulong value)
        {
            VarUInt64 varValue = MemoryPool.Acquire<VarUInt64>();
            varValue.Value = value;
            return varValue;
        }
    }
}