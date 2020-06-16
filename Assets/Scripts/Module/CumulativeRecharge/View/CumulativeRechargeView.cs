using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;

public class CumulativeRechargeView : View
{
    private RawImage adsRawImage;
    private LoopVerticalScrollRect _loopVerticalScroll;
    private List<AccumulativeRechargeVO> _accumulativeRechargeVos;
    private Button _jumpTo;
    
    

    void Awake()
    {
        _loopVerticalScroll = transform.Find("Bg/ListContent/Award/AwardList").GetComponent<LoopVerticalScrollRect>();
        _jumpTo = transform.Find("Bg/JumpTo").GetButton();
        _jumpTo.onClick.AddListener(() =>
        {
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_SHOP,false,true,5);
            //SendMessage(new Message(MessageConst.CMD_GOTORECHARGE,Message.MessageReciverType.CONTROLLER));
        });
        _loopVerticalScroll.prefabName = "CumulativeRecharge/Prefabs/CumulativeItem";
        _loopVerticalScroll.poolSize = 6;
        _loopVerticalScroll.UpdateCallback = ListUpdateCallback;
        
        
    }

    public void SetData(List<AccumulativeRechargeVO> accumulativeRechargeVos)
    {
        _accumulativeRechargeVos = accumulativeRechargeVos;
        int refillidx = 0;
        for (int i = 0; i < accumulativeRechargeVos.Count; i++)
        {
            if (accumulativeRechargeVos[i].Weight==2)
            {
                refillidx = i;
                break;
            }
        }
        
        SetAccumulativeList(accumulativeRechargeVos,refillidx);
    }

    private void SetAccumulativeList(List<AccumulativeRechargeVO> accumulativeRechargeVos, int refillidx)
    {

        _loopVerticalScroll.totalCount = accumulativeRechargeVos.Count;
        _loopVerticalScroll.RefreshCells();
        //这个RefillCells有毒！！！
        _loopVerticalScroll.RefillCells(refillidx);
    }
    
    
    private void ListUpdateCallback(GameObject gameobj, int index)
    {
        gameobj.GetComponent<CumulativeItem>().SetData(_accumulativeRechargeVos[index]);
        
    }


}
