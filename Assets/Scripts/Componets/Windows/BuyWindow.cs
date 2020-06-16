
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class BuyWindow : AlertWindow
{

    [SerializeField] private Text _exchangeNumText;         //兑换次数

    [SerializeField] private RawImage _leftRawImage;        //左侧道具图片
    [SerializeField] private Text _leftNum;                 //左侧兑换数量

    [SerializeField] private RawImage _rightRawImage;
    [SerializeField] private Text _rightNum;

   // [SerializeField] private Text _describeText;            //中间描述

    [SerializeField] private RawImage _personBg;             //人物Image


    private string imagePath = "Prop/particular/";       //图片路径

   
       
    



    protected override void OnInit()
    {
        base.OnInit();
        _exchangeNumText.text = "";   
        _leftNum.text = "";   
        _rightNum.text = "";
       // _describeText.text = "";
       
    }

    public void InitWindowInfo(int buyItemId,int costItemId)
    {
        switch (buyItemId)
        {
            case PropConst.GoldIconId:           //购买金币
                BuyGlod(buyItemId, costItemId);
                break;
            case PropConst.PowerIconId:          //购买体力
                BuyPrower(buyItemId, costItemId);
                break;
            case PropConst.EncouragePowerId:
                BuyEncouragePower(buyItemId, costItemId);  //星源体力购买
                break;
        }
    }

    public void InitWindowInfo(int buyItemId,int costItemId,int costItemNum)
    {
        switch (buyItemId)
        {
            case PropConst.RecolletionIconId:
                BuyRecolletion(buyItemId,costItemId,costItemNum);
                break;
        }
    }

    //购买星缘回忆
    private void BuyRecolletion(int buyItemId,int costItemId,int costItemNum)
    {

        var leftNums = costItemNum;
        var rightNums = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.RESTORE_MEMORIES_POWER_BUY_VALUE);
        _leftRawImage.texture = ResourceManager.Load<Texture>(LeftImageName(costItemId), null, true);
        _rightRawImage.texture = ResourceManager.Load<Texture>(RightImageName(buyItemId), null, true);

        var npcId = GlobalData.PlayerModel.PlayerVo.NpcId;
        var npcImg = transform.GetRawImage("window/PersonBg"+npcId);
        npcImg.texture=ResourceManager.Load<Texture>("Background/PersonIcon/Npc"+npcId, null, true);
        npcImg.gameObject.Show();
     
        _leftNum.text = leftNums.ToString();
        _rightNum.text = rightNums.ToString();
    
        

        Title = I18NManager.Get("Common_RecolletionBuy"); 
        Content = I18NManager.Get("Common_RecolletionBuyContent", leftNums, rightNums);
       
    }


    //购买金币
    private void BuyGlod(int buyItemId, int costItemId)
    {
        var exchangeNum = GlobalData.PlayerModel.PlayerVo.GoldNum;     
        var itemRule = GlobalData.PlayerModel.GetBuyGemRule(BuyGemTypePB.BuyGold, exchangeNum);
        var leftNums = itemRule.Gem.ToString();
        var rightNums = itemRule.Amount.ToString();
     
        var npcId = GlobalData.PlayerModel.PlayerVo.NpcId;
        var npcImg = transform.GetRawImage("window/PersonBg"+npcId);
        npcImg.texture=ResourceManager.Load<Texture>("Background/PersonIcon/Npc"+npcId, null, true);
        npcImg.gameObject.Show();
      

       
        _exchangeNumText.text = I18NManager.Get("Common_BuyExchangeNum", exchangeNum);
        _leftRawImage.texture = ResourceManager.Load<Texture>(LeftImageName(costItemId), null, true);
        _rightRawImage.texture = ResourceManager.Load<Texture>(RightImageName(buyItemId), null, true);
        _leftNum.text = leftNums;
        _rightNum.text = rightNums;      
       
       

        Title = I18NManager.Get("Common_GlodBuy");
      
        Content= I18NManager.Get("Common_GlodButContent", leftNums, rightNums);
       
    }

    //购买体力
    private void BuyPrower(int buyItemId,int costItemId)
    {
        var exchangeNum = GlobalData.PlayerModel.PlayerVo.PowerNum;   
        var itemRule = GlobalData.PlayerModel.GetBuyGemRule(BuyGemTypePB.BuyPower, exchangeNum);
        var leftNums = itemRule.Gem.ToString();
        var rightNums = itemRule.Amount.ToString();
        
        var npcId = GlobalData.PlayerModel.PlayerVo.NpcId;
        var npcImg = transform.GetRawImage("window/PersonBg"+npcId);
        npcImg.texture=ResourceManager.Load<Texture>("Background/PersonIcon/Npc"+npcId, null, true);
        npcImg.gameObject.Show();
       

        
        _exchangeNumText.text = I18NManager.Get("Common_BuyExchangeNum", exchangeNum);
        _leftRawImage.texture = ResourceManager.Load<Texture>(LeftImageName(costItemId), null, true);
        _rightRawImage.texture = ResourceManager.Load<Texture>(RightImageName(buyItemId), null, true);
        _leftNum.text = leftNums;
        _rightNum.text = rightNums;
       
      


        Title = I18NManager.Get("Common_PowerBuy");
        Content = I18NManager.Get("Common_PowerBuyContent", leftNums, rightNums);
       

    }
    

    //购买探班行动力
    private void BuyEncouragePower(int buyItemId,int costItemId)
    {
        var exchangeNum = GlobalData.PlayerModel.PlayerVo.EncourageNum;
        var itemRule = GlobalData.PlayerModel.GetBuyGemRule(BuyGemTypePB.BuyEncouragePower, exchangeNum);
        var leftNums = itemRule.Gem.ToString();
        var rightNums = itemRule.Amount.ToString();
        
        var npcId = GlobalData.PlayerModel.PlayerVo.NpcId;
        var npcImg = transform.GetRawImage("window/PersonBg"+npcId);
        npcImg.texture=ResourceManager.Load<Texture>("Background/PersonIcon/Npc"+npcId, null, true);
        npcImg.gameObject.Show();
  
        _exchangeNumText.text = I18NManager.Get("Common_EncouragePowerBuyExchangeNum", exchangeNum);
        _leftRawImage.texture = ResourceManager.Load<Texture>(LeftImageName(costItemId), null, true);
        _rightRawImage.texture = ResourceManager.Load<Texture>(RightImageName(buyItemId), null, true);
        _leftNum.text = leftNums;
        _rightNum.text = rightNums;
      
        

        Title = I18NManager.Get("Common_EncouragePowerBuy");//"应援行动力购买";
        Content = I18NManager.Get("Common_EncouragePowerBuyContent", leftNums, rightNums);
       

    }





    private string LeftImageName(int costItemId)
    {
        string temp = "";
        switch (costItemId)
        {
            case PropConst.GemIconId:         
                temp = imagePath + PropConst.GemIconId.ToString();
                break;
          
        }
        return temp;
    }

    private string RightImageName(int buyItemId)
    {
        string temp = "";
        switch (buyItemId)
        {
            case PropConst.GoldIconId:           //购买金币
                temp = imagePath + PropConst.GoldIconId.ToString();
                break;
            case PropConst.PowerIconId:          //购买体力
                temp = imagePath + PropConst.PowerIconId.ToString();
                break;
            case PropConst.EncouragePowerId:    //购买星缘体力
                temp = imagePath + PropConst.EncouragePowerId.ToString();  
                break;
            case PropConst.RecolletionIconId:
                temp = imagePath + PropConst.RecolletionIconId.ToString();
                break;
        }
        return temp;
    }
    
   
}
