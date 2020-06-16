using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using Common;
using DataModel;
using DG.Tweening;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoveDiaryEditView : View
{
    Transform _bottom;
    Transform _toggleGroupBottom;
    Transform _toggleSelectGroupBottom;
    Transform _editContains;
    DiaryElementModel _diaryElementModel;

    Button _saveBtn;
    LoveDiaryEditType _curLoveDiaryEditType;

    GameObject _curClickElement;

    private LoopHorizontalScrollRect _imageList;
    private LoopVerticalScrollRect _labelList; //标签选项
    private Button _okSelectLabelBtn; //确认选择按钮


    private Color _textEditColor = LoveDiaryModel.TextSelectColors[0]; //文字编辑默认颜色

    public Color TextEditColor
    {
        get { return _textEditColor; }
        set
        {
            _textEditColor = value;
            SetColorToggleOn(_textEditColor);
        }
    }

    private void SetColorToggleOn(Color color)
    {
        ToggleGroup tg = _bottom.Find("BottomSelect/Text/ColorScrollView/Viewport/Content").GetComponent<ToggleGroup>();
        int inx = LoveDiaryModel.GetIndexByColor(TextEditColor);
        Toggle t = _bottom.Find("BottomSelect/Text/ColorScrollView/Viewport/Content").GetChild(inx)
            .GetComponent<Toggle>();
        t.isOn = true;
    }

    List<ElementPB> _lockElementItemList = new List<ElementPB>();
    List<ElementPB> _unlockElementItemList = new List<ElementPB>();
    List<ElementPB> _lockLabelElementItemList = new List<ElementPB>();
    List<ElementPB> _unlockLabelElementItemList = new List<ElementPB>();

    //public int CurTextCount = 0;
    //public int CurImageCount = 0;
    //public int CurRacketCount = 0;

    public LoveDiaryEditType CurLoveDiaryEditType
    {
        get { return _curLoveDiaryEditType; }
        set
        {
            if (_curLoveDiaryEditType == value)
                return;
            _curLoveDiaryEditType = value;
            SetShow(_curLoveDiaryEditType);
        }
    }

    LoveDiaryBottomEditState _curBottomEditState = LoveDiaryBottomEditState.Hide;

    public LoveDiaryBottomEditState CurBottomEditState
    {
        get { return _curBottomEditState; }
        set
        {
            if (_curBottomEditState == value)
                return;
            ToggleGroup tgG = _toggleGroupBottom.GetComponent<ToggleGroup>();
            bool AnyTogglesOn = tgG.AnyTogglesOn();
            if (AnyTogglesOn && _curBottomEditState == LoveDiaryBottomEditState.Show)
                return;
            if (!AnyTogglesOn && _curBottomEditState == LoveDiaryBottomEditState.Hide)
                return;
            _curBottomEditState = value;
//            bool bb = _curBottomEditState == LoveDiaryBottomEditState.Show;
//            _bottom.Find("Bg").gameObject.SetActive(bb);
            DoBottomMove();
        }
    }

    private LoveDiaryBottomEditType _curBottomEditType = LoveDiaryBottomEditType.Image;

    float normalPosY;

    private void DoBottomMove()
    {
        float offsetY = _curBottomEditState == LoveDiaryBottomEditState.Hide ? 0 : 300f;
        var tweener = _bottom.DOLocalMoveY(offsetY + normalPosY, 0.5f);
        tweener.SetEase(DG.Tweening.Ease.OutExpo);
    }

    public void HideTextInput(bool isOpen)
    {
//        CurBottomEditState =isOpen? LoveDiaryBottomEditState.Show:LoveDiaryBottomEditState.Hide;
//        DoBottomMove();
        float offsetY = isOpen ? 300f : 0;
        var tweener = _bottom.DOLocalMoveY(offsetY + normalPosY, 0.5f);
        tweener.SetEase(DG.Tweening.Ease.OutExpo);
    }


    public void OperateBottom(bool isOpen, LoveDiaryBottomEditType pb = LoveDiaryBottomEditType.Image)
    {
        ToggleGroup tgG = _toggleGroupBottom.GetComponent<ToggleGroup>();
        if (!isOpen)
        {
            Debug.LogError("Empty");
            _inputField.text = "";
        }
        
        if (isOpen == false && CurBottomEditState == LoveDiaryBottomEditState.Show)
        {
            //注意先后顺序
            CurBottomEditState = LoveDiaryBottomEditState.Hide;
            tgG.SetAllTogglesOff();
            _curBottomEditType = LoveDiaryBottomEditType.None;
            return;
        }

        if (isOpen == true)
        {
            if (pb == LoveDiaryBottomEditType.Text)
            {
                Debug.Log("ElementTypePB.Text");
                //_curBottomEditType = LoveDiaryBottomEditType.SecondText;
                _toggleGroupBottom.Find("Toggle3").GetComponent<Toggle>().isOn = true;
                _inputField.text = Util.GetStringFormRichText(_curClickElement.GetComponent<WordElement>().GetText());
//                TextEditColor = Util.GetColorFormRichText(_curClickElement.GetComponent<WordElement>().GetText());
                Debug.Log(TextEditColor);
            }

            if (pb == LoveDiaryBottomEditType.SecondText)
            {
                Debug.Log("ElementTypePB.Text");
                _curBottomEditType = LoveDiaryBottomEditType.SecondText;
                _toggleGroupBottom.Find("Toggle3").GetComponent<Toggle>().isOn = true;
                if (_curClickElement != null)
                {

                    Debug.LogError(_curClickElement.GetComponent<WordElement>().GetText());
                    var textColor=Util.GetColorFormRichText(_curClickElement.GetComponent<WordElement>().GetText());
                    _inputField.text =Util.GetStringFormRichText(_curClickElement.GetComponent<WordElement>().GetText());
                    TextEditColor = textColor;                    

//                    TextEditColor = Util.GetColorFormRichText(_curClickElement.GetComponent<WordElement>().GetText());  
                }


                // SetColorToggleOn(TextEditColor);
            }
            else if (pb == LoveDiaryBottomEditType.Image)
            {
                _toggleGroupBottom.Find("Toggle0").GetComponent<Toggle>().isOn = true;
            }
            else if (pb == LoveDiaryBottomEditType.Racket)
            {
                _toggleGroupBottom.Find("Toggle1").GetComponent<Toggle>().isOn = true;
            }
            else if (pb == LoveDiaryBottomEditType.Bg)
            {
                _toggleGroupBottom.Find("Toggle2").GetComponent<Toggle>().isOn = true;
            }
        }

        return;
    }


    private void Awake()
    {
        _bottom = transform.Find("Bottom");
        normalPosY = _bottom.localPosition.y;
        _imageList = _bottom.Find("BottomSelect/Image/Scroll View").GetComponent<LoopHorizontalScrollRect>();
        _imageList.prefabName = "LoveDiary/Prefabs/Items/LoveDiaryEditImageItem";
        _imageList.poolSize = 6;
        _imageList.UpdateCallback = ImageListUpdateCallback;
        _labelList = transform.Find("LabelSelect/Scroll View").GetComponent<LoopVerticalScrollRect>();
        _labelList.prefabName = "LoveDiary/Prefabs/Items/LoveDiaryEditLabelItem";
        _labelList.poolSize = 12;
        _labelList.UpdateCallback = LabelListUpdateCallback;

        _saveBtn = transform.Find("SaveBtn").GetComponent<Button>();
        _saveBtn.onClick.AddListener(SaveDiary);
        //string txt = show ? I18NManager.Get("Common_Save") : I18NManager.Get("Common_Compile");
        _saveBtn.transform.Find("Text").GetComponent<Text>().text = I18NManager.Get("Common_Save");

        _toggleGroupBottom = _bottom.Find("ToggleBottom");
        _toggleSelectGroupBottom = _bottom.Find("BottomSelect");
        _editContains = transform.Find("EditContains");
        _okInputBtn = _bottom.Find("BottomSelect/Text/BigBtn").GetComponent<Button>();
        _cancelInputBtn = _bottom.Find("BottomSelect/Text/SmallBtn").GetComponent<Button>();
        _inputField = _bottom.Find("BottomSelect/Text/InputField").GetComponent<InputField>();
        _okInputBtn.onClick.AddListener(Bigger);
        _cancelInputBtn.onClick.AddListener(Smaller);
        _inputField.onValueChanged.AddListener(InputFieldValueChange);

        transform.Find("Label/SelectBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            if (_curLoveDiaryEditType == LoveDiaryEditType.Show)
                return;
            GameObject gb = transform.Find("LabelSelect").gameObject;
            gb.SetActive(!gb.activeSelf);

        });

        _okSelectLabelBtn = transform.Find("LabelSelect/OkBtn").GetComponent<Button>();
        _okSelectLabelBtn.onClick.AddListener(() =>
        {
            transform.Find("LabelSelect").gameObject.SetActive(false);
            SetTitleLabel();
            if (isGuide)
            {
                GuideManager.Show();
                isGuide = false;
            }
        });
        UIEventListener.Get(_editContains.gameObject).onClick = OnClickBg;
//        UIEventListener.Get(_bottom.Find("Bg").gameObject).onClick = OnClickBottomBg;
    }

    bool isGuide = false;
    public void ShowLabel(bool isShow = false)
    {
        GameObject gb = transform.Find("LabelSelect").gameObject;
        gb.SetActive(isShow);
        isGuide = true;
    }

