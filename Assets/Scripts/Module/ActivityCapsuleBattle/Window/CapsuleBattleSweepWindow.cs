using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleBattleSweepWindow : Window
{
    private Transform _content;
    private Dictionary<string, DrawActivityDropItemVo> _drawActivityDropItemDict;
    private int _total;

    private void Awake()
    {
        _content = transform.Find("Scroll View/Viewport/Content");
        HideContentChild();
    }


    private void HideContentChild()
    {
        for (int i = 0; i < 10; i++)
        {
            _content.GetChild(i).gameObject.Hide();
        }
    }

    public void SetData(RepeatedField<GameResultPB> result, CapsuleLevelVo customerSelectedLevel, int exp)
    {
        transform.Find("Title/Text").GetComponent<Text>().text = customerSelectedLevel.LevelName;
        _total = 0;
        _drawActivityDropItemDict = new Dictionary<string, DrawActivityDropItemVo>();

        for (int i = 0; i <  result.Count; i++)
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
                    DrawActivityDropItemVo vo = new DrawActivityDropItemVo(pb.DroppingItem[k], arr, HolidayModulePB.ActivityCareer);
                    _drawActivityDropItemDict.Add(i+"_"+vo.DisplayIndex, vo);
                    if(vo.TotalNum > _total)
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
            SweepItem item = listItem.Find("PropContainer").GetChild(i).gameObject.AddComponent<SweepItem>();
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


    public override void Close()
    {
        base.Close();       
        EventDispatcher.TriggerEvent(EventConst.ShowLastCapsuleBattleWindow);
    }
}
