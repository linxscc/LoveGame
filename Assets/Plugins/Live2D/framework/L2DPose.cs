/**
 *
 *  You can modify and use this source freely
 *  only for the development of application related Live2D.
 *
 *  (c) Live2D Inc. All rights reserved.
 */

using System.Collections;
using System.Collections.Generic;
using live2d ;

namespace live2d.framework
{
    
    public class L2DPose
    {
        protected List<L2DPartsParam[]> partsGroupList;
        protected int fadeInMS = 500; 
        private long lastTime = 0;
        private ALive2DModel lastModel = null;


        public L2DPose()
        {
            partsGroupList = new List<L2DPartsParam[]>();
        }


        public void addPartsGroup(L2DPartsParam[] partsGroup)
        {
            partsGroupList.Add(partsGroup);
        }


        public void addPartsGroup(string[] idGroup)
        {
            L2DPartsParam[] partsGroup = new L2DPartsParam[idGroup.Length];

            for (int i = 0; i < idGroup.Length; i++)
            {
                partsGroup[i] = new L2DPartsParam(idGroup[i]);
            }

            partsGroupList.Add(partsGroup);
        }


        
        public void updateParam(ALive2DModel model)
        {
            if (model == null) return;

            
            if (model != lastModel)
            {
                
                initParam(model);
            }
            lastModel = model;

            long curTime = UtSystem.getUserTimeMSec();
            float deltaTimeSec = ((lastTime == 0) ? 0 : (curTime - lastTime) / 1000.0f);
            lastTime = curTime;

            
            if (deltaTimeSec < 0) deltaTimeSec = 0;

            for (int i = 0; i < partsGroupList.Count; i++)
            {
                normalizePartsOpacityGroup(model, partsGroupList[i], deltaTimeSec);
                copyOpacityOtherParts(model, partsGroupList[i]);
            }
        }


        
        public void initParam(ALive2DModel model)
        {
            if (model == null) return;

            for (int i = 0; i < partsGroupList.Count; i++)
            {

                L2DPartsParam[] partsGroup = partsGroupList[i];
                for (int j = 0; j < partsGroup.Length; j++)
                {
                    partsGroup[j].initIndex(model);

                    int partsIndex = partsGroup[j].partsIndex;
                    int paramIndex = partsGroup[j].paramIndex;
                    if (partsIndex < 0) continue;

                    bool v = (model.getParamFloat(paramIndex) != 0);
                    model.setPartsOpacity(partsIndex, (v ? 1.0f : 0.0f));
                    model.setParamFloat(paramIndex, (v ? 1.0f : 0.0f));
                }
            }
        }


        
        public void normalizePartsOpacityGroup(ALive2DModel model, L2DPartsParam[] partsGroup, float deltaTimeSec)
        {
            int visibleParts = -1;
            float visibleOpacity = 1.0f;

            float phi = 0.5f;
            float maxBackOpacity = 0.15f;


            
            for (int i = 0; i < partsGroup.Length; i++)
            {
                int partsIndex = partsGroup[i].partsIndex;
                int paramIndex = partsGroup[i].paramIndex;

                if (partsIndex < 0) continue;

                if (model.getParamFloat(paramIndex) != 0)
                {
                    if (visibleParts >= 0)
                    {
                        break;
                    }
                    visibleParts = i;
                    visibleOpacity = model.getPartsOpacity(partsIndex);

                    
                    visibleOpacity += deltaTimeSec / ( fadeInMS/1000.0f );
                    if (visibleOpacity > 1)
                    {
                        visibleOpacity = 1;
                    }
                }
            }

            if (visibleParts < 0)
            {
                visibleParts = 0;
                visibleOpacity = 1;
            }

            
            for (int i = 0; i < partsGroup.Length; i++)
            {
                int partsIndex = partsGroup[i].partsIndex;
                if (partsIndex < 0) continue;

                
                if (visibleParts == i)
                {
                    model.setPartsOpacity(partsIndex, visibleOpacity);
                }
                
                else
                {
                    float opacity = model.getPartsOpacity(partsIndex);
                    float a1;
                    if (visibleOpacity < phi)
                    {
                        a1 = visibleOpacity * (phi - 1) / phi + 1; 
                    }
                    else
                    {
                        a1 = (1 - visibleOpacity) * phi / (1 - phi); 
                    }

                    
                    float backOp = (1 - a1) * (1 - visibleOpacity);
                    if (backOp > maxBackOpacity)
                    {
                        a1 = 1 - maxBackOpacity / (1 - visibleOpacity);
                    }

                    if (opacity > a1)
                    {
                        opacity = a1;
                    }
                    model.setPartsOpacity(partsIndex, opacity);
                }
            }
        }


        
        public void copyOpacityOtherParts(ALive2DModel model, L2DPartsParam[] partsGroup)
        {
            for (int i_group = 0; i_group < partsGroup.Length; i_group++)
            {
                L2DPartsParam partsParam = partsGroup[i_group];

                if (partsParam.link == null) continue;
                if (partsParam.partsIndex < 0) continue;

                float opacity = model.getPartsOpacity(partsParam.partsIndex);

                for (int i_link = 0; i_link < partsParam.link.Count; i_link++)
                {
                    L2DPartsParam linkParts = partsParam.link[i_link];

                    if (linkParts.partsIndex < 0)
                    {
                        //
                        linkParts.initIndex(model);
                    }

                    if (linkParts.partsIndex < 0) continue;//
                    model.setPartsOpacity(linkParts.partsIndex, opacity);
                }
            }
        }

