using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using Common;
using DataModel;
using Module.Supporter.Data;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class SupporterView : View
    {
        //private int _power = 0;
        private int _funsPower = 0;
        private int _itemPower = 0;
        private Dictionary<int, int> _fansDict;
        private bool _moreGoods;
        private bool _moreFans;

        public void UpdatePowerData()
        {
            SumGoodsPower();
            transform.Find("Container/SupporterNum").GetComponent<Text>().text =
                I18NManager.Get("Battle_SupportHeatAdd", _funsPower + _itemPower);
        }

        private Queue<string> _fansInfo;

        private void Awake()
        {
            transform.Find("Container/OkBtn").GetComponent<Button>().onClick.AddListener(NextStep);
        }

        public void NextStep()
        {
            SumGoodsPower();
            int power = _funsPower + _itemPower;
            SendMessage(new Message(MessageConst.CMD_BATTLE_NEXT, Message.MessageReciverType.DEFAULT, _fansInfo, power)); 
        }


        public void SetFansData(LevelVo data, List<FansVo> list)
        {
            _funsPower = 0;
            transform.Find("Container/DescBg/FansText").GetComponent<Text>().text = Util.GetNoBreakingString(data.FansDescription);

            var fansPref = GetPrefab("Battle/Prefabs/FansItem");
            var fansContainer = transform.Find("Container/FansContainer");

            int useTotal = 0;
            int maxTotal = 0;
            
            _fansDict = new Dictionary<int, int>();
            
            _fansInfo =new Queue<string>();
            
            foreach (var item in data.FansMax)
            {
                var itemObj = Instantiate(fansPref);
                if (itemObj == null) continue;
                
              
                
                itemObj.transform.SetParent(fansContainer, false);
                
                var itemData = list.Find((vo) => { return vo.FansId == item.Key; });
                var num = itemData.Num > item.Value ? item.Value : itemData.Num;
                itemObj.AddComponent<BattleFansItem>().SetData(num, item.Value, itemData);
                _funsPower += num*itemData.Power;
                _fansDict.Add(itemData.FansId, num);
                
                _fansInfo.Enqueue(itemData.FansBigTexturePath);   //获取粉丝图片路径，入队
                
                useTotal += num;
                maxTotal += item.Value;
            }
     
            _moreFans = (float) useTotal / maxTotal >= 0.5;
        }

        public void SetGoodsData(LevelVo data)
        {
            var itemPref = GetPrefab("Battle/Prefabs/SupportItem");
            var supportContainer = transform.Find("Container/SupportContainer");
            var index = 0;

            
            
            foreach (var item in data.ItemMax)
            {
                var itemObj = Instantiate(itemPref) as GameObject;
                if (itemObj == null) continue;
                itemObj.transform.SetParent(supportContainer, false);

                UserPropVo propVo = GlobalData.PropModel.GetUserProp(item.Key);

                ItemPB itemPb = GlobalData.PropModel.GetPropBase(item.Key);

                int useNum = propVo.Num > item.Value ? item.Value : propVo.Num;
                
                var supportItem = itemObj.AddComponent<SupportItem>();
                string iconPath = GlobalData.PropModel.GetPropPath(item.Key);;
                supportItem.SetData(itemPb, useNum,item.Value, iconPath);
                supportItem.GetComponent<ItemShowEffect>().OnShowEffect(index * 0.2f);

                index++;
            }
            SumGoodsPower();
        }

        private void SumGoodsPower()
        {
            Dictionary<int, int> itemDict = new Dictionary<int, int>();
            _itemPower = 0;
            int useNum = 0;
            int maxNum = 0;
            var supportContainer = transform.Find("Container/SupportContainer");
            for (int i = 0; i < supportContainer.childCount; i++)
            {
                SupportItem item = supportContainer.GetChild(i).GetComponent<SupportItem>();
                _itemPower += item.TotalPower;
                itemDict.Add(item.ItemId, item.UseNum);
                useNum += item.UseNum;
                maxNum += item.MaxNum;
            }

            _moreGoods = (float) useNum / maxNum >= 0.5f;
            
            SendMessage(new Message(MessageConst.CMD_BATTLE_ITEM_DATA, Message.MessageReciverType.MODEL, itemDict, _fansDict));
        }
    }
}