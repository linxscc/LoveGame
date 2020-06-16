//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using Assets.Scripts.Framework.GalaSports.Core;
//using Assets.Scripts.Framework.GalaSports.Core.Events;
//using Assets.Scripts.Framework.GalaSports.Service;
//using Assets.Scripts.Module.NetWork;
//using Com.Proto;
//using Common;
//using Componets;
//using DataModel;
//using game.main;
//using sdk;
//using UnityEngine;

//public class SaveAndShareController : Controller
//{

//    public SaveAndShareView  _saveAndShareView;

//    public override void Start()
//    {   
//        _saveAndShareView.SetInfo(GlobalData.FavorabilityMainModel.CurrentRoleVo);

//    }

//    public override void OnMessage(Message message)
//    {
//       string name = message.Name;
//       object[] body = message.Params;
//        switch (name)
//        {
//            case MessageConst.CMD_SAVEANDSHARE_VIEW_SAVE:
//                OnSave();
//                break;
//            case MessageConst.CMD_SAVEANDSHARE_VIEW_SHARE:
//                OnShare();
//                break;
//        }
//    }


//    public override void Destroy()
//    {
//        base.Destroy();
//    }

//    /// <summary>
//    /// 保存
//    /// </summary>
//    private void OnSave()
//    { 
//        string fName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";  //用时间作为图片的名称，保证唯一性
//        _saveAndShareView.MyStartCoroutine(UploadPNG(fName));
//    }
    
//    /// <summary>
//    /// 保存图片的协程
//    /// </summary>
//    /// <param name="fileName">文件名</param>
//    /// <returns></returns>
//    private IEnumerator UploadPNG(string fileName)
//    {
//        //截图前先隐藏UI
//        _saveAndShareView.ShowOrHideBtn(false);
//        yield return new WaitForEndOfFrame();

//        //截图
//        int width = Screen.width;
//        int height = Screen.height;
//        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

//        //截图后显示UI
//        _saveAndShareView.ShowOrHideBtn(true);

//        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);         //读取Pixels 像素点
//        tex.Apply();                                                 //申请
//        byte[] bytes = tex.EncodeToPNG();                            //转换字节流
//        GameObject.Destroy(tex);                                     //销毁
//        string path = PathForFile(fileName);    //移动平台的判断(返回路径)
        
//    }

//    /// <summary>
//    /// 判断移动平台设置路径
//    /// </summary>
//    /// <param name="fileName">文件名</param>
//    private string PathForFile(string filename)
//    {
//        if (Application.platform == RuntimePlatform.IPhonePlayer)    //IOS
//        {
//            string path = Application.persistentDataPath.Substring(0, Application.persistentDataPath.Length - 5);
//            path = path.Substring(0, path.LastIndexOf('/'));
//            return Path.Combine(Path.Combine(path, "Documents"), filename);
//        }
//        else if (Application.platform == RuntimePlatform.Android)    //Android
//        {
//            string path = Application.persistentDataPath;
//            path = path.Substring(0, path.LastIndexOf('/'));
//            return Path.Combine(path, filename);
//        }
//        else
//        {
//            string path = Application.dataPath;
//            path = path.Substring(0, path.LastIndexOf('/'));
//            return Path.Combine(path, filename);
//        }
//    }

 




//    /// <summary>
//    /// 分享
//    /// </summary>
//    private void OnShare()
//    {
//        string fName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";  //用时间作为图片的名称，保证唯一性    
//        _saveAndShareView.MyStartCoroutine(SharePNG(fName));
//    }

//    private IEnumerator SharePNG(string fileName)
//    {
//        //截图前先隐藏UI
//        _saveAndShareView.ShowOrHideBtn(false);

//        yield return new WaitForEndOfFrame();

//        //截图
//        int width = Screen.width;
//        int height = Screen.height;
//        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);

//        //截图后
//        _saveAndShareView.ShowOrHideBtn(true);

//        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
//        tex.Apply();
//        byte[] bytes = tex.EncodeToPNG();
//        GameObject.Destroy(tex);
//        string path = PathForFile(fileName);    //移动平台的判断(返回路径)
//    }

   
//}
