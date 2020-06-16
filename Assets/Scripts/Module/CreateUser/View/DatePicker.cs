using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class DatePicker : MonoBehaviour
{
    private Transform _monthContent;

    private GameObject[] monthList;
    private GameObject[] dayList;

    private UIVerticalScroller _monthsVerticalScroller;
    private Transform _dayContent;
    private UIVerticalScroller _dayVerticalScroller;

    private void Awake()
    {
        _monthContent = transform.Find("MonthContainer/MonthScrollView/Content");
        RectTransform monthCenter = transform.Find("MonthContainer/MonthCenter").GetComponent<RectTransform>();

        _dayContent = transform.Find("DayContainer/DayScrollView/Content");
        RectTransform dayCenter = transform.Find("DayContainer/DayCenter").GetComponent<RectTransform>();

        InitMonth();
        InitDay();

        _monthsVerticalScroller = new UIVerticalScroller(_monthContent.GetComponent<RectTransform>(), monthList, monthCenter);
        _dayVerticalScroller = new UIVerticalScroller(_dayContent.GetComponent<RectTransform>(), dayList, dayCenter);

        _monthsVerticalScroller.StartingIndex = 0;
        _dayVerticalScroller.StartingIndex = 1;
        
        _monthsVerticalScroller.Start();
        _dayVerticalScroller.Start();

        
//        
//        transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
//        {
//            string dayString = _dayVerticalScroller.GetResults();
//            string monthString = _monthsVerticalScroller.GetResults();
//            
//            Debug.LogError(monthString);
//            Debug.LogError(dayString);
//        });

    }

    public string[] GetDate()
    {
        string dayString = _dayVerticalScroller.GetResults();
        string monthString = _monthsVerticalScroller.GetResults();

        return new[] {monthString, dayString};
    }

    private void InitDay()
    {
        dayList = new GameObject[31];

        GameObject monthItem = ResourceManager.Load<GameObject>("Module/CreateUser/Prefabs/DatePickerItem");

        for (int i = 0; i < dayList.Length; i++)
        {
            dayList[i] = Instantiate(monthItem, new Vector3(0, i * 200, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
            dayList[i].transform.SetParent(_dayContent, false);
            dayList[i].transform.localScale = new Vector3(1, 1, 1);
            dayList[i].transform.Find("Text").GetComponent<Text>().text = (dayList.Length - i).ToString();
        }
    }

    private void InitMonth()
    {
        monthList = new GameObject[12];

        GameObject monthItem = ResourceManager.Load<GameObject>("Module/CreateUser/Prefabs/DatePickerItem");

        for (int i = 0; i < monthList.Length; i++)
        {
            monthList[i] = Instantiate(monthItem, new Vector3(0, i * 200, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
            monthList[i].transform.SetParent(_monthContent, false);
            monthList[i].transform.localScale = new Vector3(1, 1, 1);
            monthList[i].transform.Find("Text").GetComponent<Text>().text = (monthList.Length - i).ToString();
        }
    }

    private void Update()
    {
        _monthsVerticalScroller.Update();
        _dayVerticalScroller.Update();
    }
}