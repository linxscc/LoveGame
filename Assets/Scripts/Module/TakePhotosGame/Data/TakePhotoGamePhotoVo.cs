using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Com.Proto;

public class TakePhotoGamePhotoVo
{
    public int Id;

    List<TakePhotoGameStateVo> _takePhotoGameStateVos;
    public  TakePhotoRulePB _takePhotoRulePB;

    public Vector2 targetPos;//目标坐标
    public float scale;//缩放系数
    public string resoucePath = "";

    public bool isFinished;

    public Texture2D targetTexture;
    public Texture2D playerTexture;

    public int GetCurScore()
    {
        int score = 0;

        foreach(var v in _takePhotoGameStateVos)
        {
            score += v.score;
        }

        return score;
    }


    public List<TakePhotoGameStateVo> GetStateVo()
    {
        return _takePhotoGameStateVos;
    }

    public TakePhotoGamePhotoVo(TakePhotoRulePB takePhotoRulePB)
    {
        _takePhotoGameStateVos = new List<TakePhotoGameStateVo>();
        _takePhotoRulePB = takePhotoRulePB;

        //Debug.LogError(_takePhotoRulePB.Coordinate);
        scale = (float)_takePhotoRulePB.Gain;
        var strs = _takePhotoRulePB.Coordinate.Split(',');

        if(strs.Length<2)
        {
            Debug.LogError("  TakePhotoGamePhotoVo  Strs length is error   ");
        }
        targetPos.x = -int.Parse(strs[0])* scale;
        targetPos.y = -int.Parse(strs[1])* scale;
        resoucePath = GetPhotoResoucePath(_takePhotoRulePB);
        isFinished = false;
    }

    public TakePhotoGameStateVo SetStateVo(TakePhotosGameShowState state,int accuracy)
    {
        var vo = _takePhotoGameStateVos.Find((m) => { return m.showState == state; });
        if (vo == null) 
        {
            vo = new TakePhotoGameStateVo();
            vo.showState = state;
            vo.accuracy = accuracy;
            vo.score = GetScore(accuracy);//通过计算出分数
            _takePhotoGameStateVos.Add(vo);
        }

        if(state==TakePhotosGameShowState.Blur)
        {
            isFinished = true;
        }

        return vo;
    }

    /// <summary>
    /// 通过准确率得到相应的分数
    /// </summary>
    /// <param name="percent"></param>
    /// <returns></returns>
    private static int GetScore(int acc)
    {
        int score = 0;
        score = 72500 / (150 - acc) - 450;
        return score;
    }

    public static string GetPhotoResoucePath(TakePhotoRulePB pb)
    {
        string path = "";
        if (pb.IsEvo == 0)
        {
            path = "Card/Image/" + pb.PictureId;
        }
        else
        {
            path = "Card/Image/EvolutionCard/" + pb.PictureId;
        }
        return path;
    }
}
