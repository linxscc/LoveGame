using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using Module.VisitBattle.Data;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class VisitBattleView : View
{
    private int _step;

    private RectTransform _point1;
    private RectTransform _point2;
    private RectTransform _point3;
    private RectTransform _map;

    private float _scaleFactor;
    private float _offsetX;
    private float _offsetY;

    private RectTransform _bus;
    private RawImage _tvImage;
    private Animator _abilityAnimator;
    private RawImage _goodsImage;
    private RectTransform _spineContainer;

    private string[] _spineIdArray = { "nian_nian", "mm", "nan_nan", "shui_shou_fu", "mei_zhuang" };

    private bool _isLeft = false;

    private Transform _battleCommon;
  //  private List<FansSpineItem> _spineList;

    private List<VisitFansSpineItem> _spineFansList;//第一个点的粉丝
    private List<VisitFansSpineItem> _spineFansList2;//第二个点的粉丝
    private List<VisitFansSpineItem> _spineAllFansList;//第一个点的粉丝
    private RectTransform _spineContainer2;

    private void Awake()
    {
        _abilityAnimator = transform.Find("AbilityAnimation").GetComponent<Animator>();

        _point1 = transform.GetRectTransform("Map/Point1");
        _point2 = transform.GetRectTransform("Map/Point2");
        _point3 = transform.GetRectTransform("Map/Point3");

        _battleCommon.DOLocalMove(new Vector3(0, 1, 0), 1);

        _spineContainer = transform.GetRectTransform("Map/SpineContainer");
        _spineContainer2 = transform.GetRectTransform("Map/SpineContainer2");
        _bus = transform.GetRectTransform("Map/Bus");

        _tvImage = transform.GetRawImage("Map/TV");
        _tvImage.gameObject.Hide();

        _goodsImage = transform.GetRawImage("Map/Goods");
        _goodsImage.gameObject.Hide();

        _step = 0;

        _map = transform.GetRectTransform("Map");

        _scaleFactor = Main.ScaleX * Main.CanvasScaleFactor;

        _offsetX = Main.StageWidth / _scaleFactor - Main.StageWidth;
        _offsetY = Main.StageHeight / (Main.ScaleY * Main.CanvasScaleFactor) - Main.StageHeight;
        _spineAllFansList = new List<VisitFansSpineItem>();
        _spineFansList = new List<VisitFansSpineItem>();
        _spineFansList2 = new List<VisitFansSpineItem>();
        Step1();

        LoadPaths();
    }

    private void Step1()
    {
        float x = _point1.anchoredPosition.x - _offsetX;

        _bus.DOAnchorPos(new Vector2(-228, -1674), 1);

        _map.DOAnchorPos(new Vector2(-x, 0), 1.2f)
            .onComplete = () =>
            {
                _step++;
                _abilityAnimator.gameObject.Show();

                _abilityAnimator.Play("Normal", 0, 0);

                ClientTimer.Instance.DelayCall(Step2, 1.6f);
            };
    }

    GameObject itemOb = null;
    private void Start()
    {
        itemOb = GetPrefab("Spine/SpineSkeletonGraphic");
        _battleCommon = transform.parent.Find("VisitBattleCommon(Clone)");
        var smallStar = _battleCommon.Find("PowerBar/ProgressBar/Bar/smallStar").GetComponent<RectTransform>();
        var bigStar = _battleCommon.Find("PowerBar/ProgressBar/Bar/bigStar").GetComponent<RectTransform>();
        StartCoroutine(StarRotation(smallStar, bigStar));
   
    }

    public void ResetView()
    {
        _goodsImage.gameObject.Hide();

        _spineContainer2.RemoveChildren();
        _map.anchoredPosition = Vector2.zero;
        _bus.anchoredPosition = new Vector2(263, -2040);
        _step = 0;
        ResetItems();
        Step1();
    }

    private void ResetItems()
    {
        foreach(var v  in _spineAllFansList)
        {
            v.Idle();
        }
        foreach(var v in _spineFansList)
        {
            Vector3 offset = new Vector3(Random.Range(-250, 250),
                   Random.Range(-250, 250), 0);
            v.GetComponent<RectTransform>().anchoredPosition = _paths[0] + offset;
        }
    }

    //加载路径
    List<Vector3> _paths;
    private void LoadPaths()
    {
        _paths = new List<Vector3>();
        Transform tf = transform.Find("Map/FansPath");
        for (int i = 0; i < tf.childCount; i++)
        {
            _paths.Add(tf.GetChild(i).localPosition);
        }
    }

    /// <summary>
    /// 广场上的粉丝动画
    /// </summary>
    /// <param name="moreFans"></param>
    public void LoadAnimation(bool moreFans)
    {
        CanvasGroup cg = _spineContainer.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.DOFade(1, 0.3f);

        Vector2[] positionArr =
        {
            new Vector2(-190, 383), //挥手
            new Vector2(-54, 363), //站 翻转

            new Vector2(-173, -128), //挥手
            new Vector2(-12, -150), //挥手 翻转
            new Vector2(-90, -285), //站

            new Vector2(-107, 645), //挥手

            new Vector2(400, 371), //站

            new Vector2(314, 921), //左右不停翻转 7

            new Vector2(423, 167), //站
     
            new Vector2(-326, -603), //来回走 index=9 
            new Vector2(-70, -454),

            new Vector2(-473, 906), //来回走 index=11
            new Vector2(144, 839),

            new Vector2(-264, 159), //来回走 index=13
            new Vector2(144, 184),
        };

        string[] animationArr =
        {
            "03_hui_shou",
            "01_dai_ji",

            "03_hui_shou",
            "03_hui_shou",
            "01_dai_ji",

            "03_hui_shou",

            "01_dai_ji",
            "01_dai_ji", //左右不停翻转

            "01_dai_ji",

            "02_zou_lu",
            "02_zou_lu",
            "02_zou_lu",
            "02_zou_lu",
            "02_zou_lu",
            "02_zou_lu",
        };


        bool[] reversalArr =
        {
            false,
            true,
            false,
            true,
            false,
            false,
            false,
            true,
            false,
            false,
            false,
            false,
            false,
            false,
        };

        List<string> spineIdList = new List<string>();
        int index = Random.Range(0, _spineIdArray.Length);
        spineIdList.Add(_spineIdArray[index]);
        spineIdList.AddRange(GetUnequalAnimationNames(2));
        spineIdList.AddRange(GetUnequalAnimationNames(3));

        for (int i = 6; i < positionArr.Length; i++)
        {
            index = Random.Range(0, _spineIdArray.Length);
            spineIdList.Add(_spineIdArray[index]);
        }

        _spineFansList.Clear();
        _spineAllFansList.Clear();
        for (int i = 0; i < 12; i++) 
         //   for (int i = 0; i < spineIdList.Count - 1; i++)
        {
            if (i == 10 || i == 12)
                continue;

            if (moreFans == false)
            {
                if (i == 2 || i == 3 || i == 5 || i == 6 || i == 11)
                    continue;
            }
            Vector3 offset = new Vector3(Random.Range(-250, 250),
                Random.Range(-250, 250), 0
                );

            VisitFansSpineItem item = CreateSpine(spineIdList[i], "01_dai_ji");
            item.GetComponent<RectTransform>().anchoredPosition = _paths[0]+ offset;
            _spineFansList.Add(item);
            _spineAllFansList.Add(item);
        }
    }

    public void LoadAnimation2()
    {
        Vector2[] positionArr =
        {
            new Vector2(-737, 1143),
            new Vector2(-709, 996),
            new Vector2(-429, 996),
            new Vector2(-296, 914),
            new Vector2(-238, 1013),
            new Vector2(-395, 1112),
            new Vector2(-614, 1232),
            new Vector2(-601, 1105),
            new Vector2(-952, 1105),
            new Vector2(-442, 931),
            new Vector2(-675, 948),
            new Vector2(-354, 1080),
            new Vector2(-553, 1024),
        };
        _spineFansList2.Clear();
        for (int i=0;i<8;i++)
        {
            int rand = Random.Range(0, _spineIdArray.Length);
            VisitFansSpineItem item = CreateSpine(_spineIdArray[rand], "01_dai_ji");

            int rand2 = Random.Range(0, positionArr.Length);
            item.GetComponent<RectTransform>().anchoredPosition =  positionArr[rand2];
            _spineFansList2.Add(item);
            _spineAllFansList.Add(item);
        }

    }

    public void FansHappy()
    {
        for (int i = 0; i < _spineAllFansList.Count; i++)
        {
            _spineAllFansList[i].WaveHand();
        }
    }

    private VisitFansSpineItem CreateSpine(string spineId, string animationName)
    {
        //SkeletonGraphic skg = InstantiatePrefab("Spine/SpineSkeletonGraphic").GetComponent<SkeletonGraphic>();
        SkeletonGraphic skg = Instantiate(itemOb).GetComponent<SkeletonGraphic>();

        skg.transform.SetParent(_spineContainer.transform, false);

        VisitFansSpineItem fansSpineItem = skg.gameObject.AddComponent<VisitFansSpineItem>();
        fansSpineItem.Init(spineId, skg, animationName);
        fansSpineItem.InitBehaviorTree();

        List<Vector3> path = new List<Vector3>();

        Vector2[] positionArr =
            {
            new Vector2(-737, 1143),
            new Vector2(-709, 996),
            new Vector2(-429, 996),
            new Vector2(-296, 914),
            new Vector2(-238, 1013),
            new Vector2(-395, 1112),
            new Vector2(-614, 1232),
            new Vector2(-601, 1105),
            new Vector2(-952, 1105),
            new Vector2(-442, 931),
            new Vector2(-675, 948),
            new Vector2(-354, 1080),
            new Vector2(-553, 1024),
        };
        path.AddRange(_paths);

        int rand = Random.Range(0, positionArr.Length);

        path[path.Count - 1] = positionArr[rand];

        fansSpineItem.SetPath(path);

        return fansSpineItem;
    }


    private List<string> GetUnequalAnimationNames(int num)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < _spineIdArray.Length; i++)
        {
            list.Add(i);
        }

        for (int i = 0; i < _spineIdArray.Length - num; i++)
        {
            int index = Random.Range(0, list.Count);
            list.RemoveAt(index);
        }

        List<string> retList = new List<string>();
        for (int i = 0; i < list.Count; i++)
        {
            retList.Add(_spineIdArray[list[i]]);
        }
        return retList;
    }

    private void Step2()
    {
        //应援会能力值加到进度条动画
        SendMessage(new Message(MessageConst.CMD_VISITBATTLE_SHOW_SUPPORTER_POWER));

        float x = _point2.anchoredPosition.x - _offsetX;
        float y = (_point2.anchoredPosition.y + Main.StageHeight) / 2 + _offsetY + ModuleManager.OffY;

        if (y > 0)
            y = 0;

        _map.DOAnchorPos(new Vector2(-x, y), 1).SetDelay(1.2f)
            .onComplete = () =>
            {
                //选择粉丝和应援物
                SendMessage(new Message(MessageConst.CMD_VISITBATTLE_SHOW_SUPPORTER_VIEW));
                _step++;
            };
    }

    private void Step3()
    {
        float x = _point3.anchoredPosition.x - _offsetX / 2;
        float y = _map.sizeDelta.y - Main.StageHeight - _offsetY - ModuleManager.OffY;

        CanvasGroup cg = _spineContainer2.GetComponent<CanvasGroup>();
        cg.alpha = 1;
        //        cg.DOFade(1, 1f).SetDelay(0.8f);
       // ClientTimer.Instance.DelayCall(LoadAnimation2, 0.32f);

        _map.DOAnchorPos(new Vector2(-x, -y), 4.0f)
            .onComplete = () =>
            {
                ClientTimer.Instance.DelayCall(() => { SendMessage(new Message(MessageConst.CMD_VISITBATTLE_NEXT)); }, 1);
            };
    }

    public void DoNext()
    {
        if (_step == 3)
        {
            //显示大屏幕粉丝的动画
            _tvImage.gameObject.Show();
            _tvImage.color = new Color(1, 1, 1, 0.3f);
            _tvImage.DOColor(Color.white, 0.8f).SetLoops(-1, LoopType.Yoyo);

            ClientTimer.Instance.DelayCall(() =>
            {
                SendMessage(new Message(MessageConst.CMD_VISITBATTLE_FANS_CALL_ANIMATION_FINISH));
            }, 3);

            _step++;
        }
    }

    public void InitData(VisitBattleModel model)
    {
        transform.GetText("AbilityAnimation/Active/Text").text = model.Active + "";
        transform.GetText("AbilityAnimation/Financial/Text").text = model.Financial + "";
        transform.GetText("AbilityAnimation/Resource/Text").text = model.Resource + "";
        transform.GetText("AbilityAnimation/Transmission/Text").text = model.Transmission + "";
    }

    IEnumerator StarRotation(RectTransform smallStar, RectTransform bigStar)
    {
        while (true)
        {
            smallStar.Rotate(-Vector3.forward * Time.deltaTime * 500.0f);
            bigStar.Rotate(-Vector3.forward * Time.deltaTime * 500.0f);
            yield return null;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _spineIdArray.Length; i++)
        {
            SpineUtil.UnloadLater(_spineIdArray[i]);
        }
    }

    public void ShowFans(bool moreFans, bool moreGoods)
    {
        if (moreGoods)
            _goodsImage.gameObject.Show();

       // LoadAnimation(moreFans);

        for(int i=0;i<5;i++)
        {
            var item= _spineFansList[i];
            //_spineFansList.Remove(item);
            //_spineFansList2.Add(item);
            item.Move();
        }
        _step++;
        ClientTimer.Instance.DelayCall(Step3, 0.3f);
    }

    public void FixedUpdate()
    {
        ItemBubbleSort();
        foreach (var v in _spineAllFansList)
        {
            v.gameObject.transform.SetAsFirstSibling();
        }
    }

    private void ItemBubbleSort()
    {

        bool flag = true;
        int length = _spineAllFansList.Count - 1;
        while (flag)
        {
            flag = false;
            for (int j = 0; j < length; j++)
            {
                if (_spineAllFansList[j].transform.position.y > _spineAllFansList[j + 1].transform.position.y)
                {
                    var temp = _spineAllFansList[j + 1];
                    _spineAllFansList[j + 1] = _spineAllFansList[j];
                    _spineAllFansList[j] = temp;
                    flag = true;
                }
            }
            length--;
        }

    }
}

