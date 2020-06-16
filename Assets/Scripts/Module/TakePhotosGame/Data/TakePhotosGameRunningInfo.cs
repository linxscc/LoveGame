using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePhotosGameRunningInfo
{
    public int CurPhotoId;
    public Texture2D originTexture;
    public Texture2D targetTexture;
    public Texture2D playerTexture;
    public TakePhotoRulePB takePhotoRulePB;

    public Vector2 pos;
    public float scale;
    public int origenWidth = 1440;
    public int origenHight = 1920;
    public int targetWidth = 500;
    public int targeHight = 500;

    private List<TakePhotoGamePhotoVo> _photoVos;


    public List<TakePhotoGamePhotoVo> GetPhotosVo()
    {
        return _photoVos;
    }

    private int _curIndex = 0;

    public int GetCurPhotoOrder
    {
        get
        {
            return _curIndex;
        }
    }
    
    /// <summary>
    /// 获取当前照相信息
    /// </summary>
    /// <returns></returns>
    public TakePhotoGamePhotoVo GetCurPhotoVo()
    {
        if (_curIndex >= _photoVos.Count)
            return null;
        return _photoVos[_curIndex];
    }

    public int GetCurScore()
    {
        int score = 0;
        foreach(var v in _photoVos)
        {
            score += v.GetCurScore();
        }
        return score;
    }

    public void AddTakePhotoGamePhotoVo(TakePhotoGamePhotoVo vo)
    {
        _photoVos.Add(vo);
    }

    //转换为发送服务端的数据
    public List<TakePhotoScorePB> GetPhotoResultPb()
    {
        List<TakePhotoScorePB> list = new List<TakePhotoScorePB>();
        foreach(var v in _photoVos)
        {
            TakePhotoScorePB pb = new TakePhotoScorePB();
            pb.Id = v.Id;

            foreach(var v1 in v.GetStateVo())
            {
                PhotoResultPB resultPb = new PhotoResultPB();
                resultPb.Stage = ShowState2Proto(v1.showState);

                resultPb.Score = v1.score;
                pb.PrhotoResult.Add(resultPb);
            }
            list.Add(pb);
        }

        return list;
    }

    /// <summary>
    /// 传入准确率a
    /// </summary>
    /// <param name="state"></param>
    /// <param name="v"></param>
    public TakePhotoGameStateVo AddCurPhotoResult(TakePhotosGameShowState state, int a)
    {
        var vo = GetCurPhotoVo();
        if (vo == null) 
        {
            return null;
        }
        return vo.SetStateVo(state, a);
    }


    /// <summary>
    /// 判断拍照流程是否完成
    /// </summary>
    /// <returns></returns>
    public bool CheckFinished()
    {
        
        foreach(var v in _photoVos)
        {
            if (!v.isFinished)
                return false;
        }
        return true;
    }

    static public int ShowState2Proto(TakePhotosGameShowState state)
    {
        return (int)state - 1;
    }

    public TakePhotosGameRunningInfo( )
    {
        _photoVos = new List<TakePhotoGamePhotoVo>();
    }

    public void Init()
    {
        _curIndex = 0;
        if (_curIndex >= _photoVos.Count)
            return;
        var vo = GetCurPhotoVo();
        HandleTexture(vo);
    }

    public void DoNext()
    {
        _curIndex++;
        if (_curIndex >= _photoVos.Count)
            return;
        var vo = GetCurPhotoVo();
        HandleTexture(vo);
    }
    void HandleTexture(TakePhotoGamePhotoVo vo)
    {
        if (vo == null)
        { return; }
        var t = ResourceManager.Load<Texture>(vo.resoucePath);
        originTexture = Texture2Texture2D(t);
        targetTexture = GenerateTexture(originTexture, -vo.targetPos, targetWidth, targeHight, vo.scale);
    
        vo.targetTexture = targetTexture;
    }

    /// <summary>
    /// 根据大小产生截图
    /// </summary>
    /// <param name="org"></param>
    /// <param name="pos"></param>
    /// <param name="w"></param>
    /// <param name="h"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public static Texture2D GenerateTexture(Texture2D org, Vector2 pos, int w, int h, float scale = 1)
    {
     //   Debug.LogError("w  " + w + " h  " + h + " scale " + scale+  " org.width "+ org.width+
           //  " org.height " + org.height + " pos " + pos.x + " pos " + pos.y);

        int x = (int)((pos.x / scale) + (org.width) * 0.5f - (w * 0.5f) / scale);
        int y = (int)((pos.y / scale) + (org.height) * 0.5f - (h * 0.5f) / scale);

        int blockW =(int)( w/ scale);//截取的像素大小
        int blockH = (int)(h / scale);//截取的像素大小

       // Debug.LogError("x  " + x + " y  " + y + " blockW " + blockW + " blockH " + blockH);

        Texture2D t = new Texture2D(blockW, blockH);
        var colors= org.GetPixels(x, y, blockW, blockH);
        t.SetPixels(colors);
        t.Apply();
        return t;
    }

    /// <summary>
    /// Texture2Texture2D
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="width"></param>
    /// <param name="hight"></param>
    /// <returns></returns>
     public  static Texture2D  Texture2Texture2D(Texture texture, int width = 1440, int hight = 1920)
    {
        Texture2D texture2D = new Texture2D(width, hight, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
   
        RenderTexture renderTexture = RenderTexture.GetTemporary(width, hight, 32);
      //  renderTexture.filterMode = FilterMode.Bilinear;
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);
        return texture2D;
    }


}
