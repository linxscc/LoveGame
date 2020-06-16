using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Module.Task.Service;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using UnityEngine;
using Utils;

namespace game.main
{
    public class AchievementChooseController : Controller
    {
        public AchievementChooseView View;
        private MissionModel _missionModel;


        public override void Start()
        {
            //获取用户任务的数据源
          //  _missionModel = GetData<MissionModel>();
            _missionModel = GlobalData.MissionModel;
            GetData();           
        }
        
        public void GetData()
        {
            GetService<MissionService>().SetCallback(GetUserData).Execute();
        }
        
        private void GetUserData(MissionModel model)
        {
            _missionModel = model;
            View.SetData(_missionModel);
        }
    
    
        public void SetViewData()
        {
            View.SetData(_missionModel);
        }


        /// <summary>
        /// 处理View消息
        /// </summary>
        /// <param name="message"></param>
        public override void OnMessage(Message message)
        {
            string name = message.Name;
            object[] body = message.Params;
            switch (name)
            {
            }
        }


        public override void Destroy()
        {
            base.Destroy();
        }
    }
}