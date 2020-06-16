using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.Proto;
using DataModel;
using UnityEngine;

public class ActivityExchangeShopVo
{
    public int ActivityId; 
    public int MallId; // 商品id   
    public int MallItemId;//兑换道具ID
    public string MallName;// 商品名称
    public string MallDesc;// 商品描述
    public string GiftImage;    // 礼包图片
    public string LabelImage;// 商品类型展示图片 
    public int Price;// 价格
    public int BuyMax;// 购买上限 
    public int BuyNum=0;// 购买次数
    public int RemainBuyNum;// 剩余购买次数  
    public List<RewardVo> Rewards;
    public string ExchangeIconPath;  //兑换消耗的Icon路径
    
    public ActivityExchangeShopVo(ActivityMallRulePB pb)
    {
        ActivityId = pb.ActivityId;
        MallId = pb.MallId;
        MallItemId = pb.MallItemId;
        MallName = pb.MallName;
        MallDesc = pb.MallDesc;
        GiftImage = pb.GiftImage;
        LabelImage = pb.LabelImage;
        Price = pb.Price;
        BuyMax = pb.BuyMax;       
        RemainBuyNum = BuyMax - BuyNum;
        InitRewards(pb.Awards.ToList());
        ExchangeIconPath = "Prop/"+pb.MallItemId;
    }

    private void InitRewards(List<AwardPB> awardPbs)
    {
        Rewards =new List<RewardVo>();
        foreach (var t in awardPbs)
        {
            RewardVo vo= new RewardVo(t);
            Rewards.Add(vo); 
        }
    }
}
