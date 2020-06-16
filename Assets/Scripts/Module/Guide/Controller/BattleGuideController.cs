using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Guide.ModuleView;
using Com.Proto;
using Common;
using DataModel;

namespace Game.Guide
{
    public class BattleGuideController : Controller
    {
        private bool _isShowFail2_4;
        private bool _isShowFail3_3;
        public BattleGuideView View;

        public override void OnMessage(Message message)
        {
            var isPass = GlobalData.LevelModel.FindLevel("1-5").IsPass;

            var name = message.Name;
            var body = message.Params;
            switch (name)
            {
                case MessageConst.CMD_BATTLE_SHOW_SUPPORTER_VIEW:
                    if (isPass)
                        return;

                    View.gameObject.Show();
                    ClientTimer.Instance.DelayCall(View.ShowSupport, 1.0f);
                    break;

                case MessageConst.TO_GUIDE_BATTLE_SUPERSTAR_START:
                    if (isPass)
                        return;

                    View.gameObject.Show();
                    ClientTimer.Instance.DelayCall(() => { View.ShowSuperStar((LevelVo) body[0]); }, 1.0f);
                    break;

                case MessageConst.CMD_BATTLE_SHOW_REWARD:
                    if (isPass)
                        return;

                    ClientTimer.Instance.DelayCall(View.ShowReward, 1f);
                    break;

                case MessageConst.TO_GUIDE_BATTLE_RESULT:
                    var res = (ChallengeRes) body[0];
                    var level = (LevelVo) body[1];
                    if (res.GameResult.Star > 0)
                    {
                        GuideManager.SetPassLevelStep(level.LevelName);
                        return;
                    }

                    if (level.LevelId == 204 && level.IsPass == false &&
                        GuideManager.CurStage() < GuideStage.MainStep_MainStory2_4_Fail) _isShowFail2_4 = true;

                    if (level.LevelId == 303) _isShowFail3_3 = true;
                    break;
                case MessageConst.MODULE_BATTLE_SHOW_FAIL:
                    View.gameObject.Show();
                    if (_isShowFail2_4)
                        ClientTimer.Instance.DelayCall(View.ShowFail2_4, 1f);
                    else if (_isShowFail3_3) ClientTimer.Instance.DelayCall(View.ShowFail3_3, 1f);
                    break;
            }
        }
    }
}