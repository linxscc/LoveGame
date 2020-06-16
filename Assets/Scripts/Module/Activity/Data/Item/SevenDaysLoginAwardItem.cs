using Assets.Scripts.Framework.GalaSports.Core.Events;

using Common;
using game.main;
using game.tools;

using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using DataModel;
using DG.Tweening;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class SevenDaysLoginAwardItem : MonoBehaviour
{


    private Transform _icon;
    private GameObject _mask;
    private GameObject _onClickBtn;
    private Text _dayText;
    private GameObject _getBtn;
    private Image _dayBg;
    private Text _num;   
    private SevenDaysLoginAwardVO _data;
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

  

    public void SetData(SevenDaysLoginAwardVO vo)   
    {      
        _data = vo;
        _icon.GetComponent<RawImage>().texture =
            ResourceManager.Load<Texture>(vo.IconPath, ModuleConfig.MODULE_ACTIVITY, true);  
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
                //触发礼包预览
                EventDispatcher.TriggerEvent(EventConst.PreviewAward, vo);

            }
             
        };

      
        _mask.SetActive(vo.IsShowGetMask);

        
      
      
        _dayText.text = I18NManager.Get("Activity_SevenActivityDay",vo.DayId);
        _getBtn.SetActive(vo.IsShowGetBtn);

        PointerClickListener.Get(_getBtn).onClick = go =>
        {
            EventDispatcher.TriggerEvent<SevenDaysLoginAwardVO>(
                vo.IsCardAward ? EventConst.GetCardAward : EventConst.GetNormalAward, vo);
        };     

        if (vo.DayId==7)
        {
            _dayBg.sprite =AssetManager.Instance.GetSpriteAtlas("UIAtlas_Activity_SevendaysActivityAwardItemDaysBg2");
            _num.transform.parent.Hide();
        }

      
        
    }


}
