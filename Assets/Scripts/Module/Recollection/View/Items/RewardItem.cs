using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using DataModel;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Module.Recollection.View
{
    public class RewardItem : MonoBehaviour
    {
        private Image _propImage;
       
        private Text _obtainText;


        private void Awake()
        {
            _obtainText = transform.Find("ObtainText").GetComponent<Text>();
           
        }

        public void ShowReward(AwardPB pb)
        {
            RewardVo vo = new RewardVo(pb);
            _propImage = transform.Find("Cards/Prop").GetComponent<Image>();
         
           // _obtainText = transform.Find("ObtainText").GetComponent<Text>();

            _propImage.sprite = ResourceManager.Load<Sprite>(vo.IconPath, ModuleConfig.MODULE_RECOLLECTION);

            if(_propImage.sprite == null)
                _propImage.sprite = ResourceManager.Load<Sprite>("Prop/1122",ModuleConfig.MODULE_RECOLLECTION);

          
            _obtainText.text = I18NManager.Get("Recollection_GetNum",vo.Num);
            
            _propImage.SetNativeSize();
            
           
        }
    }
}