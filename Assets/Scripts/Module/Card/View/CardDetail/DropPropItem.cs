using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropPropItem : MonoBehaviour
{

    private RawImage _propTexture;
    private Text _propName;
    //private Text _propNum;
    private Transform _btnGroup;
    private JumpData[] _jumpDataList;

    private void Awake()
    {
        _btnGroup = transform.Find("BtnGroup");
        _propName = transform.Find("PropNameText").GetComponent<Text>();
        _propTexture = transform.Find("PropImage").GetComponent<RawImage>();

        for (int i = 0; i < _btnGroup.childCount; i++)
        {
            _btnGroup.GetChild(i).gameObject.Hide();
        }
    }


        public void SetData(UpgradeStarRequireVo vo, CardModel cardModel, LevelModel model, int cardId,Action action=null)
        {
            if (vo==null||cardModel==null||model==null)
            {
                return;
            }
            
            
            _propName.text = vo.PropName+ "X"+vo.CurrentNum;
            _propTexture.texture = ResourceManager.Load<Texture>(vo.GetPropTexturePath);
            _jumpDataList = cardModel.GetJumpDataById(vo.PropId);
            JumpTo(vo.PropId, vo.NeedNum, cardModel, model, cardId,action);
            
        }

        public void JumpTo(int id, int neenum, CardModel cardModel, LevelModel model, int cardId = -1,Action action=null)
        {
            for (int i = 0; i < 2; i++)
            {
                Transform child = _btnGroup.GetChild(i);
                if (_jumpDataList==null||_jumpDataList.Length <= i)
                {
                    child.gameObject.Hide();
                    continue;
                }
                
                child.gameObject.Show();

                if (_jumpDataList[i].Module == ModuleConfig.MODULE_RECOLLECTION)
                {
                    _jumpDataList[i].PostData = cardId;
                }

                _jumpDataList[i].RequireId = id;
                _jumpDataList[i].RequireNum = neenum;
                
                //要新增一个逻辑：未解锁的关卡不能显示关卡名字，并且不能跳转。
                var levelVo = model.FindLevel(_jumpDataList[i]);
                if (levelVo != null && !levelVo.IsOpen)
                {
                    //颜色变得灰掉。
                    child.Find("Text").GetComponent<Text>().text = I18NManager.Get("Card_UnLock",_jumpDataList[i].DisplayText);
                        //$"<color=#808080>{_jumpDataList[i].DisplayText + "（未解锁）"}</color>";
                    child.GetComponent<Button>().onClick.AddListener(() => { FlowText.ShowMessage(I18NManager.Get("Card_UnLockMainline")); });
                }
                else
                {
                    child.Find("Text").GetComponent<Text>().text = _jumpDataList[i].DisplayText;
                    child.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        int index = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();
//                        Close();
                        if (action!=null)
                        {
                            action.Invoke();
                        }

//                        Debug.LogError(_jumpDataList[index].DisplayText + " " + _jumpDataList[index].Data + " " +
//                                       _jumpDataList[index].Module);
                        int openLevel = GuideManager.GetOpenUserLevel(ModulePB.CardMemories, FunctionIDPB.CardMemoriesEntry);
                        if (_jumpDataList[index].Module == ModuleConfig.MODULE_RECOLLECTION && 
                            GlobalData.PlayerModel.PlayerVo.Level < openLevel)
                        {
                            FlowText.ShowMessage(I18NManager.Get("Card_UpToLevel",openLevel));
                        }
                        else
                        {
                            ModuleManager.Instance.EnterModule(_jumpDataList[index]);
                        }
                    });
                }

            }
        }
    
}
