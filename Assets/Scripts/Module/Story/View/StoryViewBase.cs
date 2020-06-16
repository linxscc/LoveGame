using Assets.Scripts.Framework.GalaSports.Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class StoryViewBase : View
    {
        public bool IsWait = false;
       
        protected bool _isAutoPlay;
        
        protected Button _playBtn;
        protected Button _recordBtn;
        protected Button _skipBtn;
        
        protected bool _continueAutoPlay;
        
        protected int _currentIndex;
        
        protected bool _hasSelection;
        protected DialogSelection _dialogSelection;
        
        protected int _offsetY = 0;
        
        public void HideAllBtn()
        {
            _playBtn.gameObject.Hide();
            _recordBtn.gameObject.Hide();
            _skipBtn.gameObject.Hide();
        }

        protected virtual void OnSkip()
        {
            SendMessage(new Message(MessageConst.CMD_STORY_END, Message.MessageReciverType.CONTROLLER));
        }
        
        protected virtual void ShowRecordView()
        {
            if (_isAutoPlay)
            {
                OnAutoPlay(false);
            }
            SendMessage(new Message(MessageConst.MODULE_STORY_SHOW_RECORD_VIEW));
        }
        
        protected virtual void OnAutoPlay(bool isAutoPlay)
        {
            _isAutoPlay = isAutoPlay;
            Text text = _playBtn.transform.GetText("Text");
            if (_isAutoPlay)
            {
                text.text = I18NManager.Get("Story_Automaticing");
            }
            else
            {
                text.text = I18NManager.Get("Story_Autoplay");
            }
        }
        
        public void SetAutoPlay(bool continueAutoPlay)
        {
            _continueAutoPlay = continueAutoPlay;
        }
        
        public void ForceAutoPlay()
        {
            _continueAutoPlay = true;
            OnAutoPlay(_continueAutoPlay);
        }

        protected void EndStory()
        {
            ClientTimer.Instance.DelayCall(
                () => { SendMessage(new Message(MessageConst.CMD_STORY_END, Message.MessageReciverType.CONTROLLER)); },
                1.0f);
        }

        protected void OnEvent(EventVo evt)
        {
            ClientTimer.Instance.DelayCall(
                () => {
                    SendMessage(new Message(MessageConst.CMD_STORY_ON_EVENT, Message.MessageReciverType.DEFAULT,
                        evt.EventType, evt.EventId, _isAutoPlay));
                    _isAutoPlay = false;
                },
                0.5f);
        }
        
        protected virtual void OpenAnimation(bool showAnimation)
        {
            if(showAnimation == false)
                return;
            
            gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 1.0f);
            gameObject.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo);
        }
        
        
        protected void CreateSelection(EventVo eventVo)
        {
            if (_dialogSelection == null)
            {
                GameObject go = InstantiatePrefab("Story/Prefabs/DialogSelection");
                go.name = "DialogSelection";
                go.transform.SetParent(transform, false);
                _dialogSelection = go.GetComponent<DialogSelection>();
                go.transform.SetSiblingIndex(transform.childCount - 2);
            }

            RectTransform rect = _dialogSelection.gameObject.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, 227 - _offsetY);
            _dialogSelection.SetData(eventVo, OnSelected);
            _dialogSelection.gameObject.Show();

            _hasSelection = true;
        }
        
        protected void HideSelection()
        {
            _hasSelection = false;
            if(_dialogSelection != null)
                _dialogSelection.gameObject.Hide();
        }

        protected void OnSelected(EventType eventType, string eventId)
        {
            SendMessage(new Message(MessageConst.CMD_STORY_ON_EVENT, Message.MessageReciverType.DEFAULT, eventType,
                eventId, _isAutoPlay));
            
        }
    }
}