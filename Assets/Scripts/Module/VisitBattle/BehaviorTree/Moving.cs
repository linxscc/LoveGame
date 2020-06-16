using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : Action
{
    VisitFansSpineItem item;
    public override void OnAwake()
    {
       // Debug.Log("Moving Awake");
        base.OnAwake();
        item = gameObject.transform.GetComponent<VisitFansSpineItem>();
 
    }
    public override void OnStart()
    {
       // Debug.Log("Moving OnStart");
        item.StartMoving();
    }
    public override TaskStatus OnUpdate()
    {
      //  Debug.Log("Moving Update");
        bool finishMove= item.DoMoving();
        return finishMove? TaskStatus.Success: TaskStatus.Running; ;
    }

    public override void OnEnd()
    {
      //  Debug.Log("Moving End");
        item.CurState = FunsState.Idel;
    }
}
