using Assets.Scripts.Framework.GalaSports.Core;
using game.main;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CoerceUpdateView : View
{
    private Button _leftBtn;
    private Button _rightBtn;
    

    private Transform _panelParent;
    private Transform _pageParent;

    private ToggleGroup _toggleGroup;

    private float _width;
    private int _index = 0;


    private string _updateLater;//更新后的送礼
    private string _qq;         //安卓的QQ号

    private Image _titleImage; //就是那个2月14那种图

    private GameObject _top;
    
    private void Awake()
    {
       
        _leftBtn = transform.GetButton("Announcement/LeftBtn");
        _rightBtn = transform.GetButton("Announcement/RightBtn");
     
        _panelParent = transform.Find("Announcement/Bg/Panels/Gird");
        _pageParent = transform.Find("Announcement/Pages");

        _toggleGroup = _pageParent.GetComponent<ToggleGroup>();

        _width = _panelParent.GetComponent<RectTransform>().GetWidth();

        _titleImage = transform.GetImage("Announcement/TitleImage");

        _top = transform.Find("Announcement/Top").gameObject;
    }



    public void SetDate(string updateLater, string qq,string imageIame)
    {
        _updateLater = updateLater;
        _qq = qq;
      //  _titleImage.sprite = "";
    }
    
    
    /// <summary>
    /// 生成热更页面
    /// </summary>
    /// <param name="str">数据</param>随便写的一个集合，数据结构还没顶
    public void CreatePanelAndPage(List<string> str)
    {

        var panelPrefab = GetPrefab("Update/Prefabs/NewPanelItem");  //展示图片的Image
        var pagePrefab = GetPrefab("Update/Prefabs/PageItem");       //页标的预制体

        var iosUpdatePanelPrefab = GetPrefab("Update/Prefabs/IosUpdatePanelItem");    //苹果最后一个更新页面
        var androidPanelPrefab = GetPrefab("Update/Prefabs/AndroidPanelItem");        //安卓最后一个更新页面

        for (int i = 0; i < str.Count; i++)    
        {

            if (i==str.Count-1)       //当循环到最后一个就要区分加载预制体是安卓还是苹果
            {

                if (Application.platform==RuntimePlatform.Android)                                         //安卓
                {
                      var panel = Instantiate(androidPanelPrefab, _panelParent, false);
                      panel.name = (i + 1).ToString();
                      panel.transform.localScale = Vector3.one; 
                      panel.transform.GetButton("Bg/Button").onClick.AddListener(UpdateBtn);
                    //  panel.transform.GetText("Bg/UpdateGiveText").text = _updateLater;   //更新后登陆即送：XXXX
                  //  panel.transform.GetText("Bg/Content").text = I18NManager.Get("Update_Hint7",_qq);
                }
                else if (Application.platform == RuntimePlatform.IPhonePlayer)                            //苹果
                {
                    var panel = Instantiate(iosUpdatePanelPrefab, _panelParent, false);
                    panel.name = (i + 1).ToString();
                    panel.transform.localScale = Vector3.one;
                    
                    panel.transform.GetButton("BG/UpdateButton").onClick.AddListener(UpdateBtn);
                    //panel.transform.GetText("BG/UpdateGiveText").text = _updateLater;

                }
            }
            else
            {
                var panel = Instantiate(panelPrefab, _panelParent, false);
                panel.name = (i + 1).ToString();
                panel.transform.localScale = Vector3.one;
                //panel.transform.Find("Image").GetComponent<RawImage>().texture = ;//图片赋值
            }
                                
            var page = Instantiate(pagePrefab, _pageParent, false);
            page.GetComponent<Toggle>().group = _toggleGroup;
            page.name = (i + 1).ToString();
            page.transform.localScale = Vector3.one;
        }
        _pageParent.GetChild(_index).GetComponent<Toggle>().isOn = true;


    }



    private void UpdateBtn()
    {
      
        SendMessage(new Message(MessageConst.CMD_UPDATE_DO_UPDATE_BTN));   
    }
    

    private void LeftBtnEvent()
    {
        _index--;
        _panelParent.GetComponent<RectTransform>().DOAnchorPosX(-(_index * _width), 0.5f);
        if (_index==0)
        {
            _leftBtn.gameObject.SetActive(false);
        }
        if (_panelParent.childCount > 0)
        {
            if (_index< _panelParent.childCount - 1)
            {
               
                _rightBtn.gameObject.SetActive(true);
            }
        }
        _top.SetActive(true);
        _titleImage.gameObject.SetActive(true);
      
        _pageParent.GetChild(_index).GetComponent<Toggle>().isOn = true;
    }
   

    private void RightBtnEvent()
    {
        _index++;
        _panelParent.GetComponent<RectTransform>().DOAnchorPosX(-(_index * _width), 0.5f);
        if (_index>0)
        {
            _leftBtn.gameObject.SetActive(true);
        }
        if (_panelParent.childCount>0)
        {
            if (_index== _panelParent.childCount-1)
            {
                _top.SetActive(false);
               _titleImage.gameObject.SetActive(false);
                _rightBtn.gameObject.SetActive(false);
            }
        }
        _pageParent.GetChild(_index).GetComponent<Toggle>().isOn = true;
    }

    


}
