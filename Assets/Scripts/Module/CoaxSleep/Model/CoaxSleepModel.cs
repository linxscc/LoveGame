using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using Framework.Utils;
using game.main;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;
using System.Text;
using Assets.Scripts.Module.Download;

public class CoaxSleepModel : Model
{
    private List<CoaxSleepAudioRulePB> _rules;
    private List<UserCoaxSleepInfoPB> _userInfos;


    private List<CoaxSleepAudioJsonData> _serverAudioJsonData;//服务器文件校验数据
    
    private List<CoaxSleepAudioJsonData> _localAudioJsonData;  //本地的放的一些文件校验的数据（IOS和安卓的不一样）
    
    private List<CoaxSleepAudioDescJsonData> _audioDescJson;  //放的一些音频文案描述

    private List<MyCoaxSleepAudioData> _myUserData;

    private readonly string ExternaCoaxSleepAudioPath = Application.persistentDataPath + "/CoaxSleepAudioFolder/";
    
    /// <summary>
    /// 初始化规则
    /// </summary>
    /// <param name="res"></param>
    public void InitRule(CoaxSleepRulesRes res)
    {
       
        DownLoadServerAudioJsonData();
      
        InitJsonData();     
        _rules = res.CoaxSleepAudioRules.ToList();
        
    }


    /// <summary>
    /// 下载服务器校验文件
    /// </summary>
    private void DownLoadServerAudioJsonData()
    {
        string url = AppConfig.Instance.assetsServer + "/" + "CoaxSleepAudioFolder/" + GetPlatform()+"/coaxsleep.json";
        
        Debug.LogError("校验文件路径--->"+url);
        string localPath = ExternaCoaxSleepAudioPath + GetPlatform()+"/coaxsleep.json";
        DownloadManager.Load(url, localPath,(item =>
        {
            string text = new AssetLoader().LoadTextSync(localPath);         
            _serverAudioJsonData= JsonConvert.DeserializeObject<List<CoaxSleepAudioJsonData>>(text);     
        }),null,null);
    }
    

   
    

    /// <summary>
    /// 获取平台
    /// </summary>
    /// <returns></returns>
    private string GetPlatform()
    {
        string platform = "";
        switch (Application.platform)
        {      
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.Android:
                platform = "Android";
                break;
            case RuntimePlatform.IPhonePlayer:
                platform = "IOS";
                break;      
        }

        return platform;
    }
    
    /// <summary>
    /// 初始化Json数据
    /// </summary>
    private void InitJsonData()
    {
        string path1 = AssetLoader.GetLocalConfiguratioData("CoaxSleepCache", "coaxsleep");
        string text1 = new AssetLoader().LoadTextSync(path1);
        _localAudioJsonData = JsonConvert.DeserializeObject<List<CoaxSleepAudioJsonData>>(text1);

        string path2 = AssetLoader.GetLocalConfiguratioData("CoaxSleepAudioDesc", "coaxsleepaudiodesc");
        string text2 = new AssetLoader().LoadTextSync(path2);
        _audioDescJson = JsonConvert.DeserializeObject<List<CoaxSleepAudioDescJsonData>>(text2);
    }
    
    /// <summary>
    /// 初始化用户信息
    /// </summary>
    /// <param name="res"></param>
    public void InitData(CoaxSleepInfosRes res)
    {     
        _userInfos = res.UserCoaxSleepInfos.ToList();
        InitMyData(); 
             
    }


    /// <summary>
    /// 初始化我的用户数据
    /// </summary>
    private void InitMyData()
    {
        _myUserData = new List<MyCoaxSleepAudioData>();

        foreach (var rule in _rules)
        {
            MyCoaxSleepAudioData data = new MyCoaxSleepAudioData(rule,
                GetUserInfoToPlayer(rule.Player), GetCoaxSleepAudioDescToAudioId(rule.AudioId));
            _myUserData.Add(data);
        }
    }

    private UserCoaxSleepInfoPB GetUserInfoToPlayer(PlayerPB playerPb)
    {
        foreach (var userInfo in _userInfos)
        {
            if (userInfo.Player == playerPb)
            {
                return userInfo;
            }  
        }
        return new UserCoaxSleepInfoPB();
    }

    private CoaxSleepAudioDescJsonData GetCoaxSleepAudioDescToAudioId(int audioId)
    {
       
        foreach (var t in _audioDescJson)
        {
            if (t.AudioId == audioId)
            {
                return t;
            }
        }

        Debug.LogError("哄睡音频描述Json配置有错，请检查");
        return null;
    }


