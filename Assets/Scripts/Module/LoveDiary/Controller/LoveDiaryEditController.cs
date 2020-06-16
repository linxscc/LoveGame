using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using Framework.Utils;
using game.main;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LoveDiaryEditType
{
    Edit,
    Show
}
public enum LoveDiaryBottomEditState
{
    Hide,
    Show
}

public enum LoveDiaryBottomEditType
{
    None,
    Image,
    Racket,
    Bg,//背景
    Text,//文本编辑
    SecondText//二次文本编辑
}

public class LoveDiaryEditController : Controller
{
    LoveDiaryEditType _curEditType;

    public LoveDiaryEditType EditType
    {
        get
        {
            return _curEditType;
        }
        set
        {
            if (_curEditType == value)
                return;
            _curEditType = value;
            CurLoveDiaryEditView.CurLoveDiaryEditType = _curEditType;
        }
    }

    public bool IsModify = false;

    public LoveDiaryEditView CurLoveDiaryEditView;
    CalendarDetailVo _curCalendarDetailVo;
    public override void Start()
    {
        EventDispatcher.AddEventListener<string>(EventConst.LoveDiaryEditOkInputText, OnElementInputClick);
        EventDispatcher.AddEventListener<int>(EventConst.LoveDiaryEditItemClick, OnElementItemClick);
        EventDispatcher.AddEventListener<int>(EventConst.LoveDiaryEditBgItemClick, OnElementBgItemClick);
        EventDispatcher.AddEventListener<Color>(EventConst.LoveDiaryEditColorItemClick, OnElementColorItemClick);
        EventDispatcher.AddEventListener<int>(EventConst.LoveDiaryEditItemText, OnElementTextItemEditClick);
        EventDispatcher.AddEventListener<DiaryElementPB>(EventConst.LoveDiaryEditDeleteElement, OnElementTextItemDeleteClick);
        EventDispatcher.AddEventListener(EventConst.LoveDiaryElementModify, OnLoveDiaryElementModify);
        EventDispatcher.AddEventListener<bool>(EventConst.LoveDiaryHideEditText, ShowElementItem);
    }

    private void OnLoveDiaryElementModify()
    {
        if (IsModify == true)
            return;
        IsModify = true;
    }

    private void OnElementTextItemDeleteClick(DiaryElementPB pb)
    {
        Debug.Log("OnElementTextItemDeleteClick");
        ElementPB elementRule = CalendarDetailVo.GetElementRuleById(pb.ElementId);
        _curCalendarDetailVo.CurDiaryElementCount.SubCount(elementRule.ElementType);
        IsModify = true;
    }

    public void GoBack()
    {
        if( GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) < GuideConst.MainStep_Over)
        {
            PopupManager.ShowAlertWindow(I18NManager.Get("LoveDiary_Hint10"));
            return;
        }

