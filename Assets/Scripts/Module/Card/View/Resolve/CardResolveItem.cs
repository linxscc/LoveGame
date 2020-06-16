using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace game.main
{
    public class CardResolveItem : MonoBehaviour, IPointerClickHandler
    {
        private Image _cardQualityImage;
        private ResolveCardVo _data;
        private Button _reduceBtn;
        private Text _numTxt;
        private Image _numRegBg;

        private void Awake()
        {
            _reduceBtn = transform.Find("ReduceBtn").GetComponent<Button>();
            _cardQualityImage = transform.Find("CardQualityImage").GetComponent<Image>();

            _numTxt = transform.Find("NumImage/NumText").GetComponent<Text>();
            _numRegBg = transform.Find("NumImage").GetComponent<Image>();

            _reduceBtn.onClick.AddListener(ReduceSelect);

            PointerClickListener.Get(gameObject).onClick = AddSelect;
        }

        private void AddSelect(GameObject go)
        {
            if (_data.SelectedNum < _data.Num)
                _data.SelectedNum++;
            else
                return;
            
            SetNumText();

            EventDispatcher.TriggerEvent(EventConst.CardResolveSelectedChange, _data);
        }

        private void ReduceSelect()
        {
            if (_data.SelectedNum > 0)
                _data.SelectedNum--;
            else
                return;
            
            SetNumText();

            EventDispatcher.TriggerEvent(EventConst.CardResolveSelectedChange, _data);
        }

        public void SetData(ResolveCardVo vo)
        {
            _data = vo;

            transform.Find("NameText").GetComponent<Text>().text = vo.Name;

            SetNumText();

            _cardQualityImage.sprite = AssetManager.Instance.GetSpriteAtlas(CardUtil.GetNewCreditSpritePath(vo.Credit));
            //_cardQualityImage.SetNativeSize();
            
            RawImage cardImage = transform.Find("Mask/CardImage").GetComponent<RawImage>();
            Texture texture = ResourceManager.Load<Texture>(vo.CardPath, ModuleConfig.MODULE_CARD);

            if (texture == null)
            {
                Debug.LogError("NoCard"+vo.CardPath);
                texture = ResourceManager.Load<Texture>("Card/Image/MiddleCard/1000", ModuleConfig.MODULE_CARD);
            }

            cardImage.texture = texture;
        }

        private void SetNumText()
        {
            _numTxt.text = _data.SelectedNum + "/" + _data.Num;
            
            //关键就在这里！
            _reduceBtn.gameObject.SetActive(_data.SelectedNum != 0);
//            _numRegBg.gameObject.SetActive(_data.SelectedNum != 0);
            _numRegBg.sprite = AssetManager.Instance.GetSpriteAtlas(_data.SelectedNum != 0?"UIAtlas_Card_cardnumRedpoint":"UIAtlas_Card_cardnumgraypoint");

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventDispatcher.TriggerEvent<ResolveCardVo>(EventConst.CardResolveClick, _data);
        }
    }
}