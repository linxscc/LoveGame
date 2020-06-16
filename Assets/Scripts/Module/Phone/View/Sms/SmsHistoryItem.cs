using System;
using System.Net.Mime;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class SmsHistoryItem : MonoBehaviour
{
    public MySmsOrCallVo Data;
    public void SetData(MySmsOrCallVo data)
    {
        Data = data;
        transform.Find("Text").GetComponent<Text>().text = data.SmsRuleInfo.smsSceneInfo.SceneName;
    }
}