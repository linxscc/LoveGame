using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class CreateUserWindow : Window
{
	private InputField _inputField;
	private Button _birthdayBtn;
	private Button _okBtn;
	private Transform _timeData;
	private Text _monthTxt;
	private Text _dayTxt;
	//private DatePicker _datePicker;

    private DatePickerComponent _monthCom;
    private DatePickerComponent _dayCom;

    private void Awake()
	{
		_inputField = transform.Find("InputField").GetComponent<InputField>();
		_birthdayBtn = transform.GetButton("Birthday/BirthdayBtn");
		_okBtn = transform.GetButton("OkBtn");
		//_datePicker = transform.Find("DatePicker").GetComponent<DatePicker>();
//		_timeData=transform.Find("TimeData");

		_monthTxt = transform.GetText("Birthday/MonthText");
		_dayTxt = transform.GetText("Birthday/DayText");

		_birthdayBtn.onClick.AddListener(() =>
		{
			//_datePicker.gameObject.SetActive(true);
//			_timeData.gameObject.Show();
		});
		
		
		
//		_timeData.GetButton("Bg/CancelBtn").onClick.AddListener(() => {_timeData.gameObject.Hide(); });
//		
//		_timeData.GetButton("Bg/ConfirmBtn").onClick.AddListener(() =>
//		{
//			
////			var time = DatePickerGroup.SelectTime;
////			_monthTxt.text = time.Month.ToString();
////			_dayTxt.text = time.Day.ToString();			
//		});
		
		_inputField.onValueChanged.AddListener(InputFieldValueChange);
		_okBtn.onClick.AddListener(OnCreateUser);

        _monthCom = transform.Find("Month").GetComponent<DatePickerComponent>();
        _dayCom = transform.Find("Day").GetComponent<DatePickerComponent>();
    }


    List<int> _month;
    List<int> _day;
    List<int> _dayCount;
    private void Start()
    {
        _month = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11,12};
        _dayCount = new List<int> { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        var today = DateTime.Today;
        int day = DateTime.DaysInMonth(today.Year, today.Month);
        _day = new List<int>();
        for (int i=1;i<32;i++)
        {
            _day.Add(i);
        }
      //  Debug.LogError(_monthCom.Count);
      //  Debug.LogError(_dayCom.Count);
        _monthCom.Count = _month.Count;
        _monthCom.updateCallBack = MonthUpdateCallbaclk;
        _monthCom.cursorCallBack = MonthCursorCallbaclk;
        _monthCom.finishedMoveCallBack = FinishedMoveCursorCallbaclk;
        _monthCom.Refreash();
        _dayCom.Count = _day.Count;
        _dayCom.updateCallBack = DayUpdateCallbaclk;
        _dayCom.cursorCallBack = DayCursorCallbaclk;
        _dayCom.Refreash();
        _dayTxt.text = _day[0].ToString();
        _monthTxt.text = _month[0].ToString();
    }

    private void FinishedMoveCursorCallbaclk(int arg2)
    {
      //  Debug.LogError("FinishedMoveCursorCallbaclk "+ arg2);
        _dayCom.Count = _dayCount[_month[arg2] - 1];
        _dayCom.Refill();
        //_dayTxt.text = _day[arg2].ToString();
    }

    private void DayCursorCallbaclk(int arg2)
    {
        _dayTxt.text = _day[arg2].ToString();
    }
    private void DayUpdateCallbaclk(GameObject arg1, int arg2)
    {

        //Debug.LogError("index " + arg2 + " obj name " + arg1.name + "  _month " + _day[arg2]);
        arg1.transform.GetText("Text").text = _day[arg2].ToString();
    }
    private void MonthCursorCallbaclk( int arg2)
    {  
        _monthTxt.text = _month[arg2].ToString() ;
    }

    private void MonthUpdateCallbaclk(GameObject arg1, int arg2)
    {
      //  Debug.LogError("index " +arg2+" obj name "+arg1.name + "  _month "+ _month[arg2]); 
        arg1.transform.GetText("Text").text = _month[arg2].ToString();
    }



    private void OnCreateUser()
	{
		if (_inputField.text==string.Empty)
		{
			FlowText.ShowMessage(I18NManager.Get("CreateUser_NameHint"));
			return;
		}

		if (_dayTxt.text==string.Empty||_monthTxt.text==string.Empty)
        {
            FlowText.ShowMessage(I18NManager.Get("CreateUser_BirthdayNoSet"));
            return;
        }
		
		SendMessage(new Message(MessageConst.MODULE_CREATE_BIRTHDAY, Message.MessageReciverType.DEFAULT, _monthTxt.text, _dayTxt.text));
		//触发事件
		EventDispatcher.TriggerEvent(EventConst.CreateRoleSubmit, _inputField.text);
	}

	private void InputFieldValueChange(string str)
	{
		_inputField.text = Util.RegexString1(Util.GetNoBreakingString(str));
	}

	private void Update()
	{
		//string[] arr = _datePicker.GetDate();
		//_monthTxt.text = arr[0];
		//_dayTxt.text = arr[1];
	}

	protected override void OnClickOutside(GameObject go)
	{
		
	}

	
}
