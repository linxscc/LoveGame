using UnityEngine;
using System.Collections;
using Assets.Scripts.Framework.GalaSports.Service;

public class MonoObject : MonoBehaviour
{
    static readonly string PATH = "AppDelegate/Prefabs/[MonoGameObject]";

    private static MonoObject _instance;
   
    public static bool AutoLogin = false;
    
    public static MonoObject Instance
    {
        get { return _instance; }
    }

//    public static void Initialize()
//    {
//        if (_instance == null)
//            Instantiate(ResourceManager.Load<GameObject>(PATH));
//    }

    void Awake()
    {
        _instance = this;
        //DontDestroyOnLoad(gameObject); //切换场景是不销毁该obj
    }

    public void Coroutine(IEnumerator arg)
    {
        StartCoroutine(arg);
    }
}
