using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class UpgradeStarPropItem : MonoBehaviour
    {
        public UpgradeStarRequireVo Data;

        public void SetData(UpgradeStarRequireVo vo)
        {
            Data = vo;

            if (Data == null)
            {
                this.gameObject.Hide();
                return;
            }
            
            Text propName = transform.Find("Text").GetComponent<Text>();
            Text propNumTxt = transform.Find("Owned/Image/Text").GetComponent<Text>();

            propName.text = vo.PropName;
            propNumTxt.text = vo.CurrentNum + "/" +vo.NeedNum;

            RawImage image = transform.Find("PropImage").GetComponent<RawImage>();
            image.texture = ResourceManager.Load<Texture>("Prop/" + vo.PropId, ModuleConfig.MODULE_CARD);
//            if (sprite == null)
//            {
//                sprite = ResourceManager.Load<Sprite>("Prop/1100");
//            }
            //image.sprite = sprite;

        }

    }
}