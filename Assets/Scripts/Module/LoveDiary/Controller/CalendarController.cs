using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Module.Framework.Utils;

public class CalendarController : Controller {

    public CalendarView CurCalendarView;
    DateTime _curDT;//当前UI显示时间

    DateTime _requestDT;//请求数据的时间
    int _requestDTNum;//请求数据时间数量
    public override void Start()
    {
        base.Start();
        //开始默认当前月和前一个月的 两个月的
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        _curDT = dtStart.AddSeconds(ClientTimer.Instance.GetCurrentTimeStamp() / 1000);
        SendMyDiaryStateMsg(_curDT, 2);
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_LOVEDIARY_PREVIOUS_MONTH:
                Debug.Log("CMD_LOVEDIARY_PREVIOUS_MONTH");
                GetNewMonthData(MonthState.Previous);
                break;
            case MessageConst.CMD_LOVEDIARY_NEXT_MONTH:
                Debug.Log("CMD_LOVEDIARY_NEXT_MONTH");
                GetNewMonthData(MonthState.Next);
                break;
            case MessageConst.CMD_LOVEDIARY_CALENDAR_SELECT:
                Debug.Log("CMD_LOVEDIARY_CALENDAR_SELECT");
                DateTime Dt = (DateTime)body[0];
                if (DateUtil.CheckIsToday(Dt)==1) 
                {
                    PopupManager.ShowAlertWindow(I18NManager.Get("LoveDiary_Hint8"), I18NManager.Get("Common_Hint"),I18NManager.Get("Common_Know")); 
                    return;
                }
                if (DateUtil.CompareToday(GetData<LoveDiaryModel>().UnlockDt, Dt) == 1) 
                {
                    PopupManager.ShowAlertWindow(I18NManager.Get("LoveDiary_Hint9"), I18NManager.Get("Common_Hint"), I18NManager.Get("Common_Know"));
                    return;
                }
                SelctCalender(Dt);
                break;
        }
    }

    public void UpdateView()
    {
        List<DateTime> dts = LoveDiaryModel.ToDays(_curDT);
        CurCalendarView.SetData(
            dts,
            GetData<LoveDiaryModel>().GetCalendarDatas(dts),
            GetData<LoveDiaryModel>().UnlockDt
            );
    }

    private void SelctCalender(DateTime dt)
    {
        if (GetData<LoveDiaryModel>().CheckHasDetailData(dt))
        {
            //进入查看编辑状态
            CalendarDetailVo vo = GetData<LoveDiaryModel>().GetCalendarDetailData(dt);
            SendMessage(new Message(MessageConst.MODULE_LOVEDIARY_SHOW_EDIT_PANEL, Message.MessageReciverType.DEFAULT, LoveDiaryEditType.Show, vo));
            return;
        }

        if(GetData<LoveDiaryModel>().CheckHasCalendarData(dt))//当天时候有记录日记，但是没有详细日记数据
        {
            //获取日记集体数据 写过的日记才有  新日记不必发送
            var req = new MyDiaryDetailReq();
            //req.Id = GetData<LoveDiaryModel>().GetDiarySvrId(dt);//日记ID
            req.Year = dt.Year;
            req.Month = dt.Month;
            req.Date = dt.Day;

            var dataBytes = NetWorkManager.GetByteData(req);
            NetWorkManager.Instance.Send<MyDiaryDetailRes>(CMD.DIARYC_USER_DIARYDETAIL, dataBytes,(res)=> { OnMyDiaryDetailHandler(res, dt); } );
            return;
        }
        //全新日记
        SendMessage(new Message(MessageConst.MODULE_LOVEDIARY_SHOW_TEMPLATE_PANEL, Message.MessageReciverType.DEFAULT, dt));
    }

    private void OnMyDiaryDetailHandler(MyDiaryDetailRes res, DateTime dt)
    {
        Debug.Log("OnMyDiaryDetailHandler " + res);
        UserDiaryPB pb = res.UserDiary;
        CalendarDetailVo vo = new CalendarDetailVo(dt.Year, dt.Month, dt.Day,pb.DiaryElements.ToList());
        GetData<LoveDiaryModel>().AddCalendarDetailData(dt,vo);
        SendMessage(new Message(MessageConst.MODULE_LOVEDIARY_SHOW_EDIT_PANEL, Message.MessageReciverType.DEFAULT, LoveDiaryEditType.Show, vo));
        //todo
    }

    private void GetNewMonthData(MonthState state)
    {
        int addNum = state == MonthState.Previous ? -1 : 1;
        _curDT = _curDT.AddMonths(addNum);
        if (state==MonthState.Previous && !GetData<LoveDiaryModel>().CheckHasData(_curDT))
        {
            //没有数据需要拉去
            SendMyDiaryStateMsg(_curDT.AddMonths(-1));
            return;
        }
        List<DateTime> dts = LoveDiaryModel.ToDays(_curDT);
        CurCalendarView.SetData(
            dts,
            GetData<LoveDiaryModel>().GetCalendarDatas(dts),
            GetData<LoveDiaryModel>().UnlockDt
            );
    }

    /// <summary>
    /// 获取当前时间（包含当月），总共preMonthNum个月的数据
    /// </summary>
    /// <param name="Dt"></param>
    /// <param name="preMonthNum"></param>
    private void SendMyDiaryStateMsg(DateTime Dt, int preMonthNum = 1)
    {
        var req = new MyDiaryStateReq();
        _requestDT = Dt;
        _requestDTNum = preMonthNum;
        DateTime dt = Dt;
        UserDiaryDatePB pb;

        for (int i=0;i< preMonthNum;i++)
        {
            pb= new UserDiaryDatePB();
            pb.Year = dt.Year;
            pb.Month = dt.Month;
            dt = dt.AddMonths(-1);
            req.DiaryStates.Add(pb);
        }

        var dataBytes = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<MyDiaryStateRes>(CMD.DIARYC_USER_DIARYSTATE, dataBytes, OnMyDiaryStateHandler);
    }

    private void OnMyDiaryStateHandler(MyDiaryStateRes res)
    {
        Debug.Log("OnMyDiaryStateHandler");
        for (int i = 0; i < res.UserDiaryDates.Count; i++)
        {
            GetData<LoveDiaryModel>().AddCalendarData(res.UserDiaryDates[i]);
        }

        DateTime unlockDt = DateUtil.GetDataTime(res.UnlockTime);
        GetData<LoveDiaryModel>().UnlockDt = unlockDt;

        DateTime dt = _requestDT;
        for (int i = 0; i < _requestDTNum; i++)
        {
            GetData<LoveDiaryModel>().AddCalendarYearMonth(dt.Year * 100 + dt.Month);
            dt = _requestDT.AddMonths(-1);
        }

        List<DateTime> dts = LoveDiaryModel.ToDays(_curDT);
        CurCalendarView.SetData(
            dts,
            GetData<LoveDiaryModel>().GetCalendarDatas(dts),      
            GetData<LoveDiaryModel>().UnlockDt
            );
    }

}
