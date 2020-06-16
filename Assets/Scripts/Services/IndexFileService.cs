using System.Diagnostics;
using Assets.Scripts.Framework.GalaSports.Core;
using Newtonsoft.Json;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.Services
{
    public class IndexFileService : LocalService<ResIndex>
    {
        public IndexFileService SetPath(string path)
        {
            resPath = path;

            return this;
        }
        
        protected override void OnExecute()
        {
            useAsync = false;
            resType = ResType.Text;
        }

        protected override void ProcessData(object text)
        {
            Stopwatch sw = Stopwatch.StartNew();
            _data = JsonConvert.DeserializeObject<ResIndex>(text as string);

            Debug.Log(resPath + "IndexFileService ProcessData time：" + sw.ElapsedMilliseconds);
            sw.Stop();
        }
    }
}