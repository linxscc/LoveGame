using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActivityView : View
{ 
    private Transform _tabBar;
    private RectTransform _tabScrollView;

    private RectTransform _viewPort;
    Vector3[] _viewPortV3 =new Vector3[4];
    
    Vector2 _viewPortLeft = new Vector2();
    Vector2 _viewPortRight = new Vector2();
    private void Awake()
    {
        _viewPort = transform.GetRectTransform("TabScrollView/Viewport");
        _viewPort.GetWorldCorners(_viewPortV3);
        
        Vector2 temp1 = new Vector2(_viewPortV3[0].x,_viewPortV3[0].y);    
        _viewPortLeft = Camera.main.WorldToScreenPoint(temp1);        
             
        Vector2 temp2=new Vector2(_viewPortV3[3].x,_viewPortV3[3].y);       
        _viewPortRight =Camera.main.WorldToScreenPoint(temp2);
                            
        _tabBar = transform.Find("TabScrollView/Viewport/TabBar");
        _tabScrollView = transform.Find("TabScrollView").GetComponent<RectTransform>();
    }

    private void SetToggleItem()
    {
        
        for (int i = 0; i < _tabBar.childCount; i++)
        {
            Toggle toggle = _tabBar.GetChild(i).GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnTabChange);
        }

    }

  

   

    private void OnTabChange(bool isOn)
    {
        if (isOn == false) { return; }

        if (EventSystem.current.currentSelectedGameObject==null)
        {
            return;
        }
        
        string name = EventSystem.current.currentSelectedGameObject.name;
     
        
     
        if (!name.Contains("ActivityT"))
        {
            name = GlobalData.ActivityModel.ActivitySoleId;
        }
       
     
        SendMessage(new Message(MessageConst.CMD_ACTIVITY_SHOW_ACTIVITYDATA,name));


        var content = _tabBar.GetComponent<RectTransform>();
        

        for (int i = 0; i < _tabBar.childCount; i++)
        {
            if (name == _tabBar.GetChild(i).name)
            {
                var itemRect = _tabBar.GetChild(i).gameObject.GetComponent<RectTransform>();
                
                Vector3 [] item =new Vector3[4];
                itemRect.GetWorldCorners(item);
            
                var itemLeftPeak = item[0].x;
                var itemRightPeak = item[3].x;
               
                Vector2 curTogLeft = new Vector2(item[0].x,item[0].y);              
                Vector2 curTogRight =new Vector2(item[3].x,item[3].y);
                
                Vector2 curLeft =  Camera.main.WorldToScreenPoint(curTogLeft);
                Vector2 curRight =  Camera.main.WorldToScreenPoint(curTogRight);

                var scale = itemRect.rect.width /(curRight.x-curLeft.x) ; 

           
                if (itemLeftPeak<_viewPortV3[0].x)
                {
                    var offset = _viewPortLeft.x - curLeft.x;
                    offset = (content.anchoredPosition.x ) +offset*scale;                 
                    content.anchoredPosition=new Vector2(offset,content.anchoredPosition.y);
                }

                if (itemRightPeak > _viewPortV3[3].x)
                {
                    var offset = curRight.x- _viewPortRight.x ;
                    offset = (content.anchoredPosition.x - offset) *scale;                
                    content.anchoredPosition=new Vector2(offset,content.anchoredPosition.y);
                }
                         
                break;
                
            }
        }
        
        
        




    }

    private void ShowConcretenessActivity(string id)
    {
        _tabBar.Find(id).GetComponent<Toggle>().isOn = true;
      
       SendMessage(new Message(MessageConst.CMD_ACTIVITY_SHOW_ACTIVITYDATA, id));             
    }
  
    public void CreateActivityToggleAndActivityContent(List<ActivityVo> activityList,string soleId)
    {     
        var activityTogglePrefab = GetPrefab("Activity/Prefabs/ActivityToggle");
       
        var prefabWidth =  activityTogglePrefab.GetComponent<RectTransform>().GetWidth();
        var tabScrollViewWidth = _tabScrollView.GetWidth();


        int temp=0;

        for (int i = 0; i < activityList.Count; i++)
        {
            
            var activityToggleItem = Instantiate(activityTogglePrefab, _tabBar, false) as GameObject;
            activityToggleItem.transform.localScale = Vector3.one;

            var id = activityList[i].JumpId;
            activityToggleItem.name = id;                      
            activityToggleItem.GetComponent<Toggle>().group = _tabBar.GetComponent<ToggleGroup>();
            activityToggleItem.GetComponent<ActivityToggle>().SetData(activityList[i]);
            if (soleId == id)
            {
                temp = i;
            }  
        }
    
        var rightPeak = temp * prefabWidth+ prefabWidth;
    
        SetToggleItem();
        
        if (rightPeak> tabScrollViewWidth)
        {
            var move = rightPeak - tabScrollViewWidth;
            _tabBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(-move, 0);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_tabBar.GetRectTransform());          
        }
        
        ShowConcretenessActivity(soleId);
    }

    public  void RefreshActivityToggleRedDot(List<ActivityVo> activityList)
    {
        for (int i = 0; i < _tabBar.childCount; i++)
        {
            _tabBar.GetChild(i).GetComponent<ActivityToggle>().SetData(activityList[i]);
        }
    }

}


