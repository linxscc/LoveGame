using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using game.main;
using UnityEngine;

namespace Common
{
    public class AudioManager : MonoBehaviour
    {
        public enum AudioTypes
        {
            Bgm,
            Dubbing,
            Effect
        }


        const string PATH = "module/Audio/Prefabs/AudioManager";

        private static AudioManager _instance;

        public static AudioManager Instance => _instance;

        public AudioSource DubbingAudioSource => _dubbingAudioSource;

        public AudioSource BackgroundAudioSource
        {
            get { return _backgroundAudioSource; }
            set { _backgroundAudioSource = value; }
        }
        
        private AudioSource _backgroundAudioSource;
        private AudioSource _dubbingAudioSource;
       // private AudioSource _effect1AudioSource;
      //  private AudioSource _effect2AudioSource;
        private List<AudioSource> _effectAudioSources;

        /// <summary>
        /// 对话是否正在播放
        /// </summary>
        public bool IsPlayingDubbing => _dubbingAudioSource.isPlaying;


        public float BgMusicVolum ;
        public float DubbingVolum ;
        public float EffectVolume ;



        public AudioClip DefaultBgMusicClip;
        public AudioClip DefaultButtonEffect;

        public const string DefaultBgMusicName = "Quartet 192";
        public const string DefaultButtonEffectName = "icon_enter";
        public const string DefaultBackButtonEffectName = "icon_out";

        private float _defaultBgMusicTime = 0;
        private TweenerCore<float, float, FloatOptions> _volumTween;


        public void PlayDefaultBgMusic()
        {
            PlayBackgroundMusic(DefaultBgMusicClip, _defaultBgMusicTime);
            TweenVolumTo(BgMusicVolum, 3, 0.2f);
        }

        public static void Initialize()
        {
            if (_instance == null)
                Instantiate(ResourceManager.Load<GameObject>(PATH));
        }

        private void Awake()
        {
            _instance = this;

            _backgroundAudioSource = transform.Find("BackgroundMusic").GetComponent<AudioSource>();
            _dubbingAudioSource = transform.Find("Dubbing").GetComponent<AudioSource>();

            //todo特效以后可修改 扩展
            var _effect1AudioSource = transform.Find("Effect1").GetComponent<AudioSource>();
            var _effect2AudioSource = transform.Find("Effect2").GetComponent<AudioSource>();
            var _effect3AudioSource = transform.Find("Effect3").GetComponent<AudioSource>();
            var _effect4AudioSource = transform.Find("Effect4").GetComponent<AudioSource>();
            _effectAudioSources = new List<AudioSource>
            {
                _effect1AudioSource,
                _effect2AudioSource,
                _effect3AudioSource,
                _effect4AudioSource
            };

            InitAudioSize();
           

            new AssetLoader().LoadAudio(AssetLoader.GetBackgrounMusicById(DefaultBgMusicName), (clip, loader) =>
            {
                DefaultBgMusicClip = clip;
                PlayDefaultBgMusic();
            });
            new AssetLoader().LoadAudio(AssetLoader.GetSoundEffectById(DefaultButtonEffectName),
                (clip, loader) => { DefaultButtonEffect = clip; });
            
           
          
        }

        public void PlayEffect(string soundName, float volume = -1,bool enableloop=false,int patch=1)
        {

            if (EffectVolume==0)
            {
               return; 
            }
            new AssetLoader().LoadAudio(AssetLoader.GetSoundEffectById(soundName), (clip, loader) =>
            {
                PlayEffect(clip, volume,enableloop,patch);
            });
        }
        public void StopEffect()
        {
            for(int i=0;i< _effectAudioSources.Count;i++)
            {
                _effectAudioSources[i].Stop();
            }
        }
        public void StopEffectByClipName(string name)
        {
            for (int i = 0; i < _effectAudioSources.Count; i++)
            {
                if (_effectAudioSources[i].clip == null)
                    continue;
                if(_effectAudioSources[i].clip.name==name)
                {
                    _effectAudioSources[i].Stop();
                }
          
            }
        }

        private AudioSource GetFreeEffectAudioSource()
        {
            for (int i = 0; i < _effectAudioSources.Count; i++)
            {
                if (_effectAudioSources[i].isPlaying)
                    continue;
                return _effectAudioSources[i];
            }
            return null;
        }

