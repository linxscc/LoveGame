using Com.Proto;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneGameRunningGroupInfo
{
    public int Group;
    public float TriggerTime;
    public int CurIndex;
}

public class AirborneGameRunningInfo
{
    Dictionary<int, List<AirborneGameRunningItemVo>> _runningDic;
    List<AirborneGameRunningItemVo> _winRunningItem;//已获得物品列表


    Dictionary<int, AirborneGameRunningGroupInfo> _runningGroupDic;

    public List<AirborneGameRunningItemVo> WinRunningItem
    {
        get
        {
            return _winRunningItem;
        }
    }

    public List<GameJumpItemPB> GameJumpItemPBs
    {
        get
        {
            _gameJumpItemPBs.Clear();
            foreach (var v in _winRunningItem)
            {
                var item = _gameJumpItemPBs.Find((m) => { return m.ReourceId == v.ResourceId&&m.ItemType==v.Itemtype; });
                if (item == null) 
                {
                    item = new GameJumpItemPB();
                    item.ReourceId = v.ResourceId;
                    item.Reource =(int) v.Resource ;
                    item.ItemType = v.Itemtype;
                    item.Count = 0;
                    _gameJumpItemPBs.Add(item);
                }
                item.Count += v.Count;
            }
            return _gameJumpItemPBs;
        }
    }
    List<GameJumpItemPB> _gameJumpItemPBs;


    public float MaxTime
    {
        set;
        get;
    }
    public float TimeLine;

    public AirborneGameRunningInfo()
    {
        _runningDic = new Dictionary<int, List<AirborneGameRunningItemVo>>();
        _winRunningItem = new List<AirborneGameRunningItemVo>();
        _gameJumpItemPBs = new List<GameJumpItemPB>();
        _runningGroupDic = new Dictionary<int, AirborneGameRunningGroupInfo>();
    }


    public void AddWinRunningItem(AirborneGameRunningItemVo vo)
    {
        _winRunningItem.Add(vo);
    }



    public List<AirborneGameRunningItemVo> GetRunningItemByGroupId(int groupId)
    {
        return _runningDic[groupId];
    }

    public void  AddRunningItemsByGroupId(int groupId, List<AirborneGameRunningItemVo> list)
    {

        if(!_runningDic.ContainsKey(groupId))
        {
            _runningDic[groupId] = new List<AirborneGameRunningItemVo>();
            _runningGroupDic[groupId] = new AirborneGameRunningGroupInfo()
            {
                Group = groupId,
                CurIndex = 0,
                TriggerTime = 0,

            };
        }
        _runningDic[groupId].AddRange(list);
    }

    public void SortRunningItems()
    {
        foreach(var vk in _runningDic)
        {
            vk.Value.Sort();
        }
    }

   //int curIndex = 0;
    int curGroup = 0;//默认0，触发一次  增加一次

    public void TriggerDouble(float curTime)
    {
        curGroup++;
        _runningGroupDic[curGroup].TriggerTime = curTime;
    }

    public bool CheckHasRunningItem(float curTime)
    {
        for(int i=0;i<=curGroup;i++)
        {
            int curIndex = _runningGroupDic[i].CurIndex;
            if (curIndex >= _runningDic[i].Count)
            {
                continue;
            }
            if(_runningDic[i][curIndex].TriggerTime+ _runningGroupDic[i].TriggerTime <= curTime)
            {
                return true;
            }
        }
        return false;
    }

    public AirborneGameRunningItemVo GetRunningItem(float curTime)
    {

        for (int i = 0; i <=curGroup; i++)
        {
            int curIndex = _runningGroupDic[i].CurIndex;
            if (curIndex >= _runningDic[i].Count)
            {
                continue;
            }
            if (_runningDic[i][curIndex].TriggerTime + _runningGroupDic[i].TriggerTime <= curTime)
            {
                _runningGroupDic[i].CurIndex++;
                return _runningDic[i][curIndex];
            }
        }
        return null;
    }

}
