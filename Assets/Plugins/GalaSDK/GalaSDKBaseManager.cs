using UnityEngine;
using GalaSDKBase;
using System;
using System.Runtime.InteropServices;

public class GalaSDKBaseManager : MonoBehaviour
{
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern String onUnityGALASDKBaseCallNativeWithReturn(String func, String args);
#endif

    /// <summary>
    ///  调用底层通道
    /// </summary>
    /// <param name="function">function和data</param>
    private static string UnityGALASDKCallNative(string function, string data)
    {
#if UNITY_IOS && !UNITY_EDITOR
            return onUnityGALASDKBaseCallNativeWithReturn(function, data);
#elif UNITY_ANDROID && !UNITY_EDITOR
                try
                {
                    using (var ajc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    {
                        var ajo = ajc.GetStatic<AndroidJavaObject>("currentActivity");
            return ajo.Call<string>("onUnityGALASDKBaseCallNativeWithReturn", function, data);
                    }
                }
                catch
                {
                    return "false";
                }
#endif
        return "false";
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GalaSDKBaseFunction.UnityGALASDKCallNative = UnityGALASDKCallNative;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GalaSDKBaseFunction.OS = 1;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            GalaSDKBaseFunction.OS = 2;
        }
        gameObject.AddComponent<GalaSDKBaseCallBack>();
        GalaSDKBaseFunction.InitSDK();
    }
}
