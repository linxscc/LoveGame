using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AssetBundleTool;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Download;
using Com.Proto;
using Framework.Utils;
using game.main;
using Google.Protobuf;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;
using EventType = game.main.EventType;
using FileUtil = Framework.Utils.FileUtil;

/// <summary>
/// 打包资源文件
/// </summary>
public class PackageManager : Editor
{
    public static Dictionary<string, bool> sharedAudioDict;

    public static string Language
    {
        get
        {
            I18NManager.LanguageType languageType =
                (I18NManager.LanguageType) EditorPrefs.GetInt(PublishRes.ResPublish_LanguageKey);

            switch (languageType)
            {
                case I18NManager.LanguageType.ChineseSimplified:
                    return "zh-cn";
                case I18NManager.LanguageType.ChineseTraditionalHk:
                    return "zh-hk";
                case I18NManager.LanguageType.ChineseTraditionalTw:
                    return "zh-tw";
                case I18NManager.LanguageType.English:
                    return "en_us";
                default:
                    return "zh-cn";
            }
        }
    }

    public static string OutputPath => Application.dataPath.Replace("Assets", "PackageFiles/===Output===/") + Language +
                                       "/" + SystemTag + "/";
    public static string I18NDataPath => Application.dataPath.Replace("Assets", "I18NData/") + Language;
    public static string I18NAssetPath => Application.dataPath.Replace("Assets", "I18NAssets/") + Language;
    public static string StoryJsonPath => I18NDataPath + "/Data";
    public static string PhoneJsonPath => I18NDataPath + "/PhoneData";

    public static string SystemTag
    {
        get
        {
            bool isAndroid = EditorUserBuildSettings.selectedBuildTargetGroup == BuildTargetGroup.Android;
            return isAndroid ? "Android" : "iOS";
        }
    }

    public static string GitRoot => new DirectoryInfo(Application.dataPath).Parent.Parent.FullName;

    public static string GetPackageStorePath()
    {
        return Language + "/" + SystemTag;
    }

    public static string GetAssetBundleDir()
    {
        return AssetBundleHelper.GetOriginalBundlesPath() + "/AssetBundles/" + SystemTag;
    }

    public static string GetPackageDir()
    {
        return Application.dataPath.Replace("Assets", "PackageFiles/" + Language);
    }

    static void CopyFile_AllAudio(BlackList blackList)
    {
        Stopwatch sw = Stopwatch.StartNew();

        string dir1 = GetPackageDir() + "/LoveStoryAudio";
        string dir2 = GetPackageDir() + "/StoryAudio";
        string dir3 = GetPackageDir() + "/PhoneAudio";

        string destDir = GetPackageDir() + "/AllAudio";

        if (Directory.Exists(destDir))
            Directory.Delete(destDir, true);

        Directory.CreateDirectory(destDir);

        string[] files = Directory.GetFiles(dir1, "*", SearchOption.AllDirectories);
        CopyFile(files, destDir + "/story/dubbing");

        files = Directory.GetFiles(dir2, "*", SearchOption.AllDirectories);
        CopyFile(files, destDir + "/story/dubbing");

        files = Directory.GetFiles(dir3, "*", SearchOption.AllDirectories);
        CopyFile(files, destDir + "/music/phonedialogs");

        //压缩文件
        string packagePath = OutputPath +"AllAudio";

        if (Directory.Exists(packagePath))
            Directory.Delete(packagePath, true);
        Directory.CreateDirectory(packagePath);

        string zipFileName = packagePath + "/AllAudio.zip";

        files = Directory.GetFiles(destDir, "*", SearchOption.AllDirectories);

        EditorUtility.DisplayProgressBar("压缩", "正在压缩文件(" + Path.GetFileName(packagePath), 1);

        List<string> pathInZipList = new List<string>();
        for (int i = 0; i < files.Length; i++)
        {
            string str = Path.GetDirectoryName(files[i]);
            str = str.Replace("\\", "/");
            str = str.Replace(GetPackageDir().Replace("\\", "/") + "/AllAudio/", "");
            pathInZipList.Add(str + "/");
        }

        ZipUtil.CreateZip(files, zipFileName, pathInZipList.ToArray(), 0);

        EditorUtility.ClearProgressBar();

        //生成Index文件
        string releasePath = "AssetBundles/" + SystemTag + "/";

        ResIndex resIndex = new ResIndex();
        resIndex.language = Language;
        resIndex.belong = "AllAudio";
        resIndex.packageDict = new Dictionary<string, ResPack>();

        files = Directory.GetFiles(destDir, "*", SearchOption.AllDirectories);
        ResPack resPack = new ResPack();
        resPack.id = "AllAudio";
        resPack.items = new List<ResItem>();
        resPack.packageType = FileType.Zip;
        resPack.packageMd5 = MD5Util.Get(packagePath + "/" + resPack.id + ".zip");
        resPack.packageSize = new FileInfo(packagePath + "/" + resPack.id + ".zip").Length;
        resPack.downloadPath = GetPackageStorePath() + "/" + resIndex.belong + "/" + resPack.id + ".zip";
        resPack.releasePath = releasePath;
        resIndex.packageDict.Add(resPack.id, resPack);

        string json = JsonConvert.SerializeObject(resIndex, Formatting.Indented);
        FileUtil.SaveFileText(OutputPath, ResPath.AllAudioIndex + ".json", json);

        Debug.Log("<color='#00ff66'>CopyFile_AllAudio耗时：" + sw.ElapsedMilliseconds/1000.0f  + "</color>");
    }

