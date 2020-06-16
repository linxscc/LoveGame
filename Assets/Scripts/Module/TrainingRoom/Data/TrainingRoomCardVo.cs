using System.Collections;
using System.Collections.Generic;
using DataModel;
using game.main;
using UnityEngine;

public class TrainingRoomCardVo
{
	public bool IsChoose;
	public UserCardVo UserCardVo;
	public int AbilityNum;
	public string AbilityDesc;
	
	public TrainingRoomCardVo(UserCardVo vo)
	{
		IsChoose = false;
		UserCardVo = vo;
		AbilityNum = GetAbilityNum();
		AbilityDesc = GlobalData.TrainingRoomModel.GetAbility(GlobalData.TrainingRoomModel.CurMusicGame.NeedAbility);
	}
	
	private int GetAbilityNum()
	{
		int temp = 0;
		var curActivityType =  GlobalData.TrainingRoomModel.CurMusicGame.NeedAbility;
		AbilityPB pb = (AbilityPB)curActivityType;
		switch (pb)
		{
			case AbilityPB.Singing:
				temp = UserCardVo.Singing;
				break;
			case AbilityPB.Dancing:
				temp = UserCardVo.Dancing;
				break;
			case AbilityPB.Composing:
				temp = UserCardVo.Original;
				break;
			case AbilityPB.Popularity:
				temp = UserCardVo.Popularity;
				break;
			case AbilityPB.Charm:
				temp = UserCardVo.Glamour;
				break;
			case AbilityPB.Perseverance:
				temp = UserCardVo.Willpower;
				break;
			
		}
		return temp;
	}
}
