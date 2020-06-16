using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Framework.Utils;
using DataModel;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CalendarView : View
{
    ScrollRect _scrollRect;
    Text _YText;//年显示文本
    Text _MText;//月显示文本
    Button _leftBtn;
    Button _rightBtn;
    Button _showOrRecordBtn;//只表示今天记录 或者未记录

    DateTime _unlockDt;
    //GameObject _todatyBg;
    //GameObject _hasImg;

    Color _unnoteColor = new Color(0.99f, 0.49f, 0.70f);
    Color _noteColor = Color.white;

    private void Awake()
    {
        _YText = transform.Find("YTxt").GetComponent<Text>();
        _MText = transform.Find("MTxt").GetComponent<Text>();
        _leftBtn = transform.Find("LeftBtn").GetComponent<Button>();
        _rightBtn = transform.Find("RightBtn").GetComponent<Button>();
       // _showOrRecordBtn=transform.Find("ShowOrRecordBtn").GetComponent<Button>();
        _scrollRect = transform.Find("Scroll View").GetComponent<ScrollRect>();
        _leftBtn.onClick.AddListener(() => {

            SendMessage(new Message(MessageConst.CMD_LOVEDIARY_PREVIOUS_MONTH));
        });
        _rightBtn.onClick.AddListener(() => {
            SendMessage(new Message(MessageConst.CMD_LOVEDIARY_NEXT_MONTH));
        });
        //_showOrRecordBtn.onClick.AddListener(()=>{
        //    //todo transform param for selecting to view;
        //    SendMessage(new Message(MessageConst.MODULE_LOVEDIARY_SHOW_TEMPLATE_PANEL));
        //});

    }

    private void Start()
    {
        var prefab = GetPrefab("LoveDiary/Prefabs/CalendarItem");
        for (int i = 0; i < LoveDiaryModel.maxShowNum; i++)
        {
            var t = i;
            var item = Instantiate(prefab) as GameObject;
            item.transform.SetParent(_scrollRect.content, false);
           // item.transform.localScale = Vector3.one;
            item.GetComponent<Button>().onClick.AddListener(() => {
                Debug.Log(t);
                OnItemClick(t);
            });
        }

       // _showOrRecordBtn.transform.Find("Text").GetComponent<Text>().text = "记录今天";
    }

    private void OnItemClick(int idx)
    {
        Debug.Log("OnItemClick" + idx);
        DateTime clickDateTime = _dateTimes[idx];

        SendMessage(new Message(MessageConst.CMD_LOVEDIARY_CALENDAR_SELECT, Message.MessageReciverType.CONTROLLER,clickDateTime));

        ////判断今天是否记录过
        //bool isRecorded = _calendarVos.Find(m => { return m.Year == clickDateTime.Year 
        //    && m.Month == clickDateTime.Month && m.Day == clickDateTime.Day; }) != null;
        //_showOrRecordBtn.transform.Find("Text").GetComponent<Text>().text = isRecorded ? "查看日记" : "记录今天";

    }


    private void SetYearAndMonth(DateTime curDt)
    {
            // string str = string.Format("{0:D2}年{1:D2}月", curDt.Year, curDt.Month);
            _YText.text = curDt.Year.ToString();
            _MText.text = curDt.ToString("MMMM", new System.Globalization.CultureInfo("en-us"))
                + "|" + curDt.Month.ToString() + I18NManager.Get("LoveDiary_Month");// "月";
            DateTime today = DateUtil.GetTodayDt();
            if (curDt.Year == today.Year && curDt.Month == today.Month)
            {
                _rightBtn.interactable = false;
                Color c = new Color(1, 1, 1, 0.5f);
                _rightBtn.transform.Find("Image").GetComponent<Image>().color = c;
            }
            else
            {
                _rightBtn.interactable = true;
                _rightBtn.transform.Find("Image").GetComponent<Image>().color = Color.white;
            }

            if (_unlockDt.Year == curDt.Year && _unlockDt.Month == curDt.Month)
            {
                _leftBtn.interactable = false;
                Color c = new Color(1, 1, 1, 0.5f);
                _leftBtn.transform.Find("Image").GetComponent<Image>().color = c;
            }
            else
            {
                _leftBtn.interactable = true;
                _leftBtn.transform.Find("Image").GetComponent<Image>().color = Color.white;
            }

        
    }
    
    private List<DateTime> _dateTimes;
    private List<CalendarVo> _calendarVos;
    private void UpdateView(List<DateTime> lst, List<CalendarVo> vos)
    {
        _dateTimes = lst;
        _calendarVos = vos;
        DateTime curDt=lst[0];

        ShowTypes curShowType = curDt.Day < 8 && curDt.Day > 1 ? ShowTypes.Five : ShowTypes.Six;

        MonthState curState = curDt.Day < 8&&curDt.Day>1 ? MonthState.Current : MonthState.Previous;

        if(curState==MonthState.Current)
        {
            SetYearAndMonth(curDt);
        }

        bool isNextFive = true;

        for (int i = 0; i < lst.Count; i++)
        {
            curDt = lst[i];

            if (curDt.Day == 1)//每月第一天
            {
                curState += 1;
                if (curState == MonthState.Current)
                {
                    SetYearAndMonth(curDt);
                }else if (curState == MonthState.Next)
                {
                    isNextFive = LoveDiaryModel.IsFiveLine(curDt);
                }
            }

            GameObject item = _scrollRect.content.GetChild(i).gameObject;

            if (curState==MonthState.Previous)
            {
                item.transform.Find("Contain").gameObject.Hide();
                item.GetComponent<Button>().interactable = false;
                item.transform.Find("Contain/NextMonth").gameObject.Hide();
            }
            else if(curState ==MonthState.Current)
            {
                item.transform.Find("Contain").gameObject.Show();
                item.GetComponent<Button>().interactable = true;
                item.transform.Find("Contain/NextMonth").gameObject.Hide();
            }
            else if(curState==MonthState.Next)
            {
                if(isNextFive)
                {
                    item.transform.Find("Contain").gameObject.Hide();
                    item.GetComponent<Button>().interactable = false;
                    item.transform.Find("Contain/NextMonth").gameObject.Hide();
                }
                else
                {
                    item.transform.Find("Contain").gameObject.Show();
                    item.GetComponent<Button>().interactable = true;
                    item.transform.Find("Contain/NextMonth").gameObject.Show();
                }
            }
            //item.transform.Find("Contain").gameObject.Show();     

           // item.GetComponent<Button>().interactable = curState == MonthState.Current;

            bool isShowHasImg = vos.Find(m => { return m.Year == curDt.Year && m.Month == curDt.Month && m.Day == curDt.Day; }) == null;
            item.transform.Find("Contain/HasImg").gameObject.SetActive(!isShowHasImg);
            bool isToday = DateUtil.CheckIsToday(curDt)==0;
            item.transform.Find("Contain/TodayBg").gameObject.SetActive(isToday);

            string numpath = "UIAtlas_LoveDiary_Calendar_Num" + curDt.Day.ToString();
            item.transform.Find("Contain/Num").GetComponent<Image>().sprite = AssetManager.Instance.GetSpriteAtlas(numpath);
        }
    }

    public void SetData(List<DateTime> lst,List<CalendarVo> vos,DateTime unlockDt)
    {
        _unlockDt = unlockDt;
        UpdateView(lst,vos);
    }
}
