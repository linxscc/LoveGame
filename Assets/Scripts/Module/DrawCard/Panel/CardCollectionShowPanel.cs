using System.Security.Cryptography;
using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Com.Proto;
using Google.Protobuf.Collections;
using UnityEngine;
using Module.CardCollectionShow.Controller;
using System;

public class CardCollectionShowPanel : ReturnablePanel
{
    private CardCollectionShowController _control;

    public override void Init(IModule module)
    {
        base.Init(module);
        IView viewScript = InstantiateView<CardCollectionShowView>("DrawCard/Prefabs/ShowCardView");
        _control = new CardCollectionShowController();
        _control.View = (CardCollectionShowView) viewScript;
        _control.Start();

        RegisterController(_control);
        //RegisterModel<CardShowData>();
    }

    public override void OnBackClick()
    {
        _control.View.BackDrawCard();
    }

    public void SetData(DrawPoolTypePB poolType, DrawEventPB drawEvent)
    {
        base.Show(0);
        ShowBackBtn();
        _control.SetData(poolType, drawEvent);
    }
}