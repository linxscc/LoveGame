using UnityEngine;

namespace game.tools
{
    public class CardUtil
    {
        public static string GetCreditSpritePath(CreditPB credit)
        {
            string spName = "UIAtlas_Common_R";
            if (credit == CreditPB.Ssr)
            {
                spName = "UIAtlas_Common_SSR";
            }
            else if(credit == CreditPB.Sr)
            {
                spName = "UIAtlas_Common_SR";
            }

            return spName;
        }
        
        public static string GetNewCreditSpritePath(CreditPB credit)
        {
            string spName = "UIAtlas_Common_newR";
            if (credit == CreditPB.Ssr)
            {
                spName = "UIAtlas_Common_newSSR";
            }
            else if(credit == CreditPB.Sr)
            {
                spName = "UIAtlas_Common_newSR";
            }

            return spName;
        }
        
        public static string GetCreditSpritePath(SortCredit credit)
        {
            string spName = "UIAtlas_Common_R";
            if (credit == SortCredit.SSR)
            {
                spName = "UIAtlas_Common_SSR";
            }
            else if (credit == SortCredit.SR)
            {
                spName = "UIAtlas_Common_SR";
            }
            return spName;
        }

        public static string GetNewCreditSpritePath(SortCredit credit)
        {
            string spName = "UIAtlas_Common_newR";
            if (credit == SortCredit.SSR)
            {
                spName = "UIAtlas_Common_newSSR";
            }
            else if (credit == SortCredit.SR)
            {
                spName = "UIAtlas_Common_newSR";
            }
            return spName;
        }
        
        public static string GetBigCardPath(int CardId)
        {
            return "Card/Image/" + CardId;
        }

        public static string GetMiddleCardPath(int CardId)
        {
            return "Card/Image/MiddleCard/" + CardId;
        }

        public static string GetSmallCardPath(int CardId)
        {
            return "Card/Image/SmallCard/" + CardId;
        }
        public static string GetBigFunsCardPath(int CardId)
        {
            return "FansTexture/BigCards/" + CardId;
        }

        public static string GetCardSignaturePath(int CardId)
        {
            return "Prop/Signature/sign"+CardId/1000;
        }

    }
}