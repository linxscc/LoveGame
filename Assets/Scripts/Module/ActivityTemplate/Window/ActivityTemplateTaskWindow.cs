using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class ActivityTemplateTaskWindow : Window
{
   private Text _titleTxt;
   private Transform _parent;
   private void Awake()
   {
      _titleTxt = transform.GetText("Title/Text");
      _titleTxt.text = I18NManager.Get("ActivityTemplate_ActivityTemplateGetTitle");

      _parent = transform.Find("Tasks/Content");
      
   }



   public void SetData(List<ActivityTemplateTaskVo> list)
   {
      var prefab = GetPrefab("ActivityTemplate/Prefabs/ActivityTemplateTaskItem");
      foreach (var t in list)
      {
         GameObject go = Instantiate(prefab, _parent, false);
         go.GetComponent<ActivityTemplateTaskItem>().SetData(t);
      }
   }

   public void JumpToCloseWindow()
   {
      base.Close();
   }
   
   
}
