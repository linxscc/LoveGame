using Assets.Scripts.Framework.GalaSports.Service;
using game.main;
using Spine.Unity;
using UnityEngine;

namespace game.tools
{
    public class SpineUtil
    {
        /// <summary>
        /// spine共享材质球
        /// </summary>
        public static Material SharedMaterial;
        
        /// <summary>
        /// 动态创建一个 spine 动画数据
        /// </summary>
        /// <param name="skeletonPath">spine动画文件路径</param>
        /// <param name="skeletonGraphic"></param>
        /// <param name="material">自定义材质球</param>
        /// <returns></returns>
        public static SkeletonDataAsset BuildSkeletonDataAsset(string spineId, SkeletonGraphic skeletonGraphic, Material material = null)
        {
            string skeletonPath = "Spine/Animation/" + spineId + "/" + spineId;

            SkeletonDataAsset sda;
            sda = ScriptableObject.CreateInstance<SkeletonDataAsset>();
            sda.scale = 0.01f;
            sda.defaultMix = 0.2f;
            
            AtlasAsset[] arrAtlasData = new AtlasAsset[1];

            AtlasAsset atlasData = ScriptableObject.CreateInstance<AtlasAsset>();
            atlasData.atlasFile = ResourceManager.Load<TextAsset>(skeletonPath + "_atlas");
            atlasData.materials = new Material[1];

            if (material == null)
            {
                material = new Material(Shader.Find("Spine/SkeletonGraphic (Premultiply Alpha)"));
                SharedMaterial = material;
            }
            
            atlasData.materials[0] = material;

            skeletonGraphic.material = material;

            Texture tex = ResourceManager.Load<Texture>(skeletonPath);
            atlasData.materials[0].SetTexture("_MainTex", tex);
            arrAtlasData[0] = atlasData;
            tex.name = spineId;

            sda.atlasAssets = arrAtlasData;
            sda.skeletonJSON = ResourceManager.Load<TextAsset>(skeletonPath + "_skel");
            return sda;
        }

        public static void UnloadLater(string spineId)
        {
            string skeletonPath = "Spine/Animation/" + spineId + "/" + spineId;
            
            AssetManager.Instance.AddToLaterUnload(skeletonPath);
            AssetManager.Instance.AddToLaterUnload(skeletonPath + "_atlas");
            AssetManager.Instance.AddToLaterUnload(skeletonPath + "_skel");
        }
    }
}