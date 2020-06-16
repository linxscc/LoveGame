using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using DataModel;
using game.tools;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace game.main
{
	public class SupporterAwardWindow : Window
	{

		//private Text _propNum;
		private Transform _propGroup;
		//private Text _tips;

		private void Awake()
		{
			_propGroup = transform.Find("PropList");
			//_tips = transform.Find("Title/Text").GetComponent<Text>();
			for (int i = 0; i < _propGroup.childCount; i++)
			{
				_propGroup.GetChild(i).gameObject.Show();
			}

		}

		public void SetData(RepeatedField<AwardPB> vo)
		{
			for (int i = 0; i < _propGroup.childCount; i++)
			{
				if (vo.Count>=i+1)
				{
					SetPropData(_propGroup.GetChild(i), vo[i]);
				}
				else
				{
					_propGroup.GetChild(i).gameObject.Hide();
				}
				
				
			}

            
		}

		private void SetPropData(Transform tran,AwardPB award)
		{
			var item = tran.Find("PropImage").GetComponent<RawImage>();
			var itemName = tran.Find("PropNameText").GetComponent<Text>();
			var itemNum = tran.Find("PropNum").GetComponent<Text>();
			string path = "";
			string name = "";
			if (award.Resource == ResourcePB.Item)
			{
				name = GlobalData.PropModel.GetPropBase(award.ResourceId).Name;
			}
			else
			{
				name = ViewUtil.ResourceToString(award.Resource);
			}
			//还差一个粉丝类型！


			if (award.Resource == ResourcePB.Gold)
			{
				//  vo.OwnedNum = (int)GlobalData.PlayerModel.PlayerVo.Gold;
				path = "Prop/particular/" + PropConst.GoldIconId;
			}
			else if (award.Resource == ResourcePB.Gem)
			{
				path = "Prop/particular/" + PropConst.GemIconId;
			}
			else if (award.Resource == ResourcePB.Power)
			{
				path = "Prop/particular/" + PropConst.PowerIconId;
			}
			else if (award.Resource == ResourcePB.Fans)
			{
				item.texture = ResourceManager.Load<Texture>("Prop/" + 900011, ModuleConfig.MODULE_SUPPORTER);
                name = I18NManager.Get("Supporter_Hint9");// "随机粉丝";
			}
			else
			{
				path = "Prop/" + award.ResourceId;
			}

			//var propitem = GlobalData.PropModel.GetPropBase(award.ResourceId);
//			Debug.LogError(" " + award.Num);
			item.texture = ResourceManager.Load<Texture>(path, ModuleConfig.MODULE_SUPPORTER);
			if (item.texture == null)
			{
				item.texture = ResourceManager.Load<Texture>("Prop/" + 900011, ModuleConfig.MODULE_SUPPORTER);
			}

			itemName.text = name;
			itemNum.text = "X" + award.Num;
		}

	}
}