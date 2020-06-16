#region 模块信息
// **********************************************************************
// Copyright (C) 2018 The 深圳望尘体育科技
//
// 文件名(File Name):             DropItem.cs
// 作者(Author):                  张晓宇
// 创建时间(CreateTime):           #CreateTime#
// 修改者列表(modifier):
// 模块描述(Module description):
// **********************************************************************
#endregion
using UnityEngine;
using Assets.Scripts.Framework.GalaSports.Service;
using UnityEngine.UI;
using Assets.Scripts.Module;
using DataModel;
using Module.Battle.Data;

namespace game.main
{
	public class RewardItem : MonoBehaviour {
		
		private Image _propImage;
		private Text _propNameTxt;
		private Text _obtainText;
		private Text _ativityText;
		private Transform _ativityItem;

		void Awake () 
		{
			_propImage = transform.Find("PropImage").GetComponent<Image>();
			_propNameTxt = transform.Find("PropNameTxt").GetComponent<Text>();
			_obtainText = transform.Find("ObtainText").GetComponent<Text>();
			_ativityText = transform.Find("DrawActivityItem/Text").GetComponent<Text>();
			_ativityItem = transform.Find("DrawActivityItem");

			_propNameTxt.text = "";
			_obtainText.text = "";
		}
		
		public void SetData(RewardVo vo, DrawActivityDropItemVo drawActivityDropItem)
		{
			_propImage.sprite = ResourceManager.Load<Sprite>(vo.IconPath, ModuleConfig.MODULE_MAIN_LINE);

			if(_propImage.sprite == null)
				_propImage.sprite = ResourceManager.Load<Sprite>("Prop/1122", ModuleConfig.MODULE_MAIN_LINE);

			if (drawActivityDropItem != null)
			{
				_ativityText.text =  I18NManager.Get("ActivityTemplate_TodayLimit", 
					drawActivityDropItem.TotalNum, drawActivityDropItem.LimitNum);
			}
			else
			{
				_ativityItem.gameObject.Hide();
			}
			
			_propNameTxt.text = vo.Name;
			_obtainText.text =  vo.Num.ToString();
			_propImage.SetNativeSize();
		}
	}
}
