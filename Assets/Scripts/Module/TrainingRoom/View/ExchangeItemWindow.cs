using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;
public class ExchangeItemWindow : Window
{

   private Text _title;
   private Text _descTxt;
   private RawImage _itemIcon;
   private Text _itemNum;
   private Text _curHaveNum;
   private Text _costNum;
   private Button _buyBtn;

   private void Awake()
   {
      _title = transform.GetText("window/TitleText");
      _descTxt = transform.GetText("window/Desc");
      _itemIcon = transform.GetRawImage("window/FrameImage/ItemRawImage");
      
      _itemNum = transform.GetText("window/FrameImage/ItemNum");
      _curHaveNum = transform.GetText("window/CurNum");
      _costNum = transform.GetText("window/Content");

      _buyBtn = transform.GetButton("window/BuyBtn");
   }


   public void SetData(ExchangeVO vo)
   {
      _title.text = "兑换"+vo.Name;
      _descTxt.text = vo.Desc;
      _itemIcon.texture = ResourceManager.Load<Texture>(vo.IconPath);
      _itemNum.text = vo.Num.ToString();
      UserPropVo userPropVo = GlobalData.PropModel.GetUserProp(vo.PropId);
      _curHaveNum.text = "当前拥有： "+userPropVo.Num;
      _costNum.text = "是否花费  "+vo.Price+"      购买";

      if (vo.Price>=GlobalData.TrainingRoomModel.GetCurIntegral())
      {      
         _buyBtn.onClick.AddListener((() =>
         {
            FlowText.ShowMessage("兑换币不足");
         }));
      }
      else
      {
         _buyBtn.onClick.AddListener((() =>
         {
            WindowEvent = WindowEvent.Ok;
            Close();
         })); 
        
      }
   }
}
