namespace GameFramework
{
    internal sealed partial class EventPool<T> where T : BaseEventArgs
    {
        /// <summary>
        /// 事件结点。
        /// </summary>
        private sealed class Event : IMemory
        {
            public object Sender { get; private set; }

            public T EventArgs { get; private set; }

            public static Event Create(object sender, T e)
            {
                Event eventNode = MemoryPool.Acquire<Event>();
                eventNode.Sender = sender;
                eventNode.EventArgs = e;
                return eventNode;
            }

            public void Clear()
            {
                Sender = null;
                EventArgs = null;
            }
        }
    }
}