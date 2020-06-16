using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class HeadFrameItem : MonoBehaviour
{
    private Transform _headIconMaskTra;
    private RawImage _headImg;
    private GameObject _lock;
    private RawImage _frameImg;
    private Button _onClickBtn;

    private Text _nameTxt;
    private HeadFrameVo _data;
    private GameObject _red;
    private void Start()
    {
        _onClickBtn.onClick.AddListener(OnClickHeadFrame);
    }

    public void SetData(HeadFrameVo vo,string curHeadPath)
    {
        _red = transform.Find("Red").gameObject;
        _headIconMaskTra = transform.Find("HeadIconMask");    
        _headImg = _headIconMaskTra.GetRawImage("Image/HeadIcon");
        _lock = _headIconMaskTra.Find("Lock").gameObject;
        _frameImg = _headIconMaskTra.GetRawImage("HeadFrame");       
        _onClickBtn = _headIconMaskTra.GetButton();
        _nameTxt = transform.GetText("NameText");
        
        _data = vo;
     
        _frameImg.texture = ResourceManager.Load<Texture>(vo.Path);
      
        _nameTxt.text = vo.Name;
        
        _lock.SetActive(!vo.IsUnlock);
        SetCurHead(curHeadPath);

        if (_data.IsShowRedDot)
        {
         _red.Show();   
        }
        else
        {
           _red.Hide();    
        }
    }

    public void SetCurHead(string curHeadPath)
    {
        _headImg.texture = ResourceManager.Load<Texture>(curHeadPath);
       
    }
    
    
    private void OnClickHeadFrame()
    {
        if (_data.IsUnlock&&_data.IsShowRedDot&& !PlayerPrefs.HasKey(_data.Key))
        {
            _red.gameObject.Hide();
            _data.IsShowRedDot = false;
            PlayerPrefs.SetInt(_data.Key,1);
            
        }
        
        EventDispatcher.TriggerEvent(EventConst.OnClickHeadFrame, _data);
    }
}