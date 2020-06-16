using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Framework.Utils
{
    public class FileUtil
    {
        public static byte[] GetBytesFile(string path, string name)
        {
            if (File.Exists(path + "/" + name))
            {
                Debug.Log("GetBytesFile:" + path + "/" + name);
                byte[] data = File.ReadAllBytes(path + "/" + name);
                return data;
            }

            return null;
        }

        public static void SaveBytesFile(string path, string name, byte[] data)
        {
            Debug.Log("SaveBytesFile:" + path + "/" + name);
            if ((File.Exists(path + "/" + name)))
            {
                DeleteFile(path, name);
            }

            if (!File.Exists(path + "/" + name))
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                File.WriteAllBytes(path + "/" + name, data);
            }
        }

        /// <summary>
        /// 获取文件夹下所有文件
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="extensionName">扩展名</param>
        /// <returns></returns>
        public static List<string> GetDirectoryAllFiles(string path, List<string> extensionName = null)
        {
            List<string> list = new List<string>();

            foreach (var file in Directory.GetFiles(path))
            {
                if (extensionName != null)
                {
                    foreach (var suffix in extensionName)
                    {
                        if (Path.GetExtension(file) == suffix)
                            list.Add(Util.PathNormalize(file));
                    }
                }
                else
                    list.Add(Util.PathNormalize(file));
            }

            foreach (var dir in Directory.GetDirectories(path))
            {
                list.AddRange(GetDirectoryAllFiles(dir, extensionName));
            }

            return list;
        }
        
        public static void DeleteFile(string path, string name)
        {
            if (File.Exists(path + "/" + name))
                File.Delete(path + "/" + name);
        }

        public static byte[] Object2Bytes(object obj)
        {
            byte[] buff;
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter iFormatter = new BinaryFormatter();
                iFormatter.Serialize(ms, obj);
                buff = ms.GetBuffer();
            }

            return buff;
        }

        public static object Bytes2Object(byte[] buff)
        {
            object obj;
            using (MemoryStream ms = new MemoryStream(buff))
            {
                IFormatter iFormatter = new BinaryFormatter();
                obj = iFormatter.Deserialize(ms);
            }

            return obj;
        }

        public static void CheckFileSavePath(string path)
        {
            string realPath = path;
            int ind = path.LastIndexOf("/", StringComparison.Ordinal);
            if (ind >= 0)
            {
                realPath = path.Substring(0, ind);
            }
            else
            {
                ind = path.LastIndexOf("\\", StringComparison.Ordinal);
                if (ind >= 0)
                {
                    realPath = path.Substring(0, ind);
                }
            }

            if (!Directory.Exists(realPath))
            {
                Directory.CreateDirectory(realPath);
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void SaveFileText(string path, string content, bool needUtf8 = false)
        {
            if (needUtf8)
            {
                File.WriteAllText(path, content, Encoding.UTF8);
            }
            else
            {
                File.WriteAllText(path, content, Encoding.Default);
            }
        }

        public static string ConvertJsonString(string str)
        {
            //格式化json字符串  
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }

        //保存文件并对路径做判断
        public static void SaveFileText(string path, string name, string content, bool needUtf8 = true)
        {
            if (File.Exists(path + "/" + name))
            {
            }
            else
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            if (needUtf8)
            {
                File.WriteAllText(path + "/" + name, content, new UTF8Encoding(false));
            }
            else
            {
                File.WriteAllText(path + "/" + name, content, Encoding.Default);
            }
        }

        public static void SaveLineText(string path, string lines)
        {
            CheckFileSavePath(path);
            StreamWriter f = new StreamWriter(path, true);
            f.WriteLine(lines);
            f.Close();
        }

        public static string ReadFileText(string path, string fileName)
        {
            if (!Directory.Exists(path))
            {
                return "";
            }

            if (!File.Exists(path + fileName))
            {
                return "";
            }

            string str = File.ReadAllText(path + fileName, Encoding.Default);
            return str;
        }

        public static string ReadFileText(string path)
        {
            if (!File.Exists(path))
            {
                return "";
            }

            string str = File.ReadAllText(path, Encoding.Default);
            return str;
        }

        public static List<string> GetAllFiles(string path, string exName = null)
        {
            if (!Directory.Exists(path))
            {
                return null;
            }

            bool checkExName = false;
            if (!string.IsNullOrEmpty(exName))
            {
                checkExName = true;
                exName = exName.ToLower();
            }

            List<string> names = new List<string>();
            DirectoryInfo root = new DirectoryInfo(path);
            FileInfo[] files = root.GetFiles();
            string tempExName;
            for (int i = 0; i < files.Length; i++)
            {
                if (checkExName)
                {
                    tempExName = GetFileExName(files[i].FullName);
                    if (string.IsNullOrEmpty(tempExName)) continue;
                    tempExName = tempExName.ToLower();
                    if (!tempExName.Equals(exName))
                    {
                        continue;
                    }
                }

                names.Add(files[i].FullName.Replace('\\', '/'));
            }

            DirectoryInfo[] dirs = root.GetDirectories();
            if (dirs.Length > 0)
            {
                for (int i = 0; i < dirs.Length; i++)
                {
                    List<string> subNames = GetAllFiles(dirs[i].FullName, exName);
                    if (subNames.Count > 0)
                    {
                        for (int j = 0; j < subNames.Count; j++)
                        {
                            names.Add(subNames[j].Replace('\\', '/'));
                        }
                    }
                }
            }

            return names;
        }

        //获取文件路径的扩展名
        public static string GetFileExName(string filepath)
        {
            string exName = "";
            filepath = filepath.Replace('\\', '/');
            string fileName = filepath;

            string[] fileStrs = fileName.Split('/');
            if (fileStrs.Length > 1)
            {
                fileName = fileStrs[fileStrs.Length - 1];
            }

            string[] nameStrs = fileName.Split('.');
            if (nameStrs.Length > 1)
            {
                exName = nameStrs[nameStrs.Length - 1];
            }

            return exName;
        }

        public static void SpliteFilePathAndName(string filepath, out string destPath, out string fileName)
        {
            filepath = filepath.Replace('\\', '/');
            int index = filepath.LastIndexOf('/');
            if (index == -1)
            {
                destPath = "";
                fileName = filepath;
                return;
            }
            destPath = filepath.Substring(0, index);
            fileName = filepath.Substring(index + 1, filepath.Length - index - 1);
        }



        public static byte[] ReadBytesFile(string path)
        {
            if (File.Exists (path)) {
                byte[] data = File.ReadAllBytes (path);
                return data;
            }
            return null;
        }

        public static bool CopyFolder(string srcPath, string destPath, string ignore = null)
        {
#if UNITY_EDITOR_WIN
            srcPath = srcPath.Replace("/", "\\");
#endif
            
            Debug.LogWarning("CopyFolder->srcPath:" + srcPath + "  destPath:" + destPath);
            if(!Directory.Exists(srcPath))
                return false;

            srcPath = srcPath.Replace("\\", "/");
            
            string[] files = Directory.GetFiles(srcPath, "*", SearchOption.AllDirectories);

            foreach (var filePath in files)
            {
                if(ignore != null && filePath.Contains(ignore))
                    continue;

                string path = filePath.Replace("\\", "/");
                string relatedPath = path.Replace(srcPath, "");

                string targetPath = destPath + "/" + relatedPath;

                string dir = Path.GetDirectoryName(targetPath);
                if (Directory.Exists(dir) == false)
                    Directory.CreateDirectory(dir);

                File.Copy(path, targetPath, true);
            }

            return true;
        }
        
        private static void CopyFolder(object obj)
        {
            object[] arr = obj as object[];
            string srcPath = arr[0] as string;
            string destPath = arr[1] as string;
            string[] files = arr[2] as string[];

            if(!Directory.Exists(srcPath))
                return;
            
            foreach (var filePath in files)
            {
                string path = filePath.Replace("\\", "/");
                string relatedPath = path.Replace(srcPath, "");

                string targetPath = destPath + "/" + relatedPath;

                string dir = Path.GetDirectoryName(targetPath);
                if (Directory.Exists(dir) == false)
                    Directory.CreateDirectory(dir);

                File.Copy(path, targetPath, true);
            }
        }
        
        public static void CopyFolderThread(string srcPath, string destPath, Action onComplete, string ignore = null)
        {
#if UNITY_EDITOR_WIN
            srcPath = srcPath.Replace("/", "\\");
#endif
            
            Debug.LogWarning("CopyFolder->srcPath:" + srcPath + "  destPath:" + destPath);
            if(!Directory.Exists(srcPath))
                return;

            srcPath = srcPath.Replace("\\", "/");

            int threadNum = 8;

            List<List<string>> fileList = new List<List<string>>();
            for (int i = 0; i < threadNum; i++)
            {
                fileList.Add(new List<string>());
            }
            
            string[] files = Directory.GetFiles(srcPath, "*", SearchOption.AllDirectories);
            int count = 0;
            foreach (var filePath in files)
            {
                if(ignore != null && filePath.Contains(ignore))
                    continue;
                count++;
                fileList[count%threadNum].Add(filePath);
            }

            for (int i = 0; i < threadNum; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(CopyFolder));
                object obj = new object[]{srcPath, destPath, fileList[i].ToArray()};
                thread.Start(obj);
                
            }
        }

    }
}