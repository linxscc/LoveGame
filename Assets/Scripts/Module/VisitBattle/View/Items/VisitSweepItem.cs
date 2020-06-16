﻿using System.Collections;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using DataModel;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class VisitSweepItem : MonoBehaviour
    {
        private Frame _bigFrame;
        private Text _propNameTxt;
        private Text _obtainText;
        private Text _drawActivityText;
        private DrawActivityDropItemVo _extReward;
        private Transform _drawActivity;

        void Awake()
        {
            _bigFrame = transform.Find("BigFrame").GetComponent<Frame>();
            _propNameTxt = transform.Find("PropNameTxt").GetComponent<Text>();
            _obtainText = transform.Find("ObtainText").GetComponent<Text>();
            _drawActivityText = transform.Find("DrawActivityItem/Text").GetComponent<Text>();
            _drawActivity = transform.Find("DrawActivityItem");

            _propNameTxt.text = "";
            _obtainText.text = "";
        }
        
        public void SetData(RewardVo vo, DrawActivityDropItemVo extReward)
        {
            _extReward = extReward;

            _bigFrame.SetData(vo, ModuleConfig.MODULE_VISIT);

            if (extReward == null)
            {
                _drawActivity.gameObject.Hide();
                
                if(vo.Resource==ResourcePB.Puzzle)
                {
                    var card = GlobalData.CardModel.GetCardBase(vo.Id);
                    _propNameTxt.text = card.CardName;
                }else
                {
                    _propNameTxt.text = vo.Name;
                }
            }
            else
            {
                _propNameTxt.text = vo.Name;
                
                StartCoroutine(ResetText());
            }
            
            _obtainText.text = I18NManager.Get("Recollection_GetNum", vo.Num);
        }
        
        private IEnumerator ResetText()
        {
            yield return null;
            _drawActivityText.text = I18NManager.Get("ActivityTemplate_TodayLimit", 
                _extReward.TotalNum, _extReward.LimitNum);
        }
    }
}