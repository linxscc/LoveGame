using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using DataModel;
using game.main;
using GalaAccount.Scripts.Framework.Utils;
using UnityEngine;
using UnityEngine.UI;

public class CoaxSleepPlayerAudioView : View
{
    private Button _aniBtn;
    private Button _ruleBtn;
    private Transform _parent;


    private Text _curLoveUnlockNum;

    private string _npcName;
    private int _unlockLoveNum;

    private Transform _playerIcon;
    
    private void Awake()
    {
        _aniBtn = transform.GetButton("AniBtn");    
        _parent =transform.Find("ScrollRect/Content");
                        
        _playerIcon =transform.Find("PlayerIcon");
        _curLoveUnlockNum = _playerIcon.GetText("Unlock/Text");
        _ruleBtn = _playerIcon.GetButton("Unlock/RuleBtn");
        
        _aniBtn.onClick.AddListener(OnAniBtn); 
        _ruleBtn.onClick.AddListener(OnPlayerBtn);
    }

    private void OnPlayerBtn()
    {
        string content = I18NManager.Get("CoaxSleep_GoToLove",_npcName,_unlockLoveNum);      
        string okTxt = I18NManager.Get("CoaxSleep_GoToLoveOkTxt");
        
        var window = PopupManager.ShowConfirmWindow(content, null, okTxt);
        window.WindowActionCallback = evt =>
        {
            if (evt== WindowEvent.Ok)
            {
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_LOVEAPPOINTMENT,false, true);
            }           
        }; 
        
        
    }

    private void OnAniBtn()
    {
       SendMessage(new Message(MessageConst.CMD_COAXSLEEP_OPEN_ANI,OpenCoaxSleepAniType.PlayerViewOnClick));   
    }


    public void SetData(List<MyCoaxSleepAudioData> data)
    {           
        var curNum =GlobalData.AppointmentData.GetAppointmentUnlockNum(data[0].PlayerPb);
        _curLoveUnlockNum.text = I18NManager.Get("CoaxSleep_CurLoveUnlockNum",curNum);
       
        CreateAudioItem(data);
        
        _npcName = data[0].NpcName;
        _unlockLoveNum = curNum;
        SetPlayerIconShow(data[0].PlayerPb);
    }

    private void SetPlayerIconShow(PlayerPB playerPb)
    {
        var playerId = (int) playerPb;
        _playerIcon.Find(playerId.ToString()).gameObject.Show();
    }
    
    public void UpdateItemData(List<MyCoaxSleepAudioData> data)
    {
        foreach (var t in data)
        {
            var go = GetItem(t.AudioId.ToString());
            go.GetComponent<CoaxSleepAudioItem>().SetData(t);
        }
    }


    private GameObject GetItem(string id)
    {
        GameObject go = null;
        for (int i = 0; i < _parent.childCount; i++)
        {
            var item = _parent.GetChild(i).gameObject;
            if (id==item.name)
            {
                go = item;
                break;
            }
        }

        return go;
    }
    
    private void CreateAudioItem(List<MyCoaxSleepAudioData> data)
    {
        var prefab = GetPrefab("CoaxSleep/Prefabs/CoaxSleepAudioItem");

        for (int i = 0; i < data.Count; i++)
        {
            var go = Instantiate(prefab, _parent, false);
            go.transform.SetSiblingIndex(i);
            go.name = data[i].AudioId.ToString();
            go.GetComponent<CoaxSleepAudioItem>().SetData(data[i]);
        }
        
       _parent.GetChild(_parent.childCount-1).gameObject.Show();
    }
}
