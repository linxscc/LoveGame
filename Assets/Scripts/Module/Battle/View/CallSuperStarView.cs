using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CallSuperStarView : View
{
    private RawImage _image1;
    private RawImage _image2;

    private static readonly int[] Ids = new int[]{4008,2009,1006,3007};
    private Coroutine _delayCallCoroutine;

    private void Awake()
    {
        transform.Find("OkBtn").GetComponent<Button>().onClick.AddListener(delegate()
        {
            SendMessage(new Message(MessageConst.CMD_BATTLE_NEXT));
        });

        _image1 = transform.Find("Image1").GetComponent<RawImage>();
//        _image2 = transform.Find("Image2").GetComponent<RawImage>();
//        _image2.gameObject.Hide();
//
//        ShowNext();
    }

    private void ShowNext()
    {
        int index = Random.Range(0, 4);
        if (_image1.gameObject.activeSelf == false)
        {
            _image1.texture = ResourceManager.Load<Texture>("Card/Image/MiddleCard/" + Ids[index], ModuleName);
            _image1.gameObject.Show();
            _image2.gameObject.Hide();

            _image1.color = new Color(1,1,1,0.5f);
            _image1.DOColor(Color.white, 0.5f);
        }
        else
        {
            _image2.texture = ResourceManager.Load<Texture>("Card/Image/MiddleCard/" + Ids[index], ModuleName);
            _image1.gameObject.Hide();
            _image2.gameObject.Show();
            
            _image2.color = new Color(1,1,1,0.5f);
            _image2.DOColor(Color.white, 0.5f);
        }
        _delayCallCoroutine = ClientTimer.Instance.DelayCall(ShowNext, 4);
    }

    public void SetData(LevelVo level)
    {
        transform.Find("Text").GetComponent<Text>().text = level.HeroDescription;
       
    }

    private void OnDestroy()
    {
        ClientTimer.Instance.CancelDelayCall(_delayCallCoroutine);
    }
}
