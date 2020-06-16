using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeCycleEventTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent onAwake = new UnityEvent();
    [SerializeField] UnityEvent onEnable = new UnityEvent();
    [SerializeField] UnityEvent onDisable = new UnityEvent();

    private void Awake()
    {
        onAwake.Invoke();
    }

    private void OnEnable()
    {
        onEnable.Invoke();    
    }

    private void OnDisable()
    {
        onDisable.Invoke();
    }
}
