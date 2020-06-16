using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using DataModel;
using Module.Card.Controller;

namespace game.main
{
    public class CardCollectionPanel : ReturnablePanel
    {
        private CardCollectionView _cardCollectionView;
        private CardCollectionController _cardCollectionController;
        
        private CardPuzzleView _cardPuzzleView;
        private CardResolveView _cardResolveView;
        
        private CardPuzzleController _cardPuzzleController;
        private CardResolveController _cardResolveController;
        
        private CardCollectionView _currentView;
        private PlayerPB _currentTab = PlayerPB.None;

        public override void Init(IModule module)
        {
            SetComplexPanel();
            base.Init(module);

            _cardCollectionView = (CardCollectionView) InstantiateView<CardCollectionView>("Card/Prefabs/Collection/CardCollectionView");
            _cardCollectionController = new CardCollectionController();
            RegisterController(_cardCollectionController);
            _cardCollectionController.View = _cardCollectionView;
            
            _cardPuzzleView = (CardPuzzleView)InstantiateView<CardPuzzleView>("Card/Prefabs/Puzzle/CardPuzzleView");
            _cardPuzzleController = new CardPuzzleController();
            _cardPuzzleController.View = _cardPuzzleView;
            RegisterController(_cardPuzzleController);
            
            _cardResolveView = (CardResolveView)InstantiateView<CardResolveView>("Card/Prefabs/Resolve/CardResolveView");
            _cardResolveController = new CardResolveController();
            _cardResolveController.View = _cardResolveView;
            RegisterController(_cardResolveController);
            
            _cardPuzzleView.gameObject.Hide();
            _cardResolveView.gameObject.Hide();
            
            _cardCollectionController.Start();

            
            _currentView = _cardCollectionView;
        }

        public override void Show(float delay)
        {
            base.Show(0);
            Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
            ShowBackBtn();
        }

        public void ChangeView(CardModule.CardViewState state)
        {
            _cardCollectionView.ChangeViewStae(state);
            _cardPuzzleView.gameObject.SetActive(state == CardModule.CardViewState.Puzzle);
            _cardResolveView.gameObject.SetActive(state == CardModule.CardViewState.Resolve);

            if (state==CardModule.CardViewState.MyCard)
            {
                _cardCollectionView.SetMyCardData(GlobalData.CardModel.UserCardList, GlobalData.CardModel.CurPlayerPb);
            }
         
            if(state == CardModule.CardViewState.Resolve)
                _cardResolveView.SetData(GlobalData.CardModel.ResolveCardList, GlobalData.CardModel.CurPlayerPb);

            if (state == CardModule.CardViewState.Puzzle)
            {
                _cardPuzzleView.ShowView(GlobalData.CardModel.CurPlayerPb);
                if (GlobalData.CardModel.CardPuzzleList!=null)
                {
                    GlobalData.CardModel.CardPuzzleList.Sort();
                    _cardPuzzleView.SetData(GlobalData.CardModel.CardPuzzleList);
                }
            }

        }
        
        public void OnShow()
        {
            ShowBackBtn();
            Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        }

        public void SetResolveState()
        {
            _cardCollectionView.JumpToResolveState(); 
        }
        
        public void ChangeTabBar(PlayerPB pb,bool needtorefill=true)
        {
            //_currentTab = pb;
            _cardCollectionView.ChangeTabBar(pb,needtorefill);
            _cardPuzzleView.ChangeTabBar(pb);
            _cardResolveView.ChangeTabBar(pb);
            
        }
    }
}