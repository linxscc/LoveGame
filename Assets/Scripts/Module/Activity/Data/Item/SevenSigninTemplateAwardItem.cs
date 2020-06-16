using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Common;
using DataModel;
using game.main;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using UnityEngine;
using UnityEngine.UI;

public class SevenSigninTemplateAwardItem : MonoBehaviour
{
    private Transform _icon;
    private GameObject _mask;
    private GameObject _onClickBtn;
    private Text _dayText;
    private GameObject _getBtn;
    private Image _dayBg;
    private Text _num;
    private SevenSigninTemplateAwardVo _data;
    private GameObject _debris;


    private void Awake()
    {
        _icon = transform.Find("Award/Icons/Mask/Icon");
        _mask = transform.Find("Mask").gameObject;
        _onClickBtn = transform.Find("OnClickBtn").gameObject;
        _dayText = transform.GetText("Day/Text");
        _getBtn = transform.Find("GetBtn").gameObject;
        _dayBg = transform.GetImage("Day");
        _num = transform.GetText("Award/NumBg/Text");
        _debris = transform.Find("Award/Debris").gameObject;
    }


    public void SetData(SevenSigninTemplateAwardVo vo)
    {
        _data = vo;
        if (vo.DayId!=7)
        {
            transform.Find("Last").gameObject.Hide();
            _icon.GetComponent<RawImage>().texture = ResourceManager.Load<Texture>(vo.IconPath, ModuleConfig.MODULE_ACTIVITY, true);  
            _icon.GetComponent<RawImage>().SetNativeSize();
            _debris.SetActive(vo.IsPuzzle);
            _num.text = vo.IsGiftbag ? vo.GiftbagName : vo.Rewards[0].Num.ToString();
            PointerClickListener.Get(_onClickBtn).onClick = go =>
            {
                if (!vo.IsGiftbag)
                {
               
                    FlowText.ShowMessage(ClientData.GetItemDescById(vo.Rewards[0].Id,vo.Rewards[0].Resource).ItemDesc);   
                }
                else
                {
                    //触发礼包预览监听
                    EventDispatcher.TriggerEvent(EventConst.ActivityTemplatePreviewAward, vo);
                }
            };
        
            _mask.SetActive(vo.IsShowGetMask);
            _dayText.text = I18NManager.Get("Activity_SevenActivityDay",vo.DayId);
            _getBtn.SetActive(vo.IsShowGetBtn);
       
            PointerClickListener.Get(_getBtn).onClick = go =>
            {
                EventDispatcher.TriggerEvent(EventConst.GetActivityTemplateAward, vo);
            };  
        }
        else
        {
            transform.GetImage().enabled = false;
            transform.Find("Last/Icon").GetRawImage().enabled = false;
            transform.Find("Last/Icon/Day").localPosition =new Vector3(-171f,-46f,0);
            transform.Find("Award").gameObject.Hide();
            transform.Find("OnClickBtn").gameObject.Hide();
            transform.Find("GetBtn").gameObject.Hide();
            transform.Find("Mask").gameObject.Hide();
            transform.Find("Day").gameObject.Hide();
            transform.Find("Last").gameObject.Show();

            PointerClickListener.Get(transform.Find("Last/OnClickBtn").gameObject).onClick = go =>
            {
                if (!vo.IsGiftbag)
                {              
                    FlowText.ShowMessage(ClientData.GetItemDescById(vo.Rewards[0].Id,vo.Rewards[0].Resource).ItemDesc);   
                }
                else
                {                  
                    EventDispatcher.TriggerEvent(EventConst.ActivityTemplatePreviewAward, vo);
                } 
            };
            
            transform.Find("Last/Mask").gameObject.SetActive(vo.IsShowGetMask);
            transform.Find("Last/GetBtn").gameObject.SetActive(vo.IsShowGetBtn);
            transform.Find("Last/NameBg/Image/Text").GetText().text = vo.LastName;
          //  transform.Find("Last/Icon").GetRawImage().texture = ResourceManager.Load<Texture>(vo.LastIconPath);
          //  transform.Find("Last/Icon").GetRectTransform().sizeDelta = vo.LastIconV2;
        
            if (vo.IsShowGetMask==false && vo.IsShowGetBtn==false )//没到领取的状态
            {                          
                  transform.Find("Last/NameBg").gameObject.SetActive(true); 
            }
            else if (vo.IsShowGetBtn && vo.IsShowGetMask==false)
            {
                transform.Find("Last/Icon/Day").gameObject.Hide();
                transform.Find("Last/NameBg").gameObject.SetActive(false);  
            }
            else if(vo.IsShowGetBtn==false && vo.IsShowGetMask)
            {
                transform.Find("Last/NameBg").gameObject.SetActive(true); 
                transform.Find("Last/NameBg/Image/Text").GetText().text =
                    I18NManager.Get("Activity_EverydayPowerAlreadyGet");
            }
                
          
           
            
            PointerClickListener.Get( transform.Find("Last/GetBtn").gameObject).onClick = go =>
            {
              EventDispatcher.TriggerEvent(EventConst.GetActivityTemplateAward, vo);
            }; 
            
        }
             
    }
}
