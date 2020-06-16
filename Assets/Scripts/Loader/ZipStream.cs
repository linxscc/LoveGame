using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace SharpZipLib
{
    public class ZipStream : IDisposable
    {
        private ZipFile mZipfile = null;
        private Dictionary<string, int> mZipEntrys = new Dictionary<string, int>();

        public void Init(string filepath)
        {
            try
            {
                mZipfile = new ZipFile(filepath);
                InitFileList();
            }
            catch (Exception ex)
            {
                throw new Exception($"ZipStream.init error! {ex.Message} ,StackTrace: {ex.StackTrace}");
            }
        }

        public bool Contains(string file)
        {
            return mZipEntrys.ContainsKey(file);
        }

        private void InitFileList()
        {
            foreach (object obj in mZipfile)
            {
                ZipEntry zipEntry = obj as ZipEntry;
                if (zipEntry.IsFile)
                    mZipEntrys.Add(zipEntry.Name.ToLower(), zipEntry.ZipFileIndex);
            }
        }

        public Stream FindFileStream(string file)
        {
            if (mZipEntrys.TryGetValue(file.ToLower(), out var entryIndex))
                return mZipfile.GetInputStream(entryIndex);

            return null;
        }

        public void EachAllFile(Action<string> fun)
        {
            foreach (KeyValuePair<string, int> mZipEntry in mZipEntrys)
                fun(mZipEntry.Key);
        }

        public void Dispose()
        {
            if (mZipfile == null)
                return;
            mZipfile.Close();
            mZipfile = null;
        }
    }
}