using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game.tools;
using System;
using Com.Proto;

public class AirborneGamePlayItem : MonoBehaviour
{
    public bool SetMoving
    {
        set
        {
            _isMoving = value;
        }
        get
        {
            return _isMoving;
        }
    }
    bool _isMoving = false;
    string[] _animationArr =
{
        "01_dai_ji",
        "02_zou_lu",
        "03_hui_shou"
    };
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
        CurDerection = 0;
    }



    float _curDerection;
    float CurDerection
    {
        set
        {
            if (_curDerection == value)
                return;

            _curDerection = value;
            if (_curDerection == 0)
            {
                SetIdleAnimation();
            }
            else if (_curDerection == 1)
            {
                Reversal(true);
                SetMovingAnimation();
            }
            else if (_curDerection == -1)
            {
                Reversal(false);
                SetMovingAnimation();
            }
        }
        get
        {
            return _curDerection;
        }

    }

    bool isMoving = false;
    bool isKeyDownA = false;
    bool isKeyDownD = false;
    private void FixedUpdate()
    {
        if (!_isMoving)
            return;

        //isKeyDownA = false;
        //isKeyDownD = false;
        //if (Input.GetKey((KeyCode.A)))
        //{
        //    isKeyDownA = true;
        //}
        //else if (Input.GetKeyDown(KeyCode.A))
        //{
        //    isKeyDownA = true;
        //}
        //else if (Input.GetKeyUp(KeyCode.A))
        //{
        //}

        //if (Input.GetKey((KeyCode.D)))
        //{
        //    isKeyDownD = true;
        //}
        //else if (Input.GetKeyDown(KeyCode.D))
        //{
        //    isKeyDownD = true;
        //}
        //else if (Input.GetKeyUp(KeyCode.D))
        //{
        //}

        if (isKeyDownA&& isKeyDownD)
        {
            DoMoving(CurDerection);
        }else  if(isKeyDownA)
        {
            CurDerection = -1;
            DoMoving(CurDerection);
        }
        else if(isKeyDownD)
        {
            CurDerection = 1;
            DoMoving(CurDerection);
        }
        else
        {
            CurDerection = 0;
            SetIdleAnimation();
        }
    }

    public void RightOnUp()
    {
        isKeyDownD = false;
    }
    public void LeftOnUp()
    {
        isKeyDownA = false;
    }
    public void RightOnDown()
    {
        isKeyDownD = true;
    }
    public void LeftOnDown()
    {
        isKeyDownA = true;
    }

    private void Reversal(bool isFront = false)
    {
        if (isFront)
        {
            _rect.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            _rect.localEulerAngles = new Vector3(0, 180, 0);
        }
    }

    float speed = 450.0f;

    void DoMoving(float direction)
    {
        float dis = direction * speed * Time.deltaTime;
        float newX = transform.localPosition.x + dis;
        if(newX>500||newX<-500)
        {
            return; 
        }
        transform.localPosition = new Vector3(
             newX,
             transform.localPosition.y,
             transform.localPosition.z            
             );
    }
    void SetMovingAnimation()
    {
        _skg.AnimationState.SetAnimation(0, _animationArr[1], true);
    }
    void SetIdleAnimation()
    {
        _skg.AnimationState.SetAnimation(0, _animationArr[0], true);
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.name == "AirborneItem")
        {
            AirborneItem item = c.gameObject.GetComponent<AirborneItem>();
            switch (item.ItemType)
            {
                case ItemTypeEnum.Dead:
                    Dead();
                    break;
            }
        }
    }


    void Dead()
    {
        transform.Find("Bang").gameObject.Show();
    }
}
