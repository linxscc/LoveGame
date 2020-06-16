using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Common;
using game.main;
using game.tools;
using System.Collections;
using System.Collections.Generic;
using DataModel;
using UnityEngine;
using UnityEngine.UI;

public class MonthSginAwardItem : MonoBehaviour
{
    
    private GameObject _vip;
  





    private Text _day;
    private RawImage _icon;
    private Transform _mask;
    private Transform _retroactive;
    private Transform _alreadyGet;
    private Transform _noGet;
    private Transform _curMayGet;
    private Text _num;
    
    
    


    public void SetData(MonthSignAwardVO vO)
    {
        
        _vip = transform.Find("VIP").gameObject;
       

        _day = transform.GetText("DayBg/Text");
        _icon = transform.GetRawImage("AwardIcon");
        _mask = transform.Find("Mask");       
        _retroactive = _mask.Find("Retroactive");
        _noGet = _mask.Find("NotGet");
        _alreadyGet = _mask.Find("AlreadyGet");
        _curMayGet = _mask.Find("CurMayGet");
        _num = transform.GetText("NumBg/Text");
        
        foreach (var t in vO.Awards)
        {
            _icon.texture = ResourceManager.Load<Texture>(t.IconPath,ModuleConfig.MODULE_ACTIVITY,true);             
            _num.text = t.Num.ToString();
        }
                
        _day.text =  vO.Id.ToString();
       
        OnClick(vO);
        _vip.SetActive(vO.IsShowVipImage);
    }

    private void OnClick(MonthSignAwardVO vO)
    {
        
        switch (vO.State)
        {
            case EveryDaySignState.Retroactive:
                SetState(_retroactive.gameObject.name);                
                PointerClickListener.Get(_retroactive.gameObject).onClick = go => { EventDispatcher.TriggerEvent(EventConst.MonthRetroactive, vO); };
                break;
            case EveryDaySignState.AlreadyGet:
                SetState(_alreadyGet.gameObject.name);                         
                break;
            case EveryDaySignState.NotGet:            
                SetState(_noGet.gameObject.name);   
                PointerClickListener.Get(_noGet.gameObject).onClick = go => { FlowText.ShowMessage(ClientData.GetItemDescById(vO.Awards[0].Id,vO.Awards[0].Resource).ItemDesc); };          
                break;
            case EveryDaySignState.CurMayGet:
                SetState(_curMayGet.gameObject.name); 
                PointerClickListener.Get(_curMayGet.gameObject).onClick = go => { EventDispatcher.TriggerEvent(EventConst.MonthSigin,vO); };
                break;          
        }
    }
	
    
    private void SetState(string objName)
    {
        for (int i = 0; i <_mask.childCount; i++)
        {
            _mask.GetChild(i).gameObject.SetActive(_mask.GetChild(i).gameObject.name == objName);
        }
    }
}
