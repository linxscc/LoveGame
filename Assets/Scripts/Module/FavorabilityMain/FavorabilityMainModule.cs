using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using UnityEngine;

public class FavorabilityMainModule : ModuleBase
{
    private FavorabilityMainPanel _favorabilityMainPanel; 
    private FavorabilityEnternPanel _favorabilityEnternPanel;
    private FavorabilityNpcInfoPanel _favorabilityNpcInfoPanel;
    private FavorabilityGiftPanel _favorabilityGiftPanel;      
    public UserFavorabilityVo RoleVo;
    



    public override void Init()
    {
        GuideManager.RegisterModule(this);

        SendVisitMyVisitingReq();
    }

    
    private void SendVisitMyVisitingReq()
    {
        bool isReachVisitingLevel =GlobalData.PlayerModel.PlayerVo.Level >=GuideManager.GetOpenUserLevel(ModulePB.Visiting, FunctionIDPB.VisitingEntry);

        if (isReachVisitingLevel)
        {
            NetWorkManager.Instance.Send<MyVisitingRes>(CMD.VISITINGC_MYVISITINGS, null, OnMyVisitingHandler);
        }     
    }
    
    private void OnMyVisitingHandler(MyVisitingRes res)
    {
        GlobalData.FavorabilityMainModel.OnMyVisitingHandler(res);        
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
//            case MessageConst.CMD_FAVORABILITY_SHOW_MAIN:           //显示主界面                          
//                _favorabilityMainPanel.Show(0);
//                break;
            case MessageConst.CMD_FACORABLILITY_COMETOMAIN:
                //这里要进入好感度主界面
                GlobalData.FavorabilityMainModel.CurrentRoleVo = (UserFavorabilityVo) body[0];
                if (_favorabilityMainPanel==null)
                {
                    _favorabilityMainPanel = new FavorabilityMainPanel();
                    _favorabilityMainPanel.Init(this); 
                }
                _favorabilityMainPanel.Show(0);                
                break;                             
            case MessageConst.CMD_FACORABLILITY_BACKTOFAVORABILITY:              
                _favorabilityMainPanel.Hide();             
                _favorabilityEnternPanel?.Show(0);               
                break;
//            case MessageConst.TO_GUIDE_FAVORABILITY_PHONE_EVENT_OVER:
                        //                _favorabilityEnternPanel.OnBackClick();                
                        //                break;
            case MessageConst.MODULE_FACORABLILITY_ON_SHOW_NPC_INFO:
                _favorabilityNpcInfoPanel=new FavorabilityNpcInfoPanel();
                _favorabilityNpcInfoPanel.Init(this);
                _favorabilityNpcInfoPanel.Show(0);
                break;
            case MessageConst.CMD_FACORABLILITY_ENTER_SEND_GIVE_GIFTS:              
                GameObject go = (GameObject) body[0];
                bool isJump = (bool) body[1];
                _favorabilityGiftPanel = new FavorabilityGiftPanel();
                _favorabilityGiftPanel.IsJump = isJump;
                _favorabilityGiftPanel.Init(this);
                _favorabilityGiftPanel.Show(0);
                _favorabilityGiftPanel.StarCreate(go.transform);                               
                break;
            case MessageConst.CMD_FACORABLILITY_DESTROY_PANEL:
                string panelName = (string)body[0];
                if (panelName=="SendGift")
                {
                     _favorabilityGiftPanel?.Destroy();                    
                }
                else if(panelName=="NpcInfo")
                {
                    _favorabilityNpcInfoPanel?.Destroy();
                }
                _favorabilityMainPanel.OnShowMainBtn();
               _favorabilityMainPanel.ShowBackBtn();
                break;
            case MessageConst.CMD_FACORABLILITY_GOBACK:
                ModuleManager.Instance.GoBack();
                break;
           
        }
    }
   
    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        _favorabilityMainPanel.Show(0);
        _favorabilityMainPanel.ShowView();
    }

    public override void SetData(params object[] paramsObjects)
    {
        if (paramsObjects.Length > 0)
        {
            if (paramsObjects[0] is string)
            {
                GlobalData.FavorabilityMainModel.CurrentRoleVo =GlobalData.FavorabilityMainModel.GetUserFavorabilityVo((int)paramsObjects[1]);    
                switch ((string)paramsObjects[0])
                {
                    case "SendGift":
                        CreatFavorabilityMain();                  
                        _favorabilityMainPanel.JumpTo((string)paramsObjects[0]);                        
                        break;
                    case "Voice":
                        CreatFavorabilityMain();
                        _favorabilityMainPanel.IsCardJumpTo = true;
                        _favorabilityMainPanel.JumpTo((string)paramsObjects[0]);
                       break;                    
                }                                
            }
            else
            {
                RoleVo = (UserFavorabilityVo) paramsObjects[0];
                GlobalData.FavorabilityMainModel.CurrentRoleVo = RoleVo; //当前角色Id
                _favorabilityEnternPanel=new FavorabilityEnternPanel();
                _favorabilityEnternPanel.Init(this);
                _favorabilityEnternPanel.Show(0);
            }
            
        }
        else
        {
            if (_favorabilityEnternPanel==null)
            {
                _favorabilityEnternPanel=new FavorabilityEnternPanel();
                _favorabilityEnternPanel.Init(this);
                _favorabilityEnternPanel.Show(0);
            }
        }
    }

    private void CreatFavorabilityMain()
    {
        if (_favorabilityMainPanel==null)
        {
            _favorabilityMainPanel = new FavorabilityMainPanel();
            _favorabilityMainPanel.Init(this); 
        }
        _favorabilityMainPanel.Show(0); 
    }
  

}