/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using UnityEngine;
using XLua;
using System;

[Serializable]
public class Injection
{
    public string name;
    public GameObject value;
}

[LuaCallCSharp]
public class LuaBehaviour : MonoBehaviour {
    public TextAsset luaScript;
    public Injection[] injections;

    internal static LuaEnv LuaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
    internal static float LastGcTime = 0;
    internal const float GcInterval = 5;//1 second 

    private Action _luaStart;
    private Action _luaUpdate;
    private Action _luaOnDestroy;

    private LuaTable _scriptEnv;

    void Awake()
    {
        _scriptEnv = LuaEnv.NewTable();

        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = LuaEnv.NewTable();
        meta.Set("__index", LuaEnv.Global);
        _scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        _scriptEnv.Set("self", this);
        foreach (var injection in injections)
        {
            _scriptEnv.Set(injection.name, injection.value);
        }

        LuaEnv.DoString(luaScript.text, "LuaBehaviour", _scriptEnv);

        Action luaAwake = _scriptEnv.Get<Action>("Awake");
        _scriptEnv.Get("Start", out _luaStart);
        _scriptEnv.Get("Update", out _luaUpdate);
        _scriptEnv.Get("OnDestroy", out _luaOnDestroy);

        luaAwake?.Invoke();
    }

	void Start ()
    {
        _luaStart?.Invoke();
    }
	
	void Update ()
    {
        _luaUpdate?.Invoke();
        if (Time.time - LastGcTime > GcInterval)
        {
            LuaEnv.Tick();
            LastGcTime = Time.time;
        }
	}

    void OnDestroy()
    {
        _luaOnDestroy?.Invoke();
        _luaOnDestroy = null;
        _luaUpdate = null;
        _luaStart = null;
        _scriptEnv.Dispose();
        injections = null;
    }
}
