using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Debug = UnityEngine.Debug;

namespace Utils
{
    public class ZipUtil
    {
        public static long TotalSize { get; private set; }
        public static long CurrentSize { get; private set; }

        public static void UnzipThread(string zipFilePath, string targetDir)
        {
            DirectoryInfo dir = new DirectoryInfo(targetDir);
            if (dir.Exists == false)
                dir.Create();

            ZipFile zFile = new ZipFile(zipFilePath);
            TotalSize = 0;
            CurrentSize = 0;
            Stopwatch sw = Stopwatch.StartNew();
            foreach (ZipEntry e in zFile)
            {
                TotalSize += e.Size;
            }
            zFile.Close();
            Debug.LogWarning("计算压缩包大小=====>"+sw.ElapsedMilliseconds + "ms");

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(targetDir + "/" + directoryName);
                    }

                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(targetDir + "/" + theEntry.Name))
                        {
                            int size = 2048;
                            byte[] data = new byte[size];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                    CurrentSize += size;
                                }
                                else
                                    break;
                            }
                        }
                    }
                }
            }
            
            Debug.LogWarning("ZipFile解压完成：" + zipFilePath + "\n解压目录" + targetDir);
        }

        public static void Unzip(string zipFilePath, string targetDir)
        {
            DirectoryInfo dir = new DirectoryInfo(targetDir);
            if (dir.Exists == false)
                dir.Create();

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    Console.WriteLine(theEntry.Name);

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(targetDir + "/" + directoryName);
                    }

                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(targetDir + "/" + theEntry.Name))
                        {
                            int size = 2048;
                            byte[] data = new byte[size];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            Debug.LogWarning("ZipFile解压完成：" + zipFilePath + "\n解压目录" + targetDir);
        }

        public static IEnumerator Unzip(string zipFilePath, string targetDir, Action<float> onProgress, int frameBuffer = 2 * 1024 * 1024)
        {
            Debug.LogWarning("frameBuffer==========>" + frameBuffer);
            
            DirectoryInfo dir = new DirectoryInfo(targetDir);
            if (dir.Exists == false)
                dir.Create();

            ZipFile zFile = new ZipFile(zipFilePath);
            int fileCount = zFile.Size;

            int count = 0;
            long sizeCount = 0;
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    Console.WriteLine(theEntry.Name);

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(targetDir + "/" + directoryName);
                    }

                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(targetDir + "/" + theEntry.Name))
                        {
                            int size = 2048;
                            byte[] data = new byte[size];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    try
                                    {
                                        streamWriter.Write(data, 0, size);
                                    }
                                    catch (IOException e)
                                    {
                                        Debug.LogError("IOException=>" + e.Message);
                                        onProgress?.Invoke(-1);
                                        yield break;
                                    }
                                }
                                else
                                {
                                    break;
                                }

                                sizeCount += size;
                            }
                        }
                    }

                    count++;
                    
                    if (sizeCount > frameBuffer)
                    {
                        onProgress?.Invoke((float)count/fileCount);
                        sizeCount = 0;
                        yield return null;
                    }
                       
                }
            }
            
            onProgress?.Invoke(1);

            Debug.LogWarning("ZipFile解压完成：" + zipFilePath + "\n解压目录" + targetDir);
        }

        /// <summary>
        /// 创建Zip压缩包
        /// </summary>
        /// <param name="filenames">文件列表</param>
        /// <param name="zipFilePath">压缩包输出路径</param>
        /// <param name="pathInZipList">文件在压缩包里面的父路径，传一个值表示应用于所有文件，默认null表示没有父路径，长度和filenames一样表示一一对应，长度是其他情况抛出异常</param>
        /// <param name="quality">0-9，默认6</param>
        public static void CreateZip(string[] filenames, string zipFilePath, string[] pathInZipList = null,
            int quality = 6)
        {
            string parentPath = "";
            bool useParentPath = false;

            if (pathInZipList != null)
            {
                if (pathInZipList.Length == 1)
                {
                    parentPath = pathInZipList[0] + "/";
                }
                else
                {
                    if (pathInZipList.Length == filenames.Length)
                    {
                        useParentPath = true;
                    }
                    else
                    {
                        throw new Exception("pathInZip数组长度不合法");
                    }
                }
            }

            using (ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipFilePath)))
            {
                zipStream.SetLevel(quality); // 0 - store only to 9 - means best compression

                byte[] buffer = new byte[4096];

                for (int i = 0; i < filenames.Length; i++)
                {
                    string file = filenames[i];
                    if (useParentPath)
                    {
                        parentPath = pathInZipList[i];
                    }

                    var entry = new ZipEntry(parentPath + Path.GetFileName(file));
                    entry.DateTime = DateTime.Now;
                    zipStream.PutNextEntry(entry);

                    using (FileStream fs = File.OpenRead(file))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            zipStream.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }

                zipStream.Finish();
                zipStream.Close();
            }

            Debug.Log("<color='#666666'>ZipFile压缩完成：</color>" + zipFilePath);
        }
    }
}