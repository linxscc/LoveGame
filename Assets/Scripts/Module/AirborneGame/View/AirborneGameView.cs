using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AirborneGameView : View
{
    Transform _contains;

    List<GameObject> _ItemPool;
    int _ItemPoolCount = 20;
    List<Vector3> _startPoint;

    GameObject _right;
    GameObject _left;
    Image _addTimeImg;
    private void Awake()
    {
        _contains = transform.Find("Contains");
        _ItemPool = new List<GameObject>();
        _right = transform.Find("RightBtn").gameObject;
        _left = transform.Find("LeftBtn").gameObject;
        UIEventListener.Get(_right).onDown = RightOnDown;
        UIEventListener.Get(_right).onUp = RightOnUp;
        UIEventListener.Get(_left).onDown = LeftOnDown;
        UIEventListener.Get(_left).onUp = LeftOnUp;
        _addTimeImg = transform.Find("AddTime").GetComponent<Image>();
    }
    private void Start()
    {
        _startPoint = new List<Vector3>();
        for (int i = 0; i < 7; i++)
        {
            string path = "V" + i.ToString();
            Debug.Log(path);
            Vector3 oldpos = _contains.Find(path).GetComponent<RectTransform>().localPosition;
            Vector3 pos = new Vector3(
               oldpos.x,
               oldpos.y + 200,
               oldpos.z);
            _startPoint.Add(pos);
        }
    }

    private void RightOnUp(PointerEventData eventData)
    {
        if (_playItem == null)
        { return; }
      //  Debug.LogError("RightOnUp");
        _playItem.RightOnUp();
    }

    private void RightOnDown(PointerEventData eventData)
    {
        if (_playItem == null)
        { return; }
      //  Debug.LogError("RightOnDown");
        _playItem.RightOnDown();
    }
    private void LeftOnUp(PointerEventData eventData)
    {
        if (_playItem == null)
        { return; }
      //  Debug.LogError("LeftOnUp");
        _playItem.LeftOnUp();
    }

    private void LeftOnDown(PointerEventData eventData)
    {
        if (_playItem == null)
        { return; }
        //Debug.LogError("LeftOnDown");
        _playItem.LeftOnDown();
    }



    private GameObject GetEmptyItem()
    {
        foreach(var v in _ItemPool)
        {
            if (v.activeSelf == false)
                return v; 
        }
        return null;
    }

    public void SetPlay()
    {
        _playItem = CreateSpine(_spineIdArray[0], _animationArr[0]);
        CreateAirborneItem();
    }

    public void SetData()
    {
    }
    string[] _animationArr =
    {
        "01_dai_ji",
        "02_zou_lu",
        "03_hui_shou"
    };

    public void Play()
    {
        foreach (var v in _ItemPool)
        {
            if (v.activeSelf == true)
            {
                v.GetComponent<AirborneItem>().SetMoving = true;
            }
        }
        if (_playItem != null)
            _playItem.SetMoving = true;
    }
    public void Pause()
    {
        foreach (var v in _ItemPool)
        {
            if (v.activeSelf == true)
            {
                v.GetComponent<AirborneItem>().SetMoving = false;
            }
        }
        if (_playItem != null)
            _playItem.SetMoving = false;
    }

    private string[] _spineIdArray = { "nian_nian", "mm", "nan_nan", "shui_shou_fu", "mei_zhuang" };

    AirborneGamePlayItem _playItem;
    private AirborneGamePlayItem CreateSpine(string spineId, string animationName)
    {
        SkeletonGraphic skg = InstantiatePrefab("AirborneGame/Items/AirbornePlayer").GetComponent<SkeletonGraphic>();
        skg.transform.SetParent(_contains, false);

        AirborneGamePlayItem fansSpineItem = skg.gameObject.AddComponent<AirborneGamePlayItem>();
        fansSpineItem.Init(spineId, skg, animationName);
        fansSpineItem.transform.localPosition = new Vector3(0, -806, 0);
        fansSpineItem.name = "Player";
        return fansSpineItem;
    }

    public void SetItemDropDown(AirborneGameRunningItemVo vo)
    {
        var obj= GetEmptyItem();
        if (obj == null) 
        {
            Debug.LogError("need more lager pool");
            return;
        }
  
        int idx = UnityEngine.Random.Range(0, _startPoint.Count);
        Vector3 start = _startPoint[idx];   
        obj.GetComponent<AirborneItem>().SetData(vo, start);
        obj.SetActive(true);
   
       obj.transform.SetAsLastSibling();
        if (vo.Itemtype == ItemTypeEnum.Dead) 
        {        
            ShowWarining(idx);
        }
    }

    private void ShowWarining(int idx)
    {
        string path = "V" + idx.ToString();
        Debug.Log("ShowWarining" + idx);
        _contains.Find(path).gameObject.SetActive(true);
    }

    private void CreateAirborneItem()
    {
        GameObject go = GetPrefab("AirborneGame/Items/AirborneItem");
        for(int i=0;i< _ItemPoolCount;i++)
        {
            GameObject item = Instantiate(go);
            item.transform.SetParent(_contains, false);
            item.transform.localPosition = new Vector3(0,1200,0);
            item.name = "AirborneItem";
            item.SetActive(false);
            _ItemPool.Add(item);
        }
    }

    public void ShowAddTime()
    {
        ShowTextAni();
    }

    private void ShowTextAni(float duration = 0.5f)
    {
        Image img = transform.Find("AddTime").GetComponent<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
        img.gameObject.SetActive(true);


        img.transform.localPosition = new Vector3(img.transform.localPosition.x, 50);

        Tweener move1 = img.transform.DOLocalMoveY(100, 0.3f).SetEase(DG.Tweening.Ease.OutSine);
        Tweener move2 = img.transform.DOLocalMoveY(200, 0.3f).SetEase(DG.Tweening.Ease.OutSine);

        Tweener alpha1 = img.DOColor(new Color(img.color.r, img.color.g, img.color.b, 0), 0.3f);


        var tween = DOTween.Sequence()
                  .Append(move1)
                  .AppendInterval(duration)
                  .Append(move2)
                  .Join(alpha1);
        tween.OnComplete(TweenOver);//动画完成的回调
    }

    private void TweenOver()
    {
        _addTimeImg.transform.DOKill();
        _addTimeImg.gameObject.SetActive(false);
    }

}
