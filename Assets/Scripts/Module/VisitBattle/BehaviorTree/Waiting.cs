using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
public class Waiting : Action
{
    VisitFansSpineItem item;
    public override void OnAwake()
    {
       // Debug.Log("Waiting Awake");
        base.OnAwake();
        item = gameObject.transform.GetComponent<VisitFansSpineItem>();
    }
    public override void OnStart()
    {
     //   Debug.Log("Waiting OnStart");
        item.StartWaiting();
    }
    public override TaskStatus OnUpdate()
    {
        item.DoWaiting();
        return TaskStatus.Success;
    }
    public override void OnEnd()
    {
     //   Debug.Log("Waiting End");
        item.CurState = FunsState.None;
    }
}
