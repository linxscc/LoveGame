

using System;

[Serializable]
public class ActivityTemplateUIVo
{

    public string BgPath;   
    public float[] Size;
    public float[] Pivot;
    public string[] ImagePaths;


    public ActivityTemplateUIVo Clone()
    {
        ActivityTemplateUIVo vo =new ActivityTemplateUIVo();

        vo.BgPath = BgPath;      
        vo.Size = Size;
        vo.Pivot = Pivot;
        vo.ImagePaths = ImagePaths;
        
        return vo;
    }

}
