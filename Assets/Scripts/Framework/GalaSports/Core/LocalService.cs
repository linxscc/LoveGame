using System;
using game.main;

namespace Assets.Scripts.Framework.GalaSports.Core
{
    public abstract class LocalService<T> : Service<T>
    {
        /// <summary>
        ///  是否异步加载（默认：异步）
        /// </summary>
        protected bool useAsync;

        /// <summary>
        /// 资源路径
        /// </summary>
        protected string resPath;

        /// <summary>
        /// 资源存储类型（默认：文本）
        /// </summary>
        protected ResType resType;

        protected LocalService()
        {
            useAsync = true;
            resType = ResType.Text;
        }

        protected Action<T> OnComplete;

        public LocalService<T> SetCallback(Action<T> onComplete)
        {
            OnComplete = onComplete;
            return this;
        }

        protected override void DoExecute()
        {
            if (resType == ResType.Text)
            {
                if (useAsync)
                {
                    new AssetLoader().LoadText(resPath, (text, loader) =>
                    {
                        ProcessData(text);
                        OnLoadData();
                    });
                }
                else
                {
                    string str = new AssetLoader().LoadTextSync(resPath);
                    ProcessData(str);
                    OnLoadData();
                }
            }
            else
            {
                if (useAsync)
                {
                    throw new Exception("二进制加载不支持异步");
                }
                else
                {
                    byte[] bytes = new AssetLoader().LoadBytes(resPath);
                    ProcessData(bytes);
                    OnLoadData();
                }
            }
        }

        protected abstract void ProcessData(object data);

        protected override void OnLoadData()
        {
            OnComplete?.Invoke(_data);
            OnFinish?.Invoke(true, this);
        }

        protected enum ResType
        {
            Text,
            Binary
        }
    }
}