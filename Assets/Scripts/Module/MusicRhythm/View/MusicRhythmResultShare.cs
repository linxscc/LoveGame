using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Module.MusicRhythm.View
{
    public class MusicRhythmResultShare : MonoBehaviour
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
        private Text _nameText;
        private Text _levelText;
        private RawImage _headIcon;
        private RawImage _headFrame;
        private RawImage _cover;
        private RawImage _bg;

        private void Awake()
        {
            _bg = transform.GetRawImage("Panel");
            
            _name = transform.GetText("MusicImage/Name");
            _level = transform.GetText("MusicImage/LevelBg/Level");
            _score = transform.GetText("Score/Num");
            _combo = transform.GetText("MaxCombo/Num");

            _cover = transform.GetRawImage("MusicImage");
            
            _perfect = transform.GetText("Total/1");
            _good = transform.GetText("Total/2");
            _bad = transform.GetText("Total/3");
            _miss = transform.GetText("Total/4");

            _headIcon = transform.GetRawImage("BottomBg/ScoreBg/HeadIcon/Head");
            _nameText = transform.GetText("BottomBg/ScoreBg/HeadIcon/Name/Text");
            _levelText = transform.GetText("BottomBg/ScoreBg/HeadIcon/Level/Text");
            _headFrame = transform.GetRawImage("BottomBg/ScoreBg/HeadIcon/Frame");
        }

        public void SetData(MusicRhythmRunningInfo info)
        {
            _name.text = info.musicName;
            _level.text = info.diffName;
            _score.text = info.Socre.ToString();

            _combo.text = info.MaxCombo.ToString();
            
            _bg.texture = ResourceManager.Load<Texture>("TrainingRoom/background/" + info.musicId,
                ModuleConfig.MODULE_MUSICRHYTHM);

            _perfect.text =I18NManager.Get("MusicRhythm_Perfect", info.Perfect);
            _good.text = I18NManager.Get("MusicRhythm_Good", info.Good);
            _bad.text = I18NManager.Get("MusicRhythm_Bad", info.Bad);
            _miss.text = I18NManager.Get("MusicRhythm_Miss", info.Miss);

            string iconPath = "UIAtlas_MusicRhythm_Level" + info.gameScoreLevel.ToString();
            Image level = transform.GetImage("BottomBg/ScoreBg/Image");
            level.sprite= AssetManager.Instance.GetSpriteAtlas(iconPath);
            level.SetNativeSize();

            
            _cover.texture = ResourceManager.Load<Texture>("TrainingRoom/cover1/" + info.musicId, ModuleConfig.MODULE_MUSICRHYTHM);
            
            var playerVo = GlobalData.PlayerModel.PlayerVo;
            _nameText.text = playerVo.UserName;
            _levelText.text = "Lv." + playerVo.Level;
            
            var userOther = GlobalData.PlayerModel.PlayerVo.UserOther;
            _headIcon.texture = ResourceManager.Load<Texture>(GlobalData.DiaryElementModel.GetHeadPath(userOther.Avatar, ElementTypePB.Avatar));
            _headFrame.texture = ResourceManager.Load<Texture>(GlobalData.DiaryElementModel.GetHeadPath(userOther.AvatarBox,ElementTypePB.AvatarBox));
        }
    }
}