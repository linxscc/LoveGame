using System.Collections;
using System.Collections.Generic;
using Com.Proto;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class CommonAwardWindow : Window
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

	/// <summary>
	/// 设置通用奖励窗口数据
	/// </summary>
	/// <param name="list">奖励集合</param>
	/// <param name="isPreview">是否预览，true是标题为预览，false是标题为获得奖励</param>
	/// <param name="module">模块名</param>
	/// <param name="unloadLater">是否卸载</param>
	public void SetData(List<AwardPB> list,bool isPreview,string module =null, bool unloadLater = false)
	{
		_titleTxt.text = I18NManager.Get(isPreview ? "Common_PreviewAward" : "Common_GetAward");
		
		if (list.Count>3)
		{
			_scrollRect.movementType = ScrollRect.MovementType.Elastic;
			_awardsRect.pivot =new Vector2(0,0.5f);
		}

		List<RewardVo> awards =new List<RewardVo>();
		foreach (var t in list)
		{
			RewardVo vo =new RewardVo(t);
			awards.Add(vo);
		}
		
		var prefab = GetPrefab("GameMain/Prefabs/AwardWindow/CommonAwardItem");
		foreach (var t in awards)
		{
			var go = Instantiate(prefab, _parentTra, false);
			go.GetComponent<CommonAwardItem>().SetData(t,module,unloadLater);
		}
	}
}
