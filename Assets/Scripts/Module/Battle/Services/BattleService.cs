    using Assets.Scripts.Framework.GalaSports.Core;
    using Assets.Scripts.Module.NetWork;
    using Com.Proto;
    using Module.Battle.Data;

    public class BattleService : RemoteService<ChallengeRes>
    {
        protected override void OnExecute()
        {
            InitData(CMD.CAREERC_CHALLENGE, requstBytes);
        }

        protected override void ProcessData()
        {
            
        }
    }
