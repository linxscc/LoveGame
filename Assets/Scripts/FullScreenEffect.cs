using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Module
{
    public class FullScreenEffect : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private Canvas canvas;
        private RectTransform _rect;

        private void Start()
        {
            canvas = GetComponent<Canvas>();
            GameObject go = Instantiate(ResourceManager.Load<GameObject>("module/UIEffect/ClickEffect/ClickEffect"),
                canvas.transform, false);
            
            go.name = "ParticleSystem";
            go.transform.SetAsLastSibling();
            
            _particleSystem = go.transform.GetComponent<ParticleSystem>();
            _particleSystem.playbackSpeed = 1.5f;

            _rect = _particleSystem.transform.GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 localPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera,
                    out localPos);
                
                _particleSystem.Play();
                _rect.anchoredPosition = localPos;

                GameObject obj = EventSystem.current.currentSelectedGameObject;
                if (obj != null)
                {
                    Button btn = obj.GetComponent<Button>();
                    if (btn != null)
                    {
                        ButtonSound bs = obj.GetComponent<ButtonSound>();
                        if (bs != null)
                        {
                            //如果SoundName没有设置就不播放按钮音频
                            if(bs.SoundName != null)
                                btn.PlayButtonEffect(bs.SoundName);
                        }
                        else
                        {
                            //播放默认音频
                            btn.PlayButtonEffect();
                        }
                    }
                }
            }
        }
    }
}