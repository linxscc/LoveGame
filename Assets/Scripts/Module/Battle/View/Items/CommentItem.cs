using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class CommentItem : MonoBehaviour
    {
        private Text _nameTxt;
        private Text _commentText;
        private Text _likeText;
        private RawImage _headIcon;
        private RectTransform _bg;

        private void Awake()
        {
            _nameTxt = transform.Find("Bg/NameText").GetComponent<Text>();
            _commentText = transform.Find("Bg/CommentText").GetComponent<Text>();
            _likeText = transform.Find("Bg/LikeText").GetComponent<Text>();

            _headIcon = transform.Find("Bg/HeadMask/Image").GetComponent<RawImage>();
            _bg = transform.Find("Bg").GetComponent<RectTransform>();
        }

        public void SetData(LevelCommentRulePB comment)
        {
            _nameTxt.text = comment.Name;
            _commentText.text = comment.Content;
            _likeText.text = comment.LikeNum + "";
            //Debug.LogError(comment);
            _headIcon.texture = ResourceManager.Load<Texture>("FansTexture/Head/" + (1000 + comment.Id % 9), ModuleConfig.MODULE_BATTLE);//Random.Range(1000,1010));
            
            float height = _commentText.preferredHeight + 150;
            _bg.sizeDelta = new Vector2(_bg.sizeDelta.x, height);

            transform.GetComponent<LayoutElement>().preferredHeight = height;

            RectTransform rt = transform.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
        }

        public void SetData(VisitingLevelCommentRulePB comment)
        {
            _nameTxt.text = comment.Name;
            _commentText.text = comment.Content;
            _likeText.text = comment.LikeNum + "";
            //Debug.LogError(comment);
            _headIcon.texture = ResourceManager.Load<Texture>("FansTexture/Head/" + (1000 + comment.Id % 9), ModuleConfig.MODULE_BATTLE);//Random.Range(1000,1010));

            float height = _commentText.preferredHeight + 150;
            _bg.sizeDelta = new Vector2(_bg.sizeDelta.x, height);

            transform.GetComponent<LayoutElement>().preferredHeight = height;

            RectTransform rt = transform.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
        }


        public void SetCapsuleBattleData()
        {
            
        }
    }
}