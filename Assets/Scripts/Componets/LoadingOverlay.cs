using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Componets
{
    public class LoadingOverlay : MonoBehaviour
    {
        private RectTransform _star1;
        private RectTransform _star2;
        private RectTransform _star3;
        private static LoadingOverlay _instance;
        private bool _isShow = false;

        public static LoadingOverlay Instance { get { return _instance; } }

        private float _startTime;

        public float Timeout = 60;
        public float DelayShow = 1.2f;
        private RectTransform _starContainer;
        private Image _textImage;

        private void Awake()
        {
            _instance = this;
            
            _starContainer = transform.Find("Stars").GetComponent<RectTransform>();
            _textImage = transform.Find("TextImage").GetComponent<Image>();
            
            _star1 = transform.Find("Stars/Star1").GetComponent<RectTransform>();
            _star2 = transform.Find("Stars/Star2").GetComponent<RectTransform>();
            _star3 = transform.Find("Stars/Star3").GetComponent<RectTransform>();

            Hide();
        }

        public void Show()
        {
            _isShow = true;
            _startTime = Time.realtimeSinceStartup;
            gameObject.Show();
        }
        
        public void Hide()
        {
            _isShow = false;
            gameObject.Hide();
            _starContainer.gameObject.Hide();
            _textImage.gameObject.Hide();
        }

        public void ShowMask(bool showMask)
        {
            gameObject.SetActive(showMask);
        }
        
        

        private void Update()
        {
            if (_isShow)
            {
                if (Time.realtimeSinceStartup - _startTime > DelayShow)
                {
                    _starContainer.gameObject.Show();
                    _textImage.gameObject.Show();
                    
                    float speed = 6;
                    _star1.Rotate(Vector3.back * speed);
                    _star2.Rotate(Vector3.back * speed);
                    _star3.Rotate(Vector3.back * speed);
                    if (Time.realtimeSinceStartup - _startTime > Timeout)
                    {
                        Hide();
                    }
                }
            }
        }
    }
}