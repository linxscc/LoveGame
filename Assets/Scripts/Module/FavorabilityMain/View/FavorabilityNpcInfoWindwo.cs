using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class FavorabilityNpcInfoWindwo : Window
{
   private Text _title;
   private Text _content;


   private void Awake()
   {
      _title = transform.GetText("Title/Text");
      _content = transform.GetText("ContentRect/Viewport/Content");
   }


   public void SetData(string title,string content)
   {
      _title.text = title;
      _content.text = content;
   }
}
