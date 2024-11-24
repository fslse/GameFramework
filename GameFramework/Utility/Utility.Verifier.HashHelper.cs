using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace GameFramework
{
    public static partial class Utility
    {
        public static partial class Verifier
        {
            /// <summary>
            /// Hash 辅助器。
            /// </summary>
            /// <remarks>MD5, SHA1, SHA256, SHA384, SHA512</remarks>
            public static class HashHelper
            {
                /// <summary>
                /// 获取二进制流的哈希值。
                /// </summary>
                /// <param name="data">二进制流。</param>
                /// <typeparam name="T">要使用的哈希算法对应的类型。</typeparam>
                /// <returns>二进制流的哈希值。</returns>
                public static byte[] ComputeHash<T>(byte[] data) where T : HashAlgorithm
                {
                    return (HashAlgorithm.Create(typeof(T).Name) as T)!.ComputeHash(data);
                }

                /// <summary>
                /// 获取二进制流的哈希值。
                /// </summary>
                /// <param name="data">二进制流。</param>
                /// <typeparam name="T">要使用的哈希算法对应的类型。</typeparam>
                /// <returns>二进制流的哈希值。</returns>
                public static byte[] ComputeHash<T>(Stream data) where T : HashAlgorithm
                {
                    return (HashAlgorithm.Create(typeof(T).Name) as T)!.ComputeHash(data);
                }

                /// <summary>
                /// 获取指定文件的哈希值。
                /// </summary>
                /// <param name="filePath">要获取哈希值的文件的路径。</param>
                /// <typeparam name="T">要使用的哈希算法对应的类型。</typeparam>
                /// <returns>文件的哈希值。</returns>
                public static string ComputeHash<T>(string filePath) where T : HashAlgorithm
                {
                    using FileStream fs = new FileStream(filePath, FileMode.Open);
                    byte[] retVal = (HashAlgorithm.Create(typeof(T).Name) as T)!.ComputeHash(fs);
                    return string.Join("", retVal.Select(v => v.ToString("x2")));
                }
            }
        }
    }
}