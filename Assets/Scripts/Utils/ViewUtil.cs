using System;
using Google.Protobuf.Collections;
using UnityEngine;

namespace game.tools
{
    public class ViewUtil
    {
        public static string AbilitiesToString(RepeatedField<AbilityPB> abs)
        {
            string temp = "";
            //AbilityPB.
            foreach (var item in abs)
            {
                if (temp == "")
                {
                    temp = AbilityToString(item);
                }
                else
                {
                    temp += "、"+AbilityToString(item);
                }
                
            }

            return temp;
        }

        public static string AbilityToString(AbilityPB ab)
        {
            switch (ab)
            {
                case AbilityPB.Charm:
                    return I18NManager.Get("Common_Glamour");
                case AbilityPB.Composing:
                    return I18NManager.Get("Common_Original");
                case AbilityPB.Dancing:
                    return I18NManager.Get("Common_Dancing");
                case AbilityPB.Perseverance:
                    return I18NManager.Get("Common_Willpower");
                case AbilityPB.Popularity:
                    return I18NManager.Get("Common_Popularity");
                case AbilityPB.Singing:
                    return I18NManager.Get("Common_Sing");
            }

            return "";
        }
        
        public static string ResourceToString(ResourcePB ab)
        {
            switch (ab)
            {
                case ResourcePB.Card:
                    return I18NManager.Get("Card_CardTap");//Card_CardTap"卡片";
                case ResourcePB.Fans:
                    return I18NManager.Get("Common_Fans");
                case ResourcePB.Gem:
                    return I18NManager.Get("Common_Gem");//"星钻";
                case ResourcePB.Gold:
                    return I18NManager.Get("Common_Gold");//"金币";
                case ResourcePB.Item:
                    return I18NManager.Get("Common_Props");//"物品";
                case ResourcePB.Power:
                    return I18NManager.Get("Common_Power");//"体力";
                case ResourcePB.Puzzle:
                    return I18NManager.Get("Card_Puzzles");//"星缘碎片";
                case ResourcePB.Memories:
                    return I18NManager.Get("Common_RecolletionPuzzle");//"记忆碎片";
                case ResourcePB.Exp:
                    return I18NManager.Get("Common_ExpTxt");//"经验";
                case ResourcePB.EncouragePower:
                    return I18NManager.Get("Common_EncouragePower");//"应援活动体力";
                case ResourcePB.Favorability:
                    return I18NManager.Get("FavorabilityMain_TitleName");//"好感度";
                case ResourcePB.Element:
                    return I18NManager.Get("Common_ElementProps");//"特殊元素（日记、闹钟等）";
            }

            return "";
        }
    }
}