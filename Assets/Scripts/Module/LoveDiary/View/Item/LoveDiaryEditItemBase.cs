using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoveDiaryEditItemBase : MonoBehaviour, IPointerClickHandler
{
    protected ElementPB _pb;
    private LoopHorizontalScrollRect rect;
    private void Awake()
    {      
    }
    public virtual void SetData(ElementPB pb, bool isLock = false)
    {
        _pb = pb;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       Debug.Log("OnPointerClick");
       EventDispatcher.TriggerEvent<int>(EventConst.LoveDiaryEditItemClick, _pb.Id);
    }
}
