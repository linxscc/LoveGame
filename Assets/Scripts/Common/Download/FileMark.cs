using System.IO;
using System.Text;
using Framework.Utils;

namespace Assets.Scripts.Module.Download
{
    /// <summary>
    /// 用于标记下载文件的状态
    /// </summary>
    public class FileMark
    {
        public string Path;

        public string Key;

        public string Value;

        public FileMark(string path, string key)
        {
            Path = path;
            Key = key;

            Value = "";
        }

        public bool IsMatch
        {
            get
            {
                ReadRecord();
                return Value == AppConfig.Instance.version + "";
            }
        }

        public string ReadRecord()
        {
            Value = FileUtil.ReadFileText(Path + "/" + Key);
            return Value;
        }
        public void UpdateRecord(string value = null)
        {
            if (value != null)
                Value = value;

            string path = Path + "/" + Key;
            string dir = System.IO.Path.GetDirectoryName(path);
            if (Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);
            
            FileStream fileStream =
                new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            byte[] bytes = Encoding.UTF8.GetBytes(Value);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }
        
        public void Delete()
        {
            File.Delete(Path + "/" + Key);
        }
    }
}