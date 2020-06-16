using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Framework.GalaSports.Service;
using Google.Protobuf.Collections;

namespace Assets.Scripts.Module.TrainingRoom.Service
{
    public class StartMusicGameService : RemoteService<PlayingMusicRes>
    {
        public RemoteService<PlayingMusicRes> Request(PlayingMusicReq req)
        {
            requstBytes = NetWorkManager.GetByteData(req);
            return this;
        }   
        
        protected override void OnExecute()
        {
            InitData(CMD.MUSICGAMEC_PALYING, requstBytes);
        }

        protected override void ProcessData()
        {
            
        }
    }
}