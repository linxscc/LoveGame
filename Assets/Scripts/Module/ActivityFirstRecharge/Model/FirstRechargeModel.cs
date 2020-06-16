using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class FirstRechargeModel: Model
{

    
    private List<FirstRechargeVO> _fixedAwards;            //固定奖励集合
    private List<FirstRechargeVO> _optionalAwards;         //可选择奖励集合
    private ActivityOptionalAwardRulePB _firstAwardRule;   //首充规则
    
    
    public FirstRechargeModel()
    {
        InitFirstAwardRule();
        InitFirstRechargeAwards();
    }


    /// <summary>
    /// 初始化首充规则
    /// </summary>
    private void InitFirstAwardRule()
    {
        var baseActivityRule = GlobalData.ActivityModel.BaseActivityRule;
        foreach (var t in baseActivityRule.ActivityOptionalAwardRules)
        {
            if (t.OptionalActivityType== OptionalActivityTypePB.OptionalFirstRush)
            {
                _firstAwardRule = t;
                break;
            }   
        }
    }

    /// <summary>
    /// 初始化首充奖励
    /// </summary>
    private void InitFirstRechargeAwards()
    {       
        var fixedAwardsRule =_firstAwardRule.FixedAward;
        var optionalAwardsRule = _firstAwardRule.OptionalAward;
    
        if (_fixedAwards == null)
            _fixedAwards = new List<FirstRechargeVO>();

        if (_optionalAwards == null)
            _optionalAwards = new List<FirstRechargeVO>();
      
        foreach (var t in fixedAwardsRule)
        {
            var vo = new FirstRechargeVO(t);
            _fixedAwards.Add(vo);
        }

        for (int i = 0; i < optionalAwardsRule.Count; i++)
        {
            if (optionalAwardsRule[i].Resource != ResourcePB.Card) continue;
            var vo = new FirstRechargeVO(optionalAwardsRule[i]) {Group = i + 1};
            _optionalAwards.Add(vo);
        }   
        
        SetOptionalAwardsSort();
    }


    /// <summary>
    /// 手动设置可选奖励排序。集合顺序的修改来自美术图（这样是不好的行为）
    /// </summary>
    private void SetOptionalAwardsSort()
    {
        var temp = _optionalAwards[0];
        _optionalAwards[0] = _optionalAwards[1];
        _optionalAwards[1] = temp;
       
    }
    

    /// <summary>
    /// 获取首冲固定奖励
    /// </summary>
    /// <returns></returns>
    public List<FirstRechargeVO> GetFixedAwards()
    {
        return _fixedAwards;
    }

    /// <summary>
    /// 获取首冲可选择奖励
    /// </summary>
    /// <returns></returns>
    public List<FirstRechargeVO> GetOptionalAwards()
    {
        return _optionalAwards;
    }

     

    /// <summary>
    /// 是否首冲过
    /// </summary>
    /// <returns></returns>
    public bool IsRecharge()
    {
        bool isRecharge = false;     
        var firstPrize = GlobalData.PlayerModel.PlayerVo.ExtInfo.FirstPrize;    //领取状态
        if (firstPrize == FirstPrizeStatusPB.FpUnaccalimed )
        {
            isRecharge = true;
        }
        return isRecharge;
    }
}
