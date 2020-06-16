using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RankingItem : MonoBehaviour
{
    private Text _rankingNum; //名次
    private Image _playerIcon;
    private Text _playerName;
    private Image _grade;   //评分SSS
    private Text _score;   //得分
    
    private void Awake()
    {
        _rankingNum = transform.GetText("RankingNum/Text");
        _playerIcon = transform.GetImage("Icon");
        _playerName = transform.GetText("Name");
        _grade = transform.GetImage("Grade");
        _score = transform.GetText("Score");

    }

    public void SetData(RankingVO vo)
    {
        _rankingNum.text = vo.RatingStr;
        _playerName.text = vo.UserName;
        _score.text = vo.Score.ToString();
    }
}
