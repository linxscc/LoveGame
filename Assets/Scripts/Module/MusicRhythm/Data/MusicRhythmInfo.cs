using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRhythmInfo
{
    public float Speed = 0;
    public List<Tick> Ticks;
    //MusicRhythmTickInfo tickInfo;
    public MusicRhythmInfo()
    {
      //  tickInfo = new MusicRhythmTickInfo();
        Ticks = new List<Tick>();
    }

}

//public class MusicRhythmTickInfo
//{ 
//    public MusicRhythmTickInfo()
//    {       
//    }
//}

public class Tick : IComparable<Tick>
{
    public float TickTime;//打击点时间
    public int Way;//音轨1，2，3，
    public int TickType;//打击类型1 单击，2 长按
    public float Duration;//持续的时间

    public int CompareTo(Tick other)
    {
        return TickTime.CompareTo(other.TickTime);
    }
}
