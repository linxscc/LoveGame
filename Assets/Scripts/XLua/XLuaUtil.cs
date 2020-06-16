using System;
using System.Reflection;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using UnityEngine;
using XLua;

[CSharpCallLua]
public delegate void DelegateBytes(byte[] data);
[CSharpCallLua]
public delegate void DelegateLuaTable(LuaTable data);

public class XLuaUtil
{
    public static MethodInfo FindGenericMethod(string className, string methodName, string originalName,
        params Type[] types)
    {
        Type type = Type.GetType(className);
        if (type == null)
            return null;

        MethodInfo[] methods = type.GetMethods();
        foreach (var methodInfo in methods)
        {
            if (methodInfo.Name == methodName && methodInfo.ToString().Contains(originalName))
            {
                MethodInfo mi = methodInfo.MakeGenericMethod(types);
                return mi;
            }
        }

        return null;
    }
    
    public static void TriggerEvent1(string eventName, object arg1)
    {
        EventDispatcher.TriggerEvent(eventName, arg1);
    }
        
    public static void TriggerEvent2(string eventName, object arg1, object arg2)
    {
        EventDispatcher.TriggerEvent(eventName, arg1, arg2);
    }
        
    public static void TriggerEvent3(string eventName, object arg1, object arg2, object arg3)
    {
        EventDispatcher.TriggerEvent(eventName, arg1, arg2, arg3);
    }
        
    public static void TriggerEvent4(string eventName, object arg1, object arg2, object arg3, object arg4)
    {
        EventDispatcher.TriggerEvent(eventName, arg1, arg2, arg3, arg4);
    }
    
    public LuaHttp SendLuaHttp(string cmd, 
        byte[] data, 
        DelegateBytes successCallback, 
        object customerData,
        bool cache = false, 
        string version = "", 
        string serverUrl = null, 
        int httpTimeout = -1)
    {
        LuaHttp msg = null;
        if (NetWorkManager.Serverurl != null || serverUrl != null)
        {
            msg = new LuaHttp(cmd, data, successCallback, customerData, cache, version, serverUrl, httpTimeout);
            msg.SendRequest();
        }
        else
        {
            Debug.Log("NetWorkManager.SERVER 为空！");
        }
        return msg;
    }
}

