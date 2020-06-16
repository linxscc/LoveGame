using System;
using System.Collections.Generic;
using Assets.Scripts.Module.Framework.Utils;
using Assets.Scripts.Services;
using game.main;


namespace DataModel
{
    public class ClientData
    {
        /// <summary>
        /// 玩家选择的关卡
        /// </summary>
        public static LevelVo CustomerSelectedLevel;

        public static CapsuleLevelVo CustomerSelectedCapsuleLevel;
        
        public static Dictionary<string, string[]> JumpDataDict;
//        public static Dictionary<string, string> TextInfoDict;
        public static Dictionary<string, string> ItemDescDict;
        public static Dictionary<string, string> SpecialDescDice;
        
        public static Dictionary<int, string> ErrorCodeDict;
        public static Dictionary<int, List<ExpressionInfo>> ExpressionInfoDict;//Key为NpcId
        public static Dictionary<int, PhoneUnlockInfo> PhoneUnlockInfoDict;//Key为sceneId
        static Random rand = new Random();

        public static void LoadJumpData(Action<Dictionary<string, string[]>> onLoaded)
        {
            if (JumpDataDict!=null&&JumpDataDict.Count>0)
            {
                return;
            }
            
            new JumpDataService().SetCallback(dict =>
            {
                JumpDataDict = dict;
                onLoaded?.Invoke(JumpDataDict);
            }).Execute();
        }

        public static void LoadItemDescData(Action<Dictionary<string,string>> onLoaded)
        {
            if (ItemDescDict!=null&&ItemDescDict.Count>0)
            {
                return;
            }
            
            
            new AssetLoader().LoadText(AssetLoader.GetItemDescDataPath(), (text, loader) =>
            {
                ItemDescDict=new Dictionary<string, string>();
                var strings = text.Split('\n');
                for (int i = 0; i <strings.Length ; i++)
                {
                    int index = strings[i].IndexOf(',');
                    if(index == -1)
                        continue;

                    string[] arr = strings[i].Split(','); 
//                    Debug.LogError(arr[0]);
                    ItemDescDict.Add(arr[0],strings[i]);
                }
                onLoaded?.Invoke(ItemDescDict);
            });

        }
        
        public static void LoadSpecialItemDescData(Action<Dictionary<string,string>> onLoaded)
        {
            if (SpecialDescDice!=null&&SpecialDescDice.Count>0)
            {
                return;
            }
            new AssetLoader().LoadText(AssetLoader.GetSpecialItemDescDataPath(), (text, loader) =>
            {
                SpecialDescDice=new Dictionary<string, string>();
                var strings = text.Split('\n');
                for (int i = 0; i <strings.Length ; i++)
                {
                    int index = strings[i].IndexOf(',');
                    if(index == -1)
                        continue;

                    string[] arr = strings[i].Split(','); 
//                    Debug.LogError(arr[0]);
                    SpecialDescDice.Add(arr[0],strings[i]);
                }
                onLoaded?.Invoke(SpecialDescDice);
            });

        }
        public static void LoadPhoneUnlockData(Action<Dictionary<int, PhoneUnlockInfo>> onLoaded)
        {
            if(PhoneUnlockInfoDict!=null)
            {
                onLoaded?.Invoke(PhoneUnlockInfoDict);
            }
            new AssetLoader().LoadText(AssetLoader.GetPhoneUnlockDataPath(), (text, loader) =>
            {
                PhoneUnlockInfoDict = new Dictionary<int, PhoneUnlockInfo>();
                var strings = text.Split('\n');
                for (int i = 1; i < strings.Length; i++)
                {
                    int index = strings[i].IndexOf(',');
                    if (index == -1)
                        continue;
                    string[] arr = strings[i].Split(',');
                    if (arr[0] == "")
                        continue;
                    PhoneUnlockInfo info = new PhoneUnlockInfo(arr);
                    PhoneUnlockInfoDict[info.sceneId] = info;
                 
                }
                onLoaded?.Invoke(PhoneUnlockInfoDict);
            });
        }
        public static PhoneUnlockInfo GetPhoneUnlockInfoById(int sceneID)
        {
            if(PhoneUnlockInfoDict==null)
            {
                return null;
            }
            if(!PhoneUnlockInfoDict.ContainsKey(sceneID))
            {
                return null;
            }
            return PhoneUnlockInfoDict[sceneID];
        }