        public void PlayEffect(AudioClip clip, float volume = -1,bool enableloop=false,int pitch=1)
        {
            if (EffectVolume==0)
            {
              return;  
            }
            
            AudioSource effect1AudioSource= GetFreeEffectAudioSource();
            if (effect1AudioSource==null)
            {
                    return;
            }
            
            if (effect1AudioSource.clip != null)
            {
                effect1AudioSource.clip.UnloadAudioData();
            }

            effect1AudioSource.clip = clip;
            effect1AudioSource.Play();
            if (volume >= 0)
            {
                effect1AudioSource.volume = volume;
            }
            else
            {
                effect1AudioSource.volume = EffectVolume;
            }
            //可以直接放大倍率！
            effect1AudioSource.pitch = pitch;
            //Debug.LogError(effect1AudioSource.pitch);
            effect1AudioSource.loop = enableloop;
        }

        public void PlayBackgroundMusic(AudioClip clip, float time = 0)
        {
            if (clip != DefaultBgMusicClip)
            {
                _defaultBgMusicTime = _backgroundAudioSource.time;
                Debug.Log("背景音乐当前播放时间：" + _defaultBgMusicTime);
            }

            _backgroundAudioSource.clip = clip;
            _backgroundAudioSource.Play();
        
            _backgroundAudioSource.volume = BgMusicVolum;
           
            _backgroundAudioSource.loop = true;

            if (time >= 0 && time < clip.length)
            {
                _backgroundAudioSource.time = time;
            }
        }

        public void PlayDubbing(AudioClip clip)
        {
            if (_dubbingAudioSource.clip != null)
            {
                _dubbingAudioSource.clip.UnloadAudioData();
            }

            _dubbingAudioSource.clip = clip;
            _dubbingAudioSource.Play();
           
            _dubbingAudioSource.volume = DubbingVolum;
            _dubbingAudioSource.loop = false;
        }

        public void StopBackgroundMusic()
        {
            _backgroundAudioSource.Stop();
        }
        public void PauseBackgroundMusic()
        {
            _backgroundAudioSource.Pause();
        }
        public void PlayBackgroundMusic()
        {
            _backgroundAudioSource.Play();
        }

        public void StopDubbing()
        {
            if (_dubbingAudioSource.clip)
            {
                _dubbingAudioSource.clip.UnloadAudioData();
                _dubbingAudioSource.clip = null;
            }

            _dubbingAudioSource.Stop();
        }

        /// <summary>
        /// 渐变背景音乐
        /// </summary>
        /// <param name="toVolum"></param>
        /// <param name="duration"></param>
        /// <param name="startVolum"></param>
        public void TweenVolumTo(float toVolum, float duration, float startVolum = -1)
        {
          
            if (_backgroundAudioSource.volume==0)
            {
                     return;
            }
            else
            {
                toVolum = BgMusicVolum;
            }
            
            if (_volumTween != null)
                DOTween.Kill(_volumTween);

            if (startVolum > -1)
                _backgroundAudioSource.volume = startVolum;

           
            _volumTween = DOTween.To(() => _backgroundAudioSource.volume, (v) => { _backgroundAudioSource.volume = v; },
                toVolum, duration);
        }

        public void SetAudioSize(AudioTypes type,float volume)
        {
            switch (type)
            {
                case AudioTypes.Bgm:
                    BgMusicVolum = volume;
                    _backgroundAudioSource.volume = volume;
                    break;
                case AudioTypes.Dubbing:
                    DubbingVolum = volume;
                    _dubbingAudioSource.volume = volume;
                    break;
                case AudioTypes.Effect:
                    EffectVolume = volume;
                    for (int i = 0; i < _effectAudioSources.Count; i++)
                    {
                        _effectAudioSources[i].volume = volume;
                    }                  
                    break;
               
            }
        }

        private void InitAudioSize()
        {
            if (!PlayerPrefs.HasKey("SetAudioSize"))
            {                                    
                BgMusicVolum = 1.0f;
                DubbingVolum = 0.9f;
                EffectVolume = 0.5f;
            }
            else
            {               
                BgMusicVolum= PlayerPrefs.GetFloat("BgMusicVolum");
                DubbingVolum = PlayerPrefs.GetFloat("DubbingVolum");
                EffectVolume =  PlayerPrefs.GetFloat("EffectVolume");
                for (int i = 0; i < _effectAudioSources.Count; i++)
                {
                   
                    _effectAudioSources[i].volume = EffectVolume; 
                }
            }
        }
    }
}