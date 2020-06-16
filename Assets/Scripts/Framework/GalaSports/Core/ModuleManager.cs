using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using UnityEngine;

namespace Assets.Scripts.Framework.GalaSports.Core
{
    class ModuleManager : MonoBehaviour
    {
        private IModule _currentModule;
        private IModule _currentCommonModule;
        private List<string> _modulePath = new List<string>();
        public static float OffY;
        private GameObject _moduleParent;
        private GameObject _commonParent;
        private Dictionary<string, IModule> _modulesDic;
        private Dictionary<string, IModule> _commonModulesDic;

        private static ModuleManager _instance;

        public static ModuleManager Instance
        {
            get { return _instance; }
        }

        public void Awake()
        {
            _instance = this;
            _modulesDic = new Dictionary<string, IModule>();
            _commonModulesDic = new Dictionary<string, IModule>();
        }

        public GameObject InstantiatePrefab(string resUrl)
        {
            GameObject obj = Instantiate(ResourceManager.Load<GameObject>("module/"+resUrl));
            return obj;
        }

        public void DestroyObj(GameObject obj)
        {
            DestroyImmediate(obj);
        }

        public void SetOffY(float offY)
        {
            OffY = offY;
        }

        public void SetContainer(GameObject parent)
        {
            _moduleParent = parent;
        }

        bool isOpening = false;

        /// <summary>
        /// 进入一个模块
        /// </summary>
        /// <param name="moduleName">名字</param>
        /// <param name="removePrev">是否移除上一个模块</param>
        /// <param name="hidePrev">是否隐藏上一个模块</param>
        /// <param name="paramObjects">传参</param>
        /// <returns></returns>
        public void EnterModule(string moduleName, bool removePrev = true, bool hidePrev = false,
            params object[] paramObjects)
        {
            if (_commonModulesDic.ContainsKey(moduleName))
            {
                OpenCommonModule(moduleName, _commonParent, paramObjects);
            }

            if (_moduleParent == null)
            {
                throw new Exception("module parent null, do SetContainer");
            }

            if (isOpening) return;
            isOpening = true;
            CheckPath(moduleName);
            _modulePath.Add(moduleName);
            OpenModuleAsync(moduleName,()=> {
                if (removePrev && _currentModule != null)
                {
                    Remove(_currentModule.ModuleName);
                }
                if (!removePrev && hidePrev && _currentModule != null)
                {
                    Hide(_currentModule.ModuleName);
                }
                isOpening = false;
            }, paramObjects);

            //CheckPath(moduleName);
            //_modulePath.Add(moduleName);
            //return OpenModule(moduleName, paramObjects);
        }

        /// <summary>
        /// 跳转到其他模块
        /// </summary>
        /// <param name="data"></param>
        /// <param name="removePrev"></param>
        /// <param name="hidePrev"></param>
        /// <returns></returns>
        public void EnterModule(JumpData data, bool removePrev = false, bool hidePrev = true)
        {
             EnterModule(data.Module, removePrev, hidePrev, data);
        }
        
        private void RemoveModulePath(string moduleName)
        {
            var index = _modulePath.IndexOf(moduleName);
            if (index != -1)
            {
                _modulePath.RemoveAt(index);
            }
        }
        private void CheckPath(string moduleName)
        {
            try
            {
                var index = _modulePath.IndexOf(moduleName);
                if (index != -1)
                {
                    var num = _modulePath.Count - index;
                    _modulePath.RemoveRange(index, num);
                }
            }
            catch (Exception)
            {
                
            }
           
        }

        public IModule OpenCommonModule(string moduleName, GameObject parent, params object[] paramObjects)
        {
            RemoveAllModule();//进入通用模块，移除其他所有模块
            _modulePath.Clear();
            _currentModule = null;

            if (parent == null)
            {
                if (_commonParent != null)
                {
                    parent = _commonParent;
                }
                else
                {
                    throw new Exception("no common parent");
                }
            }
            if (_currentCommonModule != null && _currentCommonModule.ModuleName != moduleName)
            {
                _currentCommonModule.OnHide();
            }

            IModule module;
            //模块不创建多个实例
            if (_commonModulesDic.ContainsKey(moduleName))
            {
                Debug.Log("SHOW COMMON MODULE " + moduleName);
                module = _commonModulesDic[moduleName];
                module.SetData(paramObjects);
                module.LoadAssets();
                module.OnShow(0);
                _currentCommonModule = module;
                return module;
            }
            Debug.Log("OPEN COMMON MODULE " + moduleName);
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
            object obj = assembly.CreateInstance(moduleName + "Module"); // 
            module = (IModule)obj;
            if (module != null)
            {
                module.ModuleName = moduleName;
                module.Parent = parent;
                module.SetData(paramObjects);
                module.LoadAssets();
                module.Init();

                SdkHelper.StatisticsAgent.OnEvent(moduleName);
                
                _currentCommonModule = module;
                _commonModulesDic.Add(moduleName, module);
                _commonParent = parent;
            }
            else
            {
                throw new Exception("module init fail");
            }
            return module;
        }



        public IModule OpenModule(string moduleName, params object[] paramObjects)
        {
            if (_commonModulesDic.ContainsKey(moduleName))
            {
                throw new Exception("Can not open a CommonModule, use OpenCommonModule");
            }
            IModule module;
            //模块不创建多个实例
            if (_modulesDic.ContainsKey(moduleName))
            {
                Debug.Log("SHOW MODULE " + moduleName);
                module = _modulesDic[moduleName];
                module.SetData(paramObjects);
                module.LoadAssets();
                module.OnShow(0);
                _currentModule = module;
                return module;
            }
            Debug.Log("OPEN MODULE " + moduleName);
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
            object obj = assembly.CreateInstance(moduleName + "Module"); // 
            module = (IModule)obj;
            if (module != null)
            {
                module.ModuleName = moduleName;
                module.Parent = _moduleParent;
                module.SetData(paramObjects);
                module.LoadAssets();
                module.Init();
                
                SdkHelper.StatisticsAgent.OnEvent(moduleName);
                
                _currentModule = module;
                _modulesDic.Add(moduleName, module);
            }
            else
            {
                throw new Exception("module init fail");
            }
            return module;
        }

