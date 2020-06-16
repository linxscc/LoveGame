using System.Collections;
using System.Collections.Generic;
using Com.Proto;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;


// 活动模板奖励窗口（预览或获得）

public class ActivityTemplateAwardWinodw : Window
{

	private Text _titleTxt;
	private Transform _parent;

	private void Awake()
	{
		_titleTxt = transform.GetText("Title/Text");
		_parent = transform.Find("Awards/Content");
		
	}


	/// <summary>
	/// 设置预览数据
	/// </summary>
	public void SetParentAwardsData(RepeatedField<AwardPB> list)
	{
		_titleTxt.text = I18NManager.Get("ActivityTemplate_PreviewAwards");
		CreateItem(list, false);
	}


	/// <summary>
	/// 设置获奖奖励数据
	/// </summary>
	public void SetGetAwardsData(RepeatedField<AwardPB> list)
	{
		_titleTxt.text = I18NManager.Get("ActivityTemplate_GetAwards");
		CreateItem(list, true);
	}


	private void CreateItem(RepeatedField<AwardPB> list,bool isShowNum)
	{
		var prefab = GetPrefab("ActivityTemplate/Prefabs/ActivityTemplateAwardItem");
		foreach (var t in list)
		{
			GameObject go = Instantiate(prefab, _parent, false);
			go.GetComponent<ActivityTemplateAwardItem>().SetData(t,isShowNum);			
		}
	}
}
