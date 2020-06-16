using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using game.main;
using game.tools;
using System.Collections;
using System.Collections.Generic;
using DataModel;
using UnityEngine;
using UnityEngine.UI;
public class FirstRechargeItem : MonoBehaviour
{
    private RawImage _icon;
    private Text _num;

    private void Awake()
    {
        _icon = transform.Find("Icon").GetComponent<RawImage>();
        _num = transform.Find("NumBg/Num").GetComponent<Text>(); 
       
    }



    public void SetData(FirstRechargeVO vO)
    {
        _icon.texture = ResourceManager.Load<Texture>(vO.RewardVo.IconPath, ModuleConfig.MODULE_ACTIVITY); 
        _icon.GetComponent<RectTransform>().sizeDelta =new Vector2(200,200);
        _num.text = vO.RewardVo.Num.ToString();


        if ( vO.RewardVo.Resource != ResourcePB.Card)
        {
            PointerClickListener.Get(gameObject).onClick = go => { FlowText.ShowMessage(ClientData.GetItemDescById(vO.RewardVo.Id,vO.RewardVo.Resource).ItemDesc); };   
        }

       
       
        
    }


}