        public void OpenModuleAsync(string moduleName, Action finish, params object[] paramObjects)
        {
            if (_commonModulesDic.ContainsKey(moduleName))
            {
                throw new Exception("Can not open a CommonModule, use OpenCommonModule");
            }
            IModule module;
            //模块不创建多个实例
            if (_modulesDic.ContainsKey(moduleName))
            {
                Debug.Log("SHOW MODULE " + moduleName);
                module = _modulesDic[moduleName];
                module.SetData(paramObjects);

                        finish?.Invoke();
                        module.OnShow(0);
                        _currentModule = module;
                   
                return;
            }
            Debug.Log("OPEN MODULE " + moduleName);
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
            object obj = assembly.CreateInstance(moduleName + "Module"); // 
            module = (IModule)obj;
            if (module != null)
            {
                module.ModuleName = moduleName;
                module.Parent = _moduleParent;
                module.SetData(paramObjects);
                module.LoadAssetsAsync(() =>
                {
                    finish?.Invoke();
                    module.Init();

                    SdkHelper.StatisticsAgent.OnEvent(moduleName);

                    _currentModule = module;
                    _modulesDic.Add(moduleName, module);

                    
                });
            }
            else
            {
                throw new Exception("module init fail");
            }
        }

        public void Remove(string moduleName)
        {
            if (_modulesDic.ContainsKey(moduleName))
            {
                IModule module = _modulesDic[moduleName];
                module.Remove(0);
                _modulesDic.Remove(moduleName);
                RemoveModulePath(moduleName);
                Debug.Log("ModuleManager Remove:" + moduleName);
            }
            else
            {
                if (_commonModulesDic.ContainsKey(moduleName))
                {
                    Debug.LogWarning("Common Module Can't be removed");
                }
                else
                {
                    Debug.LogWarning("module no found: " + moduleName);
                }
            }
        }

        public void Hide(string moduleName)
        {
            if (_modulesDic.ContainsKey(moduleName))
            {
                IModule module = _modulesDic[moduleName];
                module.OnHide();
                Debug.Log("ModuleManager Hide:" + moduleName);
            }
            else if(_commonModulesDic.ContainsKey(moduleName))
            {
                IModule module = _commonModulesDic[moduleName];
                module.OnHide();
            }
        }
        public void Show(string moduleName)
        {
            if (_modulesDic.ContainsKey(moduleName))
            {
                IModule module = _modulesDic[moduleName];
                module.OnShow(0);
            }
            else if(_commonModulesDic.ContainsKey(moduleName))
            {
                IModule module = _commonModulesDic[moduleName];
                module.OnShow(0);
            }
            else
            {
                throw new Exception("module no found: " + moduleName);
            }
        }

        public void GoBack()
        {
            if (_modulePath.Count > 1)
            {
                var curModuleName = _modulePath[_modulePath.Count - 1];
                var prevModuleName = _modulePath[_modulePath.Count - 2];
                _modulePath.RemoveAt(_modulePath.Count - 1);
                Remove(curModuleName);
                OpenModuleAsync(prevModuleName,null);
            }
            else
            {
                //GoHome();
                RemoveAllModule();
                _modulePath.Clear();
                _currentModule = null;
                if (_currentCommonModule != null)
                {
                    _currentCommonModule.OnShow(0);
                }
            }
        }

        public bool CnaGoBack()
        {
            return _modulePath.Count > 0;
        }
        

        /// <summary>
        /// 销毁除CommonModule以外所有Module
        /// </summary>
        private void RemoveAllModule()
        {
            while (_modulesDic.Keys.Count > 0)
            {
                var moduleName = _modulesDic.Keys.ElementAt(0);
                Remove(moduleName);
            }
        }

        /// <summary>
        /// 销毁所有module，包括CommonModule
        /// </summary>
        public void DestroyAllModule()
        {
            foreach (var module in _modulesDic)
            {
                module.Value.Remove(0);
            }

            foreach (var module in _commonModulesDic)
            {
                module.Value.Remove(0);
            }
            _modulesDic.Clear();
            _commonModulesDic.Clear();
            _modulePath.Clear();
        }

        /// <summary>
        /// 销毁所有module，除了CommonModule
        /// </summary>
        public void DestroyAllModuleBackToCommon()
        {
            RemoveAllModule();
            _modulePath.Clear();
            _currentModule = null;
            if (_currentCommonModule != null)
            {
                _currentCommonModule.OnShow(0);
            }
        }

        public void SendGlobalMessage(string msg)
        {
            foreach (KeyValuePair<string, IModule> module in _modulesDic)
            {
                module.Value.SendMessage(new Message("global", Message.MessageReciverType.DEFAULT, msg));
            }
        }

        public bool HasModule(string moduleName)
        {
            if (_modulesDic.ContainsKey(moduleName))
                return true;

            if (_commonModulesDic.ContainsKey(moduleName))
                return true;

            return false;
        }

        public int ModuleCount => _modulesDic.Count;

        public string CurrentModule
        {
            get
            {
                if (_currentModule != null)
                {
                    return _currentModule.ModuleName;
                }
                return "NONE";
            }
        }
    }
}