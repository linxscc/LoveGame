using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Module.Supporter.Data;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class BattleFansItem:MonoBehaviour
    {
        public void SetData(int num,int max, FansVo fansVo)
        {
            transform.Find("FansItemNum/NumText").GetComponent<Text>().text =  num+"/"+max;
            transform.Find("FansItemName/NameText").GetComponent<Text>().text = fansVo.Name;
            transform.Find("FansItemName").GetComponent<Image>().sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_fansNameTag" + fansVo.FansId);
            RawImage tipText = transform.Find("TipText").GetComponent<RawImage>();
            tipText.gameObject.SetActive(num == 0);

            RawImage fansImage = transform.Find("Mask/fansImage").GetComponent<RawImage>();
            fansImage.texture = ResourceManager.Load<Texture>(fansVo.FansBigTexturePath, ModuleConfig.MODULE_BATTLE);
            
            if (num == 0)
            {
                fansImage.color = Color.grey;;
            }
        }
    }
}