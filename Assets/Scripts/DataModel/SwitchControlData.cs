using Com.Proto.Server;
using UnityEngine;

namespace DataModel
{
    public class SwitchControlData
    {
        public bool Recharge;
        public bool Game;
        public bool CustomerServices;
        public bool Code;
        public bool Share;
        public bool Copyright;
        public bool NeedRealName;
        public bool AntiAddiction;
        public bool FacebookLogin;
        
        public bool AppleLogin; //苹果登录
        
        /// <summary>
        /// 防沉迷等级
        /// 0防沉迷关闭，新老不受限制，不需要实名;
        /// 1防沉迷关闭：新老 提示不强制实名;
        /// 2防沉迷打开:新老不实名不让玩;
        /// 3防沉迷打开:老玩家体验1个小时，新不实名不让玩
        /// 4防沉迷打开:老提示不强制实名，新不实名不让玩
        /// </summary>
        public string  AddictionType;   
        
        /// <summary>
        /// 支付限制等级（开关）
        /// 0开启（默认）
        /// 1关闭
        /// 2是新用户开启，老用户关闭
        /// </summary>
        public string CheckAdultPay;     
        
        /// <summary>
        /// 限制游戏时间，防沉迷限制时间，
        ///  -1不限制
        /// >=0需倒计时
        /// </summary>
        //public long AddictionTime; 
        
        public void Init(SwitchControlPB pb)
        {
            Recharge = pb.Recharge == 1;
            Game = pb.Game == 1;
            CustomerServices = pb.CustomerServices == 1;
            Code = pb.Code == 1;
            Share = pb.Share == 1;
            Copyright = pb.Copyright == 1;
          
            if (string.IsNullOrEmpty(pb.Ext))
            {
                NeedRealName = true;
                AntiAddiction = true;
                FacebookLogin = false;
                // AddictionType = "2";
                AddictionType = "0";
                CheckAdultPay = "0";
                AppleLogin = false;
            }
            else
            {
                JSONObject json = new JSONObject(pb.Ext);
                if (json.HasField("AntiAddiction") && json["AntiAddiction"].str == "false")
                {
                    AntiAddiction = false;
                }
                else
                {
                    AntiAddiction = true;
                }
                
                if (json.HasField("NeedRealName") && json["NeedRealName"].str == "false")
                {
                    NeedRealName = false;
                }
                else
                {
                    NeedRealName = true;
                }

                if (json.HasField("FacebookLogin") && json["FacebookLogin"].str == "true")
                {
                    FacebookLogin = true;
                }
                else
                {
                    FacebookLogin = false;
                }

                if (json.HasField("AddictionType"))
                {
                    AddictionType = json["AddictionType"].str;
                }
                else
                {
                    AddictionType = "2";
                }

                if (json.HasField("CheckAdultPay"))
                {
                    CheckAdultPay = json["CheckAdultPay"].str;
                }
                else
                {
                    CheckAdultPay = "0";
                }

                if (json.HasField("AppleLogin")&&json["AppleLogin"].str=="true")
                {
                    AppleLogin = true;
                }
                else
                {
                    AppleLogin = false;
                }
            }
        }

        /// <summary>
        /// 设置用户可玩时间
        /// </summary>
//        public void SetAddictionTime()
//        {
//           
//            AddictionTime = GalaAccountManager.Instance.FetchUserCanPlayTime();
//            Debug.LogError("********"+AddictionTime);
//        }
    }
}