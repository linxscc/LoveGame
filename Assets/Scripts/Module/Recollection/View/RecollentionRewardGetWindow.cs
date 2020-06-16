using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using GalaAccountSystem;
using Google.Protobuf.Collections;
using QFramework;
using UnityEngine;

public class RecollentionRewardGetWindow : Window
{

    private Transform _content;

    private void Awake()
    {
        _content = transform.Find("ScrollView/Viewport/Content");
        ClientData.LoadSpecialItemDescData(null);
        ClientData.LoadItemDescData(null);
    }


    public void SetData(RepeatedField<AwardPB> awardPbs,int playerNum)
    {
        var prefab1 = GetPrefab("Recollection/Prefabs/RecollentionItem1");
        var prefab2 = GetPrefab("Recollection/Prefabs/RecollentionItem2");


        for (int i = 0; i < awardPbs.Count; i++)
        {
            var item1 = Instantiate(prefab1, _content, false);
            var parent = item1.transform.Find("PropContainer");
            var common = item1.transform.Find("Title/common");
            common.GetText("Text").text = I18NManager.Get("Recollection_commonAward",i+1);
            common.gameObject.Show();
            var item2 =  Instantiate(prefab2, parent, false);
            
            RewardVo  vo =new RewardVo(awardPbs[i]);
            item2.transform.GetText("ObtainText").text = I18NManager.Get("Recollection_RecollentionRewardNum",vo.Num);
            item2.transform.GetText("PropNameTxt").text = vo.Name;
            item2.transform.GetRawImage("PropImage").texture = ResourceManager.Load<Texture>(vo.IconPath);


            PointerClickListener.Get(item2.gameObject).onClick = go =>
            {
                var desc = ClientData.GetItemDescById(vo.Id,vo.Resource);
                if (desc!=null)
                {
                    FlowText.ShowMessage(desc.ItemDesc); 			
                }
            };
        }
        
        if (awardPbs.Count> playerNum)
        {
            
            _content.GetChild(_content.childCount-1).Find("Title/Extra").gameObject.Show();
            _content.GetChild(_content.childCount-1).Find("Title/common").gameObject.Hide();
        }
//        foreach (var pb in awardPbs)
//        {
//            var item1 = Instantiate(prefab1, _content, false);
//            var parent = item1.transform.Find("PropContainer");
//            var item2 =  Instantiate(prefab2, parent, false);
//            RewardVo  vo =new RewardVo(pb);
//            
//            item2.transform.GetText("ObtainText").text = I18NManager.Get("Recollection_RecollentionRewardNum",vo.Num);
//            item2.transform.GetText("PropNameTxt").text = vo.Name;
//            item2.transform.GetRawImage("PropImage").texture = ResourceManager.Load<Texture>(vo.IconPath);
//            
//        }
    }


    protected override void OnClickOutside(GameObject go)
    {
        if (GuideManager.GetRemoteGuideStep(GuideTypePB.CardMemoriesGuide)<1030)
        {
            EventDispatcher.TriggerEvent(EventConst.RecollentionRewardGetWindowClose);   
        }
        
        base.OnClickOutside(go);      
    }
}
