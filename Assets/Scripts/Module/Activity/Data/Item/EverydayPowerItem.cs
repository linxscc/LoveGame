using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module;
using Common;
using game.main;
using game.tools;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;
public class EverydayPowerItem : MonoBehaviour {



    private Transform _icon;
    private Text _num;
    private Transform _mask;
    private Text _des;
    private GameObject _gem;   //钻石图标


    private Transform _retroactive;
    private Transform _noGet;
    private Transform _alreadyGet;
    private Transform _curMayGet;
    
    
    
    
    
    private void Awake()
    {


        _icon = transform.Find("Top/IconImage");
        _num = transform.GetText("Top/NumBg/Text");
        _mask = transform.Find("Top/Mask");
        _des = transform.GetText("Bottom/Bg/Text");
        _gem = transform.Find("Bottom/Bg/Image").gameObject;

        _retroactive = _mask.Find("Retroactive");
        _noGet = _mask.Find("NoGet");
        _alreadyGet = _mask.Find("AlreadyGet");
        _curMayGet = _mask.Find("CurMayGet");
                             
    }

    public void SetData(EveryDayPowerVO vO)
    {

        _num.text = vO.Awards[0].Num.ToString();
      
    
        switch (vO.State)
        {
           
            case EveryDaySignState.Retroactive:
                
                SetState(_retroactive.gameObject.name);
                _des.text = I18NManager.Get("Activity_Retroactive",vO.Gem);
                _gem.SetActive(true);

                IsShowBigIcon(false);
                
                PointerClickListener.Get(_retroactive.gameObject).onClick = go =>
                {                 
                    EventDispatcher.TriggerEvent<EveryDayPowerVO>(EventConst.OpenEverydayPowerAwardRetroactiveWindow, vO);
                };

                break;
            case EveryDaySignState.AlreadyGet:
                
                SetState(_alreadyGet.gameObject.name);
                _gem.SetActive(false);
                _des.text = I18NManager.Get("Activity_EverydayPowerAlreadyGet");

                var small = _icon.GetChild(1).GetComponent<Image>();
                var color = small.color;
                color =new Color(1,1,1,color.a);
                color=new Color(color.r*0.435f,color.g*0.376f,color.b*0.376f,color.a);
                small.color = color;
                
                transform.Find("Top/NumBg").gameObject.SetActive(false);
                
                IsShowBigIcon(false);
                break;
            case EveryDaySignState.NotGet:
                 _icon.GetChild(1).GetComponent<Image>().color =new Color(1,1,1);
                
                IsShowBigIcon(false);
                SetState(_noGet.gameObject.name);
                _gem.SetActive(false);
                _des.text = I18NManager.Get("Activity_EverydayPowerTime",vO.StarTime,vO.EndTime);
                PointerClickListener.Get(_noGet.gameObject).onClick = go =>
                {
                    FlowText.ShowMessage(I18NManager.Get("Activity_NoTimeTo"));//"还没有到时间哦"
                    
                };
                break;
            case EveryDaySignState.CurMayGet:
                
                
                IsShowBigIcon(true);
                _gem.SetActive(false);
                SetState(_curMayGet.gameObject.name);
                _des.text = I18NManager.Get("Activity_EverydayPowerTime",vO.StarTime,vO.EndTime);
                PointerClickListener.Get(_curMayGet.gameObject).onClick = go => 
                {                   
                    EventDispatcher.TriggerEvent<EveryDayPowerVO>(EventConst.GetEverydayPowerAward, vO);
                };
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

    private void IsShowBigIcon(bool isShowBigImage)
    {
        _icon.GetChild(0).gameObject.SetActive(isShowBigImage);
        _icon.GetChild(1).gameObject.SetActive(!isShowBigImage); 
    }
}
