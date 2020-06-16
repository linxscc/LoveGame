using System;
using Com.Proto;
using DataModel;
using game.tools;
using UnityEngine;

namespace DataModel
{
    public class RewardVo
    {
        public int Num;
        public string Name;
        public string IconPath;

        public int Id;
        public ResourcePB Resource;

        private bool _autoUpdateData;

        /// <summary>
        /// 奖励数据
        /// </summary>
        /// <param name="award"></param>
        /// <param name="autoUpdateData">自动更新数据</param>
        public RewardVo(AwardPB award, bool autoUpdateData = false)
        {
            _autoUpdateData = autoUpdateData;
            InitAward(award);
        }

        private void InitAward(AwardPB award)
        {
            Id = award.ResourceId;
            Resource = award.Resource;

            switch (award.Resource)
            {
                case ResourcePB.Card:
                    IconPath = "Head/" + Id;
                    break;
                case ResourcePB.Item:
                    IconPath = "Prop/" + award.ResourceId;
                    if (_autoUpdateData)
                        GlobalData.PropModel.AddProp(award);
                    break;
                case ResourcePB.Puzzle:
                    IconPath = "Head/" + award.ResourceId;
                    if (_autoUpdateData)
                    {
                        GlobalData.CardModel.AddUserPuzzle(award);
                    }
                    break;
                case ResourcePB.Power:
                    IconPath = "Prop/particular/" + PropConst.PowerIconId;
                    Id = PropConst.PowerIconId;
                    if (_autoUpdateData)
                        GlobalData.PlayerModel.AddPower(award.Num);
                    break;
                case ResourcePB.Gem:
                    IconPath = "Prop/particular/" + PropConst.GemIconId;
                    Id = PropConst.GemIconId;
                    if (_autoUpdateData)
                        GlobalData.PlayerModel.UpdateUserGem(award.Num);
                    break;
                case ResourcePB.Gold:
                    IconPath = "Prop/particular/" + PropConst.GoldIconId;
                    Id = PropConst.GoldIconId;
                    if (_autoUpdateData)
                        GlobalData.PlayerModel.UpdateUserGold(award.Num);
                    break;
                case ResourcePB.Fans:
                    IconPath ="FansTexture/Head/"+award.ResourceId;                    
                    break;
                case ResourcePB.Exp:
                    break;
                case ResourcePB.EncouragePower:
                    IconPath = "Prop/particular/" + PropConst.EncouragePowerId;
                    Id = PropConst.EncouragePowerId;
                    break;
                case ResourcePB.Favorability:
                    break;
                case ResourcePB.Memories:
                    IconPath = "Prop/particular/" + PropConst.RecolletionIconId;
                    Id =PropConst.RecolletionIconId; 
                    if (_autoUpdateData)
                        GlobalData.PlayerModel.AddRecollectionEnergy(award.Num);
                    break;
                case ResourcePB.Element:
                    var element = GlobalData.DiaryElementModel.GetElementRuleById(Id);
                    if (element==null)
                    {
                      Debug.LogError("找不到元素");  
                      return;
                    }
                    var type = element.ElementType;
                    switch (type)
                    {                     
                        case ElementTypePB.Avatar:
                            var isEvolutionBefore = Id % 100 == 11;
                            IconPath = isEvolutionBefore ? "Head/" + Id / 100 : "Head/EvolutionHead/" + Id / 100;
                            break;
                        case ElementTypePB.AvatarBox:
                            IconPath = "HeadFrame/"+Id;
                            break; 
                        default:
                            Debug.LogError("元素其他类型没设置图片路径");
                            break;
                    }
                                        
                    break;
            }

            if (award.Resource == ResourcePB.Item)
            {
                Name = GlobalData.PropModel.GetPropBase(award.ResourceId)?.Name;
            }
            else if(award.Resource == ResourcePB.Element)
            {
                var element = GlobalData.DiaryElementModel.GetElementRuleById(Id);
                Name = element==null ? "" : element.Name;
            }
            else
            {
                Name = ViewUtil.ResourceToString(award.Resource);
            }

            Num = award.Num;
        }
    }
}