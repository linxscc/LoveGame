/**
 *
 *  You can modify and use this source freely
 *  only for the development of application related Live2D.
 *
 *  (c) Live2D Inc. All rights reserved.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using live2d;

namespace live2d.framework
{
    public interface IPlatformManager
    {
        byte[] loadBytes(String path);
        void UnloadBytes(String path);
        String loadString(String path);
        void UnloadString(String path);
        ALive2DModel loadLive2DModel(String path);
        void loadTexture(ALive2DModel model, int no, String path);
        void UnloadTexture(String path);
        void log(String txt);
    }
}