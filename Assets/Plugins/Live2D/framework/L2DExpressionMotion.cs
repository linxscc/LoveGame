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
    
    public class L2DExpressionMotion : AMotion
    {
        private const string EXPRESSION_DEFAULT = "DEFAULT";

        public const int TYPE_SET = 0;
        public const int TYPE_ADD = 1;
        public const int TYPE_MULT = 2;

        private List<L2DExpressionParam> paramList;

        
        public L2DExpressionMotion()
        {
            paramList = new List<L2DExpressionParam>();
        }


        
        public override void updateParamExe(ALive2DModel model, long timeMSec, float weight, MotionQueueEnt motionQueueEnt)
        {
            for (int i = paramList.Count - 1; i >= 0; --i)
            {
                L2DExpressionParam param = paramList[i];
                if (param.type == TYPE_ADD)
                {
                    model.addToParamFloat(param.id, param.value, weight);
                }
                else if (param.type == TYPE_MULT)
                {
                    model.multParamFloat(param.id, param.value, weight);
                }
                else if (param.type == TYPE_SET)
                {
                    model.setParamFloat(param.id, param.value, weight);
                }
            }
        }

        public static L2DExpressionMotion loadJson(byte[] buf)
        {
            return loadJson(System.Text.Encoding.GetEncoding("UTF-8").GetString(buf));
        }


        public static L2DExpressionMotion loadJson(string buf)
        {
            return loadJson(buf.ToCharArray());
        }

        
        public static L2DExpressionMotion loadJson(char[] buf)
        {
            L2DExpressionMotion ret = new L2DExpressionMotion();

            Value json = Json.parseFromBytes(buf);

            ret.setFadeIn(json.get("fade_in").toInt(1000));
            ret.setFadeOut(json.get("fade_out").toInt(1000));

            if (!json.getMap(null).ContainsKey("params")) return ret;

            
            Value parameters = json.get("params");
            int paramNum = parameters.getVector(null).Count;

            ret.paramList = new List<L2DExpressionParam>(paramNum);

            for (int i = 0; i < paramNum; i++)
            {
                Value param = parameters.get(i);
                string paramID = param.get("id").toString();
                float value = param.get("val").toFloat();

                
                int calcTypeInt = TYPE_ADD;
                string calc = param.getMap(null).ContainsKey("calc") ? (param.get("calc").toString()) : "add";
                if (calc.Equals("add"))
                {
                    calcTypeInt = TYPE_ADD;
                }
                else if (calc.Equals("mult"))
                {
                    calcTypeInt = TYPE_MULT;
                }
                else if (calc.Equals("set"))
                {
                    calcTypeInt = TYPE_SET;
                }
                else
                {
                    
                    calcTypeInt = TYPE_ADD;
                }

                
                if (calcTypeInt == TYPE_ADD)
                {
                    float defaultValue = (!param.getMap(null).ContainsKey("def")) ? 0 : param.get("def").toFloat();
                    value = value - defaultValue;
                }
                
                else if (calcTypeInt == TYPE_MULT)
                {
                    float defaultValue = (!param.getMap(null).ContainsKey("def")) ? 1 : param.get("def").toFloat(0);
                    if (defaultValue == 0) defaultValue = 1;
                    value = value / defaultValue;
                }

                
                L2DExpressionParam item = new L2DExpressionParam();

                item.id = paramID;
                item.type = calcTypeInt;
                item.value = value;

                ret.paramList.Add(item);
            }
            return ret;
        }


        
        static public Dictionary<string, AMotion> loadExpressionJsonV09(byte[] bytes)
        {
            Dictionary<string, AMotion> expressions = new Dictionary<string, AMotion>();

            char[] buf = System.Text.Encoding.GetEncoding("UTF-8").GetString(bytes).ToCharArray();
            Value mo = Json.parseFromBytes(buf);

            Value defaultExpr = mo.get(EXPRESSION_DEFAULT);

            List<string> keys = mo.keySet();
            foreach (string key in keys)
            {
                if (EXPRESSION_DEFAULT.Equals(key)) continue;

                Value expr = mo.get(key);

                L2DExpressionMotion exMotion = loadJsonV09(defaultExpr, expr);
                expressions.Add(key, exMotion);
            }

            return expressions;
        }


        
        static private L2DExpressionMotion loadJsonV09(Value defaultExpr, Value expr)
        {

            L2DExpressionMotion ret = new L2DExpressionMotion();
            ret.setFadeIn(expr.get("FADE_IN").toInt(1000));
            ret.setFadeOut(expr.get("FADE_OUT").toInt(1000));

            
            Value defaultParams = defaultExpr.get("PARAMS");
            Value parameters = expr.get("PARAMS");
            List<string> paramID = parameters.keySet();
            List<string> idList = new List<string>();

            foreach (string id in paramID)
            {
                idList.Add(id);
            }

            
            for (int i = idList.Count - 1; i >= 0; --i)
            {
                string id = idList[i];

                float defaultV = defaultParams.get(id).toFloat(0);
                float v = parameters.get(id).toFloat(0.0f);
                float values = (v - defaultV);
                //			ret.addParam(id, value,L2DExpressionMotion.TYPE_ADD);
                L2DExpressionParam param = new L2DExpressionParam();
                param.id = id;
                param.type = L2DExpressionMotion.TYPE_ADD;
                param.value = values;
                ret.paramList.Add(param);
            }

            return ret;
        }


        
        public class L2DExpressionParam
        {
            public string id;
            //public int index=-1;
            public int type;
            public float value;
        }
    }
}