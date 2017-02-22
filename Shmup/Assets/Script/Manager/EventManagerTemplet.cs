using System;
using System.Collections.Generic;

// Taken almost verbatim from Tim Miller's post on Type Safe Events in Unity 3D: http://www.willrmiller.com/?p=87

namespace GM {
    public class GameEvent{}

    public class EventManager
    {
        public delegate void EventDelegate<T> (T e) where T : GameEvent;
        private delegate void EventDelegate (GameEvent e);
        private readonly Dictionary<Type, EventDelegate> _delegates = new Dictionary<Type, EventDelegate>();
        private readonly Dictionary<Delegate, EventDelegate> _delegateLookup = new Dictionary<Delegate, EventDelegate>();
        private readonly List<GameEvent> _queuedEvents = new List<GameEvent>();
        private readonly object _queueLock = new object();

        private static readonly EventManager _instance = new EventManager();
        public static EventManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private EventManager() {}

        public void AddHandler<T> (EventDelegate<T> del) where T : GameEvent
        {
            if (_delegateLookup.ContainsKey(del))
            {
                return;
            }

            EventDelegate internalDelegate = (e) => del((T)e);
            _delegateLookup[del] = internalDelegate;

            EventDelegate tempDel;
            if (_delegates.TryGetValue(typeof(T), out tempDel)) {
                _delegates[typeof(T)] = tempDel += internalDelegate;
            } else {
                _delegates[typeof(T)] = internalDelegate;
            }
        }

        public void RemoveHandler<T>(EventDelegate<T> del) where T : GameEvent
        {
            EventDelegate internalDelegate;
            if (_delegateLookup.TryGetValue(del, out internalDelegate)) {
                EventDelegate tempDel;
                if (_delegates.TryGetValue(typeof(T), out tempDel)) {
                    tempDel -= internalDelegate;
                    if (tempDel == null) {
                        _delegates.Remove(typeof(T));
                    } else {
                        _delegates[typeof(T)] = tempDel;
                    }
                }
                _delegateLookup.Remove(del);
            }
        }

        public void Clear()
        {
            lock (_queueLock)
            {
                if (_delegates != null) _delegates.Clear();
                if (_delegateLookup != null) _delegateLookup.Clear();
                if (_queuedEvents != null) _queuedEvents.Clear();
            }
        }

        public void Fire(GameEvent e)
        {
            EventDelegate del;
            if (_delegates.TryGetValue(e.GetType(), out del))
            {
                del.Invoke(e);
            }
        }

        public void ProcessQueuedEvents()
        {
            List<GameEvent> events;
            lock (_queueLock)
            {
                if (_queuedEvents.Count > 0)
                {
                    events = new List<GameEvent>(_queuedEvents);
                    _queuedEvents.Clear();
                }
                else
                {
                    return;
                }
            }

            foreach(var e in events)
            {
                Fire(e);
            }
        }

        public void Queue(GameEvent e)
        {
            lock (_queueLock)
            {
                _queuedEvents.Add(e);
            }
        }

    }

}