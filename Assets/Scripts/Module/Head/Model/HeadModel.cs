using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using UnityEngine;

public class HeadModel : Model
{
    private List<ElementPB> _headRule;
    private List<ElementPB> _headFrameRule;

    private List<HeadVo> _userHeadData;
    private List<HeadFrameVo> _userHeadFrameData;

    /// <summary>
    /// 用户头像框数据
    /// </summary>
    public List<HeadFrameVo> UserHeadFrameData => _userHeadFrameData;
       
    public HeadModel()
    {
        InitRule();
        InitUserData();
    }
    
    private void InitRule()
    {
        _headRule  = new List<ElementPB>();
        _headFrameRule = new List<ElementPB>();
    
        var baseRule = GlobalData.DiaryElementModel.BaseElementRule;//元素规则
        var openCardDic = GlobalData.CardModel.OpenBaseCardDict;//开放的卡牌数据
        
        foreach (var rule in baseRule)
        {
            var type = rule.ElementType;
            switch (type)
            {        
                case ElementTypePB.Avatar:
                  
                    if (openCardDic.ContainsKey(rule.UnlockClaim.CardId)||rule.UnlockClaim.CardId==0)
                    {
                        _headRule.Add(rule);
                   //    Debug.LogError("头像"+rule);
                    }
                    break;
                case ElementTypePB.AvatarBox:
                   //   Debug.LogError("头像框--->"+rule);
                    _headFrameRule.Add(rule);
                    break;        
            }
        }     
    }

    private void InitUserData()
    {
        _userHeadData = new List<HeadVo>();
        _userHeadFrameData = new List<HeadFrameVo>();


        //初始化用户头像数据
        foreach (var rule in _headRule)
        {
            var vo = new HeadVo(rule);
            if (vo.IsUnlock)
            {
                _userHeadData.Add(vo);                 
            }          
        }
        _userHeadData.Sort();    
        //初始化用户头像框数据
        foreach (var rule in _headFrameRule)
        {
            var vo = new HeadFrameVo(rule);         
            _userHeadFrameData.Add(vo);
        }
        _userHeadFrameData.Sort((x, y) => x.Sort.CompareTo(y.Sort));
       
    }

    
   

    public string GetCurPlayerHeadPath()
    {       
        var userOther = GlobalData.PlayerModel.PlayerVo.UserOther;
        return  GlobalData.DiaryElementModel.GetHeadPath(userOther.Avatar, ElementTypePB.Avatar);              
    }
    
    /// <summary>
    /// 获取用户头像List数据
    /// </summary>
    /// <param name="playerPb"></param>
    /// <returns></returns>
    public List<HeadVo> GetUserHeadData(PlayerPB playerPb)
    {
        List<HeadVo> data =new  List<HeadVo>();

        foreach (var t in _userHeadData)
        {
            if (t.PlayerPb == playerPb)
            {
                data.Add(t);
            }  
        }
     
        
        return data;
    }

    public List<HeadVo> GetAllUserHeadData()
    {
        return _userHeadData;
    }

    /// <summary>
    /// 获取头像具体信息
    /// </summary>
    /// <param name="id">头像Id</param>
    /// <returns></returns>
    public HeadVo GetHeadInfo(int id)
    {
        HeadVo vo = null;
        foreach (var data in _userHeadData)
        {
            if (data.Id == id)
            {
                vo = data;
                break;
            }
        }
        return vo;
    }

    /// <summary>
    /// 获取头像框具体信息
    /// </summary>
    /// <param name="id">头像框Id</param>
    /// <returns></returns>
    public HeadFrameVo GetHeadFrameVo(int id)
    {
        HeadFrameVo vo = null;
        foreach (var data in _userHeadFrameData)
        {
            if (data.Id == id)
            {
                vo = data;
                break;
            }
        }

        return vo;
    }

    public void UpdateUserHeadFrameData(HeadFrameVo vo)
    {
        for (int i = 0; i < _userHeadFrameData.Count; i++)
        {
            if (vo.Id ==_userHeadFrameData[i].Id)
            {
                _userHeadFrameData[i] = vo;
                break;
            }
        }
    }

    public bool IsShowHeadFrameRedDot()
    {
        bool isShow = false;
        foreach (var data in _userHeadFrameData)
        {
            if (data.IsShowRedDot)
            {
                isShow = true;
                break;
            }
        }

        return isShow;
    }
    
}
