using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class AirborneItem : MonoBehaviour {

    private float _speed = 250;
    private Transform _umbrella1;
    private Transform _umbrella2;
    private Transform _itemOutline;
    private Transform _specialItem;

    public ItemTypeEnum ItemType
    {
        get
        {
            return _vo.Itemtype;
        }
    }

    public bool SetMoving
    {
        set
        {
            _isMoving = value;
        }
        get
        {
            return _isMoving;
        }
    }
    bool _isMoving = false;

    private void Awake()
    {
        _umbrella1 = transform.Find("Umbrella/1");
        _umbrella2 = transform.Find("Umbrella/2");
        _itemOutline = transform.Find("ItemOutline");
        _specialItem = transform.Find("SpecialItem");
    }

    private void FixedUpdate()
    {
        if (!_isMoving)
            return;
        DoMoving();
    }

    void DoMoving()
    {
        float dis = _speed * Time.deltaTime;
        transform.localPosition = new Vector3(
              transform.localPosition.x ,
              transform.localPosition.y - dis,
              transform.localPosition.z
              );
    }
    AirborneGameRunningItemVo _vo;
    public void SetData(AirborneGameRunningItemVo vo,Vector3  startPos)
    {
        _vo = vo;
        transform.localPosition = startPos;
        _isMoving = true;
        _speed = vo.Speed * 350;
        SetView(vo);
    }

    private void SetView(AirborneGameRunningItemVo vo)
    {
        string iconPath = "";
        switch (vo.Itemtype)
        {
            case ItemTypeEnum.Normal:
            case ItemTypeEnum.Rare:
            case ItemTypeEnum.GirlStar:
            case ItemTypeEnum.Gems:
                _umbrella1.gameObject.Show();
                _umbrella2.gameObject.Hide();
                _itemOutline.gameObject.Show();
                _specialItem.gameObject.Hide();
                iconPath = GlobalData.PropModel.GetPropPath(vo.ResourceId);
                _itemOutline.Find("Item").GetComponent<Image>().sprite = ResourceManager.Load<Sprite>(iconPath, ModuleConfig.MODULE_AIRBORNEGAME);
                _itemOutline.Find("Text").GetComponent<Text>().text = "x" + vo.Count;
                break;
            case ItemTypeEnum.Dead:
                _umbrella1.gameObject.Hide();
                _umbrella2.gameObject.Hide();
                _itemOutline.gameObject.Hide();
                _specialItem.gameObject.Show();
                iconPath = "UIAtlas_AirborneGame_" + vo.Itemtype;
                _specialItem.GetComponent<Image>().sprite = AssetManager.Instance.GetSpriteAtlas(iconPath);
                Debug.LogError("SetView  " + vo.Itemtype + "  AirborneItem  " + gameObject.name);
                break;
            case ItemTypeEnum.Double:
            case ItemTypeEnum.Overtime:
                _umbrella1.gameObject.Hide();
                _umbrella2.gameObject.Show();
                _itemOutline.gameObject.Hide();
                _specialItem.gameObject.Show();
                iconPath = "UIAtlas_AirborneGame_" + vo.Itemtype;
                _specialItem.GetComponent<Image>().sprite = AssetManager.Instance.GetSpriteAtlas(iconPath);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        Debug.Log(c.name);
        if (c.name == "Player") 
        {
            Debug.Log("OnTriggerEnter2D   Player ");
            EventDispatcher.TriggerEvent<AirborneGameRunningItemVo>(EventConst.AirborneGameItemOnTriggerEnter2D, _vo);

            AudioManager.Instance.PlayEffect("progress_bar");
        }
        else  if (c.name == "Bottom")
        {
            Debug.Log("OnTriggerEnter2D   Bottom ");
        }
        _isMoving = false;
        transform.gameObject.SetActive(false);
    }
}
