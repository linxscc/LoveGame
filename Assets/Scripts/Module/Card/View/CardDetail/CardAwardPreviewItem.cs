using game.main;
using game.tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardAwardPreviewItem : MonoBehaviour {


        private Text _voiceName;
        private Transform _onClick;
        private Transform _mask;
        private Transform _icon;

        private Transform _lock;
        private CardAwardPreInfo _info;
        private Transform _redPoint;

        private void Awake()
        {
            _voiceName = transform.GetText("VoiceName");
            _onClick = transform.Find("OnClick");
            _mask = transform.Find("Mask");
            _lock = transform.Find("Lock");
            _icon = transform.Find("Icon");
            _redPoint = transform.Find("RedPoint");

            _onClick.gameObject.Hide();
            _mask.gameObject.Hide();
            _lock.gameObject.Hide();
            _icon.gameObject.Hide();
            _redPoint.gameObject.Hide();

        //PointerClickListener.Get(_onClick.gameObject).onClick = go =>
        //{

        //    if (_info.isNew)
        //    {
        //        Util.DeleteRedPoint(_info.key, false);
        //    }

        //    EventDispatcher.TriggerEvent(EventConst.OnClickVoiceItem, _info);

        //};
        //PointerClickListener.Get(_icon.gameObject).onClick = go =>
        //{
        //    if (_info.isNew)
        //    {
        //        Util.DeleteRedPoint(_info.key, false);
        //    }
        //    EventDispatcher.TriggerEvent(EventConst.OnClickVoiceItem, _info);

        //};
        PointerClickListener.Get(_mask.gameObject).onClick = go =>
        {
            FlowText.ShowMessage(_info.UnlockDescription);
        };
        PointerClickListener.Get(_lock.gameObject).onClick = go =>
        {
            FlowText.ShowMessage(_info.UnlockDescription);
        };
    }


        public void SetData(CardAwardPreInfo info)
        {

            _info = info;

            _voiceName.text = I18NManager.Get(_info.isUnlock ? "Favorability_AlreadyUnlock" : "Favorability_NoUnlock", info.ShowContent);
            SetIsUnlock();
            SetRedPoint();

        }

        private void SetRedPoint()
        {
            if (_info.isNew)
            {
                _redPoint.gameObject.Show();

            }
            else
            {
                _redPoint.gameObject.Hide();
            }
        }

        private void SetIsUnlock()
        {
            if (_info.isUnlock)
            {
                _onClick.gameObject.Show();
                _mask.gameObject.Hide();
                _lock.gameObject.Hide();
                _icon.gameObject.Show();
            }
            else
            {
                _onClick.gameObject.Hide();
                _mask.gameObject.Show();
                _lock.gameObject.Show();
                _icon.gameObject.Hide();
            }
        }
    }

