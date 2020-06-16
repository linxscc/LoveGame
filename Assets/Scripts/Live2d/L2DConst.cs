
using System;
using UnityEngine;
/**
*
*  You can modify and use this source freely
*  only for the development of application related Live2D.
*
*  (c) Live2D Inc. All rights reserved.
*/
public class L2DConst
{
	public const string Live2dCameraPath = "Module/Live2d/Live2dCamera";
	public const string Live2dGraphicPath = "Live2d/Live2dGraphic";
	
    public const string MOTION_MOTIONLESS = "@motionless";
    public const string MOTION_EYEBLINK = "@eyeblink";
    
	public static bool DEBUG_LOG = false;
    public static bool DEBUG_TOUCH_LOG = false;
	public static bool DEBUG_DRAW_HIT_AREA = false;
	
	
	public const float VIEW_MAX_SCALE = 2f;
	public const float VIEW_MIN_SCALE = 0.8f;
	
	public const float VIEW_LOGICAL_LEFT = -1;
	public const float VIEW_LOGICAL_RIGHT = 1;
	
	public const float VIEW_LOGICAL_MAX_LEFT = -2;
	public const float VIEW_LOGICAL_MAX_RIGHT = 2;
	public const float VIEW_LOGICAL_MAX_BOTTOM = -2;
	public const float VIEW_LOGICAL_MAX_TOP = 2;
	
	public const float SCREEN_WIDTH = 20.0f;
	public const float SCREEN_HEIGHT = 20.0f;
	
	
	public const string MOTION_GROUP_IDLE			="idle";		
	public const string MOTION_GROUP_TAP_BODY		="tap_body";	
	public const string MOTION_GROUP_FLICK_HEAD		="flick_head";	
	public const string MOTION_GROUP_PINCH_IN		="pinch_in";	
	public const string MOTION_GROUP_PINCH_OUT		="pinch_out";	
	public const string MOTION_GROUP_SHAKE			="shake";		
	
	
	public const string HIT_AREA_HEAD				="head";
	public const string HIT_AREA_BODY				="body";

	
	public const int PRIORITY_NONE			= 0;
	public const int PRIORITY_IDLE			= 1;
	public const int PRIORITY_NORMAL		= 2;
	public const int PRIORITY_FORCE			= 3;
}

public class ExpressionInfo
{
    public int Id;
    public string Name;
    public string Remark;
    public int ActionId;
    public EXPRESSIONTRIGERTYPE TriggerType;
    public int Weight;
    public string Dialog;
    public int NpcId;
    public string StartTime;
    public string EndTime;
    public string LabelId;//Label元素Id
    public string DialogId;//音频元素Id
    public string GiftId;//送礼道具ID
    public string DialogName;//音频ID
    public string UnlockDescription;//解锁描述
    public ExpressionInfo(string[] arr)
    {
        //解析数据
        Id = int.Parse(arr[0]);
        Name = arr[1];
        Remark =  arr[2];
        ActionId = int.Parse(arr[3]);
        TriggerType = (EXPRESSIONTRIGERTYPE) int.Parse(arr[4]);
        Weight = int.Parse(arr[5]);
        Dialog = arr[6];
        NpcId = int.Parse(arr[7]);
        StartTime = arr[8];
        EndTime = arr[9];
        LabelId = arr[10];
        DialogId = arr[11];
        GiftId = arr[12];
        DialogName = arr[13];
        UnlockDescription = arr[14];
    }

    public bool IsTimeRange(DateTime dt)
    {
        if (StartTime == "")
            return true;
     
        DateTime dt1 = Convert.ToDateTime(StartTime);
        DateTime dt2 = Convert.ToDateTime(EndTime);

        if (DateTime.Compare(dt, dt1)<0)
        {
            return false;
        }
        if (DateTime.Compare(dt2,dt )<0)
        {
            return false;
        }
        return true;
    }

}
public enum EXPRESSIONTRIGERTYPE
{
    NO,//无
    NORMAL,//常态
    BODY,//身体
    HEAD,//头
    LOGIN,//登陆播放表情
    LOVEDIARY,//恋爱日记标签触发
    GIFT,//送礼道具触发
    DRAWCARD,//抽卡语音
}
public enum LIVE2DVIEWTYPE
{
    STORY,
    MAINPANLE
}
