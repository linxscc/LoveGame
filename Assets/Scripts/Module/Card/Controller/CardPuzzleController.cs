using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using UnityEngine;

public class CardPuzzleController : Controller {
	
	public CardPuzzleView View;
	private List<CardPuzzleVo> _puzzleList;

	public CardPuzzleController()
	{
		EventDispatcher.AddEventListener<CardPuzzleVo>(EventConst.CardPuzzleClick, OnCardPuzzleClick);
	}

	private void OnCardPuzzleClick(CardPuzzleVo vo)
	{
		byte[] buffer = NetWorkManager.GetByteData(new CompoundReq
		{
			CardId = vo.CardId,
			Num = 1
		});
		NetWorkManager.Instance.Send<CompoundRes>(CMD.CARDC_COMPOUND, buffer, OnCompound);
	}

	private void OnCompound(CompoundRes res)
	{
		//同步版号包
		//GlobalData.CardModel.UpdateUserCardsByIdAndNum(res.UserCards[0].CardId,res.UserCards[0].Num);
//		var usercardvo=new UserCardPB[]{};
//		for (int i = 0; i < res.UserCards.Count; i++)
//		{
//			usercardvo[i] = res.UserCards[i].Clone();
//		}
		
		
		GlobalData.CardModel.UpdateUserCards(res.UserCards.ToArray());


		GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
		
		for (int i = 0; i < _puzzleList.Count; i++)
		{
			if (_puzzleList[i].CardId == res.UserPuzzle.CardId)
			{
				if (res.UserPuzzle.Num == 0)
				{
					_puzzleList.RemoveAt(i);
				}
				else
				{
					_puzzleList[i].Num = res.UserPuzzle.Num;
				}
				break;
			}
		}
		SendMessage(new Message(MessageConst.CMD_CARD_REFRESH_USER_CARDS));
		FlowText.ShowMessage(I18NManager.Get("Card_Compound"));
		Action finish = () =>
		{
			AudioManager.Instance.PlayEffect("compoundPuzzle");

			if (GlobalData.CardModel.GetUserCardById(res.UserPuzzle.CardId).CardVo.Credit<=CreditPB.Sr)
			{
				EventDispatcher.TriggerEvent(EventConst.ShowStoreScore);

			}

         
		};
		List<AwardPB> Awards = new List<AwardPB>();
		AwardPB pb = new AwardPB();
		pb.ResourceId =  res.UserPuzzle.CardId ;
		pb.Resource = ResourcePB.Card;
		Awards.Add(pb);
		ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD, false, false, "DrawCard_CardShow", Awards, finish);
		_puzzleList.Sort();
		View.SetData(_puzzleList);
		
		
		

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
	        case MessageConst.CMD_CARD_COLLECTION_GET_USER_PUZZLUE:
		        GetUserPuzzle();
		        break;;
        }
    }

	private void GetUserPuzzle()
	{
		NetWorkManager.Instance.Send<MyPuzzleRes>(CMD.CARDC_MYPUZZLE, null, OnGetMyPuzzle);
	}

	private void OnGetMyPuzzle(MyPuzzleRes res)
	{
		_puzzleList = new List<CardPuzzleVo>();
		for (int i = 0; i < res.UserPuzzles.Count; i++)
		{
			CardPuzzleVo vo = new CardPuzzleVo(res.UserPuzzles[i]);
			_puzzleList.Add(vo);
		}
		_puzzleList.Sort();
		GlobalData.CardModel.CardPuzzleList = _puzzleList;
		View.SetData(GlobalData.CardModel.CardPuzzleList);
	}

	public override void Destroy()
	{
		base.Destroy();
		EventDispatcher.RemoveEventListener<CardPuzzleVo>(EventConst.CardPuzzleClick, OnCardPuzzleClick);
	}
}
