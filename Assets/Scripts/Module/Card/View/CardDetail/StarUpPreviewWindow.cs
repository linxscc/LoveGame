using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using DataModel;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace game.main
{
    public class StarUpPreviewWindow : Window
    {
        private Text _title;
        private Text _tips;
        private Transform _propDropContent;
        private LoopVerticalScrollRect _propDropList;
        Dictionary<int, UpgradeStarRequireVo> upgradeStarRequireVos;
        private List<UpgradeStarRequireVo> _starUpList;
        private LevelModel _levelModel;
        private int _cardId;


        private void Awake()
        {
            _title = transform.GetText("Title/Text");
            _tips = transform.GetText("Tips");
            _propDropContent = transform.Find("PropDropGroup");
            _propDropList = transform.Find("PropDropGroup/ProDropList").GetComponent<LoopVerticalScrollRect>();
            _propDropList.prefabName = "Card/Prefabs/CardDetail/DropPropItem";
            _propDropList.poolSize = 2;
            _propDropList.UpdateCallback = UpdatePropDropInfo;
            upgradeStarRequireVos = new Dictionary<int, UpgradeStarRequireVo>();
            _starUpList=new List<UpgradeStarRequireVo>();
        }

        private void UpdatePropDropInfo(GameObject go, int index)
        {
            go.GetComponent<DropPropItem>().SetData(_starUpList[index],GlobalData.CardModel,_levelModel,_cardId, () =>
            {
                Close();
            });
        }

        public void SetData(UserCardVo userCardVo,LevelModel levelmodel)
        {
            _levelModel = levelmodel;
            _cardId = userCardVo.CardId;
            CardStarUpRulePB rule = GlobalData.CardModel.GetCardStarUpRule(userCardVo.CardId, (StarPB) userCardVo.Star);
            if (rule != null)
            {
                foreach (KeyValuePair<int, int> pair in rule.Consume)
                {
                    //需要刷选出不重复的list
                    if (!upgradeStarRequireVos.ContainsKey(pair.Key))
                    {
                        UpgradeStarRequireVo vo = new UpgradeStarRequireVo();
                        vo.PropId = pair.Key;
                        vo.NeedNum = pair.Value;
                        vo.PropName = GlobalData.PropModel.GetPropBase(vo.PropId).Name;
                        UserPropVo userProp = GlobalData.PropModel.GetUserProp(vo.PropId);
                        vo.CurrentNum = 0;
                        if (userProp != null)
                            vo.CurrentNum = userProp.Num;
                        upgradeStarRequireVos.Add(vo.PropId, vo);
                    }
                }
            }


//            for (int i = 0; i < userCardVo.CardVo.MaxStar; i++)
//            {
//
//            }

            foreach (var v in upgradeStarRequireVos)
            {
                _starUpList.Add(v.Value);
            }
            
            

            SetPropDropList();
            upgradeStarRequireVos.Clear();
        }

        private void SetPropDropList()
        {
            _propDropList.RefillCells();
            _propDropList.totalCount = _starUpList.Count;
            _propDropList.RefreshCells();
        }
    }
}