using System;

namespace Sdurlanik.Merge2.Core
{
    public interface IEvent { }

    /// <summary>
    /// type-safe static Event Bus.
    /// EventBus<MyEvent>.Publish(new MyEvent());
    /// EventBus<MyEvent>.OnEvent += DumpEventHandler;
    /// </summary>
    /// <typeparam name="T"> Type of event that implements IEvent interface</typeparam>
    public static class EventBus<T> where T : IEvent
    {
        public static event Action<T> OnEvent;

        public static void Publish(T eventData)
        {
            OnEvent?.Invoke(eventData);
        }
    }
}