    private static void CopyFile(string[] files, string destDir)
    {
        if (Directory.Exists(destDir) == false)
            Directory.CreateDirectory(destDir);

        foreach (var file in files)
        {
            string fileName = Path.GetFileName(file);
            string destPath = destDir + "/" + fileName;
            if (File.Exists(destPath))
                continue;

            File.Copy(file, destPath);
        }
    }

    static void CreateZip(string dir, string outputDir, bool useSubDir = true)
    {
        if (Directory.Exists(outputDir) == false)
            Directory.CreateDirectory(outputDir);

        string[] subDir = new[] {dir};
        if (useSubDir)
        {
            subDir = Directory.GetDirectories(dir);
        }

        int count = 0;

        foreach (var zipPath in subDir)
        {
            count++;
            string zipFileName = outputDir + "/" + Path.GetFileName(zipPath) + ".zip";
            string[] files = Directory.GetFiles(zipPath, "*", SearchOption.AllDirectories);

            EditorUtility.DisplayProgressBar("压缩",
                "正在压缩文件(" + Path.GetFileName(outputDir) + ")：" + count + "/" + subDir.Length,
                (float) count / subDir.Length);

            ZipUtil.CreateZip(files, zipFileName, null, 0);
        }

        EditorUtility.ClearProgressBar();
    }

