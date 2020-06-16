using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;

namespace Framework.GalaSports.Service
{
    public class ServiceQueue
    {
        private int _count;
        private Action<List<IService>> _onFinish;
        private List<IService> _queueList;
        private List<IService> _failList;

        /// <summary>
        /// Service队列，按顺序执行Service
        /// </summary>
        /// <param name="onFinish">回传失败Service列表</param>
        public ServiceQueue(Action<List<IService>> onFinish)
        {
            _onFinish = onFinish;
            
            _failList = new List<IService>();
            _queueList = new List<IService>();

            _count = 0;
        }

        public ServiceQueue Append(IService service)
        {
            _queueList.Add(service);
            return this;
        }

        public ServiceQueue Start()
        {
            if (_queueList.Count == 0)
                return this;
            DoNext();

            return this;
        }

        private void DoNext()
        {
            IService service = _queueList[_count];
            service.Execute();
            service.OnFinish = OnNext;
            _count++;
        }

        private void OnNext(bool isSuccess, IService service)
        {
            if (isSuccess == false)
            {
                _failList.Add(service);
            }
            if (_count >= _queueList.Count)
            {
                _onFinish?.Invoke(_failList);
            }
            else
            {
                DoNext();
            }
        }
    }
}