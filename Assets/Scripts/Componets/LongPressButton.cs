using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Componets
{
    [RequireComponent(typeof(Button))]
    public class LongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        private bool _isDown = false;
        public Action OnDown;
        public Action OnUp;
        
        private void OnEnable()
        {
            _isDown = false;
        }
        
        private float _interval = 0.05f;
        public float Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }
        
        private float _delay = 0.3f;
        public float Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        private float lastTime;
        private float _downTime;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
        }

        
        /// <summary>
        /// Down跟Click是有本质区别的，一个是按下即触发，一需要长按才触发！
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerDown(PointerEventData eventData)
        {
            if(_button.gameObject.activeSelf == false)
                return;
            
            OnDown?.Invoke();
            _downTime = Time.time;
            _isDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnUp?.Invoke();
            //Debug.LogError("UP");
            _isDown = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isDown = false;
        }

        private void Update()
        {
            if (_isDown)
            {
                float time = Time.time - _downTime ;
                if (time < _delay)
                    return;
                
                if (Time.time - lastTime > _interval)
                {
                    if (_button && _button.onClick != null)
                    {
                        OnDown?.Invoke();
                        _button.onClick.Invoke();
                    }
                    lastTime = Time.time;
                }
            }
        }

    }
}