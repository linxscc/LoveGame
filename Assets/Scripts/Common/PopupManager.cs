#region 模块信息

// **********************************************************************
// Copyright (C) 2018 The 望尘体育科技
//
// 文件名(File Name):             PopupManager.cs
// 作者(Author):                  张晓宇
// 创建时间(CreateTime):          2018/8/15 11:33:55
// 修改者列表(modifier):
// 模块描述(Module description):
// **********************************************************************

#endregion

using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using DataModel;
using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using UnityEngine;
using Common;

namespace game.main
{
    public class PopupManager : MonoBehaviour
    {
        private static PopupManager _instance;

        private static Stack<Window> _popupWindows = new Stack<Window>();

        private Transform _windowLayer;

        private void Awake()
        {
            _instance = this;
            _windowLayer = transform;
            _waitToShows = new List<MySmsOrCallVo>();
        }

        public static bool IsOnBack()
        {
            return _popupWindows.Count > 0;
        }

      
        
        /// <summary>
        /// 安卓返回键处理
        /// </summary>
        public static void OnBackKeyPress()
        {
            //_currentWindow.Close();
            if (_popupWindows.Count == 0)
                return;
            Window closeWindow = _popupWindows.Peek();
            closeWindow.Close();
        }

        /// <summary>
        /// 关闭最后一个窗口
        /// </summary>
        public static void CloseLastWindow()
        {
            if(_popupWindows.Count > 0)
            {
                Window closeWindow = _popupWindows.Peek();
                if(closeWindow != null)
                    closeWindow.Close();
            }
        }

        public static T ShowWindow<T>(string windowName, IModule module = null) where T : Window
        {
            GameObject window = InitPopupPrefab(windowName);
            
            var win = window.AddScriptComponent<T>();
            win.Container = module;
            win.AssetName = windowName;

            Popup(window);
            
            return win;
        }

        public static void Popup(GameObject go)
        {
            Window win = go.GetComponent<Window>();           
            go.transform.SetParent(_instance._windowLayer, false);
            
            win.OnOpen();
            _popupWindows.Push(win);
                      
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;
        }

        public static void CloseWindow(Window win)
        {
            win.Container = null;
            Destroy(win.transform.gameObject);
            AssetManager.Instance.UnloadBundle("module/"+win.AssetName);
            if(_popupWindows.Count > 0)
                _popupWindows.Pop();
        }

        public static GameObject InitPopupPrefab(string prefabPath)
        {
            return Instantiate(ResourceManager.Load<GameObject>("module/" + prefabPath));
        }

        /// <summary>
        /// 通用提示窗口
        /// </summary>
        /// <param name="content">提示内容</param>
        /// <param name="title">标题</param>
        /// <param name="okBtnText">确定按钮文本</param>
        /// <returns></returns>
        public static AlertWindow ShowAlertWindow(string content, string title = null, string okBtnText = null)
        {
            if (title == null)
            {
                title = I18NManager.Get("Common_Hint");
            }

            if (okBtnText == null)
            {
                okBtnText = I18NManager.Get("Common_OK2");
            }

            AlertWindow win = ShowWindow<AlertWindow>(Constants.AlertWindowPath);
            win.Content = content;
            win.Title = title;
            win.OkText = okBtnText;

            return win;
        }
        
        /// <summary>
        /// 下载提示窗口
        /// </summary>
        /// <param name="content">提示内容</param>
        /// <param name="title">标题</param>
        /// <param name="okBtnText">确定按钮文本</param>
        /// <returns></returns>
        public static DownloadTipsWindow ShowDownloadTipsWindow(long size, string title = null, string okBtnText = null,bool isReceive=false)
        {
            if (title == null)
            {
                title = I18NManager.Get("Common_Hint");
            }

            if (okBtnText == null)
            {
                okBtnText = I18NManager.Get("Common_Download1");

            }

            DownloadTipsWindow win = ShowWindow<DownloadTipsWindow>(Constants.DownloadTipsWindowPath);
            if (isReceive)
            {
                win.Content = I18NManager.Get("Download_AwardTips2",Math.Round(size * 1f / 1048576f, 2));

            }
            else
            {
                win.Content = I18NManager.Get("Download_AwardTips",Math.Round(size * 1f / 1048576f, 2),5);
            }
            //win.Content = content;
            win.SetCardShow(!isReceive);
            win.Title = title;
            win.OkText = okBtnText;

            return win;
        }

