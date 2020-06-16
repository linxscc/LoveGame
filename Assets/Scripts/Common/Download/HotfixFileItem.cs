using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace Assets.Scripts.Module.Download
{
    [Serializable]
    public class HotfixFileItem : IDownloadItem
    {
        public long Size { get; set; }
        public string Path { get; set; }
        public string Md5 { get; set; }

        public FileType FileType { get; set; }


#if UNITY_EDITOR
        /// <summary>
        /// 本地文件绝对路径
        /// </summary>
        ///
        [NonSerialized] public string AbsPath;
       

        public HotfixFileItem(string absPath)
        {
            AbsPath = absPath;

            FileInfo fileInfo = new FileInfo(absPath);

            Init(fileInfo);
        }

        public HotfixFileItem(FileInfo fileInfo)
        {
            Init(fileInfo);
        }
        private void Init(FileInfo fileInfo)
        {
            AbsPath = fileInfo.FullName;

            Size = (int) fileInfo.Length;

            string hotfixDir = Application.dataPath.Replace("SuperStarGame/Assets", "Hotfix/").Replace("\\", "/");

            Path = AbsPath.Replace("\\", "/").Replace(hotfixDir, "");

            using (FileStream stream = new FileStream(AbsPath, FileMode.Open, FileAccess.Read))
            {
                Md5 = BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(stream)).Replace("-", "");
            }
        }
#endif
        
    }

    public enum FileType
    {
        None,
        /// <summary>
        /// zip压缩包
        /// </summary>
        Zip,
        /// <summary>
        /// 原始文件
        /// </summary>
        Original,
    }
}