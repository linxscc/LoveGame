using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Common;
using DG.Tweening;
using game.tools;
using GalaSDKBase;
using Module.Login.View;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Framework.GalaSports.Core.Events;

namespace game.main
{
    public class LoginView : View
    {
        private Transform _loginPanel;
        private Button loginBtn;
 
        private Transform _chooseServer;

        private GameObject _serverObj;
        //private Dropdown _serverDropdown;

        private Button _btnServer;
        private GameObject _serverPanel;
        private Text _serverName;
     
        private Transform _onClickGotoGame;
        private Image _ani;
               
        private void Awake()
        {
            _onClickGotoGame = transform.Find("OnClickGotoGame");
            _ani = _onClickGotoGame.GetImage("Ani");
   
            _loginPanel = transform.Find("Panel");
            _chooseServer = transform.Find("ChooseServer");
            _serverObj = _chooseServer.transform.Find("TempGroup/ServerList").gameObject;
            //_serverDropdown = _chooseServer.Find("TempGroup/ServerListDropdown").GetComponent<Dropdown>();

            _btnServer = _chooseServer.transform.Find("TempGroup/ServerBtn").GetComponent<Button>();
            _serverPanel = _chooseServer.transform.Find("ChooseServerPanel").gameObject;
            _serverName = _btnServer.transform.Find("ServerName").GetComponent<Text>();
            _btnServer.onClick.AddListener(OnBtnServerClick);

            TipsTween();

            Button customServiceBtn = transform.GetButton("TopRight/CustomServiceBtn");
            customServiceBtn.onClick.AddListener(() => { SdkHelper.CustomServiceAgent.Show(); });
            
            customServiceBtn.gameObject.SetActive(AppConfig.Instance.SwitchControl.CustomerServices);

           
            PointerClickListener.Get(_onClickGotoGame.gameObject).onClick = go =>
            {
                SendMessage(new Message(MessageConst.CMD_LOGIN_DO_LOGIN, Message.MessageReciverType.CONTROLLER));
            };



            //公告
            Button AnnouncementBtn = transform.Find("TopRight/AnnouncementBtn").GetComponent<Button>();
            AnnouncementBtn.onClick.AddListener(() => {
                Debug.Log("AnnouncementBtn Onclick");
                SendMessage(new Message(MessageConst.CMD_LOGIN_ANNOUNCEMENT));
            });
            //切换账号
            Button SwitchAccountBtn = transform.Find("TopRight/SwitchAccountBtn").GetComponent<Button>();
            SwitchAccountBtn.gameObject.SetActive(AppConfig.Instance.UseGalaLogin);
            SwitchAccountBtn.onClick.AddListener(() => {
                Debug.Log("SwitchAccountBtn Onclick");
                SendMessage(new Message(MessageConst.CMD_LOGIN_SWITCH_LOGIN, Message.MessageReciverType.CONTROLLER));

            });
            //修复包
            Button RepairBtn = transform.Find("TopRight/RepairBtn").GetComponent<Button>();
            RepairBtn.onClick.AddListener(() =>
            {
                PopupManager.ShowConfirmWindow(I18NManager.Get("Login_RepairTip")).WindowActionCallback = OnRepairConfirm;
            });

          

            string version = "V" + AppConfig.Instance.versionName;
            if(AppConfig.Instance.hotVersion > 0)
                version += "_" + AppConfig.Instance.hotVersion;
            transform.GetText("VersionId").text = version;

            HandelTencent();
            
            HandleTest();

            OnRefreshServerData();
            EventDispatcher.AddEventListener(EventConst.OnConnetToServer, OnRefreshServerData);
            EventDispatcher.AddEventListener(EventConst.OnChooseServer, OnChooseServer);
        }

        private void OnChooseServer()
        {
            _serverPanel.gameObject.SetActive(false);
            _serverName.text = AppConfig.Instance.serverName;
        }

        private void OnRefreshServerData()
        {
            Debug.LogWarning("OnConnectedToServerFinish:" + NetWorkManager.Instance.serverIds.Count);

            if (NetWorkManager.Instance.serverIds.Count <= 1)
            {
                _serverObj.gameObject.SetActive(true);
                _btnServer.gameObject.SetActive(false);
            }
            else
            {
                _serverObj.gameObject.SetActive(false);
                _btnServer.gameObject.SetActive(true);
            }
            _serverName.text = AppConfig.Instance.serverName;
        }

        private void OnBtnServerClick()
        {
            _serverPanel.gameObject.SetActive(true);
        }

        private void OnServerDropDownChange(int index)
        {
            if (index < 0 || index > NetWorkManager.Instance.serverIds.Count - 1) return;
            string serverId = NetWorkManager.Instance.serverIds[index];
            NetWorkManager.Instance.ChooseServer(serverId);
        }

        private void OnRepairConfirm(WindowEvent evt)
        {
            if (evt == WindowEvent.Ok)
            {
                ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_UPDATE,true, false, LoginCallbackType.RepairResource);
            }
        }

