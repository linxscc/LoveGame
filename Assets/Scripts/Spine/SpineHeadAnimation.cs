using System.Collections.Generic;
using game.main;
using Spine.Unity;
using UnityEngine;

namespace game.tools
{
    public class SpineHeadAnimation : MonoBehaviour
    {
        private SkeletonGraphic _skg;
        private SpinePlaySequence _sequence;
        private List<SpinePlayableVo> _playableList;

        public void LoadAnimation(string spineId, List<SpinePlayableVo> playableList = null, bool autoInit = true)
        {
            _skg = GetComponent<SkeletonGraphic>();

//            RectTransform rect = GetComponent<RectTransform>();
//            rect.pivot = new Vector2(0.5f, 0);
//            rect.sizeDelta = new Vector2(388,388);

            SkeletonDataAsset skData =  SpineUtil.BuildSkeletonDataAsset(spineId, _skg);
            _skg.skeletonDataAsset = skData;

            _playableList = playableList;
            
            _skg.Initialize(true);
            
            if (autoInit && playableList != null)
            {
                _sequence = new SpinePlaySequence(_skg, _playableList);
                _sequence.Play();
            }
        }
        
        public void Play()
        {
            _sequence?.Play();
        }

        private void OnDestroy()
        {
            _sequence?.Dispose();
        }
    }
}