    static void CopyFile_EvoCard()
    {
        string destDir = GetPackageDir() + "/EvoCard";
        if (Directory.Exists(destDir) == false)
            Directory.CreateDirectory(destDir);

        string srcPath = GetAssetBundleDir() + "/card/image/evolutioncard";

        List<string> filesToMove = new List<string>();

        string[] files = Directory.GetFiles(srcPath);
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].Contains(".manifest"))
                continue;
            string destFile = destDir + "/" + Path.GetFileName(files[i]);
            File.Copy(files[i], destFile, true);

            filesToMove.Add(destFile);

            EditorUtility.DisplayProgressBar("复制进化后星缘", "(" + i + "/" + files.Length + ")", (float) i / files.Length);
        }

        string packagePath = OutputPath + ModuleConfig.MODULE_CARD;

        //打个整包
        CreateZip(destDir, packagePath, false);
        GenerateIndexFile(destDir, Language, ModuleConfig.MODULE_CARD, ResPath.EvoCardPackageIndex,
            "card/image/evolutioncard", packagePath, false);

        GenerateIndexFile(destDir, Language, ModuleConfig.MODULE_CARD, ResPath.EvoCardIndex,
            "card/image/evolutioncard", packagePath, false, FileType.Original);

        foreach (var file in filesToMove)
        {
            File.Move(file, packagePath + "/" + Path.GetFileName(file));
        }

        EditorUtility.ClearProgressBar();
    }

    static void CopyFile_LoveStoryAudio(BlackList blackList)
    {
        Dictionary<string, List<string>> fileDict = FindLoveStoryAudio();
        string destDir = GetPackageDir() + "/LoveStoryAudio";

        EditorUtility.DisplayProgressBar("复制音频文件", "", 0);

        foreach (var name in blackList.List)
        {
            if (fileDict.ContainsKey(name))
            {
                fileDict.Remove(name);
            }
        }

        foreach (var file in fileDict)
        {
            DirectoryInfo dir = new DirectoryInfo(destDir + "/" + file.Key);
            if (dir.Exists == false)
                dir.Create();

            int count = 0;
            foreach (var fileName in file.Value)
            {
                count++;
                CopyDubbingFile(dir.FullName, fileName);

                EditorUtility.DisplayProgressBar("复制音频文件",
                    "(" + count + "/" + file.Value.Count + ")" + dir.FullName,
                    (float) count / file.Value.Count);
            }
        }

        EditorUtility.ClearProgressBar();

        string packagePath = OutputPath + ModuleConfig.MODULE_LOVEAPPOINTMENT;
        CreateZip(destDir, packagePath);
        GenerateIndexFile(destDir, Language, ModuleConfig.MODULE_LOVEAPPOINTMENT,
            ResPath.LoveStoryIndex, "story/dubbing", packagePath);
    }

    static void CopyFile_VisitStoryAudio(BlackList blackList)
    {
        Dictionary<string, List<string>> fileDict = FindVisitStoryAudio();
        string destDir = GetPackageDir() + "/VisitStoryAudio";

        EditorUtility.DisplayProgressBar("复制音频文件", "", 0);

        foreach (var file in fileDict)
        {
            DirectoryInfo dir = new DirectoryInfo(destDir + "/" + file.Key);
            if (dir.Exists == false)
                dir.Create();

            int count = 0;
            foreach (var fileName in file.Value)
            {
                if(IsContains(fileName+".bytes", blackList))
                    continue;
                
                count++;
                CopyDubbingFile(dir.FullName, fileName);

                EditorUtility.DisplayProgressBar("复制音频文件",
                    "(" + count + "/" + file.Value.Count + ")" + dir.FullName,
                    (float) count / file.Value.Count);
            }
        }

        EditorUtility.ClearProgressBar();

        string packagePath = OutputPath + ModuleConfig.MODULE_VISIT;
        CreateZip(destDir, packagePath);
        GenerateIndexFile(destDir, Language, ModuleConfig.MODULE_VISIT,
            ResPath.LoveVisitIndex, "story/dubbing", packagePath);
    }

    static void GenerateIndexFile(string destDir, string language, string moduleName, string indexFileName,
        string releasePath, string packagePath, bool useSubDir = true, FileType fileType = FileType.Zip,
        bool ignoreItems = false)
    {
        releasePath = "AssetBundles/" + SystemTag + "/" + releasePath;

        ResIndex resIndex = new ResIndex();
        resIndex.language = language;
        resIndex.belong = moduleName;
        resIndex.packageDict = new Dictionary<string, ResPack>();

        string[] dirs = new string[] {destDir};

        if (useSubDir)
        {
            dirs = Directory.GetDirectories(destDir);
        }

        for (int i = 0; i < dirs.Length; i++)
        {
            ResPack resPack = new ResPack();
            resPack.id = new FileInfo(dirs[i]).Name;

            if (ignoreItems == false)
            {
                resPack.items = new List<ResItem>();
                string[] files = Directory.GetFiles(dirs[i]);
                for (int j = 0; j < files.Length; j++)
                {
                    string filePath = files[j];
                    FileInfo fileInfo = new FileInfo(filePath);
                    ResItem resItem = new ResItem();
                    resItem.Path = fileInfo.Name;
                    resItem.Md5 = MD5Util.Get(filePath);
                    resItem.Size = fileInfo.Length;
                    resPack.items.Add(resItem);
                }
            }

            resPack.packageType = fileType;
            if (fileType == FileType.Zip)
            {
                resPack.packageMd5 = MD5Util.Get(packagePath + "/" + resPack.id + ".zip");
                resPack.packageSize = new FileInfo(packagePath + "/" + resPack.id + ".zip").Length;
                resPack.downloadPath = GetPackageStorePath() + "/" + resIndex.belong + "/" + resPack.id + ".zip";
            }
            else
            {
                resPack.downloadPath = GetPackageStorePath() + "/" + resIndex.belong;
            }

            resPack.releasePath = releasePath;
            resIndex.packageDict.Add(resPack.id, resPack);
        }

        string json = JsonConvert.SerializeObject(resIndex, Formatting.Indented);
        FileUtil.SaveFileText(OutputPath, indexFileName + ".json", json);
    }

    /// <summary>
    /// 找出剧情第4章以后的音频
    /// </summary>
    static void CopyFile_StoryChapter(BlackList blackList)
    {
        Dictionary<string, List<string>> dict = FindAllStoryAudio();

        Dictionary<string, List<string>> fileDict = new Dictionary<string, List<string>>();

        int fileCount = 0;

        Dictionary<string, bool> includeDict = new Dictionary<string, bool>();

        foreach (var json in dict)
        {
            if (json.Key.Contains("-"))
            {
                int result = 0;
                if (Int32.TryParse(json.Key[0].ToString(), out result))
                {
                    int result2 = 0;
                    if (Int32.TryParse(json.Key.Substring(0, 2), out result2))
                    {
                        if (CheckChapter(result2, blackList))
                        {
                            if (!fileDict.ContainsKey(result2.ToString()))
                            {
                                fileDict.Add(result2.ToString(), new List<string>());
                            }

                            fileDict[result2.ToString()].AddRange(json.Value);
                            fileCount++;
                        }
                        else
                        {
                            AddToInclude(ref includeDict, json.Value);
                        }
                    }
                    else
                    {
                        if (CheckChapter(result, blackList))
                        {
                            if (!fileDict.ContainsKey(result.ToString()))
                            {
                                fileDict.Add(result.ToString(), new List<string>());
                            }

                            fileDict[result.ToString()].AddRange(json.Value);
                            fileCount++;
                        }
                        else
                        {
                            AddToInclude(ref includeDict, json.Value);
                        }
                    }
                }
            }
        }

        Debug.LogError("剧情json数量：" + fileCount);

        string destDir = GetPackageDir() + "/StoryAudio";

        foreach (var file in fileDict)
        {
            DirectoryInfo dir = new DirectoryInfo(destDir + "/" + file.Key);
            if (dir.Exists == false)
                dir.Create();

            int count = 0;
            foreach (var fileName in file.Value)
            {
                CopyDubbingFile(dir.FullName, fileName);

                EditorUtility.DisplayProgressBar("复制音频文件",
                    "(" + count + "/" + file.Value.Count + ")" + dir.FullName,
                    (float) count / file.Value.Count);
            }
        }

        EditorUtility.ClearProgressBar();

        string packagePath = OutputPath + ModuleConfig.MODULE_MAIN_LINE;
        CreateZip(destDir, packagePath);
        GenerateIndexFile(destDir, Language, ModuleConfig.MODULE_MAIN_LINE,
            ResPath.MainStoryIndex, "story/dubbing", packagePath);

        //创建包含文件索引
        ResIndex resIndex = new ResIndex();
        resIndex.language = Language;
        resIndex.packageDict = new Dictionary<string, ResPack>();

        resIndex.belong = "IncludeFiles";
        ResPack resPack = new ResPack();
        resPack.id = "story0-story1";
        resPack.releasePath = "AssetBundles/" + SystemTag + "/story/dubbing";
        resPack.items = new List<ResItem>();
        resIndex.packageDict.Add("IncludePack", resPack);

        foreach (var include in includeDict)
        {
            ResItem item = new ResItem();
            item.Path = include.Key + ".bytes";
            resPack.items.Add(item);
        }

        string str = JsonConvert.SerializeObject(resIndex, Formatting.Indented);
        FileUtil.SaveFileText(OutputPath, ResPath.IncludeIndex + ".json", str);
    }

    private static bool CheckChapter(int result, BlackList blackList)
    {
        foreach (var name in blackList.List)
        {
            if (result.ToString() == name)
                return false;
        }

        return true;
    }

    static void AddToInclude(ref Dictionary<string, bool> dict, List<string> ids)
    {
        foreach (var id in ids)
        {
            if (dict.ContainsKey(id) == false)
                dict.Add(id, true);
        }
    }

    private static void CopyDubbingFile(string destDir, string fileName)
    {
        string pathTemp = "/AssetBundles/Android/story/dubbing/";
#if UNITY_IOS
        pathTemp = "/AssetBundles/iOS/story/dubbing/";
#endif
        string srcPath = AssetBundleHelper.GetOriginalBundlesPath() + pathTemp +
                         fileName + ".bytes";

        string destPath = destDir + "/" + fileName + ".bytes";
        File.Copy(srcPath, destPath, true);
    }

    static Dictionary<string, List<string>> FindLoveStoryAudio()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/DataCache/appointment_c");
        if (dir.Exists == false)
        {
            Debug.LogError("appointment_c文件夹不存在！");
            return null;
        }

        FileInfo[] files = dir.GetFiles("*");
        FileInfo targetFile = null;
        foreach (FileInfo file in files)
        {
            if (file.Name.ToLower().StartsWith("rules"))
            {
                targetFile = file;
                break;
            }
        }

        if (targetFile == null)
        {
            Debug.LogError("appointment_c/rules文件不存在！");
            return null;
        }

        Dictionary<string, List<string>> fileDict = new Dictionary<string, List<string>>();

        EditorUtility.DisplayProgressBar("查找LoveStory音频", "", 0);

        byte[] dataFile = FileUtil.GetBytesFile(targetFile.DirectoryName, targetFile.Name);
        MemoryStream m = new MemoryStream(dataFile);
        MessageParser<AppointmentRuleRes>
            parser = new MessageParser<AppointmentRuleRes>(() => new AppointmentRuleRes());
        AppointmentRuleRes res = parser.ParseFrom(m);
        int count = 0;
        foreach (var rule in res.AppointmentRules)
        {
            count++;
            string cardId = rule.ActiveCards[0].ToString();
            fileDict.Add(cardId, new List<string>());
            foreach (var pb in rule.GateInfos)
            {
                fileDict[cardId].AddRange(GetStoryAudio(pb.SceneId));
            }

            EditorUtility.DisplayProgressBar("查找LoveStory音频",
                "cardId:" + cardId + "(" + count + "/" + res.AppointmentRules.Count + ")",
                (float) count / res.AppointmentRules.Count);
        }

        string str = JsonConvert.SerializeObject(fileDict, Formatting.Indented);
        FileUtil.SaveFileText(GetPackageDir(), "LoveStoryAudio.json", str);

        EditorUtility.ClearProgressBar();

        return fileDict;
    }

    static Dictionary<string, List<string>> FindVisitStoryAudio()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/DataCache/visiting_c");
        if (dir.Exists == false)
        {
            Debug.LogError("visiting_c文件夹不存在！");
            return null;
        }

        FileInfo[] files = dir.GetFiles("*");
        FileInfo targetFile = null;
        foreach (FileInfo file in files)
        {
            if (file.Name.ToLower().StartsWith("rules"))
            {
                targetFile = file;
                break;
            }
        }

        if (targetFile == null)
        {
            Debug.LogError("visiting_c/rules文件不存在！");
            return null;
        }

        Dictionary<string, List<string>> fileDict = new Dictionary<string, List<string>>();

        EditorUtility.DisplayProgressBar("查找VisitStory音频", "", 0);

        byte[] dataFile = FileUtil.GetBytesFile(targetFile.DirectoryName, targetFile.Name);
        MemoryStream m = new MemoryStream(dataFile);
        MessageParser<VisitingRuleRes>
            parser = new MessageParser<VisitingRuleRes>(() => new VisitingRuleRes());
        VisitingRuleRes res = parser.ParseFrom(m);
        int count = 0;
        foreach (var rule in res.LevelRules)
        {
            count++;
            if (rule.Type == LevelTypePB.Story)
            {
                string playerId = ((int) rule.Player) + "";
                if (fileDict.ContainsKey(playerId) == false)
                    fileDict.Add(playerId, new List<string>());

                fileDict[playerId].AddRange(GetStoryAudio(rule.LevelMark));

                EditorUtility.DisplayProgressBar("查找VisitStory音频", "playerId:" + playerId + "(" + count + ")", 1);
            }
        }

        string str = JsonConvert.SerializeObject(fileDict, Formatting.Indented);
        FileUtil.SaveFileText(GetPackageDir(), "VisitStoryAudio.json", str);

        EditorUtility.ClearProgressBar();

        return fileDict;
    }

    static Dictionary<string, List<string>> FindAllStoryAudio()
    {
        DirectoryInfo dir = new DirectoryInfo(StoryJsonPath);
        FileInfo[] files = dir.GetFiles("*.json");

        Dictionary<string, List<string>> fileDict = new Dictionary<string, List<string>>();
        foreach (var file in files)
        {
            fileDict.Add(file.Name, new List<string>());
            fileDict[file.Name].AddRange(GetStoryAudio(file.Name));
        }

        string str = JsonConvert.SerializeObject(fileDict, Formatting.Indented);
        FileUtil.SaveFileText(GetPackageDir(), "StoryAudio.json", str);

        return fileDict;
    }

    static List<string> GetEventAudio(EventVo eventVo)
    {
        List<string> list = new List<string>();
        switch (eventVo.EventType)
        {
            case EventType.Telephone:
                list.AddRange(GetTelephoneAudio(eventVo.EventId));
                break;
            case EventType.Sms:
                list.AddRange(GetSmsAudio(eventVo.EventId));
                break;
            case EventType.Story:
                list.AddRange(GetStoryAudio(eventVo.EventId));
                break;
            case EventType.Selection:
                for (int i = 0; i < eventVo.SelectionTypes.Count; i++)
                {
                    switch (eventVo.SelectionTypes[i])
                    {
                        case EventType.Telephone:
                            list.AddRange(GetTelephoneAudio(eventVo.SelectionIds[i]));
                            break;
                        case EventType.Sms:
                            list.AddRange(GetSmsAudio(eventVo.SelectionIds[i]));
                            break;
                        case EventType.Story:
                            list.AddRange(GetStoryAudio(eventVo.SelectionIds[i]));
                            break;
                    }
                }

                break;
        }

        return list;
    }

    private static List<string> GetStoryAudio(string eventId)
    {
        eventId = eventId.Replace(".json", "");

        List<string> list = new List<string>();

        FileInfo fileInfo = new FileInfo(StoryJsonPath + "/" + eventId + ".json");
        if (fileInfo.Exists == false)
        {
            Debug.LogError("剧情json文件不存在=====ID:" + eventId);
            return list;
        }

        string text = FileUtil.ReadFileText(fileInfo.FullName);

        List<DialogVo> dialogList = JsonConvert.DeserializeObject<List<DialogVo>>(text);

        foreach (var dialogVo in dialogList)
        {
            //处理剧情里的对话音频
            if (string.IsNullOrEmpty(dialogVo.DubbingId) == false)
            {
                if(sharedAudioDict != null && sharedAudioDict.ContainsKey(dialogVo.DubbingId))
                    continue;
                
                list.Add(dialogVo.DubbingId);
            }

            if (dialogVo.Event != null)
            {
                list.AddRange(GetEventAudio(dialogVo.Event));
            }
        }

        return list;
    }

    private static List<string> GetTelephoneAudio(string eventId)
    {
        eventId = eventId.Replace(".json", "");

        List<string> list = new List<string>();

        FileInfo fileInfo =
            new FileInfo(I18NDataPath + "/Telphone/" + eventId + ".json");
        if (fileInfo.Exists == false)
        {
            Debug.LogError("手机json文件不存在=====ID:" + eventId);
        }
        else
        {
            string text = FileUtil.ReadFileText(fileInfo.FullName);
            TelephoneVo vo = JsonConvert.DeserializeObject<TelephoneVo>(text);
            foreach (var telephoneDialogVo in vo.dialogList)
            {
                if (string.IsNullOrEmpty(telephoneDialogVo.AudioId) == false)
                {
                    if(sharedAudioDict != null && sharedAudioDict.ContainsKey(telephoneDialogVo.AudioId))
                        continue;
                    
                    list.Add(telephoneDialogVo.AudioId);
                }
            }

            if (vo.Event != null)
            {
                list.AddRange(GetEventAudio(vo.Event));
            }
        }

        return list;
    }

    private static List<string> GetSmsAudio(string eventId)
    {
        eventId = eventId.Replace(".json", "");

        List<string> list = new List<string>();

        FileInfo fileInfo = new FileInfo(I18NDataPath + "/Sms/" + eventId + ".json");
        if (fileInfo.Exists == false)
        {
            Debug.LogError("短信json文件不存在=====ID:" + eventId);
        }
        else
        {
            string text = FileUtil.ReadFileText(fileInfo.FullName);
            SmsVo vo = JsonConvert.DeserializeObject<SmsVo>(text);
            if (vo.Event != null)
            {
                list.AddRange(GetEventAudio(vo.Event));
            }
        }

        return list;
    }


    private static Dictionary<int, Dictionary<string, bool>> FindPhoneAudio()
    {
        DirectoryInfo dir = new DirectoryInfo(PhoneJsonPath);
        FileInfo[] files = dir.GetFiles("*.json");

        Dictionary<int, Dictionary<string, bool>> fileDict = new Dictionary<int, Dictionary<string, bool>>();
        foreach (var file in files)
        {
            string num = file.Name.Replace(".json", "");
            int result;
            if (int.TryParse(num, out result) == false || result >= 20000)
                continue;

            string text = FileUtil.ReadFileText(file.FullName);
            SmsInfo info = JsonConvert.DeserializeObject<SmsInfo>(text);
            int npcId = -1;
            foreach (var talkInfo in info.smsTalkInfos)
            {
                if (talkInfo.NpcId != 0)
                {
                    npcId = talkInfo.NpcId;
                }

                string mId = talkInfo.MusicID;
                if (string.IsNullOrEmpty(mId) || mId == "0")
                    continue;

                if (fileDict.ContainsKey(npcId) == false)
                    fileDict[npcId] = new Dictionary<string, bool>();

                if (fileDict[npcId].ContainsKey(mId) == false)
                {
                    fileDict[npcId].Add(mId, true);
                }
            }
        }

        string str = JsonConvert.SerializeObject(fileDict, Formatting.Indented);
        FileUtil.SaveFileText(GetPackageDir(), "PhoneAudio.json", str);

        return fileDict;
    }

    static void CopyFile_PhoneAudio(BlackList blackList)
    {
        string destDir = GetPackageDir() + "/PhoneAudio";
        if (Directory.Exists(destDir) == false)
            Directory.CreateDirectory(destDir);

        string srcPath = GetAssetBundleDir() + "/music/phonedialogs";

        Dictionary<int, Dictionary<string, bool>> fileDict = FindPhoneAudio();
        foreach (var dir in fileDict)
        {
            string subDir = destDir + "/" + dir.Key;
            if (Directory.Exists(subDir) == false)
                Directory.CreateDirectory(subDir);

            foreach (var file in dir.Value)
            {
                string src = srcPath + "/" + file.Key.ToLower() + ".music";
                if (IsContains(src, blackList))
                    continue;

                string targetPath = subDir + "/" + file.Key.ToLower() + ".music";
                File.Copy(src, targetPath, true);
            }
        }

        string packagePath = OutputPath + ModuleConfig.MODULE_PHONE;

        //打个整包
        CreateZip(destDir, packagePath, true);
        GenerateIndexFile(destDir, Language, ModuleConfig.MODULE_PHONE, ResPath.PhoneAudioPackageIndex,
            "music/phonedialogs", packagePath, true);

        EditorUtility.ClearProgressBar();
    }

    private static bool IsContains(string src, BlackList blackList)
    {
        foreach (var name in blackList.List)
        {
            if (src.Contains(name))
                return true;
        }

        return false;
    }
    
    
    public static Dictionary<string, bool> FindSharedAudio()
    {
        Stopwatch sw = Stopwatch.StartNew();
        
        Dictionary<string, bool> dict = new Dictionary<string, bool>();
        
        DirectoryInfo dir = new DirectoryInfo(StoryJsonPath);
        FileInfo[] files = dir.GetFiles("*.json");

        foreach (var file in files)
        {
            List<string> list = GetStoryAudio(file.Name);

            foreach (var id in list)
            {
                if(dict.ContainsKey(id) == false)
                    dict.Add(id, false);
                else if(dict[id] == false)
                {
                    dict[id] = true;
                }
            }
        }

        Dictionary<string,bool> fileDict = new Dictionary<string, bool>();
        foreach (var id in dict)
        {
            if (id.Value)
            {
                fileDict.Add(id.Key, true);
            }
        }

        sharedAudioDict = fileDict;

        Debug.Log("<color='#00ff66'>FindSharedAudio：" + sw.ElapsedMilliseconds/1000.0f + "s</color>");
        return fileDict;
    }
    
