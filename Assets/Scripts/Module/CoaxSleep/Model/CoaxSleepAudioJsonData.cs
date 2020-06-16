using System;


[Serializable]
public class CoaxSleepAudioJsonData
{
   public int AudioId; 
   public string MD5;
   public string Path;
   public long Size;
   public CoaxSleepAudioJsonData Clone()
   {
      CoaxSleepAudioJsonData data =new CoaxSleepAudioJsonData();
      data.AudioId = AudioId;
      data.MD5 = MD5;
      data.Path = Path;     
      data.Size = Size;
      return data;
   }
   
   
   
   

}
