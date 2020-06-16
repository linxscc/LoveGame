//using Assets.Scripts.Framework.GalaSports.Core.Events;
//using Assets.Scripts.Framework.GalaSports.Service;
//using Com.Proto;
//using Common;
//using DataModel;
//using game.main;
//using game.tools;
//using Google.Protobuf.Collections;
//using UnityEngine;
//using UnityEngine.UI;
//
//namespace Assets.Scripts.Module.Recollection.View
//{
//    public class RecollentionRewardWindow : Window
//    {
//        private RepeatedField<AwardPB> _awardPbs;
//        private Button closeBtn;
//        private int _index;
//        private Text _descTxt;
//      
//        private void Awake()
//        {
//            closeBtn = transform.Find("DescText/CloseBtn").GetComponent<Button>();
//        }
//
//        public void SetData(RepeatedField<AwardPB> awardPbs, int chanllengeNum )
//        {
//         //   MaskAlpha = 0.5f;
//            
//            _awardPbs = awardPbs;
//
//            _index = 0;
//            
//            Transform list = transform.Find("List");
//          
//
//            _descTxt = transform.Find("DescText").GetComponent<Text>();
//            //  _descTxt.text= $"<size=60>翻取奖励</size>" + $"\r\n剩余 <color=#fe4c4c>{awardPbs.Count.ToString()} </color>次机会";
//            _descTxt.text = I18NManager.Get("Recollection_RecollentionRewardWindowDescText", awardPbs.Count.ToString());
//
//
//
//              RectTransform rect = transform.GetComponent<RectTransform>();
//
//            int extraNum = awardPbs.Count - chanllengeNum;
//            
//            int itemNum = chanllengeNum * 3;   //chanllengeNum这个数据值只能是1或者3
//            if (chanllengeNum == 1)
//            {
//                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 700);
//            }
//            else if(chanllengeNum == 2)
//            {
//                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 900);
//            }
//
//            if (extraNum > 0)
//                FlowText.ShowMessage(I18NManager.Get("Recollection_Hint4", extraNum));//("本次可额外选择" + extraNum +"次!");
//           
//
//
//          
//
//
//            string[] spriteName = new string[3] { "logo0", "logo1", "logo2" };
//
//            for (int i = 0; i < itemNum; i++)
//            {
//                GameObject go = InstantiatePrefab("Recollection/Prefabs/Cards");
//                go.transform.SetParent(list, false);
//                go.transform.Find("Cards/logo").GetComponent<Image>().sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Recollection_" + spriteName[Random.Range(0,3)]);
//                PointerClickListener.Get(go).onClick = ShowReward;
//            }
//        }
//
//        protected override void OnClickOutside(GameObject go)
//        {
//            if (_index < _awardPbs.Count)
//            {
//                FlowText.ShowMessage(I18NManager.Get("Recollection_Hint3"));//("还有奖励未开启");
//            }
//            else
//            {
//
//                
//              // base.OnClickOutside(go);
//            }
//        }
//
//        private void ShowReward(GameObject go)
//        {
//            if (_index >= _awardPbs.Count)
//                return;
//          
//            go.GetComponent<Animator>().enabled = true;
//           
//            RewardItem item = go.AddComponent<RewardItem>();
//            item.ShowReward(_awardPbs[_index++]);
//           
//            PointerClickListener.Get(go).onClick = null;
//
//
//            //  _descTxt.text = $"<size=60>翻取奖励</size>" + $"\r\n剩余 <color=#fe4c4c>{(_awardPbs.Count - _index).ToString()} </color>次机会";
//            _descTxt.text = I18NManager.Get("Recollection_RecollentionRewardWindowDescText", (_awardPbs.Count - _index).ToString());
//            if (_awardPbs.Count - _index == 0)
//            {
//               
//                closeBtn.gameObject.SetActive(true);
//                closeBtn.onClick.AddListener(() =>
//                {
//                   
//                   EventDispatcher.TriggerEvent(EventConst.RecollentionRewardGetWindowClose);
//                   EventDispatcher.TriggerEvent(EventConst.AwardIsEnough);
//                   base.Close();
//                });
//            }
//          
//        }
//
//
//        
//    }
//}