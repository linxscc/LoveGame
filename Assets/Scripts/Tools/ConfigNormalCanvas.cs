using UnityEngine;
using UnityEngine.UI;

namespace game.tools
{
    [ExecuteInEditMode]
    public class ConfigNormalCanvas : MonoBehaviour {

        void Start()
        {
            CanvasScaler canvasScaler = transform.GetComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1080, 1920);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            
            float rate = (float)Screen.height / Screen.width;
            if (rate > 1.8)
            {
                canvasScaler.matchWidthOrHeight = 0.0f;
            }
            else
            {
                canvasScaler.matchWidthOrHeight = 1.0f;
            }
            
        }
    }
}
