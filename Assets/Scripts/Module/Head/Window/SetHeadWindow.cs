using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using GalaAccount.Scripts.Framework.Utils;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetHeadWindow : Window
{
    private RawImage _headImg;
    private RawImage _headFrameImg;


    private Transform _headContent;
    private Transform _headFrameContent;

    private Button _cancelBtn;
    private Button _okBtn;


    private int _headId;
    private int _headFrameId;

    private Transform _headFrameParent;
    private Transform _headParent;

    private Transform _headTogs;
    private HeadModel _model;

    private string _curHeadPath;

    private Transform _frameTog;
    
    private void Awake()
    {
        _headImg = transform.GetRawImage("Head/HeadIconMask/HeadIcon");
        _headFrameImg = transform.GetRawImage("Head/HeadIconMask/HeadFrame");

        _frameTog = transform.Find("Togs/FrameTog");
        
        SetToggle(transform.Find("Togs"));

        _headContent = transform.Find("Middle/HeadContent");
        _headFrameContent = transform.Find("Middle/HeadFrameContent");
        _headParent = _headContent.Find("Content/HeadParent");
        _headFrameParent = _headFrameContent.Find("Content/HeadFrameParent");
        _cancelBtn = transform.GetButton("CancelBtn");
        _okBtn = transform.GetButton("OkBtn");

        _cancelBtn.onClick.AddListener(OnClickCancel);
        _okBtn.onClick.AddListener(OnClickOk);
        
        _headTogs = transform.Find("Middle/HeadContent/HeadTogsContent/HeadTogs");
        SetHeadTog(_headTogs);
        AddEvent();
    }

    private void SetToggle(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Toggle toggle = parent.GetChild(i).GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnTabChange);
        }
    }


    private void SetHeadTog(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Toggle toggle = parent.GetChild(i).GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnHeadTabChange);
        } 
    }

    
    
    private void OnHeadTabChange(bool isOn)
    {
        if (isOn == false)
            return;
        string name = EventSystem.current.currentSelectedGameObject.name;

       
        PlayerPB pb = PlayerPB.None;

        if (name=="All")
        {
            SetFontColor("All");
            CreateHeadItem(_model.GetAllUserHeadData());
            return;  
        }
        
        switch (name)
        {                  
            case "Other":
                SetFontColor("Other");
                pb =  PlayerPB.None;
                break;
            case "Tang":
                SetFontColor("Tang");
                pb =  PlayerPB.TangYiChen;
                break;
            case "Qin":
                SetFontColor("Qin");
                pb = PlayerPB.QinYuZhe;
                break;
            case "Yan":
                SetFontColor("Yan");
                pb = PlayerPB.YanJi;
                break;
            case "Chi":
                SetFontColor("Chi");
                pb = PlayerPB.ChiYu;
                break;
        }
        
       
        CreateHeadItem(_model.GetUserHeadData(pb));
    }

    private void SetFontColor(string togName)
    {
        Debug.LogError("---"+togName);
        for (int i = 0; i < _headTogs.childCount; i++)
        {
            var go = _headTogs.GetChild(i).gameObject;
            var label = go.transform.Find("Label").gameObject;
            var label2 = go.transform.Find("Label2").gameObject;
            var isPitchOn = togName == go.name;

          

            if (isPitchOn)
            {
                label.Show();
                label2.Hide();
            }
            else
            {
                label.Hide();
                label2.Show();
            }
            
           

        }
    }
    
    private void OnTabChange(bool isOn)
    {
        if (isOn == false)
        {
            return;
        }

        string name = EventSystem.current.currentSelectedGameObject.name;
        switch (name)
        {
            case "HeadTog":
                _headContent.gameObject.Show();
                _headFrameContent.gameObject.Hide();
                break;
            case "FrameTog":
                _headContent.gameObject.Hide();
                _headFrameContent.gameObject.Show();
                break;
        }
    }

    //点击Ok按钮
    private void OnClickOk()
    {
        SendChangeHeadReq();
    }

    //点击Cancel按钮
    private void OnClickCancel()
    {
        RemoveEvent();
        WindowEvent = WindowEvent.Cancel;
        CloseAnimation();
    }

    private void SetHeadFrameRedDot()
    {
      var redDot =  _frameTog.Find("Red").gameObject;
      var isShow = _model.IsShowHeadFrameRedDot();
      redDot.SetActive(isShow);
    }
    
    public void SetData(HeadModel model)
    {
        _model = model;
        _curHeadPath = model.GetCurPlayerHeadPath();
        var userData = GlobalData.PlayerModel.PlayerVo.UserOther;
        _headId = userData.Avatar;
        _headFrameId = userData.AvatarBox;

             
        CreateHeadItem(model.GetAllUserHeadData());
        SetFontColor("All");
        CreateHeadFrameItem(model.UserHeadFrameData);
       
       var  curHeadVo = model.GetHeadInfo(_headId);
       var curHeadFrameVo = model.GetHeadFrameVo(_headFrameId);
           
      
        
     
       _headImg.texture = ResourceManager.Load<Texture>(curHeadVo.Path);
       _headFrameImg.texture =ResourceManager.Load<Texture>(curHeadFrameVo.Path);

       _headContent.GetText("Text").text = "";
       _headFrameContent.GetText("Text").text = "";
       SetHeadFrameRedDot();
    }
  
    //生成头像Item
    private void CreateHeadItem(List<HeadVo> list)
    {
        GameObject prefab = GetPrefab("GameMain/Prefabs/HeadItem");

        if (_headParent.childCount>0)
        {
            for (int i = _headParent.childCount - 1; i >= 0; i--)
            {
               Destroy(_headParent.GetChild(i).gameObject); 
            }
        }
        
        foreach (var data in list)
        {
            var go = Instantiate(prefab, _headParent, false);
            go.name = data.Id.ToString();
            if (_headId==data.Id)
            {
                go.transform.Find("Bottom").gameObject.Show(); 
            }

            go.AddComponent<HeadItem>();
            go.GetComponent<HeadItem>().SetData(data);
        }
    }

    //生成头像框Item
    private void CreateHeadFrameItem(List<HeadFrameVo> list)
    {
        GameObject prefab = GetPrefab("GameMain/Prefabs/HeadFrameItem");
     
        foreach (var data in list)
        {
            var go = Instantiate(prefab, _headFrameParent, false);
            go.name = data.Id.ToString();
            if (_headFrameId == data.Id)
            {
                go.transform.Find("Bottom").gameObject.Show();
            }      
            go.GetComponent<HeadFrameItem>().SetData(data,_curHeadPath);
        }
    }


    //发送更换头像请求
    private void SendChangeHeadReq()
    {
        //本地校验

        var isHeadUnlock = _model.GetHeadInfo(_headId).IsUnlock;
        var isHeadFrameUnlock =_model.GetHeadFrameVo(_headFrameId).IsUnlock;

        if (!isHeadUnlock || !isHeadFrameUnlock)
        {
            if (!isHeadUnlock)
                FlowText.ShowMessage("当前头像未解锁~");

            if (!isHeadFrameUnlock)
                FlowText.ShowMessage("当前头像框未解锁~");

            return;
        }

        //发更换请求
        LoadingOverlay.Instance.Show();
        ReplaceUserAvatarOrBoxReq req = new ReplaceUserAvatarOrBoxReq {  Avatar = _headId,  AvatarBox = _headFrameId,};   
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<ReplaceUserAvatarOrBoxRes>(CMD.USERC_REPLACEUSERAVATARORBOX, data,
            GetChangeHeadRes);

    }

    //获得更换头像回包
    private void GetChangeHeadRes(ReplaceUserAvatarOrBoxRes res)
    {
   
        GlobalData.PlayerModel.SetUser(res.User);

        UpdateHeadFrame();
        EventDispatcher.TriggerEvent(EventConst.UpdateMainViewHeadInfo);
        LoadingOverlay.Instance.Hide();
        RemoveEvent();
        WindowEvent = WindowEvent.Ok;
        CloseAnimation();
    }

    //添加事件
    private void AddEvent()
    {
        EventDispatcher.AddEventListener<HeadFrameVo>(EventConst.OnClickHeadFrame, OnClickHeadFrame);
        EventDispatcher.AddEventListener<HeadVo>(EventConst.OnClickHead, OnClickHead);
       
    }


    private void UpdateHeadFrame()
    {
        _curHeadPath = _model.GetCurPlayerHeadPath();
        
        for (int i = 0; i < _headFrameParent.childCount; i++)
        {
            var go = _headFrameParent.GetChild(i).gameObject;
            go.GetComponent<HeadFrameItem>().SetCurHead(_curHeadPath);
        }
    }
    
   
    
    //移除事件
    private void RemoveEvent()
    {
        EventDispatcher.RemoveEvent(EventConst.OnClickHead);
        EventDispatcher.RemoveEvent(EventConst.OnClickHeadFrame);
    }


    /// <summary>
    /// 点击头像Item
    /// </summary>
    /// <param name="vo"></param>
    private void OnClickHead(HeadVo vo)
    {
        _headId = vo.Id;
        Text dest = _headContent.GetText("Text");
        dest.text = vo.Desc;
        ShowPitchOnItemBottom(_headParent, vo.Id);
        _headImg.texture = ResourceManager.Load<Texture>(vo.Path);
    }

    /// <summary>
    /// 点击头像框Item
    /// </summary>
    /// <param name="vo"></param>
    private void OnClickHeadFrame(HeadFrameVo vo)
    {
        _headFrameId = vo.Id;
        Text dest = _headFrameContent.GetText("Text");
        dest.text = vo.Desc;
     
        ShowPitchOnItemBottom(_headFrameParent, vo.Id);
        _headFrameImg.texture = ResourceManager.Load<Texture>(vo.Path);
        _model.UpdateUserHeadFrameData(vo);
        SetHeadFrameRedDot();
    }


    private void ShowPitchOnItemBottom(Transform parent, int id)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).name == id.ToString())
            {
                parent.GetChild(i).transform.Find("Bottom").Show();
            }
            else
            {
                parent.GetChild(i).transform.Find("Bottom").Hide();
            }
        }
    }
    
    protected override void OnClickOutside(GameObject go)
    {
    }
}