using System;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
using DataModel;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace game.main
{
    public class DialogFrameTelephone : MonoBehaviour, IDragHandler
    {
        private Text _contentTxt;
        private Text _nameTxt;
        private Text _nameTxt2;
        private Image _nameBg;
        private Image _bg;

        private string _dialogStr;

        private int _typingIndex = 0;

        private int _typingSpeed = NormalSpeed;
        private bool _isPlaying;

        public const float MinPlayingTime = 0.3f;
        public const float MinDuration = 0.3f;

        private const int NormalSpeed = 10;
        private const int FastSpeed = 100;

        public bool IsPlaying
        {
            get { return _isPlaying; }
        }

        public bool IsTyping;
        public Action OnStepEnd;

        private float _playingTime;
        private float _textIndexTime;
        private float _duration;

        private Coroutine _coroutine;

        /// <summary>
        /// 根据文本长度计算的播放时间
        /// </summary>
        private float DurationTime;

        /// <summary>
        /// 开始播放
        /// </summary>
        private bool _isStart = false;

        private bool _isTweenText = false;
        private float _lineHeight;
        private float _lineSpacing;
        private int _lastLineCount;
        private float _shakeTime = 0;

        private Vector3 _pos;

        private float _radian = 1f;

        [SerializeField] private Vector3 _deltaPos;
        [SerializeField] private float _max = 30;
        [SerializeField] private float _min = 10;
        [SerializeField] private float ShakeTime = 0.5f;

        private Transform _headContainer;
        private RawImage _headRole;
        
        private TelephoneDialogVo _telephoneDialogVo;
        private string _roleName;
        private int _contentTxtY;
        private float _moveY;

        private void Awake()
        {
            _contentTxt = transform.Find("ContextMask/ContentTxt").GetComponent<Text>();
            _nameTxt = transform.Find("NameBg/NameTxt").GetComponent<Text>();
            _nameTxt2 = transform.Find("HeadContainer/NameText").GetComponent<Text>();

            _nameBg = transform.Find("NameBg").GetComponent<Image>();

            _headContainer = transform.Find("HeadContainer");
            _headRole = transform.Find("HeadContainer/Role").GetComponent<RawImage>();

            _bg = transform.Find("Bg").GetComponent<Image>();

            _contentTxt.text = "";
            _lineHeight = _contentTxt.preferredHeight;
            _lineSpacing = (_contentTxt.lineSpacing - 1) * _contentTxt.fontSize;

            RectTransform rect = _contentTxt.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, _lineHeight * 3);
        }

        public void Typing()
        {
            if (_dialogStr == null)
            {
                throw new Exception("_dialogStr == null");
            }

            if (IsTyping == false)
            {
                _isPlaying = true;
                IsTyping = true;

                _playingTime = (float) _dialogStr.Length / 35 * 5.0f;
                _duration = _playingTime;
                if (_duration < MinPlayingTime)
                {
                    _duration = MinPlayingTime;
                }

                DurationTime = _duration;

                _textIndexTime = 0;
                _typingSpeed = NormalSpeed;
            }
            else
            {
                _typingSpeed = FastSpeed;
                if (_duration > MinDuration)
                    _duration = MinDuration;
                DurationTime = _duration;
            }
        }

        private void Update()
        {
            if (_isPlaying == false || _isStart == false)
                return;

            if (_typingSpeed == NormalSpeed)
            {
                _playingTime -= Time.deltaTime;
            }
            else
            {
                _playingTime -= Time.deltaTime * FastSpeed / NormalSpeed;
            }

            _duration -= Time.deltaTime;

            if (_shakeTime > 0)
            {
                transform.localPosition -= _deltaPos;

                float ran = Random.Range(_min, _max);
                float radian = Random.Range(0, _radian);
                _deltaPos = new Vector2((float) (ran * Math.Sin(radian)), (float) (-ran * Math.Cos(radian)));

                transform.localPosition += _deltaPos;

                _shakeTime -= Time.deltaTime;
            }
            else
            {
                transform.localPosition = _pos;
            }

            if (_playingTime <= 0)
            {
                //结束打字动画，可以进行下一步
                if (_duration <= 0)
                {
                    _isPlaying = false;
                    IsTyping = false;
                    _isStart = false;
                    OnStepEnd?.Invoke();
                    return;
                }
            }

            _textIndexTime += _typingSpeed * Time.deltaTime;
            int index = Math.Min(Convert.ToInt32(_textIndexTime), _dialogStr.Length);
            _contentTxt.text = _dialogStr.Substring(0, index);

            float h = _contentTxt.preferredHeight;

            int lineCount = (int) Math.Ceiling(h / (_lineHeight + _lineSpacing));

            if (lineCount > 3 && lineCount > _lastLineCount && _isTweenText == false)
            {
                _lastLineCount = lineCount;
                RectTransform rect = _contentTxt.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, h);
                _moveY = rect.anchoredPosition.y + _lineHeight * (lineCount - 3) - _lineSpacing / 2 * lineCount;
                rect.DOAnchorPosY(_moveY, 0.2f).onComplete = () => { _isTweenText = false; };
            }
        }

        public void Show(float showDelay)
        {
            _pos = transform.localPosition;

            _dialogStr = Util.GetNoBreakingString(_telephoneDialogVo.Content);

            CancelDelayCall();

            _coroutine = ClientTimer.Instance.DelayCall(() =>
            {
                _isStart = true;
//                ChangeStyle();
                

                gameObject.SetActive(true);

                if (false)
                {
                    _shakeTime = ShakeTime;
                    _deltaPos = Vector3.zero;
                    _pos = transform.localPosition;
                }
                else
                {
                    _shakeTime = 0;
                }

                Typing();
            }, showDelay);
        }

        public void SetData(TelephoneDialogVo vo, string roleName)
        {
            _headContainer.gameObject.SetActive(false);
            
            _telephoneDialogVo = vo;
            
            if (vo.IsHeroine)
            {
                _roleName = GlobalData.PlayerModel.PlayerVo.UserName;
            }
            else
            {
                _roleName = roleName;
            }
            
            _nameTxt.text = _roleName;
            _nameTxt2.text = _roleName;

            if (!string.IsNullOrEmpty(_telephoneDialogVo.HeadId))
            {
                _headRole.texture = ResourceManager.Load<Texture>(AssetLoader.GetHeadImageById(vo.HeadId), ModuleConfig.MODULE_STORY);
            }

            _isPlaying = true;
            _lastLineCount = 0;

            if (_contentTxt != null)
                _contentTxt.text = "";
            
            ChangeStyle();
        }

        public void SetAudioTime(float audioTime)
        {
            if (audioTime < 1.0f || audioTime < DurationTime)
                return;

            float time = DurationTime - _duration;
            float dValue = audioTime - time;
            if (dValue > _duration)
                _duration = dValue + 0.3f;
        }

        private void SetHeadTexture(Texture texture)
        {
            _headRole.texture = texture;
        }

        private void ChangeStyle()
        {
            _nameBg.gameObject.SetActive(true);

            SetContentTextWidthByHead();

            EntityVo.DialogFrameStyle frameStyleId = _telephoneDialogVo.DialogFrameStyle;

            if (frameStyleId == EntityVo.DialogFrameStyle.None)
            {
                _bg.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Story_dialogFrameBg");
                _contentTxt.color = ColorUtil.HexToColor("333333");
            }
            else
            {
                _bg.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Story_dialogFrameBlack");
                _contentTxt.color = Color.white;
            }

            Outline outline = _nameTxt.GetComponent<Outline>();
            switch (frameStyleId)
            {
                case EntityVo.DialogFrameStyle.None:
                    _nameBg.gameObject.SetActive(false);
                    break;
                case EntityVo.DialogFrameStyle.Heroine:
                    _nameBg.gameObject.SetActive(false);
                    break;
                case EntityVo.DialogFrameStyle.Qinyuzhe:
                    _nameBg.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Story_nameBgQin");
                    outline.effectColor = ColorUtil.HexToColor("9B78A3");
                    break;
                case EntityVo.DialogFrameStyle.Tangyichen:
                    _nameBg.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Story_nameBgTang");
                    outline.effectColor = ColorUtil.HexToColor("B19823");
                    break;
                case EntityVo.DialogFrameStyle.Chiyu:
                    _nameBg.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Story_nameBgChi");
                    outline.effectColor = ColorUtil.HexToColor("9C5B87");
                    break;
                case EntityVo.DialogFrameStyle.Yanji:
                    _nameBg.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Story_nameBgYan");
                    outline.effectColor = ColorUtil.HexToColor("53558A");
                    break;
                case EntityVo.DialogFrameStyle.Npc:
                    _nameBg.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Story_nameBgNpc");
                    outline.effectColor = ColorUtil.HexToColor("6B9372");
                    break;
                case EntityVo.DialogFrameStyle.Heroine2:
                    _nameBg.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Story_nameBgHeroine");
                    outline.effectColor = ColorUtil.HexToColor("B485C2");
                    break;
            }
        }

        private void SetContentTextWidthByHead()
        {
            RectTransform rt = _contentTxt.GetComponent<RectTransform>();
            if (string.IsNullOrEmpty(_telephoneDialogVo.HeadId))
            {
                _contentTxtY = 0;
                rt.sizeDelta = new Vector2(790, 230);
                rt.anchoredPosition = new Vector3(113, _contentTxtY);
                _headContainer.gameObject.SetActive(false);
            }
            else
            {
                _contentTxtY = 0;
                rt.sizeDelta = new Vector2(530, 230);
                rt.anchoredPosition = new Vector3(383, _contentTxtY);
                _headContainer.gameObject.SetActive(true);
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            RectTransform rt = _contentTxt.GetComponent<RectTransform>();
            float targetY = rt.anchoredPosition.y + eventData.delta.y;

            if(targetY < _contentTxtY)
            {
                targetY = _contentTxtY;
            }
            else if(targetY > _moveY)
            {
                targetY = _moveY;
            }
            
            rt.anchoredPosition = new Vector3(rt.anchoredPosition.x, targetY);
        }

        private void OnDestroy()
        {
            CancelDelayCall();
        }

        private void CancelDelayCall()
        {
            if (_coroutine != null)
                ClientTimer.Instance.CancelDelayCall(_coroutine);
        }
        
    }
}