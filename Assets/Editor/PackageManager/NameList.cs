using System.Collections.Generic;
using System.IO;

    public class NameList
    {
        /// <summary>
        /// 根目录
        /// </summary>
        public string RootFolder;
        
        /// <summary>
        /// 通配符
        /// </summary>
        protected string Wildcard;

        public NameList()
        {
            List = new List<string>();
        }

        /// <summary>
        /// 名单
        /// </summary>
        public List<string> List;
        
    }
