#region 模块信息
// **********************************************************************
// Copyright (C) 2018 The 深圳望尘体育科技
//
// 文件名(File Name):             DropItem.cs
// 作者(Author):                  张晓宇
// 创建时间(CreateTime):           #CreateTime#
// 修改者列表(modifier):
// 模块描述(Module description):
// **********************************************************************
#endregion
using UnityEngine;
using System.Collections;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using DataModel;
using game.tools;
using Module.Battle.Data;
using UnityEngine.UI;
using Assets.Scripts.Module;
using Module.VisitBattle.Data;

namespace game.main
{
    public class VisitDropItem : MonoBehaviour
    {

        private Frame _bigFrame;
        private Text _propNameTxt;
        private Text _ownTxt;
        private VisitDropVo _dropVo;

        void Awake()
        {
            _bigFrame = transform.Find("CenterLayout/BigFrame").GetComponent<Frame>();
            _propNameTxt = transform.Find("PropNameTxt").GetComponent<Text>();
            _ownTxt = transform.Find("OwnTxt").GetComponent<Text>();

            _propNameTxt.text = "";
            _ownTxt.text = "";

            PointerClickListener.Get(gameObject).onClick = go =>
            {
                string tips;
                if (_dropVo.Resource == ResourcePB.Puzzle)
                {
                    tips = ClientData.GetItemDescById(PropConst.PuzzleIconId,_dropVo.Resource).ItemDesc;
                }
                else
                {
                    tips = ClientData.GetItemDescById(_dropVo.PropId,_dropVo.Resource).ItemDesc;
                }

                if (tips != null)
                    FlowText.ShowMessage(tips);
            };
        }

        public void SetData(VisitDropVo data)
        {
            _dropVo = data;

            RewardVo vo = new RewardVo(new AwardPB()
            {
                ResourceId = data.PropId,
                Resource = data.Resource,
                Num = data.OwnedNum
            });
            
            _bigFrame.SetData(vo);

            _propNameTxt.text = data.Resource == ResourcePB.Puzzle?$"{data.Name}({I18NManager.Get("Card_PuzzleTap")})":data.Name;

            _ownTxt.text = I18NManager.Get("MainLine_Have", data.OwnedNum);
        }

    }
}
