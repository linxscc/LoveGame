using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf.Collections;
using UnityEngine;

/// <summary>
/// 兑换商店Model
/// </summary>
public class ExchangeShopModel : Model
{


	private string _noRefreshHint = string.Empty; //不能刷新提示语
	private int _refreshMallNum;  //兑换商店刷新次数
	private List<ExchangeVO> _exchangeShops; //兑换商店售卖兑换商品List
	
	
	private List<RefreshDataPB> _exchangeShopRefreshRules; //兑换商店刷新规则
	private List<MallInfoPB> _exchangeShopInfoRules;      //兑换商店信息规则
	
	
	/// <summary>
	/// 进入兑换商店界面
	/// </summary>
	/// <param name="res"></param>
	public void InitOpenExchangeShop(MallRes res)
	{
		InitExchangeShopRefreshRules(GlobalData.TrainingRoomModel.GetRefreshRules());
		InitExchangeShopInfoRules(GlobalData.TrainingRoomModel.GetExchangeShopRules());		
		_refreshMallNum = res.RefreshMallCount;   						
		InitExchangeShopInfo(res.Info);		
	}

	
	///初始化商店售卖道具List
	private void InitExchangeShopInfo(RepeatedField<ShopInfo> list)
	{
		_exchangeShops = new List<ExchangeVO>();
		foreach (var t in list)
		{
			MallInfoPB pb = GetShopItemInfo(t.ShopId);
			ExchangeVO vo =new ExchangeVO(pb);
			vo.IsBuy = t.Buy;
			_exchangeShops.Add(vo);
		}
              
	}

	///初始化商店刷新规则
	private void InitExchangeShopRefreshRules(RepeatedField<RefreshDataPB> list)
	{
		if (_exchangeShopRefreshRules==null)
		{
			_exchangeShopRefreshRules =new List<RefreshDataPB>();
		}
		foreach (var t in list)
		{
			if (t.RefreshType==1)
			{
			  _exchangeShopRefreshRules.Add(t);	
			}
		}
	}

	///初始化商店信息规则
	private void InitExchangeShopInfoRules(RepeatedField<MallInfoPB> list)
	{
		if (_exchangeShopInfoRules==null)
		{
			_exchangeShopInfoRules= new List<MallInfoPB>();
		}
		_exchangeShopInfoRules = list.ToList();
	}
	
	
	
	private MallInfoPB GetShopItemInfo(int shopId)
	{
		MallInfoPB pb =null;
		var rules = _exchangeShopInfoRules;
		foreach (var t in rules)
		{
			if (t.ShopId==shopId)
			{
				pb= t;
				break;
			}  
		}
		return pb;
	}
	
	/// <summary>
	/// 获取兑换商店售卖道具List
	/// </summary>
	/// <returns></returns>
	public List<ExchangeVO> GetExchangeShops()
	{
		return _exchangeShops;
	}

	/// <summary>
	/// (刷新)更新兑换商店售卖道具List
	/// </summary>
	/// <param name="list"></param>
	public void UpdateExchangeShops(RepeatedField<ShopInfo> list)
	{
	   	_exchangeShops =new List<ExchangeVO>();
	    foreach (var t in list)
	    {
		    MallInfoPB pb = GetShopItemInfo(t.ShopId);
		    ExchangeVO vo =new ExchangeVO(pb);
		    vo.IsBuy = t.Buy;
		    _exchangeShops.Add(vo);
	    }
	}


	/// <summary>
	/// (购买)更新兑换商店售卖道具
	/// </summary>
	/// <param name="vo"></param>
	public void BuyLaterUpdateExchangeShops(ExchangeVO vo)
	{
		for (int i = 0; i < _exchangeShops.Count; i++)
		{
			if (_exchangeShops[i].ShopId==vo.ShopId)
			{
				_exchangeShops[i] = vo;
				break;				
			}
		}
	}
	
	

	/// <summary>
	/// 获取兑换商店刷新次数
	/// </summary>
	/// <returns></returns>
	public int GetRefreshMallNum()
	{
		return _refreshMallNum;
	}


	/// <summary>
	/// 更新兑换商店商店刷新次数
	/// </summary>
	/// <param name="num"></param>
	public void UpdateRefreshMallNum(int num)
	{
		_refreshMallNum = num;
	}



	/// <summary>
	/// 获取当前刷新规则
	/// </summary>
	/// <param name="curNum">当前刷新次数</param>
	/// <returns></returns>
	public RefreshDataPB GetCurRefreshRules(int curNum)
	{
		RefreshDataPB pb = null;
		// var refreshNum = curNum + 1;
		foreach (var t in _exchangeShopRefreshRules)
		{
			if (t.RefreshTimes==curNum)
			{
				pb = t;
				break;
			}
		}		
		return pb;
	}


	/// <summary>
	/// 获取今日可以刷新次数
	/// </summary>
	/// <returns></returns>
	public int GetTodayMayRefreshNum()
	{
		return _exchangeShopRefreshRules.Count - _refreshMallNum;
	}


    /// <summary>
    /// 是否可以刷新
    /// </summary>
    /// <returns></returns>
	public bool IsMayRefresh()
	{
		bool temp = true;
		//钻石数量
		var gem = GlobalData.PlayerModel.PlayerVo.Gem;		
		//当前需要消耗的数量
		var curCostGemNum = GetCurRefreshRules(_refreshMallNum).ResourceNum;
		if (gem<curCostGemNum)
		{
			temp = false;
			_noRefreshHint = "星钻不足";
		}						
		return temp;
	}

    /// <summary>
    /// 获取不能刷新提示语
    /// </summary>
    /// <param name="hint"></param>
    /// <returns></returns>
    public string GetNoRefreshHint()
    {	    
	    return _noRefreshHint;
    }
	
}
