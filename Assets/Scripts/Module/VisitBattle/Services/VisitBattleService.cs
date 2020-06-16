    using Assets.Scripts.Framework.GalaSports.Core;
    using Assets.Scripts.Module.NetWork;
    using Com.Proto;
    using Module.VisitBattle.Data;

    public class VisitBattleService : RemoteService<ChallengeRes>
    {
        protected override void OnExecute()
        {
            InitData(CMD.VISITINGC_CHALLENGE, requstBytes);
        }

        protected override void ProcessData()
        {
            
        }
    }
