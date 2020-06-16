using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Module.Framework.Utils
{
    public class DateUtil
    {
        /// <summary>
        /// 毫秒转换为xx天xx小时xx分xx秒格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetTimeFormat(long time)
        {
            string result = "";
            int t = (int) time / 1000; //转换为秒
            int day, hour, minute, second;
//        string dayStr = LanguageService.Instance.GetStringByKey("Common/Day");
//        string hourStr = LanguageService.Instance.GetStringByKey("Common/hour");
//        string minuteStr = LanguageService.Instance.GetStringByKey("Common/minute");
//        string secondStr = LanguageService.Instance.GetStringByKey("Common/second");

            string dayStr = I18NManager.Get("Common_Hint7");
            string hourStr = I18NManager.Get("Common_Hint8");
            string minuteStr =  I18NManager.Get("Common_Hint9");
            string secondStr =I18NManager.Get("Common_Hint10");

            if (t >= 86400) //天,
            {
                day = Convert.ToInt16(t / 86400);
                hour = Convert.ToInt16((t % 86400) / 3600);
                minute = Convert.ToInt16((t % 86400 % 3600) / 60);
                second = Convert.ToInt16(t % 86400 % 3600 % 60);
                result = day + dayStr + (hour > 0 ? hour + hourStr : "")
                         + (minute > 0 ? minute + minuteStr : "")
                         + (second > 0 ? second + secondStr : "");
            }
            else if (t >= 3600) //时,
            {
                hour = Convert.ToInt16(t / 3600);
                minute = Convert.ToInt16((t % 3600) / 60);
                second = Convert.ToInt16(t % 3600 % 60);
                result = hour + hourStr
                              + (minute > 0 ? minute + minuteStr : "")
                              + (second > 0 ? second + secondStr : "");
            }
            else if (t >= 60) //分
            {
                minute = Convert.ToInt16(t / 60);
                second = Convert.ToInt16(t % 60);
                result = minute + minuteStr
                                + (second > 0 ? second + secondStr : "");
            }
            else
            {
                second = Convert.ToInt16(t);
                result = second + secondStr;
            }

            return result;
        }
        
        /// <summary>
        /// 好友登录时间专用的
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetTimeFormatMinute(long time)
        {
            string result = "";
            int t = (int)(time / 1000); //转换为秒
            int day, hour, minute, second;
//        string dayStr = LanguageService.Instance.GetStringByKey("Common/Day");
//        string hourStr = LanguageService.Instance.GetStringByKey("Common/hour");
//        string minuteStr = LanguageService.Instance.GetStringByKey("Common/minute");
//        string secondStr = LanguageService.Instance.GetStringByKey("Common/second");

            string dayStr = I18NManager.Get("Common_Hint7");
            string hourStr = I18NManager.Get("Common_Hint8");
            string minuteStr = I18NManager.Get("Common_Hint9");
            string secondStr = I18NManager.Get("Common_Hint10");;

            if (t >= 86400) //天,
            {
                day = Convert.ToInt32(t / 86400);
                result = day + dayStr ;
            }
            else if (t >= 3600) //时,
            {
                hour = Convert.ToInt32(t / 3600);
                result = hour + hourStr;
            }
            else if (t >= 60) //分
            {
                minute = Convert.ToInt32(t / 60);
                result = minute + minuteStr;
            }
            else
            {
                second = Convert.ToInt32(t);
                result = second + secondStr;
            }

            return result;
        }
        

        public static string GetDay(long time)
        {
            string result = "";
            int t = (int) time / 1000; //转换为秒
            int day, hour;
            string dayStr =I18NManager.Get("Common_Hint7");  
            string hourStr = I18NManager.Get("Common_Hint8");
            if (t >= 86400) //天,
            {
                day = Convert.ToInt16(t / 86400);
                hour = Convert.ToInt16((t % 86400) / 3600);
                result = day + dayStr + (hour > 0 ? hour + hourStr : "");
            }
            else if (t >= 3600) //时,
            {
                hour = Convert.ToInt16(t / 3600);
                result = hour + hourStr;
            }

            return result;
        }
        
        /// <summary>
        /// 毫秒转换为xx天xx小时或者xx小时xx分或xx分xx秒格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetTimeFormat2(long time)
        {
            string result = "";
            int t = (int) time / 1000; //转换为秒
            int day, hour, minute, second;
            string dayStr = I18NManager.Get("Common_Hint7");
            string hourStr = I18NManager.Get("Common_Hint8");
            string minuteStr = I18NManager.Get("Common_Hint9");
            string secondStr = I18NManager.Get("Common_Hint10");
            if (t >= 86400) //天,
            {
                day = Convert.ToInt16(t / 86400);
                hour = Convert.ToInt16((t % 86400) / 3600);
                minute = Convert.ToInt16((t % 86400 % 3600) / 60);
                second = Convert.ToInt16(t % 86400 % 3600 % 60);
                result = day + dayStr + (hour > 0 ? hour + hourStr : "");
            }
            else if (t >= 3600) //时,
            {
                hour = Convert.ToInt16(t / 3600);
                minute = Convert.ToInt16((t % 3600) / 60);
                second = Convert.ToInt16(t % 3600 % 60);
                result = hour + hourStr
                              + (minute > 0 ? minute + minuteStr : "");
            }
            else if (t >= 60) //分
            {
                minute = Convert.ToInt16(t / 60);
                second = Convert.ToInt16(t % 60);
                result = minute + minuteStr
                                + (second > 0 ? second + secondStr : "");
            }
            else
            {
                second = Convert.ToInt16(t);
                result = second + secondStr;
            }

            return result;
        }

        public static string GetTimeFormat3(long time)
        {
            string result = "";
            int t = Convert.ToInt32(time / 1000); //转换为秒
            int day, hour, minute, second;
            string dayStr = I18NManager.Get("Common_Hint7");
            string hourStr = ":";
            string minuteStr = ":";
            string secondStr = "";
            if (t >= 86400) //天,
            {
                day = Convert.ToInt32(t / 86400);
                hour = Convert.ToInt32((t % 86400) / 3600);
                minute = Convert.ToInt32((t % 86400 % 3600) / 60);
                second = Convert.ToInt32(t % 86400 % 3600 % 60);
                result = day + dayStr + (hour > 0 ? hour + hourStr+minute+minuteStr+second : "");
            }
            else if (t >= 3600) //时,
            {
                hour = Convert.ToInt32(t / 3600);
                minute = Convert.ToInt32((t % 3600) / 60);
                second = Convert.ToInt32(t % 3600 % 60);
                result = hour + hourStr
                              + (minute > 0 ? minute + minuteStr : "")+second;
            }
            else if (t >= 60) //分
            {
                minute = Convert.ToInt32(t / 60);
                second = Convert.ToInt32(t % 60);
                string seStr;
                if (second < 10)
                {
                    seStr = "0" + second.ToString();
                }
                else
                {
                    seStr = second.ToString();
                }

                result = minute + minuteStr
                                + (second > 0 ? seStr + secondStr : "");
            }
            else
            {
                second = Convert.ToInt32(t);
                string seStr;
                if (second < 10)
                {
                    seStr = "0" + second.ToString();
                }
                else
                {
                    seStr = second.ToString();
                }

                result = "00" + minuteStr + seStr + secondStr;
            }

            return result;
        }

        public static string GetTimeFormat4(long lefTime)
        {
            long s = (lefTime / 1000) % 60;
            long m = (lefTime / (60 * 1000)) % 60;
            long h = lefTime / (60 * 60 * 1000);
            return string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
        }
        
        public static string GetYMD(long t)
        {
            long left = t;
            //System.DateTime dt = new System.DateTime(1970, 1, 1).AddMilliseconds(left);
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            System.DateTime dt = dateTimeStart.AddMilliseconds(left);
            string mat = string.Format("{0:yyyy.MM.dd}", dt);
            return mat;
        }

        /// <summary>
        /// 时间格式xx年xx月xx日xx:xx
        /// </summary>
        public static string GetYMDD(long t)
        {
            if (t <= 0) return string.Empty;
            long left = t;
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            System.DateTime dt = dateTimeStart.AddMilliseconds(left);
            string minuteStr = dt.Minute.ToString();
            if (dt.Minute == 0)
            {
                minuteStr = "00";
            }
            else if (dt.Minute < 10)
            {
                minuteStr = "0" + dt.Minute;
            }
            string mat = I18NManager.Get("Common_YMDD", dt, dt.Hour, minuteStr);
            return mat;
        }

        /// <summary>
        /// 获取时分
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetHM(long t)
        {
            long left = t;
            System.DateTime dt = new DateTime(1970, 1, 1).AddMilliseconds(left);            
            string mat = dt.Hour + "." + dt.Minute;
            return mat;
        }

        /// <summary>
        /// 剩余多少天
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int GetLeftDay(long time)
        {
  
            long t = time / 1000; //转换为秒
            int day;
            if (t >= 86400) //天,
            {
                day = Convert.ToInt32(t / 86400);

            }
            else
            {
                day=0;
            }

            return day;
        }


        public static DateTime GetDataTime(long t)
        {
            long left = t;
            return new DateTime(1970, 1, 1,8,0,0).AddMilliseconds(left);
        }

        public static long GetTimeStamp(DateTime t)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long unixTime = (long)System.Math.Round((t - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            return unixTime;
        }


       
        /// <summary>
        /// 获取剩余天数
        /// </summary>
        /// <param name="curTimeStamp">当前时间戳</param>
        /// <param name="endTimeStamp">结束时间戳</param>    
        public static int GetSurplusDay(long curTimeStamp,long endTimeStamp)
        {
            if (curTimeStamp>endTimeStamp)
            {
                Debug.LogError("curTimeStamp > endTimeStamp");
                return 0;
            }
            
            DateTime starDt = GetDataTime(curTimeStamp);
            DateTime endDt = GetDataTime(endTimeStamp);
            
            TimeSpan starTs =new TimeSpan(starDt.Ticks);
            TimeSpan endTs =new TimeSpan(endDt.Ticks);

            int residueDay = starTs.Subtract(endTs).Duration().Days;

            return residueDay;
        }
        
        
        /// <summary>
        /// 获取非时区的时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetNotTimezoneTimeStamp(DateTime t)
        {
          var starDT = new DateTime(1970,1,1,8,0,0);
          var timeStampOffset = (t - starDT).TotalMilliseconds;
          return Convert.ToInt64(timeStampOffset);
        }
        
        public static DateTime GetTodayDt()
        {
            DateTime dtStart = new DateTime(1970, 1, 1, 8, 0, 0);//
            //    DateTime dtStart1 = dtStart.AddHours(8);
            DateTime dtNow = dtStart.AddSeconds(ClientTimer.Instance.GetCurrentTimeStamp() / 1000);
            return dtNow;
        }

        /// <summary>
        /// before return -1;curday retrun 0;after 1;
        /// </summary>
        /// <returns></returns>
        public static int CheckIsToday(DateTime Dt)
        {
            DateTime dtNow = GetTodayDt();
            if (Dt.Year > dtNow.Year ||
            (Dt.Year == dtNow.Year && Dt.Month > dtNow.Month) ||
            (Dt.Year == dtNow.Year && Dt.Month == dtNow.Month && Dt.Day > dtNow.Day))
            {
                return 1;
            }
            if (Dt.Year == dtNow.Year && Dt.Month == dtNow.Month && Dt.Day == dtNow.Day)
            {
                return 0;
            }
            return -1;
        }
        /// <summary>
        /// before return -1;curday retrun 0;after 1;
        /// </summary>
        /// <returns></returns>
        public static int CompareToday(DateTime Dt1, DateTime Dt2)
        {
           // DateTime dtNow = GetTodayDt();
            if (Dt1.Year > Dt2.Year ||
            (Dt1.Year == Dt2.Year && Dt1.Month > Dt2.Month) ||
            (Dt1.Year == Dt2.Year && Dt1.Month == Dt2.Month && Dt1.Day > Dt2.Day))
            {
                return 1;
            }
            if (Dt1.Year == Dt2.Year && Dt1.Month == Dt2.Month && Dt1.Day == Dt2.Day)
            {
                return 0;
            }
            return -1;
        }
        
        /// <summary>
        /// before return -1;curday retrun 0;after 1;
        /// </summary>
        /// <returns></returns>
        public static int CheckNeedToRefresh(DateTime Dt1, DateTime Dt2)
        {
            // DateTime dtNow = GetTodayDt();
            if (Dt1.Year > Dt2.Year ||
                (Dt1.Year == Dt2.Year && Dt1.Month > Dt2.Month) ||
                (Dt1.Year == Dt2.Year && Dt1.Month == Dt2.Month && Dt1.Day > Dt2.Day&&Dt1.Hour>Dt2.Hour&&Dt1.Minute>Dt2.Minute))
            {
                return 1;
            }
            if (Dt1.Year == Dt2.Year && Dt1.Month == Dt2.Month && Dt1.Day == Dt2.Day)
            {
                return 0;
            }
            return -1;
        }


        /// <summary>
        /// 获取活动结束时间（6点）
        /// </summary>
        /// <param name="activityStartTimeTamp">活动开始的时间戳</param>
        /// <param name="activityContinueDay">活动持续的天数</param>
        /// <returns></returns>
        public static DateTime GetActivityOverTime(long activityStartTimeTamp,int activityContinueDay)
        {
            var activityStartDateTime = GetDataTime(activityStartTimeTamp);
            var point =new DateTime(activityStartDateTime.Year,activityStartDateTime.Month,activityStartDateTime.Day,6,0,0);
            var overPoint = point.AddDays(activityContinueDay);
            Debug.LogError("overDay===>"+overPoint.Day+"overPoint===>"+overPoint.Hour);
            return overPoint;
        }
    }
}