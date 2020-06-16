using System.Collections;
using System.Collections.Generic;
using Com.Proto;
using DataModel;
using game.main;


public class UserMusicGameVO
{

    public int ActivityId;     //音乐活动类型(1~4,4是Vip专属)
    public int MusicChapterId; //音乐活动id
    public int NeedAbility;   //需要的能力类型
    public int Finish;        //是否完成 0是未完成， 1是已完成
    
    
    
    
 
   
    public string ActivityName; //活动名字
    public int AwardIntegral;    //奖励积分
    public string NeedAbilityDesc; //演奏要求描述
    public bool IsVipMusicGame;//是否Vip专属音乐游戏
    public bool IsFinish;      //是否完成  
   
    public bool IsVip;
    public string Ability;
    public int NeedAbilityNum;
    public int NeedStarNum;
    public List<TrainingRoomCardVo> UserCards =new List<TrainingRoomCardVo>();     
    
    public UserMusicGameVO(UserMusicGamePB pb)
    {
        ActivityId = pb.ActivityId;
        MusicChapterId = pb.MusicChapterId;
        NeedAbility = pb.NeedAbility;
        Finish = pb.Finish;

        var musicData = GlobalData.TrainingRoomModel.GetMusicData(pb.ActivityId, pb.MusicChapterId);
        ActivityName = musicData.MusicName;
        AwardIntegral = musicData.Score;
        NeedAbilityDesc = musicData.NeedStarNum + "个" + GlobalData.TrainingRoomModel.GetAbility(pb.NeedAbility) + "大于" + musicData.NeedAbilityNum + "的星缘";
        
        Ability = GlobalData.TrainingRoomModel.GetAbility(pb.NeedAbility);
        NeedAbilityNum = musicData.NeedAbilityNum;
        NeedStarNum = musicData.NeedStarNum;
        
        SetVipMusicGame();
        IsFinish = Finish == 1;
        IsVip = GlobalData.PlayerModel.PlayerVo.IsOnVip;
    }


    private void SetVipMusicGame()
    {
        IsVipMusicGame = ActivityId==4;
    }


    
    
    
   

   
    
}