        public static void LoadExpressiongData(Action<Dictionary<int, List<ExpressionInfo>>> onLoaded)
        {
            if (ExpressionInfoDict != null) 
            {
                onLoaded?.Invoke(ExpressionInfoDict);
            }
            new AssetLoader().LoadText(AssetLoader.GetExpresssionDataPath(), (text, loader) =>
            {
                ExpressionInfoDict = new Dictionary<int, List<ExpressionInfo>>();
                var strings = text.Split('\n');
                for (int i = 3; i < strings.Length; i++)
                {
                    int index = strings[i].IndexOf(',');
                    if (index == -1)
                        continue;
                    string[] arr = strings[i].Split(',');
                    if (arr[0] == "")
                        continue;
                    ExpressionInfo info = new ExpressionInfo(arr);
                    if(!ExpressionInfoDict.ContainsKey(info.NpcId))
                    {
                        ExpressionInfoDict[info.NpcId] = new List<ExpressionInfo>();
                    }
                    ExpressionInfoDict[info.NpcId].Add(info);
                }
                onLoaded?.Invoke(ExpressionInfoDict);
            });
        }
        /// <summary>
        /// 通过对话ID等到标签ID
        /// </summary>
        /// <param name="NpcId"></param>
        /// <param name="expressonType"></param>
        /// <param name="dialogId"></param>
        /// <returns></returns>
        public static int GetExpressionLabelIdByDialogID(int NpcId, EXPRESSIONTRIGERTYPE expressonType,int dialogId)
        {
            if (!ExpressionInfoDict.ContainsKey(NpcId))
                return -1;
            ExpressionInfo expressionInfo = null;
            for (int i = 0; i < ExpressionInfoDict[NpcId].Count; i++)
            {
                expressionInfo = ExpressionInfoDict[NpcId][i];
                if (expressionInfo.TriggerType != expressonType)
                    continue;
                if (int.Parse( expressionInfo.DialogId) != dialogId)
                    continue;
                return int.Parse(expressionInfo.LabelId);
            }
            return -1;

        }


        public static List<ExpressionInfo>  GetDrawCardExpressionInfos(int NpcId, EXPRESSIONTRIGERTYPE expressionType )
        {
            if (!ExpressionInfoDict.ContainsKey(NpcId))
                return null;
            ExpressionInfo expressionInfo = null;
            List<ExpressionInfo> list = new List<ExpressionInfo>();
            for (int i = 0; i < ExpressionInfoDict[NpcId].Count; i++)
            {
                expressionInfo = ExpressionInfoDict[NpcId][i];
                if (expressionInfo.TriggerType== expressionType
                    ) 
                {
                    list.Add(expressionInfo);
                }
            }
            return list;
        }


        /// <summary>
        /// 根据配置表，获取角色对应触发条件expressonType的随机表情
        /// </summary>
        /// <param name="NpcId"></param>
        /// <param name="expressonType"></param>
        /// <returns></returns>
        public static ExpressionInfo GetRandomExpression(int NpcId, EXPRESSIONTRIGERTYPE expressonType, int labelId = -1)
        {
            ExpressionInfo expressionInfo = null;
            if (!ExpressionInfoDict.ContainsKey(NpcId))
                return null;

            List<ExpressionInfo> list = new List<ExpressionInfo>();
            for (int i = 0; i < ExpressionInfoDict[NpcId].Count; i++) 
            {
                expressionInfo = ExpressionInfoDict[NpcId][i];
                if (CheckIsSatisfaction(expressionInfo, expressonType, labelId)) 
                    //expressionInfo.IsTimeRange(DateUtil.GetTodayDt())) 
                {
                    list.Add(expressionInfo);
                }
            }

            return GetRandomExpression(list);
        }
        public static ExpressionInfo GetRandomGiftExpression(int NpcId, int GiftId = -1)
        {
            ExpressionInfo expressionInfo = null;
            if (!ExpressionInfoDict.ContainsKey(NpcId))
                return null;

            List<ExpressionInfo> list = new List<ExpressionInfo>();
            for (int i = 0; i < ExpressionInfoDict[NpcId].Count; i++)
            {
                expressionInfo = ExpressionInfoDict[NpcId][i];
                if (expressionInfo.TriggerType == EXPRESSIONTRIGERTYPE.GIFT &&
                  int.Parse(expressionInfo.GiftId) == GiftId) 
                {
                   
                    list.Add(expressionInfo);
                }
            }
            return GetRandomExpression(list);
        }

