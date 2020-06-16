using System;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public enum IconType
{
    WeChatFriend,
    WeChatFriendCircle,
    QQFriend,
    SinaWeibo,
    Facebook,
    Twitter,
    Alipay
}

public class IconSelectWindow : Window
{


    RectTransform _content;
    Text _title;
    private void Awake()
    {
        _content = transform.Find("Scroll View").GetComponent<ScrollRect>().content;
        _title = transform.GetText("Text");
    }


    public Action<IconType> clickCallback =null;

    public void SetData(string title="",params IconType[] iconTypes)
    {
        _title.text = title;
        for (int i = 0; i < iconTypes.Length; i++)
        {
            var v = iconTypes[i];
            var prefab = InstantiatePrefab("Prefabs/ShareItem", _content);
            string path = "UIAtlas_Common_Share_" + v.ToString();
            prefab.transform.GetImage("Image").sprite = AssetManager.Instance.GetSpriteAtlas(path);
            UIEventListener.Get(prefab).onClick = (obj) =>
            {
                Debug.Log("ClickCallback " + v);
                clickCallback?.Invoke(v);
            };
        }
    }
}
