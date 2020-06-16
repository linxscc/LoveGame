using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Effect;
using DataModel;
using DG.Tweening;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class StorySmsView : StoryViewBase
    {
        private RawImage _bgImage;
        
        private BackgroundBlurEffect _bgBlurEffect;
        private Text _nameText;
        private Transform _content;

        private SmsVo _smsVo;

        private SmsItem _currentSmsItem;
        private float _lastTime;

        private void Awake()
        {
            _bgImage = transform.GetRawImage("BgImage");
            _nameText = transform.GetText("TopImage/Text");
            _content = transform.Find("Scroll View/Viewport/Content");
            
            _skipBtn = transform.Find("SkipBtn").GetComponent<Button>();
            _skipBtn.onClick.AddListener(OnSkip);
            
            _recordBtn = transform.Find("RecordBtn").GetComponent<Button>();
            _recordBtn.onClick.AddListener(ShowRecordView);

            _playBtn = transform.Find("PlayBtn").GetComponent<Button>();
            _playBtn.transform.GetText("Text").text = I18NManager.Get("Story_Autoplay");
            _playBtn.onClick.AddListener(()=>
            {
                OnAutoPlay(!_isAutoPlay);
            });
            
            _bgBlurEffect = EffectManager.CreateImageBlurEffect();
            
            IsWait = true;
            
            PointerClickListener.Get(gameObject).onClick = NextStep;
        }
        
        public void SetData(SmsVo vo, bool showAnimation)
        {
            OpenAnimation(showAnimation);
            
            _smsVo = vo;
            _bgBlurEffect.StartRecord(ResourceManager.Load<Texture>(AssetLoader.GetStoryBgImage(_smsVo.bgImageId), ModuleName), tex =>
            {
                _bgImage.texture = tex;
            });
            _nameText.text = vo.GetRoleName();
            
            _currentIndex = 0;
            IsWait = false;

            AddNewItem();

            OnAutoPlay(_continueAutoPlay);
            
            NextStep(null);
        }

        public void Append(SmsVo vo)
        {
            HideSelection();
            SetData(vo, false);
        }

        public void ResetView()
        {
            HideSelection();
            IsWait = true;
            _content.RemoveChildren();
        }

        private void AddNewItem()
        {
            SmsDialogVo data = _smsVo.dialogList[_currentIndex];
            _currentSmsItem = CreateItem(data.IsLeft);
            _currentSmsItem.SetData(data);
            _currentIndex++;
            
            string roleName = data.IsLeft ?  _smsVo.GetRoleName() : GlobalData.PlayerModel.PlayerVo.UserName;
            SendMessage(new Message(MessageConst.CMD_STORY_RECODE_DIALOG, Message.MessageReciverType.DEFAULT,
                data,roleName));
        }
        private void NextStep(GameObject go)
        {
            if(IsWait || _hasSelection)
                return;
            
            if (_smsVo.dialogList.Count > _currentIndex)
            {
                if (_currentSmsItem.IsEnd)
                {
                    AddNewItem();
                }
            }
            else
            {
                if (_currentSmsItem.IsEnd == false)
                    return;
                
                if ( _smsVo.Event != null )
                {
                    if (_smsVo.Event.EventType == EventType.Selection)
                    {
                        CreateSelection(_smsVo.Event);
                    }
                    else
                    {
                        OnEvent(_smsVo.Event);
                    }
                }
                else
                {
                    EndStory();
                }

                IsWait = true;
            }
        }

        private SmsItem CreateItem(bool isLeft)
        {
            SmsItem item = null;
            GameObject go = null;
            if (isLeft)
            {
                go = InstantiatePrefab("Story/Prefabs/SmsItemLeft");
                item = go.AddComponent<SmsItemLeft>();
                item.SetHead(_smsVo.Role);
            }
            else
            {
                go = InstantiatePrefab("Story/Prefabs/SmsItemRight");
                item = go.AddComponent<SmsItemRight>();
            }

            go.transform.SetParent(_content, false);

            return item;
        }

        private void Update()
        {
            if(IsWait || _isAutoPlay == false)
                return;

            if(Time.realtimeSinceStartup -_lastTime > 0.1f)
            {
                _lastTime = Time.realtimeSinceStartup;
                NextStep(null);
            }
        }
        
        private void OnDestroy()
        {
            EffectManager.DestroyBackgroundEffect();
        }
    }
}