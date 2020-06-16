using System;


[Serializable]
public class ActivityPlayRuleVo
{
    public string Name;
    public string Key;
    public string TitleName;
    public string[] Paths;


    public ActivityPlayRuleVo Clone()
    {
        ActivityPlayRuleVo vo =new ActivityPlayRuleVo();

        vo.Name = Name;
        vo.Key = Key;
        vo.TitleName = TitleName;
        vo.Paths = Paths;

        return vo;
    }
}
