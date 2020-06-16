using Assets.Scripts.Common;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Interfaces;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Componets;
using DataModel;

public class CardListPanel : ReturnablePanel {
	private CardListView _view;

	public override void Init(IModule module)
	{
	    base.Init(module);

		_view = (CardListView)InstantiateView<CardListView>("Recollection/Prefabs/CardListView");
		RegisterView(_view);
		NetWorkManager.Instance.Send<DrawProbRes>(CMD.DRAWC_DRAW_PROBS, null, OnGetCardTotalNum, null, true, GlobalData.VersionData.VersionDic[CMD.DRAWC_DRAW_PROBS]);
		
	}

	private void OnGetCardTotalNum(DrawProbRes res)
	{
		LoadingOverlay.Instance.Hide();
		DrawData drawData = GetData<DrawData>();
		drawData.InitData(res);
		_view.SetTotalNum(drawData.GetTotalNum(DrawEventPB.GemBase));
	}

	public override void OnBackClick()
	{
		SendMessage(new Message(MessageConst.COMMON_BACK));
	}

	public override void Hide()
    {
	    _view.gameObject.Hide();
    }

    public override void Show(float delay)
    {
	    ShowBackBtn();
	    
	    _view.gameObject.Show();
	    _view.SetMyCardData(GlobalData.CardModel.UserCardList);
    }

}
