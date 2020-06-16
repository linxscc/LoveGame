using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;

public class CapsuleBattleView : View
{
   private int _step;
    private Animator _abilityAnimator;
    private Transform _battleCommon;
    private Transform _fans;
    private Transform _mask;

    private Transform _ani01;
    private Transform _ani02;

    private Animator _prepareGiftAni; //准备礼物动画
    private Animator _concertAni; //演唱会动画
    private Animator _lightsAni; //灯光动画

    private void Awake()
    {
        _abilityAnimator = transform.Find("AbilityAnimation").GetComponent<Animator>();

        _fans = transform.Find("Fans");
        _mask = transform.Find("Mask");

        _ani01 = transform.Find("Ani01");
        _ani02 = transform.Find("BackGround/02BG/Ani02");

        _prepareGiftAni = _ani01.GetComponent<Animator>();
        _concertAni = _ani02.Find("01").GetComponent<Animator>();
        _lightsAni = _ani02.Find("02").GetComponent<Animator>();

        CalculateOffset();

        _step = 0;
        Step1();
    }

    private void CalculateOffset()
    {
        float w = 1080f;

        var rect = _ani01.GetComponent<RectTransform>();

        if (w != rect.rect.width)
        {
            var offset = (rect.rect.width - w) / 2;
            if (offset < 0)
            {
                offset = -offset;
            }

            rect.anchoredPosition = new Vector2(offset, rect.anchoredPosition.y);
        }
    }

    private void Step1()
    {
        _step++;
        _abilityAnimator.gameObject.Show();

        _abilityAnimator.Play("Normal", 0, 0);

        ClientTimer.Instance.DelayCall(Step2, 1.0f);
    }

    private void Start()
    {
        _battleCommon = transform.parent.Find("CapsuleBattleCommon(Clone)");
        var smallStar = _battleCommon.Find("PowerBar/ProgressBar/Bar/smallStar").GetComponent<RectTransform>();
        var bigStar = _battleCommon.Find("PowerBar/ProgressBar/Bar/bigStar").GetComponent<RectTransform>();
        StartCoroutine(StarRotation(smallStar, bigStar));
    }

    public void ResetView()
    {
        _step = 0;
        Step1();
        ResetFans(); //重置粉丝设置
        ResetMask(); //重置Mask设置
        RestConcertAni();
        ResetPrepareGifeAni();
        transform.Find("BackGround/01BG").gameObject.SetActive(false);
        transform.Find("BackGround/02BG").gameObject.SetActive(false);
    }

