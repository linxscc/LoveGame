using Com.Proto;
using Google.Protobuf.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneGameInfo  {

    public int CurPlayedNum;//当日已经玩过的次数
    public int CurPlayedLeftNum
    {
        get
        {
            return MaxGameNum - CurPlayedNum;
        }
    }


    public int MaxGameNum
    {
        get
        {
            return _gameJumpRulePB.Count;
        }
    }

    public int MaxGameTime
    {
        get
        {
            return _gameJumpRulePB.Time;
        }
    }
    GameJumpRulePB _gameJumpRulePB;

    private RepeatedField<GameJumpItemRulePB> _itemRules;

    private Dictionary<ItemTypeEnum, GameJumpLevelRulePB> _JumpLevelRule;  //量级信息

    public int _nextUnlockLevel;
    public int NextUnlockLevel
    {
        get
        {
            return _nextUnlockLevel;
        }
    }

    public AirborneGameInfo(GameJumpInfosRes res,int level)
    {
        CurPlayedNum = res.DailyCount;
        _gameJumpRulePB = res.Rules[0];
        _itemRules = res.ItemRules;
        _JumpLevelRule = new Dictionary<ItemTypeEnum, GameJumpLevelRulePB>();
        _nextUnlockLevel = int.MaxValue;
        foreach (var v in res.LevelRules) 
        {
            if(v.Level>level)
            {
                if(_nextUnlockLevel> v.Level)
                {
                    _nextUnlockLevel = v.Level;
                }
                continue;
            }
            if(!_JumpLevelRule.ContainsKey(v.ItemType))
            {
                _JumpLevelRule[v.ItemType] = v;
                continue;
            }
            if(_JumpLevelRule[v.ItemType].Level<v.Level)
            {
                _JumpLevelRule[v.ItemType] = v;
                continue;
            }
        }
    }

    public GameJumpLevelRulePB GetGameJumpLevelRule(ItemTypeEnum type)
    {
        return _JumpLevelRule[type];
    }

    public GameJumpItemRulePB GetGameJumpItemRulePB(int resourceId, ItemTypeEnum itemTypeEnum)
    {
        foreach(var v in _itemRules)
        {
            if (v.ReourceId == resourceId && v.ItemType == itemTypeEnum)  
            {
                return v;
            }
        }
        return null;
    }

}
