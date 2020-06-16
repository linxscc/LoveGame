using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class FindSpriteReferences : EditorWindow
{
    static private FindSpriteReferences _instance;
    public static FindSpriteReferences instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (FindSpriteReferences)EditorWindow.GetWindowWithRect(typeof(FindSpriteReferences), new Rect(0, 0, 1000, 800), true);
                _instance.titleContent = new GUIContent("图集引用查找");
            }
            return _instance;
        }
    }

    private string[] _menuStrs = new string[] { "整张图集所有引用查找", "单张Sprite引用查找" };
    private int _menuId = 0;
    private Vector2 m_scrollPos;
    private void OnGUI()
    {
        _menuId = GUILayout.SelectionGrid(_menuId, _menuStrs, _menuStrs.Length, GUILayout.Width(1000 - 10), GUILayout.Height(40));
        m_scrollPos = GUILayout.BeginScrollView(m_scrollPos, true, true, GUILayout.Height(800 - 50));

        GUILayout.Label("", GUILayout.Height(5));
        if (_menuId == 0)
        {
            FindAllSpriteReferences();
        }
        else if (_menuId == 1)
        {
            FindSingleSpriteReferences();
        }
    }

    [MenuItem("Tools/UI/图集引用查找工具", false, 30003)]
    static void OpenSpriteFindEditor()
    {
        FindSpriteReferences.instance.Show();
    }




    Texture sprites;                                                                        //待查找的Texture
    private Dictionary<string, List<int>> spritesDic = new Dictionary<string, List<int>>();//记录查找的SpriteCount,key为名,value[0]为InstanceID，value[1]为数量。
    StringBuilder sb = new StringBuilder();                                                //输出的StringBuilder
    string _spriteName = string.Empty;                                                     //查找的Texture名
    int stringLength = 0;                                                                  //string分割行数，长度过大GUILayout无法完全输出

    /// <summary>
    /// 通过正则表达式对Guid进行查询,查询速度快。 子图片根据IntanceID匹配。
    /// </summary>
    private void FindAllSpriteReferences()
    {
        //EditorSettings.serializationMode = SerializationMode.ForceText;

        GUILayout.Label("选择整张图集");
        if (sprites)
            _spriteName = sprites.name;


        sprites = (Texture)EditorGUILayout.ObjectField(sprites, typeof(Texture));
        GUILayout.Label("当前选中的图集为：" + _spriteName);
        UnityEngine.Object[] array = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(sprites));
        if (GUILayout.Button("查找", GUILayout.Height(30)))
        {

            stringLength = 0;
            sb = new StringBuilder();
            spritesDic.Clear();
            for (int i = 0; i < array.Length; i++)
            {

                int nativeID = array[i].GetInstanceID();

                DictionaryAddCheck(array[i].name, new List<int>() { nativeID, 0 }, spritesDic);

            }

            ReferencesPrefabs();


            sb.Append("匹配结束");








        }
        GUILayout.Label("-------------------------------------------------------------------------");
        string[] str = sb.ToString().Split(new string[] { "|*|" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string item in str)
        {
            GUILayout.Label(item);
        }
        GUILayout.Label("---------------------各个子Sprite引用数量-----------------------------------------------");
        foreach (KeyValuePair<string, List<int>> item in spritesDic)
        {
            GUILayout.Label(item.Key + "----引用了" + item.Value[1] + "次");
        }
        GUILayout.Label("-------------------------没有在项目中引用的资源-----------------------------------------------");
        foreach (KeyValuePair<string, List<int>> item in spritesDic)
        {
            if (item.Value[1] == 0)
                GUILayout.Label(item.Key + "----引用了" + item.Value[1] + "次");

        }
        GUILayout.EndScrollView();
    }

    private void ReferencesPrefabs()
    {


        if (sprites == null) return;

        string _allFolderPath = Application.dataPath.Replace("Assets", "") + "/";


        List<string> subPaths =  Framework.Utils.FileUtil.GetAllFiles(_allFolderPath, "prefab");
        List<string> _newPath = new List<string>();
        if (subPaths == null) return;
        for (int j = 0; j < subPaths.Count; j++)
        {


            if (!string.IsNullOrEmpty(subPaths[j]))
            {

                GetPrefabReference(subPaths[j]);

            }

        }



    }

    private void GetPrefabReference(string path)
    {

        string assetPath = "Assets" + path.Replace(Application.dataPath, "");
        UnityEngine.Object assetObj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.Object));
        if (!(assetObj is GameObject)) return;
        GameObject prefabGo = (GameObject)assetObj;


        Image[] sps = prefabGo.GetComponentsInChildren<Image>(true);
        if (sps != null)
        {
            for (int j = 0; j < sps.Length; j++)
            {
                if (!sps[j].sprite | !sps[j].gameObject)//将没有引用Sprite的Image组件过滤掉。
                    continue;
                List<int> _intanceID;
                if (spritesDic.TryGetValue(sps[j].sprite.name, out _intanceID))
                {
                    if (sps[j].sprite.GetInstanceID() == _intanceID[0])
                    {

                        spritesDic[sps[j].sprite.name][1]++;
                        sb.Append(path + "\n" + prefabGo.name + "---:" + sps[j].gameObject.name + "----引用了----" + sps[j].sprite.name + "\n");
                        stringLength++;
                        if (stringLength >= 50)
                        {
                            sb.Append("|*|");
                            stringLength = 0;
                        }


                    }
                }

            }
        }



    }


    Sprite m_sprite;                                                                    //待查找的Sprite
    Texture2D m_texture;
    private Dictionary<int, int> _singleSpriteCount = new Dictionary<int, int>(); //记录查找的SpriteCount,key为InstanceID,value为数量
    StringBuilder _singleSb = new StringBuilder();                                      //输出的StringBuilder
    string _singleSpriteName = string.Empty;                                            //查找的单个Sprite名
    string _singleTextureName=string.Empty;
    int _currentInstanceID;
    private void ReferencesPrefabsSingle(bool isSprite)
    {


        if (m_sprite == null&&m_texture==null) return;

        string _allFolderPath = Application.dataPath.Replace("Assets", "") + "/";


        List<string> subPaths = Framework.Utils.FileUtil.GetAllFiles(_allFolderPath, "prefab");

        if (subPaths == null) return;
        for (int j = 0; j < subPaths.Count; j++)
        {


            if (!string.IsNullOrEmpty(subPaths[j]))
            {

                GetPrefabReferenceSingle(subPaths[j], isSprite);

            }

        }



    }

    private void GetPrefabReferenceSingle(string path,bool isSprite)
    {

        string assetPath = "Assets" + path.Replace(Application.dataPath, "");
        UnityEngine.Object assetObj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.Object));
        if (!(assetObj is GameObject)) return;
        GameObject prefabGo = (GameObject)assetObj;


        if (isSprite)
        {
            Image[] sps = prefabGo.GetComponentsInChildren<Image>(true);
            if (sps != null)
            {
                for (int j = 0; j < sps.Length; j++)
                {
                    if (!sps[j].sprite | !sps[j].gameObject)//将没有引用Sprite的Image组件过滤掉。
                        continue;
                    int count;
                    if (_singleSpriteCount.TryGetValue(sps[j].sprite.GetInstanceID(), out count))
                    {

                        if (sps[j].sprite.GetInstanceID() == m_sprite.GetInstanceID())
                        {


                            _singleSpriteCount[m_sprite.GetInstanceID()]++;

                            _singleSb.Append(path + "\n" + prefabGo.name + "---:" + sps[j].gameObject.name + "----引用了----" + sps[j].sprite.name + "\n");


                        }
                    }

                }
            }
        }
        else
        {
            RawImage[] textures = prefabGo.GetComponentsInChildren<RawImage>(true);
            if (textures != null)
            {
                for (int j = 0; j < textures.Length; j++)
                {
                    if (!textures[j].texture | !textures[j].gameObject)//将没有引用Sprite的Image组件过滤掉。
                        continue;
                    int count;
                    if (_singleSpriteCount.TryGetValue(textures[j].texture.GetInstanceID(), out count))
                    {

                        if (textures[j].texture.GetInstanceID() == m_texture.GetInstanceID())
                        {


                            _singleSpriteCount[m_texture.GetInstanceID()]++;

                            _singleSb.Append(path + "\n" + prefabGo.name + "---:" + textures[j].gameObject.name + "----引用了----" + textures[j].texture.name + "\n");


                        }
                    }

                }
            }

        }



    }

   


    /// <summary>
    /// 通过Guid获取大图，根据IntanceID匹配小图。
    /// </summary>
    private void FindSingleSpriteReferences()
    {
        //EditorSettings.serializationMode = SerializationMode.ForceText;

        GUILayout.Label("请选择单张子图集");

        m_sprite = (Sprite)EditorGUILayout.ObjectField(m_sprite, typeof(Sprite));
        if (m_sprite)
            _singleSpriteName = m_sprite.name;

        GUILayout.Label("当前选中的图集为：" + _singleSpriteName);

        GUILayout.Label("请选择Texture");

        m_texture = (Texture2D)EditorGUILayout.ObjectField(m_texture, typeof(Texture2D));
        if (m_texture)
            _singleTextureName = m_texture.name;

        GUILayout.Label("当前选中的Texture为：" + _singleTextureName);



        if (m_sprite)
        {
            if (GUILayout.Button("查找单张图集引用", GUILayout.Height(30)))
            {

                if (!m_sprite) return;
                _singleSb = new StringBuilder();
                _singleSpriteCount.Clear();
                _currentInstanceID = m_sprite.GetInstanceID();
                DictionaryAddCheck(_currentInstanceID, 0, _singleSpriteCount);




                ReferencesPrefabsSingle(true);
                sb.Append("匹配结束");
            }
        }

        if (m_texture)
        {
            if (GUILayout.Button("查找单张Texture引用", GUILayout.Height(30)))
            {

                _singleSb = new StringBuilder();
                _singleSpriteCount.Clear();
                _currentInstanceID = m_texture.GetInstanceID();
                DictionaryAddCheck(_currentInstanceID, 0, _singleSpriteCount);




                ReferencesPrefabsSingle(false);
                sb.Append("匹配结束");
            }

        }











        GUILayout.Label("-------------------------------------------------------------------------");
        GUILayout.Label(_singleSb.ToString());
        
        GUILayout.Label("---------------------各个子Sprite引用数量-----------------------------------------------");
        if (_singleSpriteCount.ContainsKey(_currentInstanceID))
        {
            GUILayout.Label(_singleTextureName + "------总共被引用了" + _singleSpriteCount[_currentInstanceID] + "次");
        }
        GUILayout.EndScrollView();
    }

    /// <summary>
    /// 项目中相对路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    static private string GetRelativeAssetsPath(string path)
    {
        return "Assets" + Path.GetFullPath(path).Replace(Path.GetFullPath(Application.dataPath), "").Replace('\\', '/');
    }




    /// <summary>
    /// 字典添加
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="dic"></param>
    private void DictionaryAddCheck<T, V>(T key, V value, Dictionary<T, V> dic)
    {
        if (!dic.ContainsKey(key))
        {
            dic.Add(key, value);
        }
        else
        {
            dic[key] = value;
        }
    }
}