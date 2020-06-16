using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using DataModel;

public class ChooseCardController : Controller
{
   public ChooseCardView View;

   public override void Init()
   {
       EventDispatcher.AddEventListener<TrainingRoomCardVo>(EventConst.OkChooseCard,OkChooseCard);
       EventDispatcher.AddEventListener<TrainingRoomCardVo>(EventConst.CancelChooseCard,CancelChooseCard);
   }

   private void CancelChooseCard(TrainingRoomCardVo vo)
   {
       GlobalData.TrainingRoomModel.CancelCard(vo);
       View.SetChooseBtnTxt(GlobalData.TrainingRoomModel.ChooseCards.Count);
   }

   private void OkChooseCard(TrainingRoomCardVo vo)
   {
       GlobalData.TrainingRoomModel.AddCard(vo);  
       View.SetChooseBtnTxt(GlobalData.TrainingRoomModel.ChooseCards.Count);
   }

   public override void Start()
   {
       GlobalData.TrainingRoomModel.InitChooseCards();
       GlobalData.TrainingRoomModel.InitTrainingRoomCardList(GlobalData.CardModel.UserCardList);         
      
       View.SetData(GlobalData.TrainingRoomModel.CurMusicGame );
       View.SetMyCardData(GlobalData.TrainingRoomModel.GetTrainingRoomCards());
       View.SetChooseBtnTxt(GlobalData.TrainingRoomModel.ChooseCards.Count);
   }


   public override void Destroy()
   {
       EventDispatcher.RemoveEvent(EventConst.OkChooseCard);
       EventDispatcher.RemoveEvent(EventConst.CancelChooseCard);
   }
}
