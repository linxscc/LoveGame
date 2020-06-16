//using System.Collections;
//using System.Collections.Generic;
//using Assets.Scripts.Framework.GalaSports.Core;
//using Assets.Scripts.Framework.GalaSports.Core.Events;
//using Com.Proto;
//using Common;
//using DataModel;
//using game.main;
//using game.tools;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class SaveAndShareView : View
//{

//    #region    ToDo...定义组件
//    private RawImage _bgBG;
//    private RawImage _bgPerson;
//    private Text _name;
//    private Text _clothingName;
//    private Text _backgroundName;
//    private Button _saveBtn;
//    private Button _shareBtn;

//    private GameObject[] _btnArray;  //按钮
//    #endregion

//    private void Awake()
//    {
//        ComponentInit();

//    }

//    /// <summary>
//    /// 组件初始化
//    /// </summary>
//    private void ComponentInit()
//    {
//        _bgBG = transform.Find("Background").GetComponent<RawImage>();
//        _bgPerson = transform.Find("BG/Person").GetComponent<RawImage>();

//        _name = transform.Find("BG/InfoBG/Name").GetComponent<Text>();
//        _clothingName = transform.Find("BG/InfoBG/Name/Dian/Clothing").GetComponent<Text>();
//        _backgroundName = transform.Find("BG/InfoBG/Name/Dian/Clothing/Dian/BackGround").GetComponent<Text>();

//        _saveBtn = transform.Find("SaveBtn").GetComponent<Button>();
//        _shareBtn = transform.Find("ShareBtn").GetComponent<Button>();

//        _saveBtn.onClick.AddListener(OnSaveBtn);
//        _shareBtn.onClick.AddListener(OnShareBtn);

//        PointerClickListener.Get(gameObject).onClick = go =>
//        {
//            SendMessage(new Message(MessageConst.MODULE_FACORABLILITY_Close_SAVEANDSHARE_VIEW));
//        };

//        _btnArray = new GameObject[2];
//        _btnArray[0] = _saveBtn.gameObject;
//        _btnArray[1] = _shareBtn.gameObject;
//    }


//    /// <summary>
//    /// 保存按钮
//    /// </summary>
//    private void OnSaveBtn()
//    {
//        SendMessage(new Message(MessageConst.CMD_SAVEANDSHARE_VIEW_SAVE));   //发送保存Mesage
//    }


//    /// <summary>
//    /// 分享按钮
//    /// </summary>
//    private void OnShareBtn()
//    {
//        SendMessage(new Message(MessageConst.CMD_SAVEANDSHARE_VIEW_SHARE));   //发送分享Mesage
//    }


//    /// <summary>
//    /// 截屏时，是否隐藏Btn图片，及其Text文字
//    /// </summary>
//    /// <param name="isShow"></param>
//    public void ShowOrHideBtn(bool isShow)
//    {
//        for (int i = 0; i < _btnArray.Length; i++)
//        {
//            _btnArray[i].GetComponent<Image>().enabled = isShow;
//            _btnArray[i].GetComponentInChildren<Text>().enabled = isShow;
//        }
//    }

//    /// <summary>
//    /// 设置界面基本信息
//    /// </summary>
//    /// <param name="vo"></param>
//    public void SetInfo(UserFavorabilityVo vo)
//    {
//        _name.text = GlobalData.FavorabilityMainModel.GetPlayerName(vo.Player);

//        Debug.Log("服装Id：" + vo.Clothes);
//        Debug.Log("背景Id" + vo.BackDrop);

//        if (vo.Clothes==0 && vo.BackDrop==0)
//        {
//            Debug.Log("首次登陆");

//            _backgroundName.text = PropConst.DEFAULT_BG_TXXT_NAME;
//            _bgBG.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(PropConst.DEFAULT_BG_IMAGE_NAME);

