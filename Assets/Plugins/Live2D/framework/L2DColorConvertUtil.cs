using UnityEngine;
using System.Collections;

namespace live2d.framework
{
    public class L2DColorConvertUtil
    {
        /**
         *
         * src color * hsl matrix = target color
         * 
         * e.g)
         *  in:( h:0 s:0 l:0 ) 
         *  out:
         *  1 0 0 0
         *  0 1 0 0
         *  0 0 1 0
         *  0 0 0 1
         *  
         *  in:( h120 s:0.8 l:0.5 ) 
         *  out:
         *  0.3316	-0.2428	0.9061	-0.0050
         *  0.9016	0.3300	-0.2415	-0.0050
         *  -0.2415	0.9016	0.3300	-0.0050
         *  0.0000	0.0000	0.0000	1.0000
         * 
         */
        public static L2DMatrix44 CreateHslMatrix(float hue, float sat, float light)
        {
            Vector3 V_DIAG = new Vector3(1, 1, 1);//対角
            Vector3 V_DIAG_NORM = new Vector3(1, 1, 1);
            V_DIAG_NORM.Normalize();

            L2DMatrix44 cm = new L2DMatrix44();

            cm.multRotate(hue, V_DIAG_NORM);//hue変換
            // sat
            {
                Vector3 vR = new Vector3(1, 0, 0);
                Vector3 vRotate = Vector3.Cross(V_DIAG, vR);
                vRotate.Normalize();

                float rad = -Vector3.Angle(V_DIAG, vR) / 180 * Mathf.PI;
                float satScale;

                if (sat > 0)
                {
                    satScale = 1 - 0.01f * sat;
                }
                else
                {
                    satScale = 1 + 0.01f * sat;
                }
                cm.multRotate(rad * 180 / Mathf.PI, vRotate);
                cm.multScale(1, satScale, satScale);
                cm.multRotate(-rad * 180 / Mathf.PI, vRotate);
            }

            // light
            float L = light * 0.01f;//-1..1
            if (L > 0)
            {
                float s = 1 - L;
                cm.multTranslate(1, 1, 1);
                cm.multScale(s, s, s);
                cm.multTranslate(-1, -1, -1);
            }
            else
            {
                float s = 1 + L;//Lはマイナス
                cm.multScale(s, s, s);
            }


            return cm;
        }
    }
}