        private static bool CheckIsSatisfaction(ExpressionInfo info, EXPRESSIONTRIGERTYPE expressonType, int labelId)
        {
            if (info.TriggerType != expressonType)
                return false;
            if (!info.IsTimeRange(DateUtil.GetTodayDt()))
                return false;
            if (labelId > 0)
            {
                if (info.LabelId == labelId.ToString()) 
                {
                    return GlobalData.DiaryElementModel.IsCanUseElement(int.Parse(info.DialogId));
                }
                return false;
            }
            return true;
        }


        private static ExpressionInfo GetRandomExpression(List<ExpressionInfo> list)
        {
            int totalWeight = 0;//计算总共权重
            for (int i = 0; i < list.Count; i++) 
            {
                totalWeight += list[i].Weight;
            }
            int randWeight = (int)(rand.NextDouble() * totalWeight);
            for (int i = 0; i < list.Count; i++)
            {
                if (randWeight <= list[i].Weight)
                {
                    return list[i];
                }
                randWeight -= list[i].Weight;
            }
            return null;
           
        }

        public static ExpressionInfo GetRandomExpression(string modelId, EXPRESSIONTRIGERTYPE expressionType)
        {
            int npcId = 0;
            bool isConversionSuccessful = int.TryParse(modelId, out npcId);
            if (isConversionSuccessful)
            {
                return GetRandomExpression((npcId/100)%10, expressionType);
            }
            else
            {
                return null;
            }
         
        }

        public static void LoadErrorCode()
        {
            string text = new AssetLoader().LoadTextSync(AssetLoader.GetErrorCodePath());
            ErrorCodeDict = new Dictionary<int, string>();
            var strings = text.Split('\n');
            for (int i = 0; i < strings.Length; i++)
            {
                var arr = strings[i].Split('\t');
                int code = Convert.ToInt32(arr[0].Trim());
                string value = arr[1].Trim();
                ErrorCodeDict.Add(code, value);
            }
        }
        
        public static JumpData[] GetJumpDataById(int id)
        {
            List<JumpData> list = new List<JumpData>();
            if (!JumpDataDict.ContainsKey(id.ToString()))
            {
                FlowText.ShowMessage(I18NManager.Get("Common_NoPropOutput"));// ("暂无此道具出处");
                return null;
            }
            
            string[] arr = JumpDataDict[id.ToString()];
            for (int i = 1; i < arr.Length; i++)
            {
                if (string.IsNullOrEmpty(arr[i].Trim()))
                    continue;
               
                JumpData data = new JumpData();
                data.ParseData(arr[i]);
                list.Add(data);
            }

            return list.ToArray();
        }


        public static DescData GetItemDescById(int id,ResourcePB resourcePb=ResourcePB.Item)
        {
            //            switch (id)
            //            {
            //                case PropConst.GoldIconId:
            //                case PropConst.GemIconId:
            //                case PropConst.PowerIconId:
            //                    DescData descData=new DescData(); 
            //                    descData.ParseSpecial(SpecialDescDice[id.ToString()]);                   
            //                    return descData ;
            //            }

            if (resourcePb!=ResourcePB.Item)
            {
                if (GetSpecialItemDescById(id,resourcePb) != null)
                {
                    return GetSpecialItemDescById(id,resourcePb);
                }
            }



            if (!ItemDescDict.ContainsKey(id.ToString()))
            {
                FlowText.ShowMessage(I18NManager.Get("Common_NoPropOutput"));// ("暂无此道具出处");
                return null;
            }

            string arr = ItemDescDict[id.ToString()];
            DescData data = new DescData();
            data.ParseData(arr);
            return data;
        }
        
