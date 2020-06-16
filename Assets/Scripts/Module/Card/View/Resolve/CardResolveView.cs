using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using DataModel;
using game.tools;
using Google.Protobuf.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class CardResolveView : View
    {
        private LoopVerticalScrollRect _list;
        private List<ResolveCardVo> _userCardList;
        private Text _selectedText;
        private Transform _props;
        private List<ResolveCardVo> _originalData;
        private Toggle _selectAllToggle;
        private PlayerPB _tabSelectedFilter = PlayerPB.None;
        private Text _tips;
        private Dictionary<int, int> _selectNumDic;
        private Button _resolveBtn;
        
        int[] ids = {
            PropConst.CardEvolutionPropChi, PropConst.CardEvolutionPropQin, PropConst.CardEvolutionPropYan,
            PropConst.CardEvolutionPropTang
        };

        //private int[] resolveAddNum = {0, 0, 0, 0};
        

        private void Awake()
        {
            _list = transform.Find("List").GetComponent<LoopVerticalScrollRect>();
            _list.prefabName = "Card/Prefabs/Resolve/CardResolveItem";
            _list.poolSize = 6;

            _list.UpdateCallback = ListUpdateCallback;
            
            _selectAllToggle = transform.Find("SelectAllToggle").GetComponent<Toggle>();
            _selectAllToggle.onValueChanged.AddListener(OnSelectAll);

            _selectedText = transform.Find("Bottom/SelectedText").GetComponent<Text>();
            _resolveBtn=transform.Find("Bottom/ResolveBtn").GetComponent<Button>();
            _resolveBtn.onClick.RemoveAllListeners();
            _resolveBtn.onClick.AddListener(OnResolveBtn);
            _tips = transform.Find("Tips").GetComponent<Text>();
            _props = transform.Find("Bottom/Props");
            _selectNumDic=new Dictionary<int, int>();
            _selectNumDic.Add(PropConst.CardEvolutionPropChi,0);
            _selectNumDic.Add(PropConst.CardEvolutionPropQin,0);
            _selectNumDic.Add(PropConst.CardEvolutionPropYan,0);
            _selectNumDic.Add(PropConst.CardEvolutionPropTang,0);

        }

        private void OnResolveBtn()
        {
            RepeatedField<UserCardSimplePB> list = new RepeatedField<UserCardSimplePB>();
            bool isContanSRorSSR = false;
            for (int i = 0; i < _userCardList.Count; i++)
            {
                ResolveCardVo uc = _userCardList[i];
                if (uc.SelectedNum > 0)
                {
                    UserCardSimplePB pb = new UserCardSimplePB();
                    pb.CardId = uc.CardId;
                    pb.Num = uc.SelectedNum;
                    list.Add(pb);
                    if (uc.Credit == CreditPB.Sr || uc.Credit == CreditPB.Ssr)
                    {
                        isContanSRorSSR = true;
                    }
                }
            }

            if (list.Count == 0)
            {
                //Debug.LogError("没有选择卡牌！！！");
                return;
            }

            if (isContanSRorSSR)
            {
                PopupManager.ShowConfirmWindow(I18NManager.Get("Card_EnsureResolveContainSRorSSrTips")).WindowActionCallback = evt =>
                {
                    if (evt == WindowEvent.Ok)
                    {
                        SendMessage(new Message(MessageConst.CMD_CARD_RESOLVE, list));
                    }
                };
            }
            else
            {
                SendMessage(new Message(MessageConst.CMD_CARD_RESOLVE, list));
                Debug.LogError("why twice?");
            }
            
        }
        public void ChangeSelect()
        {
            int sum = 0;
            //_selectNumDic.Clear();
            ClearDicNum();
            for (int i = 0; i < _userCardList.Count; i++)
            {
                sum += _userCardList[i].SelectedNum;
                
                //算法应该是这样的，每次我都会记录点击后的
                RecordNum(_userCardList[i]);

            }
            UpdateProps();
            _selectedText.text = I18NManager.Get("Card_EnsureResolve2",sum);//$"确定回溯({sum})";
            //刷新所有Cells数据
            _list.RefreshCells();
            //UpdateProps();
        }


        public void ClearDicNum()
        {
            _selectNumDic[PropConst.CardEvolutionPropChi] = 0;
            _selectNumDic[PropConst.CardEvolutionPropQin] = 0;
            _selectNumDic[PropConst.CardEvolutionPropYan] = 0;
            _selectNumDic[PropConst.CardEvolutionPropTang] = 0;
        }
        
        public void RecordNum(ResolveCardVo vo)
        {
            for (int i = 0; i < 4; i++)
            {
                if (vo.ResolveItem.ContainsKey(ids[i])&&_selectNumDic.ContainsKey(ids[i]))
                {
                    //Debug.LogError(ids[i]);
                    _selectNumDic[ids[i]] += vo.SelectedNum *vo.ResolveItem[ids[i]];
                }
            }

                  
        }
        
        public void UpdateProps()//ResolveCardVo vo
        {
            //刷新数据！回溯一张卡牌获取多少道具的规则！
//            if (vo.SelectedNum >0)
//            {
//                for (int i = 0; i < 4; i++)
//                {    
//                    if (vo.ResolveItem.ContainsKey(ids[i]))
//                    {
//                        Debug.LogError(ids[i]);
//                        Debug.LogError(vo.ResolveItem[ids[i]]);
//                        _props.GetChild(i).Find("AddNum").gameObject.SetActive(true);
//                        _props.GetChild(i).Find("AddNum/Num").GetComponent<Text>().text = (vo.SelectedNum * vo.ResolveItem[ids[i]]).ToString();
//                    }
//                }
//            }
            for (int i = 0; i < 4; i++)
            {
                _props.GetChild(i).Find("AddNum").gameObject.SetActive(_selectNumDic[ids[i]]>0);
                _props.GetChild(i).Find("AddNum/Num").GetComponent<Text>().text ="+"+ _selectNumDic[ids[i]];
            }
            

        }
        
        
        public void InitProps()
        {
            for (int i = 0; i < 4; i++)
            {
                int num = 0;
                UserPropVo vo = GlobalData.PropModel.GetUserProp(ids[i]);
                if (vo != null)
                {
                    num = vo.Num;
                }

                Transform item = _props.GetChild(i);
                //resolveAddNum[i] = 0;
                //item.Find("PropImage").GetComponent<RawImage>().texture = ResourceManager.Load<Texture>(vo==null?GlobalData.PropModel.GetPropPath(ids[i]):vo.GetTexturePath());
                item.Find("Text").GetComponent<Text>().text = num + "";
                item.Find("AddNum").gameObject.SetActive(false);
                item.Find("AddNum/Num").GetComponent<Text>().text = I18NManager.Get("Card_NoData");
                PointerClickListener.Get(item.gameObject).onClick = ShowPropTips;
                PointerClickListener.Get(item.gameObject).parameter = ids[i];
            }
            
            _selectedText.text = I18NManager.Get("Card_EnsureResolve") ;
        }



        private void ShowPropTips(GameObject go)
        {
            int id = (int)PointerClickListener.Get(go).parameter;
            FlowText.ShowMessage(I18NManager.Get("Card_PrivateItem",GlobalData.PropModel.SpliceCardName(id)));
        }

        private void OnSelectAll(bool isOn)
        {

            if(_userCardList == null)
                return;

            for (int i = 0; i < _userCardList.Count; i++)
            {
                _userCardList[i].SelectedNum = isOn ? _userCardList[i].Num : 0;
            }

            ChangeSelect();
        }

        public void SetData(List<ResolveCardVo> data, PlayerPB filter = PlayerPB.None)
        {
            InitProps();
            _tabSelectedFilter = filter;
            _list.RefillCells();
            _selectAllToggle.isOn = false;
            
            
            _originalData = data;

            if (_tabSelectedFilter != PlayerPB.None)
            {
                _userCardList = data.FindAll(match => { return match.Player == filter; });
            }
            else
            {
                _userCardList = data;
            }
            
            _list.totalCount = _userCardList.Count;
            _list.RefreshCells();

            if (_userCardList.Count==0)
            {
                //FlowText.ShowMessage("待收集");
                _tips.gameObject.Show();
            }
            else
            {
                _tips.gameObject.Hide();
            }
            
        }

        private void ListUpdateCallback(GameObject go, int index)
        {
            go.GetComponent<CardResolveItem>().SetData(_userCardList[index]);
        }


        public void ChangeTabBar(PlayerPB pb)
        {
            if(_originalData == null)
                return;
            
            SetData(_originalData, pb);
        }
    }
}