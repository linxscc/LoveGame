using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using game.main;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateController : Controller
{
    public TemplateView CurTemplateView;
    private DateTime _curDateTime;
    public override void Start()
    {

    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_LOVEDIARY_TEMPLATE_SELECT:
                Debug.Log("CMD_LOVEDIARY_PREVIOUS_MONTH");
                TemplateSelect((int)body[0]);
                break;
        }
    }

    private void TemplateSelect(int id)
    {
        Debug.LogError("TemplateSelect " + id);
        //这里需要根据模板ID选择 创建数据 并传到日记编辑panel
        // 如果读取模板数据 并生成   这里先全部默认空模板
        string text = new AssetLoader().LoadTextSync(AssetLoader.GetDiaryTemplateDataPath(id.ToString()));
        CalendarDetailVo vo = JsonConvert.DeserializeObject<CalendarDetailVo>(text);

        CalendarDetailVo newvo = new CalendarDetailVo(_curDateTime.Year, _curDateTime.Month, _curDateTime.Day, vo.DiaryElements);
        SendMessage(new Message(MessageConst.MODULE_LOVEDIARY_SHOW_EDIT_PANEL, Message.MessageReciverType.DEFAULT , LoveDiaryEditType.Edit , newvo));
    }

    public void GoBack()
    {
        SendMessage(new Message(MessageConst.MODULE_LOVEDIARY_SHOW_CALENDAR_PANEL, Message.MessageReciverType.DEFAULT));
    }

    public void SetData(DateTime dateTime)
    {
        _curDateTime = dateTime;

        List<ElementPB> unlocks = new List<ElementPB>() ;
        List<ElementPB> locks= new List<ElementPB>(); ;
        GlobalData.DiaryElementModel.GetElementByType(ElementTypePB.Template, ElementModulePB.Diary, ref unlocks, ref locks);
        CurTemplateView.SetData(unlocks, locks);
    }
}
