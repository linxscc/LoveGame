using Assets.Scripts.Framework.GalaSports.Core;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Framework.GalaSports.Core.Message;
using DataModel;

public class AirborneGameAwardView : View
{
    private LoopVerticalScrollRect _awardList;
    List<AirborneGameRunningItemVo> _winRunningItem;

    private void Awake()
    {
        transform.Find("GetBtn").GetComponent<Button>().onClick.AddListener(OnClickGet);
        //missionRulePBs = new List<MissionRulePB>();

        _awardList = transform.Find("Scroll View").GetComponent<LoopVerticalScrollRect>();
        _awardList.prefabName = "AirborneGame/Items/AirborneGameAwardItem";
        _awardList.poolSize = 6;
        _awardList.totalCount = 0;

        ClientData.LoadItemDescData(null);
        ClientData.LoadSpecialItemDescData(null);
    }

    public void SetData(List<AirborneGameRunningItemVo> winRunningItem)
    {
        Debug.Log("ActivityAwardWindow");

        _winRunningItem = winRunningItem;
        _awardList.UpdateCallback = AwardListUpdateCallback;
        _awardList.totalCount = _winRunningItem.Count;
        _awardList.RefreshCells();
    }

    private void AwardListUpdateCallback(GameObject go, int index)
    {
        go.GetComponent<AirborneGameAwardItem>().SetData(_winRunningItem[index]);
    }

    public void OnClickGet()
    {
        SendMessage(new Message(MessageConst.CMD_AIRBORNEGAME_GET_GAME_AWARD, MessageReciverType.CONTROLLER));
    }
}
