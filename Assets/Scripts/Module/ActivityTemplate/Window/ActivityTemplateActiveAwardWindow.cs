using System.Collections.Generic;
using Com.Proto;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class ActivityTemplateActiveAwardWindow : Window
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


    public void SetData(List<AwardPB> list,bool isPreview)
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
        CreateItem(awards);
			
    }


    private void CreateItem(List<RewardVo> awards)
    {
        var prefab = GetPrefab("ActivityTemplate/Prefabs/ActivityTemplateActiveAwardItem");
        foreach (var t in awards)
        {
            var go = Instantiate(prefab, _parentTra, false);
            go.GetComponent<ActivityTemplateActiveAwardItem>().SetData(t);
        }
			
    }
}


