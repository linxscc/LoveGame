using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using game.main;
using System;
using Common;

public class DrawCardModule : ModuleBase
{
    private DrawCardPanel _drawCardPanel;
    private CardCollectionShowPanel _cardCollectionShowPanel;
    private DrawPanel _drawPanel;
    private DrawCardResultPanel _drawResultPanel;
    private bool needmove = false;
    private bool isCardShow = false;
    private bool isBack = false;
    private OtherDrawPanel _otherDrawPanel;
    private bool _isShowOtherDrawViewBottomBg = true;
    public override void Init()
    {
        if (isCardShow)
        {
           _otherDrawPanel = new OtherDrawPanel();
            _otherDrawPanel.Init(this);
            _otherDrawPanel.SetData(_paramsObjects);
            // _otherDrawPanel.IsShowBottomBg(_isShowOtherDrawViewBottomBg);
            return;
        }

        GuideManager.RegisterModule(this);


        //ClientTimer.Instance.DelayCall(() =>
        //{
        //    AssetManager.Instance.LoadAtlas("uiatlas_drawcard2");
        //}, 0.2f);
        _drawCardPanel = new DrawCardPanel();
        _drawCardPanel.Init(this);
        if (needmove)
        {
            _drawCardPanel.MoveGoldView(); 
        }
       // _drawCardPanel.Show(0);
    }
   
    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        if(_drawCardPanel!=null)
        {
            _drawCardPanel.ShowBackBtn();
            _drawCardPanel.UpdateTicke();
        }
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.MODULE_CARD_SHOW_DRAW_VIEW:
                if (_drawPanel == null)
               {
                    _drawPanel = new DrawPanel();
                    _drawPanel.Init(this);
                }
                _drawPanel.Show2((List<DrawCardResultVo>)body[0]);
               // _drawPanel.Show((DrawTypePB)body[0],(long) body[1] );
                break;
            case MessageConst.MODULE_DRAWCARD_SHOW_RESULT_PANEL:
                if(_drawPanel!=null)
                {
                    _drawPanel.Hide();
                    //_drawPanel = null;
                }

                //_drawPanel.Destroy();
                //_drawPanel = null;
                List<DrawCardResultVo> awardPbs = (List<DrawCardResultVo>) message.Body;

                _drawResultPanel = new DrawCardResultPanel();
                _drawResultPanel.Init(this);
                _drawResultPanel.SetData(awardPbs);

                _drawCardPanel.UpdateCardNum();
                _drawCardPanel.Hide();            
                break;
            case MessageConst.MODULE_VIEW_BACK_DRAWCARD:
               
                if(_cardCollectionShowPanel != null)
                {
                    _cardCollectionShowPanel.Destroy();
                    _cardCollectionShowPanel = null;
                }
               
                if (_drawResultPanel != null)
                {
                    _drawResultPanel.Destroy();
                    _drawResultPanel = null;
                }
                if (_drawPanel != null)
                {
                    _drawPanel.Destroy();
                    _drawPanel = null;
                }
                
                _drawCardPanel.Show(0);
                break;
            case MessageConst.MODULE_DRAWCARD_GOTO_CARD_COLLECTION:
                if (_cardCollectionShowPanel == null)
                {
                    _cardCollectionShowPanel = new CardCollectionShowPanel();
                    _cardCollectionShowPanel.Init(this);
                }
                DrawPoolTypePB poolType = (DrawPoolTypePB)body[0];
                DrawEventPB drawEvent = (DrawEventPB)body[1];

                _drawCardPanel.Hide();
                _cardCollectionShowPanel.SetData(poolType, drawEvent);
                break;
            case MessageConst.MODULE_DRAWCARD_GOBACK:
                if(isBack)
                {
                    return;
                }
                isBack = true;
                ClientTimer.Instance.DelayCall(() => {
                    ModuleManager.Instance.GoBack();
                    isBack = false;
                    _finishedCardShow();
                }, 0.2f);        
                break;
        }
    }

    Action _finishedCardShow;
    private object[] _paramsObjects;
    public override void SetData(params object[] paramsObjects)
    {
        if (paramsObjects.Length>0)
        {
            var target = (string) paramsObjects[0];
            if (target=="DrawCard_Gold")
            {
                needmove = true;
            }
            else if(target == "DrawCard_CardShow")
            {
                _paramsObjects = paramsObjects;
                isCardShow = true;
                _finishedCardShow = (Action)paramsObjects[2];
                if(paramsObjects.Length==4)             
                {                  
                    _isShowOtherDrawViewBottomBg = (bool) paramsObjects[3];
                }
            }
        }
        
    }

    public override void Remove(float delay)
    {
        //AssetManager.Instance.UnloadAtlas("uiatlas_drawcard2");
        base.Remove(delay);
    }

    public void Close()
    {
    }
    
    
}