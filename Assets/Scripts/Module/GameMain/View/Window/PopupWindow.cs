using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using DG.Tweening;
using game.main;
using ICSharpCode.SharpZipLib.Core;
using UnityEngine;
using UnityEngine.UI;
public class PopupWindow : Window
{


     private Transform _parent;
     private Button _leftBtn;
     private Button _rightBtn;
     private int _curIndex;
     private int _maxIndex;
     private float _width;
     private bool _isMove = false;
     private Text _hintText;
     private Text _titleText;
     private Transform _pages;
     private Toggle _todayIsShowTog;
  
     private void Awake()
     {
          _parent = transform.Find("Content");
          _leftBtn = transform.GetButton("Btn/LeftBtn");
          _rightBtn = transform.GetButton("Btn/RightBtn");
          
          _leftBtn.onClick.AddListener(LeftBtn);
          _rightBtn.onClick.AddListener(RightBtn);

          _hintText = transform.GetText("Hint");
          _titleText = transform.GetText("Title/Text");
          _titleText.text = I18NManager.Get("ActivityPopup_Title");
          _pages = transform.Find("Pages");
          _todayIsShowTog = transform.GetToggle("Toggle");
          
     }

   

     private void LeftBtn()
     {
          if (_isMove)
          {
              return; 
          }
         
          _curIndex--;
          if (_curIndex<0)
          {
               _curIndex = 0;
               return;               
          }
          SetHintText(_curIndex);
          
          MoveAni(_width);

     }

     private void RightBtn()
     {
          if (_isMove)
          {
               return; 
          }
         
          _curIndex++;
          SetHintText(_curIndex);
          
          if (_curIndex>_maxIndex)
          {
              base.Close();
              return;
          }       
          MoveAni(-_width);
     }

     private void MoveAni(float offset)
     {
          for (int i = 0; i < _parent.childCount; i++)
          {
               var rect = _parent.GetChild(i).transform.GetComponent<RectTransform>();
               var endPos = rect.localPosition.x + offset;  
               var tween = rect.DOLocalMoveX(endPos,0.5f);
               tween.OnPlay(() => {_isMove = true;});
               tween.OnComplete(() =>
               {
                    _isMove = false;
                    _pages.GetChild(_curIndex).GetComponent<Toggle>().isOn = true;
               });
          }
     }

     private void CreatData(List<ActivityPopupWindowData> datas)
     {
          var prefab = GetPrefab("GameMain/Prefabs/PropWindowItem");
          var page = GetPrefab("GameMain/Prefabs/ActivityPropWindowPageItem");
          
          for (int i = 0; i < datas.Count; i++)
          {
               var item = Instantiate(prefab, _parent, false);
               item.transform.localScale = Vector3.one;
               item.gameObject.name = datas[i].Name;
               
               var rect =  item.GetComponent<RectTransform>();
               var w = rect.GetWidth();
               _width = w;               
               rect.localPosition= new Vector3(rect.localPosition.x+w*i,0);
               item.GetComponent<ActivityPopupWindowDataItem>().SetData(datas[i]);       
               
               
               var pageItem  =Instantiate(page, _pages, false);
               pageItem.transform.localScale = Vector3.one;
              // pageItem.gameObject.name = datas[i].ActivityName;
               pageItem.GetComponent<Toggle>().group = _pages.transform.GetComponent<ToggleGroup>();

               if (i==_curIndex)
               {
                    pageItem.GetComponent<Toggle>().isOn = true;
               }
              
          } 
     }
     
     
     public void SetData(ActivityPopupWindowModel model)
     {
          _curIndex = 0;
          _maxIndex = model.GetDate().Count - 1;                   
          CreatData(model.GetDate());
          SetHintText(_curIndex);
          
          _todayIsShowTog.onValueChanged.AddListener((delegate(bool isOn)
          {
               if (isOn)
               {
                  model.SetKey();  
               }
               else
               {
                    model.DeleteKey();
               }
          }));
     }

     protected override void OnClickOutside(GameObject go)
     {
          RightBtn();
     }

    
     
     public void Close()
     {
          base.Close();
     }

     private void SetHintText(int curIndex)
     {                  
          _hintText.text = curIndex==_maxIndex ? I18NManager.Get("ActivityPopup_Bottom1") : I18NManager.Get("ActivityPopup_Bottom2");
     }


     public void SetPlayRuleData(ActivityPlayRuleVo vo)
     {          
        PlayerPrefs.SetString(vo.Key,"");
        _curIndex = 0;
        _maxIndex = vo.Paths.Length - 1;       
        _todayIsShowTog.gameObject.Hide();
        _titleText.text = vo.TitleName;

        CreateRuleItem(vo.Paths);
     }
     
     public void SetGoShopping()
     {
          _curIndex = 0;
          _maxIndex = 1;
          _todayIsShowTog.gameObject.Hide();
          _titleText.text = "逛街介绍";
          CreateRuleItem(new []{"Gameplay/shoppingRule1", "Gameplay/shoppingRule2"});
     }
     
     public void SetTrainingRoom()
     {
          _curIndex = 0;
          _maxIndex = 1;
          _todayIsShowTog.gameObject.Hide();
          _titleText.text = "玩法介绍";
          CreateRuleItem(new []{"TrainingRoom/p1", "TrainingRoom/p2"});
     }

     private void CreateRuleItem(string[] paths)
     {
          var prefab = GetPrefab("GameMain/Prefabs/PropWindowItem");
          var page = GetPrefab("GameMain/Prefabs/ActivityPropWindowPageItem");

          for (int i = 0; i < paths.Length; i++)
          {
             
               var item = Instantiate(prefab, _parent, false);
               item.transform.localScale = Vector3.one;
               item.transform.GetRawImage().texture = ResourceManager.Load<Texture>(paths[i]);
               
               var rect =  item.GetComponent<RectTransform>();
               var w = rect.GetWidth();
               _width = w;               
               rect.localPosition= new Vector3(rect.localPosition.x+w*i,0);
               item.transform.RemoveComponent<ActivityPopupWindowDataItem>();
            
               
               var pageItem  =Instantiate(page, _pages, false);
               pageItem.transform.localScale = Vector3.one;
               // pageItem.gameObject.name = datas[i].ActivityName;
               pageItem.GetComponent<Toggle>().group = _pages.transform.GetComponent<ToggleGroup>();

               if (i==_curIndex)
               {
                    pageItem.GetComponent<Toggle>().isOn = true;
               }
          }
     }
}
