

using System.Collections.Generic;
using DataModel;
using UnityEngine;
using UnityEngine.UI;
using  Module.Activity.View;
namespace Module.Activity.Window
{
	public class ActivityAwardsWindow : game.main.Window
	{
		private Text _titleTxt;
		private Transform _parentTra;

		private ScrollRect _scrollRect;
		private RectTransform _awardsRect;


		private void Awake()
		{
			_titleTxt = transform.GetText("Title/Text");
			_parentTra = transform.Find("Content/Awards");
			_scrollRect = transform.Find("Content").GetComponent<ScrollRect>();
			_awardsRect = _parentTra.GetRectTransform();
		}


		public void SetData(SevenDaysLoginAwardVO vo,bool isPreview)
		{
			_titleTxt.text = I18NManager.Get(isPreview ? "Common_PreviewAward" : "Common_GetAward");
			var awards = vo.Rewards;
			
			if (awards.Count>3)
			{
				_scrollRect.movementType = ScrollRect.MovementType.Elastic;
				_awardsRect.pivot =new Vector2(0,0.5f);
			}
			
			CreateItem(awards);
			
		}

		public void SetDataActivityTemplate(SevenSigninTemplateAwardVo vo,bool isPreview)
		{
			_titleTxt.text = I18NManager.Get(isPreview ? "Common_PreviewAward" : "Common_GetAward");
			var awards = vo.Rewards;
			if (awards.Count>3)
			{
				_scrollRect.movementType = ScrollRect.MovementType.Elastic;
				_awardsRect.pivot =new Vector2(0,0.5f);
			}
			
			CreateItem(awards);
		}


		private void CreateItem(List<RewardVo> list)
		{
			var prefab = GetPrefab("Activity/Prefabs/ActivityAwardItem");
			foreach (var t in list)
			{
				var go = Instantiate(prefab, _parentTra, false);
				go.GetComponent<ActivityAwardsItem>().SetData(t);
			}
			
		}
		
		
	}
}
