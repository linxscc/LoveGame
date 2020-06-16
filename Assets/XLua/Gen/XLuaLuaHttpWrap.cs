#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class XLuaLuaHttpWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(XLua.LuaHttp);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 3, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendRequest", _m_SendRequest);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "HttpTimeOut", _g_get_HttpTimeOut);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "CustomerData", _g_get_CustomerData);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "AutoRetry", _g_get_AutoRetry);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "HttpTimeOut", _s_set_HttpTimeOut);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "CustomerData", _s_set_CustomerData);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "AutoRetry", _s_set_AutoRetry);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 2, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "OnGlobalError", _m_OnGlobalError_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 9 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && translator.Assignable<DelegateBytes>(L, 4) && translator.Assignable<object>(L, 5) && LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6) && (LuaAPI.lua_isnil(L, 7) || LuaAPI.lua_type(L, 7) == LuaTypes.LUA_TSTRING) && (LuaAPI.lua_isnil(L, 8) || LuaAPI.lua_type(L, 8) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 9))
				{
					string _cmd = LuaAPI.lua_tostring(L, 2);
					byte[] _data = LuaAPI.lua_tobytes(L, 3);
					DelegateBytes _successCallback = translator.GetDelegate<DelegateBytes>(L, 4);
					object _customerData = translator.GetObject(L, 5, typeof(object));
					bool _cache = LuaAPI.lua_toboolean(L, 6);
					string _version = LuaAPI.lua_tostring(L, 7);
					string _serverUrl = LuaAPI.lua_tostring(L, 8);
					int _httpTimeout = LuaAPI.xlua_tointeger(L, 9);
					
					XLua.LuaHttp gen_ret = new XLua.LuaHttp(_cmd, _data, _successCallback, _customerData, _cache, _version, _serverUrl, _httpTimeout);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 8 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && translator.Assignable<DelegateBytes>(L, 4) && translator.Assignable<object>(L, 5) && LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6) && (LuaAPI.lua_isnil(L, 7) || LuaAPI.lua_type(L, 7) == LuaTypes.LUA_TSTRING) && (LuaAPI.lua_isnil(L, 8) || LuaAPI.lua_type(L, 8) == LuaTypes.LUA_TSTRING))
				{
					string _cmd = LuaAPI.lua_tostring(L, 2);
					byte[] _data = LuaAPI.lua_tobytes(L, 3);
					DelegateBytes _successCallback = translator.GetDelegate<DelegateBytes>(L, 4);
					object _customerData = translator.GetObject(L, 5, typeof(object));
					bool _cache = LuaAPI.lua_toboolean(L, 6);
					string _version = LuaAPI.lua_tostring(L, 7);
					string _serverUrl = LuaAPI.lua_tostring(L, 8);
					
					XLua.LuaHttp gen_ret = new XLua.LuaHttp(_cmd, _data, _successCallback, _customerData, _cache, _version, _serverUrl);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 7 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && translator.Assignable<DelegateBytes>(L, 4) && translator.Assignable<object>(L, 5) && LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6) && (LuaAPI.lua_isnil(L, 7) || LuaAPI.lua_type(L, 7) == LuaTypes.LUA_TSTRING))
				{
					string _cmd = LuaAPI.lua_tostring(L, 2);
					byte[] _data = LuaAPI.lua_tobytes(L, 3);
					DelegateBytes _successCallback = translator.GetDelegate<DelegateBytes>(L, 4);
					object _customerData = translator.GetObject(L, 5, typeof(object));
					bool _cache = LuaAPI.lua_toboolean(L, 6);
					string _version = LuaAPI.lua_tostring(L, 7);
					
					XLua.LuaHttp gen_ret = new XLua.LuaHttp(_cmd, _data, _successCallback, _customerData, _cache, _version);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 6 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && translator.Assignable<DelegateBytes>(L, 4) && translator.Assignable<object>(L, 5) && LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6))
				{
					string _cmd = LuaAPI.lua_tostring(L, 2);
					byte[] _data = LuaAPI.lua_tobytes(L, 3);
					DelegateBytes _successCallback = translator.GetDelegate<DelegateBytes>(L, 4);
					object _customerData = translator.GetObject(L, 5, typeof(object));
					bool _cache = LuaAPI.lua_toboolean(L, 6);
					
					XLua.LuaHttp gen_ret = new XLua.LuaHttp(_cmd, _data, _successCallback, _customerData, _cache);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				if(LuaAPI.lua_gettop(L) == 5 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && translator.Assignable<DelegateBytes>(L, 4) && translator.Assignable<object>(L, 5))
				{
					string _cmd = LuaAPI.lua_tostring(L, 2);
					byte[] _data = LuaAPI.lua_tobytes(L, 3);
					DelegateBytes _successCallback = translator.GetDelegate<DelegateBytes>(L, 4);
					object _customerData = translator.GetObject(L, 5, typeof(object));
					
					XLua.LuaHttp gen_ret = new XLua.LuaHttp(_cmd, _data, _successCallback, _customerData);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to XLua.LuaHttp constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnGlobalError_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    HttpErrorVo _vo = (HttpErrorVo)translator.GetObject(L, 1, typeof(HttpErrorVo));
                    
                    XLua.LuaHttp.OnGlobalError( _vo );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendRequest(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLua.LuaHttp gen_to_be_invoked = (XLua.LuaHttp)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.SendRequest(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HttpTimeOut(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLua.LuaHttp gen_to_be_invoked = (XLua.LuaHttp)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.HttpTimeOut);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_CustomerData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLua.LuaHttp gen_to_be_invoked = (XLua.LuaHttp)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.CustomerData);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_AutoRetry(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLua.LuaHttp gen_to_be_invoked = (XLua.LuaHttp)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.AutoRetry);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_HttpTimeOut(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLua.LuaHttp gen_to_be_invoked = (XLua.LuaHttp)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.HttpTimeOut = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_CustomerData(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLua.LuaHttp gen_to_be_invoked = (XLua.LuaHttp)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.CustomerData = translator.GetObject(L, 2, typeof(object));
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_AutoRetry(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                XLua.LuaHttp gen_to_be_invoked = (XLua.LuaHttp)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.AutoRetry = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
