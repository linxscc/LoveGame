using Assets.Scripts.Framework.GalaSports.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatableModuleBase : ModuleBase
{
    UpdatableTimer _handler;
    List<IUpdatable> updatables;
    public override void Init()
    {
        Debug.Log("Init UpdatableModuleBase");

        updatables = new List<IUpdatable>();
        base.Init();
        GameObject obj = new GameObject();
        obj.name = "UpdatableTimer";
        //GameObject.Instantiate(obj);
        _handler = obj.AddComponent<UpdatableTimer>();
        _handler.Init(Update);
    }

    public override void Remove(float delay)
    {
        _handler.IsPlaying = false;
        updatables.Clear();
        GameObject.Destroy(_handler.gameObject);
        _handler = null;
        base.Remove(delay);
    }

    public void RegisterUpdatablePanel(IUpdatable updatable)
    {
        if (updatables.Contains(updatable))
            return;
        updatables.Add(updatable);
    }

    public void UnRegisterUpdatablePanel(IUpdatable updatable)
    {
        if (!updatables.Contains(updatable))
            return;
        updatables.Remove(updatable);
    }
    protected void OnStart()
    {
        for (int i = 0; i < updatables.Count; i++)
        {
            updatables[i].Start();
        }
        _handler.IsPlaying = true;

    }


    protected void OnShutdown()
    {
        for (int i = 0; i < updatables.Count; i++)
        {
            updatables[i].Shutdown();
        }
        _handler.IsPlaying = false;

    }
    protected void OnPlay()
    {
        for (int i = 0; i < updatables.Count; i++)
        {
            updatables[i].Play();
        }
        _handler.IsPlaying = true;

    }
    protected void OnPause()
    {
        for (int i = 0; i < updatables.Count; i++)
        {
            updatables[i].Pause();
        }
        _handler.IsPlaying = false;

    }
    protected void Update(float delta)
    {
        for(int i=0;i< updatables.Count;i++)
        {
            updatables[i].Update(delta);
        }
    }
}
 