    private void Step2()
    {
        //应援会能力值加到进度条动画
        SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_SHOW_SUPPORTER_POWER));

        ClientTimer.Instance.DelayCall(() =>
        {
            SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_SHOW_SUPPORTER_VIEW));
            _step++;
        }, 0.6f);
    }


    public void InitData(CapsuleBattleModel model)
    {
        transform.GetText("AbilityAnimation/Active/Text").text = model.Active + "";
        transform.GetText("AbilityAnimation/Financial/Text").text = model.Financial + "";
        transform.GetText("AbilityAnimation/Resource/Text").text = model.Resource + "";
        transform.GetText("AbilityAnimation/Transmission/Text").text = model.Transmission + "";
    }

    IEnumerator StarRotation(RectTransform smallStar, RectTransform bigStar)
    {
        while (true)
        {
            smallStar.Rotate(-Vector3.forward * Time.deltaTime * 500.0f);
            bigStar.Rotate(-Vector3.forward * Time.deltaTime * 500.0f);
            yield return null;
        }
    }

    private Queue<string> _getFansInfo;

    public void GetFansInfo(Queue<string> fansInfo)
    {
        _getFansInfo?.Clear();
        _getFansInfo = fansInfo;

        if (_step == 2) //获取完粉丝图片路径数据后，下一步就是显示粉丝对话。
        {
            FirstShowFans();
        }
    }

    private void FirstShowFans()
    {
        ResetFans();
        SetFans(I18NManager.Get("Battle_Prepare"));
    }

    public void SecondShowFans(string dialogue)
    {
        ResetFans();
        var str = I18NManager.Get("Battle_Hint");
        SetFans(str + dialogue, 0.2f);
    }

    private void SetFans(string dialogue, float time = 0)
    {
        ClientTimer.Instance.DelayCall(() =>
        {
            _fans.gameObject.SetActive(true);

            var bfd = _fans.Find("DialogFrame").GetComponent<BattleFansDialogue>();

            _fans.Find("DialogFrame/ContextMask/ContentTxt").GetComponent<Text>().text = "";

            var fansImage = _fans.GetRawImage("FansImage");
            var bgImage = _fans.GetImage("DialogFrame/Bg");
            var raw = _fans.GetRawImage("DialogFrame/RawImage");
            var nameBg = _fans.GetImage("DialogFrame/RawImage/Image");
            var nameText = _fans.GetText("DialogFrame/RawImage/NameText");
            fansImage.texture = ResourceManager.Load<Texture>(_getFansInfo.Dequeue(), ModuleName);

            if (_getFansInfo.Count == 1)
            {
                Tween alpha1 = fansImage.DOColor(new Color(fansImage.color.r, fansImage.color.g, fansImage.color.b, 1),
                    0.5f);
                Tween alpha2 = bgImage.DOColor(new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 1), 0.5f);
//                Sequence tween = DOTween.Sequence()
//                    .Join(alpha1)
//                    .AppendInterval(0.5f)
//                    .Join(alpha2);

                bfd.SetData(dialogue);
//                tween.onComplete = () => { };
            }
            else if (_getFansInfo.Count == 0)
            {
                var rect = _fans.Find("DialogFrame/ContextMask/ContentTxt").GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(381, -19.8f);
                rect.sizeDelta = new Vector2(581.5f, 204);
                nameText.text = GlobalData.PlayerModel.PlayerVo.UserName;

                Tween alpha1 = raw.DOColor(new Color(raw.color.r, raw.color.g, raw.color.b, 1), 0.5f);
                Tween alpha2 = bgImage.DOColor(new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 1), 0.5f);
                Tween alpha3 = nameBg.DOColor(new Color(nameBg.color.r, nameBg.color.g, nameBg.color.b, 1), 0.5f);
                Tween alpha4 = nameText.DOColor(new Color(nameText.color.r, nameText.color.g, nameText.color.b, 1),
                    0.5f);

                Sequence tween = DOTween.Sequence()
                    .Join(alpha1)
                    .Join(alpha2)
                    .Join(alpha3)
                    .Join(alpha4);

                tween.onComplete = () => { bfd.SetData(dialogue); };
            }

            //如果点击让打字加速
            PointerClickListener.Get(_fans.gameObject).onClick = go => { bfd.IsFastSpeed = true; };
            bfd.OnStepEnd = TypingEnd;
        }, time);
    }


    private void TypingEnd()
    {
        if (_getFansInfo.Count != 0)
        {
            //ToDo...播放大巴车声音
        }

        _fans.gameObject.transform.RemoveComponent<PointerClickListener>();

        PointerClickListener.Get(_fans.gameObject).onClick = go =>
        {
            ResetFans();
            //根据粉丝队列长度可以判断出之后是入场动画，还是出现演唱会
            if (_getFansInfo.Count != 0)
            {
                //过场动画之前先隐藏进度条
                SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_HIDE_PROGRESS));

                //过场动画
                ShowMask();
            }
            else
            {
                //显示选星缘
                ClientTimer.Instance.DelayCall(() => { SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_NEXT)); }, 0.2f);
            }
        };
    }

    private void ResetFans()
    {
        _fans.gameObject.SetActive(false);
        var fansImage = _fans.GetRawImage("FansImage");
        var bgImage = _fans.GetImage("DialogFrame/Bg");
        var raw = _fans.GetRawImage("DialogFrame/RawImage");
        var nameBg = _fans.GetImage("DialogFrame/RawImage/Image");
        var nameText = _fans.GetText("DialogFrame/RawImage/NameText");

        var rect = _fans.Find("DialogFrame/ContextMask/ContentTxt").GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(93.3f, -19.8f);
        rect.sizeDelta = new Vector2(869.4f, 184.2f);

        fansImage.color = new Color(fansImage.color.r, fansImage.color.g, fansImage.color.b, 0);
        bgImage.color = new Color(bgImage.color.r, bgImage.color.g, bgImage.color.b, 0);
        raw.color = new Color(raw.color.r, raw.color.g, raw.color.b, 0);
        nameBg.color = new Color(nameBg.color.r, nameBg.color.g, nameBg.color.b, 0);
        nameText.color = new Color(nameText.color.r, nameText.color.g, nameText.color.b, 0);
    }


    private void ShowMask()
    {
        //当Mask过场动画运行到一半时，先把送礼背景图显示出来
        ClientTimer.Instance.DelayCall(() =>
        {
            _ani01.gameObject.SetActive(true);
            _prepareGiftAni.enabled = false;
            //_ani01.Find("01/01").gameObject.SetActive(true);
            transform.Find("BackGround/01BG").gameObject.SetActive(true);

            for (int i = 0; i < _ani01.transform.childCount; i++)
            {
                _ani01.transform.GetChild(i).gameObject.SetActive(false);
            }

            _ani01.Find("lz_01").gameObject.SetActive(false);
            _ani01.Find("Particle System").gameObject.SetActive(false);
        }, 0.3f);


        _mask.gameObject.Show();
        _mask.GetComponent<Image>().sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Battle_white");
        _mask.DOLocalMoveX(-2200, 1.0f).onComplete = () =>
        {
            //当过场动画结束后播放准备礼物动画
            _prepareGiftAni.enabled = true;
            _prepareGiftAni.Play("New Animation", 0, 0);
            for (int i = 0; i < _ani01.childCount; i++)
            {
                _ani01.transform.GetChild(i).gameObject.SetActive(true);
            }

            var ps = _ani01.GetComponents<ParticleSystem>();
            foreach (var t in ps)
            {
                t.Play();
            }

            ClientTimer.Instance.DelayCall(
                () =>
                {
                    SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_CHANGE_POWER, Message.MessageReciverType.DEFAULT,
                        _tempPower));
                }, 0.1f);

            //发送第二次显示粉丝Msg
            SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_SECOND_SHOW_FANS));
        };
    }

    private void ResetMask()
    {
        _mask.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        _mask.GetComponent<RectTransform>().anchoredPosition = new Vector2(2200, 0);
        _mask.gameObject.Hide();
    }


    private int _tempPower;

    public void GetPower(int power)
    {
        _tempPower = power;
        Debug.LogError(_tempPower);
    }

    private Queue<int> _getRoleIds;

    #region 荧光棒动画

    private bool _lightAniStart;
    private RawImage _lightstick;
    private int _lightStickIndex;
    private int _intervalCount;
    private int _lightStickDirection;

    #endregion

    public void GetRolesId(Queue<int> roleIds)
    {
        _getRoleIds?.Clear();
        _getRoleIds = roleIds;

        Debug.LogError("_getRoleIds.Count===>" + _getRoleIds.Count);

        //显示演唱会动画
        ShowConcertAni();
    }


    /// <summary>
    /// 显示演唱会动画
    /// </summary>
    private void ShowConcertAni()
    {
        ResetPrepareGifeAni();

        _ani02.transform.gameObject.SetActive(true);
        // _ani02.Find("01/01").gameObject.SetActive(true);
        transform.Find("BackGround/02BG").gameObject.SetActive(true);
        transform.Find("BackGround/01BG").gameObject.SetActive(false);
        _concertAni.enabled = false;
        _lightsAni.enabled = false;


        for (int i = 0; i < _concertAni.gameObject.transform.childCount; i++)
        {
            _concertAni.gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < _lightsAni.gameObject.transform.childCount; i++)
        {
            _lightsAni.gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }

        var tx = _ani02.GetComponents<ParticleSystem>();

        foreach (var t in tx)
        {
            t.Stop();
        }

        //等待0.2s播放演出会动画
        ClientTimer.Instance.DelayCall(() =>
        {
            _concertAni.enabled = true;
            _lightsAni.enabled = true;

            _concertAni.Play("DH_01", 0, 0);
            _lightsAni.Play("DH_02", 0, 0);

            for (int i = 0; i < _concertAni.gameObject.transform.childCount; i++)
            {
                if (_concertAni.gameObject.transform.GetChild(i).name == "09")
                {
                    continue;
                }

                _concertAni.gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }

            for (int i = 0; i < _lightsAni.gameObject.transform.childCount; i++)
            {
                _lightsAni.gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }

            foreach (var t in tx)
            {
                t.Play();
            }

            ClientTimer.Instance.DelayCall(RoleImageAni, 0.3f);
            ClientTimer.Instance.DelayCall(LightStrickAni, 0.36f);
        }, 0.2f);
    }

    private void LightStrickAni()
    {
        _lightstick = transform.GetRawImage("BackGround/02BG/Ani02/Lightstick");
        _lightAniStart = true;

        _lightstick.gameObject.Show();

        _lightStickIndex = 0;

        _intervalCount = 0;
    }

    private void Update()
    {
        if (_lightAniStart == false)
            return;

        _intervalCount++;

        if (_intervalCount > 3)
        {
            _intervalCount = 0;

            if (_lightStickIndex >= 3)
            {
                _lightStickDirection = -1;
            }
            else if (_lightStickIndex <= 0)
            {
                _lightStickDirection = 1;
            }

            _lightstick.texture =
                ResourceManager.Load<Texture>("Battle/Image/lightStick" + (_lightStickIndex + 1), ModuleName);

            _lightStickIndex += _lightStickDirection;
        }
    }

    private void ResetPrepareGifeAni()
    {
        _ani01.transform.gameObject.SetActive(false);
        _prepareGiftAni.Play("New Animation", 0, 0);
        _prepareGiftAni.enabled = false;
    }

    private void RestConcertAni()
    {
        _ani02.transform.gameObject.SetActive(false);
        _concertAni.Play("DH_01", 0, 0);
        _lightsAni.Play("DH_02", 0, 0);
        _concertAni.enabled = false;
        _lightsAni.enabled = false;
    }


    /// <summary>
    /// 显示演唱会男主入场动画
    /// </summary>
    private void RoleImageAni()
    {
        if (_getRoleIds.Count == 0)
        {
            Debug.LogError("演唱会播放完毕");
            ClientTimer.Instance.DelayCall(() =>
            {
                SendMessage(new Message(MessageConst.CMD_CAPSULEBATTLE_FANS_CALL_ANIMATION_FINISH));
                return;
            }, 1.0f);
        }
        else
        {
            var image = transform.Find("BackGround/02BG/Ani02/RoleImage").GetComponent<RawImage>();
            var rect = image.GetComponent<RectTransform>();

            var roleY = 0f; //初始值给个0
            var roleId = _getRoleIds.Dequeue();

            int offsetX = 0;
            if (roleId == 1)
            {
                rect.sizeDelta = new Vector2(1440, 1602);
            }
            else if (roleId == 2)
            {
                rect.sizeDelta = new Vector2(1128, 1662);
//                offsetX = -66;
            }
            else if (roleId == 3)
            {
                rect.sizeDelta = new Vector2(2193, 1736);
                offsetX = 300;
            }
            else if (roleId == 4)
            {
                rect.sizeDelta = new Vector2(1440, 1626);
            }

            rect.anchoredPosition = new Vector2(1900, -roleY);

            image.texture = ResourceManager.Load<Texture>("Battle/Image/" + roleId, ModuleName);
            Tween move1 = rect.DOAnchorPos(new Vector2(offsetX, -roleY), 0.5f);
            Tween move2 = rect.DOAnchorPos(new Vector2(-1900, -roleY), 0.5f);

            Sequence tween = DOTween.Sequence()
                .Append(move1)
                .AppendInterval(0.5f)
                .Append(move2);

            tween.OnComplete(RoleImageAni);
        }
    }
}
