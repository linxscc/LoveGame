using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Module.Supporter.Data;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class FansItem : MonoBehaviour
    {
        private Transform _bgLeft;
        private Transform _bgRight;

        private void Awake()
        {
            _bgLeft = transform.Find("BgLeft");
            _bgRight = transform.Find("BgRight");
        }
        
        public void SetData(FansVo vo1, FansVo vo2)
        {
            SetFansData(vo1, _bgLeft);
            SetFansData(vo2, _bgRight);
        }

        private void SetFansData(FansVo vo, Transform target)
        {
            if (vo == null)//||vo?.Num<=0 //现在粉丝数量为0也可以展示了
            {
                target.gameObject.Hide();
                return;
            }
            target.gameObject.Show();
           
            target.Find("DescriptionText").GetComponent<Text>().text = vo.Description;
            //target.Find("NumTag/NumText").GetComponent<Text>().text = "人数：" + vo.Num;
            target.Find("NumText").GetComponent<Text>().text = I18NManager.Get("Supporter_Num", vo.Num);
            target.Find("NameText").GetComponent<Text>().text = vo.Name;
            //target.Find("Power").GetComponent<Text>().text ="应援能力:"+   $"<color={vo.FansTextColor}>{vo.Power}</color>";
            target.Find("Power").GetComponent<Text>().text = I18NManager.Get("Supporter_SupporterAbility", vo.FansTextColor, vo.Power);
            target.Find("Name").GetComponent<Image>().sprite =  AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_fansNameTag" + vo.FansId);
            target.Find("NumTag").GetComponent<Image>().sprite= AssetManager.Instance.GetSpriteAtlas("UIAtlas_Supporter_fansNumTag" + vo.FansId);
            target.Find("Fans/Mask/Image").GetComponent<RawImage>().texture =
                ResourceManager.Load<Texture>(vo.FansTexturePath,ModuleConfig.MODULE_SUPPORTER);//"UIAtlas_Supporter_Fan" + vo.FansId

        }
    }
}