/**
 *
 *  You can modify and use this source freely
 *  only for the development of application related Live2D.
 *
 *  (c) Live2D Inc. All rights reserved.
 */
using System.Collections;
using live2d;

namespace live2d.framework
{
    
    public class L2DMotionManager : MotionQueueManager
    {
        
        
        private int currentPriority;
        private int reservePriority;

        
        public int getCurrentPriority()
        {
            return currentPriority;
        }


        
        public int getReservePriority()
        {
            return reservePriority;
        }


        
        public bool reserveMotion(int priority)
        {
            if (reservePriority >= priority)
            {
                return false;
            }
            if (currentPriority >= priority)
            {
                return false;
            }
            reservePriority = priority;
            return true;
        }


        
        public void setReservePriority(int val)
        {
            reservePriority = val;
        }


        public override bool updateParam(ALive2DModel model)
        {
            bool updated = base.updateParam(model);
            if (isFinished())
            {
                currentPriority = 0;
            }
            return updated;
        }


        public int startMotionPrio(AMotion motion, int priority)
        {
            if (priority == reservePriority)
            {
                reservePriority = 0;
            }
            currentPriority = priority;
            return base.startMotion(motion, false);
        }
    }
}