        private void HandelTencent()
        {           
            if (Channel.IsTencent)
            {
                Transform tencent = transform.Find("Tencent");
//                tencent.gameObject.Show();
                
                tencent.GetButton("WechatBtn").onClick.AddListener(() =>
                {
                    SendMessage(new Message(MessageConst.CMD_LOGIN_DO_LOGIN, Message.MessageReciverType.CONTROLLER,
                        GalaSDKBaseFunction.GalaSDKType.WeChat));
                });
                
                tencent.GetButton("QQBtn").onClick.AddListener(() =>
                {
                    SendMessage(new Message(MessageConst.CMD_LOGIN_DO_LOGIN, Message.MessageReciverType.CONTROLLER,
                        GalaSDKBaseFunction.GalaSDKType.QQ));
                });                              
            }
        }

        public void LoginHide()
        {
            _chooseServer.gameObject.Hide();
            _loginPanel.gameObject.Hide();
        }
        private void HandleTest()
        {
            Dropdown dropdown = transform.Find("Panel/Dropdown").GetComponent<Dropdown>();
            if (AppConfig.Instance.logicServer.Contains("192.168"))
            {
                
                //内网选择服务器
                dropdown.onValueChanged.AddListener(OnSelectedIntranetServer);
                dropdown.gameObject.Show();
               
                List<Dropdown.OptionData> options = new List<Dropdown.OptionData>()
                {
                    new Dropdown.OptionData(I18NManager.Get("Login_LoginViewHint1")),   //记得删
                    new Dropdown.OptionData(I18NManager.Get("Login_LoginViewHint2")),
                    new Dropdown.OptionData(I18NManager.Get("Login_LoginViewHint3")),
                    new Dropdown.OptionData(I18NManager.Get("Login_LoginViewHint4")),
                    new Dropdown.OptionData(I18NManager.Get("Login_LoginViewHint5"))
                };
                
                dropdown.options = options;

                dropdown.value = GetServerValue("ServerKey");
                NetWorkManager.Instance.SetServer(GetUrl("ServerKey"));
            }
            else
            {
                dropdown.gameObject.Hide();
            }
        }
        private void OnSelectedIntranetServer(int index)
        {
            string server = AppConfig.Instance.logicServer;

           
            if (index == 1)
            {               
                server = "http://192.168.3.187:9001/";
            }
            else if(index == 2)
            {
                server = "http://192.168.3.66:9001/";
            }
            else if(index == 3)
            {
                server = "http://192.168.1.123:9000/";
            }
            else if(index==4)
            {
                server = "http://192.168.3.144:9001/";
            }
            
            SetServerKey(index);                      
            NetWorkManager.Instance.SetServer(server);
        }

        public void GetAnnouncementInfo()
        {
            var _item = GetPrefab("Login/Prefabs/AnnouncementPanel");
            var _go = Instantiate(_item) as GameObject;
            _go.transform.SetParent(gameObject.transform.parent, false);
            _go.transform.localScale = Vector3.one;
            _go.name = "Announcement";
            var _blankObj= _go.transform.Find("Blank").gameObject;
            var btn = _go.transform.Find("BG/Button").gameObject;
            GameObject[] gos = { _blankObj, btn };
            for (int i = 0; i < gos.Length; i++)
            {
                PointerClickListener.Get(gos[i]).onClick = go => { _go.SetActive(false); };
            }
        }

        private void TipsTween()
        {
            Tweener alpha1 = _ani.DOFade(0f, 0.5f);
            alpha1.SetAutoKill(false);
            alpha1.SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
        }


        public void ShowLoginEntrance(bool isShow)
        {
            _onClickGotoGame.gameObject.SetActive(isShow);
            _chooseServer.transform.gameObject.SetActive(isShow);
            _loginPanel.transform.gameObject.SetActive(isShow);
        }

        public void ShowTencent()
        {
           // Debug.LogError("T---- OnShow");
            Transform tencent = transform.Find("Tencent");
            tencent.gameObject.Show();
        }
        
        public void HideTencent()
        {
         //   Debug.LogError("T---- Hied");
            Transform tencent = transform.Find("Tencent");
            tencent.gameObject.Hide();
        }


        private void SetServerKey(int value)
        {
          //  Debug.LogError("Update ServerValue===>"+value);
            PlayerPrefs.SetInt("ServerKey",value);
        }

        private int GetServerValue(string key)
        {
            var isHasKey= PlayerPrefs.HasKey(key);     
            int value = isHasKey ? PlayerPrefs.GetInt(key) : 0;
            return value;
        }
        
        private string GetUrl(string key)
        {
           int value = GetServerValue(key);
           string url =String.Empty;
           string name = String.Empty;
           switch (value)
           {
             case  0:
                 url = AppConfig.Instance.logicServer;
                 name = "内网";
                break;
             case 1:
                 url = "http://192.168.3.187:9001/";
                 name = "张鹏服";
                 break;
             case 2:
                 url = "http://192.168.2.88:9001/";
                 name = "黄勤服";
                 break;
             case 3:
                 url = "http://192.168.1.123:9000/";
                 name = "测试服";
                 break;
             case 4:
                 url = "http://192.168.3.144:9001/";
                 name = "名鑫服";
                 break;
                 
           }
           
           Debug.LogError(name+"===>"+url);
           return url;
        }

        private void OnDestroy()
        {
            EventDispatcher.RemoveEventListener(EventConst.OnConnetToServer, OnRefreshServerData);
            EventDispatcher.RemoveEventListener(EventConst.OnChooseServer, OnChooseServer);
        }
    }


}