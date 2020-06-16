using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ActivitySevenSigninView : View
{

    private Transform _sevenDaysAward; 
    private Text _hint1;    //活动剩余时间

 
    
    private void Awake()
    {              
     
        
   
        _sevenDaysAward = transform.Find("Bg/SevenDaysAward");
     
      
        _hint1 = transform.GetText("Bg/Hint1");


        
    }

    

    


    /// <summary>
    /// 生成角色（背景图片/角色进化图/页码Tog）
    /// </summary>
    /// <param name="list"></param>
//    public void CreateRoleData(List<int> list)
//    {

//        
//        
//        var npcId = GlobalData.PlayerModel.PlayerVo.NpcId;
//        
//        
//        _maxRoleNums = list.Count;
//        
//        var roleImage = GetPrefab("Activity/Prefabs/SevenDayRoleImage");
//        var role = GetPrefab("Activity/Prefabs/SevenRole");
//      //  var tog = GetPrefab("Activity/Prefabs/SevenDayTog");
//
//        
//        
//        for (int i = 0; i < list.Count; i++)
//        {
//            var roleImagePre = Instantiate(roleImage,_bg,false) as GameObject;
//            roleImagePre.transform.localScale = Vector3.one;
//            roleImagePre.name = list[i].ToString();
//            
//            var  roleBgRawImage = roleImagePre.GetComponent<RawImage>();
//            roleBgRawImage.texture = ResourceManager.Load<Texture>("Activity/SevendaysActivityBG_"+list[i]);
//            
//            var rolePre =Instantiate(role,_roleParent,false)as GameObject;
//            rolePre.transform.localScale = Vector3.one;
//            rolePre.name = list[i].ToString();
//            
//            roleBgRawImage =rolePre.GetComponent<RawImage>();
//            roleBgRawImage.texture = ResourceManager.Load<Texture>("Activity/Role_"+list[i]);
//            
//            var rect =rolePre.GetComponent<RectTransform>();
////            rect .anchoredPosition = pos[i];
////            rect.sizeDelta = size[i];
//            
//            rect .anchoredPosition = _posDic[list[i]/1000];
//            rect.sizeDelta =_sizeDic[list[i]/1000];
//            
//              
//           // var togPre =Instantiate(tog,_line,false)as GameObject;
//          //  togPre.transform.localScale = Vector3.one;
//          //  togPre.name = list[i].ToString();
//          //  togPre.GetComponent<Toggle>().group = _toggleGroup;
//            
//            if (list[i]/1000 == npcId)
//            {
//                _index = i;
//               // togPre.GetComponent<Toggle>().isOn = true;
//                
//            }
//            else
//            {
//                roleImagePre.GetComponent<RawImage>().color =new Color(roleBgRawImage.color.r,roleBgRawImage.color.g,roleBgRawImage.color.b,0);
//                rolePre.GetComponent<RawImage>().color =new Color(roleBgRawImage.color.r,roleBgRawImage.color.g,roleBgRawImage.color.b,0);
//            }
//        }
        
        //延迟3秒开始执行动画方法
       // ClientTimer.Instance.DelayCall(RoleBgAni, _speed);
       
//    }


    
    
    
//    private void RoleBgAni()
//    {   
//                
//        var curBg=  _bg.GetChild(_index).GetComponent<RawImage>();
//        var curRole = _roleParent.GetChild(_index).GetComponent<RawImage>();
//        RawImage nextBg;
//        RawImage nextRole;
//             
//        if (_index+1==_maxRoleNums)
//        {
//             nextBg =_bg.GetChild(0).GetComponent<RawImage>();
//             nextRole = _roleParent.GetChild(0).GetComponent<RawImage>();
//        }
//        else
//        {
//             nextBg =_bg.GetChild(_index+1).GetComponent<RawImage>();
//             nextRole = _roleParent.GetChild(_index+1).GetComponent<RawImage>();
//        }
//                
//        Tween curBgAlpha = curBg.DOColor(new Color(curBg.color.r,curBg.color.g,curBg.color.b,0),_speed );
//        Tween nextBgAlpha = nextBg.DOColor(new Color(nextBg.color.r,nextBg.color.g,nextBg.color.b,1),_speed );
//            
//        Tween curRoleAlpha =curRole.DOColor(new Color(curRole.color.r,curRole.color.g,curRole.color.b,0),_speed );
//        Tween nextRoleAlpha =nextRole.DOColor(new Color(nextRole.color.r,nextRole.color.g,nextRole.color.b,1),_speed );
//
//        Sequence tween = DOTween.Sequence()
//                .Join(curBgAlpha)
//                .Join(curRoleAlpha)
//                .Join(nextBgAlpha)
//                .Join(nextRoleAlpha) ;
//            
//            tween.onComplete = () =>
//            {
//                _index++;              
//                ClientTimer.Instance.DelayCall(RoleBgAni, _speed);
//
//                if (_index==_maxRoleNums)
//                {
//                    _index = 0;
//                }
//              //  _line.GetChild(_index).GetComponent<Toggle>().isOn = true;               
//            };   
//           
//    }


  
    
  
    
    
    /// <summary>
    /// 设置提示语数据
    /// </summary>    
    /// <param name="residueDay"></param>
    public void SetHintData(string residueDay)
    {        
        _hint1.text = I18NManager.Get("Activity_SevenActivityResidueDays", residueDay);
    }

 


    public void CreateSevenSigninData(List<SevenDaysLoginAwardVO> list)
    {
        var item = GetPrefab("Activity/Prefabs/SevenDaysLoginAwardItem");

        for (int i = 0; i < list.Count; i++)
        {
            var go = Instantiate(item, _sevenDaysAward.GetChild(i).transform, false) as GameObject;
            go.transform.localScale = Vector3.one;
            go.name = list[i].DayId.ToString();
            go.GetComponent<SevenDaysLoginAwardItem>().SetData(list[i]);
        } 
    }
     public void Refresh(int day)
     {      
           for (int i = 0; i < _sevenDaysAward.childCount; i++)
           {
               var itemDay = int.Parse(_sevenDaysAward.GetChild(i).GetChild(0).gameObject.name);
               if (day==itemDay)
               {
                   _sevenDaysAward.GetChild(i).GetChild(0).gameObject.transform.Find("GetBtn").gameObject.SetActive(false);
                   _sevenDaysAward.GetChild(i).GetChild(0).gameObject.transform.Find("Mask").gameObject.SetActive(true);
               }
              
           }
          
     }


}
