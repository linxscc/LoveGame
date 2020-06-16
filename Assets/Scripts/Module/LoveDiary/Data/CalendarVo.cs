using Com.Proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.main
{
    public class CalendarVo
    {
        public int Year;//哪天
        public int Month;//哪天
        public int Day;//哪天
        //public int Id {
        //    set;get;
        //}//对应的日记id 新日记ID为;


        public CalendarVo(UserDiaryDatePB pb)
        {
            Year = pb.Year;
            Month = pb.Month;
            Day = pb.Date;
           // Id = pb.Id;
        }

        public CalendarVo(int year, int month,int day)
        {
            Year = year;
            Month = month;
            Day = day;
         //   Id = id;
        }

        public int ModelKey
        {
            get
            {
                return Year * 10000 + Month * 100 + Day;
            }
        }
    }
}