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
using System.Collections;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using DataModel;
using game.tools;
using Module.Battle.Data;
using UnityEngine.UI;
using Assets.Scripts.Module;

namespace game.main
{
	public class DropItem : MonoBehaviour {
		
		private Image _propImage;
		private Text _propNameTxt;
		private Text _ownTxt;
		private DropVo _dropVo;

		void Awake () 
		{
			_propImage = transform.Find("CenterLayout/PropImage").GetComponent<Image>();
			_propNameTxt = transform.Find("PropNameTxt").GetComponent<Text>();
			_ownTxt = transform.Find("OwnTxt").GetComponent<Text>();

			_propNameTxt.text = "";
			_ownTxt.text = "";

			PointerClickListener.Get(gameObject).onClick = go =>
			{
				string tips = PropConst.GetTips(_dropVo.PropId);
				if(tips != null)
					FlowText.ShowMessage(tips);
			};
		}

		public void SetData(DropVo data)
		{
			_dropVo = data;
			
			_propImage.sprite = ResourceManager.Load<Sprite>(data.IconPath, ModuleConfig.MODULE_CARD);
			
			_propNameTxt.text = data.Name;
			_ownTxt.text = "已有：" + data.OwnedNum;
            _ownTxt.text = I18NManager.Get("MainLine_Have", data.OwnedNum);
            _propImage.SetNativeSize();
		}
		
	}
}
