using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using GalaAccountSystem;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using Common;

public class ActivityCapsuleTemplateDrawView : View
{
    private ActivityCapsuleTemplateDrawPoolItem[] _drawPool;
    private Button _btnLottery;

    private Button _haveNumBtn;
    private RawImage _haveNumIcon;
    private Text _haveNumText;
    private RawImage _costNumIcon;
    private Text _costNumText;

    private void Awake()
    {
        int poolCount = 9;
        _drawPool = new ActivityCapsuleTemplateDrawPoolItem[poolCount];
        for(int i = 0; i < poolCount; ++i)
        {
            _drawPool[i] = transform.Find("LotteryPool/" + (i + 1)).GetComponent<ActivityCapsuleTemplateDrawPoolItem>();
            _drawPool[i].id = i;
        }
        _btnLottery = transform.Find("BtnLottery").GetComponent<Button>();
        _btnLottery.onClick.AddListener(OnBtnLotteryClick);

        _haveNumBtn = transform.Find("HaveNum").GetComponent<Button>();
        _haveNumBtn.onClick.AddListener(OnBtnCostClick);
        _haveNumIcon = transform.Find("HaveNum/Icon").GetComponent<RawImage>();
        _haveNumText = transform.Find("HaveNum/Num").GetComponent<Text>();
        _costNumIcon = transform.Find("CostNum/Icon").GetComponent<RawImage>();
        _costNumText = transform.Find("CostNum/Num").GetComponent<Text>();

        //Vector2 pos = _haveNumBtn.transform.GetComponent<RectTransform>().anchoredPosition;
        _haveNumBtn.transform.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, -ModuleManager.OffY);

        ClientData.LoadItemDescData(null);
        ClientData.LoadSpecialItemDescData(null);
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetData(ActivityCapsuleTemplateModel model)
    {
        //Debug.Log("draw view setdata");
        for(int i = 0;i < _drawPool.Length; ++i)
        {
            if (i >= model.capsuleItemIds.Count)
                continue;
            int id = model.capsuleItemIds[i];
            ActivityCapsuleItemPB awardData = model.GetCapsuleItem(model.capsuleItemIds[i]);
            _drawPool[i].id = id;
            //Debug.Log("capsuleItem id:"+id);
            _drawPool[i].SetData(awardData, model);
        }
        _disableIds = model.gainCapsuleItems;


        if (model.costItem != null)
        {
            _haveNumIcon.texture = PropUtils.GetPropIcon(model.costItem.ResourceId);
            _haveNumIcon.color = Color.white;
            _haveNumText.text = PropUtils.GetUserPropNum(model.costItem.ResourceId).ToString();

            _costNumIcon.texture = PropUtils.GetPropIcon(model.costItem.ResourceId);
            _costNumIcon.color = Color.white;
            _costNumText.text = "x" + model.costItem.Num;
        }
    }

    public void UpdateUserProp(ActivityCapsuleTemplateModel model)
    {
        if (model.costItem != null)
        {
            _haveNumText.text = PropUtils.GetUserPropNum(model.costItem.ResourceId).ToString();
        }
    }


    private List<int> _disableIds = new List<int>();
    private bool _drawAnimState = false;

    IEnumerator DoLotteryAnim(int finalId)
    {
        _drawAnimState = true;
        List<ActivityCapsuleTemplateDrawPoolItem> tempPool = new List<ActivityCapsuleTemplateDrawPoolItem>();
        for(int i = 0;i < _drawPool.Length; ++i)
        {
            if (_disableIds.Contains(_drawPool[i].id)) continue;
            tempPool.Add(_drawPool[i]);
        }
        //int animCount = tempPool.Count * 2 + 15;
        int animCount = 20;

        //Debug.Log("start:"+tempPool.Count);
        if (tempPool.Count > 1)
        {

            List<ActivityCapsuleTemplateDrawPoolItem> waitPool = new List<ActivityCapsuleTemplateDrawPoolItem>();
            ActivityCapsuleTemplateDrawPoolItem lastItem = null;
            int randomIndex = 0;
            float interval = 0.01f;
            float addTime = 0.01f;
            for (int i = 0; i < animCount; ++i)
            {
                if (lastItem != null)
                {
                    lastItem.SetSelectState(false);
                }

                if (tempPool.Count == 0)
                {
                    List<ActivityCapsuleTemplateDrawPoolItem> temp = tempPool;
                    tempPool = waitPool;
                    waitPool = temp;
                    //Debug.LogWarning("tempPool:" + tempPool.Count + " waitPool:"+waitPool.Count);
                }

                randomIndex = UnityEngine.Random.Range(0, tempPool.Count);

                tempPool[randomIndex].SetSelectState(true);
                lastItem = tempPool[randomIndex];
                waitPool.Add(tempPool[randomIndex]);
                tempPool.RemoveAt(randomIndex);

                interval += addTime;
                yield return new WaitForSeconds(interval);
            }

            if (finalId != -1)
            {
                if (lastItem.id != finalId)
                {
                    lastItem.SetSelectState(false);
                    tempPool.AddRange(waitPool);
                    lastItem = tempPool.Find((go) =>
                    {
                        return go.id == finalId;
                    });
                    lastItem.SetSelectState(true);
                    yield return new WaitForSeconds(interval);
                }
            }

            //yield return new WaitForSeconds(0.5f);
            WaitForSeconds wait = new WaitForSeconds(0.1f);
            lastItem.SetSelectState(false);
            yield return wait;
            lastItem.SetSelectState(true);
            yield return wait;
            lastItem.SetSelectState(false);
            yield return wait;
            lastItem.SetSelectState(true);
            yield return wait;
            lastItem.SetSelectState(false);
        }
        EventDispatcher.TriggerEvent<int>(EventConst.ActivityCapsuleTemplateDrawAnimOver, finalId);
        _drawAnimState = false;
        yield return null;
    }
    
    private void OnBtnLotteryClick()
    {

        if (_drawAnimState) return;
        EventDispatcher.TriggerEvent<System.Action<int>>(EventConst.ActivityCapsuleTemplateDraw, OnDrawSucc);
    }

    private void OnDrawSucc(int id)
    {
        //Debug.LogWarning("OnDrawSucc:" + id);
        StopAllCoroutines();
        StartCoroutine(DoLotteryAnim(id));
    }

    private void OnBtnCostClick()
    {
        FlowText.ShowMessage(I18NManager.Get("ActivityCapsuleTemplate_drawCostTips"));
    }

    public void OnShow()
    {

    }


    public void DestroyCountDown()
    {

    }
}
