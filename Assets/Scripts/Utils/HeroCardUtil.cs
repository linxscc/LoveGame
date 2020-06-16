using game.main;
using UnityEngine;

namespace game.tools
{
    public class HeroCardUtil
    {
        public static Sprite GetQualityImage(CreditPB quality)
        {
            Sprite sp;
            string suffix = "R";
            if (quality == CreditPB.Ssr)
            {
                suffix = "SSR";
            }else if (quality == CreditPB.Sr)
            {
                suffix = "SR";
            }

            sp =  AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_new" + suffix);
            return sp;
        }
    }
}