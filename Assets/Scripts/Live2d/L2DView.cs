using System;
using System.Collections.Generic;
using Common;
using live2d;
using live2d.framework;
using UnityEngine;

public class L2DView : MonoBehaviour, IDisposable
{
    private L2DModel _model;

    public L2DModel Model
    {
        get { return _model; }
        set { _model = value; }
    }

    LIVE2DVIEWTYPE _live2dViewType = LIVE2DVIEWTYPE.STORY;
    public LIVE2DVIEWTYPE Live2dViewType
    {
        get { return _live2dViewType; }
        set { _live2dViewType = value; }
    }

    private bool _lipSync;
    public bool LipSync
    {
        set { _lipSync = value; }
    }

    public string ModelId { get; private set; }

    [SerializeField] private float _x = -1;
    [SerializeField] private float _y = 1.6f;
    [SerializeField] private float _w = 2.2f;

    private float[] _samples = new float[128];
    private float _lastMax = 0;
    
    public float X
    {
        get { return _x; }
        set { _x = value; }
    }

    public float Y
    {
        get { return _y; }
        set { _y = value; }
    }

    public float Width
    {
        get { return _w; }
        set { _w = value; }
    }

    public void LoadModel(string modelId, List<string> donotUnloadIds)
    {
        if (_model != null && modelId != ModelId)
        {
            _model.UnloadAsset();
        }
        
        ModelId = modelId;

        _model = new L2DModel();
        _model.LoadAssets(modelId, donotUnloadIds);
    }
    
    public void Reload()
    {
        _model.Reload();
    }

    public float threshold= 12.0f;
    public int avgcount = 5;
    [SerializeField]
    private List<float> average=new List<float>();
    private void UpdateMouth()
    {
        if (_lipSync)
        {
            _model.setLipSync(true);
            if (!AudioManager.Instance.IsPlayingDubbing)
            {
                _model.setLipSyncValue(0);
            }
            else
            {
                AudioManager.Instance.DubbingAudioSource.GetSpectrumData(_samples, 0, FFTWindow.BlackmanHarris);
                Array.Sort(_samples);
                float num = 0;
                for (int i = 0; i < 15; i++)
                {
                    num += _samples[_samples.Length - 3 - i];
                }

                float max = num * threshold;
                max = Mathf.Clamp(max, 0, 1);
                average.Add(max);
                if(average.Count> avgcount)
                {
                    average.RemoveAt(0);
                }
                float c = 0;
                for(int i=0; i<average.Count;i++)
                {
                    c += average[i];
                }
                max = c / average.Count;
                // max = Mathf.Lerp(_lastMax, max, 0.5f);
                _lastMax = max;
                _model.setLipSyncValue(max);
            }

            _model.getLive2DModel().setParamFloat(L2DStandardID.PARAM_MOUTH_OPEN_Y, _model.getLipSyncValue(), 0.8f);
        }
    }

    void OnPostRender()
    {
        Live2DModelUnity live2DModel = (Live2DModelUnity) _model.getLive2DModel();
        var w = live2DModel.getCanvasWidth();
        var h = live2DModel.getCanvasHeight();

        L2DModelMatrix matrix = new L2DModelMatrix(w, h);
        matrix.setWidth(_w);
        matrix.setCenterPosition(0.5f, 0.5f);
        matrix.setX(_x);
        matrix.setY(_y);

        Matrix4x4 m1 = Matrix4x4.identity;
        float[] m2 = matrix.getArray();
        for (int i = 0; i < 16; i++)
        {
            m1[i] = m2[i];
        }

        live2DModel.setMatrix(m1);

        //_model.Update();_model.Draw 中有调用
        UpdateMouth();
        live2DModel.update();
        _model.Draw(Live2dViewType);
    }

    public void Dispose()
    {
        if (_model != null)
        {
            _model.UnloadAsset();
            _model = null;
        }
        Destroy(this);
    }
}