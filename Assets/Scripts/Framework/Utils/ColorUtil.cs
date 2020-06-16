using UnityEngine;

namespace Assets.Scripts.Module.Framework.Utils
{
    public class ColorUtil
    {
        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") +
                         color.a.ToString("X2");
            return hex;
        }

        public static Color DeepGray = new Color(0.15f, 0.15f, 0.15f);

        public static Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte a;
            if (hex.Length > 6)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            else
            {
                a = byte.Parse("ff", System.Globalization.NumberStyles.HexNumber);
            }

            return new Color32(r, g, b, a);
        }


        /// <summary>
        /// 获取颜色的灰度，用以判断颜色深浅
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static float GetColorGrayLevel(Color color)
        {
            float y = color.r * 0.299f + color.g * 0.587f + color.b * 0.114f;
            return y;
        }


        public static Color ChangeColor(Color color, float correctionFactor)
        {
            float red = (float) color.r;
            float green = (float) color.g;
            float blue = (float) color.b;

            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }

            if (red < 0) red = 0;

            if (red > 255) red = 255;

            if (green < 0) green = 0;

            if (green > 255) green = 255;

            if (blue < 0) blue = 0;

            if (blue > 255) blue = 255;

            return new Color(red, green, blue, color.a);
        }

        public static Color ArgbToColor(uint color)
        {
            float a = (float) ((color >> 24) & 0xff) / 255;
            float r = (float) ((color >> 16) & 0xff) / 255;
            float g = (float) ((color >> 8) & 0xff) / 255;
            float b = (float) ((color >> 0) & 0xff) / 255;

            return new Color(r, g, b, a);
        }

        public static Color RgbaToColor(uint color)
        {
            float r = (float) ((color >> 24) & 0xff) / 255;
            float g = (float) ((color >> 16) & 0xff) / 255;
            float b = (float) ((color >> 8) & 0xff) / 255;
            float a = (float) ((color >> 0) & 0xff) / 255;

            return new Color(r, g, b, a);
        }

        public static Color RgbToColor(uint color)
        {
            float r = (float) ((color >> 16) & 0xff) / 255;
            float g = (float) ((color >> 8) & 0xff) / 255;
            float b = (float) ((color >> 0) & 0xff) / 255;

            return new Color(r, g, b);
        }
    }
}