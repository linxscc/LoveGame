using System.Collections.Generic;

namespace Assets.Scripts.Module.Download
{
    public class CacheVo
    {
        /// <summary>
        /// 未下载的ID
        /// </summary>
        public List<int> ids;

        /// <summary>
        /// 下载包大小
        /// </summary>
        public List<long> sizeList;
        
        /// <summary>
        /// 是否需要下载
        /// </summary>
        public bool needDownload;
    }
}