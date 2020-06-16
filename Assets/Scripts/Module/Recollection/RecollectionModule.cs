using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Assets.Scripts.Module.Recollection.View;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using UnityEngine;

/// <summary>
/// 星缘回忆
/// </summary>
public class RecollectionModule : ModuleBase 
{
    private RecollectionPanel _panel;
    private CardListPanel _cardListPanel;
    private RecollectionModel _model;
    private int _propId;
    private CardDropPropWindow _cardDropPropWindow;

    public override void Init()
    {
        _model = RegisterModel<RecollectionModel>();
        
        _panel = new RecollectionPanel();
        _panel.Init(this);
        _panel.Show(0);

        GuideManager.RegisterModule(this);
    }

    public override void SetData(params object[] paramsObjects)
    {
        if (paramsObjects != null && paramsObjects.Length > 0 && paramsObjects[0] is JumpData)
        {
            _model = RegisterModel<RecollectionModel>();
            _model.JumpData = paramsObjects[0] as JumpData;
        }
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.COMMON_BACK:
                _cardListPanel.Hide();
                _panel.Show(0);
                break;
            case MessageConst.MODULE_RECOLLECTION_SHOW_CHOOSE_CARD:
                
                if (_cardListPanel == null)
                {
                    _cardListPanel = new CardListPanel();
                    _cardListPanel.Init(this);
                    RegisterModel<DrawData>();
                }
                _cardListPanel.Show(0);

                break;
            case MessageConst.MODULE_RECOLLECTION_CARD_SELECTED:
                _cardListPanel?.Hide();
                _panel.Show(0);
                if (_cardDropPropWindow)
                {                 
                    _cardDropPropWindow.Close();
                }
                break;
            case MessageConst.MODULE_RECOLLECTION_SHOW_CARD_DROP_PROP:
                _propId = (int) body[0];
                if (_model.OpenCardDict != null)
                {
                    ShowCardDropProp(_propId);
                }
                else
                {
                    NetWorkManager.Instance.Send<DrawProbRes>(CMD.DRAWC_DRAW_PROBS, null, OnGetOpenCardList, null, true);
                }
                break;
              
        }
    }

    private void ShowCardDropProp(int propId)
    {
        _cardDropPropWindow = PopupManager.ShowWindow<CardDropPropWindow>("Recollection/Prefabs/CardDropPropWindow");
       
        _cardDropPropWindow.SetData(_model.GetCardByPropId(propId), propId);
    }

    private void OnGetOpenCardList(DrawProbRes res)
    {
        _model.SetOpenCardDict(res.DrawProbs);
        ShowCardDropProp(_propId);
    }



    public void Destroy()
    {
		
    }
}
