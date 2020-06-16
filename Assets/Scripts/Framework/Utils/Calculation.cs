using UnityEngine;

public class Calculation : ScriptableObject
{
    /// <summary>
    /// 计算清cd消耗的宝石
    /// </summary>
    /// <param name="cd"></param>
    /// <returns></returns>
    public static int CalClearGem(long cd)
    {
        if (cd <= 0)
        {
            return 0;
        }
        int second = (int)(cd / 1000);
        int gem = 1;
        int lastGem = 0;
        int keySecond = 1;
        int lastKeySecond = 0;
        if (cd > 0L && cd <= 60000L)
        {
            //1min
            gem = 2;
            lastGem = 0;
            keySecond = 60;
            lastKeySecond = 1;
        }
        else if (cd > 60000L && cd <= 3600000L)
        {
            //1 hour
            gem = 50;
            lastGem = 2;
            keySecond = 3600;
            lastKeySecond = 60;
        }
        else if (cd > 3600000L && cd <= 86400000L)
        {
            //1 day
            gem = 800;
            lastGem = 50;
            keySecond = 86400;
            lastKeySecond = 3600;
        }
        else if (cd > 86400000L)
        {
            //1 week
            gem = 3000;
            lastGem = 800;
            keySecond = 604800;
            lastKeySecond = 86400;
        }
        float v = (gem - lastGem) * 1.0f * (second - keySecond) / (keySecond - lastKeySecond) + gem;
        return Mathf.CeilToInt(v);
    }

}