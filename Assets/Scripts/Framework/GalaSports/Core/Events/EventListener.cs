using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Framework.GalaSports.Core.Events { 
    //public class EventListener : UIBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler,
    public class EventListener : UIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler,
                                    IDropHandler, IScrollHandler, ISelectHandler, IDeselectHandler
    {
        public delegate void VoidDelegate(GameObject go);

        public delegate void EventDelegate(PointerEventData dt);

        public delegate void BaseEventDelegate(BaseEventData dt);

        public delegate void FloatDelegate(GameObject go, float fValue);

        public VoidDelegate onClick;

        public VoidDelegate onEnter;

        public VoidDelegate onExit;

        public EventDelegate onDown;

        public EventDelegate onUp;

        public EventDelegate onDrag;

        public EventDelegate onBeginDrag;

        public EventDelegate onEndDrag;

        public VoidDelegate onRepeat;

        public EventDelegate onDrop;

        public EventDelegate onScroll;

        public BaseEventDelegate onSelect;

        public BaseEventDelegate onDeselect;

        private int holdId = -1;

        private bool IsRepeat = true;

        private bool IsRepeating = false;

        //-------3dTouch 
        //按下力度1
        public VoidDelegate onTouchPressureLv1;
        //按下力度2
        public VoidDelegate onTouchPressureLv2;
        //按下回调
        public FloatDelegate onTouchPressure;
        //取消按下
        public VoidDelegate onEndTouchPressure;
        //是否触发了按下事件（力度大于一定值才触发）
        private int _3dTouchPressureState = -1;

        public const int PRESSURE_LV_1 = 5;

        public const int PRESSURE_LV_2 = 10;

        //------------------------------------------
        //public VoidDelegate onTouchTimeLv1;

        //public VoidDelegate onTouchTimeLv2;

        //public VoidDelegate onTouchTimeEnd;

        //private int _touchTimeState = -1;

        //public const float TOUCH_TIME_LV_1 = 1;

        //public const float TOUCH_TIME_LV_2 = 3;

        //private float _touchPassTime = 0;


        //-------------------------------------------

        static public EventListener Get(GameObject go)
        {
            EventListener listener = go.GetComponent<EventListener>();
            if (listener == null) listener = go.AddComponent<EventListener>();
            return listener;
        }

        static public void Remove(GameObject go)
        {
            EventListener listner = go.GetComponent<EventListener>();
            if (listner != null)
            {
                Destroy(listner);
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (onClick != null)
            {
                //Debug.Log(gameObject.name);
                onClick(gameObject);
            }
        }

        //void IDragHandler.OnDrag(PointerEventData eventData)
        //{
        //    if (onDrag != null)
        //        onDrag(eventData);
        //}

        //void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        //{
        //    if (onBeginDrag != null)
        //        onBeginDrag(eventData);
        //}

        //void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        //{
        //    if (onEndDrag != null)
        //        onEndDrag(eventData);
        //}

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null)
                onDown(eventData);
            if (onRepeat != null)
            {

            }
            _3dTouchPressureState = 0;

            //_touchPassTime = 0;
            //_touchTimeState = 0;
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null)
                onUp(eventData);
            if (this.IsRepeating == true)
            {

            }


            ////if (onTouchTimeEnd != null && _touchTimeState > 0)
            ////{
            ////    onTouchTimeEnd(gameObject);
            ////}
            //if (onEndTouchPressure != null && _touchTimeState > 0)
            //{
            //    onEndTouchPressure(gameObject);
            //}
            //_touchPassTime = 0;
            //_touchTimeState = -1;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null)
                onEnter(gameObject);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null)
                onExit(gameObject);
            if (this.IsRepeating == true)
            {

            }
        }

        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            if (onDrop != null)
                onDrop(eventData);
        }

        void IScrollHandler.OnScroll(PointerEventData eventData)
        {
            if (onScroll != null)
                onScroll(eventData);
        }

        void ISelectHandler.OnSelect(BaseEventData eventData)
        {
            if (onSelect != null)
                onSelect(eventData);
        }

        void IDeselectHandler.OnDeselect(BaseEventData eventData)
        {
            if (onDeselect != null)
                onDeselect(eventData);
        }

        public void SetRepeat(bool repeat)
        {
            this.IsRepeat = repeat;
        }

        private void OnHoldDown(float x, float y)
        {

            if (onRepeat != null && this.IsRepeat && this.IsRepeating)
            {
                onRepeat(gameObject);

            }

        }

        private void Update()
        {
            CheckTouch();
        }

        private void CheckTouch()
        {
            if (Input.touchPressureSupported)
            {
                if (_3dTouchPressureState >= 0)
                {
                    if (Input.GetTouch(0).pressure > PRESSURE_LV_2)
                    {

                        if (onTouchPressureLv2 != null && _3dTouchPressureState < PRESSURE_LV_2)
                        {
                            onTouchPressureLv2(gameObject);
                            _3dTouchPressureState = PRESSURE_LV_2;
                        }
                    }
                    else if (Input.GetTouch(0).pressure > PRESSURE_LV_1)
                    {
                        if (onTouchPressureLv1 != null && _3dTouchPressureState < PRESSURE_LV_1)
                        {
                            onTouchPressureLv1(gameObject);
                            _3dTouchPressureState = PRESSURE_LV_1;
                        }
                    }
                    if (Input.GetTouch(0).pressure > 0 && onTouchPressure != null)
                    {
                        onTouchPressure(gameObject, Input.GetTouch(0).pressure);
                    }
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        if (onEndTouchPressure != null && _3dTouchPressureState > 0)
                        {
                            onEndTouchPressure(gameObject);
                        }
                        _3dTouchPressureState = -1;
                    }
                }
            }


            //if (Input.GetMouseButton(0))
            //{
            //    if (_touchTimeState >= 0)
            //    {
            //        //Debug.LogWarning("button-----");
            //        if (_touchPassTime > TOUCH_TIME_LV_2)
            //        {
            //            //if (onTouchTimeLv2 != null && _touchTimeState < TOUCH_TIME_LV_2)
            //            //{
            //            //    onTouchTimeLv2(gameObject);
            //            //    _touchTimeState = (int)TOUCH_TIME_LV_2;
            //            //}
            //            if (onTouchPressureLv2 != null && _touchTimeState < TOUCH_TIME_LV_2)
            //            {
            //                onTouchPressureLv2(gameObject);
            //                _touchTimeState = (int)TOUCH_TIME_LV_2;
            //            }
            //        }
            //        else if (_touchPassTime > TOUCH_TIME_LV_1)
            //        {
            //            //if (onTouchTimeLv1 != null && _touchTimeState < TOUCH_TIME_LV_1)
            //            //{
            //            //    onTouchTimeLv1(gameObject);
            //            //    _touchTimeState = (int)TOUCH_TIME_LV_1;
            //            //}
            //            if (onTouchPressureLv1 != null && _touchTimeState < TOUCH_TIME_LV_1)
            //            {
            //                onTouchPressureLv1(gameObject);
            //                _touchTimeState = (int)TOUCH_TIME_LV_1;
            //            }
            //        }
            //        if(onTouchPressure != null)
            //        {
            //            onTouchPressure(gameObject, _touchPassTime);
            //        }
            //        _touchPassTime += Time.deltaTime;
            //    }
            //}
            //else
            //{
            //    if (_touchTimeState >= 0)
            //    {
            //        if (onEndTouchPressure != null && _touchTimeState > 0)
            //        {
            //            onEndTouchPressure(gameObject);
            //        }
            //        _touchPassTime = 0;
            //        _touchTimeState = -1;
            //    }
            //}
        }
    }
}

