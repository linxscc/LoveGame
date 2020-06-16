using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Module.Framework.Utils;
using game.main;
using UnityEngine.EventSystems;

public class TabSelectAnimBtnItem : MonoBehaviour
{
    private RectTransform _root;
    public RectTransform root
    {
        get
        {
            return _root;
        }
    }

    private GameObject _selectObj;
    private Toggle _tabBtn;
    private Text _tabText;

    private GameObject _openObj;
    private Text _openText;

    private GameObject _lockObj;

    private TabSelectAnimBtnData _data;
    public TabSelectAnimBtnData data
    {
        get
        {
            return _data;
        }
    }

    private System.Action<string, bool> _onValueChange;
    public System.Action onSelectNextTab;
    public System.Action onSelectPreTab;

    private TimerHandler _countDown = null;

    private void Awake()
    {
        _root = transform.GetComponent<RectTransform>();
        _selectObj = transform.Find("SelectObj").gameObject;
        _tabBtn = transform.GetComponent<Toggle>();
        _tabText = transform.Find("Label").GetComponent<Text>();
        _openObj = transform.Find("OpenObj").gameObject;
        _openText = _openObj.transform.Find("OpenLabel").GetComponent<Text>();
        _lockObj = transform.Find("LockObj").gameObject;

        _tabBtn.onValueChanged.AddListener(OnValueChange);
        _selectObj.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        UIEventListener.Get(_tabBtn.gameObject).onBeginDrag = OnBeginDrag;
        UIEventListener.Get(_tabBtn.gameObject).onEndDrag = OnEndDrag;
    }

    public void Init(TabSelectAnimBtnData tabData, System.Action<string, bool> onValueChange)
    {
        _data = tabData;
        _onValueChange = onValueChange;

        if (_countDown != null)
        {
            ClientTimer.Instance.RemoveCountDown(_countDown);
            _countDown = null;
        }
        if (_data.endTime != 0)
        {
            _openObj.Show();
            long endTime = _data.endTime;
            //long endTime = ClientTimer.Instance.GetCurrentTimeStamp() + 5000000;
            _countDown = ClientTimer.Instance.AddCountDown("TabSelectAnimBtnItem_"+_data.path, endTime, 1, onCountdown, onCountdownFinish);
        }
        else
        {
            _openObj.Hide();
        }

        _lockObj.SetActive(_data.lockState);

        if (_data.lockState)
        {
            _tabBtn.interactable = false;
            UIEventListener.Get(_tabBtn.gameObject).onClick = OnLockClick;
        }
    }

    public bool isOn
    {
        set
        {
            _selectObj.SetActive(value);
            _tabBtn.isOn = value;
        }
    }

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }


    private void OnValueChange(bool state)
    {
        _selectObj.SetActive(state);
        if (_onValueChange != null)
        {
            _onValueChange(_data.path, state);
        }
    }

    private float _beginDragPos;
    private void OnBeginDrag(PointerEventData eventData)
    {
        _beginDragPos = eventData.position.x;
    }

    private void OnEndDrag(PointerEventData eventData)
    {
        float delta = eventData.position.x - _beginDragPos;
        if (_data == null) return;
        if(delta > 0)
        {
            if (onSelectPreTab != null)
                onSelectPreTab();
        }
        else
        {
            if (onSelectNextTab != null)
                onSelectNextTab();
        }
    }

    private void OnLockClick(GameObject go)
    {
        FlowText.ShowMessage(I18NManager.Get("Common_Underdevelopment"));
    }

    private void onCountdown(int time)
    {
        if (time >= 0)
            _openText.text = I18NManager.Get("ActivityTemplate_ActivityTemplateTime2", DateUtil.GetTimeFormat4(time));
    }

    private void onCountdownFinish()
    {
        _openObj.Hide();
        if (!_data.isAlwayShow)
            gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (_countDown != null)
        {
            ClientTimer.Instance.RemoveCountDown(_countDown);
            _countDown = null;
        }
    }
}
