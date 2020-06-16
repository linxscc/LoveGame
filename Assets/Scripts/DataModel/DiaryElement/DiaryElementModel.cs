using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using Google.Protobuf.Collections;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DataModel
{
    public class DiaryElementModel : Model
    {
        //public DiaryElementModel()
        //{
        //}
        private RepeatedField<ElementPB> _rules; //元素规则
        private RepeatedField<UserElementPB> _userElement; //玩家元素

        /// <summary>
        /// 元素规则
        /// </summary>
        public List<ElementPB> BaseElementRule => _rules.ToList();

        public void InitElementRule(ElementRes res)
        {
            _rules = res.Elements;
        }

        public void InitMyElement(MyElementRes res)
        {
            _userElement = res.UserElements;
        }


        /// <summary>
        /// 获取头像（框）路径
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetHeadPath(int id,ElementTypePB type)
        {
            string path = "";

            foreach (var rule in _rules)
            {
                if (rule.Id == id)
                {
                    if (type== ElementTypePB.AvatarBox)
                    {
                        path = "HeadFrame/" + rule.Id;
                        break;
                    }
                    else if(type == ElementTypePB.Avatar)
                    {
                        var isOther = rule.UnlockClaim.CardId == 0;
                        if (isOther)
                        {
                            path = "Head/OtherHead/" + rule.Id;  
                        }
                        else
                        {
                            var isEvolutionBefore = rule.Id % 100 == 11;
                            path = isEvolutionBefore ? "Head/" + rule.Id/100 : "Head/EvolutionHead/" + rule.Id/100;
                        }
                        break;
                    }
                }
            }
            return path;
        }

        //供恋爱模块使用，查看是否已经有
        public void UpdateElement(int id, int num)
        {
            foreach (var v in _userElement)
            {
                if (v.ElementId != id) continue;
                v.Num += num;
                return;
            }

            _userElement.Add(new UserElementPB() {ElementId = id, Num = num});
        }

        public ElementPB GetElementRuleById(int id)
        {
            for (int i = 0; i < _rules.Count; i++)
            {
                ElementPB pb = _rules[i];
                if (pb.Id == id)
                    return pb;
            }

            return null;
        }

        /// <summary>
        /// 获取解锁unlockeds和未解锁lockeds的元素
        /// </summary>
        /// <param name="pbType"></param>
        /// <param name="unlockeds"></param>
        /// <param name="lockeds"></param>
        public void GetElementByType(ElementTypePB pbType, ElementModulePB moduleType, ref List<ElementPB> unlockeds,
            ref List<ElementPB> lockeds)
        {
            //fen 两个队列返回
            if (unlockeds != null)
            {
                unlockeds.Clear();
            }

            if (lockeds != null)
            {
                lockeds.Clear();
            }

            ElementPB pb;
            for (int i = 0; i < _rules.Count; i++)
            {
                pb = _rules[i];
                if (pb.ElementType != pbType || pb.ElementModule != moduleType)
                {
                    continue;
                }

                if (IsCanUseElement(pb.Id)) //pb.Gem == 0免费
                {
                    if (unlockeds == null)
                        continue;
                    unlockeds.Add(pb);
                }
                else
                {
                    if (lockeds == null)
                        continue;
                    lockeds.Add(pb);
                }
            }

            return;
        }

        public bool IsUserElement(int id)
        {
            for (int i = 0; i < _userElement.Count; i++)
            {
                if (_userElement[i].ElementId == id)
                {
                    // Debug.LogError(_userElement[i]);
                    return true;
                }
            }

            return false;
        }

        public bool IsCanUseElement(int id)
        {
            ElementPB pb = GetElementRuleById(id);
            // pb.NeedUnlock//1表示需要解锁；0表示不解锁
            if (pb.NeedUnlock == 0)
                return true;
            //if (//pb.UnlockClaim.Gem == 0
            //    //&&pb.UnlockClaim.LevelId==0&&
            //    //pb.UnlockClaim.EvolutionLevel == 0&&
            //    // pb.UnlockClaim.CardId==0
            //    ) 
            //{ return true; }
            for (int i = 0; i < _userElement.Count; i++)
            {
                if (_userElement[i].ElementId == id)
                {
                    return true;
                }
            }

            return false;
        }

        public ElementPB GetDialogByCardId(int cardId)
        {
            ElementPB pb = null;
            for (int i = 0; i < _rules.Count; i++)
            {
                pb = _rules[i];
                if (pb.UnlockClaim.CardId == cardId)
                {
                    break;
                }
            }

            return pb;

            // int labelId= ClientData.GetExpressionLabelIdByDialogID(
            //     (int)GlobalData.CardModel.GetCardBase(cardId).Player,
            //     EXPRESSIONTRIGERTYPE.LOVEDIARY,
            //     pb.Id
            //     );
            // return GetElementRuleById(labelId);
        }

        public List<ElementPB> GetDialogsByCardId(int cardId)
        {
            List<ElementPB> elementPBs = new List<ElementPB>();

            ElementPB pb = null;
            for (int i = 0; i < _rules.Count; i++)
            {
                pb = _rules[i];
                if (pb.UnlockClaim.CardId == cardId)
                {
                    elementPBs.Add(pb);
                    continue;
                }
            }

            //int labelId = ClientData.GetExpressionLabelIdByDialogID(
            //    (int)GlobalData.CardModel.GetCardBase(cardId).Player,
            //    EXPRESSIONTRIGERTYPE.LOVEDIARY,
            //    pb.Id
            //    );
            return elementPBs;
        }
    }
}