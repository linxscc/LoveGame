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
	public class SupporterRewardWindow : Window
	{

		//private Text _propNum;
		private Transform _propGroup;
		//private Text _tips;
		private Transform _extraRewards;
		private GameObject _extraRewardTxt;
		private Transform _bgtips;
		private GameObject _extraBg;

		private void Awake()
		{
			_propGroup = transform.Find("PropList");
			_extraRewards = transform.Find("ExtraReward");
			//_tips = transform.Find("Title/Text").GetComponent<Text>();
			_extraRewardTxt = transform.Find("ExtraRewardTxt").gameObject;
			_extraRewardTxt.Hide();
			_extraBg = transform.Find("ExtraBG").gameObject;
			for (int i = 0; i < _propGroup.childCount; i++)
			{
				_propGroup.GetChild(i).gameObject.Hide();
			}

			//_extraRewards.GetComponent<Image>().enabled = false;
			for (int i = 0; i < _extraRewards.childCount; i++)
			{
				_extraRewards.GetChild(i).Find("DropMax").gameObject.Hide();
				_extraRewards.GetChild(i).Find("Image").gameObject.Hide();
				_extraRewards.GetChild(i).gameObject.Hide();
			}
			_extraBg.SetActive(false);
			_bgtips = transform.Find("BGtips");


		}

		public void SetData(RepeatedField<AwardPB> awards,RepeatedField<AwardPB> awardPbs,RepeatedField<DroppingItemPB> dropPropItems)
		{
			//_tips.text =vo.Title;
//			Debug.LogError(awardPbs.Count);
			SetBgTipsState(awardPbs.Count > 0);
			_extraBg.SetActive(false);
			for (int i = 0; i < awards.Count; i++)
			{
				_propGroup.GetChild(i).gameObject.Show();
				SetPropData(_propGroup.GetChild(i), awards[i]);			
			}
			

			if (awardPbs.Count>0||dropPropItems.Count>0)
			{
				
				FlowText.ShowMessage(I18NManager.Get("SupporterActivity_GetExtraReward"));
				
				ClientTimer.Instance.DelayCall(() =>
				{
					Debug.LogError("_extraBg.SetActive(true)");
					_extraBg.SetActive(true);
					//_extraRewards.GetComponent<Image>().enabled = true;
					_extraRewardTxt.Show();
					SetExtraReward(awardPbs,dropPropItems);
				}, 0.3f);

			}



		}

		/// <summary>
		/// 额外奖励不会出现粉丝
		/// </summary>
		/// <param name="awardPbs"></param>
		/// <param name="fansType"></param>
		private void SetExtraReward(RepeatedField<AwardPB> awardPbs,RepeatedField<DroppingItemPB> dropPropItems,int fansType=0)
		{
			if (awardPbs.Count==0)
			{
				Debug.LogError("I need to show fans");
			}

			if (dropPropItems.Count>0&&awardPbs.Count<3)
			{
				_extraRewards.GetChild(awardPbs.Count).gameObject.SetActive(true);
				var dropitem = _extraRewards.GetChild(awardPbs.Count).Find("DropMax");
				var dropBg=_extraRewards.GetChild(awardPbs.Count).Find("Image");
				dropitem.gameObject.SetActive(true);
				dropBg.gameObject.SetActive(true);
				for (int i = 0; i < dropPropItems.Count; i++)
				{
					dropitem.GetComponent<Text>().text = "今日上限:"+dropPropItems[i].CurrentNum+"/"+
					                                     GlobalData.ActivityModel.Limit(dropPropItems[i].ActivityId,HolidayModulePB.ActivityEncourage);//dropPropItems[i].Num
					AwardPB awardPb=new AwardPB(){ResourceId = dropPropItems[i].ResourceId,Resource = dropPropItems[i].Resource,Num = dropPropItems[i].Num};
					SetPropData(_extraRewards.GetChild(awardPbs.Count),awardPb);
					
				}
				


			}
			
			for (int i = 0; i < awardPbs.Count; i++)
			{
				_extraRewards.GetChild(i).gameObject.Show();
				if (awardPbs[i].Resource==ResourcePB.Fans)
				{
					SetPropData(_extraRewards.GetChild(i),
						new AwardPB() {Num = 1, Resource = ResourcePB.Fans, ResourceId = fansType});
				}
				else
				{
					SetPropData(_extraRewards.GetChild(i), awardPbs[i]);
				}				
			}


			
			
			
		}
		
		
		private void SetPropData(Transform tran,AwardPB award)
		{			
//			Debug.LogError(award);
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
				var fansvo = GlobalData.DepartmentData.GetFans(award.ResourceId);

				if (fansvo==null)
				{
					FlowText.ShowMessage("error:no this fans"+award.ResourceId);
					Debug.LogError(award.ResourceId);
				}
				else
				{
					Debug.LogError(fansvo.FansHeadPath);
					path = fansvo.FansHeadPath;
					name = fansvo.Name;
				}
				

			}
			else
			{
				path = "Prop/" + award.ResourceId;
			}

			//var propitem = GlobalData.PropModel.GetPropBase(award.ResourceId);
			//Debug.LogError(" " + award.Num);
			item.texture = ResourceManager.Load<Texture>(path);
			if (item.texture == null)
			{
				item.texture = ResourceManager.Load<Texture>("Prop/" + 900011,ModuleConfig.MODULE_SUPPORTERACTIVITY);
			}

			itemName.text = name;
			itemNum.text = award.Num.ToString();
		}

		public void SetBgTipsState(bool hasestra)
		{
			if (hasestra)
			{
				 _bgtips.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,-227);
			}
			else
			{
				_bgtips.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,344);
			}
			
			
		}
		

	}
}