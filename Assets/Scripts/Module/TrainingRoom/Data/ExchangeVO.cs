
using System.Collections.Generic;
using Com.Proto;
using DataModel;
using Google.Protobuf.Collections;

public class ExchangeVO
{
    public int PropId;
    public int SlotId;
    public int Price;
    public int ShopId;

    public string Name;
    public string IconPath;
    public int Num;

    public string Desc;
    public List<RewardVo> Rewards = new List<RewardVo>();


    /// <summary>
    /// 是否已购买
    /// </summary>
    public bool IsBuy;

    public ExchangeVO(MallInfoPB pb)
    {
        SlotId = pb.SlotId;    
        Price = pb.Price;      
        ShopId = pb.ShopId;       
        AddRewards( pb.Awards);
        SetInfo();
    }

    private void SetInfo()
    {
        RewardVo rewardVo = Rewards[Rewards.Count - 1];
        Name = rewardVo.Name;
        Num = rewardVo.Num;
        IconPath = rewardVo.IconPath;
        PropId = rewardVo.Id;
       
        Desc = I18NManager.Get("Shop_GoodsDesc")+ClientData.GetItemDescById(rewardVo.Id,  rewardVo.Resource).ItemDesc;
    }

    private void AddRewards(RepeatedField<AwardPB> list)
    {
        foreach (var t in list)
        {
            RewardVo vo = new RewardVo(t);
            Rewards.Add(vo);
        }
    }
}
