using game.main;
using GalaSDKBase;
using UnityEngine;
using Framework.Utils;
using DataModel;
using Assets.Scripts.Framework.GalaSports.Service;
using Utils;
using Assets.Scripts.Module.NetWork;
using Com.Proto;

namespace Assets.Scripts.Module.Sdk
{
    public class ShareAgent
    {

        public ShareAgent()
        {

            GalaSDKBaseCallBack.Instance.GALASDKShareSuccessEvent += OnShareSuccess;
            GalaSDKBaseCallBack.Instance.GALASDKShareFailEvent += OnShareFail;

        }
        public void ShareCloth(int backId,int clothId)
        {
            IconSelectWindow win = PopupManager.ShowWindow<IconSelectWindow>(Constants.IconSelectWindowPath);
            win.clickCallback = (m) =>
            {
                ScreenShotUtil.ScreenShot(ScreenShotType.Clothes,
                    (imageUrl) =>{
                        ScreenShotCallback(m, imageUrl,ShareTypePB.ShareClothes);
                    }, backId, clothId);

            };
            win.SetData(
                I18NManager.Get("Common_ShareTo"),
                IconType.WeChatFriend,
                IconType.WeChatFriendCircle,
                IconType.QQFriend,
                IconType.SinaWeibo
                );
        }
        
        public void ShareMusicGameResult(MusicRhythmRunningInfo data)
        {
            IconSelectWindow win = PopupManager.ShowWindow<IconSelectWindow>(Constants.IconSelectWindowPath);
            win.clickCallback = (m) =>
            {
                ScreenShotUtil.ScreenShot(ScreenShotType.MusicGame,
                    (imageUrl) =>{
                        ScreenShotCallback(m, imageUrl,ShareTypePB.ShareClothes);
                    }, data);
            };
            win.SetData(
                I18NManager.Get("Common_ShareTo"),
                IconType.WeChatFriend,
                IconType.WeChatFriendCircle,
                IconType.QQFriend,
                IconType.SinaWeibo
            );
        }

        void ScreenShotCallback(IconType m, string imageUrl, ShareTypePB pb)
        {
            GalaSDKBaseFunction.GalaShareWayType galaShareWayType;
            switch (m)
            {
                case IconType.QQFriend:
                    galaShareWayType = GalaSDKBaseFunction.GalaShareWayType.QQFriend;
                    break;
                case IconType.SinaWeibo:
                    galaShareWayType = GalaSDKBaseFunction.GalaShareWayType.SinaWeibo;
                    break;
                case IconType.WeChatFriend:
                    galaShareWayType = GalaSDKBaseFunction.GalaShareWayType.WeChatFriend;
                    break;
                case IconType.WeChatFriendCircle:
                    galaShareWayType = GalaSDKBaseFunction.GalaShareWayType.WeChatFriendCircle;
                    break;
                default:
                    return;
            }
            SdkHelper.ShareAgent.Share(galaShareWayType, GalaSDKBaseFunction.GalaShareType.Image, imageUrl);
            //ShareTypePB pb;
            //pb = ShareTypePB.ShareDraw;
            SendShareRewards(pb);
        }
        public void ShareDrawCard(DrawCardResultVo drawCardResultVo)
        {
            IconSelectWindow win = PopupManager.ShowWindow<IconSelectWindow>(Constants.IconSelectWindowPath);
            win.clickCallback = (m) =>
            {
                ScreenShotUtil.ScreenShot(ScreenShotType.DrawCard,
                        (imageUrl) =>
                        {
                            ScreenShotCallback(m, imageUrl, ShareTypePB.ShareDraw);
                        }, drawCardResultVo);
            };
            win.SetData(
                I18NManager.Get("Common_ShareTo"),
                IconType.WeChatFriend,
                IconType.WeChatFriendCircle,
                IconType.QQFriend,
                IconType.SinaWeibo
                );
        }

        private static void SendShareRewards(ShareTypePB shareTypePB)
        {
            //已经领取过不在发送
            if (GlobalData.PlayerModel.PlayerVo.IsGetShareAward(shareTypePB))
            {
                return;
            }

            int id = (int)shareTypePB + 1;
            ShareRewardsReq req = new ShareRewardsReq()
            {
                Id = id
            };
            Debug.LogError("SendShareRewards" + id);
            var buffer = NetWorkManager.GetByteData(req);
            NetWorkManager.Instance.Send<ShareRewardsRes>(CMD.USERC_SHAREREWARD, buffer, OnShareRewardsHandle);
        }

        private static void OnShareRewardsHandle(ShareRewardsRes res)
        {
            Debug.LogError("OnShareRewardsHandle");
            GlobalData.PlayerModel.UpdataUserExtra(res.UserExtraInfo);
            RewardUtil.AddReward(res.Award);
            //todoShowAward           
            var awardWindow = PopupManager.ShowWindow<AwardWindow>("GameMain/Prefabs/AwardWindow/AwardWindow");

            awardWindow.SetData(res.Award);
        }

        public void Share(GalaSDKBaseFunction.GalaShareWayType galaShareWayType,GalaSDKBaseFunction.GalaShareType galaShareType,
            string imageUrl="",string content="",string title="",string link="")
        {
            GalaSDKBaseFunction.Share(galaShareWayType,galaShareType,imageUrl,content,title,link);
        }

        private void OnShareFail()
        {
            Debug.LogError("分享失败");
        }

        private void OnShareSuccess(string data)
        {
            Debug.LogError("分享成功--------------->"+data);
            FlowText.ShowMessage(I18NManager.Get("Common_ShareSuccess"));
        }


        
    }
}