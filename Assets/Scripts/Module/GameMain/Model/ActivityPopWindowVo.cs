using System;


[Serializable]
public class ActivityPopWindowVo
{
       
     public string Name;
     public int Sort;
     public int Group;
     public string Path;
     public long StarTimeStamp;
     public long EndTimeStamp;
     public int AdvanceDay;
     public string ModuleName;
     public string ActivityJumpToId;
     public string PopupType;

     public ActivityPopWindowVo Clone()
     {
          ActivityPopWindowVo vo =new ActivityPopWindowVo();
        
          vo.Name = Name;
          vo.Sort = Sort;
          vo.Group = Group;
          vo.Path = Path;
          vo.StarTimeStamp = StarTimeStamp;
          vo.EndTimeStamp = EndTimeStamp;
          vo.AdvanceDay = AdvanceDay;
          vo.ModuleName = ModuleName;
          vo.ActivityJumpToId = ActivityJumpToId;
          vo.PopupType = PopupType;
         
          return vo;
     }
}
