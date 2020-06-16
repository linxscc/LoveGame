using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SortResouce//升序 一级：卡牌>粉丝>碎片
{
    Card,
    Fans,
    Puzzle,
}
public enum SortCredit//升序 一级：卡牌>粉丝>碎片
{
    SSR,
    SR,
    R,
    NO,
}
public class ShowCardModel  {


    public int CardId;

	//public string CardLevel;

    //public CreditPB Credit;

    public bool IsNew;

	//public string CardRole;
    public PlayerPB Player;
	public string  CardName;
    public DrawEventPB DrawEvent;
    public SortResouce Resource;
    public SortCredit Credit;
    /// <summary>
    /// 通过粉丝生成
    /// </summary>
    public ShowCardModel(int cardId, SortResouce resource, SortCredit credit, DrawEventPB drawEvent,bool isNew, string cardName)
    {
        CardId = cardId;
        Resource = resource;
        DrawEvent = drawEvent;
        Credit = credit;
        IsNew = isNew;
        CardName = cardName;
    }

    /// <summary>
    /// 通过卡生成
    /// </summary>
	public ShowCardModel(int cardId, SortResouce resource, SortCredit credit, DrawEventPB drawEvent, PlayerPB player,bool isNew,string cardName )
	{
		CardId = cardId;
        Resource = resource;
        DrawEvent = drawEvent;
        Credit = credit;
        Player = player;
        IsNew = isNew;		
		CardName = cardName;
    }
	
	////根据类型来返回一个list用来控制button按钮
	//public static List<ShowCardModel> GetListByRole(PlayerPB pb,List<ShowCardModel> list)
	//{
 //       if (pb == PlayerPB.None)
 //           return list;
        
	//	List<ShowCardModel> showCardList=new List<ShowCardModel>();

	//	for (int i = 0; i < list.Count; i++)
	//	{
 //           if (list[i].Player == pb)
 //           {
 //               //Debug.Log("list[i].Player "+ list[i].Player);
 //               showCardList.Add(list[i]);
 //           }
	//	}
	//	return showCardList;
	//}

    public string BigCardPath
    {
        get { return "Card/Image/" + CardId; }
    }

    public string MiddleCardPath
    {
        get { return "Card/Image/MiddleCard/" + CardId; }
    }

    public string SmallCardPath
    {
        get { return "Card/Image/SmallCard/" + CardId; }
    }

    public string SmallCardFunPath
    {
        get { return "FansTexture/Head/" + CardId; }
    }

}
