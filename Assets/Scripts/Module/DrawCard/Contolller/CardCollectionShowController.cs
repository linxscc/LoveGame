using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using Module.Supporter.Data;
using UnityEngine;

namespace Module.CardCollectionShow.Controller
{

    public class CardCollectionShowController : Assets.Scripts.Framework.GalaSports.Core.Controller
    {
        //派发消息和执行view中的方法和data交互
        public CardCollectionShowView View;
        //public List<ShowCardModel> TempList;
        public DrawEventPB DrawEvent;

        public override void Start()
        {      
            Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
            //写数据更新
            //View.SetAllData(TempList);
           // NetWorkManager.Instance.Send<DrawProbRes>(CMD.DRAWC_DRAW_PROBS, null, OnGetCardList);
        }

        public override void OnMessage(Message message)
        {
            string name = message.Name;
            switch (name)
            {

            }
        }

        public void SetData(DrawPoolTypePB poolType, DrawEventPB drawEvent)
        {
            View.SetData(drawEvent, GetData<DrawData>().GetCardList(poolType));
        }

    }
}
