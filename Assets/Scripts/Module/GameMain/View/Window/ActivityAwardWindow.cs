using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class ActivityAwardWindow : Window
{
    private LoopVerticalScrollRect _awardList;
    private List<AwardPB> _awardPBs;
    private int _activityId;
    
    private void Awake()
    {
        transform.Find("GetBtn").GetComponent<Button>().onClick.AddListener(OnClickGet);
        //missionRulePBs = new List<MissionRulePB>();

        _awardList = transform.Find("Scroll View").GetComponent<LoopVerticalScrollRect>();
        _awardList.prefabName = "GameMain/Prefabs/ActivityAward/ActivityAwardItem";
        _awardList.poolSize = 6;
        _awardList.totalCount = 0;
    }

    public void SetData(List<MissionRulePB> missionRulePB,int activityID)
    {
        Debug.Log("ActivityAwardWindow");

        _awardPBs = new List<AwardPB>();
        foreach (var v in missionRulePB)
        {
            _awardPBs.AddRange(v.Award.ToList());
        }

        _awardList.UpdateCallback = AwardListUpdateCallback;
        _awardList.totalCount = _awardPBs.Count;
        _awardList.RefreshCells();

        _activityId = activityID;
    }

    private void AwardListUpdateCallback(GameObject go, int index)
    {
        go.GetComponent<ActivityAwardItem>().SetData(_awardPBs[index]);
    }

    public void OnClickGet()
    {
        EventDispatcher.TriggerEvent(EventConst.ActivitySignInClick, _activityId);
        Close();
    }
    protected override void OnClickOutside(GameObject go)
    {

    }


}
