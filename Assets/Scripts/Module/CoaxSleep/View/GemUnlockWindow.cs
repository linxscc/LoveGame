using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class GemUnlockWindow : Window
{

   private Button _okBtn;
   private Button _cancelBtn;
   private Text _gemTxt;
   private int _gem;
   
   private void Awake()
   {
      _okBtn = transform.GetButton("okBtn");
      _cancelBtn = transform.GetButton("cancelBtn");
      
      _okBtn.onClick.AddListener(OnOkBtn);
      _cancelBtn.onClick.AddListener(OnCancelBtn);
      _gemTxt = transform.GetText("Text");
   }


   private void OnCancelBtn()
   {
     WindowEvent = WindowEvent.Cancel;
     base.Close();
   }

   private void OnOkBtn()
   {
      var playGemNum = GlobalData.PlayerModel.PlayerVo.Gem;
      if (playGemNum>=_gem)  
      {
         WindowEvent = WindowEvent.Ok;
         base.Close();        
      }
      else
      {
         string content = I18NManager.Get("CoaxSleep_GoShop");      
         string okTxt = I18NManager.Get("CoaxSleep_GoShopOkTxt");
         var window = PopupManager.ShowConfirmWindow(content, null, okTxt);
         window.WindowActionCallback = evt =>
         {
            if (evt!= WindowEvent.Cancel)
            {
               GoToShop();
            }
           
         }; 
         base.Close();         
      }                
   }

   private void GoToShop()
   {
      ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_SHOP,false,false,5);
   }
   
   
   public void SetData(int gem)
   {
      _gem = gem;      
      _gemTxt.text =  I18NManager.Get("CoaxSleep_CostGem", gem);
   }

   public void CompleteClose()
   {
      base.Close();
   }
}
