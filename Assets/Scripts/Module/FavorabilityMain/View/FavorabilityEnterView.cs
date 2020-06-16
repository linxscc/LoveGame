using System;
using System.Collections.Generic;

using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts.Framework.GalaSports.Service;
using DG.Tweening;

/// <summary>
/// 好感度选择角色View
/// </summary>
public class FavorabilityEnterView : Window
{
    private Transform _btnGroup;
    
    private void Awake()
    {
        _btnGroup = transform.Find("BtnGroup");
    }

    public void SetView()
    {
        for (int i = 0; i < _btnGroup.childCount; i++)
        {
            Transform roleStory = _btnGroup.GetChild(i);
            Text nameText = roleStory.Find("Text").GetComponent<Text>();    
            var roleId = i + 1;          
            var vo = GlobalData.FavorabilityMainModel.GetUserFavorabilityVo(roleId);           
            nameText.text = GlobalData.FavorabilityMainModel.GetPlayerName(vo.Player);    
            PointerClickListener.Get(_btnGroup.GetChild(i).gameObject).onClick = go =>       
            {                
                SendMessage(new Message(MessageConst.CMD_FACORABLILITY_COMETOMAIN, Message.MessageReciverType.DEFAULT, vo));                
            };

        } 
    }


    



}
