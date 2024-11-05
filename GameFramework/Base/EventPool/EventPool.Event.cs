namespace GameFramework
{
    internal sealed partial class EventPool<T> where T : BaseEventArgs
    {
        /// <summary>
        /// 事件结点。
        /// </summary>
        private sealed class Event : IMemory
        {
            private object _sender;
            private T _eventArgs;

            public object Sender => _sender;

            public T EventArgs => _eventArgs;

            public static Event Create(object sender, T e)
            {
                Event eventNode = MemoryPool.Acquire<Event>();
                eventNode._sender = sender;
                eventNode._eventArgs = e;
                return eventNode;
            }

            public void Clear()
            {
                _sender = null;
                _eventArgs = null;
            }
        }
    }
}