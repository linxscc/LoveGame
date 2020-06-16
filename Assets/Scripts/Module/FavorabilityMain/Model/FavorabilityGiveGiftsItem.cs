using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using System.Collections;
using Assets.Scripts.Framework.GalaSports.Core;
using System;


public class FavorabilityGiveGiftsItem : MonoBehaviour, IPointerClickHandler
{
    private Image _redPictonOn;  
    private RawImage _itemImage; 
    public Text _itemNum;      
    private Text _nameText;      
    private FavorabilityGiveGiftsItemVO _data;   
    private float _itemWidth;
    private float _itemHeight;
    private RectTransform _rectTransform;   
   
    
    private void ComponentInit()
    {
        _redPictonOn = transform.Find("PictonOn").GetComponent<Image>();      
        _itemImage = transform.Find("ItemImage").GetComponent<RawImage>();
        _itemNum = transform.Find("ItemImage/NumText").GetComponent<Text>();
        _nameText=transform.Find("NameText").GetComponent<Text>();     
        _rectTransform = GetComponent<RectTransform>();
        _itemWidth = _rectTransform.GetWidth();
        _itemHeight = _rectTransform.GetHeight();      
        
    }
   
    public void SetData(FavorabilityGiveGiftsItemVO vo)
    {
        ComponentInit();    
        _data = vo;
        _redPictonOn.gameObject.SetActive(vo.IsShowRedFrame);                
        _itemImage.texture = ResourceManager.Load<Texture>("Prop/" + vo.ItemId);        
       _itemNum.text =  vo.ItemNum.ToString();                                 
        _nameText.text= vo.Name;                                                             
    }


    public void OnPointerClick(PointerEventData eventData)
    {       
       EventDispatcher.TriggerEvent(EventConst.FavorabilityGiveGiftsItemClick, _data);
    }


   
}
