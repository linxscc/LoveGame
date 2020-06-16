using Assets.Scripts.Framework.GalaSports.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneGameTimer : MonoBehaviour {

    List<AirborneGameUpdateController> _controllers;
    private void Awake()
    {
        _controllers = new List<AirborneGameUpdateController>();
    }

    public void InitTimer()
    {
        _controllers = new List<AirborneGameUpdateController>();
    }
    public void AddController(AirborneGameUpdateController client)
    {
        _controllers.Add(client);
    }
    public void RemoveController(AirborneGameUpdateController client)
    {
        _controllers.Remove(client);
    }

    bool isPlaying = false;

    public void Play()
    {
        isPlaying = true;
        for (int i = 0; i < _controllers.Count; i++)
        {
            _controllers[i].Play();
        }
    }

    public void Pause()
    {
        isPlaying = false;
        for (int i = 0; i < _controllers.Count; i++)
        {
            _controllers[i].Pause();
        }
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        float delta= Time.deltaTime;
        for (int i=0;i< _controllers.Count;i++)
        {
            _controllers[i].Update(delta);
        }
    }

}
