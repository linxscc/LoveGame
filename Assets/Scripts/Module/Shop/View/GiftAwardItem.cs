using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using DataModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace game.main
{
    public class GiftAwardItem : MonoBehaviour
    {

        private Text _awardName;
        private Text _awardNum;
//        private Text _desc;
        private Frame _frame;

//        private int _resourceid;
//        private ResourcePB _resourcePb;

        private void Awake()
        {
            _awardName = transform.Find("RewardItem/PropNameTxt").GetText();
            _awardNum = transform.Find("RewardItem/ObtainText").GetText();
//            _desc = transform.Find("Desc").GetText();
            _frame = transform.Find("RewardItem/SmallFrame").GetComponent<Frame>();
        }

        public void SetData(AwardPB awardPb)
        {
//            Debug.LogError(awardPb);
            RewardVo vo=new RewardVo(awardPb);

            _awardName.text = vo.Name;
            _awardNum.text = vo.Num.ToString();
            _frame.SetData(vo);
//            _desc.text = ClientData.GetItemDescById(awardPb.ResourceId)?.ItemDesc;
            

        }

//        public void OnPointerClick(PointerEventData eventData)
//        {
//            var desc = ClientData.GetItemDescById(_resourceid,_resourcePb);
//            if (desc!=null)
//            {
//                FlowText.ShowMessage(desc.ItemDesc);  
//            }
//
//        }
    } 

}


