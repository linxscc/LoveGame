using game.main;
using UnityEngine;
using XLua;

namespace Assets.Scripts
{
    public class XLuaManager
    {
        private static LuaEnv _luaEnv;

        private static bool _isInit = false;

        public static void CallLua(string code, string chunkName = "chunk", LuaTable env = null)
        {
            _luaEnv.DoString(code, chunkName, env);
        }

        public static LuaFunction GetFunction(string name)
        {
            return _luaEnv.Global.Get<LuaFunction>(name);
        }
        
        public static void InitXLua()
        {
            if (_isInit)
                return;

            _luaEnv = new LuaEnv();
            _luaEnv.AddLoader(NormalLoader);
            _luaEnv.AddLoader(ResourcesLoader);
            
//            _luaEnv.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
//            _luaEnv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
//            _luaEnv.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);
//            _luaEnv.AddBuildin("ffi", XLua.LuaDLL.Lua.LoadFFI);
        }

        private static byte[] ResourcesLoader(ref string filepath)
        {
            TextAsset ta = Resources.Load<TextAsset>(filepath + ".lua");
            if (ta != null)
            {
                Debug.Log("<color='#987456'>XLua ResourcesLoader->" + filepath + "</color>");
                return ta.bytes;
            }

            return null;
        }

        public static void LoadMainSctipt()
        {
            Debug.LogWarning("============LoadMainSctipt============");

            if (_isInit)
            {
                RestartLuaEnv();
            }

            _luaEnv.DoString("require 'main'");

            _isInit = true;
        }

        private static void RestartLuaEnv()
        {
            CallLua("LuaHotfixDispose()");
            _luaEnv.Dispose();

            _luaEnv = new LuaEnv();
            _luaEnv.AddLoader(NormalLoader);
            _luaEnv.AddLoader(ResourcesLoader);
        }

        private static byte[] NormalLoader(ref string fileName)
        {
            string path = AssetLoader.GetLuaPath(fileName);

            byte[] bytes = new AssetLoader().LoadBytes(path, false);
            if (bytes != null)
            {
                Debug.Log("<color='#987456'>XLua NormalLoader->" + fileName + "</color>   " + path);
            }

            return bytes;
        }
    }
}