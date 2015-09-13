﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoFlash.Events
{
    public class EventDispatcher
    {
        public delegate void EventHandler(Event e);

        private Dictionary<String, List<EventHandler>> handlers;

        public EventDispatcher()
        {
            handlers = new Dictionary<string, List<EventHandler>>();
        }

        public void AddEventListener(String eventType, EventHandler handlerFunction)
        {
            if (!handlers.ContainsKey(eventType))
            {
                handlers[eventType] = new List<EventHandler>();
            }
            handlers[eventType].Add(handlerFunction);
        }

        public void RemoveEventListener(String eventType, EventHandler handlerFunction)
        {
            if (!handlers.ContainsKey(eventType))
            {
                return;
            }
            handlers[eventType].Remove(handlerFunction);
        }

        public void DispatchEvent(Event e)
        {
            if (!handlers.ContainsKey(e.type))
            {
                return;
            }

            if (e.target == null)
            {
                e.target = this;
            }
            e.currentTarget = this;

            for (int i = 0; i < handlers[e.type].Count; i++)
            {
                EventHandler handler = handlers[e.type].ElementAt(i);
                handler(e);
            }
        }
    }
}
