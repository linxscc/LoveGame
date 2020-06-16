using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class DialogSelection : MonoBehaviour
    {
        private Text _selection1;
        private Text _selection2;
        private Text _selection3;

        private Action<EventType, string> _callback;
        private EventVo _eventVo;

        private void Awake()
        {
            _selection1 = transform.Find("Selection1/Text").GetComponent<Text>();
            _selection2 = transform.Find("Selection2/Text").GetComponent<Text>();
            _selection3 = transform.Find("Selection3/Text").GetComponent<Text>();

            transform.Find("Selection1").GetComponent<Button>().onClick.AddListener(Select1);
            transform.Find("Selection2").GetComponent<Button>().onClick.AddListener(Select2);
            transform.Find("Selection3").GetComponent<Button>().onClick.AddListener(Select3);
        }

        private void Select1()
        {
            _callback?.Invoke(_eventVo.SelectionTypes[0], _eventVo.SelectionIds[0]);
        }
        private void Select2()
        {
            _callback?.Invoke(_eventVo.SelectionTypes[1], _eventVo.SelectionIds[1]);
        }
        private void Select3()
        {
            _callback?.Invoke(_eventVo.SelectionTypes[2], _eventVo.SelectionIds[2]);
        }


        public void SetData(EventVo eventVo, Action<EventType, string> callback)
        {
            _eventVo = eventVo;
            
            _callback = callback;
            
            _selection1.text = eventVo.SelectionContents[0];
            _selection2.text = eventVo.SelectionContents[1];
            if (eventVo.SelectionContents.Count == 3)
            {
                _selection3.text = eventVo.SelectionContents[2];
                _selection3.transform.parent.gameObject.Show();
                GetComponent<VerticalLayoutGroup>().spacing = 50;
            }
            else
            {
                _selection3.transform.parent.gameObject.Hide();
                GetComponent<VerticalLayoutGroup>().spacing = 68;
            }
        }
    }
}