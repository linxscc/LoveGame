using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Module.Download
{
    [Serializable]
    public class HotfixIndexFile
    {
        public const string FileHeader = "HIF";

        public int HotVersion = 1;
        
        public int FileVersion = 1;
        
        public List<HotfixFileItem> FileItems;

        public string PackageFileName;

        public string PackageFileMd5;

        public int PackageFileSize;

        public long FileTotalSize = 0;

        [NonSerialized]
        public HotfixFileFormat HotfixFileFormat;

#if UNITY_EDITOR
        public void AddFile(string filePath)
        {
            if (FileItems == null)
                FileItems = new List<HotfixFileItem>();

            HotfixFileItem item = new HotfixFileItem(filePath);
            FileItems.Add(item);
            FileTotalSize += item.Size;
        }
        
        public void AddFile(FileInfo fileInfo)
        {
            if (FileItems == null)
                FileItems = new List<HotfixFileItem>();

            HotfixFileItem item = new HotfixFileItem(fileInfo);
            FileItems.Add(item);
            FileTotalSize += item.Size;
        }
#endif
        public static HotfixIndexFile Deserialize(byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BinaryReader binaryReader = new BinaryReader(stream);

            string header = Encoding.UTF8.GetString(binaryReader.ReadBytes(4));
            if (header.Contains(FileHeader) == false)
            {
                Debug.LogError("HotfixIndexFile: File Format Error");
                return null;
            }
            
            int fileVersion = binaryReader.ReadInt32();
            HotfixFileFormat format = (HotfixFileFormat) binaryReader.ReadInt32();

            BinaryFormatter b = new BinaryFormatter();
            HotfixIndexFile clz = b.Deserialize(stream) as HotfixIndexFile;
            
            stream.Close();
            binaryReader.Close();
            
            return clz;
        }
        
        public void GenerateBin(string path, HotfixFileFormat format, int hotVersion)
        {
            HotVersion = hotVersion;
            
            if (FileItems == null || FileItems.Count == 0)
                return;

            byte[] contentBuffer;
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);

                contentBuffer = stream.GetBuffer();
            }
            
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                BinaryWriter br = new BinaryWriter(fileStream);
                br.Write(FileHeader);
                br.Write(FileVersion);
                br.Write((int) HotfixFileFormat);

                if (HotfixFileFormat == HotfixFileFormat.EncryptZip)
                {
                    contentBuffer = Encrypt(contentBuffer);
                }
                
                br.Write(contentBuffer);
                br.Close();
            }
        }

        private byte[] Encrypt(byte[] contentBuffer)
        {
            return contentBuffer;
        }
    }

    
    [Serializable]
    public enum HotfixFileFormat
    {
        None,
        EncryptZip
    }
}