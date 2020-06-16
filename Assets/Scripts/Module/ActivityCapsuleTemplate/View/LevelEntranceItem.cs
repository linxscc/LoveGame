using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using game.main;
using game.tools;
using UnityEngine;

public class LevelEntranceItem : MonoBehaviour
{

    public void SetData(CapsuleLevelVo vo,int id)
    {
       var open= transform.Find("Open").gameObject;
       var noOpen= transform.Find("NoOpen").gameObject;

       if (vo.IsOpen)
       {
           open.Show();
           noOpen.Hide();
       }
       else
       {
           open.Hide();
           noOpen.Show();
       }
       
       transform.GetRawImage("LevelNameIcon").texture = ResourceManager.Load<Texture>("ActivityCapsuleTemplate/Level"+id);

       PointerClickListener.Get(transform.Find("OnClick").gameObject).onClick = go =>
       {
           if (vo.IsOpen)
           {
               //触发进入战斗的事件
               EventDispatcher.TriggerEvent(EventConst.OnClickCapsuleBattleEntrance,vo);
           }
           else
           {
               FlowText.ShowMessage(I18NManager.Get("ActivityCapsuleTemplate_PleasePassLastLevel"));
           } 
       };      
    }
}
