using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiaryElementBase : MonoBehaviour {
    protected GameObject _rotation;
    protected GameObject _scale;
    protected GameObject _delete;
    protected GameObject _edit;
    protected GameObject _outline;
    protected RectTransform _rect;
    protected DiaryElementPB pb;
    protected Vector2 _originalSize;
    protected bool _isRotation=false;
    protected bool _isDelete=false;
    protected bool _isEdit=false;
    protected bool _isScale=false;
    protected bool _isMove=false;
    protected bool _isOutline = false;
    protected bool _isSelectAndUniformScale=false;//等比缩放

    protected bool _isNeedtoHideInput=false;
    private bool _hiding=false;

    private RectTransform _container;

    private float MaxScale = 1.5f;
    private float MinScale = 0.5f;
   

    LoveDiaryEditType _loveDiaryEditType;
    public LoveDiaryEditType loveDiaryEditType {
        set {
            if (value == _loveDiaryEditType)
                return;
            _loveDiaryEditType = value;
//            Debug.LogError(this.name+" "+loveDiaryEditType);
            SetEditShow();
        }
        get {
            return _loveDiaryEditType;
        }
    }

    void SetEditShow()
    {
        bool isShow = _loveDiaryEditType == LoveDiaryEditType.Edit;
        if (_isRotation)
        {
            _rotation.SetActive(isShow);
        }
        if (_isEdit)
        {
            _edit.SetActive(isShow);

        }
        if(_isScale)
        {
            _scale.SetActive(isShow);
        }
        if(_isDelete)
        {
            _delete.SetActive(isShow);
        }
        if (_isMove)
        {
            if(isShow==true)
            {
                UIEventListener.Get(gameObject).onDrag = OnMove;
            }
            else
            {
                UIEventListener.Get(gameObject).onDrag = null;
            }
        }

        if(_isOutline)
        {
            _outline.SetActive(isShow);
        }
    }

    private void Awake()
    {
        Init();
    }
    protected  virtual void Init()
    {
        if (_isDelete)
        {
            _delete = transform.Find("Delete").gameObject;
            UIEventListener.Get(_delete.gameObject).onClick = OnDelete;
        }
        if (_isEdit)
        {
            _edit = transform.Find("Edit").gameObject;
            UIEventListener.Get(_edit).onClick = OnEditClick;
        }
        if (_isRotation)
        {
            _rotation = transform.Find("Rotation").gameObject;
            UIEventListener.Get(_rotation).onBeginDrag = OnBeginDrag;
            UIEventListener.Get(_rotation).onEndDrag = OnEndDrag;
            UIEventListener.Get(_rotation).onDrag = OnDrag;
        }
        if (_isScale)
        {
            _scale = transform.Find("Scale").gameObject;
            UIEventListener.Get(_scale).onDown = OnScaleBeginDrag;
            UIEventListener.Get(_scale).onUp = OnScaleEndDrag;
            UIEventListener.Get(_scale).onDrag = OnScaleDrag;
        }
        if (_isMove)
        {
            UIEventListener.Get(gameObject).onDrag = OnMove;
            UIEventListener.Get(gameObject).onEndDrag = OnEndDrag;
        }
        _rect = transform.GetComponent<RectTransform>();
        _outline = transform.Find("Rect").gameObject;
    }

    private void OnEditClick(GameObject go)
    {
        EventDispatcher.TriggerEvent(EventConst.LoveDiaryEditItemText);
    }

    private void OnDelete(GameObject go)
    {
        EventDispatcher.TriggerEvent(EventConst.LoveDiaryEditDeleteElement, pb);
        DestroyImmediate(gameObject);
    }

    public void SetData(DiaryElementPB diaryElementPB,RectTransform container)
    {
        _container = container;
        pb = diaryElementPB;
        Debug.LogError(pb);
        UpdateView();
        loveDiaryEditType = LoveDiaryEditType.Show;
    }
    protected virtual void UpdateView()
    {
    }

    public DiaryElementPB GetDiaryElementData()
    {
        return pb;
    }

    protected void UpdatePosData()
    {
        pb.XPos = _rect.localPosition.x;
        pb.YPos = _rect.localPosition.y;
        EventDispatcher.TriggerEvent(EventConst.LoveDiaryElementModify);
    }
    protected void UpdateScaleData()
    {
        pb.ScaleX = _rect.GetWidth() / _originalSize.x;
        pb.ScaleY = _rect.GetHeight() / _originalSize.y;
        UpdatePosData();
    }
    protected void UpdateRotationData()
    {
        pb.Rotation = transform.eulerAngles.z;
        EventDispatcher.TriggerEvent(EventConst.LoveDiaryElementModify);
    }

    private void OnMove(PointerEventData eventData)
    {

        Vector3 pos1 = Camera.main.ScreenToWorldPoint(eventData.position);
        if (!ChechIsEditRange(pos1))
        {
            return;
        }
        transform.position = new Vector3(pos1.x, pos1.y, transform.position.z);
        UpdatePosData();
        
        if (_isNeedtoHideInput)
        {

            if (!_hiding)
            {
                _hiding = true;
                EventDispatcher.TriggerEvent<bool>(EventConst.LoveDiaryHideEditText, false); 
            }

        }
    }

    private bool ChechIsEditRange(Vector3 position)
    {
        Vector2 size = _container.GetSize();
        Vector3[] fourCornersArray = new Vector3[4];
        _container.GetWorldCorners(fourCornersArray);

        Debug.Log("click  " + position + "  container " + _container.position + "  size " + size);
        if (position.x < fourCornersArray[0].x || position.y < fourCornersArray[0].y) 
            return false;
        if (position.x > fourCornersArray[2].x || position.y > fourCornersArray[2].y) 
            return false;
        return true;
    }

    private Vector2 GetNewSize(float rectAngle, Vector2 passPos, Vector2 clickPos)
    {

        float angle1 = rectAngle * Mathf.Deg2Rad;
        float angle2 = (rectAngle + 270) * Mathf.Deg2Rad;

        float d1 = Mathf.Sin(angle1) * (clickPos.x - passPos.x) - Mathf.Cos(angle1) * (clickPos.y - passPos.y);
        float d2 = Mathf.Sin(angle2) * (clickPos.x - passPos.x) - Mathf.Cos(angle2) * (clickPos.y - passPos.y);

        return new Vector2(Mathf.Abs(d2), Mathf.Abs(d1));
    }


    protected virtual void OnScaleDrag(PointerEventData eventData)
    {
        //修改size
        RectTransform rect = transform.GetComponent<RectTransform>();
        // rect.SetSize(rect.GetSize() + new Vector2(5, 5));
        //判定尺寸
        Vector3 pos1 = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 pos2 = eventData.position;
        Vector2 newSize = GetNewSize(rect.eulerAngles.z, pos1, pos2);

        if(_isSelectAndUniformScale)//是否等比縮放
        {
            float scaleX = newSize.x / _originalSize.x;
            float scaleY = newSize.y / _originalSize.y;
            float maxScale = Mathf.Max(scaleX, scaleY);

            maxScale = Mathf.Max(maxScale, MinScale );
            maxScale = Mathf.Min(maxScale, MaxScale);

            newSize = new Vector2(_originalSize.x * maxScale, _originalSize.y * maxScale); 
        }

        if (newSize.x<=50)
        {
             return;
        }
        
        
        rect.SetSize(newSize);
    }


    private void OnScaleEndDrag(PointerEventData eventData)
    {
        RectTransform rect = transform.GetComponent<RectTransform>();
        SetPivotAndOffset(ref rect, new Vector2(0.5f, 0.5f));
        UpdateScaleData();

    }

    private void OnScaleBeginDrag(PointerEventData eventData)
    {
        //todo 设置中心点和坐标
        RectTransform rect = transform.GetComponent<RectTransform>();
        SetPivotAndOffset(ref rect, new Vector2(0, 1));
    }

    /// <summary>
    /// 修改中心点并调整偏移
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="newPivot"></param>
    private static void SetPivotAndOffset(ref RectTransform rect, Vector2 newPivot)
    {
        Vector2 oldPivot = rect.pivot;
        rect.pivot = newPivot;
        //变换坐标后的偏移
        Vector2 offset = newPivot - oldPivot;
        offset = offset * rect.GetSize();
        Vector2 centerPos = new Vector2(0, 0);
        float angle = rect.localEulerAngles.z;
        Vector2 offsetNew = PointRotateAroundPointByAngle(offset, centerPos, angle * Mathf.Deg2Rad);
        float offsetX =offsetNew.x;
        float offsetY = offsetNew.y;
        Vector3 pos = rect.transform.localPosition;
        Vector3 pos1 = new Vector3(pos.x + offsetX, pos.y + offsetY, pos.z);
        rect.transform.localPosition = pos1;
    }

    /// <summary>
    /// 二维坐标系，一个点绕任意点旋转angle度后的点的坐标
    /// </summary>
    /// <param name="vector2"></param>
    /// <param name="center"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    private static Vector2 PointRotateAroundPointByAngle(Vector2 vector2, Vector2 center, float angle)
    {
        Vector2 newVector2 = new Vector2();
        newVector2.x = (vector2.x - center.x) * Mathf.Cos(angle) - (vector2.y - center.y) * Mathf.Sin(angle) + center.x;
        newVector2.y = (vector2.x - center.x) * Mathf.Sin(angle) + (vector2.y - center.y) * Mathf.Cos(angle) + center.y;
        return newVector2;
    }

    Vector3 eulerAngles;
    Vector2 beginDragPos;
    Vector2 centerPos;

    private void OnEndDrag(PointerEventData eventData)
    {
//        Debug.LogError("OnEndDrag " + eventData);
        //todo  保存参数
//        Debug.LogError(transform.eulerAngles);
        UpdateRotationData();
        if (_isNeedtoHideInput)
        {
            if (_hiding)
            {
                _hiding = false;
                EventDispatcher.TriggerEvent<bool>(EventConst.LoveDiaryHideEditText,true); 
            }

        }
    }

    private void OnDrag(PointerEventData eventData)
    {
        //Debug.LogError("OnDrag " + eventData);
        Vector2 dir1 = beginDragPos - centerPos;
        Vector2 dir2 = eventData.position - centerPos;
        float angle = Vector3.Angle(dir1, dir2);//求夹角
        Vector3 normal = Vector3.Cross(dir1, dir2);//叉乘求出法线向量
        angle *= Mathf.Sign(normal.z);  //求法线向量与物体上方向向量点乘，结果为1或-1，修正旋转方向
        transform.eulerAngles = eulerAngles + new Vector3(0, 0, angle);
    }
    private void OnBeginDrag(PointerEventData eventData)
    {
        beginDragPos = eventData.position;
        centerPos = Camera.main.WorldToScreenPoint(transform.position);
        eulerAngles = transform.eulerAngles;//欧拉角 0-360
        Debug.Log(eulerAngles);
        if (_isNeedtoHideInput)
        {

            if (!_hiding)
            {
                _hiding = true;
                EventDispatcher.TriggerEvent<bool>(EventConst.LoveDiaryHideEditText, false); 
            }

        }
    }

    
}
