using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Proto;
using DataModel;
using Assets.Scripts.Module;
using game.main;
using Assets.Scripts.Module.Framework.Utils;
using game.tools;

public class ActivityCapsuleTemplateDrawPoolItem : MonoBehaviour {

    private Image _light;
    private Image _bg;
    private Image _selectImg;
    private RawImage _icon;
    private Image _credit;
    private Text _countText;
    private GameObject _disableObj;
    RewardVo _rewardData;

    public int id = -1;

    private void Awake()
    {
        _light = transform.Find("light").GetComponent<Image>();
        _bg = transform.Find("Bg").GetComponent<Image>();
        _selectImg = transform.Find("Select").GetComponent<Image>();
        _icon = transform.Find("Icon").GetComponent<RawImage>();
        _credit = _icon.transform.Find("credit").GetComponent<Image>();
        _countText = transform.Find("Text").GetComponent<Text>();
        _disableObj = transform.Find("disable").gameObject;
    }

    // Use this for initialization
    void Start () {
        _bg.raycastTarget = true;
        PointerClickListener.Get(_bg.gameObject).onClick = go =>
        {
            OnClick();
        };
    }

    public void SetData(ActivityCapsuleItemPB data, ActivityCapsuleTemplateModel model)
    {
        _light.gameObject.SetActive(false);
        _credit.gameObject.SetActive(false);
        _rewardData = new RewardVo(data.AwardPB);
        PropUtils.SetPropItemIcon(_rewardData, _icon, ModuleConfig.MODULE_ACTIVITYCAPSULETEMPLATE, false, false);
        _icon.color = Color.white;
        _countText.text = "x" + _rewardData.Num;

        CheckBg(_rewardData);
        SetDisable(model.IsGainCapsuleItem(data.Id));

    }

    private void CheckBg(RewardVo data)
    {
        if (data.Resource == ResourcePB.Card)
        {
            int credit = (data.Id % 1000) / 100;
            if (credit == 0)
            {
                _bg.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_ActivityCapsuleTemplate_Item_Bg_SSR");
                _bg.SetNativeSize();
                _light.gameObject.SetActive(true);
                _light.color = ColorUtil.HexToColor("FFFF00FF");
                _credit.gameObject.SetActive(true);
            }
            else
            {
                _bg.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_ActivityCapsuleTemplate_Item_Bg_SR");
                _bg.SetNativeSize();
                _light.gameObject.SetActive(true);
                _light.color = ColorUtil.HexToColor("8fe0faFF");
            }
        }
        if(data.Id == 12541)
        {
            _bg.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_ActivityCapsuleTemplate_Item_Bg_SR");
            _bg.SetNativeSize();
            _light.gameObject.SetActive(true);
            _light.color = ColorUtil.HexToColor("8fe0faFF");
        }
    }
	
    public void SetSelectState(bool state)
    {
        _selectImg.gameObject.SetActive(state);
    }

    public void SetDisable(bool state)
    {
        _disableObj.SetActive(state);
        if (state)
            _light.gameObject.SetActive(false);
    }

    private void OnClick()
    {
        if (_rewardData == null) return;
        PropUtils.ShowPropDesc(_rewardData);
    }
}
