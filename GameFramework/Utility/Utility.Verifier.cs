﻿using System;
using System.IO;

namespace GameFramework
{
    public static partial class Utility
    {
        /// <summary>
        /// 校验相关的实用函数。
        /// </summary>
        public static partial class Verifier
        {
            private const int CachedBytesLength = 0x1000;
            private static readonly byte[] _cachedBytes = new byte[CachedBytesLength];
            private static readonly Crc32 _algorithm = new();

            /// <summary>
            /// 计算二进制流的 CRC32。
            /// </summary>
            /// <param name="bytes">指定的二进制流。</param>
            /// <returns>计算后的 CRC32。</returns>
            public static int GetCrc32(byte[] bytes)
            {
                if (bytes == null)
                {
                    throw new GameFrameworkException("Bytes is invalid.");
                }

                return GetCrc32(bytes, 0, bytes.Length);
            }

            /// <summary>
            /// 计算二进制流的 CRC32。
            /// </summary>
            /// <param name="bytes">指定的二进制流。</param>
            /// <param name="offset">二进制流的偏移。</param>
            /// <param name="length">二进制流的长度。</param>
            /// <returns>计算后的 CRC32。</returns>
            public static int GetCrc32(byte[] bytes, int offset, int length)
            {
                if (bytes == null)
                {
                    throw new GameFrameworkException("Bytes is invalid.");
                }

                if (offset < 0 || length < 0 || offset + length > bytes.Length)
                {
                    throw new GameFrameworkException("Offset or length is invalid.");
                }

                _algorithm.HashCore(bytes, offset, length);
                int result = (int)_algorithm.HashFinal();
                _algorithm.Initialize();
                return result;
            }

            /// <summary>
            /// 计算二进制流的 CRC32。
            /// </summary>
            /// <param name="stream">指定的二进制流。</param>
            /// <returns>计算后的 CRC32。</returns>
            public static int GetCrc32(Stream stream)
            {
                if (stream == null)
                {
                    throw new GameFrameworkException("Stream is invalid.");
                }

                while (true)
                {
                    int bytesRead = stream.Read(_cachedBytes, 0, CachedBytesLength);
                    if (bytesRead > 0)
                    {
                        _algorithm.HashCore(_cachedBytes, 0, bytesRead);
                    }
                    else
                    {
                        break;
                    }
                }

                int result = (int)_algorithm.HashFinal();
                _algorithm.Initialize();
                Array.Clear(_cachedBytes, 0, CachedBytesLength);
                return result;
            }

            /// <summary>
            /// 获取 CRC32 数值的二进制数组。
            /// </summary>
            /// <param name="crc32">CRC32 数值。</param>
            /// <returns>CRC32 数值的二进制数组。</returns>
            public static byte[] GetCrc32Bytes(int crc32)
            {
                return new[] { (byte)((crc32 >> 24) & 0xff), (byte)((crc32 >> 16) & 0xff), (byte)((crc32 >> 8) & 0xff), (byte)(crc32 & 0xff) };
            }

            /// <summary>
            /// 获取 CRC32 数值的二进制数组。
            /// </summary>
            /// <param name="crc32">CRC32 数值。</param>
            /// <param name="bytes">要存放结果的数组。</param>
            /// <param name="offset">CRC32 数值的二进制数组在结果数组内的起始位置。</param>
            public static void GetCrc32Bytes(int crc32, byte[] bytes, int offset = 0)
            {
                if (bytes == null)
                {
                    throw new GameFrameworkException("Result is invalid.");
                }

                if (offset < 0 || offset + 4 > bytes.Length)
                {
                    throw new GameFrameworkException("Offset or length is invalid.");
                }

                bytes[offset] = (byte)((crc32 >> 24) & 0xff);
                bytes[offset + 1] = (byte)((crc32 >> 16) & 0xff);
                bytes[offset + 2] = (byte)((crc32 >> 8) & 0xff);
                bytes[offset + 3] = (byte)(crc32 & 0xff);
            }
        }
    }
}