        /// <summary>
        /// 通用确认窗口
        /// </summary>
        /// <param name="content">提示内容</param>
        /// <param name="title">标题</param>
        /// <param name="okBtnText">确定按钮文本</param>
        /// <param name="cancelBtnText">取消按钮文本</param>
        /// <param name="module">所属模块</param>
        /// <returns></returns>
        public static ConfirmWindow ShowConfirmWindow(string content, string title = null, string okBtnText = null,
            string cancelBtnText = null, IModule module = null)
        {
            if (title == null)
            {
                title = I18NManager.Get("Common_Hint");
            }

            if (okBtnText == null)
            {
                okBtnText = I18NManager.Get("Common_OK2");
            }

            if (cancelBtnText == null)
            {
                cancelBtnText = I18NManager.Get("Common_Cancel2");
            }
            
            ConfirmWindow win = ShowWindow<ConfirmWindow>(Constants.ConfirmWindowPath);
            win.Content = content;
            win.Container = module;
            win.Title = title;
            win.OkText = okBtnText;
            win.CancelText = cancelBtnText;

            return win;
        }


        public static RuleExplainWindow ShowRuleExplainWindow(string title,string ruleDesc,Vector2 sizeDelta)
        {
            RuleExplainWindow win = ShowWindow<RuleExplainWindow>(Constants.RuleExplainWindow);
            win.SetData(title,ruleDesc,sizeDelta);
            return win;
        }
   
        public static BuyWindow ShowBuyWindow(int buyItemId,int costItemId)
        {
            BuyWindow win = ShowWindow<BuyWindow>(Constants.BuyWindowPath);
            win.MaskColor = new Color(0, 0, 0, 0.5f);
            win.InitWindowInfo(buyItemId, costItemId);
            return win;
        }

        public static BuyWindow ShowBuyWindow(int buyItemId,int costItemId,int costItemNum)
        {
            BuyWindow win = ShowWindow<BuyWindow>(Constants.BuyWindowPath);
            win.MaskColor = new Color(0, 0, 0, 0.5f);
            win.InitWindowInfo(buyItemId, costItemId,costItemNum);
            return win;
        }
    
    
        public static BuyItemWindow ShowBuyItemWindow(int buyItemId, int costItemId)    
        {
            BuyItemWindow win = ShowWindow<BuyItemWindow>(Constants.StarcardWindowPath);
            win.MaskColor = new Color(0, 0, 0, 0.5f);
            win.InitWindowInfo(buyItemId, costItemId);
            return win;
        }

        
        /// <summary>
        /// 显示网络异常重新连接窗口
        /// </summary>
        /// <returns></returns>
        public static RetryWindow ShowRetryWindow(string content=null,string retry=null,string cancel=null,string title=null)
        {
            if (content ==null)
            {
                content = "       "+I18NManager.Get("Common_RetryWiondwContent");
            }

            if (retry==null)
            {
                retry = I18NManager.Get("Common_RetryBtn");
               
            }

            if (cancel==null)
            {
                cancel = I18NManager.Get("Common_Cancel2");
            }
            
            
            RetryWindow win = ShowWindow<RetryWindow>(Constants.RetryWindowPath);
            win.MaskColor = new Color(0, 0, 0, 0.5f);
            win.Content = content;
            win.Title = title;
            win.OkText = retry;                      
            win.CancelText = cancel;
            return win;
        }

        /// <summary>
        /// 展示通用规则窗口
        /// </summary>
        /// <param name="content"></param>
        /// <param name="tittle"></param>
        /// <returns></returns>
        public static CommonRuleWindow ShowCommonRuleWindow(string content, string tittle = null)
        {
            CommonRuleWindow win = ShowWindow<CommonRuleWindow>("Prefabs/CommonRuleWindow");
            win.SetData(content, tittle);
            return win;
        }


