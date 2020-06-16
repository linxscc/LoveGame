using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using DataModel;
using game.main;
using game.tools;
using System.Collections.Generic;
using Module.Activity.View;
using UnityEngine;
using UnityEngine.UI;

public class NormalAwardWindow : Window
{

    private Transform _content;
    private Transform _parent;
    private Text _titleText;
    private Text _retroactiveText;
    private Button _ok;
        
    private void Awake()
    {
        _content = transform.Find("Content");
        _parent = _content.Find("Awards");
        _titleText = transform.Find("Title/Text").GetComponent<Text>();             
        _retroactiveText = transform.GetText("RetroactiveText");
        _ok = transform.GetButton("Ok");
    }


    /// <summary>
    /// 月签获得奖励
    /// </summary>
    /// <param name="list"></param>
    public void SetData(List<RewardVo> list)
    {
        _titleText.text = I18NManager.Get("Common_GetAward");
        CreateNormalAwardItem(list);
        _ok.gameObject.Show();
        _ok.onClick.AddListener(() =>
        {
            base.Close();
        });
    }
    
    /// <summary>
    /// 七日签到获取奖励
    /// </summary>
    /// <param name="vO"></param>
    public void SetData(SevenDaysLoginAwardVO vO)  
    {
        _titleText.text = I18NManager.Get("Common_GetAward");
        CreateNormalAwardItem(vO.Rewards);
        _ok.gameObject.Show();
        _ok.onClick.AddListener(() =>
        {
            base.Close();
        });
    }
    
    /// <summary>
    /// 七日签到模板获取奖励
    /// </summary>
    /// <param name="vO"></param>
    public void SetSevenSigninTemplateAwardData(SevenSigninTemplateAwardVo vO)  
    {
        _titleText.text = I18NManager.Get("Common_GetAward");
        CreateNormalAwardItem(vO.Rewards);
        _ok.gameObject.Show();
        _ok.onClick.AddListener(() =>
        {
            base.Close();
        });
    }

    /// <summary>
    /// 每日体力窗口
    /// </summary>
    /// <param name="vO">每日奖励</param>
    /// <param name="isRetroactive">是否补签</param>
    public void SetData(EveryDayPowerVO vO,bool isRetroactive=false)
    {    
        CreateNormalAwardItem(vO.Awards);

        if (isRetroactive)
        {        
            _titleText.text = I18NManager.Get("Common_Retroactive");
            _titleText.transform.parent.gameObject.Show();
            _content.gameObject.Hide();
            _retroactiveText.gameObject.Show();
            _retroactiveText.text = I18NManager.Get("Activity_RetroactiveHint1",vO.Gem,vO.Awards[0].Num);
            var btn = transform.Find("Btn");
            btn.gameObject.SetActive(true);

            PointerClickListener.Get(btn.Find("CancelButton").gameObject).onClick = go => { base.Close();};
            PointerClickListener.Get(btn.Find("OkButton").gameObject).onClick = go => {
                EventDispatcher.TriggerEvent(EventConst.SendRetroactiveEverydayPowerAwardReq, vO);
                base.Close();
            };
        }
        else
        {
            _titleText.text = I18NManager.Get("Common_GetAward");
            _ok.gameObject.Show();
            _ok.onClick.AddListener(() =>
            {
                base.Close();
            });  
        }

    }
   
    /// <summary>
    /// 生成预制体
    /// </summary>
    /// <param name="list"></param>
    private void CreateNormalAwardItem(List<RewardVo> list)
    {
        var prefab = GetPrefab("Activity/Prefabs/ActivityAwardItem");
        foreach (var t in list)
        {
            var go = Instantiate(prefab,_parent,false) ;                      
            go.GetComponent<ActivityAwardsItem>().SetData(t);
        }
    }

    protected override void OnClickOutside(GameObject go)
    {
        //base.OnClickOutside(go);
    }
}

