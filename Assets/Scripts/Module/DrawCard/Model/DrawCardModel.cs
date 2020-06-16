using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DrawCardModel {

    public string CardId;
    public string CardLevel;
    public bool IsNew;
    public string FlauntId;
    public string ShowId;
    public string  CardName;

    public DrawCardModel(string cardId,string cardLevel,bool isNew,string flauntId,string showId ,string cardName )
    {
        CardId = cardId;
        CardLevel = cardLevel;
        IsNew = isNew;
        FlauntId = flauntId;
        ShowId = showId;
        CardName = cardName;
    }

}
