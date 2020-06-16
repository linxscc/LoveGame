using System;


[Serializable]
public class CoaxSleepAudioDescJsonData
{
    public int AudioId;
    public float Time;    
    public string Desc;   
    public CoaxSleepAudioDescJsonData Clone()
    {
        CoaxSleepAudioDescJsonData data = new CoaxSleepAudioDescJsonData();
        data.AudioId = AudioId;
        data.Time = Time;      
        data.Desc = Desc;      
        return data;
    }

}
