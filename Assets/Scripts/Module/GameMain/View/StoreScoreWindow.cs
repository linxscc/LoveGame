using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using game.main;
using game.tools;
using GalaAccount.Scripts.Framework.Utils;
using UnityEngine;
using UnityEngine.UI;

public class StoreScoreWindow : Window
{

    private Transform _scoretran;
    private Transform _confirmToStoretran;
    private Transform _advicetran;

    private Transform _whiteHeartGroup;
    private Transform _redHeartGroup;
    private Button _okBtn;

    private Button _gotoStoreBtn;
    private Button _latterBtn;

    private InputField _inputField;
    private Button _commitBtn;
    private Button _latterStoreBtn;

    private int _score=0;
    private RawImage _scoreRawImage;
    

    private void Awake()
    {
        _score = 0;
        _scoretran = transform.Find("Score");
        _confirmToStoretran = transform.Find("ConfirmToStore");
        _advicetran = transform.Find("Advice");

        _whiteHeartGroup = transform.Find("Score/HeartGroup/HeartContainerBG");
        for (int i = 0; i < _whiteHeartGroup.childCount; i++)
        {
            PointerClickListener.Get(_whiteHeartGroup.GetChild(i).gameObject).onClick = go => { SetHeartCount(go.transform.GetSiblingIndex()+1); };
        }
        
        _redHeartGroup = transform.Find("Score/HeartGroup/HeartContainer");
        _okBtn = transform.Find("Score/GetBtn").GetButton();
        _okBtn.onClick.AddListener(JumpToScore);

        _gotoStoreBtn = transform.Find("ConfirmToStore/okBtn").GetButton();
        _gotoStoreBtn.onClick.AddListener(GotoStore);
        
        _latterBtn = transform.GetButton("ConfirmToStore/cancelBtn");
        _latterBtn.onClick.AddListener(() =>
        {
            Close();
        });

        _inputField = transform.Find("Advice/AdviceTxt/InputField").GetComponent<InputField>();
        _commitBtn = transform.GetButton("Advice/AdviceTxt/GotoStoreBtn");
        _commitBtn.onClick.AddListener(CommitAdvice);
        _latterStoreBtn = transform.GetButton("Advice/AdviceTxt/LaterBtn");
        _latterStoreBtn.onClick.AddListener(() =>
        {
            Close();
        });

        var npcId = GlobalData.PlayerModel.PlayerVo.NpcId;
        _scoreRawImage = transform.GetRawImage("Score/RoleImage"+npcId);
        _scoreRawImage.texture=ResourceManager.Load<Texture>("Background/PersonIcon/Npc"+npcId); 
        _scoreRawImage.gameObject.Show();
      
        
        
       
        
       

    }

    private void CommitAdvice()
    {
        //提交评论
        SendMessage(new Message(MessageConst.CMD_MAIN_STORESCORECOMMENT,Message.MessageReciverType.CONTROLLER,false,_inputField.text,_score));
        
    }

    private void GotoStore()
    {
        //评分并且发送领奖协议
        Debug.LogError("NoScore");
        SendMessage(new Message(MessageConst.CMD_MAIN_STORESCORECOMMENT,Message.MessageReciverType.CONTROLLER,true,"",_score));
    }

    private void JumpToScore()
    {
        if (_score>=3)
        {
            //显示评分
            _scoretran.gameObject.SetActive(false);
            _confirmToStoretran.gameObject.SetActive(true);
        }
        else
        {
            //显示评论


            if (_score==0)
            {
                 FlowText.ShowMessage(I18NManager.Get("Guide_ChooseComment"));
            }
            else
            {
                _scoretran.gameObject.SetActive(false);
                _advicetran.gameObject.SetActive(true); 
            }
            
        }
    }

    private void SetHeartCount(int count)
    {
        for (int i = 0; i < _redHeartGroup.childCount; i++)
        {
            _redHeartGroup.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < count; i++)
        {
            _redHeartGroup.GetChild(i).gameObject.SetActive(true);
        }

        _score = count;
    }


    
    
}
