using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.Proto;
using DataModel;
using UnityEngine;


/*
 * 星路历程活跃奖励预览Vo
 * 
 */

public class AwardPreviewVo
{
    public int Km;   
    public string Desc;
    public List<RewardVo>  Rewards;
    public bool IsGet = false;
    public PlayerPB Player;
    
    
    public AwardPreviewVo(MissionActivityRewardRulePB pb)
    {
        Km = pb.Weight;
        Desc = I18NManager.Get("Achievement_AwardPreview", Km);
        Player = pb.Player;
        SetAwards(pb.Awards.ToList());
        SetIsGet();
    }


    private void SetAwards(List<AwardPB> awardPbs)
    {
        Rewards =new  List<RewardVo>();
        foreach (var t in awardPbs)
        {
            var vo = new RewardVo(t);
            Rewards.Add(vo);
        }
        
    }

    private void SetIsGet()
    {
        var isKey = GlobalData.MissionModel.StarCourseSchedule.ContainsKey(Player);

        if (!isKey)
            return;

        var value = GlobalData.MissionModel.StarCourseSchedule[Player];
        var isGet = value.List.Contains(Km);

        if (isGet)
            IsGet = true;
    }
    
    
}
