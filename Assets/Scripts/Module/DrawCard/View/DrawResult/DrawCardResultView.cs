using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using game.main;
using game.tools;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Module.DrawCard
{
    public class DrawCardResultView : View
    {
        private LoopVerticalScrollRect _drawCardResultList;
        private LoopVerticalScrollRect _drawCardFunResultList;
        private List<DrawCardResultVo> _awardData;
        private List<DrawCardResultVo> _awardFunData;
        private RectTransform _funsTitle;
        private void Awake()
        {
            _funsTitle = transform.Find("FunsTitle").GetComponent<RectTransform>();

            _awardData = new List<DrawCardResultVo>();
            _awardFunData = new List<DrawCardResultVo>();
            _drawCardResultList = transform.Find("List").GetComponent<LoopVerticalScrollRect>();
            _drawCardResultList.prefabName = "DrawCard/Prefabs/DrawResultItem";
            _drawCardResultList.poolSize = 6;
            _drawCardResultList.totalCount = 0;

            _drawCardFunResultList = transform.Find("FunList").GetComponent<LoopVerticalScrollRect>();
            _drawCardFunResultList.prefabName = "DrawCard/Prefabs/DrawResultFunItem";
            _drawCardFunResultList.poolSize = 6;
            _drawCardFunResultList.totalCount = 0;

            //PointerClickListener.Get(gameObject).onClick = go =>
            //{
            //    SendMessage(new Message(MessageConst.MODULE_VIEW_BACK_DRAWCARD));
            //};
        }

        public void SetData(List<DrawCardResultVo> data)
        {
            foreach (var v in data)
            {
                Debug.Log("CardId " + v.CardId + " Name " + v.Name + "Credit " + v.Credit);
            }

            foreach (var v in data)
            {
                if (v.Resource == ResourcePB.Fans)
                {
                    _awardFunData.Add(v);
                }
                else
                {
                    _awardData.Add(v);
                }
            }

            _drawCardResultList.UpdateCallback = ResultListUpdateCallback;
            _drawCardResultList.totalCount = _awardData.Count;
            _drawCardResultList.RefreshCells();

            if (_awardFunData.Count > 0)
            {
                float a = _awardData.Count;
                float offset = Mathf.Ceil(a / 3);
                float posY = _drawCardFunResultList.gameObject.GetComponent<RectTransform>().localPosition.y - offset * 250;
                _funsTitle.localPosition = new Vector3(
                    _funsTitle.localPosition.x,
                    _funsTitle.localPosition.y - offset * 250,
                    _funsTitle.localPosition.z
                    );

                _drawCardFunResultList.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(
                    _drawCardFunResultList.gameObject.GetComponent<RectTransform>().localPosition.x,
                    _drawCardFunResultList.gameObject.GetComponent<RectTransform>().localPosition.y - offset * 250,
                    _drawCardFunResultList.gameObject.GetComponent<RectTransform>().localPosition.z
                    );

                _drawCardFunResultList.UpdateCallback = ResultFunListUpdateCallback;
                _drawCardFunResultList.totalCount = _awardFunData.Count;
                _drawCardFunResultList.RefreshCells();
            }
            else
            {
                _drawCardFunResultList.gameObject.SetActive(false);
                _funsTitle.gameObject.SetActive(false);
            }

        }
        private void ResultListUpdateCallback(GameObject go, int index)
        {
             go.GetComponent<DrawCardResultItem>().SetData(_awardData[index]);
        }
        private void ResultFunListUpdateCallback(GameObject go, int index)
        {
              go.GetComponent<DrawCardResultFunItem>().SetData(_awardFunData[index]);
        }

    }
}