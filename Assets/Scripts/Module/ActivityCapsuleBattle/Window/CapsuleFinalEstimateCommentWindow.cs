using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using game.main;
using game.tools;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleFinalEstimateCommentWindow : Window
{
   private Transform _starContainer;
   private Transform _commentContainer;
   private Transform _nextBtn;
   private void Awake()
   {      
      _nextBtn = transform.Find("NextBtn");
      _starContainer = transform.Find("Bg/StarAndGrade/Star");      
      for (int i = 0; i < _starContainer.childCount; i++)
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
      SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_SHOW_REWARD));
   }
   public void SetData(CapsuleBattleResultData data, LevelVo level)
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


   protected override void OnClickOutside(GameObject go)
   {
      
   }

 

 
}
