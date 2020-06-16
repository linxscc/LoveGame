using System;
using Com.Proto;
using DataModel;
using game.tools;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
	public class AwardWindow : Window
	{

		private Button _closeBtn;
//		private LoopHorizontalScrollRect _propList;
		private RepeatedField<AwardPB> _awardPbs;
		private Transform _contentRoot;
		private Text _title;
		private void Awake()
		{
			_title = transform.GetText("Title/Text");
			_closeBtn = transform.Find("CloseBtn").GetButton();
			_contentRoot = transform.Find("AwardList/Content");
//			_propList = transform.Find("AwardList").GetComponent<LoopHorizontalScrollRect>();
//			_propList.prefabName = "Shop/Prefab/AwardWindow/AwardItem";
//			_propList.poolSize = 3;
			 
           ClientData.LoadItemDescData(null);
           ClientData.LoadSpecialItemDescData(null);

			_closeBtn.onClick.AddListener(() =>
			{
				this.Close();
			});
		}

		public void SetData(RepeatedField<AwardPB> awardlist,string title=null)
		{

			if (title==null)
			{
				_title.text = I18NManager.Get("Common_GetAward");
			}
			else
			{
				_title.text = title;
			}
			
			//_awardPbs = awardlist;
			_awardPbs=new RepeatedField<AwardPB>();
			bool contain = false;
			foreach (var v in awardlist)
			{
				foreach (var u in _awardPbs)
				{
					if (u.ResourceId==v.ResourceId)//&&u.Resource==v.Resource
					{
						u.Num += v.Num;
						contain = true;
						break;
					}

					contain = false;
				}

				if (!contain)
				{
					_awardPbs.Add(v);
				}
				
			}
			
			
			for (int i = 0; i < _awardPbs.Count; i++)
			{
				var go = InstantiatePrefab("GameMain/Prefabs/AwardWindow/AwardItem");
				go.transform.SetParent(_contentRoot,false);


				
				
				UpdateAwardItem(go,i);
			}
//			_propList.RefillCells();
//			_propList.UpdateCallback = UpdateAwardItem;
//			_propList.totalCount = _awardPbs.Count;
//			_propList.RefreshCells();
		}

		private void UpdateAwardItem(GameObject go, int index)
		{
			go.GetComponent<AwardItem>().SetData(_awardPbs[index]);
			PointerClickListener.Get(go.gameObject).onClick = g =>
			{
				var desc = ClientData.GetItemDescById(_awardPbs[index].ResourceId,_awardPbs[index].Resource);
				if (desc!=null)
				{
					FlowText.ShowMessage(desc.ItemDesc); 			
				}
			};
			
		}
	}

}


