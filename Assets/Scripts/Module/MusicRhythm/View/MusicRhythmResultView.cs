using System;
using Assets.Scripts.Framework.GalaSports.Core;
using game.main;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using GalaAccount.Scripts.Framework.Utils;
using UnityEngine;
using UnityEngine.UI;
public class MusicRhythmResultView : View
{
    Text _score;
    Text _combo;
    Text _perfect;
    Text _good;
    Text _bad;
    Text _miss;
    Button _restart;

    Text _name;
    Text _level;
    private Button _shareBtn;
    private MusicRhythmRunningInfo _data;
    private RawImage _cover;
    private RawImage _bg;

    private void Awake()
    {
        _name = transform.GetText("MusicImage/Name");
        _level = transform.GetText("MusicImage/LevelBg/Level");
        _score = transform.GetText("Score/Num");
        _combo = transform.GetText("MaxCombo/Num");

        _cover = transform.GetRawImage("MusicImage");
        
        _perfect = transform.GetText("Total/1");
        _good = transform.GetText("Total/2");
        _bad = transform.GetText("Total/3");
        _miss = transform.GetText("Total/4");
        _restart = transform.GetButton("Restart");

        _bg = transform.GetRawImage("Panel");

        _shareBtn = transform.GetButton("ShareBtn");

        if (true)
        {
            _shareBtn.onClick.AddListener(OnShareClick);
        }
        else
        {
            _shareBtn.gameObject.Hide();
        }
    }

    private void OnShareClick()
    {
        SdkHelper.ShareAgent.ShareMusicGameResult(_data);
    }

    public void SetData(MusicRhythmRunningInfo info, MusicGameType musicGameType)
    {
        _data = info;

        _bg.texture = ResourceManager.Load<Texture>("TrainingRoom/background/" + info.musicId,
            ModuleConfig.MODULE_MUSICRHYTHM);
            
        _name.text = info.musicName;
        _level.text = info.diffName;
        _score.text = info.Socre.ToString();

        _combo.text = info.MaxCombo.ToString();

        _perfect.text =I18NManager.Get("MusicRhythm_Perfect", info.Perfect);
        _good.text = I18NManager.Get("MusicRhythm_Good", info.Good);
        _bad.text = I18NManager.Get("MusicRhythm_Bad", info.Bad);
        _miss.text = I18NManager.Get("MusicRhythm_Miss", info.Miss);

        string iconPath = "UIAtlas_MusicRhythm_Level" + info.gameScoreLevel.ToString();
        Image level = transform.GetImage("BottomBg/ScoreBg/Image");
        level.sprite= AssetManager.Instance.GetSpriteAtlas(iconPath);
        level.SetNativeSize();

        _cover.texture = ResourceManager.Load<Texture>("TrainingRoom/cover1/" + info.musicId, ModuleConfig.MODULE_MUSICRHYTHM);
        
        switch (musicGameType)
        {
            case MusicGameType.Activity:
            case MusicGameType.TrainingRoomPractice:
                _restart.onClick.AddListener(() =>
                {
                    SendMessage(new Message(MessageConst.MODULE_MUSICRHYRHM_RESTARTGAME));
                });
                break;
            
            case MusicGameType.TrainingRoom:
                transform.Find("BottomBg/ScoreBg/Reward").gameObject.Show();
                
                _restart.transform.GetText("Text").text = "继续";
                _restart.onClick.AddListener(() =>
                {
                    SendMessage(new Message(MessageConst.MODULE_MUSICRHYRHM_BACK));
                });
                break;
        }
    }

    public void ShowReward(long num)
    {
        transform.GetText("BottomBg/ScoreBg/Reward/Frame/Text").text = "x" + num;
    }
}
