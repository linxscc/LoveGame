using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Guide;
using Com.Proto;
using Common;
using DataModel;
using GalaAccount.Scripts.Framework.Utils;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayView : View
{
    private Transform _storyBanner;
    private Button _backBtn;
    private Transform _viewContent;
    private Transform _recollectionBanner;
    private Transform _supporterActBanner;
    private Transform _exerciseRoomBanner;
    private Transform _exploringBanner;
    private Transform _visitunlock;
    private Transform _recollectionunlock;
    private Transform _encorageactunlock;

    private Transform _trainingRoomBanner;
    private Transform _trainingRoomUnlock;

    private Transform _arrowParent;
    private Transform _arrowContent;

    private Transform _gameChooser;
    private Transform _goWindwoShopping;
    private Transform _goWindowShoppingBanner;
    private Transform _goWindowShoppingBannerUnlock;
    private Button _helpBtn;

    private void Awake()
    {
        _gameChooser = transform.Find("GameChooser");
        _goWindwoShopping = transform.Find("GoWindwoShopping");

        _helpBtn = transform.GetButton("GoWindwoShopping/Button");
        _helpBtn.onClick.AddListener(ShowGoShoppingHelp);
        _helpBtn.gameObject.Hide();
        
        _arrowContent = transform.Find("GameChooser/Viewport/ArrowContent");
        _arrowParent = transform.Find("GameChooser/Viewport/ArrowContent/StoryBanner/ArrowParent");
        GuideArrow.DoAnimation(_arrowParent);
        _storyBanner = transform.Find("GameChooser/Viewport/Content/StoryBanner");
        PointerClickListener.Get(_storyBanner.gameObject).onClick = GotoMainLine;

        _supporterActBanner = transform.Find("GameChooser/Viewport/Content/SupporterActivityBanner");

        PointerClickListener.Get(_supporterActBanner.gameObject).onClick = GotoSupporterActivity;

        _recollectionBanner = transform.Find("GameChooser/Viewport/Content/RecollectionBanner");
        PointerClickListener.Get(_recollectionBanner.gameObject).onClick = GotoRecollection;


        //_exerciseRoomBanner = transform.Find("Viewport/Content/ExerciseRoomBanner");
        //PointerClickListener.Get(_exerciseRoomBanner.gameObject).onClick = GotoExerciseRoom;
        _visitunlock = transform.Find("GameChooser/Viewport/Content/VisitBanner/Bg");
        _recollectionunlock = transform.Find("GameChooser/Viewport/Content/RecollectionBanner/Bg");
        _encorageactunlock = transform.Find("GameChooser/Viewport/Content/SupporterActivityBanner/Bg");
        _goWindowShoppingBannerUnlock = transform.Find("GameChooser/Viewport/Content/GoWindowShoppingBanner/Bg");

        _exploringBanner = transform.Find("GameChooser/Viewport/Content/VisitBanner");
        PointerClickListener.Get(_exploringBanner.gameObject).onClick = GotoVisit;

        _trainingRoomBanner = transform.Find("GameChooser/Viewport/Content/TrainingRoomBanner");
        PointerClickListener.Get(_trainingRoomBanner.gameObject).onClick = GotoTrainingRoomBanner;

        _trainingRoomUnlock = transform.Find("GameChooser/Viewport/Content/TrainingRoomBanner/Bg");

        Button takePhotoGameBtn = transform.GetButton("GoWindwoShopping/Bg/TakePhotoGameBtn");
        takePhotoGameBtn.onClick.AddListener(OnTakePhotoGame);

        Button airborneGameBtn = transform.GetButton("GoWindwoShopping/Bg/AirborneGameBtn");
        airborneGameBtn.onClick.AddListener(OnAirborneGame);
        
        _goWindowShoppingBanner = transform.Find("GameChooser/Viewport/Content/GoWindowShoppingBanner");
        PointerClickListener.Get(_goWindowShoppingBanner.gameObject).onClick = GoWindowShopping;
    }

    private void GoWindowShopping(GameObject go)
    {
        SendMessage(new Message(MessageConst.MODULE_GAMEPLAY_GOTO_SHOPPING));
    }

    public void ShowGoWindowShopping()
    {
        _gameChooser.gameObject.Hide();
        _goWindwoShopping.gameObject.Show();
        _helpBtn.gameObject.Show();
        
        string key = "GameplayView_ShowGoWindowShopping";
        string str = PlayerPrefs.GetString(key);
        if (string.IsNullOrEmpty(str))
        {
            ShowGoShoppingHelp();
            PlayerPrefs.SetString(key, "1688");
            PlayerPrefs.Save();
        }
    }
    
    private void ShowGoShoppingHelp()
    {
        var window = PopupManager.ShowWindow<PopupWindow>("GameMain/Prefabs/PropWindow");
        window.SetGoShopping();    
    }

    private void OnAirborneGame()
    {
        var openLevel = GuideManager.GetOpenUserLevel(ModulePB.Start, FunctionIDPB.DepartmentGame);
        if (GlobalData.PlayerModel.PlayerVo.Level < openLevel)
        {
            FlowText.ShowMessage(I18NManager.Get("GamePlay_Hint1", openLevel));
        }
        else
        {
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_AIRBORNEGAME, false);
        }
    }

    private void OnTakePhotoGame()
    {
        var openLevel = GuideManager.GetOpenUserLevel(ModulePB.Shopping, FunctionIDPB.TakePhoto);
        if (GlobalData.PlayerModel.PlayerVo.Level < openLevel)
        {
            FlowText.ShowMessage(I18NManager.Get("GamePlay_Hint1", openLevel));
        }
        else
        {
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_TAKEPHOTOSGAME, false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _arrowContent.gameObject.Hide();
        }
    }

    public void IsShowArrow(bool isShow)
    {
        _arrowContent.gameObject.SetActive(isShow);
    }

    public void SetRedPoint()
    {
        _supporterActBanner.Find("RedPoint").gameObject.SetActive(GlobalData.DepartmentData.CanGetSupporterActAward);

        int openlevel = GuideManager.GetOpenUserLevel(ModulePB.Visiting, FunctionIDPB.VisitingEntry);
        _visitunlock.Find("Unlock").GetText().text = I18NManager.Get("Visit_LockLevel", openlevel);
        _visitunlock.gameObject.SetActive(GlobalData.PlayerModel.PlayerVo.Level < openlevel);

        int openlevel1 = GuideManager.GetOpenUserLevel(ModulePB.CardMemories, FunctionIDPB.CardMemoriesEntry);
        _recollectionunlock.Find("Unlock").GetText().text =
            I18NManager.Get("Visit_LockLevel", openlevel1); 
        _recollectionunlock.gameObject.SetActive(GlobalData.PlayerModel.PlayerVo.Level < openlevel1);

        int openlevel2 = GuideManager.GetOpenUserLevel(ModulePB.EncourageAct, FunctionIDPB.EncourageActEntry);
        _encorageactunlock.Find("Unlock").GetText().text =
            I18NManager.Get("Visit_LockLevel", openlevel2);                 
        _encorageactunlock.gameObject.SetActive(GlobalData.PlayerModel.PlayerVo.Level < openlevel2);

        int openlevelG = GuideManager.GetOpenUserLevel(ModulePB.Shopping, FunctionIDPB.FunctionShopping);
        _goWindowShoppingBannerUnlock.Find("Unlock").GetText().text =
            I18NManager.Get("Visit_LockLevel", openlevelG);          
        _goWindowShoppingBannerUnlock.gameObject.SetActive(GlobalData.PlayerModel.PlayerVo.Level < openlevelG);
        
        
        //练习室开放等级
        int openlevel3 = GuideManager.GetOpenUserLevel(ModulePB.PracticeRoom, FunctionIDPB.PracticeRoomEntry);
        _trainingRoomUnlock.Find("Unlock").GetText().text = I18NManager.Get("Visit_LockLevel", openlevel3);          
        _trainingRoomUnlock.gameObject.SetActive(GlobalData.PlayerModel.PlayerVo.Level <openlevel3);
    }

    private void GotoTrainingRoomBanner(GameObject go)
    {
        // FlowText.ShowMessage(I18NManager.Get("Common_Expect"));
        // return;
        SendMessage(new Message(MessageConst.MODULE_GAMEPLAY_GOTO_TRAININGROOM));
    }

    private void GotoVisit(GameObject go)
    {
        SendMessage(new Message(MessageConst.MODULE_GAMEPLAY_GOTO_VISIT));
    }

    private void GotoExerciseRoom(GameObject go)
    {
        SendMessage(new Message(MessageConst.MODULE_GAMEPLAY_GOTO_EXERCISE_ROOM));
    }

    private void GotoRecollection(GameObject go)
    {
        SendMessage(new Message(MessageConst.MODULE_GAMEPLAY_GOTO_RECOLLECTION));
    }

    //应援活动
    private void GotoSupporterActivity(GameObject go)
    {
        SendMessage(new Message(MessageConst.MODULE_GAMEPLAY_GOTO_SUPPORTERACTIVITY));
    }

    private void GotoMainLine(GameObject go)
    {
        SendMessage(new Message(MessageConst.MODULE_GAMEPLAY_GOTO_MAIN_LINE));
    }

    public void GoBack()
    {
        if (_goWindwoShopping.gameObject.activeSelf)
        {
            _goWindwoShopping.gameObject.Hide();
            _gameChooser.gameObject.Show();
            _helpBtn.gameObject.Hide();
        }
        else
        {
            ModuleManager.Instance.GoBack();
        }
    }
}