using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using BehaviorDesigner.Runtime;
using game.tools;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Random = UnityEngine.Random;

public class VisitFansSpineItem : MonoBehaviour,IComparable<VisitFansSpineItem>
{
    public int CompareTo(VisitFansSpineItem item)
    {
        return transform.localPosition.y.CompareTo(item.transform.localPosition.y);
    }

    private RectTransform _rect;
    private SkeletonGraphic _skg;
    private string _animationName;
    public void Init(string spineId, SkeletonGraphic skg, string animationName)
    {
        _skg = skg;
        _animationName = animationName;

        SkeletonDataAsset skData = SpineUtil.BuildSkeletonDataAsset(spineId, skg);
        skg.skeletonDataAsset = skData;
        skg.Initialize(true);
        skg.AnimationState.SetAnimation(0, animationName, true);

        _rect = skg.GetComponent<RectTransform>();
        _rect.pivot = new Vector2(0.5f, 0f);
        _rect.anchorMin = new Vector2(0.5f, 0.5f);
        _rect.anchorMax = new Vector2(0.5f, 0.5f);
        _rect.sizeDelta = new Vector2(300, 500);
        _rect.localScale = new Vector3(0.5f, 0.5f, 1);

        moveTowardPosition = transform.localPosition;

        _paths = new List<Vector3>();
    }

    FunsState _curState = FunsState.None;
    public FunsState CurState
    {
        get
        {
            return _curState;
        }
        set
        {
            if (_curState == value)
                return;
            _curState = value;
            int cur = (int)_curState;
            _bt.SetVariableValue("CurState", cur);
            if (_curState == FunsState.None)
            {
                //todo
            }
        }
    }

    private BehaviorTree _bt;
    public void InitBehaviorTree()
    {
        if (_bt != null)
        {
            return;
        }
        //var bt = gameObject.GetComponent<BehaviorTree>();
        var bt = gameObject.AddComponent<BehaviorTree>();
        //string pathPre = "VisitBattle/BehaviourTree/VisitBattleFuns1.asset";
        string pathPre = "VisitBattle/BehaviourTree/VisitBattleFuns";
        //var extBt  = ResourceManager.Load<ExternalBehaviorTree>(pathPre, ModuleConfig.MODULE_VISITBATTLE);
        var extBt = ResourceManager.Load<ExternalBehaviorTree>(pathPre, ModuleConfig.MODULE_VISITBATTLE);

        bt.RestartWhenComplete = true;
        bt.StartWhenEnabled = false;
        bt.ExternalBehavior = extBt;
        bt.EnableBehavior();
        _bt = bt;
        CurState = FunsState.None;
    }

    public void SetPath(List<Vector3> paths)
    {
        _paths = paths;
        //Vector3 offset = new Vector3(
        //    Random.Range(-250, 250),
        // Random.Range(-250, 250), 0);
        //_paths[_paths.Count - 1] = _paths[_paths.Count - 1] + offset;
    }

    List<Vector3> _paths;
    int _index = 0;
    public void Walk(Vector3[] paths)
    {

    }
    bool isMove = false;

    public void StartWaiting()
    {
        _skg.AnimationState.SetAnimation(0, "01_dai_ji", true);
        _skg.AnimationState.TimeScale = 1;
    }

    public bool DoWaiting()
    {
        return false;
    }




    private void Reversal(bool isFront = false)
    {
        if (isFront)
        {
            _rect.localEulerAngles = new Vector3(0, 0, 0);
        //    _rect.Rotate(0, 0, 0);
        }
        else
        {
            //    _rect.Rotate(0, 180, 0);
            _rect.localEulerAngles = new Vector3(0, 180, 0);
        }
    }
    public bool StartWaveing()
    {
        _skg.AnimationState.SetAnimation(0, "03_hui_shou", true);
        _skg.AnimationState.TimeScale = 1;
        return false;
    }
    public bool DoWaveing()
    {
        return false;
    }

    public void StartMoving()
    {
        _index = 1;
        _skg.AnimationState.SetAnimation(0, "02_zou_lu", true);
        _skg.AnimationState.TimeScale = 2;
        //moveTowardPosition = _paths[1];
        moveTowardPosition= transform.localPosition;
    }


    public bool DoMoving()
    {
        //1、获得当前位置
        Vector3 curenPosition = transform.localPosition;
        //2、获得方向
        if (Vector3.Distance(curenPosition, moveTowardPosition) < 0.01f)
        {
            transform.localPosition = moveTowardPosition;
          
            if (_index < _paths.Count)
            {
                moveTowardPosition = _paths[_index];

                bool front = curenPosition.x - moveTowardPosition.x < 0;
                Reversal(front);
                _index++;
            }
            else
            {
                return true;
            }
        }
        else
        {
            //3、插值移动
            //距离就等于 间隔时间乘以速度即可
            float maxDistanceDelta = Time.deltaTime * moveSpeed;
            transform.localPosition = Vector3.MoveTowards(curenPosition, moveTowardPosition, maxDistanceDelta);
        }

        return false;
    }

    public float moveSpeed = 500f;
    Vector3 moveTowardPosition;
    public void WaveHand()
    {
        CurState = FunsState.Wave;
    }

    public void Move()
    {
        CurState = FunsState.Move;
    }
    public void Idle()
    {
        CurState = FunsState.Idel;
    }

}
