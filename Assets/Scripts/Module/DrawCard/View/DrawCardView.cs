using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
using Assets.Scripts.Module;
using UnityEngine.Events;
using game.tools;
using game.main;
using Common;
using DataModel;
using UnityEngine.Experimental.AI;

public class DrawCardView : View
{
    public const string LIMIT_UI_0 = "0";
    public const string GOLD_UI = "1";
    public const string GEM_UI = "2"; 
    public const string ACTIVITY_UI = "3"; 
    public const string LIMIT_UI = "4";

    private TabSelectAnimBtnWidget _tabBtnBar;

    private DrawCardGemUI _drawCardGemUI;
    private DrawCardGoldUI _drawCardGoldUI;
    private DrawCardLimitUI _drawCardLimitUI;
    private DrawCardActivityUI _drawCardActivityUI;
    

    private void Awake()
    {
        _tabBtnBar = transform.Find("TabBtnBar").GetComponent<TabSelectAnimBtnWidget>();

        _drawCardGemUI = transform.Find("GemUI").GetComponent<DrawCardGemUI>();
        _drawCardGoldUI = transform.Find("GoldUI").GetComponent<DrawCardGoldUI>();
        _drawCardLimitUI = transform.Find("LimitUI").GetComponent<DrawCardLimitUI>();
        _drawCardActivityUI = transform.Find("ActivityUI").GetComponent<DrawCardActivityUI>();

        RectTransform rect = transform.GetRectTransform("Bg/BG2");
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y - ModuleManager.OffY/2);
    }

    private void Start()
    {
        _drawCardGemUI.Container = _container;
        _drawCardGoldUI.Container = _container;
        _drawCardLimitUI.Container = _container;
        _drawCardActivityUI.Container = _container;

        _tabBtnBar.Init(OnTabBtnSelect);
        InitTabBtnBarData();
        SetTabBtnPage(GEM_UI);
    }

    private void InitTabBtnBarData()
    {
        List<TabSelectAnimBtnData> list = new List<TabSelectAnimBtnData>();
        TabSelectAnimBtnData data = new TabSelectAnimBtnData();
        data.path = LIMIT_UI_0;
        data.isAlwayShow = true;
        data.lockState = true;
        list.Add(data);

        data = new TabSelectAnimBtnData();
        data.path = GOLD_UI;
        data.isAlwayShow = true;
        list.Add(data);

        data = new TabSelectAnimBtnData();
        data.path = GEM_UI;
        data.isAlwayShow = true;
        list.Add(data);

        data = new TabSelectAnimBtnData();
        data.path = ACTIVITY_UI;
        data.isAlwayShow = false;
        data.startTime = GlobalData.ConfigModel.GetGameTimeConfigByKey(GameConfigKey.DRAW_ACTIVITY_START_TIME);
        data.endTime = GlobalData.ConfigModel.GetGameTimeConfigByKey(GameConfigKey.DRAW_ACTIVITY_END_TIME);
        //Debug.LogWarning("drawCard endTime:"+ GlobalData.ConfigModel.GetGameTimeConfigByKey(GameConfigKey.DRAW_ACTIVITY_END_TIME));
        list.Add(data);

        data = new TabSelectAnimBtnData();
        data.path = LIMIT_UI;
        data.isAlwayShow = true;
        data.lockState = true;
        list.Add(data);

        _tabBtnBar.SetData(list);
    }

    public void SetTabBtnPage(string tab)
    {
        _tabBtnBar.SelectTabBtn(tab);
    }


    private void OnTabBtnSelect(string tab)
    {
        ShowSubUI(_drawCardGemUI, false);
        ShowSubUI(_drawCardGoldUI, false);
        ShowSubUI(_drawCardLimitUI, false);
        ShowSubUI(_drawCardActivityUI, false);
        switch (tab) {
            case GEM_UI:
                ShowSubUI(_drawCardGemUI, true);
                break;
            case LIMIT_UI:
                ShowSubUI(_drawCardLimitUI, true);
                break;
            case GOLD_UI:
                ShowSubUI(_drawCardGoldUI, true);
                break;
            case ACTIVITY_UI:
                ShowSubUI(_drawCardActivityUI, true);
                break;
                
        }

    }

    private void ShowSubUI(View ui, bool state)
    {
        ui.transform.localScale = (state) ? Vector3.one : Vector3.zero;
    }


    /// <summary>
    /// 更新星卡Text文本
    /// </summary>
    public void UpdateTicke(int num)
    {
        if (_drawCardGemUI != null)
        {
            _drawCardGemUI.UpdateTicke(num);
        }
        if (_drawCardActivityUI != null)
        {
            _drawCardActivityUI.UpdateTicke(num);
        }
    }

    public void SetData(int gemnum, int gemtotal, int goldnum, int goldtotal, int activityNum, int activityTotal, int[] costNum, int dropGemNum)
    {
        if (_drawCardGemUI != null)
        {
            _drawCardGemUI.SetData(gemnum, gemtotal, costNum, dropGemNum);
        }
        if (_drawCardGoldUI != null)
        {
            _drawCardGoldUI.SetData(goldnum, goldtotal, costNum);
        }
        if(_drawCardActivityUI != null)
        {
            _drawCardActivityUI.SetData(activityNum, activityTotal, costNum, dropGemNum);
        }
    }
    public void SetRemainTime(string str1, string str2, int[] costNum)
    {
        if(_drawCardGemUI != null)
        {
            _drawCardGemUI.SetRemainTime(str1, costNum);
        }
        if(_drawCardGoldUI != null)
        {
            _drawCardGoldUI.SetRemainTime(str2, costNum);
        }
    }


    public override void Hide()
    {
        gameObject.SetActive(false);
    }
    public override void Show(float delay)
    {
        gameObject.SetActive(true);

    }

    public void BuyStarcard(GameObject go)
    {
        string content = go == null ? I18NManager.Get("DrawCard_Hint14") :
              I18NManager.Get("DrawCard_Hint13");

        PopupManager.ShowConfirmWindow(content).WindowActionCallback = evt =>
        {
            if (evt == WindowEvent.Ok)
            {
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_SHOP, false, false,4);
            }
        };
    }
}
