using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SleepChooseView : View
{
    private Transform[] _roleBtns;

    private void Awake()
    {
        int len = 4;
        _roleBtns = new Transform[len];
        for(int i = 0;i < len; ++i)
        {
            _roleBtns[i] = transform.Find("Viewport/Content/Role" + (i + 1));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetData();
    }
    
    private void OnBtnRoleClick(GameObject go)
    {
        int id = (int)PointerClickListener.Get(go).parameter;
        Debug.Log("OnBtnRoleClick:"+id);
        SendMessage(new Message(MessageConst.MODULE_SLEEP_CHOOSE_VIEW_BTN_ROLE, id));
    }

    public void SetData()
    {
        int[] ids = {
            PropConst.CardEvolutionPropChi, PropConst.CardEvolutionPropQin, PropConst.CardEvolutionPropTang,
            PropConst.CardEvolutionPropYan
        };
        for(int i = 0;i < ids.Length; ++i)
        {
            Transform roleBtn = _roleBtns[i];
            PointerClickListener.Get(roleBtn.gameObject).parameter = ids[i];//appointmentRuleVos[i];
            PointerClickListener.Get(roleBtn.gameObject).onClick = OnBtnRoleClick;
        }
    }
}
