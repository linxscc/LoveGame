using System;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using Common;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using game.main;
using DataModel;
using System.Collections.Generic;



public class ReloadingItem : MonoBehaviour, IPointerClickHandler
{
   
      private ReloadingVO _data;

 
   

   


    public void SetData(ReloadingVO vo)
    { 
        
        transform.Find("RedDot").gameObject.SetActive(vo.IsShowRedDot); 
        transform.Find("PictononFrame").gameObject.SetActive(vo.IsPitchOn);
        transform.Find("Frame").gameObject.SetActive(!vo.IsPitchOn);
        transform.GetImage("Icon").sprite = ResourceManager.Load<Sprite>(vo.IconPath);
        
        transform.GetText("InfoText").text = vo.Name;
        transform.Find("Lock").gameObject.SetActive(!vo.IsGet);
        _data = vo;

    }

    public void OnPointerClick(PointerEventData eventData)
    {     
       EventDispatcher.TriggerEvent<ReloadingVO>(EventConst.ReloadingItemClick, _data);
    }

}