//    [MenuItem("AssetBundle/===发布===/TEST")]
    public static void AllBundleConfig()
    {
        Stopwatch sw = Stopwatch.StartNew();
        
        Dictionary<string, BundleStruct> dict = new Dictionary<string, BundleStruct>();
        
        DirectoryInfo dir = new DirectoryInfo(GetAssetBundleDir());

        string rootPath = GetAssetBundleDir().Replace("\\", "/");
        int len = rootPath.Length;
        FileInfo[] files = dir.GetFiles("*.*", SearchOption.AllDirectories);

        foreach (var fileInfo in files)
        {
            if (fileInfo.Extension == ".manifest")
                continue;
            
            string key = fileInfo.FullName.Replace("\\", "/");
            key = key.Substring(len+1);

            BundleStruct val = new BundleStruct();
            val.Md5 = MD5Util.Get(fileInfo.FullName);
            val.Size = fileInfo.Length;
            dict.Add(key, val);
        }
        
        byte[] contentBuffer;
        using (MemoryStream stream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, dict);

            contentBuffer = stream.GetBuffer();
        }
            
        using (FileStream fileStream = new FileStream(OutputPath + "/bundle.config", FileMode.Create, FileAccess.Write, FileShare.None))
        {
            BinaryWriter br = new BinaryWriter(fileStream);
            br.Write(contentBuffer);
            br.Close();
        }
        
        Debug.Log("<color='#00ff66'>AllBundleConfig：" + sw.ElapsedMilliseconds/1000.0f  + "s</color>");
    }
    
//    [MenuItem("AssetBundle/===发布===/TEST222222")]
    public static void ReadConfig()
    {
        FileStream stream = new FileStream(OutputPath + "/bundle.config", FileMode.Open);
      
        BinaryFormatter b = new BinaryFormatter();
        Dictionary<string, BundleStruct> config = b.Deserialize(stream) as Dictionary<string, BundleStruct>;
    }
}