//            switch (vo.Player)
//            {
//                case PlayerPB.None:
//                    break;
//                case PlayerPB.TangYiChen:
//                    _clothingName.text = PropConst.DEFAULT_CLOTHING_TANGYICHEN_BG_TEXT_NAME;
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(PropConst.DEFAULT_CLOTHING_TANGYICHEN_BG_IAMGE_NAME);
//                    break;
//                case PlayerPB.QinYuZhe:
//                    _clothingName.text = PropConst.DEFAULT_CLOTHING_QINYUZHE_BG_TEXT_NAME;
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(PropConst.DEFAULT_CLOTHING_QINYUZHE_BG_IAMGE_NAME);
//                    break;
//                case PlayerPB.YanJi:
//                    _clothingName.text = PropConst.DEFAULT_CLOTHING_YANJI_BG_TEXT_NAME;
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(PropConst.DEFAULT_CLOTHING_YANJI_BG_IAMGE_NAME);
//                    break;
//                case PlayerPB.ChiYu:
//                    _clothingName.text = PropConst.DEFAULT_CLOTHING_CHIYU_BG_TEXT_NAME;
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(PropConst.DEFAULT_CLOTHING_CHIYU_BG_IAMGE_NAME);
//                    break;
//            }
//        }
//        else if (vo.Clothes!=0 && vo.BackDrop ==0)
//        {
//            Debug.Log("不是首次登陆设置了服装，没设置背景");
//            _backgroundName.text = PropConst.DEFAULT_BG_TXXT_NAME;
//            _bgBG.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(PropConst.DEFAULT_BG_IMAGE_NAME);

//            switch (vo.Player)
//            {
//                case PlayerPB.None:
//                    break;
//                case PlayerPB.TangYiChen:
//                    _clothingName.text = PlayerPrefs.GetString("NoFirstClothingName_TangYiChen");
//                    string _imageTangYiChen = PlayerPrefs.GetString("NoFirstClothingImage_TangYiChen");
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageTangYiChen);
//                    break;
//                case PlayerPB.QinYuZhe:
//                    _clothingName.text = PlayerPrefs.GetString("NoFirstClothingName_QinYuZhe");
//                    string _imageQinYuZhe = PlayerPrefs.GetString("NoFirstClothingImage_QinYuZhe");
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageQinYuZhe);
//                    break;
//                case PlayerPB.YanJi:
//                    _clothingName.text = PlayerPrefs.GetString("NoFirstClothingName_YanJi");
//                    string _imageYanJi = PlayerPrefs.GetString("NoFirstClothingImage_YanJi");
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageYanJi);
//                    break;
//                case PlayerPB.ChiYu:
//                    _clothingName.text = PlayerPrefs.GetString("NoFirstClothingName_ChiYu");
//                    string _imageChiYu = PlayerPrefs.GetString("NoFirstClothingImage_ChiYu");
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageChiYu);

//                    break;
               
//            }
//        }
//        else if (vo.Clothes==0 && vo.BackDrop !=0)
//        {
//            Debug.Log("不是首次登陆设置了背景，没设置服装");
//            switch (vo.Player)
//            {
//                case PlayerPB.None:
//                    break;
//                case PlayerPB.TangYiChen:

//                    _clothingName.text = PropConst.DEFAULT_CLOTHING_TANGYICHEN_BG_TEXT_NAME;
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(PropConst.DEFAULT_CLOTHING_TANGYICHEN_BG_IAMGE_NAME);

//                    _backgroundName.text = PlayerPrefs.GetString("NoFirstBackGroundName_TangYiChen");
//                    string _imageTangYiChenBG = PlayerPrefs.GetString("NoFirstBackGroundImage_TangYiChen");
//                    _bgBG.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageTangYiChenBG);

//                    break;
//                case PlayerPB.QinYuZhe:

//                    _clothingName.text = PropConst.DEFAULT_CLOTHING_QINYUZHE_BG_TEXT_NAME;
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(PropConst.DEFAULT_CLOTHING_QINYUZHE_BG_IAMGE_NAME);

//                    _backgroundName.text = PlayerPrefs.GetString("NoFirstBackGroundName_QinYuZhe");
//                    string _imageQinYuZheBG = PlayerPrefs.GetString("NoFirstBackGroundImage_QinYuZhe");
//                    _bgBG.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageQinYuZheBG);

//                    break;
//                case PlayerPB.YanJi:

