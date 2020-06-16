using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using Componets;
using GalaAccount.Scripts.Framework.Utils;
using GalaAccountSystem;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActivityMonthSigninView : View
{
  
    private Image _mask;

    private Transform _accumulateSigninCardTra; 
    private Transform _accumulateSigninItemTra;
    private Text _nameTxt;
    private Text _descTxt;
    private Text _curAccumulateSigninDayTxt;

    private Transform _parent;
    
    private Button _getAccumulativeAwardBtn;  //累计18天领取奖励按钮
    
    private void Awake()
    {
        _mask = transform.GetImage("Content/Awards");
        _accumulateSigninCardTra = transform.Find("Content/AccumulateSigninCard");
        _accumulateSigninItemTra = transform.Find("Content/AccumulateSigninItem");
        _nameTxt = transform.GetText("Content/Name");
        _descTxt=transform.GetText("Content/Desc");
        _curAccumulateSigninDayTxt = transform.GetText("Content/CurAccumulateSigninDayBg/Text");
        _parent = transform.Find("Content/Awards/Content");
        _getAccumulativeAwardBtn = transform.GetButton("Content/GetAccumulativeAwardBtn");
        _getAccumulativeAwardBtn.onClick.AddListener(GetAccumulativeAward);

    }

    private void GetAccumulativeAward()
    {
        Debug.LogError("发送累签请求");
        SendMessage(new Message(MessageConst.CMD_ACTIVITY_MONTH_SIGIN_GET_BTN)); 
    }


    public void ShowMask(bool isShow)
    {
        _mask.enabled = isShow;
    }
    
    public void Init(MonthSignExtraVO vO, int signNum,int totalDate,int isGet)
    {

        _curAccumulateSigninDayTxt.text = I18NManager.Get("Activity_Hint6",GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_SIGN_RESET_DAY), signNum);
        _nameTxt.text = vO.Awards[0].Name;
        if (vO.IsCard)
        {
            _accumulateSigninCardTra.gameObject.Show();
            _accumulateSigninItemTra.gameObject.Hide();

            var path ="Activity/MonthSign_"+ vO.Awards[0].Id;
            _accumulateSigninCardTra.GetRawImage().texture = ResourceManager.Load<Texture>(path, ModuleConfig.MODULE_ACTIVITY);          
        }
        else
        {
            _accumulateSigninCardTra.gameObject.Hide();
            _accumulateSigninItemTra.gameObject.Show();
            var path ="Activity/MonthSign_"+ vO.Awards[0].Id;
            _accumulateSigninItemTra.GetRawImage().texture = ResourceManager.Load<Texture>(path, ModuleConfig.MODULE_ACTIVITY);           
        }
        
        if (signNum >= totalDate && isGet ==1)
        {
            _descTxt.text = I18NManager.Get("Activity_Hint8",vO.TotalDate);
        }
        else
        {
            _descTxt.text = I18NManager.Get("Activity_Hint1",vO.TotalDate);
        }

        CheckGetAccumulativeAward(signNum, isGet, totalDate);


    }




    /// <summary>
    /// 刷新累计签到显示
    /// </summary>
    public void RefreshAccumulativeSignin(int signNum,int isGet,int totalDate)
    {
      
        _curAccumulateSigninDayTxt.text = I18NManager.Get("Activity_Hint6",GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.MONTH_SIGN_RESET_DAY),signNum);
        CheckGetAccumulativeAward(signNum, isGet, totalDate);

    }

    private void CheckGetAccumulativeAward(int signNum,int isGet,int totalDate)
    {
        if (signNum<totalDate)
        {
          _getAccumulativeAwardBtn.gameObject.Hide();   
        }
        else if(signNum>=totalDate)
        {
            if (isGet==0)
            {
                 _getAccumulativeAwardBtn.gameObject.Show();  
            }
            else
            {
                _getAccumulativeAwardBtn.gameObject.Hide();    
            }
        }
    }

    public void RefreshAccumulate(int totalDate)
    {
        _descTxt.text = I18NManager.Get("Activity_Hint8",totalDate);  
    }


    public void CreateMonthSignData(List<MonthSignAwardVO> monthSignAwards)
    {
        DestroyItem();
        var prefab =  GetPrefab("Activity/Prefabs/MonthSginAwardItem");

        foreach (var t in monthSignAwards)
        {
            var item = Instantiate(prefab, _parent, false) ;
            
            item.name = t.DayId.ToString();
            item.GetComponent<MonthSginAwardItem>().SetData(t);
        }
    }

    private void DestroyItem()
    {
        Debug.LogError("DestroyItem");
        for (int i = _parent.childCount - 1; i >= 0; i--)
        {
           DestroyImmediate(_parent.GetChild(i).gameObject); 
        }
    }

    public void UpdateMonthSignItemUI(MonthSignAwardVO vO,int dayId)
    {

        for (int i = 0; i < _parent.childCount; i++)
        {
            if (_parent.GetChild(i).name== dayId.ToString())
            {
               _parent.GetChild(i).GetComponent<MonthSginAwardItem>() .SetData(vO);
               break;
            }
        }
       //_parent.GetChild(id - 1).GetComponent<MonthSginAwardItem>().SetData(vO);
    }

    
    /// <summary>
    /// 设置Content位置
    /// </summary>
    public void SetContentPos(int toDayId)
    {
      
        var parentRect = _parent.GetComponent<RectTransform>();               
        var height = _parent.GetComponent<GridLayoutGroup>().cellSize.y;   //每行的高度
        float num = _parent.GetComponent<GridLayoutGroup>().constraintCount; //每行的个数
        var line =Convert.ToInt32(Math.Ceiling(toDayId/num))-1  ;
        var y = line * height;

        if (line >=3)
        {
            y = 438f;
        }
        
        if (toDayId>30 && line >=3)
        {
            y = 696f; 
        }
        parentRect.DOAnchorPos(new Vector2(0, y), 1f,true); 

    }



    

}
