using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Framework.GalaSports.Core.Message;

public class VisitFirstPassAwardWindow : Window
{

    Transform _puzzle;
    Transform _props;

    private void Awake()
    {
        _puzzle = transform.Find("Puzzle");
        _props = transform.Find("Props");
        transform.Find("GetBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            SendMessage(new Message(MessageConst.CMD_VISIT_LEVEL_FIRSTAWARDGET_CLICK, MessageReciverType.CONTROLLER, _vo.LevelId));
        });
    }
    private VisitLevelVo _vo;
    public void Init(VisitLevelVo vo)
    {
        if (vo.levelFirstPassPB.Awards.Count == 0)
            return;
        _vo = vo;

        transform.Find("GetBtn").gameObject.SetActive(vo.IsPass);

        AwardPB aPb = vo.levelFirstPassPB.Awards[0];

        if (aPb.Resource == ResourcePB.Puzzle)
        {
            _puzzle.gameObject.Show();
            _props.gameObject.Hide();
            SetShowPuzzle(aPb);
        }
        else if (aPb.Resource == ResourcePB.Item) 
        {
            _props.gameObject.Show();
            _puzzle.gameObject.Hide();
            SetShowProps(aPb);
        }
    }

    private Image _cardQualityImage;

    private void SetShowPuzzle(AwardPB apb)
    {

        var card = GlobalData.CardModel.GetCardBase(apb.ResourceId);
        _cardQualityImage = transform.Find("Puzzle/CardQualityImage").GetComponent<Image>();
        transform.Find("Puzzle/NameText").GetComponent<Text>().text = card.CardName+ I18NManager.Get("Visit_Hint5") + apb.Num.ToString();

        bool isPuzzle;
        isPuzzle = apb.Resource == ResourcePB.Puzzle ? true : false;
        transform.Find("Puzzle/PuzzleRawImage").gameObject.SetActive(isPuzzle);
        transform.Find("Puzzle/RawImage").gameObject.SetActive(isPuzzle);
        _cardQualityImage.sprite = AssetManager.Instance.GetSpriteAtlas(CardUtil.GetNewCreditSpritePath(card.Credit));
       // _cardQualityImage.SetNativeSize();

        RawImage cardImage = transform.Find("Puzzle/Mask/CardImage").GetComponent<RawImage>();
        Texture texture = ResourceManager.Load<Texture>("Card/Image/SmallCard/"+ apb.ResourceId, ModuleConfig.MODULE_VISIT);

        if (texture == null)
            texture = ResourceManager.Load<Texture>("Card/Image/SmallCard/1420", ModuleConfig.MODULE_VISIT);
        cardImage.texture = texture;

        transform.Find("Puzzle/PuzzleRawImage/Text").GetText().text = apb.Num.ToString();

    }

    private RawImage _propImage;
    private Text _propNameTxt;
    private Text _ownTxt;
    private void SetShowProps(AwardPB awardPB)
    {
        Debug.Log(awardPB.ResourceId + " " + awardPB.Num + " " + awardPB.Resource);
        string path = "";
        string name = "";
        if (awardPB.Resource == ResourcePB.Item)
        {
            name = GlobalData.PropModel.GetPropBase(awardPB.ResourceId).Name;
        }
        else
        {
            name = ViewUtil.ResourceToString(awardPB.Resource);
        }

        if (awardPB.Resource == ResourcePB.Gold)
        {
            path = "Prop/particular/" + PropConst.GoldIconId;
        }
        else if (awardPB.Resource == ResourcePB.Gem)
        {
            path = "Prop/particular/" + PropConst.GemIconId;
        }
        else if (awardPB.Resource == ResourcePB.Power)
        {
            path = "Prop/particular/" + PropConst.PowerIconId;
        }
        else if (awardPB.Resource == ResourcePB.Card)
        {
            path = "Head/" + awardPB.ResourceId;

            CardPB pb = GlobalData.CardModel.GetCardBase(awardPB.ResourceId);
            name = CardVo.SpliceCardName(pb.CardName, pb.Player);
        }
        else if (awardPB.Resource == ResourcePB.Memories)
        {
            path = "Prop/particular/123456789";
        }
        else
        {
            path = "Prop/" + awardPB.ResourceId;
        }
        _propImage = transform.Find("Props/PropImage").GetComponent<RawImage>();
        _propNameTxt = transform.Find("Props/PropNameTxt").GetComponent<Text>();
        _ownTxt = transform.Find("Props/ObtainText").GetComponent<Text>();

        _propImage.texture = ResourceManager.Load<Texture>(path);
        _propNameTxt.text = name;
        _ownTxt.text = I18NManager.Get("GameMain_ActivityAwardItemObtainText", awardPB.Num);
    }
}
