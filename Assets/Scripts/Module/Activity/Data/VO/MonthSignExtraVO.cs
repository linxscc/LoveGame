using Com.Proto;
using DataModel;
using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonthSignExtraVO
{


    public int Year;
    public int Month;
    public List<MonthSignExtraAwardVO> Awards = new List<MonthSignExtraAwardVO>();
    public int TotalDate; // 需要签到天数
    public bool IsCard=false;
    
    public MonthSignExtraVO(MonthSignExtraRulePB pB)
    {
        Year = pB.Year;
        Month = pB.Month;
        AddAwards(pB.Awards);
        TotalDate = pB.TotalDate;
       
    }

    private void AddAwards(RepeatedField<AwardPB> list)
    {
        foreach (var t in list)
        {
            var award = new MonthSignExtraAwardVO(t);
            if (t.Resource== ResourcePB.Card)
            {
                IsCard = true;
            }
            Awards.Add(award);
        }
    }






}
