using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using UnityEngine;


public class ReloadingController : Controller
{
    public ReloadingView View;
    private ReloadingModel _model;

    private ReloadingVO _cloth;
    private ReloadingVO _background;


   

    public override void Start()
    {

       
        _model = new ReloadingModel();
        var curRoleVo = GlobalData.FavorabilityMainModel.CurrentRoleVo;
        
        View.SetInfo(curRoleVo,_model.GetBgImagePath(curRoleVo.Apparel[1]));
     
        View.CreateClothsAndBackgrounds(_model.GetList(ReloadingListState.Clothing),_model.GetList(ReloadingListState.Backgroud));
        
        EventDispatcher.AddEventListener<ReloadingVO>(EventConst.ReloadingItemClick, OnReloadingItemClick);
       
    }
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        var pb = GlobalData.FavorabilityMainModel.CurrentRoleVo.Player;
        switch (name)
        {
            
          case MessageConst.CMD_RELOADING_RESET_CLOTH_RED_FRAME:
              var clothItemId = Convert.ToInt32(message.Body) ;  
              _model.UpdateListItemRedFrameShow(clothItemId, ReloadingListState.Clothing);
              View.RefreshRedFrameShow(_model.GetList(ReloadingListState.Clothing), ReloadingListState.Clothing);
              _cloth = _model.GetData(clothItemId, ReloadingListState.Clothing);
              break;
          case MessageConst.CMD_RELOADING_RESET_BACKGROUND_RED_FARME:
              var backgroundItemId = Convert.ToInt32(message.Body) ;
              _model.UpdateListItemRedFrameShow(backgroundItemId, ReloadingListState.Backgroud);
              View.RefreshRedFrameShow(_model.GetList(ReloadingListState.Backgroud), ReloadingListState.Backgroud);
              _background = _model.GetData(backgroundItemId, ReloadingListState.Backgroud);
              break;            
            case MessageConst.CMD_FACORABLILITY_VIEW_ONSAVE_BTN:
                OnClickSaveBtnEvent();              
                break;
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEventListener<ReloadingVO>(EventConst.ReloadingItemClick, OnReloadingItemClick);
    }


    /// <summary>
    /// 隐藏
    /// </summary>
    public void Hide()
    {
        View.gameObject.Hide();
    }

    /// <summary>
    /// 显示
    /// </summary>
    public void Show()
    {
        View.transform.SetAsLastSibling();
        View.gameObject.Show();
    }



    /// <summary>
    /// 点击预设物
    /// </summary>
    private void OnReloadingItemClick(ReloadingVO vo)
    {
       
        switch (vo.ItemType)
        {
            case DressUpTypePB.TypeClothes:
                _cloth = vo;               
                _model.UpdateListItemRedFrameShow(vo.ItemId, ReloadingListState.Clothing);
                View.ShowHint(vo,GlobalData.FavorabilityMainModel.CurrentRoleVo,_model);
                View.RefreshRedFrameShow(_model.GetList(ReloadingListState.Clothing),ReloadingListState.Clothing);
                break;
            case DressUpTypePB.TypeBackground:
                _background = vo;             
                _model.UpdateListItemRedFrameShow(vo.ItemId, ReloadingListState.Backgroud);               
                var path = _model.GetBgImagePath(vo.ItemId);
                View.ShowHint(vo,GlobalData.FavorabilityMainModel.CurrentRoleVo,_model,path);
                View.RefreshRedFrameShow(_model.GetList(ReloadingListState.Backgroud),ReloadingListState.Backgroud);
                break;           
        }
        
    }

    /// <summary>
    /// 点击保存按钮设置Key值
    /// </summary>
    private void OnClickSaveBtnEvent()
    {

        var curRole = GlobalData.FavorabilityMainModel.CurrentRoleVo;
        var clothId = curRole.Apparel[0];
        var background = curRole.Apparel[1];
        
        if (_cloth==null)
        {
            _cloth = _model.GetData(clothId, ReloadingListState.Clothing);
        }
        if (_background==null)
        {
            _background = _model.GetData(background, ReloadingListState.Backgroud); 
        }

        if (_cloth.IsGet && _background.IsGet)
        {
           //发请求 
           SendSaveReq(curRole.Player);
        }
        else
        {
            if (_cloth.IsGet==false )
            {
                FlowText.ShowMessage(I18NManager.Get("Reloading_ClothNounlock"));
            }

            if (_background.IsGet==false)
            {
                FlowText.ShowMessage(I18NManager.Get("Reloading_BackgroundNounlock"));
            }
        }        
    }


    private void SendSaveReq(PlayerPB player)
    {
       DressUpReq req =new  DressUpReq
       {
           Player = player,
           ItemIds = { _cloth.ItemId,_background.ItemId}
       };
       
       byte[] data = NetWorkManager.GetByteData(req);

       NetWorkManager.Instance.Send<DressUpRes>(CMD.FAVORABILITY_DRESSUP, data, DressUpSucceed);
    }

    private void DressUpSucceed(DressUpRes res)
    {
        GlobalData.FavorabilityMainModel.UpdataCurrentRoleVo(res.UserFavorability);
        GlobalData.PropModel.UpdateProps(res.UserItems);
        GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
     
        var curRoleVo = GlobalData.FavorabilityMainModel.CurrentRoleVo;        
        View.SetInfo(curRoleVo,_model.GetBgImagePath(curRoleVo.Apparel[1]));
        
       FlowText.ShowMessage(I18NManager.Get("Common_SaveSucceed"));
        
    }
}
