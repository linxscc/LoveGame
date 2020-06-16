using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using DataModel;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class AwardItem : MonoBehaviour
    {

        //private RawImage _propimage;
        private Text _propname;
        private Text _propNum;
        private Frame _frame;

        private void Awake()
        {
            //_propimage = transform.Find("PropImage").GetRawImage();
            _propname = transform.Find("PropNameText").GetText();
            _propNum = transform.Find("PropNum").GetText();
            _frame = transform.GetComponent<Frame>();
        }

        public void SetData(AwardPB awardPb)
        {
//            Debug.LogError(awardPb);
            if (awardPb==null)
            {
                Debug.LogError("why awardPB is null?");
            }
            RewardVo vo=new RewardVo(awardPb);
            _frame.SetData(vo);
            _propname.text = vo.Name;
            _propNum.text = vo.Num.ToString();

        }
    
    }  

}


