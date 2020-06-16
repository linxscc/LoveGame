using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Recollection.Data;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;



using game.tools;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;

namespace Assets.Scripts.Module.Recollection.View
{
    public class CardDropPropWindow : Window
    {
        private LoopVerticalScrollRect _cardList;
        private List<RecollectionCardDropVo> _cards;
        private RawImage _propImage;
        private Text _num;
        private void Awake()
        {
            _cardList = transform.Find("List").GetComponent<LoopVerticalScrollRect>();

            
          
            //_propImage = transform.Find("PropImage").GetComponent<RawImage>();
            _propImage = transform.Find("FrameImage/PropImage").GetComponent<RawImage>();
           _num= transform.Find("FrameImage/PropImage/Text").GetComponent<Text>();
        }

        private void ListUpdateCallback(GameObject go, int index)
        {
           
            go.GetComponent<CardPropItem>().SetData(_cards[index]);
        }

        public void SetData(List<RecollectionCardDropVo> cards, int propId)
        {
            //  MaskAlpha = 0.5f;
            MaskColor = new Color(0, 0, 0, 0.6f);
            _cards = cards;


            _cardList.prefabName = "Recollection/Prefabs/CardPropItem";
            _cardList.poolSize = 6;
            _cardList.UpdateCallback = ListUpdateCallback;

            _cardList.totalCount = _cards.Count;
            _cardList.RefreshCells();

            _propImage.texture = ResourceManager.Load<Texture>("Prop/" + propId);
           
            _num.text = I18NManager.Get("Recollection_CardDropPropWindowHaveNum", GlobalData.PropModel.GetUserProp(propId).Num);
            PointerClickListener.Get(_propImage.gameObject).onClick = go =>
            {
                FlowText.ShowMessage(PropConst.GetTips(propId));
            };

        }

     
       


      
    }
}