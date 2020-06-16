using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using Module.Supporter.Data;
using UnityEngine;
using UnityEngine.UI;

public  class CapsuleSupporterView : View
{
    private Text _descTxt; 
    private Transform _fansCont;
    private Transform _supportItemCont;
    private Button _okBtn;

    //private int _power = 0; //粉丝提供的能力
    private Dictionary<int, int> _fansDict;      
    private Queue<string> _fansInfo;
    private int _funsPower = 0;
    private int _itemPower = 0;
    private void Awake()
    {
        _descTxt = transform.GetText("Container/DescBg/FansText");
        _fansCont = transform.Find("Container/FansContainer");
        _supportItemCont = transform.Find("Container/SupportContainer");
        _okBtn = transform.GetButton("Container/OkBtn");
        _okBtn.onClick.AddListener(NextStep);
    }

    public void UpdatePowerData()
    {
        SumGoodsPower();
        transform.Find("Container/SupporterNum").GetComponent<Text>().text =
            I18NManager.Get("Battle_SupportHeatAdd", _funsPower + _itemPower);
    }
    
    private void NextStep()
    {
         SumGoodsPower();
         int power = _funsPower + _itemPower;
         SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_NEXT, Message.MessageReciverType.DEFAULT, _fansInfo,power)); 
    }

   


    public void SetData(CapsuleLevelVo data, List<FansVo> list)
    {
        SetFansData(data,list);
        SetGoodsData(data);
    }

    private void SetFansData(CapsuleLevelVo data, List<FansVo> list)
    {
        _funsPower = 0;
        _descTxt.text =Util.GetNoBreakingString(data.FansDescription);        
              
        int useTotal = 0;
        int maxTotal = 0;
        _fansDict = new Dictionary<int, int>();
        _fansInfo =new Queue<string>();
        
        var prefab =GetPrefab("ActivityCapsuleBattle/Prefabs/FansItem");

        foreach (var item in data.FansMax)
        {
           var itemObj= Instantiate(prefab,_fansCont,false);
           var itemData = list.Find((vo) => vo.FansId == item.Key);
           var num = itemData.Num > item.Value ? item.Value : itemData.Num;
           itemObj.AddComponent<CapsuleBattleFansItem>().SetData(num, item.Value, itemData);
           _funsPower += num*itemData.Power;
           _fansDict.Add(itemData.FansId, num);
           _fansInfo.Enqueue(itemData.FansBigTexturePath);   //获取粉丝图片路径，入队
           useTotal += num;
           maxTotal += item.Value;
        }
        
    }

    private void SetGoodsData(CapsuleLevelVo data)
    {
        var prefab = GetPrefab("ActivityCapsuleBattle/Prefabs/CapsuleSupportItem");
        var index = 0;
        foreach (var item in data.ItemMax)
        {
            var itemObj = Instantiate(prefab, _supportItemCont, false);
            UserPropVo propVo = GlobalData.PropModel.GetUserProp(item.Key);
            ItemPB itemPb = GlobalData.PropModel.GetPropBase(item.Key);
            int useNum = propVo.Num > item.Value ? item.Value : propVo.Num;
            var capsuleSupportItem = itemObj.AddComponent<CapsuleSupportItem>();
            string iconPath = GlobalData.PropModel.GetPropPath(item.Key);
            capsuleSupportItem.SetData(itemPb, useNum,item.Value, iconPath);
            capsuleSupportItem.GetComponent<ItemShowEffect>().OnShowEffect(index * 0.2f);
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
        for (int i = 0; i < _supportItemCont.childCount; i++)
        {
            var item =_supportItemCont.GetChild(i).GetComponent<CapsuleSupportItem>();
            _itemPower += item.TotalPower;
            itemDict.Add(item.ItemId, item.UseNum);
            useNum += item.UseNum;
            maxNum += item.MaxNum;
        }
        SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_ITEM_DATA, Message.MessageReciverType.MODEL, itemDict, _fansDict));
    }
    
}
