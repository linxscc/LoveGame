

using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class GetFristCardAwardWindow : Window
{
   private Button _okBtn;
   private RawImage _icon;
   private Text _name;
   
   private void Awake()
   {
      _okBtn = transform.GetButton("okBtn");
      _okBtn.onClick.AddListener(OkBtn);
      _icon = transform.GetRawImage("Item/CenterLayout/PropImage");
      _name = transform.GetText("Item/PropNameTxt");
   }


   public void SetData(RewardVo vo)
   {
      
      _icon.texture = ResourceManager.Load<Texture>(vo.IconPath);
      _name.text =  RoleName(vo.Id)+" • "+GlobalData.CardModel.GetCardBase(vo.Id).CardName;
      
   }
   
   
   private string RoleName(int cardId)
   {
      var roleId = cardId / 1000;
      return I18NManager.Get("Common_Role"+roleId);       
   }
   
   private void OkBtn()
   {
      WindowEvent = WindowEvent.Ok;
      Close();
   }


   protected override void OnClickOutside(GameObject go)
   {
     
   }
}
