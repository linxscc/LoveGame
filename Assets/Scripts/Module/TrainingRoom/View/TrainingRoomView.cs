using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using DataModel;
using game.main.Live2d;
using UnityEngine;
using UnityEngine.UI;

public class TrainingRoomView : View
{

	private RawImage _bgImage;
	private Transform _roleContent;
	private Transform _btnContent;
	private Transform _songContent;
	private Live2dGraphic _live2dGraphic;

	private void Awake()
	{
		_bgImage = transform.GetRawImage("Bg");
		_roleContent = transform.Find("RoleContent");
		_btnContent = transform.Find("BtnContent");
		_songContent = transform.Find("SongContent");
			
		_btnContent.GetButton("PlayBtn").onClick.AddListener(PlayBtn);
		_btnContent.GetButton("RankingBtn").onClick.AddListener(RankingBtn);
		_btnContent.GetButton("ExchangeShopBtn").onClick.AddListener(ExchangeShopBtn);

		_live2dGraphic = transform.Find("CharacterContainer/Live2dGraphic").GetComponent<Live2dGraphic>();
		
		if ((float) Screen.height / Screen.width > 1.8f)
		{
			_live2dGraphic.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
		}
	}

	private void PlayBtn()
	{		
		SendMessage(new Message(MessageConst.MODULE_TRAININGROOM_GOTO_PLAY_PANEL));
	}
	
	private void RankingBtn()
	{
		SendMessage(new Message(MessageConst.MODULE_TRAININGROOM_GOTO_RANKING_PANEL));
	}
	
	private void ExchangeShopBtn()
	{
		SendMessage(new Message(MessageConst.MODULE_TRAININGROOM_GOTO_SHOP_PANEL));
	}

	public void SetData(MusicInfoPB todaySongInfo,MusicInfoPB tomorrowSongInfo)
	{				
      _bgImage.texture = ResourceManager.Load<Texture>("TrainingRoom/background/"+todaySongInfo.MusicId);

      _songContent.GetRawImage("SongBg/SongCDBg/SongImage").texture = ResourceManager.Load<Texture>("TrainingRoom/cover1/"+todaySongInfo.MusicId);
      
      _songContent.GetText("SongBg/TomorrowSongName").text = "明日应援曲目：" +tomorrowSongInfo.MusicName;

      int id = int.Parse(todaySongInfo.MusicId.ToString()[0].ToString());
      if (id < 5)
      {
	      string[] live2dIds = {"12101", "12201", "12301", "12401"};
	      _live2dGraphic.LoadAnimationById(live2dIds[id-1]);
      }
      else
      {
	      _live2dGraphic.Hide();
      }
	}
	
}
