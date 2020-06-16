using System.Collections.Generic;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PayView : Window 
{
	
	private void Awake()
	{
//		Transform item = transform.Find("BuyGem/GemItem");
//		PointerClickListener.Get(item.gameObject).onClick = BuyGem1;

		List<ProductVo> list = GlobalData.PayModel.GetGemProducts();
		Transform panel = transform.Find("Panel");
		for (int i = 0; i < list.Count; i++)
		{
			panel.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
			{
				GameObject obj = EventSystem.current.currentSelectedGameObject;
				if (obj != null)
				{
					SdkHelper.PayAgent.Pay(list[obj.transform.GetSiblingIndex()]);
				}
			});
		}

		List<ProductVo> list2 = GlobalData.PayModel.GetGiftProducts();
		Transform panel2 = transform.Find("Panel2");
		for (int i = 0; i < 6; i++)
		{
			panel2.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
			{
				GameObject obj = EventSystem.current.currentSelectedGameObject;
				if (obj != null)
				{
					SdkHelper.PayAgent.PayGift(list2[obj.transform.GetSiblingIndex()]);
				}
			});
		}
		
		transform.GetButton("GBtn").onClick.AddListener(() =>
		{
			SdkHelper.PayAgent.PayGrowthFund();
		});
		
		transform.GetButton("MBtn").onClick.AddListener(() =>
		{
			SdkHelper.PayAgent.PayMonthCard();
		});
		
	}

	private void BuyGem1(GameObject go)
	{
//		SdkHelper.PayAgent.Pay(GlobalData.PayModel.GetGemProducts()[0]);
	}
}


