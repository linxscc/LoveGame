using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Componets;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module.Framework.Utils;
using Assets.Scripts.Module.Supporter.Data;
using Com.Proto;
using Common;
using DataModel;
using game.tools;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace game.main
{
    public class BuyGemView : View
    {

        private LoopVerticalScrollRect _gemPageList;
        private Text _bottomTips;
        private List<UserBuyRmbMallVo> _gemItemList;
        private Button _tasteCard;
        private Button _buyMonthCard;


        private string GoodPath = "Shop/Prefab/GoodsItem/GoodsItem";
        private TimerHandler _handler;
        private BuyGemModel _shopModel;
        private int _costGem = 0;
        private Button bakcbtn;
        private GameObject _gameObject;
        private bool _record;
    
        private RectTransform _buggemcontentbg;
        private float _bgHeight = 428f;

        private void Awake()
        {
            _gemPageList = transform.Find("ContentList/GemItemList").GetComponent<LoopVerticalScrollRect>();
            _buggemcontentbg = transform.Find("ContentList/GemItemList/ContentBg").GetRectTransform();
            bakcbtn = transform.Find("BackBtn").GetButton();
            bakcbtn.onClick.AddListener(() =>
            {
                ModuleManager.Instance.GoBack();
 
            });
            _bottomTips = transform.Find("BottomTips/Text").GetComponent<Text>();       
            _gemPageList.prefabName = GoodPath;
            _gemPageList.poolSize = 8;
            _tasteCard = transform.Find("ShouPic/TasteCard").GetButton();
            _buyMonthCard = transform.Find("ShouPic/BuyMonthCard").GetButton();
            _tasteCard.onClick.AddListener(() =>
            {
                SendMessage(new Message(MessageConst.CMD_USETASTECARD));
            });
            
            _buyMonthCard.onClick.AddListener(() =>
            {
                SendMessage(new Message(MessageConst.CMD_BUYMONTHCARD));
            });

            
        }


        public void SetData(BuyGemModel shopModel)
        {
            _shopModel = shopModel;
            _bottomTips.text = shopModel.BuyGemDesc;
            _gemItemList=shopModel.GetBuyGemRmbMallList;
            SetGemMallPage(_gemItemList);

        }
    
 
        public void SetGemMallPage(List<UserBuyRmbMallVo> gemItemVos)
        {
        //    Debug.LogError(gemItemVos.Count);

            _gemPageList.RefillCells();
            _gemPageList.UpdateCallback = GemPageListCallBack;
            _gemPageList.totalCount = gemItemVos.Count;
            _gemPageList.RefreshCells(); 
            _buggemcontentbg.anchoredPosition =  Vector2.zero;
            var scroll = _gemPageList.transform.GetComponent<ScrollRect>();
            if ( gemItemVos.Count<=12)
            {
                _buggemcontentbg.SetHeight(1712);
                scroll.enabled = false;
                _gemPageList.movementType = LoopScrollRect.MovementType.Clamped;
            }
            else if(gemItemVos.Count>13&&gemItemVos.Count<=16)
            {
                _buggemcontentbg.SetHeight(_bgHeight*((float)Math.Ceiling(gemItemVos.Count*0.25f)));    
                _gemPageList.movementType = LoopScrollRect.MovementType.Clamped;
                scroll.enabled = true;
                scroll.movementType=ScrollRect.MovementType.Clamped;
            }

            else
            {
                _buggemcontentbg.SetHeight(_bgHeight*((float)Math.Ceiling(gemItemVos.Count*0.25f)));                
                _gemPageList.movementType = LoopScrollRect.MovementType.Elastic;
                scroll.enabled = true;

            }         
        }

        private void GemPageListCallBack(GameObject go, int index)
        {   
            //go.GetComponent<GoodsItem>().SetData(_shopModel.RmbMallDic[_gemItemList[index].MallId],_gemItemList[index],_shopModel);   
        }


    }  

}

