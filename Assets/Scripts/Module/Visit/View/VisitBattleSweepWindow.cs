using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using DataModel;
using Google.Protobuf.Collections;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class VisitBattleSweepWindow : Window
    {
        private Transform _content;
        private Dictionary<string, DrawActivityDropItemVo> _drawActivityDropItemDict;
        private int _total;

        private void Awake()
        {
            _content = transform.Find("Scroll View/Viewport/Content");

            for (int i = 0; i < 5; i++)
            {
                _content.GetChild(i).gameObject.Hide();
            }
        }

        bool _isVip;
        int levelId;
        RepeatedField<GameResultPB> _result;
        
        public void SetData(RepeatedField<GameResultPB> result, VisitLevelVo customerSelectedLevel, int exp)
        {
            _isVip = GlobalData.PlayerModel.PlayerVo.IsOnVip; 
            _result = result;
            levelId = customerSelectedLevel.LevelId;

            transform.Find("Title/Text").GetComponent<Text>().text = customerSelectedLevel.LevelName;
            
            _total = 0;
            _drawActivityDropItemDict = new Dictionary<string, DrawActivityDropItemVo>();
            
            for (int i = 0; i < result.Count; i++)
            {
                GameResultPB pb = result[i];
                
                Dictionary<int, RewardVo> rewardDict = new Dictionary<int, RewardVo>();
                for (int j = 0; j < pb.Awards.Count; j++)
                {
                    RewardVo vo = new RewardVo(pb.Awards[j], true);
                    if (rewardDict.ContainsKey(vo.Id))
                    {
                        rewardDict[vo.Id].Num += vo.Num;
                    }
                    else
                    {
                        rewardDict.Add(vo.Id, vo);
                    }
                }
                
                RectTransform listItem = _content.GetChild(i).GetComponent<RectTransform>();
                var isOnVip = GlobalData.PlayerModel.PlayerVo.IsOnVip;
                if (isOnVip)
                {
                    listItem.Find("Title/Text2").GetComponent<Text>().text = "+" + exp + I18NManager.Get("Common_VIPExp");   
                }
                else
                {
                    listItem.Find("Title/Text2").GetComponent<Text>().text = "+" + exp + I18NManager.Get("Common_Exp");   
                }
                
                listItem.gameObject.Show();
                List<RewardVo> arr = rewardDict.Values.ToList();
                var prop = arr[arr.Count - 1];
                if (prop.Resource == ResourcePB.Item && prop.Id >= PropConst.CardUpgradePropSmall && prop.Id <= PropConst.CardUpgradePropLarge)
                {
                    arr.Insert(1,prop);
                    arr.RemoveAt(arr.Count-1);
                }

               
                if (pb.DroppingItem != null && pb.DroppingItem.Count > 0)
                {
                    for (int k = 0; k< pb.DroppingItem.Count; k++)
                    {
                        DrawActivityDropItemVo vo = new DrawActivityDropItemVo(pb.DroppingItem[k], arr, HolidayModulePB.ActivityVisiting);
                        _drawActivityDropItemDict.Add(i+"_"+vo.DisplayIndex, vo);
                        _total = vo.TotalNum;
                    }
                }
                
                SetItemReward(listItem, arr, _drawActivityDropItemDict);

                float height = 440;
                if (arr.Count > 3)
                {
                    //2行的情况
                    height = 766;
                }
                float lineY = -height + 5;
                
                listItem.sizeDelta = new Vector2(listItem.sizeDelta.x, height);
                RectTransform linePos = listItem.Find("Line").GetComponent<RectTransform>();
                linePos.anchoredPosition = new Vector2(linePos.anchoredPosition.x, lineY);
            }
            
            //显示最终值
            foreach (var dropItemVo in _drawActivityDropItemDict)
            {
                dropItemVo.Value.TotalNum = _total;
            }
        }

        private void SetItemReward(RectTransform listItem, List<RewardVo> rewardList,
            Dictionary<string, DrawActivityDropItemVo> drawActivityDropItemDict)
        {
            for (int i = 0; i < 6; i++)
            {
                VisitSweepItem item = listItem.Find("PropContainer").GetChild(i).gameObject.AddComponent<VisitSweepItem>();
                if (i >= rewardList.Count)
                {
                    item.gameObject.Hide();
                }
                else
                {
                    DrawActivityDropItemVo extReward;
                    drawActivityDropItemDict.TryGetValue(listItem.GetSiblingIndex() + "_" + i, out extReward);
                    item.SetData(rewardList[i], extReward);
                }
            }
        }
        
//        public void SetData(RepeatedField<GameResultPB> result, VisitLevelVo customerSelectedLevel, int exp)
//        {
//
//            _isVip = GlobalData.PlayerModel.PlayerVo.IsOnVip; 
//            _result = result;
//            transform.Find("Title/Text").GetComponent<Text>().text = customerSelectedLevel.LevelName;
//            levelId = customerSelectedLevel.LevelId;
//            //_result1 = result;
//            Debug.Log(_result.Count);
//            _rect.totalCount = _result.Count;
//            _rect.RefreshCells();
//
//        }


        public override void Close()
        {
            base.Close();
            EventDispatcher.TriggerEvent<int>(EventConst.ShowLastVisitBattleWindow, levelId);
        }
    }
}