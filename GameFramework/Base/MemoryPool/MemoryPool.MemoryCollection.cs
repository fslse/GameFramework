using System;
using System.Collections.Generic;

namespace GameFramework
{
    public static partial class MemoryPool
    {
        /// <summary>
        /// 内存池对象容器。
        /// </summary>
        private sealed class MemoryCollection
        {
            private readonly Type _memoryType;
            private readonly Queue<IMemory> _memories;

            public int UnusedMemoryCount => _memories.Count;
            public int UsingMemoryCount { get; private set; }
            public int AcquireMemoryCount { get; private set; }
            public int ReleaseMemoryCount { get; private set; }
            public int AddMemoryCount { get; private set; }
            public int RemoveMemoryCount { get; private set; }

            public MemoryCollection(Type memoryType)
            {
                _memories = new Queue<IMemory>();
                _memoryType = memoryType;
                UsingMemoryCount = 0;
                AcquireMemoryCount = 0;
                ReleaseMemoryCount = 0;
                AddMemoryCount = 0;
                RemoveMemoryCount = 0;
            }

            public T Acquire<T>() where T : class, IMemory, new()
            {
                if (typeof(T) != _memoryType)
                {
                    throw new GameFrameworkException("Type is invalid.");
                }

                UsingMemoryCount++;
                AcquireMemoryCount++;
                lock (_memories)
                {
                    if (_memories.Count > 0)
                    {
                        return (T)_memories.Dequeue();
                    }
                }

                AddMemoryCount++;
                return new T();
            }

            public IMemory Acquire()
            {
                UsingMemoryCount++;
                AcquireMemoryCount++;
                lock (_memories)
                {
                    if (_memories.Count > 0)
                    {
                        return _memories.Dequeue();
                    }
                }

                AddMemoryCount++;
                return (IMemory)Activator.CreateInstance(_memoryType);
            }

            public void Release(IMemory memory)
            {
                memory.Clear();
                lock (_memories)
                {
                    if (EnableStrictCheck && _memories.Contains(memory))
                    {
                        throw new GameFrameworkException("The memory has been released.");
                    }

                    _memories.Enqueue(memory);
                }

                ReleaseMemoryCount++;
                UsingMemoryCount--;
            }

            public void Add<T>(int count) where T : class, IMemory, new()
            {
                if (typeof(T) != _memoryType)
                {
                    throw new GameFrameworkException("Type is invalid.");
                }

                lock (_memories)
                {
                    AddMemoryCount += count;
                    while (count-- > 0)
                    {
                        _memories.Enqueue(new T());
                    }
                }
            }

            public void Add(int count)
            {
                lock (_memories)
                {
                    AddMemoryCount += count;
                    while (count-- > 0)
                    {
                        _memories.Enqueue((IMemory)Activator.CreateInstance(_memoryType));
                    }
                }
            }

            public void Remove(int count)
            {
                lock (_memories)
                {
                    if (count > _memories.Count)
                    {
                        count = _memories.Count;
                    }

                    RemoveMemoryCount += count;
                    while (count-- > 0)
                    {
                        _memories.Dequeue();
                    }
                }
            }

            public void RemoveAll()
            {
                lock (_memories)
                {
                    RemoveMemoryCount += _memories.Count;
                    _memories.Clear();
                }
            }
        }
    }
}