using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using UnityEngine;

public class FavorabilityGiftController : Controller
{
   public FavorabilityGiftView View;
   private FavorabilityGiveGiftsModel _model;

   public override void Start()
   {      
      EventDispatcher.AddEventListener<FavorabilityGiveGiftsItemVO>(EventConst.FavorabilityGiveGiftsItemClick, OnFavorabilityGiveGiftsItemClick);
   }

   public void StarCreate(Transform tra)
   {
      _model =new FavorabilityGiveGiftsModel();
      View.SetLevelTra(tra);
      View.IsShowHintText(_model.UserGiveGiftsItemList.Count);
      View.IsPitchOnItem(_model.IsPitchOn());
      View.CreateGiveGiftsItems(_model.UserGiveGiftsItemList);
   }
   
   
   private void OnFavorabilityGiveGiftsItemClick(FavorabilityGiveGiftsItemVO vo)
   {
      _model.UpdataListItemRedFrameShow(vo);
      View.IsPitchOnItem(_model.IsPitchOn());
      View.RefreshGiveGiftsItemRedFrameShow(_model.UserGiveGiftsItemList);
      _model.CurrentItemVo = vo;
      View.GetCurPitchOnGiveGiftsItem(_model.UserGiveGiftsItemList.IndexOf(vo), vo);    
   }

   public override void OnMessage(Message message)
   {
      string name = message.Name;
      object[] body = message.Params;
      switch (name)
      {
         case MessageConst.CMD_FAVORABILITY_GIVEGIFTS_BTN:
            View.IsGetRemoteServerData = false;
            LoadingOverlay.Instance.Show();
            var num = Convert.ToInt32(message.Body) ;
            SendGiveGiftsRequest(_model.CurrentItemVo, num);  
            break;
         case MessageConst.CMD_DISIPOSITION_REMO_GIVEGIFTS_ITEM:   
            var index =  Convert.ToInt32(message.Body);  
            _model.Remove(index); 
            _model.ResetRedFrameState();
            View.IsShowHintText(_model.UserGiveGiftsItemList.Count);
            View.IsPitchOnItem(_model.IsPitchOn());         
            break;
      }
   }

   private void SendGiveGiftsRequest(FavorabilityGiveGiftsItemVO vo, int useNum)
   {
      UpgradeFavorabilityLevelReq req = new UpgradeFavorabilityLevelReq
      {
         Player = GlobalData.FavorabilityMainModel.CurrentRoleVo.Player,
         ItemId = vo.ItemId,
         Num = useNum,
      };
      byte[] data = NetWorkManager.GetByteData(req);
      NetWorkManager.Instance.Send<UpgradeFavorabilityLevelRes>(CMD.FAVORABILITY_UPGRADEFAVORABILITYLEVEL, data,  res =>      
      {
         var realyExp =  GlobalData.FavorabilityMainModel.GetCurExp(res.UserFavorability.Exp, res.UserFavorability.Level);
         GlobalData.PropModel.UpdateProps(new UserItemPB[] { res.UserItem });
         GlobalData.FavorabilityMainModel.UpdataCurrentRoleVo(res.UserFavorability);
         View.UpDataDummyData(res.UserItem.Num, realyExp, res.UserFavorability.Level,res.UserFavorability.Exp);       
         _model.UpDataListItemNum(res.UserItem.ItemId, res.UserItem.Num);
         View.IsGetRemoteServerData = true;
         LoadingOverlay.Instance.Hide(); 
         var pbId =(int) GlobalData.FavorabilityMainModel.CurrentRoleVo.Player;
         
         SendMessage(new Message(MessageConst.CMD_FACORABLILITY_TIGGER_LIVE2D_VOICE, Message.MessageReciverType.CONTROLLER,pbId,vo.ItemId));
      }, errorVo =>
      {
         View.IsGetRemoteServerData = true;
         LoadingOverlay.Instance.Hide(); 
      });
   }

   public override void Destroy()
   {
      EventDispatcher.RemoveEventListener<FavorabilityGiveGiftsItemVO>(EventConst.FavorabilityGiveGiftsItemClick, OnFavorabilityGiveGiftsItemClick);
   }
   
}
