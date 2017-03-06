using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager {

    // Just some singleton boiler plate
    static private EventManager instance;
    static public EventManager Instance { get {
        if (instance == null) {
            instance = new EventManager();
        }
        return instance;
    } }

    // Here is the core of the system: a dictionary that maps types (specifically Event types in this case)
    // to Event.Handlers
    private Dictionary<Type, Event.Handler> registeredHandlers = new Dictionary<Type, Event.Handler>();

    // This is where you can add handlers for events. We use generics for 2 reasons:
    // 1. Passing around Type objects can be tedious and verbose
    // 2. Using generics allows us to add a little type safety, by getting
    // the compiler to ensure we're registering for an Event type and not some other random type
    public void Register<T>(Event.Handler handler) where T : Event {
        // NOTE: This method does not check to see if a handler was registered twice
        // I would add an assert here, but you can add in a more permanent check too if interested
        Type type = typeof(T);
        if (registeredHandlers.ContainsKey(type)) {
            registeredHandlers[type] += handler;
        } else {
            registeredHandlers[type] = handler;
        }
    }

    // This is where you stop listening to an event. Make sure to balance
    // any calls to Register with corresponding calls to Unregister
    public void Unregister<T>(Event.Handler handler) where T : Event {
        Type type = typeof(T);
        Event.Handler handlers;
        if (registeredHandlers.TryGetValue(type, out handlers)) {
            handlers -= handler;
            if (handlers == null) {
                registeredHandlers.Remove(type);
            } else {
                registeredHandlers[type] = handlers;
            }
        }
    }

    // This is how you "publish" and event. All it entails is looking up
    // the event type and calling the delegate containing all the handlers
    public void Fire(Event e) {
        Type type = e.GetType();
        Event.Handler handlers;
        if (registeredHandlers.TryGetValue(type, out handlers)) {
            handlers(e);
        }
    }
}