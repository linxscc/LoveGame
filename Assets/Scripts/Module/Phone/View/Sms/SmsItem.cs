using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Download;
using Common;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class SmsItem:MonoBehaviour
{
    private void Awake()
    {
        
    }

    private MySmsOrCallVo FindCurShowData(List<MySmsOrCallVo> list)
    {
        MySmsOrCallVo temp = null;
        temp = list.Find((item) => { return item.IsReaded == false && item.selectIds.Count > 0; });
        if (temp != null)
        {
            return temp;
        }
        temp = list.Find((item) => { return item.IsReaded == false && item.IsPlayerTrigger == false; });
        if (temp != null)
        {
            return temp;
        }
        temp = list.Find((item) => { return item.IsReaded == false && item.IsPlayerTrigger == true; });
        if(temp!=null)
        {
            return temp;
        }
        return temp;
    }

    List<MySmsOrCallVo> _data;
    public void SetData(List<MySmsOrCallVo>  data)
    {
        MySmsOrCallVo smsVo = FindCurShowData(data);
        _data = data;

        if (smsVo==null)
        {
            data.Sort((a, b) => { return a.FinishTime.CompareTo(b.FinishTime); });
            smsVo = data[data.Count - 1];
            Debug.LogError(smsVo.SceneId);
            // transform.Find("MsgText").GetComponent<Text>().text = "无消息";
            transform.Find("MsgText").GetComponent<Text>().text = smsVo.CurTalkInfo.TalkContent;
            transform.Find("NumTips").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("NumTips").gameObject.SetActive(true);
            transform.Find("NumTips/Text").GetComponent<Text>().text = "1";
            transform.Find("MsgText").GetComponent<Text>().text  = I18NManager.Get("Phone_Sms_New");
 
        }

        transform.Find("NameText").GetComponent<Text>().text = smsVo.Sender;     
        transform.Find("LeftHeadIcon/Image").GetComponent<RawImage>().texture = ResourceManager.Load<Texture>(PhoneData.GetHeadPath(smsVo.NpcId), ModuleConfig.MODULE_PHONE);
         var date = new DateTime(smsVo.CreateTime);
        string dateStr = "";

        long hasPassedStamp = ClientTimer.Instance.GetCurrentTimeStamp() - smsVo.CreateTime;
        if(hasPassedStamp<0)
        {
            dateStr = I18NManager.Get("Phone_Tips1");
        }
        else if(hasPassedStamp<1000*60*60*24)
        {
           // long s = (smsVo.CreateTime / 1000) % 60;
            long m = (smsVo.CreateTime / (60 * 1000)) % 60;
            long h = (smsVo.CreateTime / (60 * 60 * 1000) + 8) % 24;
            // dateStr = string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);    
            dateStr = string.Format("{0:D2}:{1:D2}", h, m);
        }
        else
        {
            dateStr = I18NManager.Get("Phone_Tips2"); 
        }
        transform.Find("Text").GetComponent<Text>().text = dateStr;
        //transform.Find("Text").GetComponent<Text>().text = date.ToShortTimeString();

        PointerClickListener.Get(transform.gameObject).onClick = delegate (GameObject go)      
        {
            if (CacheManager.isGuideSmsBySceneId(smsVo.SceneId)) 
            {
                EventDispatcher.TriggerEvent(EventConst.PhoneSmsItemClick, data);
                return;
            }

            CacheManager.ClickItem(smsVo.NpcId,()=> {
                EventDispatcher.TriggerEvent(EventConst.PhoneSmsItemClick, data);
            },()=>{
                EventDispatcher.TriggerEvent(EventConst.PhoneSmsItemClick, data);
            }
            
            );
           // EventDispatcher.TriggerEvent(EventConst.PhoneSmsItemClick, data);
        };
    }



}