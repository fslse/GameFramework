using System;
using System.Collections.Generic;

namespace GameFramework
{
    /// <summary>
    /// 事件池。
    /// </summary>
    /// <typeparam name="T">事件类型。</typeparam>
    internal sealed partial class EventPool<T> where T : BaseEventArgs
    {
        private readonly GameFrameworkMultiDictionary<int, EventHandler<T>> _eventHandlers;
        private readonly Queue<Event> _events;
        private readonly Dictionary<object, LinkedListNode<EventHandler<T>>> _cachedNodes;
        private readonly Dictionary<object, LinkedListNode<EventHandler<T>>> _tempNodes;
        private readonly EEventPoolMode _eventPoolMode;
        private EventHandler<T> _defaultHandler;

        /// <summary>
        /// 初始化事件池的新实例。
        /// </summary>
        /// <param name="mode">事件池模式。</param>
        public EventPool(EEventPoolMode mode)
        {
            _eventHandlers = new GameFrameworkMultiDictionary<int, EventHandler<T>>();
            _events = new Queue<Event>();
            _cachedNodes = new Dictionary<object, LinkedListNode<EventHandler<T>>>();
            _tempNodes = new Dictionary<object, LinkedListNode<EventHandler<T>>>();
            _eventPoolMode = mode;
            _defaultHandler = null;
        }

        /// <summary>
        /// 获取事件数量。
        /// </summary>
        public int EventCount => _events.Count;

        /// <summary>
        /// 获取事件处理函数的数量。
        /// </summary>
        public int EventHandlerCount => _eventHandlers.Count;

        /// <summary>
        /// 事件池轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            lock (_events)
            {
                while (_events.Count > 0)
                {
                    Event eventNode = _events.Dequeue();
                    HandleEvent(eventNode.Sender, eventNode.EventArgs);
                    MemoryPool.Release(eventNode);
                }
            }
        }

        /// <summary>
        /// 关闭并清理事件池。
        /// </summary>
        public void Shutdown()
        {
            Clear();
            _eventHandlers.Clear();
            _cachedNodes.Clear();
            _tempNodes.Clear();
            _defaultHandler = null;
        }

        /// <summary>
        /// 清理事件。
        /// </summary>
        public void Clear()
        {
            lock (_events)
            {
                _events.Clear();
            }
        }

        /// <summary>
        /// 获取事件处理函数的数量。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <returns>事件处理函数的数量。</returns>
        public int Count(int id)
        {
            return _eventHandlers.TryGetValue(id, out var range) ? range.Count : 0;
        }

        /// <summary>
        /// 检查是否存在事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要检查的事件处理函数。</param>
        /// <returns>是否存在事件处理函数。</returns>
        public bool Check(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new GameFrameworkException("Event handler is invalid.");
            }

            return _eventHandlers.Contains(id, handler);
        }

        /// <summary>
        /// 订阅事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要订阅的事件处理函数。</param>
        public void Subscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new GameFrameworkException("Event handler is invalid.");
            }

            if (!_eventHandlers.Contains(id))
            {
                _eventHandlers.Add(id, handler);
            }
            else if ((_eventPoolMode & EEventPoolMode.AllowMultiHandler) != EEventPoolMode.AllowMultiHandler)
            {
                throw new GameFrameworkException($"Event '{id}' not allow multi handler.");
            }
            else if ((_eventPoolMode & EEventPoolMode.AllowDuplicateHandler) != EEventPoolMode.AllowDuplicateHandler && Check(id, handler))
            {
                throw new GameFrameworkException($"Event '{id}' not allow duplicate handler.");
            }
            else
            {
                _eventHandlers.Add(id, handler);
            }
        }

        /// <summary>
        /// 取消订阅事件处理函数。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <param name="handler">要取消订阅的事件处理函数。</param>
        public void Unsubscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new GameFrameworkException("Event handler is invalid.");
            }

            if (_cachedNodes.Count > 0)
            {
                foreach (KeyValuePair<object, LinkedListNode<EventHandler<T>>> cachedNode in _cachedNodes)
                {
                    if (cachedNode.Value != null && cachedNode.Value.Value == handler)
                    {
                        _tempNodes.Add(cachedNode.Key, cachedNode.Value.Next);
                    }
                }

                if (_tempNodes.Count > 0)
                {
                    foreach (KeyValuePair<object, LinkedListNode<EventHandler<T>>> cachedNode in _tempNodes)
                    {
                        _cachedNodes[cachedNode.Key] = cachedNode.Value;
                    }

                    _tempNodes.Clear();
                }
            }

            if (!_eventHandlers.Remove(id, handler))
            {
                throw new GameFrameworkException($"Event '{id}' not exists specified handler.");
            }
        }

        /// <summary>
        /// 设置默认事件处理函数。
        /// </summary>
        /// <param name="handler">要设置的默认事件处理函数。</param>
        public void SetDefaultHandler(EventHandler<T> handler)
        {
            _defaultHandler = handler;
        }

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        public void Fire(object sender, T e)
        {
            if (e == null)
            {
                throw new GameFrameworkException("Event is invalid.");
            }

            Event eventNode = Event.Create(sender, e);
            lock (_events)
            {
                _events.Enqueue(eventNode);
            }
        }

        /// <summary>
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        public void FireNow(object sender, T e)
        {
            if (e == null)
            {
                throw new GameFrameworkException("Event is invalid.");
            }

            HandleEvent(sender, e);
        }

        /// <summary>
        /// 处理事件结点。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        private void HandleEvent(object sender, T e)
        {
            bool noHandlerException = false;
            if (_eventHandlers.TryGetValue(e.Id, out var range))
            {
                LinkedListNode<EventHandler<T>> current = range.First;
                while (current != null && current != range.Terminal)
                {
                    _cachedNodes[e] = current.Next != range.Terminal ? current.Next : null;
                    current.Value(sender, e);
                    current = _cachedNodes[e];
                }

                _cachedNodes.Remove(e);
            }
            else if (_defaultHandler != null)
            {
                _defaultHandler(sender, e);
            }
            else if ((_eventPoolMode & EEventPoolMode.AllowNoHandler) == 0)
            {
                noHandlerException = true;
            }

            MemoryPool.Release(e);

            if (noHandlerException)
            {
                throw new GameFrameworkException($"Event '{e.Id}' not allow no handler.");
            }
        }
    }
}