        static Window PhoneWin = null;
        static SmsTipsWindow smsTipsWindow = null;


        static List<MySmsOrCallVo> _waitToShows;//等待显示一般只有四个NPC的
        /// <summary>
        /// 通用手机tips
        /// </summary>
        /// <param name="content">提示内容</param>
        /// <param name="title">标题</param>
        /// <param name="okBtnText">确定按钮文本</param>
        /// <param name="cancelBtnText">取消按钮文本</param>
        /// <returns></returns>
        public static void ShowPhoneTipsWindow(Action finished=null)
        {
            var curStep = GuideManager.CurStage();

            //如果能保证新手引导要使用的资源提前达到包里的话，该判断条件可以去掉
            if (curStep < GuideStage.ExtendDownloadStage)
            {
                finished?.Invoke();
                return;
            }


            _waitToShows.Clear();
            bool isHoldShow = false;      

            byte[] buffer = NetWorkManager.GetByteData(new Com.Proto.UserPhoneTipDataReq()
            {
            });
            NetWorkManager.Instance.Send<Com.Proto.UserPhoneTipDataRes>(Assets.Scripts.Module.NetWork.CMD.PHONEC_TIPS_DATA, buffer, (res) =>
            {
                List<int> EliminatSceneIds = new List<int>();

                for (int i = 0; i < res.FriendCircles.Count; i++)
                {
                    FriendCircleVo vo = PhoneData.TransFriendCircleData(res.FriendCircles[i]);
                    if (vo == null)
                        continue;
                    GlobalData.PhoneData.AddFriendCircleData(vo);
                    Util.SetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_FC, true);
                    EliminatSceneIds.Add(vo.SceneId);
                }

                for (int i = 0; i < res.MicroBlogs.Count; i++)
                {          
                    WeiboVo vo = PhoneData.TransWeiboData(res.MicroBlogs[i]);
                    if (vo == null)
                        continue;
                    GlobalData.PhoneData.AddWeiboData(vo);
                    Util.SetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_WEIBO, true);
                    EliminatSceneIds.Add(vo.SceneId);
                }

                bool isShowTips = false;
                //MySmsOrCallVo voTips = null;

                for (int i = 0; i < res.MsgOrCalls.Count; i++)
                {
                    //一次只会触发一条短信  先默认最后一条
                    //先转数据格式
                    MySmsOrCallVo vo = PhoneData.TransSmsOrCallData(res.MsgOrCalls[i]);

                    if (vo == null || vo.FirstTalkInfo.NpcId == 0) 
                    {
                        continue;
                    }
                    isShowTips = GlobalData.PhoneData.IsSmsOrCallTipsShow(vo);
                    if (!isShowTips)
                    {
                        if (vo.SceneId < 10000)
                        {
                            Util.SetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_SMS, true);
                        }
                        else
                        {
                            Util.SetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_CALL, true);
                        }
                        GlobalData.PhoneData.AddSmsOrCallData(vo);
                        EliminatSceneIds.Add(vo.SceneId);
                    }
                    else
                    {
                        if (_waitToShows.Find((m) => { return m.NpcId == vo.NpcId; }) != null) 
                        {
                            continue;
                        }
                        _waitToShows.Add(vo);
         
      
                    }
                }

               
                if (_waitToShows.Count > 0)//第一个默认添加
                {
                    _waitToShows.Sort();
                    GlobalData.PhoneData.AddSmsOrCallData(_waitToShows[0]);
                    EliminatSceneIds.Add(_waitToShows[0].SceneId);
                }

