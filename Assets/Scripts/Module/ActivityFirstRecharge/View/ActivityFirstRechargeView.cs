using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using game.main;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActivityFirstRechargeView : View
{


   private FirstRechargeWindow _window;
   private Transform _roleCard; 
   private Transform _awards;
   private Transform _line;
   
   
   private Button _left;
   private Button _right;
   private Button _getBtn;
   private Button _rechargeBtn;
   
   private ToggleGroup _group;
   
   private List<FirstRechargeVO> _optionalAwards;      //可选奖励的集合
   private int _optionalAwardsCount;                   //可选奖励的长度
   private float _speed=0.5f;
   private int _curIndex;
   private int _nextIndex;
   
   private GameObject _bg;
   private Vector2 _prePressPos;
    

   private List<FirstRechargeVO> _temp;               //回包奖励集合

   private GameObject _hint;

 
   private bool _isAutomation;
   private void Awake()
   {

      _bg = transform.Find("BG").gameObject;
      UIEventListener.Get(_bg.gameObject).onDown = OnDown;
      UIEventListener.Get(_bg.gameObject).onUp = OnUp;

      _hint = transform.Find("Content/Text").gameObject;


      if (ModuleManager.OffY / 2!=0)
      {
         transform.Find("Content").GetComponent<RectTransform>().anchoredPosition =new Vector2(0,0f); 
      }
          
      _roleCard = transform.Find("Content/RoleCard");
   
      _awards = transform.Find("Content/FixedAwards/Content");
      _line = transform.Find("Content/Tog/Line");
      
      _left = transform.GetButton("Content/LeftBtn");
      _right = transform.GetButton("Content/RightBtn");
      _left.onClick.AddListener(Left);
      _right.onClick.AddListener(Right);
          
      ArrowsAni(_left.transform.GetChild(0), new Vector2(1.5f,1.5f),new Vector2(1f, 1f), 0.7f ,0.3f);
      ArrowsAni(_right.transform.GetChild(0), new Vector2(1.5f,1.5f),new Vector2(1f, 1f), 0.7f ,0.3f);
      _group = _line.GetComponent<ToggleGroup>();


      _rechargeBtn = transform.GetButton("Content/RechargeBtn");
      _getBtn = transform.GetButton("Content/GetBtn");
      
      ClientData.LoadItemDescData(null);
      ClientData.LoadSpecialItemDescData(null);
      
   }

   
   private void OnDown(PointerEventData data)
   {
      _prePressPos = data.pressPosition;
   }
   
   private void OnUp(PointerEventData data)
   {
      float dis = (data.position - _prePressPos).magnitude;
      bool isRight = (_prePressPos.x - data.position.x) > 0 ? true : false;

      if (dis > 100)
      {
         ScrollingDisplay(isRight);
      }
		
   }
   
   private void ScrollingDisplay(bool isRight)
   {
      if (isRight)
      {
         Right();
      }
      else
      {
         Left();
      }
   }
      
   private void ArrowsAni(Transform arrows,Vector2 starValue,Vector2 endValue ,float duration,float waitTime)
   {
      var t1 = arrows.DOScale(endValue, duration).SetEase(Ease.InOutSine );
      var t2 = arrows.DOScale(starValue, duration).SetEase(Ease.InOutSine );

      var tween = DOTween.Sequence()
         .Append(t2)
        
         .Append(t1);
      tween.onComplete = () => { ArrowsAni(arrows, starValue, endValue, duration, waitTime); };      
   }
  
   
   private void Right()
   {
      _nextIndex = _curIndex + 1;
      if (_nextIndex==_optionalAwardsCount)
      {
         _nextIndex = 0;
      }
      Ani();
   }

   private void Left()
   {      
      _nextIndex = _curIndex - 1;
      if (_nextIndex<0)
      {
         _nextIndex = _optionalAwardsCount - 1;
      }
      Ani();
   }

   //手动切换动画
   private void Ani()
   {
      var curBg = _roleCard.GetChild(_curIndex).GetComponent<RawImage>();
      var nextBg = _roleCard.GetChild(_nextIndex).GetComponent<RawImage>();
           
      Tween curBgAlpha = curBg.DOColor(new Color(curBg.color.r,curBg.color.g,curBg.color.b,0),_speed );
      Tween nextBgAlpha = nextBg.DOColor(new Color(nextBg.color.r,nextBg.color.g,nextBg.color.b,1),_speed );

      Sequence tween = DOTween.Sequence()
         .Join(curBgAlpha)
         .Join(nextBgAlpha);
        
      tween.onComplete = () =>
      {
         _curIndex = _nextIndex;			
         _line.GetChild(_curIndex).GetComponent<Toggle>().isOn = true;
      };
   }
     

   private int _curOkIndex = 0;
   
   private void GetBtn()
   {
      _curOkIndex = _curIndex;
      string name = _optionalAwards[_curOkIndex].CardName;
      
      PopupManager.ShowConfirmWindow(I18NManager.Get("Activity_FirstRechargeHint", name)).WindowActionCallback = evt =>      
      {
         if (evt ==WindowEvent.Ok)
         {            
             var vo = _optionalAwards[_curOkIndex];             
             SendMessage(new Message(MessageConst.CMD_ACTIVITY_FIRSTRECHARGE_GET_BTN,vo));                      
             EventDispatcher.TriggerEvent<bool>(EventConst.CloseFirstRechargeBtn,false);
         }         
      }; 
   }


   private void RechargeBtn()
   {
       SendMessage(new Message(MessageConst.CMD_ACTIVITY_FIRST_RECHARGE_BTN));
       ModuleManager.Instance.GoBack();
   }


   private void Start()
   {
      _bg.transform.GetRawImage().texture = ResourceManager.Load<Texture>("Background/mtl1"); 
   }

   
   
   public void SetData(bool isRecharge,List<FirstRechargeVO> fixedAwards, List<FirstRechargeVO> optionalAwards)
   {
      
      
      if (isRecharge)
      {
         _hint.gameObject.SetActive(false);
         _getBtn.gameObject.SetActive(true);
         _getBtn.onClick.AddListener(GetBtn);
      }
      else
      {
         _hint.gameObject.SetActive(true);
         _rechargeBtn.gameObject.SetActive(true);
         _rechargeBtn.onClick.AddListener(RechargeBtn);        
      }

      _optionalAwards = optionalAwards;
      _optionalAwardsCount = optionalAwards.Count;
    
      CreateFixedAwards(fixedAwards);
      CreateRoleCardAndSignature(optionalAwards);            
   }

   private void CreateFixedAwards(List<FirstRechargeVO> fixedAwards)
   {
      var prefab = GetPrefab("ActivityFirstRecharge/Prefabs/FirstRechargeItem");
      foreach (var i in fixedAwards)
      {
         var item = Instantiate(prefab, _awards, false);
         item.transform.localScale = Vector3.one;        
         item.GetComponent<FirstRechargeItem>().SetData(i);
      }
   }
   
   private void CreateRoleCardAndSignature(List<FirstRechargeVO> optionalAwards)
   {
      var roleCardPre = GetPrefab("ActivityFirstRecharge/Prefabs/RoleCard");      
      var pagePre =GetPrefab("ActivityFirstRecharge/Prefabs/Tog");
      var npcId = GlobalData.PlayerModel.PlayerVo.NpcId;
           
      for (int i = 0; i < optionalAwards.Count; i++)
      {
         var roleCard = Instantiate(roleCardPre, _roleCard, false);
         roleCard.transform.localScale = Vector3.one;
         roleCard.name = optionalAwards[i].RewardVo.Id.ToString();
         
         var roleCardImage =roleCard.GetComponent<RawImage>();
         roleCardImage.texture =ResourceManager.Load<Texture>(optionalAwards[i].ShowCardImage);
       
         var page =Instantiate(pagePre, _line, false);
         page.transform.localScale = Vector3.one;
         page.name = optionalAwards[i].RewardVo.Id.ToString();
         page.GetComponent<Toggle>().group = _group;

         if (optionalAwards[i].RewardVo.Id/1000 == npcId)
         {
            _curIndex = i;
            page.GetComponent<Toggle>().isOn = true;
         }
         else
         {
            roleCardImage.color =new Color(roleCardImage.color.r,roleCardImage.color.g,roleCardImage.color.b,0);          
         }
      }
      
   }
   
   /// <summary>
   /// 显示得到卡的动画
   /// </summary>
   public void ShowGetCardAnimation(List<AwardPB> list)
   {     
      Action finish = () =>
      {
         //完成后首先隐藏         
         gameObject.Hide();
         
         _temp=  new List<FirstRechargeVO>();
         foreach (var item in list)
         {
            if (item.Resource == ResourcePB.Card)
            {
               continue; 
            }
            FirstRechargeVO vo =new FirstRechargeVO(item);
            _temp.Add(vo);
         }
      
         ModuleManager.Instance.GoBack(); 
         //打开奖励窗口
         if (_window==null)
         {
            _window = PopupManager.ShowWindow<FirstRechargeWindow>("ActivityFirstRecharge/Prefabs/FirstRechargeAwardShowWindow");
            _window.SetData(_temp);                      
         }
       
      };
      
      //从首充奖励回包中把卡的AwardPB拿出来     
      List<AwardPB> temp =new List<AwardPB>();
      foreach (var item in list)
      {
         if (item.Resource== ResourcePB.Card)
         {
            temp.Add(item);
            break;
         }
      }
      
     
      SendMessage(new Message(MessageConst.CMD_FIRSTRECHARGE_HIDE_BACK_BTN));
      
      ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD,
         false,false,"DrawCard_CardShow",temp,finish,false);
      
   }
}
