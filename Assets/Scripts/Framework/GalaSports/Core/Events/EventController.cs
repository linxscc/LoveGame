using UnityEngine;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Framework.GalaSports.Core.Events
{
        public class EventController
        {
            private Dictionary<string, Delegate> _eventTable = new Dictionary<string, Delegate>();

            public void AddEventListener(string eventName, Action handler)
            {
                CheckAddEvent(eventName, handler);
                _eventTable[eventName] = (Action)Delegate.Combine((Action)_eventTable[eventName], handler);
            }
            public void AddEventListener<T>(string eventName, Action<T> handler)
            {
                CheckAddEvent(eventName, handler);
                _eventTable[eventName] = (Action<T>)Delegate.Combine((Action<T>)_eventTable[eventName], handler);
            }
            public void AddEventListener<T, U>(string eventName, Action<T, U> handler)
            {
                CheckAddEvent(eventName, handler);
                _eventTable[eventName] = (Action<T, U>)Delegate.Combine((Action<T, U>)_eventTable[eventName], handler);
            }

            public void AddEventListener<T, U, V>(string eventName, Action<T, U, V> handler)
            {
                CheckAddEvent(eventName, handler);
                _eventTable[eventName] = (Action<T, U, V>)Delegate.Combine((Action<T, U, V>)_eventTable[eventName], handler);
            }

            public void AddEventListener<T, U, V, W>(string eventName, Action<T, U, V, W> handler)
            {
                CheckAddEvent(eventName, handler);
                _eventTable[eventName] = (Action<T, U, V, W>)Delegate.Combine((Action<T, U, V, W>)_eventTable[eventName], handler);
            }

            public void Cleanup()
            {
                List<string> list = new List<string>();
                foreach (KeyValuePair<string, Delegate> pair in _eventTable)
                {
                    list.Add(pair.Key);

                }
                foreach (string str in list)
                {
                    _eventTable.Remove(str);
                }
            }

            private void CheckAddEvent(string eventName, Delegate addListener)
            {
                if (!_eventTable.ContainsKey(eventName))
                {
                    _eventTable.Add(eventName, null);
                }
                Delegate delegate2 = _eventTable[eventName];
                if ((delegate2 != null) && (delegate2.GetType() != addListener.GetType()))
                {
                    throw new Exception(string.Format("Add listener {0}\failed, Current type is {1}, adding type is {2}.", eventName, delegate2.GetType().Name, addListener.GetType().Name));
                }
            }

            public void RemoveEevent(string eventName)
            {
                if (_eventTable.ContainsKey(eventName))
                {
                    _eventTable.Remove(eventName);
                }
            }

            private bool CheckRemoveEvent(string eventName, Delegate removeListener)
            {
                if (!_eventTable.ContainsKey(eventName))
                {
                    return false;
                }
                Delegate delegate2 = _eventTable[eventName];
                if ((delegate2 != null) && (delegate2.GetType() != removeListener.GetType()))
                {
                    throw new Exception(string.Format("Remove listener {0}\" failed, Current type is {1}, adding type is {2}.", eventName, delegate2.GetType(), removeListener.GetType()));
                }
                return true;
            }

            public void RemoveEventListener(string eventName, Action handler)
            {
                if (CheckRemoveEvent(eventName, handler))
                {
                    _eventTable[eventName] = (Action)Delegate.Remove((Action)_eventTable[eventName], handler);
                    if(_eventTable[eventName] == null)
                        RemoveEevent(eventName);
                }
            }

            public void RemoveEventListener<T>(string eventName, Action<T> handler)
            {
                if (CheckRemoveEvent(eventName, handler))
                {
                    _eventTable[eventName] = (Action<T>)Delegate.Remove((Action<T>)_eventTable[eventName], handler);
                    if (_eventTable[eventName] == null)
                        RemoveEevent(eventName);
                }
            }

            public void RemoveEventListener<T, U>(string eventName, Action<T, U> handler)
            {
                if (CheckRemoveEvent(eventName, handler))
                {
                    _eventTable[eventName] = (Action<T, U>)Delegate.Remove((Action<T, U>)_eventTable[eventName], handler);
                    if (_eventTable[eventName] == null)
                        RemoveEevent(eventName);
                }
            }

            public void RemoveEventListener<T, U, V>(string eventName, Action<T, U, V> handler)
            {
                if (CheckRemoveEvent(eventName, handler))
                {
                    _eventTable[eventName] = (Action<T, U, V>)Delegate.Remove((Action<T, U, V>)_eventTable[eventName], handler);
                    if (_eventTable[eventName] == null)
                        RemoveEevent(eventName);
                }
            }

            public void RemoveEventListener<T, U, V, W>(string eventName, Action<T, U, V, W> handler)
            {
                if (CheckRemoveEvent(eventName, handler))
                {
                    _eventTable[eventName] = (Action<T, U, V, W>)Delegate.Remove((Action<T, U, V, W>)_eventTable[eventName], handler);
                    if (_eventTable[eventName] == null)
                        RemoveEevent(eventName);
                }
            }

            public void TriggerEvent(string eventName)
            {
                Delegate delegate2;
                if (_eventTable.TryGetValue(eventName, out delegate2))
                {
                    Delegate[] invocationList = delegate2.GetInvocationList();
                    for (int i = 0; i < invocationList.Length; i++)
                    {
                        Action action = invocationList[i] as Action;
                        if (action == null)
                        {
                            throw new Exception(string.Format("TriggerEvent {0} error: types of parameters are not match.", eventName));
                        }
                        try
                        {
                            action();
                        }
                        catch (Exception exception)
                        {
                            Debug.Log(exception, null);
                        }
                    }
                }
            }

            public void TriggerEvent<T>(string eventName, T arg1)
            {
                Delegate delegate2;
                if (_eventTable.TryGetValue(eventName, out delegate2))
                {
                    Delegate[] invocationList = delegate2.GetInvocationList();
                    for (int i = 0; i < invocationList.Length; i++)
                    {
                        Delegate action = invocationList[i];
                        if (action == null)
                        {
                            throw new Exception(string.Format("TriggerEvent {0} error: types of parameters are not match.", eventName));
                        }
                        try
                        {
                            action.DynamicInvoke(arg1);
                        }
                        catch (Exception exception)
                        {
                            Debug.LogError(exception, null);
                        }
                    }
                }
            }

            public void TriggerEvent<T, U>(string eventName, T arg1, U arg2)
            {
                Delegate delegate2;
                if (_eventTable.TryGetValue(eventName, out delegate2))
                {
                    Delegate[] invocationList = delegate2.GetInvocationList();
                    for (int i = 0; i < invocationList.Length; i++)
                    {
                        Delegate action = invocationList[i];
                        if (action == null)
                        {
                            throw new Exception(string.Format("TriggerEvent {0} error: types of parameters are not match.", eventName));
                        }
                        try
                        {
                            action.DynamicInvoke(arg1, arg2);
                        }
                        catch (Exception exception)
                        {
                            Debug.Log(exception, null);
                        }
                    }
                }
            }

            public void TriggerEvent<T, U, V>(string eventName, T arg1, U arg2, V arg3)
            {
                Delegate delegate2;
                if (_eventTable.TryGetValue(eventName, out delegate2))
                {
                    Delegate[] invocationList = delegate2.GetInvocationList();
                    for (int i = 0; i < invocationList.Length; i++)
                    {
                        Delegate action = invocationList[i];
                        if (action == null)
                        {
                            throw new Exception(string.Format("TriggerEvent {0} error: types of parameters are not match.", eventName));
                        }
                        try
                        {
                            action.DynamicInvoke(arg1, arg2, arg3);
                        }
                        catch (Exception exception)
                        {
                            Debug.Log(exception, null);
                        }
                    }
                }
            }

            public void TriggerEvent<T, U, V, W>(string eventName, T arg1, U arg2, V arg3, W arg4)
            {
                Delegate delegate2;
                if (_eventTable.TryGetValue(eventName, out delegate2))
                {
                    Delegate[] invocationList = delegate2.GetInvocationList();
                    for (int i = 0; i < invocationList.Length; i++)
                    {
                        Delegate action = invocationList[i];
                        if (action == null)
                        {
                            throw new Exception(string.Format("TriggerEvent {0} error: types of parameters are not match.", eventName));
                        }
                        try
                        {
                            action.DynamicInvoke(arg1, arg2, arg3, arg4);
                        }
                        catch (Exception exception)
                        {
                            Debug.Log(exception, null);
                        }
                    }
                }
            }

            public Dictionary<string, Delegate> EventTable
            {
                get
                {
                    return _eventTable;
                }
            }
        }
    }