        if (IsModify)
        {                                                //编辑内容未保存，是否返回
            PopupManager.ShowConfirmWindow(I18NManager.Get("LoveDiary_Hint5")).WindowActionCallback = evt =>
            {
                if (evt != WindowEvent.Ok)
                {
                    return;
                }
                SendMessage(new Message(MessageConst.MODULE_LOVEDIARY_SHOW_CALENDARORTEMPLATE_PANEL, Message.MessageReciverType.DEFAULT));
            };
            return;
        }
        SendMessage(new Message(MessageConst.MODULE_LOVEDIARY_SHOW_CALENDARORTEMPLATE_PANEL, Message.MessageReciverType.DEFAULT));
    }

    private void OnElementTextItemEditClick(int elementId)
    {
        Debug.Log("OnElementTextItemEditClick");
        
        //有个问题啊，要知道点击的是什么类型的元素！
        ElementPB elementRule = CalendarDetailVo.GetElementRuleById(elementId);
        var bottomEditType = LoveDiaryBottomEditType.SecondText;
        switch (elementRule.ElementType)
        {
            case ElementTypePB.Text:
                bottomEditType = LoveDiaryBottomEditType.SecondText;
                break;
            case ElementTypePB.Image:
                bottomEditType = LoveDiaryBottomEditType.Image;
                break;
            case ElementTypePB.Racket:
                bottomEditType = LoveDiaryBottomEditType.Racket;
                break;
            case ElementTypePB.Bg:
                bottomEditType = LoveDiaryBottomEditType.Bg;
                break;
            default:
                bottomEditType = LoveDiaryBottomEditType.SecondText;
                break;
        }
        
        CurLoveDiaryEditView.OperateBottom(true, bottomEditType);
        IsModify = true;
    }

    private void OnElementInputClick(string str)
    {
        //int id = 7000;// rule 默认 Text id==7000;
        ElementPB elementRule = CalendarDetailVo.GetElementRuleById(7000);
        if (_curCalendarDetailVo.CurDiaryElementCount.IsUpperLimited(elementRule.ElementType))
        {
            FlowText.ShowMessage(I18NManager.Get("LoveDiary_Hint6"));//("已达上限！");
            return;
        }

        CurLoveDiaryEditView.AddElement(GetNewDiaryElementPB(7000,str),true);
        _curCalendarDetailVo.CurDiaryElementCount.AddCount(elementRule.ElementType);
        //CurLoveDiaryEditView.OperateBottom(false);
        IsModify = true;
    }

    private DiaryElementPB GetNewDiaryElementPB(int id,string content="")
    {
        DiaryElementPB pb = new DiaryElementPB();
        pb.ElementId = id;
        pb.XPos = 0;
        pb.YPos = 0;
        pb.ScaleX = id==7000?10: 1;
        pb.ScaleY = 1;
        pb.Content = content;
        pb.Rotation = 0;
        pb.Size = 50;
        return pb;
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_LOVEDIARY_EDIT_SAVE:
                List<DiaryElementPB> pbs = (List<DiaryElementPB>)body[0];
                _curCalendarDetailVo.DiaryElements.Clear();
                _curCalendarDetailVo.DiaryElements.AddRange(pbs);
               // SaveToJson();
                SaveDiaryReq req = new SaveDiaryReq();
                req.Year = _curCalendarDetailVo.Year;
                req.Month = _curCalendarDetailVo.Month;
                req.Date = _curCalendarDetailVo.Day;
                req.DiaryElements.AddRange(_curCalendarDetailVo.DiaryElements);
                var dataBytes = NetWorkManager.GetByteData(req);
                NetWorkManager.Instance.Send<SaveDiaryRes>(CMD.DIARYC_SAVEDIARY, dataBytes, OnMyDiarySaveHandler);
                break;
            case MessageConst.CMD_LOVEDIARY_ENTER_EDITTYPE:
                EditType = (LoveDiaryEditType)body[0];
                break;
            case MessageConst.CMD_LOVEDIARY_ENTER_SELECT_IMAGE:
                CurLoveDiaryEditView.OperateBottom(true, LoveDiaryBottomEditType.Image);
                break;
            case MessageConst.CMD_LOVEDIARY_ENTER_SELECT_NONE:
                CurLoveDiaryEditView.OperateBottom(false);
                break;
            case MessageConst.CMD_LOVEDIARY_ENTER_SELECT_LABEL:
                bool isShow = (bool)body[0];
                CurLoveDiaryEditView.ShowLabel(isShow);
                //GuideManager.Hide();
                break;
        }
    }

    private void SaveToJson()
    {
        //string str = FileUtil.ReadFileText(Path.PhoneSmsPath, "/" + fileName + ".json");
        //if (str == null)
        //{
        //    FlowTextCtrl.ShowMessage("文件不存在");
        //    return;
        //}
        //CalendarDetailVo infos = JsonConvert.DeserializeObject<CalendarDetailVo>(str);
       // JsonConvert.DeserializeObject<CalendarDetailVo>("111");
        string json = JsonConvert.SerializeObject(_curCalendarDetailVo);
        json = FileUtil.ConvertJsonString(json);
        //string path = AssetLoader.GetDiaryTemplateDataPath("Sb11d");
        FileUtil.SaveFileText("DiaryTemplate/Data", "SB110.json", json);
    }

    private void OnMyDiarySaveHandler(SaveDiaryRes res)
    {
        Debug.Log(res);
        if(res.Ret!=-1)
        {
            FlowText.ShowMessage(I18NManager.Get("Common_SaveFailure"));// ("保存失败!");
            return;
        }
        GuideManager.SetRemoteGuideStep(GuideTypePB.MainGuide, GuideConst.MainStep_Over);

        if (res.UserFavorability!=null)
        {
            //todo暂时在这里更新 考虑去主界面更新
            GlobalData.FavorabilityMainModel.UpdateUserFavorability(res.UserFavorability);
        }
        _curCalendarDetailVo.Year = res.UserDiary.Year;
        _curCalendarDetailVo.Month = res.UserDiary.Month;
        _curCalendarDetailVo.Day = res.UserDiary.Date;

        DateTime now = DateUtil.GetTodayDt();

        bool isHasData = GetData<LoveDiaryModel>().CheckHasDetailData(
            res.UserDiary.Year, 
            res.UserDiary.Month,
            res.UserDiary.Date);
        if (!isHasData)//恋爱日记元素使用
        {
            Dictionary<int, int> onUse = new Dictionary<int, int>();
            foreach(var v in _curCalendarDetailVo.DiaryElements)
            {
                if(!onUse.ContainsKey(v.ElementId))
                {
                    onUse[v.ElementId] = 1;
                    continue;
                }
                onUse[v.ElementId]++;
            }
            foreach(var vk in onUse)
            {               
                string name = I18NManager.Get("LoveDiary_Hint11", vk.Key);
                SdkHelper.StatisticsAgent.OnUse(name, vk.Value);
            }

        }

        GetData<LoveDiaryModel>().AddCalendarDetailData(_curCalendarDetailVo.Year,
            _curCalendarDetailVo.Month,
            _curCalendarDetailVo.Day, _curCalendarDetailVo);

        GetData<LoveDiaryModel>().AddCalendarData(
            _curCalendarDetailVo.Year, 
            _curCalendarDetailVo.Month,
            _curCalendarDetailVo.Day);

        if (!isHasData &&
            res.UserDiary.Year == now.Year &&
            res.UserDiary.Month == now.Month &&
            res.UserDiary.Date == now.Day) //表示今天
        {
            FlowText.ShowMessage(I18NManager.Get("Common_SaveSucceed"), 0.3f);
            ClientTimer.Instance.DelayCall(() =>
            {
                // ModuleManager.Instance.GoBack();
                ModuleManager.Instance.DestroyAllModuleBackToCommon();
               // ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_GAME_MAIN);
                EventDispatcher.TriggerEvent<int>(
                    EventConst.LoveDiaryEditSaveAndGoBackMainModule,
                    CurLoveDiaryEditView.CurLabelElementId);
            }, 0.5f);
        }
        else
        {
            SendMessage(new Message(MessageConst.MODULE_LOVEDIARY_SHOW_CALENDAR_PANEL, Message.MessageReciverType.DEFAULT));
        }
    }
    private void OnElementColorItemClick(Color color)
    {
       // GetData<LoveDiaryModel>().CurTextColor = color;
        CurLoveDiaryEditView.TextEditColor = color;
        IsModify = true;
    }

    private void ShowElementItem(bool enable)
    {
        Debug.Log("ShowElementItem");
        CurLoveDiaryEditView.HideTextInput(enable);
    }
    
    private void OnElementBgItemClick(int id)
    {
        Debug.Log("OnElementBgItemClick");
        CurLoveDiaryEditView.SetBgElement(id);
        IsModify = true;
    }
    private void OnElementItemClick(int id)
    {
        Debug.Log("OnElementItemClick........................ id " + id);

        ElementPB elementRule = CalendarDetailVo.GetElementRuleById(id);


        if (GlobalData.DiaryElementModel.IsCanUseElement(id)) //表示需要购买
        {
            if (_curCalendarDetailVo.CurDiaryElementCount.IsUpperLimited(elementRule.ElementType))
            {
                FlowText.ShowMessage(I18NManager.Get("LoveDiary_Hint6"));// ("已达上限！");
                return;
            }

            if (elementRule.ElementType == ElementTypePB.Label)
            {
                CurLoveDiaryEditView.CurLabelElementId = id;
            }
            else
            {
                CurLoveDiaryEditView.AddElement(GetNewDiaryElementPB(id),false);
            }
            _curCalendarDetailVo.CurDiaryElementCount.AddCount(elementRule.ElementType);
            CurLoveDiaryEditView.OperateBottom(false);
            IsModify = true;
        }
        else
        {
            string stype = "";
            switch(elementRule.ElementType)
            {
                case ElementTypePB.Bg:
                    stype = I18NManager.Get("LoveDiary_Background");//"背景";
                    break;
                case ElementTypePB.Image:
                    stype = I18NManager.Get("LoveDiary_Tags");//"贴纸";
                    break;
            }
            string str = I18NManager.Get("LoveDiary_Hint12") + elementRule.UnlockClaim.Gem + I18NManager.Get("LoveDiary_Hint7") + stype;
            PopupManager.ShowConfirmWindow(str).WindowActionCallback = evt =>
            {
                if (evt != WindowEvent.Ok)
                { return; }

                BuyElementReq req = new BuyElementReq();
                req.ElementId = id;
                var dataBytes = NetWorkManager.GetByteData(req);
                NetWorkManager.Instance.Send<BuyElementRes>(CMD.DIARYC_ELEMENTS_BUY, dataBytes, OnMyBuyElementHandler);
            };
        }
    }


    private void OnMyBuyElementHandler(BuyElementRes res)
    {
        Debug.Log("OnMyBuyElementHandler");
        GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
        GlobalData.DiaryElementModel.UpdateElement(res.UserElement.ElementId, res.UserElement.Num);
        ElementPB pb= GlobalData.DiaryElementModel.GetElementRuleById(res.UserElement.ElementId);
        CurLoveDiaryEditView.UpdateItemState(pb);
        FlowText.ShowMessage(I18NManager.Get("Common_BuySucceed"));// ("购买成功！");
    }

    public void SetData(CalendarDetailVo calendarDetailVo)
    {
        Debug.Log("LoveDiaryEditController   SetData");
        _curCalendarDetailVo = calendarDetailVo;
        _curCalendarDetailVo.CurDiaryElementCount.MaxImageCount = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.DIARY_MAX_IMAG_COUNT);
        _curCalendarDetailVo.CurDiaryElementCount.MaxTextCount = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.DIARY_MAX_TEXT_COUNT);
        _curCalendarDetailVo.CurDiaryElementCount.MaxRacketCount = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.DIARY_MAX_RACKET_COUNT);
        _curCalendarDetailVo.CurDiaryElementCount.CurImageCount = 0;
        _curCalendarDetailVo.CurDiaryElementCount.CurTextCount = 0;
        _curCalendarDetailVo.CurDiaryElementCount.CurRacketCount = 0;
        DiaryElementPB pb;
        for (int i = 0; i < _curCalendarDetailVo.DiaryElements.Count; i++) 
        {
            pb = _curCalendarDetailVo.DiaryElements[i];
            ElementPB elementRule = CalendarDetailVo.GetElementRuleById(pb.ElementId);
            if (elementRule.ElementType == ElementTypePB.Image) {
                _curCalendarDetailVo.CurDiaryElementCount.CurImageCount++;
            }
            else if(elementRule.ElementType == ElementTypePB.Text) {
                _curCalendarDetailVo.CurDiaryElementCount.CurTextCount++;
            }
            else if (elementRule.ElementType == ElementTypePB.Racket) {
                _curCalendarDetailVo.CurDiaryElementCount.CurRacketCount++;
            }
        }
        CurLoveDiaryEditView.SetData(_curEditType, GlobalData.DiaryElementModel, calendarDetailVo);
    }


    private void OnMyDiaryDetailHandler(MyDiaryDetailRes res)
    {
        //todo
    }

    public override void Destroy()
    {

        Debug.LogError("Destroy..................");
       EventDispatcher.RemoveEventListener<int>(EventConst.LoveDiaryEditItemClick, OnElementItemClick);
        EventDispatcher.RemoveEventListener<int>(EventConst.LoveDiaryEditBgItemClick, OnElementBgItemClick);
        EventDispatcher.RemoveEventListener<Color>(EventConst.LoveDiaryEditColorItemClick, OnElementColorItemClick);
        EventDispatcher.RemoveEventListener<string>(EventConst.LoveDiaryEditOkInputText, OnElementInputClick);
        EventDispatcher.RemoveEventListener<int>(EventConst.LoveDiaryEditItemText, OnElementTextItemEditClick);
        EventDispatcher.RemoveEventListener<DiaryElementPB>(EventConst.LoveDiaryEditDeleteElement, OnElementTextItemDeleteClick);
        EventDispatcher.RemoveEventListener(EventConst.LoveDiaryElementModify, OnLoveDiaryElementModify);
        base.Destroy();

    }

}
