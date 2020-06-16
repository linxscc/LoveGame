using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class DatePickerComponent : MonoBehaviour
{
    Transform _content;
    RectTransform _contentRect;
    List<RectTransform> _items;

    //int first = 0;
    private void Awake()
    {
        _content = transform.Find("MonthContainer/Content");
        _contentRect = _content.GetComponent<RectTransform>();
        _items = new List<RectTransform>();
        for (int i = 0; i < _content.childCount; i++)
        {
            _items.Add(_content.GetChild(i).GetComponent<RectTransform>());
        }

        UIEventListener.Get(_content.gameObject).onBeginDrag = onBeginDrag;
        UIEventListener.Get(_content.gameObject).onEndDrag = onEndDrag;
        UIEventListener.Get(_content.gameObject).onDrag = onDrag;
    }

    bool isDrag = false;
    private void onDrag(PointerEventData eventData)
    {
    //    Debug.LogError("onDrag "+ eventData.position);
        float YYY = ScreenToLocalY(eventData.position);
        preDragTime = Time.time;
        dragOffsetY = YYY - preDragPosY;
        preDragPosY = YYY;
        preBeginPosY = eventData.position.y;
    }


    public void Refreash()
    {
        InitItem();
    }

    public void Refill()
    {
        Debug.LogError("Refill " + curShowIndex + " Count " + Count);
        if(curShowIndex> Count - 1)
        {
            curShowIndex = curShowIndex % Count;
        }

        for (int i = 0; i < _items.Count; i++)
        {
            int index = (listFirst + i)% _items.Count;
            var item = _items[index];
            UpdateCallBack(item.gameObject, curShowIndex - 2 + i);
        }

        cursorCallBack(curShowIndex);

    }

    private  float ScreenToLocalY(Vector2 pos)
    {
        Vector3 pos1 = Camera.main.ScreenToWorldPoint(pos);
         return _content.InverseTransformPoint(pos1).y;
    }

     float dffDic=0;
     float maxSpeed = 0;
    private void onEndDrag(PointerEventData eventData)
    {
        //preOffY = eventData.pressPosition.y;
       // Debug.LogError("onEndDrag   " + eventData.position.y);
        float dff = eventData.position.y - preBeginPosY;
        float dffTime = Time.time - preDragTime;
        speed = (dff* dffDic)/ dffTime;
        if(Mathf.Abs(speed)> maxSpeed)
        {
            speed = Mathf.Sign(speed) * maxSpeed;
        }

        isBack = false;
        isDrag = false;
        isFinshedMove = false;
    }
    float preDragTime = 0;
    float preBeginPosY = 0;
    float preDragPosY = 0;
    float dragOffsetY = 0;
    private void onBeginDrag(PointerEventData eventData)
    {
      //  Debug.LogError("onBeginDrag   " + eventData.position.y);
        preDragPosY = ScreenToLocalY(eventData.position);
        isDrag = true;
    }
    List<int> month;
    public int Count;
    private void Start()
    {
    }
    

    private void InitItem()
    {
        for (int i = 0; i < _content.childCount; i++) 
        {
            GameObject obj = _content.GetChild(i).gameObject;
            int idx =(i-2- curShowIndex + Count) % Count;
            UpdateCallBack(obj, idx);
        }
    }


    public Action<int> cursorCallBack;
    public Action<GameObject, int> updateCallBack;
    public Action<int> finishedMoveCallBack;
    private void UpdateCallBack(GameObject obj,int index)
    {
        index = (index+Count) % Count;
       // Debug.LogError(index);
       // obj.transform.GetText("Text").text = index.ToString();
        updateCallBack?.Invoke(obj, index);
    }

    public float acc = 400;
    float speed = 0;
    float middlePosY = 0;

    int middleIndex
    {
        get
        {
            return  (listFirst + 2) % _items.Count;
        }
    }
    int curShowIndex = 0;
    float returnDelta = 0.5f;
    float returnAcc = 800;
    float returnSpeed = 200;
    bool isBack = false;
    float childsize = 100;
    int childCount = 5;
    float range = 250;

    int listFirst = 0;
    private void SetChildrenItemPosY(float OffsetY)
    {
        if (OffsetY == 0)
            return;
        int moveCount = 0;

        if(OffsetY>0)
        {
            for(int i=0;i<_items.Count;i++)
            {
                int idx = (i + listFirst) % _items.Count;
                RectTransform rect = _items[idx];
                float y = rect.localPosition.y + OffsetY;
                if (y > 300)
                {
                    moveCount++;
                    y = y - 2 * range;
                    UpdateCallBack(rect.gameObject, curShowIndex + 3);
                    curShowIndex++;
                    curShowIndex = (curShowIndex + Count) % Count;
                    cursorCallBack(curShowIndex);
                }
                rect.SetPositionOfPivot(new Vector2(rect.localPosition.x, y));
            }
        }
        else
        {
            for (int i = _items.Count-1; i > -1; i--)
            {
                int idx = (i + listFirst) % _items.Count;
                RectTransform rect = _items[idx];
                float y = rect.localPosition.y + OffsetY;
                if (y <- 300)
                {
                    moveCount++;
                    y = y + 2 * range;
                    //rect.SetAsFirstSibling();
                    UpdateCallBack(rect.gameObject, curShowIndex - 3);
                    curShowIndex--;
                    curShowIndex = (curShowIndex + Count) % Count;
                    cursorCallBack(curShowIndex);
                }
                rect.SetPositionOfPivot(new Vector2(rect.localPosition.x, y));
            }

        }

        listFirst = OffsetY > 0 ? listFirst + moveCount : listFirst - moveCount;
        listFirst = (listFirst + _items.Count) % _items.Count;
    }


    bool isFinshedMove = true;

    private void Update()
    {
        if(isDrag)
        {
            SetChildrenItemPosY(dragOffsetY);
            dragOffsetY = 0;
            return;
        }

        //if (!isBack)//惯性滑动
        //{
        //    float offSetY = 0.5f * speed * Time.deltaTime;
        //    SetChildrenItemPosY(offSetY);
        //    if (speed > 0)
        //    {
        //        speed = speed - Time.deltaTime * acc;
        //        if (speed <= 0)
        //        {
        //            isBack = true;
        //            speed = 0;
        //        }
        //    }
        //    else
        //    {
        //        speed = speed + Time.deltaTime * acc;
        //        if (speed >= 0)
        //        {
        //            isBack = true;
        //            speed = 0;
        //        }
        //    }
        //}
        //else
        //{
            //middleIndex = (listFirst + 2) % _items.Count;
            RectTransform middlerect = _content.GetChild(middleIndex).GetComponent<RectTransform>();
            if (middlerect.localPosition.y==0)
            {
                if(!isFinshedMove)
                {
                    isFinshedMove = true;
                    finishedMoveCallBack?.Invoke(curShowIndex);
                }
                
                return;
            }

            float oldY = middlerect.localPosition.y;
            float sign = Mathf.Sign(oldY);

            float offsetY = -sign * returnSpeed * Time.deltaTime;
            float newY = oldY + offsetY;
            if (oldY * newY <= 0)
            {
                offsetY = -oldY;
            }

            foreach (var v in _items)
            {
                RectTransform rect = v;
                float y = rect.localPosition.y + offsetY;
                if (y > range)
                {
                    y = y - 2 * range;
                    // rect.SetAsLastSibling();
                    //UpdateCallBack(rect.gameObject, curShowIndex + 3);
                    //curShowIndex++;
                    //curShowIndex = (curShowIndex + Count) % Count;
                    //cursorCallBack(curShowIndex);
                }
                else if (y < -range)
                {
                    y = y + 2 * range;
                    // rect.SetAsFirstSibling();
                    //UpdateCallBack(rect.gameObject, curShowIndex - 3);
                    //curShowIndex--;
                    //curShowIndex = (curShowIndex + Count) % Count;
                    //cursorCallBack(curShowIndex);
                }
                rect.SetPositionOfPivot(new Vector2(rect.localPosition.x, y));
            }
           // SetChildrenItemPosY(offsetY);          
       // }
    }
}