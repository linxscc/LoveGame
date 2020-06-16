using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NumBarWidget : MonoBehaviour
{
    private Scrollbar _bar;
    private GameObject _btnAdd;
    private GameObject _btnSub;

    private float _maxValue;
    private float _minValue;
    private float _value;
    private float _percent;

    private float _speed = 1f;

    private float _pastTime = 0f;

    private bool _isBtnAddDown;
    private bool _isBtnSubDown;

    public System.Action<float> onValueChange;

    private void Awake()
    {
        _bar = transform.Find("Scrollbar").GetComponent<Scrollbar>();
        _btnAdd = transform.Find("BtnAdd").gameObject;
        _btnSub = transform.Find("BtnSub").gameObject;

        _bar.onValueChanged.AddListener(OnBarValueChange);

        UIEventListener.Get(_btnAdd).onDown = OnBtnAddDown;
        UIEventListener.Get(_btnAdd).onUp = OnBtnAddUp;
        UIEventListener.Get(_btnSub).onDown = OnBtnSubDown;
        UIEventListener.Get(_btnSub).onUp = OnBtnSubUp;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(float minValue, float maxValue, float value)
    {
        _maxValue = maxValue;
        _minValue = minValue;
        _value = value;
        _percent = _value / (_maxValue - _minValue);
        _bar.value = _percent;
    }

    private void OnBarValueChange(float precent)
    {
        Debug.Log("OnBarValueChange:"+ precent);
        _percent = precent;
        CheckPercent();
        if (onValueChange != null) onValueChange(_value);
    }

    private void CheckPercent()
    {
        if (_percent > 1) _percent = 1f;
        if (_percent < 0) _percent = 0f;
        _value = _percent * (_maxValue - _minValue);
    }

    private void OnBtnAddDown(PointerEventData eventData)
    {
        _value += _speed;
        _percent = _value / (_maxValue - _minValue);
        _bar.value = _percent;
        _isBtnSubDown = false;
        _isBtnAddDown = true;
        _pastTime = 0f;
    }

    private void OnBtnAddUp(PointerEventData eventData)
    {
        _isBtnAddDown = false;
    }

    private void OnBtnSubDown(PointerEventData eventData)
    {
        _value -= _speed;
        _percent = _value / (_maxValue - _minValue);
        _bar.value = _percent;
        _isBtnAddDown = false;
        _isBtnSubDown = true;
        _pastTime = 0f;
    }

    private void OnBtnSubUp(PointerEventData eventData)
    {
        _isBtnSubDown = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (!_isBtnAddDown && !_isBtnSubDown) return;
        if(_pastTime > 0.01f)
        {
            _value += (_isBtnAddDown) ? _speed : -_speed;
            _percent = _value / (_maxValue - _minValue);
            _bar.value = _percent;
            _pastTime = 0f;
            return;
        }
        _pastTime += Time.deltaTime;

    }
}
