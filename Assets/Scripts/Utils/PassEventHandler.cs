using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 把UGUI事件穿透到下面
/// </summary>
public class PassEventHandler : MonoBehaviour,IPointerClickHandler ,IPointerDownHandler,IPointerUpHandler
{
    /// <summary>
    /// 穿透下面所有UI
    /// </summary>
    public bool PassAll = false;
    
    //监听按下
    public void OnPointerDown(PointerEventData eventData)
    {
        PassEvent(eventData,ExecuteEvents.pointerDownHandler);
    }
 
    //监听抬起
    public void OnPointerUp(PointerEventData eventData)
    {
        PassEvent(eventData,ExecuteEvents.pointerUpHandler);
    }
 
    //监听点击
    public void OnPointerClick(PointerEventData eventData)
    {
        PassEvent(eventData,ExecuteEvents.submitHandler);
        PassEvent(eventData,ExecuteEvents.pointerClickHandler);
    }
 
 
    //把事件透下去
    public void  PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
        where T : IEventSystemHandler
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results); 
        GameObject current = data.pointerCurrentRaycast.gameObject ;
        for(int i =0; i< results.Count;i++)
        {
            if(current!= results[i].gameObject)
            {
                ExecuteEvents.Execute(results[i].gameObject, data,function);
                if(PassAll == false)
                    break;
                //RaycastAll后ugui会自己排序，如果你只想响应透下去的最近的一个响应，这里ExecuteEvents.Execute后直接break就行。
            }
        }
    }
}