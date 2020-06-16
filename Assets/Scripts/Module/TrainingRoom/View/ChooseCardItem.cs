using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;
public class ChooseCardItem : MonoBehaviour {

	private Image _cardQualityImage;
	private Text _name;
	private Transform _heartBar;
	private RawImage _cardImage;
	private Text _level;
	private Text _ability;


	private void Awake()
	{
		_cardQualityImage = transform.Find("CardQualityImage").GetComponent<Image>();
		_name = transform.Find("NameText").GetComponent<Text>();
		_heartBar = transform.Find("QualityBg/HeartBar");
		_cardImage =transform.Find("Mask/CardImage").GetComponent<RawImage>();
		_level = transform.Find("QualityBg/LevelText").GetComponent<Text>();
		_ability =transform.Find("Ability").GetComponent<Text>();
	}


	public void SetData(TrainingRoomCardVo vo)
	{
		_cardQualityImage.sprite = AssetManager.Instance.GetSpriteAtlas(CardUtil.GetNewCreditSpritePath(vo.UserCardVo.CardVo.Credit));
		_name.text = vo.UserCardVo.CardVo.CardName;
		
		for (int i = 0; i < 5; i++)
		{
			Transform item = _heartBar.GetChild(i);
			var redHeart = item.Find("RedHeart");
			redHeart.gameObject.SetActive(vo.UserCardVo.Star > i);
			item.gameObject.SetActive(i < vo.UserCardVo.MaxStars);
		}
		
		Texture texture = ResourceManager.Load<Texture>(vo.UserCardVo.CardVo.MiddleCardPath(vo.UserCardVo.UserNeedShowEvoCard()), ModuleConfig.MODULE_CARD);
		if (texture ==null)
		{
			texture = ResourceManager.Load<Texture>(vo.UserCardVo.CardVo.MiddleCardPath(),ModuleConfig.MODULE_CARD);
		}
		_cardImage.texture = texture;
		_level.text = vo.UserCardVo.Level.ToString();
		
		_ability.text = vo.AbilityDesc + ":" + vo.AbilityNum;
	}
}
