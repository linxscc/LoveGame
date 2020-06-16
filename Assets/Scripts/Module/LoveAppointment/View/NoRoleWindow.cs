using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using DataModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace game.main
{
    public class NoRoleWindow : Window
    {

        //private Text _propNum;
        private Transform _btnGroup;
        private Text _tips;

        private void Awake()
        {
            _btnGroup = transform.Find("BtnGroup");
            _tips = transform.Find("Tips").GetComponent<Text>();
            for (int i = 0; i < _btnGroup.childCount; i++)
            {
                _btnGroup.GetChild(i).gameObject.Hide();
            }

        }

        public void SetData(AppointmentRuleVo vo)
        {
            _tips.text = I18NManager.Get("LoveAppointment_NoRoleWindowTips");//"获得此星缘可解锁恋爱剧情";
            
            for (int i = 0; i < _btnGroup.childCount; i++)
            {
                Transform roleStory = _btnGroup.GetChild(i);
                //Debug.LogError(vo.ActiveCards[i]);
                if (vo.ActiveCards[i]<=0)
                {
                    continue;
                }
                _btnGroup.GetChild(i).gameObject.Show();
                RawImage role = roleStory.Find("RoleImage").GetComponent<RawImage>();
                Text storyName = roleStory.Find("Text").GetComponent<Text>();
                var card=GlobalData.CardModel.GetCardBase(vo.ActiveCards[i]);
                role.texture=ResourceManager.Load<Texture>("Card/Image/MiddleCard/"+vo.ActiveCards[i],ModuleConfig.MODULE_LOVEAPPOINTMENT);
                storyName.text = CardVo.SpliceCardName(card.CardName,card.Player);
            }
            
            
        }


    }
}