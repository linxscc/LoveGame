﻿using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleSupportItem : MonoBehaviour
{
    private int _useNum=0;
    private int _power=0;
    private int _maxNum=0;
    private Text _nameText;
    private string _name;
    private int _canUseNum = 0;
    private Text _numText;
    private RawImage _tipText;
    private Button _reduceBtn;
    private RawImage _propImage;
    
    public int UseNum => _useNum;   
    public int MaxNum => _maxNum;
    public int TotalPower => _useNum * _power;
    
    public int ItemId;

    private void Awake()
    {
        _reduceBtn = transform.Find("ReduceBtn").GetComponent<Button>();
        _reduceBtn.gameObject.SetActive(false);
        
        if ( GuideManager.IsPass1_9())
        {
            _reduceBtn.gameObject.SetActive(true);
            _reduceBtn.onClick.AddListener(Reduce);
            transform.GetComponent<Button>().onClick.AddListener(Add);
        }
        
        _nameText = transform.Find("NameText").GetComponent<Text>();
        _numText = transform.Find("SupportItemNum/NumText").GetComponent<Text>();
        _tipText = transform.Find("TipText").GetComponent<RawImage>();
        _propImage = transform.Find("PropImage").GetComponent<RawImage>();
    }
    
    private void Reduce()
    {
        if(_useNum>0)
            _useNum--;
        _numText.text =  _useNum + "/" + _maxNum;
            
        _reduceBtn.gameObject.SetActive(_useNum != 0);
        EventDispatcher.TriggerEvent(EventConst.CapsuleBattleUpdateSupporterNum);
    }
    
    private void Add()
    {
        if (_useNum < _maxNum&&_useNum<_canUseNum)
        {
            _useNum++;
        }
        _numText.text =  _useNum + "/" + _maxNum;
            
        _reduceBtn.gameObject.SetActive(_useNum != 0);
        EventDispatcher.TriggerEvent(EventConst.CapsuleBattleUpdateSupporterNum);   
    }
    
    public void SetData(ItemPB itemPb, int use, int max, string iconPath)
    {
                     
        ItemId = itemPb.ItemId;
            
        _canUseNum = use;
        _name = itemPb.Name;
        _useNum = use;
        _maxNum = max;
        _nameText.text = _name;
        _numText.text =  _useNum + "/" + _maxNum;
        _power = itemPb.Power;

        _tipText.gameObject.SetActive(_canUseNum == 0);
        
        if ( GuideManager.IsPass1_9())
        {
            _reduceBtn.gameObject.SetActive(_canUseNum != 0);
        }
        
        _propImage.color = _canUseNum == 0 ? Color.gray : Color.white;

        _propImage.texture = ResourceManager.Load<Texture>(iconPath, ModuleConfig.MODULE_ACTIVITYCAPSULEBATTLE);
    }
}
