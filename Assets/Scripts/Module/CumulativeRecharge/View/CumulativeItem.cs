using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class CumulativeItem : MonoBehaviour
{

    private Text _titleName;
    private ProgressBar _progressBar;
    private Text _progressText;
    private Transform _awardList;
    private Button _receiveaward;
    private GameObject _finishtips;
    private AccumulativeRechargeVO _accumulativeRechargeVo;
    
    
    
    
    private void Awake()
    {
        _titleName = transform.Find("TitleName").GetText();
        _progressBar = transform.Find("ProgressText/ExpSlider/ProgressBar").GetComponent<ProgressBar>();
        _progressText = transform.Find("ProgressText").GetText();
        _awardList = transform.Find("AwardList/Content");
        _receiveaward = transform.Find("ReceiveBtn").GetButton();
        _receiveaward.onClick.AddListener(ReceiveAwardClick);
        _finishtips = transform.Find("FinishTask").gameObject;
        for (int i = 0; i <_awardList.childCount; i++)
        {
            _awardList.GetChild(i).gameObject.SetActive(false);
        }

    }

    private void ReceiveAwardClick()
    {
        Debug.LogError("_accumulativeRechargeVo"+_accumulativeRechargeVo.GearId+" "+_accumulativeRechargeVo.GearAmound);
        EventDispatcher.TriggerEvent(EventConst.ReceiveCumulativeAward,_accumulativeRechargeVo);
    }
    
    

    public void SetData(AccumulativeRechargeVO vo)
    {
        //对象池问题！
        for (int i = 0; i <_awardList.childCount; i++)
        {
            _awardList.GetChild(i).gameObject.SetActive(false);
        }
        _accumulativeRechargeVo = vo;
       // _titleName.text = $"累积充值<size=40><color='#FF97CB'>{vo.GearAmound}</color></size>元";
       _titleName.text = I18NManager.Get("Activity_CumulativePayItemTitle",vo.GearAmound);
       _progressBar.gameObject.SetActive(vo.Weight==1);
        _progressText.gameObject.SetActive(vo.Weight==1);
        _progressBar.Progress = (int) ((float) vo.CurAmount / vo.GearAmound * 100f);
        _progressText.text = $"<color='#FF97CB'>{vo.CurAmount}</color>"+"/"+vo.GearAmound;
        _receiveaward.gameObject.SetActive(vo.Weight==2);
        _titleName.gameObject.SetActive(vo.Weight!=0);
        _finishtips.SetActive(vo.Weight==0);
        for (int i = 0; i < vo.Awards.Count; i++)
        {
            _awardList.GetChild(i).gameObject.SetActive(true);

            var item=_awardList.GetChild(i);
            item.GetComponent<AwardItem>().SetData(vo.Awards[i]);
//            RewardVo rewardVo=new RewardVo(vo.Awards[i]);
//            item.GetComponent<Frame>().SetData(rewardVo);

            bool special = GlobalData.FavorabilityMainModel.GetDressUpUnlockRulePb(vo.Awards[i].ResourceId) != null ||
                           vo.Awards[i].Resource == ResourcePB.Card;
            SetIconSize(special,item.Find("Mask/Icon").GetRectTransform());
            item.Find("PropNum").gameObject.SetActive(!special);

            if (vo.Awards[i].Resource == ResourcePB.Card)
            {
                var cardvo = GlobalData.CardModel.CardBaseDataDict[vo.Awards[i].ResourceId];
                item.Find("PropNameText").GetText().text=  cardvo.Credit.ToString().ToUpper()+"·" +cardvo.CardName;
            }

            //AwardPB rewardVo = vo.Awards[i];
//            Debug.LogError(rewardVo);
//            PointerClickListener.Get(_awardList.GetChild(i).gameObject).onClick = go =>
//            {
//                var desc = ClientData.GetItemDescById(rewardVo.ResourceId, rewardVo.Resource);
//                FlowText.ShowMessage(desc.ItemDesc);
//            };
        }
        

    }

    public void SetIconSize(bool specialItem,RectTransform icon)
    {
        if (!specialItem)
        {
            icon.SetWidth(120);
            icon.SetHeight(120);
        }
        else
        {
            icon.SetWidth(164);
            icon.SetHeight(164);
        }
        
        
    }
    
    
}
