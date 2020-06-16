using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace game.main.Live2d
{
    public class Live2dGraphic : MonoBehaviour
    {
        private Live2dCanvas _live2dCanvas;
        private RawImage _image;

        public RawImage Image
        {
            get { return _image; }
        }

        private L2DView _live2dView;
        public L2DView GetMainLive2DView
        {
            get { return _live2dView; }
        }

        private EntityVo _entityVo;
        private Live2dPlaySequence _sequence;
        private RenderTexture _rtTex;

        public bool raycastTarget
        {
            set { _image.raycastTarget = value; }
        }

        public Color Color
        {
            get { return _image.color; }
            set { _image.color = value; }
        }

        private List<string> _donotUnloadIds;
        
        /// <summary>
        /// 要停止自动卸载live2d资源就要在加载Live2d之前调用这个方法
        /// </summary>
        /// <param name="ids"></param>
        public void AddDonotUnloadIds(params string[] ids)
        {
            if(_donotUnloadIds == null)
                _donotUnloadIds = new List<string>();
            
            _donotUnloadIds.AddRange(ids);
        }
        
        private void Awake()
        {
            _live2dCanvas = Instantiate(ResourceManager.Load<GameObject>(L2DConst.Live2dCameraPath)).GetComponent<Live2dCanvas>();
        }

        private void OnEnable()
        {
            if(_live2dView != null && _live2dView.Model != null)
                _live2dView.Reload();
        }


        /// <summary>
        /// 加载BundleAssets\SingleFile\Live2d\Animation路径下对应id的动画
        /// </summary>
        /// <param name="id">live2d id，文件夹名</param>
        /// <param name="x">x方向偏移量</param>
        /// <param name="y">y方向偏移量</param>
        /// <param name="width">缩放比例</param>
        public void LoadAnimationById(string id, float x = -0.85f, float y = 1.32f, float width = 1.7f, int textureWidth = 1440, int textureHeight = 2500)
        {
            if (_rtTex != null)
            {
                if(_live2dCanvas != null && _live2dCanvas.Live2dCamera != null)
                    _live2dCanvas.Live2dCamera.targetTexture = null;

                DestroyImmediate(_rtTex);
            }
            
            _rtTex = new RenderTexture(textureWidth, textureHeight, (int)Screen.dpi, RenderTextureFormat.ARGB32);
            _live2dCanvas.Live2dCamera.targetTexture = _rtTex;

            _image = transform.GetComponent<RawImage>();
            _image.texture = _live2dCanvas.Live2dCamera.targetTexture;
            
            if (_live2dView == null)
            {
                _live2dView = Live2dManager.CreateL2DView(id, _live2dCanvas, _donotUnloadIds);
                _live2dView.Live2dViewType = LIVE2DVIEWTYPE.MAINPANLE;
            }
            else
            {
                _live2dView.LoadModel(id, _donotUnloadIds);
            }

            _live2dView.X = x;
            _live2dView.Y = y;
            _live2dView.Width = width;
        }

        public void SetData(EntityVo vo)
        {
            if (_live2dCanvas.Live2dCamera.targetTexture != null)
            {
                Debug.LogError("===========_live2dCanvas.Live2dCamera.targetTexture==============");
            }
            
            var w = vo.width;
            var h = vo.height;
            var target = new RenderTexture((int)w, (int)h, (int)Screen.dpi, RenderTextureFormat.ARGB32);
            _live2dCanvas.Live2dCamera.targetTexture = target;

            _image = transform.GetComponent<RawImage>();
            _image.texture = _live2dCanvas.Live2dCamera.targetTexture;
            
            _entityVo = vo;
            if (_live2dView == null)
            {
                _live2dView = Live2dManager.CreateL2DView(vo.id, _live2dCanvas, _donotUnloadIds);
              //  _live2dView.Live2dViewType = LIVE2DVIEWTYPE.MAINPANLE;
            }
            else
            {
                _live2dView.LoadModel(vo.id, _donotUnloadIds);
            }

            _live2dView.X = vo.L2dScaleDataList[0];
            _live2dView.Y = vo.L2dScaleDataList[1];
            _live2dView.Width = vo.L2dScaleDataList[2];

            _live2dView.LipSync = vo.lipSpync;
        }
        
        /// <summary>
        /// 在live2dId相同的情况下才能使用
        /// </summary>
        /// <param name="vo"></param>
        public void ChangeAnimation(EntityVo vo)
        {
            _entityVo = vo;
            _sequence.ChangeAnimation(vo.l2dPlayableList);
            _live2dView.LipSync = vo.lipSpync;
        }

        public void Play()
        {
            _sequence = new Live2dPlaySequence(_live2dView, _entityVo.l2dPlayableList);
            _sequence.Play();
            _sequence.EndCallback = EndCallback;
        }

        private void EndCallback()
        {
            _sequence = null;
        }

        private void OnDestroy()
        {
            if (_live2dCanvas != null && _live2dCanvas.Live2dCamera != null)
            {
                _live2dCanvas.Live2dCamera.targetTexture = null;
                DestroyImmediate(_live2dCanvas.gameObject);
            }
            DestroyImmediate(_rtTex);
            if (_live2dView != null) _live2dView.Dispose();
        }

        private void Update()
        {
            _sequence?.Update();
        }

        public void Hide()
        {
            gameObject.Hide();
            _live2dCanvas.gameObject.Hide();
            if(_live2dView != null)
                _live2dView.gameObject.Hide();
        }

        public void Show()
        {
            gameObject.Show();
            _live2dCanvas.gameObject.Show();
            if(_live2dView != null)
                _live2dView.gameObject.Show();
        }

        
    }
}