    /// <summary>
    /// 获取对应Npc的音频数据
    /// </summary>
    /// <param name="playerPb"></param>
    /// <returns></returns>
    public List<MyCoaxSleepAudioData> GetMyUserDataToPlayer(PlayerPB playerPb)
    {
        List<MyCoaxSleepAudioData> list =new  List<MyCoaxSleepAudioData>();
        foreach (var uData in _myUserData)
        {
            if (uData.PlayerPb==playerPb)
            {
               list.Add(uData); 
            }
        }
        return list;
    }


    /// <summary>
    /// 校验下(如果已经解锁了（条件解锁的），后端数据没有记录，要告诉后端)
    /// </summary>
    /// <param name="playerPb"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsSendReq(PlayerPB playerPb, int id)
    {
        bool isSend = false;

        var userInfos = GetUserInfoToPlayer(playerPb);
        var isKey = userInfos.Audios.ContainsKey(id);

        if (isKey == false)
        {
            isSend = true;
        }


        return isSend;
    }

    public void UpdateUserInfo(UserCoaxSleepInfoPB pb)
    {
        bool isHaveData = false;
        int index = 0;

        for (int i = 0; i < _userInfos.Count; i++)
        {
            index = i;
            if (_userInfos[i].Player == pb.Player)
            {
                isHaveData = true;
                break;
            }
        }

        if (isHaveData)
        {
            _userInfos[index] = pb;
        }
        else
        {
            _userInfos.Add(pb);
        }


        InitMyData();
    }


    /// <summary>
    /// 是否需要下载
    /// </summary>
    /// <param name="audioId">音频Id</param>
    /// <returns></returns>
    public bool IsNeedDownload(int audioId)
    {
        bool isDownload = false;

        var loadData = GetLoadAudioJsonData(audioId);
        if (loadData == null)
            return isDownload = true;

        var serverData = GetServerAudioJsonData(audioId);

        bool isSameMd5 = loadData.MD5 == serverData.MD5;

        if (!isSameMd5)
            return isDownload = true;
     
        var filePath = ExternaCoaxSleepAudioPath + loadData.Path;        
      
        var isExists =  File.Exists(filePath);

        if (!isExists)
            return isDownload = true;

        Debug.LogError("是否需要下载--->"+isDownload);
        return isDownload;
    }


    /// <summary>
    /// 获取本地文件,
    /// 存在--->返回本地文件，
    /// 不存在--->返回null
    /// </summary>
    /// <param name="audioId"></param>
    /// <returns></returns>
    public CoaxSleepAudioJsonData GetLoadAudioJsonData(int audioId)
    {
        CoaxSleepAudioJsonData data = null;
        foreach (var t in _localAudioJsonData)
        {
            if (t.AudioId==audioId)
            {
                data = t;
                break;
            }
        }
        return data;
    }


    /// <summary>
    /// 获取远端文件
    /// </summary>
    /// <param name="audioId"></param>
    /// <returns></returns>
    public CoaxSleepAudioJsonData GetServerAudioJsonData(int audioId)
    {       
        return _serverAudioJsonData.Find(x=>x.AudioId ==audioId);
    }
    
   
    public string GetUrl(int audioId)
    {
        var curData = GetLoadAudioJsonData(audioId) ?? GetServerAudioJsonData(audioId);
        string url = AppConfig.Instance.assetsServer + "/"+"CoaxSleepAudioFolder/"+curData.Path;
        Debug.LogError("audioId"+url);
        return url;
    }

    public string GetLocalPath(int audioId)
    {
        var curData = GetLoadAudioJsonData(audioId) ?? GetServerAudioJsonData(audioId);
        string localPath = ExternaCoaxSleepAudioPath +curData.Path;
        return localPath;
    }

    public void CheckUpdateAudioJsonData(int audioId)
    {
        var localData = GetLoadAudioJsonData(audioId);
        var serverData = GetServerAudioJsonData(audioId);
        string path =  AssetLoader.GetLocalConfiguratioData("CoaxSleepCache", "coaxsleep");
        if (localData==null)
        {          
            _localAudioJsonData.Add(serverData);           
            WriteInJson(_localAudioJsonData,path);
        }
        else
        {
            bool isSameMd5 = localData.MD5 == serverData.MD5;          
            if (!isSameMd5)
            {
                UpdateLoadAudioJsonData(serverData);
                WriteInJson(_localAudioJsonData,path);
            }
        }
    }

    private void UpdateLoadAudioJsonData(CoaxSleepAudioJsonData data)
    {
        foreach (var t in _localAudioJsonData)
        {
            if (t.AudioId==data.AudioId)
            {
                t.MD5 = data.MD5;
                t.Size = data.Size;
                break;
            }   
        }  
    }
   
    
    private  void WriteInJson(List<CoaxSleepAudioJsonData> list,string path)
    {
       if ( File.Exists(path))
          File.Delete(path);
       
        string json = JsonConvert.SerializeObject(list,Formatting.Indented);
        File.WriteAllText(path ,json  ,new UTF8Encoding(false));
       
    }
    

}

