using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using game.main;
using game.tools;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleDropItem : MonoBehaviour
{

    private Image _propImage;
    private Text _propNameTxt;
    private Text _ownTxt;
    private Text _dropNum;
    private DropVo _capsuleDropVo;
    private void Awake()
    {
        _propImage = transform.GetImage("CenterLayout/PropImage");
        _propNameTxt = transform.GetText("PropNameTxt");
        _ownTxt = transform.GetText("OwnTxt");
        _dropNum = transform.GetText("DropNum");
        
        PointerClickListener.Get(gameObject).onClick = go =>
        {
            string tips = PropConst.GetTips(_capsuleDropVo.PropId);
            if(tips != null)
                FlowText.ShowMessage(tips);
        };
    }

    public void SetData(DropVo vo)
    {
        _capsuleDropVo = vo;
        _propImage.sprite = ResourceManager.Load<Sprite>(vo.IconPath);
        _propImage.SetNativeSize();
        _propNameTxt.text = vo.Name;
        _ownTxt.text = I18NManager.Get("MainLine_Have", vo.OwnedNum);
        _dropNum.text = vo.MaxNum.ToString();
    }
}
