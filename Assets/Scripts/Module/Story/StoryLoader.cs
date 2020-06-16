using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using DG.Tweening;
using UnityEngine;

namespace game.main
{
    public class StoryLoader
    {
        private int _loadCount;
        private List<DialogVo> _dialogList;
        private Dictionary<string, Texture> _bgImageCache;
        private Dictionary<string, Texture> _headImageCache;
        private Dictionary<string, Texture>  _roleImageCache;

        public Dictionary<string, Texture> BgImageCache => _bgImageCache;
        public Dictionary<string, Texture> RoleImageCache => _roleImageCache;
        public Dictionary<string, Texture> HeadImageCache => _headImageCache;

        private List<bool> _loadedList;
        private int _currentIndex;

        private Action<int> _onAssetLoaded;

        public bool IsLoading;

        public StoryLoader(List<DialogVo> dialogList, Action<int> onAssetLoaded)
        {
            _dialogList = dialogList;

            _onAssetLoaded = onAssetLoaded;
            
            _bgImageCache = new Dictionary<string, Texture>();
            _roleImageCache = new Dictionary<string, Texture>();
            _headImageCache = new Dictionary<string, Texture>();

            _loadedList = new List<bool>(_dialogList.Count);
            for (int i = 0; i < _dialogList.Count; i++)
            {
                _loadedList.Add(false);
            }
        }
        
        public void PreLoadAsset(int index, Action<int> onAssetLoaded)
        {
            if(index >= _dialogList.Count)
                return;
            
            _onAssetLoaded = onAssetLoaded;
            
            _currentIndex = index;
            
            _loadCount = 0;
            IsLoading = true;

            Texture tex = null;
            
            DialogVo vo = _dialogList[index];
            if (_bgImageCache.ContainsKey(vo.BgImageId) == false)
            {
                tex = ResourceManager.Load<Texture>(AssetLoader.GetStoryBgImage(vo.BgImageId), ModuleConfig.MODULE_STORY);
                _bgImageCache[vo.BgImageId] = tex;
            }
           
            if (vo.EntityList.Count > 0)
            {
                for (int i = 0; i < vo.EntityList.Count; i++)
                {
                    EntityVo entity = vo.EntityList[i];
                    if (entity.type == EntityVo.EntityType.Role)
                    {
                        tex = ResourceManager.Load<Texture>(AssetLoader.GetStoryRoleImageById(entity.id), ModuleConfig.MODULE_STORY);
                        _roleImageCache[entity.id] = tex;
                    }
                    else if(entity.type == EntityVo.EntityType.DialogFrame)
                    {
                        if (!string.IsNullOrEmpty(entity.headId))
                        {
                            tex = ResourceManager.Load<Texture>(AssetLoader.GetHeadImageById(entity.headId), ModuleConfig.MODULE_STORY);
                            _headImageCache[entity.headId] = tex;
                        }
                    }
                }
            }

            if (_loadCount <= 0)
            {
                LoadComplete();
            }
        }

        private void LoadComplete()
        {
            _loadCount--;
            if (_loadCount <= 0)
            {
                _loadedList[_currentIndex] = true;

                _onAssetLoaded(_currentIndex);

                _onAssetLoaded = null;

                IsLoading = false;
            }
        }

        public void Dispose()
        {
            _bgImageCache.Clear();
            _roleImageCache.Clear();

            UnloadBundles();
        }

        private void UnloadBundles()
        {
            foreach (var dialogVo in _dialogList)
            {
                if (string.IsNullOrEmpty(dialogVo.DubbingId) == false)
                {
                    AssetLoader.UnloadBundle(AssetLoader.GetDubbingById(dialogVo.DubbingId));
                }
                if (string.IsNullOrEmpty(dialogVo.BgMusicId) == false)
                {
                    AssetLoader.UnloadBundle(AssetLoader.GetBackgrounMusicById(dialogVo.BgMusicId));
                }

                foreach (var key in _bgImageCache.Keys)
                {
                    AssetManager.Instance.UnloadBundle(AssetLoader.GetStoryBgImage(key));
                }
                
                foreach (var key in _headImageCache.Keys)
                {
                    AssetManager.Instance.UnloadBundle(AssetLoader.GetHeadImageById(key));
                }
                
                foreach (var key in _roleImageCache.Keys)
                {
                    AssetManager.Instance.UnloadBundle(AssetLoader.GetStoryRoleImageById(key));
                }
            }
        }
    }
}