using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Download;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class SmsTipsWindow : Window
    {
        protected override void OnInit()
        {

        }
        private void Awake()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        MySmsOrCallVo _vo;
        public void SetData(MySmsOrCallVo vo)
        {
            AudioManager.Instance.PlayEffect("sms_tips", 1);
            Debug.Log("SetData");
            transform.Find("LeftHeadIcon/Image").GetComponent<RawImage>().texture =
                ResourceManager.Load<Texture>(PhoneData.GetHeadPath(vo.NpcId), null, true);
            _vo = vo;
            //string showStr = "";
            //if (vo.FirstTalkInfo.TalkContent.Length >)
            transform.Find("ReadBtn/Text").GetComponent<Text>().text = vo.FirstTalkInfo.TalkContent;
            transform.Find("NpcName").GetComponent<Text>().text = vo.Sender;
            transform.Find("ReadBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("ReadBtn ....");
                if (CacheManager.isGuideSmsBySceneId(vo.SceneId)) 
                {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_PHONE,
                        true, false, vo
                        );
                    Close();
                    return;
                }

                CacheManager.ClickItem(vo.NpcId, () =>
                {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_PHONE,
                        true, false, vo
                        );
                    Close();
                },()=> {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_PHONE,
                     true, false, vo
                         );
                    Close();
                });
                //ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_PHONE,
                //    true, false, vo
                //    );
                //Close();
            });
        }

        public void EnterPhone()
        {

            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_PHONE,
                true, false, _vo);
            Close();


            //CacheManager.ClickItem(_vo.NpcId, () =>
            //{
            //    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_PHONE,
            //        true, false, _vo
            //        );
            //    Close();
            //},()=> {
            //    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_PHONE,
            //     true, false, _vo
            //       );
            //    Close();

            //});

        }
    }
}
