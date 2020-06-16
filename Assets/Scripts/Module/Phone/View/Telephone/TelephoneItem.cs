using System;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Download;
using Common;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class TelephoneItem:MonoBehaviour
{
    private void Awake()
    {
        
    }

    public void SetData(MySmsOrCallVo data)
    {
        transform.Find("NameText").GetComponent<Text>().text = data.Sender;
       //var date = new DateTime(data.CreateTime);
        string dateStr = "";
        long hasPassedStamp = ClientTimer.Instance.GetCurrentTimeStamp() - data.CreateTime;
        if (hasPassedStamp < 0)
        {
            dateStr = I18NManager.Get("Phone_Tips1");
        }
        else if (hasPassedStamp < 1000 * 60 * 60 * 24)
        {

           // long s = (data.CreateTime / 1000) % 60;
            long m = (data.CreateTime / (60 * 1000)) % 60;
            long h = (data.CreateTime / (60 * 60 * 1000) + 8) % 24;
            // dateStr = string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);    
            dateStr = string.Format("{0:D2}:{1:D2}", h, m);
        }
        else
        {
            dateStr = I18NManager.Get("Phone_Tips2");
        }

        // transform.Find("MsgText").GetComponent<Text>().text = date.ToShortTimeString();
   
        transform.Find("MsgText").GetComponent<Text>().text = dateStr;

        transform.Find("CallTips/Tips").gameObject.SetActive(!data.IsReaded);
        transform.Find("LeftHeadIcon/Image").GetComponent<RawImage>().texture= ResourceManager.Load<Texture>(PhoneData.GetHeadPath(data.NpcId), ModuleConfig.MODULE_PHONE);

        PointerClickListener.Get(transform.gameObject).onClick= delegate(GameObject go)
        {
            //EventDispatcher.TriggerEvent(EventConst.PhoneCallItemClick, data);

            CacheManager.ClickItem(data.NpcId, () =>
            {
                EventDispatcher.TriggerEvent(EventConst.PhoneCallItemClick, data);
            });
        };
    }
}