using System.Collections;
using System.Collections.Generic;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class RuleExplainWindow : Window
{


    private RectTransform _rect;
    private Text _rule;
    private Text _title;

    private void Awake()
    {
        _rect = transform.GetRectTransform();
        _rule=transform.GetText("RuleTxt");
        _title = transform.GetText("Title");
    }

    public void SetData(string title,string ruleDesc,Vector2 sizeDelta)
    {
        _title.text = title;
        _rule.text = ruleDesc;
        _rect.sizeDelta =sizeDelta;
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rect); 
    }
    
    

}
