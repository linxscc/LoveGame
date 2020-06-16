using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MonthState
{
    Previous,
    Current,
    Next
}
public enum ShowTypes
{
    Five,//五行日历逻辑
    Six,//六行日历逻辑
}

namespace DataModel
{


    public class LoveDiaryModel : Model
    {
        public static List<Color> TextSelectColors = new List<Color>()
        {
            Color.black,
            Color.red,         
            new Color(0,0.5f,0),//浅蓝  //Color.green,
            Color.blue,
            Color.yellow,
            new Color(0,1,1),//浅蓝
            new Color(0.5f,0,0.5f),
        };

        public static List<string> TextSelectColors2 = new List<string>()
        {
            "000000FF",
            "FF0000FF",
            "008000FF",//浅蓝  //Color.green,
            "0000FFFF",
            "FFFF02FF",
            "02FFFFFF",//浅蓝
            "F200F2FF",
        };

        public static int GetIndexByColor(Color color)
        {
            int idex = 0;
            Color c;
            for (int i=0;i< TextSelectColors.Count;i++ )
            {
                c = TextSelectColors[i];
                if (Math.Abs( c.r - color.r)<0.005 && Math.Abs( c.g -color.g )< 0.005 && Math.Abs(c.b - color.b) < 0.005)
                    return i;
            }
            return idex;
        }


        //private Color _curTextColor;
        //public Color CurTextColor
        //{
        //    set
        //    {
        //        _curTextColor = value;
        //    }
        //    get{
        //        return _curTextColor;
        //    }
        //}

        Dictionary<int, CalendarVo> _calendarDic;//key=dt.Year * 10000 + dt.Month * 100 + dt.Day;
        List<int> _curYearMonths;//当前已经获得数据的年月data= dt.Year * 100 + dt.Month
        Dictionary<int, CalendarDetailVo> _calendarDetailDic;//key=dt.Year * 10000 + dt.Month * 100 + dt.Day;

        public DateTime UnlockDt;


        public LoveDiaryModel()
        {
            _calendarDic = new Dictionary<int, CalendarVo>();
            _calendarDetailDic = new Dictionary<int, CalendarDetailVo>();
            _curYearMonths = new List<int>();
        }

        public bool CheckHasDetailData(DateTime Dt)
        {
            DateTime dt = Dt;
            int key = dt.Year * 10000 + dt.Month * 100 + dt.Day;
            return _calendarDetailDic.ContainsKey(key);
        }

        public bool CheckHasDetailData(int Year, int Month, int Day)
        {
            int key = Year * 10000 + Month * 100 + Day;
            return _calendarDetailDic.ContainsKey(key);
        }

        public CalendarDetailVo GetCalendarDetailData(DateTime Dt)
        {
            DateTime dt = Dt;
            int key = dt.Year * 10000 + dt.Month * 100 + dt.Day;
            return _calendarDetailDic[key];
        }
        public void AddCalendarDetailData(DateTime Dt, CalendarDetailVo vo)
        {
            DateTime dt = Dt;
            int key = dt.Year * 10000 + dt.Month * 100 + dt.Day;
            _calendarDetailDic[key] = vo;
        }
        public void AddCalendarDetailData(int Year, int Month, int Day, CalendarDetailVo vo)
        {
            int key = Year * 10000 + Month * 100 + Day;
            _calendarDetailDic[key] = vo;
        }

        /// <summary>
        /// 检查是否有年月数据
        /// </summary>
        /// <returns></returns>
        public bool CheckHasData(DateTime Dt)
        {
            DateTime dt = Dt;
            int key = dt.Year * 100 + dt.Month;
            if (!_curYearMonths.Contains(key))
            { return false; }

            dt = dt.AddMonths(-1);
            key = dt.Year * 100 + dt.Month;

            if (!_curYearMonths.Contains(key))
            { return false; }

            return true;
        }

        public void AddCalendarYearMonth(int data)
        {
            //if (_curYearMonths.Contains(data))
            //    return;
            _curYearMonths.Add(data);
        }

         /// <summary>
         /// 判断是否当天日记有记录
         /// </summary>
         /// <param name="Dt"></param>
         /// <returns></returns>
        public bool CheckHasCalendarData(DateTime Dt)
        {
            int findKey = Dt.Year * 10000 + Dt.Month * 100 + Dt.Day;
            return _calendarDic.ContainsKey(findKey);
 
        }

        public void AddCalendarData(UserDiaryDatePB pb)
        {
            CalendarVo vo = new CalendarVo(pb);
            _calendarDic[vo.ModelKey] = vo;
        }

        public void AddCalendarData(int year, int month, int day)
        {
            CalendarVo vo = new CalendarVo(year,month,day);
            _calendarDic[vo.ModelKey] = vo;
        }

        /// <summary>
        /// 获取当月对应的四十二天的数据
        /// </summary>
        public List<CalendarVo> GetCalendarDatas(List<DateTime> dts)
        {
            List<CalendarVo> list = new List<CalendarVo>();
            for (int i=0;i<dts.Count;i++)
            {
                int findKey = dts[i].Year * 10000 + dts[i].Month * 100 + dts[i].Day;
                if (_calendarDic.ContainsKey(findKey))
                {
                    list.Add(_calendarDic[findKey]);
                }
            }
            return list;
        }


        public const int maxShowNum = 35;
        public static List<DateTime> ToDays(DateTime month)
        {
            List<DateTime> days = new List<DateTime>();
            DateTime firstDay = new DateTime(month.Year, month.Month, 1);
            DayOfWeek week = firstDay.DayOfWeek;

            int  daysNum = DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
            DateTime nextMonth = firstDay.AddMonths(1);
            int lastMonthDays = (int)week;
            //if (lastMonthDays.Equals(0))
            //    lastMonthDays = 0;

            if (daysNum>28)//一个月大于28天
            {
                int outNum = daysNum - 28;
                if (lastMonthDays+outNum>7)
                {
                    firstDay= firstDay.AddDays(7);
                }
            }
        
            for (int i = lastMonthDays; i > 0; i--)
                days.Add(firstDay.AddDays(-i));
            for (int i = 0; i < maxShowNum - lastMonthDays; i++)
                days.Add(firstDay.AddDays(i));
            return days;
        }

        public static bool IsFiveLine(DateTime month)
        {
            DateTime firstDay = new DateTime(month.Year, month.Month, 1);
            int daysNum = DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
            DayOfWeek week = firstDay.DayOfWeek;
            int lastMonthDays = (int)week;
            if (daysNum > 28)//一个月大于28天
            {
                int outNum = daysNum - 28;
                if (lastMonthDays + outNum > 7)
                {
                    return false;
                }
            }
            return true;
        }
        
        public static int DateTime2YMD(DateTime dt)
        {
            return dt.Year * 10000 + dt.Month * 100 + dt.Day;
        }
        //public int GetDiarySvrId(DateTime dt)
        //{
        //    return _calendarDic[DateTime2YMD(dt)].Id; 
        //}


    }
}