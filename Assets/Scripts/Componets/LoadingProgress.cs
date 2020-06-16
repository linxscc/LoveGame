using System;
using game.main;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadingProgress : MonoBehaviour {

    private static LoadingProgress _instance;

    public static LoadingProgress Instance { get { return _instance; } }

    public int speed;
    ProgressBar _progressBar;

    private RectTransform _smallStar;
    private RectTransform _bigStar;
    //private RawImage _bg;
    private RectTransform _starRectTra;
    private RectTransform _mask;
    private Text _downloadText;
    private Transform _downloadtran;
    public Transform AlertWindow; 
    public Text Title; 
    public Text OkText; 
    public Text Content; 
    public Button okBtn;

    public int HasDownloadCount = 0;
    private int PackCount=0;
    private List<int> roleIds;
    
    private double _totalSize;
    private Transform _bgRoot;
    private int aniIndex = 0;
    
    public double TotalSize
    {
        get { return _totalSize; }
        set
        {
            //Debug.LogError("_totalSize"+value);
            _totalSize = value;
            if (TotalSize > 0)
            {
                ClientTimer.Instance.DelayCall(() => { _downloadtran.gameObject.SetActive(true); }, 0.6f);
            }
            else
            {
                _downloadtran.gameObject.SetActive(false);
            } 
        }
    }


    private void Awake()
    {
        roleIds=new List<int>();
        int firstrole = Random.Range(1, 5);
        roleIds.Add(firstrole);
        //Debug.LogError(firstrole);
        for (int i = 1; i < 4; i++)
        {
            int rid = firstrole + i;
            if (rid>4)
            {
                rid = rid-4;
            }
            roleIds.Add(rid);

        }

        _bgRoot = transform.Find("bgRoot");
        
        _instance = this;
        _progressBar = transform.Find("ProgressBar").GetComponent<ProgressBar>();

//        Color color = transform.Find("Bg").GetComponent<RawImage>().color;
//        color.a = 1;
//        _bg = transform.Find("Bg").GetComponent<RawImage>();
//        _bg.color = color;
        Hide();
        
        _starRectTra = transform.Find("ProgressBar/Star").GetComponent<RectTransform>();
        _mask = transform.Find("ProgressBar/Mask").GetComponent<RectTransform>();
        
         _smallStar = transform.Find("ProgressBar/Star/smallStar").GetComponent<RectTransform>();
         _bigStar = transform.Find("ProgressBar/Star/bigStar").GetComponent<RectTransform>();
        _downloadtran = transform.Find("DownloadText");
        _downloadText = transform.Find("DownloadText/progressText").GetText();
        AlertWindow = transform.Find("AlertWindow");
        Title = AlertWindow.Find("Title/Text").GetText();
        OkText = AlertWindow.Find("okBtn/Text").GetText();
        Content = AlertWindow.Find("contentText").GetText();
        okBtn = AlertWindow.Find("okBtn").GetButton();
    }

    private void Start()
    {
        _progressBar.DeltaX = 0;
        _progressBar.Progress = 0;
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            _smallStar.Rotate(-Vector3.forward*Time.deltaTime *500.0f);
            _bigStar.Rotate(-Vector3.forward *Time.deltaTime *500.0f);
            _starRectTra.localPosition = new Vector3(_mask.GetWidth(), _starRectTra.localPosition.y, 0);
        }
    }

    public void SetBg()
    {
        for (int i = 0; i < _bgRoot.childCount; i++)
        {
            var bg=_bgRoot.GetChild(i).GetComponent<RawImage>();
//            Debug.LogError(roleIds[i]+1);
            Texture tex=ResourceManager.Load<Texture>("Background/download/" +roleIds[i]);
            bg.texture = tex;
            Color color = bg.color;
            color.a = i==0 ? 1 : 0;
            bg.color = color;
        }

        ChangeRoleAni();
    }
    
    public void SetPercent(float per,bool isUnzip=false)
    {
        _progressBar.Progress = (int)per;
        if (TotalSize>0)
        {
            if (isUnzip)
            {
                _downloadText.text = I18NManager.Get("Download_UnzipProgress",per.ToString("f2")); //$"资源正在解压{per.ToString("f2")}%\n"; 
            }
            else
            {
                if (PackCount>HasDownloadCount)
                {
                    _downloadText.text = I18NManager.Get("Download_AssetDownloadnum",per.ToString("f2"),HasDownloadCount,PackCount,(per*0.01*TotalSize).ToString("f2"),TotalSize);
                    //$"资源下载进度{per.ToString("f2")}%(已下载资源数({HasDownloadCount}/{PackCount}))\n正在下载{(per*0.01*TotalSize).ToString("f2")}MB/{TotalSize}MB";
                }
                else
                {
                    _downloadText.text = I18NManager.Get("Download_AssetDownloadnum2",per.ToString("f2"),(per*0.01*TotalSize).ToString("f2"),TotalSize);
                    //$"资源下载进度{per.ToString("f2")}%\n正在下载{(per*0.01*TotalSize).ToString("f2")}MB/{TotalSize}MB";
                }

            }

        }
 
    }

    public void AddPackCount(int count)
    {
        PackCount += count;
        Debug.LogError(PackCount);
    }

    public void ClearPackCount()
    {
        PackCount = 0;
        HasDownloadCount = 0;
    }
    
    public void ShowAlertWindw(bool show,string content="",string title=null, string okBtnText = null,Action action=null)
    {
        if (title == null)
        {
            title = I18NManager.Get("Common_Hint");
        }
        if (okBtnText == null)
        {
            okBtnText = I18NManager.Get("Common_OK2");
        }
        AlertWindow.gameObject.SetActive(show);
        Content.text = content;
        Title.text = title;
        OkText.text = okBtnText;
        okBtn.onClick.AddListener(() =>
        {
            AlertWindow.gameObject.SetActive(false);
            action?.Invoke();
        }); 
    }

    public void Show(long size=0)
    {
        gameObject.Show();
        TotalSize = Math.Round(size * 1f / 1048576f, 2);
        if (TotalSize > 0)
        {
            ClientTimer.Instance.DelayCall(() => { _downloadtran.gameObject.SetActive(true); }, 0.6f);
        }
        else
        {
            _downloadtran.gameObject.SetActive(false);
        } 
        
    }
    
    public void Showview(bool enable=false)
    {
        _downloadText.text = I18NManager.Get("Download_CheckFiles");
        gameObject.Show();
        _downloadtran.gameObject.SetActive(enable);
    }

    public void SetDownloadText(string text)
    {
        _downloadText.text = text;
    }

    #region 角色背景切换动画

    public void ChangeRoleAni()
    {
        var curBg=  _bgRoot.GetChild(aniIndex).GetComponent<RawImage>();
        RawImage nextBg;

             
        if (aniIndex+1==4)
        {
            nextBg =_bgRoot.GetChild(0).GetComponent<RawImage>();
        }
        else
        {
            nextBg =_bgRoot.GetChild(aniIndex+1).GetComponent<RawImage>();
        }
                
        Tween curBgAlpha = curBg.DOColor(new Color(curBg.color.r,curBg.color.g,curBg.color.b,0),1f );
        Tween nextBgAlpha = nextBg.DOColor(new Color(nextBg.color.r,nextBg.color.g,nextBg.color.b,1),1f );
            


        Sequence tween = DOTween.Sequence()
            .Join(curBgAlpha)
            .Join(nextBgAlpha) ;
            
        tween.onComplete = () =>
        {
            aniIndex++;              
            ClientTimer.Instance.DelayCall(ChangeRoleAni, 5f);

            if (aniIndex==4)
            {
                aniIndex = 0;
            }          
        };   
        
        
        
    }
    

    #endregion
    

    public void Hide()
    {
        gameObject.Hide();
    }
}