                for (int i = 0; i < res.MsgOrCalls.Count; i++)
                {
                    //一次只会触发一条短信  先默认最后一条
                    //先转数据格式
                    MySmsOrCallVo vo = PhoneData.TransSmsOrCallData(res.MsgOrCalls[i]);
                    if (vo == null || vo.FirstTalkInfo.NpcId != 0) 
                    {
                        continue;
                    }
                     GlobalData.PhoneData.AddSmsOrCallData(vo);           
                    EliminatSceneIds.Add(vo.SceneId);
                    if (vo.SceneId < 10000)
                    {
                        Util.SetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_SMS, true);
                    }
                    else
                    {

                        Util.SetIsRedPoint(Constants.REDPOINT_PHONE_BGVIEW_CALL, true);
                    }
                }
                //test
               EliminatEsceneId(EliminatSceneIds);
                finished?.Invoke();
                // if (!isShowTips) return;
                HandleShowWindow(_waitToShows);
                return;

            }, (v) =>
            {
                finished?.Invoke();
            });
        }

        public static void StopHandleShowWindow()
        {
            if(_curTipCoroutine!=null)
            {
                ClientTimer.Instance.CancelDelayCall(_curTipCoroutine);
                _curTipCoroutine = null;
            }
            if(_curTipsShow!=null)
            {
                _curTipsShow.Close();
                _curTipsShow = null;
            }
        }

        static Window _curTipsShow = null;
        static Coroutine _curTipCoroutine = null;
        private static void HandleShowWindow(List<MySmsOrCallVo> waitToShows)
        {
            var lsit = waitToShows;
            if (lsit.Count==0)
            {
                return;
            }

            MySmsOrCallVo vo = lsit[0];
            if (!lsit.Remove(vo)) return;

            if (vo.SceneId < 10000)
            {
                var win  = PopupManager.ShowWindow<SmsTipsWindow>("Phone/Prefabs/Tips/SmsTipsWindow");
                win.SetData(vo);
                _curTipsShow = win;
            }
            else
            {
                var win = PopupManager.ShowWindow<CallTipsWindow>("Phone/Prefabs/Tips/CallTipsWindow");
                win.SetData(vo);
                _curTipsShow = win;
            }
            _curTipCoroutine = ClientTimer.Instance.DelayCall(() => {
                _curTipsShow.Close();
                _curTipsShow = null;
                _curTipCoroutine = null;
                if (lsit.Count > 0)
                {
                    MySmsOrCallVo v1 = lsit[0];
                    HandleShowWindow(lsit);
                    GlobalData.PhoneData.AddSmsOrCallData(v1);
                    List<int> l = new List<int>();
                    l.Add(v1.SceneId);
                    EliminatEsceneId(l);
                }
            }, 8.0f);
        }

        private static void EliminatEsceneId(List<int> sceneId)
        {
            
            if (sceneId.Count == 0)
                return;
            Com.Proto.EliminatEsceneReq eliminatEsceneReq = new Com.Proto.EliminatEsceneReq();
            eliminatEsceneReq.SceneId.AddRange(sceneId);
            byte[] buffer1 = NetWorkManager.GetByteData(eliminatEsceneReq);
            NetWorkManager.Instance.Send<Com.Proto.EliminatEsceneRes>(Assets.Scripts.Module.NetWork.CMD.PHONEC_ELIMINAT_ESCENE, buffer1, (eliminatEsceneRes) =>
            {
                if(eliminatEsceneRes.Ret!=-1)
                {
                    Debug.Log("PHONEC_ELIMINAT_ESCENE error!");
                }
                Debug.Log("PHONEC_ELIMINAT_ESCENE success!");
            });
        }

        public static void ClosePhoneTipsWindow()
        {
           // Debug.LogError("ClosePhoneTipsWindow  ");
            if (PhoneWin == null)
                return;
            PhoneWin.Close();
            PhoneWin = null;
        }
        public static void EnterSmsTipsWindow()
        {
//            if (GuideManager.IsMainGuidePass(GuideConst.MainStep_PhoneSms_End) == false)
//            {
//                GuideManager.SetRemoteGuideStep(GuideTypePB.MainGuide, GuideConst.MainStep_PhoneSms_End);
//            }
          //   GuideManager.SetRemoteGuideStep(GuideTypePB.MainGuide, GuideConst.MainStep_PhoneSms_End);            
            if (smsTipsWindow == null)
                return;
            smsTipsWindow.EnterPhone();
            smsTipsWindow = null;
        }

        public static void CloseAllWindow()
        {
            StopHandleShowWindow();
            foreach (var win in _popupWindows)
            {
                if(win != null)
                    win.Close();
            }
            _popupWindows = new Stack<Window>();
        }
    }
}