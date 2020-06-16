using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActivityTemplateCardAniWindow : Window
{

	private Text _titleTxt;
	//private Button _closeBtn;
	private Transform _parent;
	private RepeatedField<AwardPB> _awardPbs;
	private int _index;
	
	private void Awake()
	{
		_titleTxt = transform.GetText("TitleTxt");
		_titleTxt.text = I18NManager.Get("ActivityTemplate_ActivityTemplateGetCardTitle");
		
		//_closeBtn = transform.GetButton("TitleTxt/CloseBtn");
		_parent = transform.Find("List");
		
//		_closeBtn.onClick.AddListener(() =>
//		{
//			EventDispatcher.TriggerEvent(EventConst.ActivityTemplateCardAniOver,_awardPbs);
//			base.Close();
//		});

    
	        
	}


	protected override void OnClickOutside(GameObject go)
	{
	
		bool isFinish = _awardPbs.Count - _index == 0;
		if (isFinish)
		{
			base.OnClickOutside(go);
		}
	}


	public void SetData(RepeatedField<AwardPB> awardPbs)
	{
		BgMask.transform.GetRawImage().color =new Color(0,0,0,0.8f);
		_awardPbs = awardPbs;
		_index = 0;
		
		string[] spriteName = new string[3] { "logo0", "logo1", "logo2" };
		for (int i = 0; i < awardPbs.Count; i++)
		{
			GameObject go = InstantiatePrefab("ActivityTemplate/Prefabs/Cards");
			go.transform.SetParent(_parent, false);
			go.transform.Find("Cards/logo").GetComponent<Image>().sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_ActivityTemplate_" + spriteName[Random.Range(0,3)]);
			PointerClickListener.Get(go).onClick = ShowReward;
		}
	}

	private void ShowReward(GameObject go)
	{
		if (_index >= _awardPbs.Count)
			return;
		
		 go.GetComponent<Animator>().enabled = true;
		 //播放音频
		 AudioManager.Instance.PlayEffect("fanpai");
		 CardAniItem item = go.AddComponent<CardAniItem>();
		 var vo = _awardPbs[_index++];
		 item.ShowReward(vo);
		 if (vo.Resource== ResourcePB.Item)
		 {
			 var id =vo.ResourceId;
			 SetMaterial(id, go);
		 }else if (vo.Resource == ResourcePB.Puzzle)
        {
            SetLightMat(vo.ResourceId, go);
        }


        PointerClickListener.Get(go).onClick = null;

		 CloseWindow();

	}


	private void SetMaterial(int id,GameObject go)
	{
		if (GlobalData.PropModel.GetPropBase(id).ExpType==4)
		{
            SetLightMat(id, go);
        }
	}

    private void SetLightMat(int id, GameObject go)
    {
        go.name = id.ToString();
        string path = "ActivityTemplate/Animation/Card";
        go.transform.Find("Cards/light").GetComponent<Image>().material = ResourceManager.Load<Material>(path);
        go.transform.Find("Cards/light").GetRectTransform().sizeDelta = new Vector2(417.3f, 521.5f);
        go.transform.Find("Cards/light").GetRectTransform().anchoredPosition = new Vector2(2.8f, -1.7f);
    }

    private void CloseWindow()
	{
		bool isFinish = _awardPbs.Count - _index == 0;
		if (isFinish)
		{
			ClientTimer.Instance.DelayCall(() =>
			{
				EventDispatcher.TriggerEvent(EventConst.ActivityTemplateCardAniOver,_awardPbs);
				base.Close();
			}, 1f);
		}
		
	}
}