        public static DescData GetSpecialItemDescById(int id,ResourcePB resourcePb=ResourcePB.Item)
        {
            if (resourcePb!=ResourcePB.Item)
            {
               // FlowText.ShowMessage(I18NManager.Get("Common_NoPropOutput"));// ("暂无此道具出处");
                DescData resourece=new DescData(); 
                switch (resourcePb)
                {
                    case ResourcePB.Gold:
                        resourece.ParseSpecial(SpecialDescDice["10001"]);
                        return resourece;
                    case ResourcePB.Power:
                        resourece.ParseSpecial(SpecialDescDice["20001"]);
                        return resourece;
                    case ResourcePB.Gem:
                        resourece.ParseSpecial(SpecialDescDice["30001"]);
                        return resourece;
                    case ResourcePB.EncouragePower:
                        resourece.ParseSpecial(SpecialDescDice["40001"]);
                        return resourece;
                    case ResourcePB.Memories:
                        resourece.ParseSpecial(SpecialDescDice["50001"]);
                        return resourece;
                    case ResourcePB.Puzzle:
                        resourece.ParseSpecial(SpecialDescDice["200001"]);
                        return resourece;
                    case ResourcePB.Fans:
                        if (SpecialDescDice.ContainsKey("300001"))
                        {
                            resourece.ParseSpecial(SpecialDescDice["300001"]);
                        }
                        return resourece;
                    case ResourcePB.Card:
                        resourece.ParseSpecial($"{id},0,星缘,星缘：{GlobalData.CardModel.GetCardBase(id).CardName}");
                        return resourece;
                }
                
//                return null;                 
            }

            string arr = "";

            if (SpecialDescDice.ContainsKey(id.ToString()))
            {                                    
                 arr = SpecialDescDice[id.ToString()];
            }
            DescData data=new DescData();
            if (!String.IsNullOrEmpty(arr))
            {
                data.ParseSpecial(arr);
                return data; 
            }

            return null;

        }
        
        public static void Clear()
        {
//            JumpDataDict?.Clear();
//            JumpDataDict = null;
        }
        
        public static List<List<LevelData>> LoadLevelData()
        {
            string text = new AssetLoader().LoadTextSync(AssetLoader.GetLevelDataPath());
            List<LevelData> list = new List<LevelData>();
            List<List<LevelData>> chapterList = new List<List<LevelData>>();
            
            var strings = text.Split('\n');
            for (int i = 1; i < strings.Length; i++)
            {
                int index = strings[i].IndexOf(',');
                if(index == -1)
                    continue;

                int chapter = (i-1) / 31;
                if (chapter >= chapterList.Count)
                {
                    chapterList.Add(new List<LevelData>());
                }

                string[] arr = strings[i].Trim().Split(',');

                LevelData vo = new LevelData
                {
                    index = Convert.ToInt32(arr[0]),
                    levelId = Convert.ToInt32(arr[1]),
                };
                if (arr.Length > 2)
                    vo.itemId = arr[2];
                chapterList[chapter].Add(vo);
            }

            return chapterList;
        }


        public static List<DrawDialogInfo> LoadDrawCardDialogData()
        {
            string text = new AssetLoader().LoadTextSync(AssetLoader.GetDrawCardDialogDataPath());
            List<DrawDialogInfo> list = new List<DrawDialogInfo>();
            var strings = text.Split('\n');

            for (int i = 1; i < strings.Length; i++)
            {
                int index = strings[i].IndexOf(',');
                if (index == -1)
                    continue;
                string[] arr = strings[i].Trim().Split(',');
                DrawDialogInfo vo = new DrawDialogInfo(arr);
                list.Add(vo);
            }
            return list;
        }
    }
}