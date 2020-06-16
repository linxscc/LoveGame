using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using GalaAccount.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Module.Login
{
    public class UserInfoView : Assets.Scripts.Framework.GalaSports.Core.View
    {
        private RawImage _headIcon;
        private RawImage _headFrame;
        private PlayerVo data;
        private void Awake()
        {
            _headIcon = transform.Find("HeadIconMask/HeadIcon").GetComponent<RawImage>();
            _headFrame =transform.Find("HeadIconMask/HeadFrame").GetComponent<RawImage>();
        }

        public void OnShow()
        {
            var userOther = GlobalData.PlayerModel.PlayerVo.UserOther;
            _headIcon.texture = ResourceManager.Load<Texture>(GlobalData.DiaryElementModel.GetHeadPath(userOther.Avatar, ElementTypePB.Avatar));
            _headFrame.texture = ResourceManager.Load<Texture>(GlobalData.DiaryElementModel.GetHeadPath(userOther.AvatarBox,ElementTypePB.AvatarBox));
                
        }

       
        public PlayerVo GetUserInfo() { return data; }
      
        public void InitData(PlayerVo vo)
        {
            data = vo;
           
            transform.Find("NameBg/NameTxt").GetComponent<Text>().text = vo.UserName;
            transform.Find("LevelTxt").GetComponent<Text>().text = "Lv." + vo.Level;

//            PointerClickListener.Get(_headIcon.gameObject).onClick = go => { FlowText.ShowMessage("功能暂未开放，敬请期待"); };
            
            //SetHeadIcon(vo.BgPicId);
            //todo
//            vo.LogoId
        }

        public void SetLevel(int level)
        {
            transform.Find("LevelTxt").GetComponent<Text>().text = level + ""; 
        }
    }
}