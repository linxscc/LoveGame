using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class FavorabilityNpcInfo
{
   public string Name;
   public string CV;
   public string Birthday;
   public string Constellation;
   public string Height;
   public string Weight;
   public string BloodType;
   public string Interest;
   public string Hobby;
   public string LikeFood;
   public string MoreInfo;
   public int NpcId;
   public string BgPath;
   public string VoiceId; 
   public string BtnPath;
   

   public FavorabilityNpcInfo Clone()
   {
      FavorabilityNpcInfo info =new FavorabilityNpcInfo();
      info.Name = Name;
      info.CV = CV;
      info.Birthday = Birthday;
      info.Constellation = Constellation;
      info.Height = Height;
      info.Weight = Weight;
      info.BloodType = BloodType;
      info.Interest = Interest;
      info.Hobby = Hobby;
      info.LikeFood = LikeFood;
      info.MoreInfo = MoreInfo;
      info.NpcId = NpcId;
      info.BgPath = BgPath;
      info.VoiceId = VoiceId;   
      info.BtnPath = BtnPath;
      return info;
   }
}
