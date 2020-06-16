using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
using Assets.Scripts.Module;
using UnityEngine.Events;
using game.tools;
using game.main;
using Common;

public class DrawCardLimitUI : View
{
    private Button _btnChangeChara;
    private Button _keyBtn;


    private void Awake()
    {
        _btnChangeChara = transform.GetButton("BtnChangeChara");
        _keyBtn = transform.GetButton("Key/KeyBtn");

        _btnChangeChara.onClick.AddListener(OnBtnChangeCharaClick);
        _keyBtn.onClick.AddListener(OnBtnKeyBtn);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnBtnChangeCharaClick()
    {
        PopupManager.ShowWindow<DrawCardLimitCharaWindow>("DrawCard/Prefabs/DrawCardLimitCharaWindow");
    }

    private void OnBtnKeyBtn()
    {
        PopupManager.ShowWindow<DrawCardLimitKeyWindow>("DrawCard/Prefabs/DrawCardLimitKeyWindow");
    }
}
