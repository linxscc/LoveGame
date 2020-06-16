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
using Module.Activity.View;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FirstRechargeWindow : Window
{


       private Transform _parent;
       private Button _ok;
       private ScrollRect _scrollRect;
       private RectTransform _awardsRect;
       
       private void Awake()
       {
           _parent = transform.Find("GetAward/Content/Awards");
           _ok = transform.GetButton("GetAward/OkBtn");
           _awardsRect = _parent.GetRectTransform();
           _scrollRect = transform.Find("GetAward/Content").GetComponent<ScrollRect>();
           _ok.onClick.AddListener((() =>
           {                           
               base.Close();
           }));
       }



       public void SetData(List<FirstRechargeVO> awards)
       {

           if (awards.Count>3)
           {
               _scrollRect.movementType = ScrollRect.MovementType.Elastic;
               _awardsRect.pivot =new Vector2(0,0.5f);
           }
           var prefab = GetPrefab("Activity/Prefabs/ActivityAwardItem");
           
           foreach (var i in awards)
           {
               var item = Instantiate(prefab, _parent, false);
               item.transform.localScale = Vector3.one;        
               item.GetComponent<ActivityAwardsItem>().SetData(i.RewardVo);
           }
           
       }


       protected override void OnClickOutside(GameObject go)
       {
         
       }
}
