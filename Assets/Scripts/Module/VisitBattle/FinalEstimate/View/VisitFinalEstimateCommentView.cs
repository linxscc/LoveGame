using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using Common;
using game.main;
using game.tools;
using Module.VisitBattle.Data;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class VisitFinalEstimateCommentView : View {
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

	void Start () {
        PointerClickListener.Get(_nextBtn.gameObject).onClick = ShowNextWindow;
    }
	
	void ShowNextWindow(GameObject go) {
      
		SendMessage(new Message(MessageConst.CMD_VISITBATTLE_SHOW_REWARD));
	}

    public void SetData(VisitBattleResultData data, VisitLevelVo level)
    {

        var commentContainer = transform.Find("Bg/CommentContainer");
	    List<VisitingLevelCommentRulePB> list = data.GetRandomComments(data.Star);

	    for (int i = 0; i < list.Count; i++)
        {
            var item = InstantiatePrefab("VisitBattle/FinalEstimate/CommentItem");
	        item.transform.SetParent(commentContainer, false);
	        CommentItem comment = item.GetComponent<CommentItem>();
	        comment.SetData(list[i]);
        }

     //   transform.Find("StarAndGrade/Text").GetComponent<Text>().text = "应援热度：<b> " + data.Cap + "</b>";
        transform.Find("Bg/StarAndGrade/Text/Text").GetComponent<Text>().text = data.Cap.ToString();
        transform.Find("Bg/StarAndGrade/Star").GetComponent<VisitStarComponent>().ShowStarAnimation(data.Star);

	    transform.Find("Bg/TitleText").GetComponent<Text>().text = level.LevelName;
	    
	   
    }

	
	public void Show(float delay)
    {
    }

    public void Hide()
    {
       
    }
	
}
