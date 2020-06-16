using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Common;
using DataModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace game.main
{
	public class EnsureOpenGateWindow : Window
	{


		private Text _tips;
		private Text _ensureTxt;
		private Button _cacle;
		private Button _ensure;
		private RawImage _prop;

		private void Awake()
		{
			_tips = transform.Find("Title/Tips1").GetComponent<Text>();
			_prop = transform.Find("Title/Prop").GetComponent<RawImage>();
			_cacle = transform.Find("BtnGroup/Cancle").GetComponent<Button>();
			_ensure = transform.Find("BtnGroup/Ensure").GetComponent<Button>();
			_ensureTxt = transform.Find("BtnGroup/Ensure/Text").GetText();//Common_OK1
			_cacle.onClick.AddListener(() =>
			{
				Close();
			});

		}

		public void SetData(AppointmentGateRuleVo vo,int appointmentId)
		{
			this.gameObject.Show();
			_ensure.onClick.RemoveAllListeners();
			bool isEnough = false;
			foreach (var v in vo.CosumesDic)
			{
				isEnough = v.Value <= GlobalData.PropModel.GetUserProp(v.Key).Num;
                //_tips.text = $"是否消耗           X{v.Value}\n(剩余{GlobalData.PropModel.GetUserProp(v.Key).Num}，通过回溯星缘获得)\n解锁新的恋爱剧情？";
                _tips.text = I18NManager.Get(isEnough?"LoveAppointment_EnsureOpenGateWindowTips":"LoveAppointment_EnsureOpenGateWindowTips2", v.Value, GlobalData.PropModel.GetUserProp(v.Key).Num);

                _prop.texture = ResourceManager.Load<Texture>("Prop/"+v.Key,ModuleConfig.MODULE_LOVEAPPOINTMENT);
			}


			_ensureTxt.text =isEnough? I18NManager.Get("Common_OK1"):I18NManager.Get("Common_Goto");

			if (isEnough)
			{
				_ensure.onClick.AddListener(OpenGate);
			}
			else
			{
				//跳转到星缘界面！
				_ensure.onClick.AddListener(GotoCardResolve);
			}
			
			//发送后要判断是否有足够的道具。

			
		}

		private void OpenGate()
		{
			EventDispatcher.TriggerEvent(EventConst.OpenGate);
			//this.gameObject.Hide();
		}
		
		private void GotoCardResolve()
		{
			EventDispatcher.TriggerEvent(EventConst.LoveAppointmentGotoCardResolve);
			//this.gameObject.Hide();
		}

		private void OnDestroy()
		{
			EventDispatcher.RemoveEvent(EventConst.OpenGate);
		}
	}
}