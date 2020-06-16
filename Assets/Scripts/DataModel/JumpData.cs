using Assets.Scripts.Module;
using UnityEngine;

namespace DataModel
{
    /// <summary>
    /// 模块跳转
    /// </summary>
    public class JumpData
    {
        /// <summary>
        /// 要跳转的模块
        /// </summary>
        public string Module;
        
        /// <summary>
        /// 类型
        /// </summary>
        public string Type;
        
        /// <summary>
        /// 数据
        /// </summary>
        public string Data;

        /// <summary>
        /// 用于显示的文本
        /// </summary>
        public string DisplayText;

        public ResourcePB RequireType;
        public int RequireId;
        public int RequireNum;

        public object PostData;
        
        public void ParseData(string text)
        {
            string[] arr = text.Split('_');
            if (arr.Length == 1)
            {
                Module = text.Trim();
            }
            else
            {
                Module = arr[0].Trim();
                Data = arr[1].Trim();
            }

            if (Module == ModuleConfig.MODULE_MAIN_LINE)
            {
               
                DisplayText = I18NManager.Get("Common_Hint1", Data);
                RequireType = ResourcePB.Item;
            }
            else if(Module == ModuleConfig.MODULE_RECOLLECTION)
            {
                DisplayText = I18NManager.Get("Common_Recollection");
                RequireType = ResourcePB.Item;
            }
            else
            {
                Debug.LogError("Error Module->"+Module);
            }

        }
    }
}