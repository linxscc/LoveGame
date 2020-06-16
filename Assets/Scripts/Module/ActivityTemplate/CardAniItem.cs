
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using DataModel;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Module;

public class CardAniItem : MonoBehaviour 
{

   public void ShowReward(AwardPB pb)
   {
        RewardVo vo = new RewardVo(pb);
        var propImage = transform.Find("Cards/Prop").GetComponent<Image>();
        //propImage.sprite = ResourceManager.Load<Sprite>(vo.IconPath);
        //propImage.SetNativeSize();
        PropUtils.SetPropItemIcon(vo, propImage, ModuleConfig.MODULE_ACTIVITYTEMPLATE);
    }

    
    
}