//    private void OnClickBottomBg(GameObject go)
//    {
//        OperateBottom(false);
//        if (_curClickElement == null)
//            return;
//        _curClickElement.GetComponent<DiaryElementBase>().loveDiaryEditType = LoveDiaryEditType.Show;
//        _curClickElement = null;
//        _curBottomEditType = LoveDiaryBottomEditType.None;
//    }

    private void OnClickBg(GameObject go)
    {
        Debug.Log("OnClickBg");
        if (_curClickElement != null)
        {
            _curClickElement.GetComponent<DiaryElementBase>().loveDiaryEditType = LoveDiaryEditType.Show;
            _curClickElement = null; 
        }


        OperateBottom(false);
        _curBottomEditType = LoveDiaryBottomEditType.None;
    }

    private void SetTitleLabel()
    {
        string str = "";
        if (curLabelPb.ElementId == -1)
        {
            str = I18NManager.Get("LoveDiary_SelectLabel"); //"选择标签";
        }
        else
        {
            ElementPB pb = GlobalData.DiaryElementModel.GetElementRuleById(curLabelPb.ElementId);
            str = pb.Name;
        }

        transform.Find("Label/SelectBtn/Text").GetComponent<Text>().text = str;
    }


    DiaryElementPB curLabelPb; //当前LabelPb；

    int _curLabelElementId = -1; //当前LabelPbId；

    public int CurLabelElementId
    {
        set
        {
            if (_curLabelElementId == value)
                return;
            _curLabelElementId = value;
            curLabelPb.ElementId = _curLabelElementId;
            _labelList.RefreshCells();
        }
        get { return _curLabelElementId; }
    }


    private void LabelListUpdateCallback(GameObject go, int index)
    {
        //   Debug.Log("LabelListUpdateCallback");
        ElementPB pb;
        if (index < _unlockLabelElementItemList.Count)
        {
            pb = _unlockLabelElementItemList[index];
            go.GetComponent<LoveDiaryEditItemBase>().SetData(pb, false);
        }
        else
        {
            int idx = index - _unlockLabelElementItemList.Count;
            pb = _lockLabelElementItemList[index];
            go.GetComponent<LoveDiaryEditItemBase>().SetData(pb, true);
        }

        go.GetComponent<LoveDiaryEditLabelItem>().SetViewBg(index);
        bool b = _curLabelElementId == pb.Id;
        go.GetComponent<LoveDiaryEditLabelItem>().SetSelectState(b);
    }

    private void ImageListUpdateCallback(GameObject go, int index)
    {
        if (index < _unlockElementItemList.Count)
        {
            go.GetComponent<LoveDiaryEditItemBase>().SetData(_unlockElementItemList[index], false);
        }
        else
        {
            int idx = index - _unlockElementItemList.Count;
            go.GetComponent<LoveDiaryEditItemBase>().SetData(_lockElementItemList[idx], true);
        }
    }

    private void SetShow(LoveDiaryEditType editType)
    {
        bool show = editType == LoveDiaryEditType.Edit;
        _toggleGroupBottom.gameObject.SetActive(show);
        _toggleSelectGroupBottom.gameObject.SetActive(show);
        string txt = show ? I18NManager.Get("Common_Save") : I18NManager.Get("Common_Compile");
        //   _saveBtn.transform.Find("Text").GetComponent<Text>().text = show ? I18NManager.Get("Common_Save") : I18NManager.Get("Common_Compile");
        _saveBtn.transform.Find("Text").GetComponent<Text>().text = txt;
    }

    Button _okInputBtn;
    Button _cancelInputBtn;
    InputField _inputField;

    private void Start()
    {
        Toggle tgl;
        ToggleGroup tgggg;
        for (int i = 0; i < _toggleGroupBottom.childCount; i++)
        {
            int idx = i;
            tgl = _toggleGroupBottom.GetChild(i).GetComponent<Toggle>();
            tgl.onValueChanged.AddListener((b) => { OnToggle(b, idx); });

            UIEventListener.Get(_toggleGroupBottom.GetChild(i).Find("ClickImage").gameObject).onClick = ToggleOnClick;
        }
        //Toggle tg2;


        Transform paretTf = _bottom.Find("BottomSelect/Text/ColorScrollView").GetComponent<ScrollRect>().content;
        for (int i = 0; i < paretTf.childCount; i++)
        {
            int idx = i;
            tgl = paretTf.GetChild(i).GetComponent<Toggle>();
            tgl.onValueChanged.AddListener((b) => { OnColorToggle(b, idx); });
        }
    }


    private void ToggleOnClick(GameObject go)
    {
        Debug.Log("ToggleOnClick");
//        if (_curBottomEditType == LoveDiaryBottomEditType.SecondText)
//        {
//            go.transform.parent.GetComponent<Toggle>().isOn = !go.transform.parent.GetComponent<Toggle>().isOn;
//            if (_curClickElement != null)
//            {
//                _curClickElement.GetComponent<DiaryElementBase>().loveDiaryEditType = LoveDiaryEditType.Show;
//            }
//
//
//            _curClickElement = null;
            //_inputField.text = "";
//            _curBottomEditType = LoveDiaryBottomEditType.None;
//
//        }
//        else
//        {
//            go.transform.parent.GetComponent<Toggle>().isOn = !go.transform.parent.GetComponent<Toggle>().isOn;
//        }
        if (_curClickElement != null)
        {
            _curClickElement.GetComponent<DiaryElementBase>().loveDiaryEditType = LoveDiaryEditType.Show; 
            _curClickElement = null;
            _inputField.text = "";
            
        }
        
        go.transform.parent.GetComponent<Toggle>().isOn = !go.transform.parent.GetComponent<Toggle>().isOn;
    }

    private void InputFieldValueChange(string arg0)
    {
        Debug.Log("length:" + arg0.Length + "  content:" + arg0);
        if (String.IsNullOrEmpty(_inputField.text))
        {
            return; 
        }
        _inputField.text = Util.RegexString1(Util.GetNoBreakingString(arg0));
        if (_curClickElement == null)
        {
//            Debug.LogError("need to creat!");
  //          Debug.LogError("call twice");
            EventDispatcher.TriggerEvent<string>(EventConst.LoveDiaryEditOkInputText, _inputField.text);
        }
        InputImmeediate();
        SetInputCount(arg0.Length);

    }

    private void SetInputCount(int count)
    {
        _bottom.Find("BottomSelect/Text/CountTxt").GetComponent<Text>().text = count.ToString() + "/200";
    }

    private void InputImmeediate()
    {
        Debug.LogError("Come here");
        if (String.IsNullOrEmpty(_inputField.text))
        {
            return; 
        }
        string c = ColorUtility.ToHtmlStringRGBA(TextEditColor);
        string inputText = "<color=#" + c + ">" + _inputField.text + "</color>";
        if (_curBottomEditType == LoveDiaryBottomEditType.SecondText)
        {
//            if (_curClickElement == null)
//            {
//                Debug.LogError("_curClickElement == null");
//                EventDispatcher.TriggerEvent<string>(EventConst.LoveDiaryEditOkInputText, _inputField.text);
//            }
//            else
//            {
//                _curClickElement.GetComponent<WordElement>().SetText(inputText);
//            }
            _curClickElement.GetComponent<WordElement>().SetText(inputText);
        }

//        else
//        {
//            EventDispatcher.TriggerEvent<string>(EventConst.LoveDiaryEditOkInputText, inputText);
//        }
    }


    private void Bigger()
    {
        Debug.Log("Bigger");
        if (_curBottomEditType == LoveDiaryBottomEditType.SecondText)
        {
            if (_curClickElement == null)
            {
                Debug.LogError("_curClickElement == null");
            }

            _curClickElement.GetComponent<WordElement>().OnSizeAdd(30);
        }
    }

    private void Smaller()
    {
        Debug.Log("Smaller");
        if (_curBottomEditType == LoveDiaryBottomEditType.SecondText)
        {
            if (_curClickElement == null)
            {
                Debug.LogError("_curClickElement == null");
            }

            _curClickElement.GetComponent<WordElement>().OnSizeReduce(30);
        }
    }


    private void SaveDiary()
    {
        if (_curLoveDiaryEditType == LoveDiaryEditType.Edit)
        {
            List<DiaryElementPB> pbs = new List<DiaryElementPB>();
            pbs.Add(curLabelPb);
            if (curLabelPb.ElementId == -1)
            {
                //  PopupManager.ShowAlertWindow("请输入标签");
                FlowText.ShowMessage(I18NManager.Get("LoveDiary_Hint4"));
                return;
            }

            for (int i = 0; i < _editContains.transform.childCount; i++)
            {
                DiaryElementPB pb = _editContains.transform.GetChild(i).GetComponent<DiaryElementBase>()
                    .GetDiaryElementData();
                if (pb == null)
                {
                    continue;
                }

//                Debug.LogError(pb);
                pbs.Add(pb);
            }

            SendMessage(new Message(MessageConst.CMD_LOVEDIARY_EDIT_SAVE, Message.MessageReciverType.CONTROLLER, pbs));
        }
        else
        {
            SendMessage(new Message(MessageConst.CMD_LOVEDIARY_ENTER_EDITTYPE, Message.MessageReciverType.CONTROLLER,
                LoveDiaryEditType.Edit));
        }
    }

    //给界面添加物品

    public void AddElement(DiaryElementPB pb, bool enable)
    {
        CreateElementItem(pb, enable);
    }

    //删除
    public void DelElement()
    {
    }

    //设置
    public void SetElement()
    {
    }

    private void CreateElementItem(DiaryElementPB diaryElementPB, bool immidiateInput = false)
    {
        ElementPB elementRule = CalendarDetailVo.GetElementRuleById(diaryElementPB.ElementId);
        string prefabPath = "";
        switch (elementRule.ElementType)
        {
            case ElementTypePB.Image:
                prefabPath = "LoveDiary/Prefabs/Elements/ImageElement";
                break;
            case ElementTypePB.Racket:
                prefabPath = "LoveDiary/Prefabs/Elements/ImageElement";
                //    prefabPath = "LoveDiary/Prefabs/LoveDiaryEditRacketItem";
                break;
            case ElementTypePB.Bg:
                _editContains.GetChild(0).GetComponent<BgElememt>()
                    .SetData(diaryElementPB, _editContains.GetComponent<RectTransform>());
                SetBgElement(diaryElementPB.ElementId);
                return;
            case ElementTypePB.Text:
                prefabPath = "LoveDiary/Prefabs/Elements/WordElement";
                break;
            default:
                return;
        }

        var prefab = GetPrefab(prefabPath);
        var item = Instantiate(prefab) as GameObject;
        item.transform.SetParent(_editContains, false);
        item.transform.localScale = Vector3.one;
        item.transform.eulerAngles = Vector3.zero;
        item.GetComponent<DiaryElementBase>().SetData(diaryElementPB, _editContains.GetComponent<RectTransform>());
        if (immidiateInput)
        {
            _curClickElement = item;
            _curClickElement.GetComponent<DiaryElementBase>().loveDiaryEditType = LoveDiaryEditType.Edit;
            _curBottomEditType = LoveDiaryBottomEditType.SecondText;
        }

        UIEventListener.Get(item).onClick = OnClickElememtItem;
        return;
    }

    private void OnClickElememtItem(GameObject go)
    {
        if (_curLoveDiaryEditType == LoveDiaryEditType.Show)
            return;
        if (_curClickElement != null)
        {
            _curClickElement.GetComponent<DiaryElementBase>().loveDiaryEditType = LoveDiaryEditType.Show;
        }

        _curClickElement = go;
//        Debug.LogError("Here");
        var elementBase=_curClickElement.GetComponent<DiaryElementBase>();
        elementBase.loveDiaryEditType = LoveDiaryEditType.Edit;   
        EventDispatcher.TriggerEvent(EventConst.LoveDiaryEditItemText,elementBase.GetDiaryElementData().ElementId);
    }

    public void SetBgElement(int ElementId)
    {
        Debug.LogError("SetBgElement " + ElementId);

        BgElememt bg = _editContains.GetChild(0).GetComponent<BgElememt>();
        if (bg.GetDiaryElementData() == null)
        {
            DiaryElementPB pb = new DiaryElementPB();
            pb.ElementId = ElementId;
            pb.XPos = 0;
            pb.YPos = 0;
            pb.ScaleX = 1;
            pb.ScaleY = 1;
            pb.Content = "";
            pb.Rotation = 0;
            bg.SetData(pb, _editContains.GetComponent<RectTransform>());
        }

        _editContains.GetChild(0).GetComponent<BgElememt>().SetBg(ElementId);
    }

    public void UpdateItemState(ElementPB pb)
    {
        //  List<ElementPB> _lockElementItemList;
        ElementPB unPb;
        for (int i = 0; i < _lockElementItemList.Count; i++)
        {
            unPb = _lockElementItemList[i];
            if (unPb.Id == pb.Id)
            {
                _lockElementItemList.Remove(unPb);
                _unlockElementItemList.Add(unPb);
                _imageList.RefreshCells();
            }
        }
    }

    public void SetShowSelectItem(ElementTypePB elementType)
    {
        _imageList.RefillCells();
        _diaryElementModel.GetElementByType(elementType, ElementModulePB.Diary, ref _unlockElementItemList,
            ref _lockElementItemList);
        _toggleSelectGroupBottom.Find("Image/Tips").gameObject.Hide();
        if (elementType == ElementTypePB.Racket)
        {
            _lockElementItemList.Clear();
            if (_unlockElementItemList.Count == 0)
            {
                _toggleSelectGroupBottom.Find("Image/Tips").gameObject.Show();
            }
        }

        _imageList.totalCount = _lockElementItemList.Count + _unlockElementItemList.Count;
        _imageList.RefreshCells();
    }

    public void SetShowLabelSelectItem()
    {
        //_labelList.RefillCells();
        _diaryElementModel.GetElementByType(ElementTypePB.Label, ElementModulePB.Diary, ref _unlockLabelElementItemList,
            ref _lockLabelElementItemList);
        _labelList.totalCount = _lockLabelElementItemList.Count + _unlockLabelElementItemList.Count;
        _labelList.RefreshCells();
    }


    public void SetData(LoveDiaryEditType editType, DiaryElementModel diaryElementModel,
        CalendarDetailVo calendarDetailVo)
    {
        _diaryElementModel = diaryElementModel;
        SetShowSelectItem(ElementTypePB.Image);
        SetShowLabelSelectItem();

        for (int i = 0; i < calendarDetailVo.DiaryElements.Count; i++)
        {
            CreateElementItem(calendarDetailVo.DiaryElements[i]);
        }

        curLabelPb = calendarDetailVo.LabelElement;
        _curLabelElementId = curLabelPb.ElementId;
        SetTitleLabel();
    }

    private void CreateColorItem(Color color)
    {
        string prefabPath = "LoveDiary/Prefabs/Items/LoveDiaryEditColorItem";
        Transform paretTf = transform.Find("BottomSelect/Text/ColorScrollView").GetComponent<ScrollRect>().content;
        var prefab = GetPrefab(prefabPath);
        var item = Instantiate(prefab) as GameObject;
        item.transform.SetParent(paretTf, false);
        item.transform.localScale = Vector3.one;
        item.GetComponent<LoveDiaryEditColorItem>().SetData(color);
    }

    private void OnColorToggle(bool b, int idx)
    {
        Debug.Log(b + "  bool " + idx);
        if (b)
        {
            TextEditColor = LoveDiaryModel.TextSelectColors[idx];
            InputImmeediate();
//            string c = ColorUtility.ToHtmlStringRGBA(TextEditColor);
//            _inputField.text = "<color=#" + c+">"+ _inputField.text+ "</color>";
//            if (_curBottomEditType == LoveDiaryBottomEditType.SecondText)
//            {
//                if (_curClickElement == null)
//                {
//                    Debug.LogError("_curClickElement == null");
//                }
//                _curClickElement.GetComponent<WordElement>().SetText(inputText);
//            }
        }
    }

    private void OnToggle(bool b, int idx)
    {
        if (_curBottomEditType == LoveDiaryBottomEditType.SecondText)
        {
            if (b == false)
            {
            }
        }

        if (idx < 3)
        {
            _toggleSelectGroupBottom.Find("Image").gameObject.SetActive(b);
        }
        else
        {
            _toggleSelectGroupBottom.Find("Text").gameObject.SetActive(b);
        }

        //ToggleGroup tgG= _toggleGroupBottom.GetComponent<ToggleGroup>();
        CurBottomEditState = b == true ? LoveDiaryBottomEditState.Show : LoveDiaryBottomEditState.Hide;

        if (!b)
            return;
        switch (idx)
        {
            case 0: //贴纸
                _curBottomEditType = LoveDiaryBottomEditType.Image;
                SetShowSelectItem(ElementTypePB.Image);
                break;
            case 1: //拍立得
                SetShowSelectItem(ElementTypePB.Racket);
                _curBottomEditType = LoveDiaryBottomEditType.Racket;
                break;
            case 2: //背景
                SetShowSelectItem(ElementTypePB.Bg);
                _curBottomEditType = LoveDiaryBottomEditType.Bg;
                break;
            case 3:
                if (_curBottomEditType == LoveDiaryBottomEditType.SecondText)
                {
                    return;
                }
                else
                {
                    _curBottomEditType = LoveDiaryBottomEditType.Text;
//                    _inputField.text = "";
                    TextEditColor = LoveDiaryModel.TextSelectColors[0];
                }

                break;
        }

//        if (_curClickElement == null)
//            return;
//        
////        Debug.LogError(_curClickElement.GetComponent<DiaryElementBase>().loveDiaryEditType);
//        _curClickElement.GetComponent<DiaryElementBase>().loveDiaryEditType = LoveDiaryEditType.Show;//LoveDiaryEditType.Show
//        _curClickElement = null;
    }

    private void CloseToggle(int index = -1)
    {
        return;
        if (index != -1)
        {
        }

        Toggle tgl;
        for (int i = 0; i < _toggleGroupBottom.childCount; i++)
        {
            int idx = i;
            tgl = _toggleGroupBottom.GetChild(i).GetComponent<Toggle>();
            tgl.isOn = false;
            if (i == 3)
            {
                transform.Find("BottomSelect/Text/CountTxt").GetComponent<Text>().text = "";
            }
        }
    }
}