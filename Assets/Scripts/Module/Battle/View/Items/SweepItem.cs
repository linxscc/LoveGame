using System.Collections;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using DataModel;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class SweepItem : MonoBehaviour
    {
        private Image _propImage;
        private Text _propNameTxt;
        private Text _obtainText;
        private Text _drawActivityText;
        private DrawActivityDropItemVo _extReward;
        private Transform _drawActivity;

        void Awake () 
        {
            _propImage = transform.Find("PropImage").GetComponent<Image>();
            _propNameTxt = transform.Find("PropNameTxt").GetComponent<Text>();
            _obtainText = transform.Find("ObtainText").GetComponent<Text>();
            _drawActivityText = transform.Find("DrawActivityItem/Text").GetComponent<Text>();
            _drawActivity = transform.Find("DrawActivityItem");

            _propNameTxt.text = "";
            _obtainText.text = "";
        }
		
        public void SetData(RewardVo vo, DrawActivityDropItemVo extReward)
        {
            _extReward = extReward;
            
            _propImage.sprite = ResourceManager.Load<Sprite>(vo.IconPath, ModuleConfig.MODULE_MAIN_LINE);

            if(_propImage.sprite == null)
                _propImage.sprite = ResourceManager.Load<Sprite>("Prop/1122", ModuleConfig.MODULE_MAIN_LINE);

            if (extReward == null)
            {
                _drawActivity.gameObject.Hide();
            }
            else
            {
                StartCoroutine(ResetText());
            }
            
            _propNameTxt.text = vo.Name;
            _obtainText.text = I18NManager.Get("Recollection_GetNum", vo.Num);
            _propImage.SetNativeSize();
        }

        private IEnumerator ResetText()
        {
            yield return null;
            _drawActivityText.text = I18NManager.Get("ActivityTemplate_TodayLimit", 
                _extReward.TotalNum, _extReward.LimitNum);
        }
    }
}