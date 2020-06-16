using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class Waving : Action
{
    VisitFansSpineItem item;
    public override void OnAwake()
    {
      //  Debug.Log("Waving Awake");
        base.OnAwake();
        item = gameObject.transform.GetComponent<VisitFansSpineItem>();

    }

    public override void OnStart()
    {
       // Debug.Log("Waving OnStart");
        item.StartWaveing();
    }

    public override void OnEnd()
    {
  //      Debug.Log("Waving End");
        item.CurState = FunsState.None;
    }

}
