using Assets.Scripts.Framework.GalaSports.Service;
using live2d;
using live2d.framework;
using UnityEngine;

namespace game.main.Live2d
{
    public class Live2dPlatformManager : IPlatformManager
    {
        public byte[] loadBytes(string path)
        {
           path = path.Replace(".", "_");
           return ResourceManager.Load<TextAsset>(path).bytes;
        }

        public void UnloadBytes(string path)
        {
            path = path.Replace(".", "_");
            AssetManager.Instance.UnloadBundle(path);
        }

        public string loadString(string path)
        {
            path = path.Replace(".", "_");

            TextAsset textAsset = ResourceManager.Load<TextAsset>(path);
            if (textAsset == null)
                return null;

            return textAsset.text;
        }

        public void UnloadString(string path)
        {
            path = path.Replace(".", "_");
            AssetManager.Instance.UnloadBundle(path);
        }

        public ALive2DModel loadLive2DModel(string path)
        {
            path = path.Replace(".", "_");
            
            var data = loadBytes(path);
            var live2DModel = Live2DModelUnity.loadModel(data);

            return live2DModel;
        }

        public void loadTexture(ALive2DModel model, int no, string path)
        {
            path = path.Replace(".png", "").Replace(".", "_");
            
            Texture texture =  ResourceManager.Load<Texture>(path);
            ((Live2DModelUnity)model).setTexture(no, (Texture2D) texture);
        }

        public void UnloadTexture(string path)
        {
            path = path.Replace(".png", "").Replace(".", "_");
            AssetManager.Instance.UnloadBundle(path);
        }

        public void log(string txt)
        {
            Debug.Log("Live2dPlatformManager:"+txt);
        }
    }
}