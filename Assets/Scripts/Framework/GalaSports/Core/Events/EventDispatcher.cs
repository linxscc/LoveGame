using System;
using System.Collections.Generic;

    namespace Assets.Scripts.Framework.GalaSports.Core.Events
    {
        public class EventDispatcher
        {
            private static EventController _eventController = new EventController();

            public static void AddEventListener<T>(string eventName, Action<T> handler)
            {
                _eventController.AddEventListener<T>(eventName, handler);
            }

            public static void AddEventListener(string eventName, Action handler)
            {
                _eventController.AddEventListener(eventName, handler);
            }

            public static void AddEventListener<T, U>(string eventName, Action<T, U> handler)
            {
                _eventController.AddEventListener<T, U>(eventName, handler);
            }

            public static void AddEventListener<T, U, V>(string eventName, Action<T, U, V> handler)
            {
                _eventController.AddEventListener<T, U, V>(eventName, handler);
            }

            public static void AddEventListener<T, U, V, W>(string eventName, Action<T, U, V, W> handler)
            {
                _eventController.AddEventListener<T, U, V, W>(eventName, handler);
            }

            public static void Cleanup()
            {
                _eventController.Cleanup();
            }

            public static void RemoveEvent(string eventName)
            {
                _eventController.RemoveEevent(eventName);
            }

            public static void RemoveEventListener(string eventName, Action handler)
            {
                _eventController.RemoveEventListener(eventName, handler);
            }

            public static void RemoveEventListener<T>(string eventName, Action<T> handler)
            {
                _eventController.RemoveEventListener<T>(eventName, handler);
            }

            public static void RemoveEventListener<T, U>(string eventName, Action<T, U> handler)
            {
                _eventController.RemoveEventListener<T, U>(eventName, handler);
            }

            public static void RemoveEventListener<T, U, V>(string eventName, Action<T, U, V> handler)
            {
                _eventController.RemoveEventListener<T, U, V>(eventName, handler);
            }

            public static void RemoveEventListener<T, U, V, W>(string eventName, Action<T, U, V, W> handler)
            {
                _eventController.RemoveEventListener<T, U, V, W>(eventName, handler);
            }

            public static void TriggerEvent(string eventName)
            {
                _eventController.TriggerEvent(eventName);
            }

            public static void TriggerEvent<T>(string eventName, T arg1)
            {
                _eventController.TriggerEvent<T>(eventName, arg1);
            }
            
            public static void TriggerEventTest<T>(string eventName, T arg1)
            {
                _eventController.TriggerEvent<T>(eventName, arg1);
            }

            public static void TriggerEvent<T, U>(string eventName, T arg1, U arg2)
            {
                _eventController.TriggerEvent<T, U>(eventName, arg1, arg2);
            }

            public static void TriggerEvent<T, U, V>(string eventName, T arg1, U arg2, V arg3)
            {
                _eventController.TriggerEvent<T, U, V>(eventName, arg1, arg2, arg3);
            }

            public static void TriggerEvent<T, U, V, W>(string eventName, T arg1, U arg2, V arg3, W arg4)
            {
                _eventController.TriggerEvent<T, U, V, W>(eventName, arg1, arg2, arg3, arg4);
            }

            public static Dictionary<string, Delegate> EventTable
            {
                get
                {
                    return _eventController.EventTable;
                }
            }
        }
    }
