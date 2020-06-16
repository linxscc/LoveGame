using Assets.Scripts.Framework.GalaSports.Core;
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
    public class CallTipsWindow : Window
    {
        private void Awake()
        {


        }
        protected override void OnInit()
        {

        }


        public void SetData(MySmsOrCallVo vo)
        {
            AudioManager.Instance.PlayEffect("call_tips", 1);
            //todo 根据vo设置头像
            transform.Find("Bg/NpcName").GetComponent<Text>().text = vo.Sender;

            transform.Find("RefuseBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("RefuseBtn");

                Close();
            });
            transform.Find("AnswerBtn").GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("AnswerBtn");

                CacheManager.ClickItem(vo.NpcId, () =>
                {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_PHONE,
            true, false, vo
            );
                    Close();
                },()=>
                {
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


        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }

}