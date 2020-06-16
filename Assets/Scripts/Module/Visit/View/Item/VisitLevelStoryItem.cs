using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitLevelStoryItem : VisitLevelItem
{
    public override void SetData(VisitLevelVo vo)
    {
        Debug.Log("VisitLevelStoryItem");
        base.SetData(vo);
    }
}
