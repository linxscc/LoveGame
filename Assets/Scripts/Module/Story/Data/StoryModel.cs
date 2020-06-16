#region 模块信息

// **********************************************************************
// Copyright (C) 2018 The 望尘体育科技
//
// 文件名(File Name):             StoryModel.cs
// 作者(Author):                  张晓宇
// 创建时间(CreateTime):           2018/3/2 10:21:41
// 修改者列表(modifier):
// 模块描述(Module description):
// **********************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Framework.GalaSports.Core;
using DataModel;
using Newtonsoft.Json;

namespace game.main
{
    public class StoryModel : Model
    {
        public LevelVo Level;
        public VisitLevelVo VisitLevel;

        public StoryType StoryType;
        private StringBuilder stringBuilder;
        public string StoryId;

        public void Reset()
        {
            Level = null;
            VisitLevel = null;
            stringBuilder = new StringBuilder();
        }

        public void LoadStroyById(string id, Action<List<DialogVo>> onComplete)
        {
            //            id = "xxx";
            StoryId = id;
            string text = new AssetLoader().LoadTextSync((AssetLoader.GetStoryDataPath(id)));
            text = text.Replace("{$player}", GlobalData.PlayerModel.PlayerVo.UserName);
            List<DialogVo> list = JsonConvert.DeserializeObject<List<DialogVo>>(text);
            onComplete(list);
        }
        
        public void LoadTelphoneById(string id, Action<TelephoneVo> onComplete)
        {
            string text = new AssetLoader().LoadTextSync(AssetLoader.GetStoryTelphoneDataPath(id));
            text = text.Replace("{$player}", GlobalData.PlayerModel.PlayerVo.UserName);
            TelephoneVo vo = JsonConvert.DeserializeObject<TelephoneVo>(text);
            onComplete(vo);
        }
        
        public void LoadSmsById(string id, Action<SmsVo> onComplete)
        {
            string text = new AssetLoader().LoadTextSync(AssetLoader.GetStorySmsDataPath(id));
            text = text.Replace("{$player}", GlobalData.PlayerModel.PlayerVo.UserName);
            SmsVo vo = JsonConvert.DeserializeObject<SmsVo>(text);
            onComplete(vo);
        }

        public string GetCurrentDialog()
        {
            return stringBuilder.ToString();
        }

        public void AddDialog(object obj, string heroName)
        {
            if(stringBuilder == null)
                stringBuilder = new StringBuilder();
            
            Type type = obj.GetType();
            if (type == typeof(EntityVo))
            {
                EntityVo entity = (EntityVo) obj;
                if(entity.id == "0")
                {
                    stringBuilder.Append("【旁白】");
                }
                else
                {
                    stringBuilder.Append("【")
                        .Append(heroName)
                        .Append("】");
                }

                stringBuilder.Append(entity.dialog)
                    .Append("\n\n");
            }
            else if(type == typeof(TelephoneDialogVo))
            {
                TelephoneDialogVo telDialogVo = (TelephoneDialogVo) obj;
                stringBuilder.Append("【")
                    .Append(heroName)
                    .Append("】");
                stringBuilder.Append(telDialogVo.Content)
                    .Append("\n\n");
            }
            else if(type == typeof(SmsDialogVo))
            {
                SmsDialogVo smsDialogVo = (SmsDialogVo) obj;
                stringBuilder.Append("【")
                    .Append(heroName)
                    .Append("】");
                stringBuilder.Append(smsDialogVo.ContextText)
                    .Append("\n\n");
            }
        }
    }

    public enum StoryType
    {
        MainStory,
        CreateUser,
        LoveAppointment,
        Visit,
        ActivityCapsule,
    }
}