        public static L2DPose load(byte[] buf)
        {
            return load(System.Text.Encoding.GetEncoding("UTF-8").GetString(buf));
        }


        public static L2DPose load(string buf)
        {
            return load(buf.ToCharArray());
        }


        
        public static L2DPose load(char[] buf)
        {
            L2DPose ret = new L2DPose();

            Value json = Json.parseFromBytes(buf);

            
            if (json.getMap(null).ContainsKey("fade_in")) 
            {
               ret.fadeInMS = json.get("fade_in").toInt();              
            }

            
            List<Value> poseListInfo = json.get("parts_visible").getVector(null);
            int poseNum = poseListInfo.Count;

            for (int i_pose = 0; i_pose < poseNum; i_pose++)
            {
                Value poseInfo = poseListInfo[i_pose];

                
                List<Value> idListInfo = poseInfo.get("group").getVector(null);
                int idNum = idListInfo.Count;
                L2DPartsParam[] partsGroup = new L2DPartsParam[idNum];
                for (int i_group = 0; i_group < idNum; i_group++)
                {
                    Value partsInfo = idListInfo[i_group];
                    L2DPartsParam parts = new L2DPartsParam(partsInfo.get("id").toString());
                    partsGroup[i_group] = parts;

                    
                    if (!partsInfo.getMap(null).ContainsKey("link")) continue;
                    List<Value> linkListInfo = partsInfo.get("link").getVector(null);
                    int linkNum = linkListInfo.Count;
                    parts.link = new List<L2DPartsParam>();
                    for (int i_link = 0; i_link < linkNum; i_link++)
                    {
                        //					string linkID = idListInfo.get(i_group).tostring();//parts ID
                        L2DPartsParam linkParts = new L2DPartsParam(linkListInfo[i_link].toString());
                        parts.link.Add(linkParts);
                    }
                }
                ret.addPartsGroup(partsGroup);
            }
            return ret;
        }
    }


    
    public class L2DPartsParam
    {
        public const int TYPE_VISIBLE = 0;
        public const bool optimize = false;
        public string id;
        public int paramIndex = -1;
        public int partsIndex = -1;
        public int type = TYPE_VISIBLE;

        public List<L2DPartsParam> link = null;


        public L2DPartsParam(string id)
        {
            this.id = id;
        }


        
        public void initIndex(ALive2DModel model)
        {
            if (type == TYPE_VISIBLE)
            {
                paramIndex = model.getParamIndex("VISIBLE:" + id);
            }
            partsIndex = model.getPartsDataIndex(PartsDataID.getID(id));
            model.setParamFloat(paramIndex, 1);
            //Log.d("live2d",id+ " param:"+paramIndex+" parts:"+partsIndex);
        }
    }
}