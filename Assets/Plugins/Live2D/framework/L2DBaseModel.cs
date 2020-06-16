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
    public class L2DBaseModel
    {
        
        protected ALive2DModel live2DModel = null;      
        protected L2DModelMatrix modelMatrix = null;        

        
        protected Dictionary<string, AMotion> expressions;  
        protected Dictionary<string, AMotion> motions;      

        protected L2DMotionManager mainMotionManager;       
        protected L2DMotionManager expressionManager;       
        protected L2DEyeBlink eyeBlink;             
        protected L2DPhysics physics;               
        protected L2DPose pose;                 

        protected bool debugMode = false;
        protected bool initialized = false; 
        protected bool updating = false;        
        protected bool lipSync = false;     
        protected float lipSyncValue;           

        
        protected float accelX = 0;
        protected float accelY = 0;
        protected float accelZ = 0;

        
        protected float dragX = 0;
        protected float dragY = 0;

        protected long startTimeMSec;


        public L2DBaseModel()
        {
            
            mainMotionManager = new L2DMotionManager();
            expressionManager = new L2DMotionManager();

            motions = new Dictionary<string, AMotion>();
            expressions = new Dictionary<string, AMotion>();
        }


        public L2DModelMatrix getModelMatrix()
        {
            return modelMatrix;
        }


        
        public bool isInitialized()
        {
            return initialized;
        }


        public void setInitialized(bool v)
        {
            initialized = v;
        }


        
        public bool isUpdating()
        {
            return updating;
        }


        public void setUpdating(bool v)
        {
            updating = v;
        }


        
        public ALive2DModel getLive2DModel()
        {
            return live2DModel;
        }


        public void setLipSync(bool v)
        {
            lipSync = v;
        }
        public float getLipSyncValue()
        {
            return lipSyncValue;
        }


        public void setLipSyncValue(float v)
        {
            lipSyncValue = v;
        }


        public void setAccel(float x, float y, float z)
        {
            accelX = x;
            accelY = y;
            accelZ = z;
        }


        public void setDrag(float x, float y)
        {
            dragX = x;
            dragY = y;
        }


        public MotionQueueManager getMainMotionManager()
        {
            return mainMotionManager;
        }


        public MotionQueueManager getExpressionManager()
        {
            return expressionManager;
        }

        public void loadModelData(String path)
        {
            IPlatformManager pm = Live2DFramework.GetPlatformManager();

            if (debugMode) pm.log("Load model : " + path);

            live2DModel = pm.loadLive2DModel(path);
            live2DModel.saveParam();

            if (Live2D.getError() != Live2D.L2D_NO_ERROR)
            {
                pm.log("Error : Failed to loadModelData().");
                return;
            }

            var w = live2DModel.getCanvasWidth();
            var h = live2DModel.getCanvasHeight();
            modelMatrix = new L2DModelMatrix(w, h);

            if (w>h)
            {
                modelMatrix.setWidth(2);                
            }
            else
            {
                modelMatrix.setHeight(2);
            }

            modelMatrix.setCenterPosition(0, 0);
        }


        public void loadTexture(int no, String path)
        {
            IPlatformManager pm = Live2DFramework.GetPlatformManager();
            if (debugMode) pm.log("Load Texture : " + path);

            pm.loadTexture(live2DModel, no, path);
        }

        public AMotion loadMotion(String name, String path)
        {
            IPlatformManager pm = Live2DFramework.GetPlatformManager();
            if (debugMode) pm.log("Load Motion : " + path);

            Live2DMotion motion = null;

            byte[] buf = pm.loadBytes(path);
            motion = Live2DMotion.loadMotion(buf);

            if (name != null)
            {
                motions.Add(name, motion);
            }

            return motion;
        }

        public void loadExpression(String name, String path)
        {
            IPlatformManager pm = Live2DFramework.GetPlatformManager();
            if (debugMode) pm.log("Load Expression : " + path);

            expressions.Add(name, L2DExpressionMotion.loadJson(pm.loadBytes(path)));
        }


        public void loadPose(String path)
        {
            IPlatformManager pm = Live2DFramework.GetPlatformManager();
            if (debugMode) pm.log("Load Pose : " + path);
            pose = L2DPose.load(pm.loadBytes(path));
        }

        public void loadPhysics(String path)
        {
            IPlatformManager pm = Live2DFramework.GetPlatformManager();
            if (debugMode) pm.log("Load Physics : " + path);
            physics = L2DPhysics.load(pm.loadBytes(path));
        }


        public bool getSimpleRect(String drawID,out float left,out float right,out float top,out float bottom)
        {
            int drawIndex = live2DModel.getDrawDataIndex(drawID);
            if (drawIndex < 0)
            {
                left = 0;
                right = 0;
                top = 0;
                bottom = 0;
                return false;
            }
            float[] points = live2DModel.getTransformedPoints(drawIndex);

            float l = live2DModel.getCanvasWidth();
            float r = 0;
            float t = live2DModel.getCanvasHeight();
            float b = 0;

            for (int j = 0; j < points.Length; j = j + 2)
            {
                float x = points[j];
                float y = points[j + 1];
                if (x < l) l = x;   
                if (x > r) r = x;   
                if (y < t) t = y;       
                if (y > b) b = y;
            }
            
            left=l;
            right=r;
            top=t;
            bottom=b;

            return true;
        }


        public bool hitTestSimple(String drawID, float testX, float testY)
        {
            float left =0;
            float right =0;
            float top =0;
            float bottom =0;

            if ( ! getSimpleRect(drawID, out left,out right,out top,out bottom))
            {
                return false;
            }

            float tx = modelMatrix.invertTransformX(testX);
            float ty = modelMatrix.invertTransformY(testY);

            return (left <= tx && tx <= right && top <= ty && ty <= bottom);
        }
    }
}