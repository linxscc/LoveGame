using Common;
using DataModel;
using game.main;

public class RandomEventManager
{
    public static void ShowNewTriggerGift()
    {
        int index = GlobalData.RandomEventModel.GetNewGiftIndex();
        if(index == -1)
            return;
        
        RandowGiftWindow win =
            PopupManager.ShowWindow<RandowGiftWindow>("GameMain/Prefabs/RandomGift/RandowGiftWindow");
        win.SetData(GlobalData.RandomEventModel.GiftList, index);
    }
    
    public static void ShowGiftWindow(int index = 0)
    {
        GlobalData.RandomEventModel.FilterTimeOut();
        if(GlobalData.RandomEventModel.GiftList.Count == 0)
            return;
        
        RandowGiftWindow win =
            PopupManager.ShowWindow<RandowGiftWindow>("GameMain/Prefabs/RandomGift/RandowGiftWindow");
        win.SetData(GlobalData.RandomEventModel.GiftList, index);
    }
}