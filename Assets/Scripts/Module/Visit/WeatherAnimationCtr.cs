//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public enum WeatherCommonTransition
//{
//    None,
//    Any2Out,
//    Any2Enter,
//    Any2OutEnter,
//}


//public class WeatherAnimationCtr : MonoBehaviour {


//    public void Hide()
//    {
//        Debug.Log(gameObject.name);
//        gameObject.Hide();
//    }
//    public void OutEnd()
//    {
//        Debug.Log("OutEnd "+gameObject.name);
//        Animator fromAni = GetComponent<Animator>();
//        fromAni.SetInteger("State", 0);
//        gameObject.Hide();
//    }

//    public void StartChange()
//    {
//        Debug.Log("StartChange  "+ gameObject.name);

     
//        transform.parent.parent.GetComponent<WeatherView>().StartChange();

//    }

//    public void StartEnter()
//    {
//        Debug.Log("StartEnter "+ gameObject.name);
//        transform.parent.parent.GetComponent<WeatherView>().StartEnter();
//    }

//    public void EnterEnd()
//    {
//        Debug.Log("EnterEnd "+gameObject.name);
//        transform.parent.parent.GetComponent<WeatherView>().BlessingEnd();
//    }
//    public void StartAni()
//    {
//        Debug.Log("StartAni "+gameObject.name);
//        Animator toAni = GetComponent<Animator>();
//        toAni.SetInteger("State", 0);
//    }

//    public void ChangeAnimationEnd()
//    {
//        Debug.Log("ChangeAnimationEnd "+ gameObject.name);
//        gameObject.Hide();
//    }
//}
