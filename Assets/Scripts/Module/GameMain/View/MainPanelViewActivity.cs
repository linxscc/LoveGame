
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using Componets;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public partial class MainPanelView
{
    public void RefreshActivityImageAndActivityPage()
    {
        _activityPanel.GetComponent<LoopCarouselImage>().RefreshLoop();
      //  CreateActivityImageAndActivityPage(GlobalData.ActivityModel.GetActivityList());
        CreateActivityImageAndActivityPage(GlobalData.ActivityModel. GetActivityVoList());
       
    }

    public void CreateActivityImageAndActivityPage(List<ActivityVo> activityList)
    {
       // Debug.LogError("生成");
        var activityImagePrefab = GetPrefab("GameMain/Prefabs/ActivityImage");
        var activityPagePrefab = GetPrefab("GameMain/Prefabs/ActivityPage");

        for (int i = 0; i < activityList.Count; i++)
        {
            var activityImageItem = Instantiate(activityImagePrefab, _activityPanel, false) as GameObject;
            activityImageItem.transform.localScale = Vector3.one;
            
//            int activityType = (int) activityList[i].Type;            
//            activityImageItem.transform.Find("Mask/Image").GetComponent<Image>().sprite =
//                ResourceManager.Load<Sprite>("GameMain/GameMainBar"+activityType);
//            
//            activityImageItem.name = activityType.ToString();
//            PointerClickListener.Get(activityImageItem).onClick = ShowActivityWindow;

            var activity = activityList[i];
            activityImageItem.transform.Find("Mask/Image").GetComponent<Image>().sprite = ResourceManager.Load<Sprite>(activity.GameMainBarPath);
            activityImageItem.name = activity.JumpId;
            PointerClickListener.Get(activityImageItem).onClick = go =>
            {
                SendMessage(new Message(MessageConst.CMD_MAIN_ON_ACTIVITY_BTN, activity.JumpId));
            };

            var rect = activityImageItem.GetComponent<RectTransform>();
            var w = rect.GetWidth();
            var localPosition = rect.localPosition;
            localPosition = new Vector3(localPosition.x + w * i, localPosition.y, 0);
            rect.localPosition = localPosition;


            var activityPageItem = Instantiate(activityPagePrefab, _activityPage, false) as GameObject;
           // activityPageItem.name =activityType.ToString();      
           activityPageItem.name = activity.Name;
            activityPageItem.transform.localScale = Vector3.one;

            activityPageItem.GetComponent<Toggle>().group = _activityPage.GetComponent<ToggleGroup>();
        }

        _activityPanel.GetComponent<LoopCarouselImage>().AddChildOver = true;
    }
}