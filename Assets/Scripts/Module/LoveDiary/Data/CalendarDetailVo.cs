using Com.Proto;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.main
{
    public class DiaryElementCount
    {
        public int MaxImageCount = 0;
        public int CurImageCount = 0;
        public int MaxRacketCount = 0;
        public int CurRacketCount = 0;
        public int MaxTextCount = 0;
        public int CurTextCount = 0;

        public bool IsUpperLimited(ElementTypePB typePb)
        {
            int max=0;
            int cur=0;
            switch(typePb)
            {
                case ElementTypePB.Image:
                    max = MaxImageCount;
                    cur = CurImageCount;
                    break;
                case ElementTypePB.Text:
                    max = MaxTextCount;
                    cur = CurTextCount;
                    break;
                case ElementTypePB.Racket:
                    max = MaxRacketCount;
                    cur = CurRacketCount;
                    break;
                default:
                    return false;
            }
            return cur >= max;
        }

        public void AddCount(ElementTypePB typePb)
        {
            switch (typePb)
            {
                case ElementTypePB.Image:
                    CurImageCount++;
                    break;
                case ElementTypePB.Text:
                    CurTextCount++;
                    break;
                case ElementTypePB.Racket:
                    CurRacketCount++;
                    break;
            }
        }
        public void SubCount(ElementTypePB typePb)
        {
            switch (typePb)
            {
                case ElementTypePB.Image:
                    CurImageCount--;
                    break;
                case ElementTypePB.Text:
                    CurTextCount--;
                    break;
                case ElementTypePB.Racket:
                    CurRacketCount--;
                    break;
            }
        }

    }
    public class CalendarDetailVo 
    {
        public int Year {
            set; get;
        }//哪天
        public int Month;//哪天
        public int Day;//哪天
        public int Id {
            set;get;
        }//对应的日记id 新日记ID为0;

        public DiaryElementCount CurDiaryElementCount = new DiaryElementCount();

        public List<DiaryElementPB> DiaryElements
        {
            get;
            set;
        }

        DiaryElementPB _labelElement;
        public DiaryElementPB LabelElement
        {
            set
            {
                if (_labelElement == value)
                {
                    return;
                }
                _labelElement = value;
            }
            get
            {
                if(_labelElement!=null)
                {
                    return _labelElement;
                }

                for (int i = 0; i < DiaryElements.Count; i++)
                {
                    ElementPB pb = GetElementRuleById(DiaryElements[i].ElementId);
                    if (pb.ElementType==ElementTypePB.Label)
                    {
                        _labelElement = DiaryElements[i];
                        return _labelElement;
                    }
                }
                _labelElement = new DiaryElementPB();
                _labelElement.ElementId = -1;
                return _labelElement;

            }
        }

        public CalendarDetailVo(int year, int month, int day, List<DiaryElementPB> diaryElementPBs = null)
        {
         
            DiaryElements = new List<DiaryElementPB>();
            Year = year;
            Month = month;
            Day = day;
            if(diaryElementPBs!=null)
                DiaryElements.AddRange(diaryElementPBs);
        }

        public static ElementPB  GetElementRuleById(int id)
        {
            return GlobalData.DiaryElementModel.GetElementRuleById(id);
        }

        public void FindRuleByType()
        {

        }
    }
}