//                    _clothingName.text = PropConst.DEFAULT_CLOTHING_YANJI_BG_TEXT_NAME;
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(PropConst.DEFAULT_CLOTHING_YANJI_BG_IAMGE_NAME);

//                    _backgroundName.text = PlayerPrefs.GetString("NoFirstBackGroundName_YanJi");
//                    string _imageYanJiBG = PlayerPrefs.GetString("NoFirstBackGroundImage_YanJi");
//                    _bgBG.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageYanJiBG);

//                    break;
//                case PlayerPB.ChiYu:

//                    _clothingName.text = PropConst.DEFAULT_CLOTHING_CHIYU_BG_TEXT_NAME;
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(PropConst.DEFAULT_CLOTHING_CHIYU_BG_IAMGE_NAME);

//                    _backgroundName.text = PlayerPrefs.GetString("NoFirstBackGroundName_ChiYu");
//                    string _imageChiYuBG = PlayerPrefs.GetString("NoFirstBackGroundImage_ChiYu");
//                    _bgBG.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageChiYuBG);

//                    break;
//            }
//        }
//        else if (vo.Clothes != 0 && vo.BackDrop != 0)
//        {
//            Debug.Log("不是首次登陆");

//            switch (vo.Player)
//            {
//                case PlayerPB.None:
//                    break;
//                case PlayerPB.TangYiChen:
//                    _clothingName.text = PlayerPrefs.GetString("NoFirstClothingName_TangYiChen");
//                    string _imageTangYiChen = PlayerPrefs.GetString("NoFirstClothingImage_TangYiChen");
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageTangYiChen);

//                    _backgroundName.text = PlayerPrefs.GetString("NoFirstBackGroundName_TangYiChen");
//                    string _imageTangYiChenBG = PlayerPrefs.GetString("NoFirstBackGroundImage_TangYiChen");
//                    _bgBG.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageTangYiChenBG);
//                    break;
//                case PlayerPB.QinYuZhe:

//                    _clothingName.text = PlayerPrefs.GetString("NoFirstClothingName_QinYuZhe");
//                    string _imageQinYuZhe = PlayerPrefs.GetString("NoFirstClothingImage_QinYuZhe");
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageQinYuZhe);

//                    _backgroundName.text = PlayerPrefs.GetString("NoFirstBackGroundName_QinYuZhe");
//                    string _imageQinYuZheBG = PlayerPrefs.GetString("NoFirstBackGroundImage_QinYuZhe");
//                    _bgBG.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageQinYuZheBG);

//                    break;
//                case PlayerPB.YanJi:

//                    _clothingName.text = PlayerPrefs.GetString("NoFirstClothingName_YanJi");
//                    string _imageYanJi = PlayerPrefs.GetString("NoFirstClothingImage_YanJi");
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageYanJi);

//                    _backgroundName.text = PlayerPrefs.GetString("NoFirstBackGroundName_YanJi");
//                    string _imageYanJiBG = PlayerPrefs.GetString("NoFirstBackGroundImage_YanJi");
//                    _bgBG.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageYanJiBG);

//                    break;
//                case PlayerPB.ChiYu:
//                    _clothingName.text = PlayerPrefs.GetString("NoFirstClothingName_ChiYu");
//                    string _imageChiYu = PlayerPrefs.GetString("NoFirstClothingImage_ChiYu");
//                    _bgPerson.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageChiYu);

//                    _backgroundName.text = PlayerPrefs.GetString("NoFirstBackGroundName_ChiYu");
//                    string _imageChiYuBG = PlayerPrefs.GetString("NoFirstBackGroundImage_ChiYu");
//                    _bgBG.texture = Assets.Scripts.Framework.GalaSports.Service.ResourceManager.Load<Texture>(_imageChiYuBG);

//                    break;
          
//            }
//        }
//    }


//    /// <summary>
//    /// 开启我的协程
//    /// </summary>
//    /// <param name="enumerator"></param>
//    public void MyStartCoroutine(IEnumerator enumerator)
//    {
//        StartCoroutine(enumerator);
//    }
//}
