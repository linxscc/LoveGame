using System;
using System.IO;
using Assets.Scripts.Module.MusicRhythm.View;
using game.main;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework.Utils
{
    public enum ScreenShotType
    {
        DrawCard,
        Clothes,
        MusicGame
    }

    public class ScreenShotUtil
    {
        private static int generateWidth = 1080;
        private static int generateHeight = 1920;


        public static void ScreenShot(ScreenShotType screenShotType, Action<string> finsh, params object[] Params)
        {
            var parent = InitPopupPrefab("Prefabs/ScreenShot/ScreenShotCamera");
            var path = "";
            GameObject go;
            switch (screenShotType)
            {
                case ScreenShotType.DrawCard:
                    path = "Prefabs/ScreenShot/DrawViewScreenShot";
                    go = InitPopupPrefab(path);
                    Popup(go, parent);
                    go.GetComponent<DrawViewScreenShot>().SetData((DrawCardResultVo) Params[0]);
                    break;
                case ScreenShotType.Clothes:
                    path = "Prefabs/ScreenShot/FavorabilityMainViewScreenShot";
                    go = InitPopupPrefab(path);
                    Popup(go, parent);
                    go.GetComponent<FavorabilityMainViewScreenShot>().SetData((int) Params[0], (int) Params[1]);
                    break;
                case ScreenShotType.MusicGame:
                    path = "MusicRhythm/Prefabs/MusicRhythmResultShare";
                    go = InitPopupPrefab(path);
                    go.AddScriptComponent<MusicRhythmResultShare>().SetData(Params[0] as MusicRhythmRunningInfo);
                    Popup(go, parent);
                    break;
                default:
                    return;
            }

            ClientTimer.Instance.DelayCall(() =>
            {
                var _camera1 = parent.GetComponent<Camera>();
                var _rect1 = parent.transform.Find("Canvas").GetChild(0).GetComponent<RectTransform>().rect;
                var urlPath1 = CaptureCamera(_camera1, _rect1);
                finsh?.Invoke(urlPath1);

                parent.GetComponent<ScreenShotRender>().DestorySelf();
            }, 0.3f);

            //Camera _camera = parent.GetComponent<Camera>();
            //Rect _rect = parent.transform.Find("Canvas").GetChild(0).GetComponent<RectTransform>().rect;
            //string urlPath = ScreenShotUtil.CaptureCamera(_camera, _rect);

            // return urlPath;
        }


        public static void Popup(GameObject go, GameObject parent)
        {
            go.transform.SetParent(parent.transform.Find("Canvas"));
            var rect = go.GetComponent<RectTransform>();
            //rect.anchoredPosition = Vector2.zero;
            go.transform.localPosition = Vector3.zero;
            rect.localScale = Vector3.one;
        }

        /// <summary>
        ///     对相机截图。
        /// </summary>
        /// <returns>The screenshot2.</returns>
        /// <param name="camera">Camera.要被截屏的相机</param>
        /// <param name="rect">Rect.截屏的区域</param>
        public static string CaptureCamera(Camera camera, Rect rect)
        {
            // 创建一个RenderTexture对象  
            var rt = new RenderTexture((int) rect.width, (int) rect.height, 0);
            // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
            camera.targetTexture = rt;
            camera.Render();
            //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
            //ps: camera2.targetTexture = rt;  
            //ps: camera2.Render();  
            //ps: -------------------------------------------------------------------  

            // 激活这个rt, 并从中中读取像素。  
            RenderTexture.active = rt;
            var screenShot = new Texture2D((int) rect.width, (int) rect.height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(new Rect(0, 0, rect.width, rect.height), 0,
                0); // 注：这个时候，它是从RenderTexture.active中读取像素  
            screenShot.Apply();

            // 重置相关参数，以使用camera继续在屏幕上显示  
            camera.targetTexture = null;
            //ps: camera2.targetTexture = null;  
            RenderTexture.active = null; // JC: added to avoid errors  
            Object.Destroy(rt);
            // 最后将这些纹理数据，成一个png图片文件  
            var bytes = screenShot.EncodeToPNG();
            var filename = Application.temporaryCachePath + "/Screenshot.png";
            File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("截屏了一张照片: {0}", filename));


            return filename;
        }

        //public static void Popup(GameObject go)
        //{
        //    go.transform.SetParent(, false);
        //    RectTransform rect = go.GetComponent<RectTransform>();
        //    rect.anchoredPosition = Vector2.zero;
        //}


        private static GameObject InitPopupPrefab(string prefabPath)
        {
            return PopupManager.InitPopupPrefab(prefabPath);
        }
    }
}