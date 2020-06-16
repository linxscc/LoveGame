using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianBlurHelp {


    public static void GetGaussianBlur(Texture source,
        RenderTexture dest,
        Material material,
        float blurSize,
        int iterations = 3,
        int downSample = 1,
        int width = 1440,
        int height = 1920
        )
    {
        int rtW = width / downSample;
        int rtH = height / downSample;

        RenderTexture buffer0 = RenderTexture.GetTemporary(rtW, rtH, 0);
        buffer0.filterMode = FilterMode.Bilinear;

        Graphics.Blit(source, buffer0);
        for (int i = 0; i < iterations; i++)
        {
            material.SetFloat("_BlurSize", 1.0f + i * blurSize);
            RenderTexture buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);
            Graphics.Blit(buffer0, buffer1, material, 0);
            RenderTexture.ReleaseTemporary(buffer0);
            buffer0 = buffer1;
            buffer1 = RenderTexture.GetTemporary(rtW, rtH, 0);

            // Render the horizontal pass
            Graphics.Blit(buffer0, buffer1, material, 1);

            RenderTexture.ReleaseTemporary(buffer0);
            buffer0 = buffer1;
        }
        Graphics.Blit(buffer0, dest);
        RenderTexture.ReleaseTemporary(buffer0);
       // mat.SetFloat("_BlurSize", blurSize);
      //  Graphics.Blit(source, buffer, mat, 0);
       // Graphics.Blit(buffer, dest, mat, 1);
       // RenderTexture.ReleaseTemporary(buffer);
        return;
    }



}
