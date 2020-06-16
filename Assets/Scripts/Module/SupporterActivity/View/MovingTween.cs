using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MovingTween : MonoBehaviour
{

    public GameObject Road;
    public GameObject City;
    public GameObject Sky;

    private Material _road;
    private Material _city;
    private Material _sky;


    private RawImage _roadRawImage;
    private RawImage _cityRawImage;
    private RawImage _skyRawImage;

    public float roadduration=2f;
    public float cityduration=8f;
    public float skyduration=16f;

    private float _roadvalue = 0f;
    private float _cityvalue = 0f;
    private float _skyvalue = 0f;
    public Tweener roadmoving;
    public Tweener citymoving;
    public Tweener skymoving;

    public Animator _busAni;

//    public bool isPase=false;
//    private bool state=false;
    

    private void Awake()
    {
//        _road = Road.GetComponent<Image>().material;
//        _city = City.GetComponent<Image>().material;
//        _sky = Sky.GetComponent<Image>().material;
//        StartTween();

        _roadRawImage = Road.GetComponent<RawImage>();
        _cityRawImage = City.GetComponent<RawImage>();
        _skyRawImage = Sky.GetComponent<RawImage>();
        _roadvalue = _roadRawImage.uvRect.x;
        _cityvalue = _cityRawImage.uvRect.x;
        _skyvalue = _skyRawImage.uvRect.x;
        StartRawImageTween();
        //PaseTween();
    }

    public void StartRawImageTween()
    {
        _busAni.enabled = true;
        _roadRawImage.uvRect=new Rect(0,0,1,1); 
        _cityRawImage.uvRect=new Rect(0,0,1,1); 
        _skyRawImage.uvRect=new Rect(0,0,1,1); 
        
        
        roadmoving= DOTween.To(()=>_roadRawImage.uvRect,x=>_roadRawImage.uvRect=x,new Rect(-1,0,1,1), roadduration);
        roadmoving.SetAutoKill(false);
        roadmoving.SetEase(Ease.Linear).SetLoops(-1,LoopType.Restart);//循环播放
        
        citymoving=DOTween.To(()=>_cityRawImage.uvRect,x=>_cityRawImage.uvRect=x,new Rect(-1,0,1,1),cityduration);
        citymoving.SetAutoKill(false);
        citymoving.SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);//循环播放

        skymoving= DOTween.To(()=>_skyRawImage.uvRect,x=>_skyRawImage.uvRect=x,new Rect(-1,0,1,1),skyduration);
        skymoving.SetAutoKill(false);
        skymoving.SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);//循环播放     
        
    }

    public void PaseTween()
    {
        //DOTween.Pause(this);
       // Debug.LogError("PaseTween");
        roadmoving.Pause();
        citymoving.Pause();
        skymoving.Pause();
        _busAni.enabled = false;

    }

    public void PlayTween()
    {
        //DOTween.Play(this);
     //   Debug.LogError("PlayTween");
        roadmoving.Play();
        citymoving.Play();
        skymoving.Play();
        _busAni.enabled = true;  
    }

//    private void Update()
//    {
////        _roadRawImage.uvRect=new Rect(_roadvalue,0,1,1);
////        _cityRawImage.uvRect=new Rect(_cityvalue,0,1,1);
////        _skyRawImage.uvRect=new Rect(_skyvalue,0,1,1);
//        if (isPase)
//        {
//            if (state==false)
//            {
//                state = true;
//                PaseTween(); 
//            }
//
//        }
//        else
//        {
//            if (state)
//            {
//                state = false;
//                StartRawImageTween();
//            } 
//        }
//    }
//
//    private void StartTween()
//    {
//        _road.SetTextureOffset("_MainTex",new Vector2(0,0));
//        _city.SetTextureOffset("_MainTex",new Vector2(0,0));
//        _sky.SetTextureOffset("_MainTex",new Vector2(0,0));
//
//        Tweener roadmoving = _road.DOOffset(new Vector2(-2f, 0), roadduration);
//        roadmoving.SetAutoKill(false);
//        roadmoving.SetEase(Ease.Linear).SetLoops(-1,LoopType.Restart);//循环播放
//        
//        Tweener citymoving=_city.DOOffset(new Vector2(-2f, 0), cityduration);
//        citymoving.SetAutoKill(false);
//        citymoving.SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);//循环播放
//
//        Tweener skymoving = _sky.DOOffset(new Vector2(-2f, 0), skyduration);
//        skymoving.SetAutoKill(false);
//        skymoving.SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);//循环播放
//
//    }
}
