using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using DataModel;
using DG.Tweening;
using GalaAccount.Scripts.Framework.Utils;
using game.main;
using game.tools;
using GalaAccountSystem;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Module.Guide.ModuleView
{
    public class FavorabilityGuideView : View
    {

        public void ShowChangeRole()
        {		
            Transform view = transform.Find("FavorabilityChangeRole");
            view.gameObject.Show();
            Transform btnGroup = view.Find("BtnGroup");

            //四个角色绑定事件
            for (int i = 0; i < btnGroup.childCount; i++)
            {
                var roleId = i + 1;
                var vo = GlobalData.FavorabilityMainModel.GetUserFavorabilityVo(roleId);
                PointerClickListener.Get(btnGroup.GetChild(i).gameObject).onClick = go =>
                {										
                    SendMessage(new Message(MessageConst.CMD_FACORABLILITY_COMETOMAIN,Message.MessageReciverType.UnvarnishedTransmission,vo));
                    view.gameObject.Hide();  //隐藏选角色
                    ShowFavprabilityMain();  //显示好感度主界面
                };
            }

            Transform returnBtn = view.Find("ReturnBtn");
            PointerClickListener.Get(returnBtn.gameObject).onClick = go =>
            {
                FlowText.ShowMessage(I18NManager.Get("Guide_FavorabilityHint4"));
            };


            Transform hint = view.Find("Hint");
            hint.gameObject.Show();
            hint.GetText("DialogFrame/Text").text =I18NManager.Get("Guide_FavorabilityHint1");	
			
            Transform bg = view.Find("BG");

            PointerClickListener.Get(bg.gameObject).onClick = go =>
            {
                hint.gameObject.Hide();
            };

        }


        private void ShowFavprabilityMain()
        {
            Transform view = transform.Find("FavorabilityMain");
            view.gameObject.Show();

            var show = view.Find("Btn/Show").gameObject;
            var hint = view.Find("Hint");
            hint.GetText("DialogFrame/Text").text = I18NManager.Get("Guide_FavorabilityHint2");

            GuideArrow.DoAnimation(show.transform);

            PointerClickListener.Get(show.gameObject).onClick = go =>
            {
                //保存按钮事件 
                SendMessage(new Message(MessageConst.MODULE_DISIPOSITION_SHOW_MAINVIEW,Message.MessageReciverType.UnvarnishedTransmission));
                GuideManager.SetRemoteGuideStep(GuideTypePB.MainGuide,GuideConst.MainLineStep_OnClick_FavorabilityShowMainViewBtn);	
				
                //防止网络异常
                UserGuidePB guidePb = new UserGuidePB()
                {
                    GuideId = GuideConst.MainLineStep_OnClick_FavorabilityShowMainViewBtn,
                    GuideType = GuideTypePB.MainGuide
                };
                GuideManager.UpdateRemoteGuide(guidePb);
				
                GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_FavorabilityShowMainViewBtn);								
                view.Find("Btn").gameObject.SetActive(false);
                hint.GetText("DialogFrame/Text").text = I18NManager.Get("Guide_FavorabilityHint3");

                //此时给返回按钮绑定一个事件 

                var returnBtn = transform.Find("FavorabilityMain/ReturnBtn").gameObject;
                returnBtn.gameObject.Show();
				
                GuideArrow.DoAnimation(returnBtn.transform);
				
                PointerClickListener.Get(returnBtn).onClick = o =>
                {
                    SendMessage(new Message(MessageConst.CMD_FACORABLILITY_BACKTOFAVORABILITY,Message.MessageReciverType.UnvarnishedTransmission));
                    view.gameObject.Hide();
                    ShowFavorabilityChangeRoleBack();
                };
            };
					
        }

        private void ShowFavorabilityChangeRoleBack()
        {
            transform.Find("FavorabilityChangeRole").gameObject.Show();
            
            transform.Find("FavorabilityChangeRole/Hint").gameObject.Hide();
            var returnBtn = transform.Find("FavorabilityChangeRole/ReturnBtn").gameObject;
            returnBtn.gameObject.Show();
            				
            GuideArrow.DoAnimation(returnBtn.transform);
            
            PointerClickListener.Get(returnBtn).onClick = o =>
            {
                ModuleManager.Instance.GoBack();
            };
        }
    }
}