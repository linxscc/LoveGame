

using Com.Proto;
using DataModel;

public class MailAwardVO
{
 
    public RewardVo Reward;
    
    public MailAwardVO(AwardPB pB)
    {    
                           
        Reward = new RewardVo(pB);
    }
   
}
