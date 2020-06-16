using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using XLua;

public static class XLuaConfig
{
    [Hotfix]
    public static List<Type> by_property
    {
        get
        {
            List<Type> list = new List<Type>();

            DirectoryInfo di = new DirectoryInfo(Application.dataPath);
            string url = di.Parent.FullName + "/Library/ScriptAssemblies/Assembly-CSharp.dll";
            Type[] arr = Assembly.LoadFile(url).GetTypes();
            foreach (var type in arr)
            {
                if (type.Namespace == "game.main" || type.Namespace == null)
                {
                    list.Add(type);
                }
                else if (type.Namespace != null &&
                         (type.Namespace.IndexOf("DataModel", StringComparison.Ordinal) != -1 ||
                          type.Namespace.IndexOf("Assets.Scripts", StringComparison.Ordinal) != -1 ||
                          type.Namespace.ToLower().StartsWith("game") ||
                          type.Namespace.ToLower().StartsWith("module") ||
                          type.Namespace.ToLower().StartsWith("common") ||
                          type.Namespace.ToLower().StartsWith("componets"))
                )
                {
                    list.Add(type);
                }
            }

            return list;
        }
    }

    //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
    [LuaCallCSharp] public static List<Type> LuaCallCSharp = new List<Type>()
    {
        typeof(System.Object),
        typeof(MethodInfo),
        typeof(UnityEngine.Object),
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(Quaternion),
        typeof(Color),
        typeof(Ray),
        typeof(Bounds),
        typeof(Ray2D),
        typeof(Time),
        typeof(GameObject),
        typeof(Component),
        typeof(Behaviour),
        typeof(Transform),
        typeof(Resources),
        typeof(TextAsset),
        typeof(Keyframe),
        typeof(AnimationCurve),
        typeof(AnimationClip),
        typeof(MonoBehaviour),
        typeof(ParticleSystem),
        typeof(SkinnedMeshRenderer),
        typeof(Renderer),
        typeof(WWW),
        typeof(Light),
        typeof(Mathf),
        typeof(UnityWebRequest),
        typeof(System.Collections.Generic.List<int>),
        typeof(System.Array),
        typeof(Action<string>),
        typeof(Action),
        typeof(UnityEngine.Debug)
    };

    /// <summary>
    /// dotween的扩展方法在lua中调用
    /// </summary>
    [LuaCallCSharp] [ReflectionUse] public static List<Type> DotweenLuaCallCsList = new List<Type>()
    {
        typeof(DG.Tweening.AutoPlay),
        typeof(DG.Tweening.AxisConstraint),
        typeof(DG.Tweening.Ease),
        typeof(DG.Tweening.LogBehaviour),
        typeof(DG.Tweening.LoopType),
        typeof(DG.Tweening.PathMode),
        typeof(DG.Tweening.PathType),
        typeof(DG.Tweening.RotateMode),
        typeof(DG.Tweening.ScrambleMode),
        typeof(DG.Tweening.TweenType),
        typeof(DG.Tweening.UpdateType),

        typeof(DG.Tweening.DOTween),
        typeof(DG.Tweening.DOVirtual),
        typeof(DG.Tweening.EaseFactory),
        typeof(DG.Tweening.Tweener),
        typeof(DG.Tweening.Tween),
        typeof(DG.Tweening.Sequence),
        typeof(DG.Tweening.TweenParams),
        typeof(DG.Tweening.Core.ABSSequentiable),

        typeof(DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>),

        typeof(DG.Tweening.TweenCallback),
        typeof(DG.Tweening.TweenExtensions),
        typeof(DG.Tweening.TweenSettingsExtensions),
        typeof(DG.Tweening.ShortcutExtensions),
        typeof(DG.Tweening.ShortcutExtensions43),
        typeof(DG.Tweening.ShortcutExtensions46),
        typeof(DG.Tweening.ShortcutExtensions50),
        
        //添加需要使用的泛型
        typeof(Action),
        typeof(Action<bool>),
        typeof(Action<int>),
        typeof(Action<string>),
        typeof(UnityAction),
        typeof(UnityAction<bool>),
        typeof(UnityAction<int>),
        typeof(UnityAction<string>),
        typeof(XLuaUtil),
        typeof(AssetBundle)
        
        

        //dotween pro 的功能
        //typeof(DG.Tweening.DOTweenPath),
        //typeof(DG.Tweening.DOTweenVisualManager),
    };

    //黑名单
    [BlackList] public static List<List<string>> BlackList = new List<List<string>>()
    {
#if UNITY_WEBGL
        new List<string>(){"UnityEngine.WWW", "threadPriority"},
    #endif
#if !UNITY_WEBPLAYER
        new List<string>() {"UnityEngine.Application", "ExternalEval"},
#endif
        new List<string>() {"System.Xml.XmlNodeList", "ItemOf"},
        new List<string>() {"UnityEngine.WWW", "movie"},
        new List<string>() {"UnityEngine.Texture2D", "alphaIsTransparency"},
        new List<string>() {"UnityEngine.Security", "GetChainOfTrustValue"},
        new List<string>() {"UnityEngine.CanvasRenderer", "onRequestRebuild"},
        new List<string>() {"UnityEngine.Light", "areaSize"},
        new List<string>() {"UnityEngine.Light", "lightmapBakeType"},
        new List<string>() {"UnityEngine.WWW", "MovieTexture"},
        new List<string>() {"UnityEngine.WWW", "GetMovieTexture"},
        new List<string>() {"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},

        new List<string>() {"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
        new List<string>() {"UnityEngine.Component", "networkView"}, //4.6.2 not support
        new List<string>()
        {
            "System.IO.FileInfo",
            "GetAccessControl",
            "System.Security.AccessControl.AccessControlSections"
        },
        new List<string>() {"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
        new List<string>()
        {
            "System.IO.DirectoryInfo",
            "GetAccessControl",
            "System.Security.AccessControl.AccessControlSections"
        },
        new List<string>()
        {
            "System.IO.DirectoryInfo",
            "SetAccessControl",
            "System.Security.AccessControl.DirectorySecurity"
        },
        new List<string>()
        {
            "System.IO.DirectoryInfo",
            "CreateSubdirectory",
            "System.String",
            "System.Security.AccessControl.DirectorySecurity"
        },
        new List<string>() {"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
        new List<string>() {"UnityEngine.MonoBehaviour", "runInEditMode"},
        new List<string>() {"UnityEngine.Light", "shadowRadius"},
        new List<string>() {"UnityEngine.Light", "shadowAngle"},
        new List<string>() {"UnityEngine.Light", "SetLightDirty"},
    };
}