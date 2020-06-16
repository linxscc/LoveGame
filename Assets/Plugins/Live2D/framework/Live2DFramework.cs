/**
 *
 *  You can modify and use this source freely
 *  only for the development of application related Live2D.
 *
 *  (c) Live2D Inc. All rights reserved.
 */
using System.Collections;
using System.Collections.Generic;
using live2d;

namespace live2d.framework
{
    public class Live2DFramework
    {
        private static IPlatformManager _platformManager;

        public static IPlatformManager GetPlatformManager()
        {
            return _platformManager;
        }

        public static void SetPlatformManager(IPlatformManager platformManager)
        {
            Live2DFramework._platformManager = platformManager;
        }


    }
}