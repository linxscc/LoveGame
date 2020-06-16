using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class CommonRuleWindow : Window
    {
        private Text _tittleText;
        private Text _contentText;

        protected override void OnInit()
        {
            base.OnInit();

            _tittleText = transform.Find("Title/Text").GetComponent<Text>();
            _contentText = transform.Find("ContentRect/Viewport/Content").GetComponent<Text>();
        }

        public void SetData(string content, string tittle = null)
        {
            if(tittle == null)
                tittle = I18NManager.Get("ActivityTemplate_ActivityTemplateRule_Tittle");
            _tittleText.text = tittle;
            _contentText.text = content;
        }
    }
}
