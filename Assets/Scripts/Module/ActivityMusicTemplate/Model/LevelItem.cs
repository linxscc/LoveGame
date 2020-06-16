using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Common;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    private Button _enterBtn;
    private Button _lockBtn;
    private CapsuleLevelVo _data;
    
    private void Awake()
    {
        _enterBtn = transform.GetButton("EnterBtn");
        _lockBtn =transform.GetButton("LockBtn");
        
        _enterBtn.onClick.AddListener(OnClickEnterBtn);
        _lockBtn.onClick.AddListener(OnClickLockBtn);
    }


    
    private void OnClickLockBtn()
    {
        FlowText.ShowMessage(I18NManager.Get("ActivityCapsuleTemplate_PleasePassLastLevel")); 
    }

    private void OnClickEnterBtn()
    {
       
       EventDispatcher.TriggerEvent(EventConst.OnClickMusicCapsuleBattleEntrance,_data);
    }

    public void SetData(CapsuleLevelVo vo,int id)
    {
        _data = vo;
        string level = "ActivityMusicTemplate/Level_" + id;
        string levelLock = "ActivityMusicTemplate/LevelLock_" + id;
        _enterBtn.transform.GetRawImage("Icon").texture = ResourceManager.Load<Texture>(level,ModuleConfig.MODULE_ACTIVITYMUSICTEMPLATE,true);
        _lockBtn.transform.GetRawImage("Icon").texture =ResourceManager.Load<Texture>(levelLock,ModuleConfig.MODULE_ACTIVITYMUSICTEMPLATE,true);
        
        if (vo.IsOpen)
        {
            _enterBtn.gameObject.SetActive(true);
            _lockBtn.gameObject.SetActive(false);
        }
        else
        {
           _enterBtn.gameObject.SetActive(false);
           _lockBtn.gameObject.SetActive(true); 
        }
    }
}
