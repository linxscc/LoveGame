using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Componets
{
    public class BackBtnComponent : MonoBehaviour
    {
        public delegate void BackClickDelegate();

        public BackClickDelegate OnBackClick;

        public void Start()
        {
            gameObject.AddComponent<ButtonSound>().SoundName = "icon_out";
            GetComponent<Button>().onClick.AddListener(delegate()
            {
                OnBackClick.Invoke();
            });
        }
    }
}