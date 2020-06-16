using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Module
{
    public class LoadingPage : MonoBehaviour
    {
        private RawImage _page1;

        private void Awake()
        {
            _page1 = transform.Find("Page1").GetComponent<RawImage>();
            _page1.color = new Color(1,1,1,0.5f);

            Tweener tween1 = _page1.DOFade(1, 0.6f);

            DOTween.Sequence()
                .Append(tween1)
                .AppendInterval(1.0f)
                .AppendInterval(1.0f)
                .onComplete = () => { SceneManager.LoadSceneAsync("Main").completed += OnLoadScene; };

            if((float)Screen.height / Screen.width > 1.80)
            {
                CanvasScaler scaler = transform.GetComponent<CanvasScaler>();
                scaler.matchWidthOrHeight = 1.0f;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                BuglyAgent.ConfigDebugMode (false);
                BuglyAgent.ConfigAutoQuitApplication (false);
//                BuglyAgent.InitWithAppId ("6f4e68ecf4"); //新马
                BuglyAgent.InitWithAppId ("f50da4d1b1");  //国内
            }
        }

        private void OnLoadScene(AsyncOperation obj)
        {
            
        }
    }
}