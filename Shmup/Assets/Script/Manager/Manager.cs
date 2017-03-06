using System;
using System.Collections.Generic;
using UnityEngine;

public class Manger{
    static private Manger instance;
    static public Manger Instance{
        get{
            if(instance == null)
            {
                instance = new Manger();
            }
            return instance;
        }
    }

    private Dictionary<Type,Event.Handler> handler = new Dictionary<Type,Event.Handler>();

    public void Unregister<T>(Event.Handler _handler) where T: Event
    {
        Type type = typeof(T);
        Event.Handler handlers;
        if(handler.TryGetValue(type, out handlers))
        {
            handlers -= _handler;
            if(handlers == null)
            {
                handler.Remove(type);
            } else
            {
                handler[type] = handlers;
            }
        }
    }

    public void Register<T>(Event.Handler _handler) where T: Event
    {
        Type type = typeof(T);
        if(!handler.ContainsKey(type))
        {
            handler[type] = _handler;
        }
        else
        {
            handler[type] += _handler;
        }
    }

    public void Fire()
    {}
}