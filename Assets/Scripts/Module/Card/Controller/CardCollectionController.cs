using System.Collections.Generic;
using System.Collections;
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

namespace Module.Card.Controller
{
	public class CardCollectionController : Assets.Scripts.Framework.GalaSports.Core.Controller {
	
		public CardCollectionView View;

		public override void Start()
		{
			LoadingOverlay.Instance.Show();
			EventDispatcher.AddEventListener<UserCardVo>(EventConst.CollectedCardClick, OnCardItemClick);
            NetWorkManager.Instance.Send<MyCardRes>(CMD.CARDC_MYCARD, null, OnGetCardList);
            View.SetMyCardData(GlobalData.CardModel.UserCardList);
			
			
			//需要获得抽卡的卡池数据，有点坑爹，为什么不放到静态数据中去呢?
			NetWorkManager.Instance.Send<DrawProbRes>(CMD.DRAWC_DRAW_PROBS, null, OnGetCardTotalNum, null, true, GlobalData.VersionData.VersionDic[CMD.DRAWC_DRAW_PROBS]);
			
			//一开始就要设置全部标签为0
			GlobalData.CardModel.CurPlayerPb = PlayerPB.None;
        }

		private void OnGetCardTotalNum(DrawProbRes res)
		{
			LoadingOverlay.Instance.Hide();
			DrawData drawData = GetData<DrawData>();
			drawData.InitData(res);
			
			//显示总数！
			View.SetCardNum(GlobalData.CardModel.OpenBaseCards);
			
		}

		/// <summary>
		/// 处理View消息
		/// </summary>
		/// <param name="message"></param>
		public override void OnMessage(Message message)
		{
			string name = message.Name;
			object[] body = message.Params;
			switch (name)
			{
				case MessageConst.CMD_CARD_REFRESH_USER_CARDS:
//					Debug.LogError(GlobalData.CardModel.UserCardList.Count);
					View.SetMyCardData(GlobalData.CardModel.UserCardList);
					break;;
			}
		}
		

		private void OnGetCardList(MyCardRes res)
		{
			GlobalData.CardModel.InitMyCards(res);
			View.SetMyCardData(GlobalData.CardModel.UserCardList);
		}

		private void OnCardItemClick(UserCardVo vo)
		{
			Debug.Log("1Card Item Click"+vo.CardId);

			SendMessage(new Message(MessageConst.MODULE_CARD_COLLECTION_SHOW_CARD_DETAIL_VIEW,
				Message.MessageReciverType.DEFAULT, vo));
		}

		public override void Destroy()
		{
			base.Destroy();
			EventDispatcher.RemoveEventListener<UserCardVo>(EventConst.CollectedCardClick, OnCardItemClick);
			
		}
		
		
	}


}
