using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FinalEstimateCommentWindow : Window
{
    private Transform _starContainer;
    private Transform _nextBtn;

    private void Awake()
    {
        _nextBtn = transform.Find("NextBtn");
        _starContainer = transform.Find("Bg/StarAndGrade/Star");
        for (int i = 0; i < 3; i++)
        {
            _starContainer.GetChild(i).gameObject.Hide();
        }
    }

    void Start()
    {
        PointerClickListener.Get(_nextBtn.gameObject).onClick = ShowNextWindow;
    }

    private void ShowNextWindow(GameObject go)
    {
        SendMessage(new Message(MessageConst.CMD_BATTLE_SHOW_REWARD));
    }


    protected override void OnClickOutside(GameObject go)
    {
    }

    public void SetData(BattleResultData data, LevelVo level)
    {
        var commentContainer = transform.Find("Bg/CommentContainer");
        List<LevelCommentRulePB> list = data.GetRandomComments(data.Star);

        for (int i = 0; i < list.Count; i++)
        {
            var item = InstantiatePrefab("Battle/FinalEstimate/CommentItem");
            item.transform.SetParent(commentContainer, false);
            CommentItem comment = item.GetComponent<CommentItem>();
            comment.SetData(list[i]);
        }

        //   transform.Find("StarAndGrade/Text").GetComponent<Text>().text = "应援热度：<b> " + data.Cap + "</b>";
        transform.Find("Bg/StarAndGrade/Text/Text").GetComponent<Text>().text = data.Cap.ToString();
        transform.Find("Bg/StarAndGrade/Star").GetComponent<StarComponent>().ShowStarAnimation(data.Star);

        transform.Find("Bg/TitleText").GetComponent<Text>().text = level.LevelName;
    }
}