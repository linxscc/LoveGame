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
    public class XLuaUtilWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(XLuaUtil);
			Utils.BeginObjectRegister(type, L, translator, 0, 1, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SendLuaHttp", _m_SendLuaHttp);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 6, 0, 0);
			Utils.RegisterFunc(L, Utils.CLS_IDX, "FindGenericMethod", _m_FindGenericMethod_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "TriggerEvent1", _m_TriggerEvent1_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "TriggerEvent2", _m_TriggerEvent2_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "TriggerEvent3", _m_TriggerEvent3_xlua_st_);
            Utils.RegisterFunc(L, Utils.CLS_IDX, "TriggerEvent4", _m_TriggerEvent4_xlua_st_);
            
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					XLuaUtil gen_ret = new XLuaUtil();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to XLuaUtil constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FindGenericMethod_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _className = LuaAPI.lua_tostring(L, 1);
                    string _methodName = LuaAPI.lua_tostring(L, 2);
                    string _originalName = LuaAPI.lua_tostring(L, 3);
                    System.Type[] _types = translator.GetParams<System.Type>(L, 4);
                    
                        System.Reflection.MethodInfo gen_ret = XLuaUtil.FindGenericMethod( _className, _methodName, _originalName, _types );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TriggerEvent1_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _eventName = LuaAPI.lua_tostring(L, 1);
                    object _arg1 = translator.GetObject(L, 2, typeof(object));
                    
                    XLuaUtil.TriggerEvent1( _eventName, _arg1 );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TriggerEvent2_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _eventName = LuaAPI.lua_tostring(L, 1);
                    object _arg1 = translator.GetObject(L, 2, typeof(object));
                    object _arg2 = translator.GetObject(L, 3, typeof(object));
                    
                    XLuaUtil.TriggerEvent2( _eventName, _arg1, _arg2 );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TriggerEvent3_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _eventName = LuaAPI.lua_tostring(L, 1);
                    object _arg1 = translator.GetObject(L, 2, typeof(object));
                    object _arg2 = translator.GetObject(L, 3, typeof(object));
                    object _arg3 = translator.GetObject(L, 4, typeof(object));
                    
                    XLuaUtil.TriggerEvent3( _eventName, _arg1, _arg2, _arg3 );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TriggerEvent4_xlua_st_(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
            
                
                {
                    string _eventName = LuaAPI.lua_tostring(L, 1);
                    object _arg1 = translator.GetObject(L, 2, typeof(object));
                    object _arg2 = translator.GetObject(L, 3, typeof(object));
                    object _arg3 = translator.GetObject(L, 4, typeof(object));
                    object _arg4 = translator.GetObject(L, 5, typeof(object));
                    
                    XLuaUtil.TriggerEvent4( _eventName, _arg1, _arg2, _arg3, _arg4 );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SendLuaHttp(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                XLuaUtil gen_to_be_invoked = (XLuaUtil)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 9&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DelegateBytes>(L, 4)&& translator.Assignable<object>(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)&& (LuaAPI.lua_isnil(L, 7) || LuaAPI.lua_type(L, 7) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 8) || LuaAPI.lua_type(L, 8) == LuaTypes.LUA_TSTRING)&& LuaTypes.LUA_TNUMBER == LuaAPI.lua_type(L, 9)) 
                {
                    string _cmd = LuaAPI.lua_tostring(L, 2);
                    byte[] _data = LuaAPI.lua_tobytes(L, 3);
                    DelegateBytes _successCallback = translator.GetDelegate<DelegateBytes>(L, 4);
                    object _customerData = translator.GetObject(L, 5, typeof(object));
                    bool _cache = LuaAPI.lua_toboolean(L, 6);
                    string _version = LuaAPI.lua_tostring(L, 7);
                    string _serverUrl = LuaAPI.lua_tostring(L, 8);
                    int _httpTimeout = LuaAPI.xlua_tointeger(L, 9);
                    
                        XLua.LuaHttp gen_ret = gen_to_be_invoked.SendLuaHttp( _cmd, _data, _successCallback, _customerData, _cache, _version, _serverUrl, _httpTimeout );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 8&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DelegateBytes>(L, 4)&& translator.Assignable<object>(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)&& (LuaAPI.lua_isnil(L, 7) || LuaAPI.lua_type(L, 7) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 8) || LuaAPI.lua_type(L, 8) == LuaTypes.LUA_TSTRING)) 
                {
                    string _cmd = LuaAPI.lua_tostring(L, 2);
                    byte[] _data = LuaAPI.lua_tobytes(L, 3);
                    DelegateBytes _successCallback = translator.GetDelegate<DelegateBytes>(L, 4);
                    object _customerData = translator.GetObject(L, 5, typeof(object));
                    bool _cache = LuaAPI.lua_toboolean(L, 6);
                    string _version = LuaAPI.lua_tostring(L, 7);
                    string _serverUrl = LuaAPI.lua_tostring(L, 8);
                    
                        XLua.LuaHttp gen_ret = gen_to_be_invoked.SendLuaHttp( _cmd, _data, _successCallback, _customerData, _cache, _version, _serverUrl );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 7&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DelegateBytes>(L, 4)&& translator.Assignable<object>(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)&& (LuaAPI.lua_isnil(L, 7) || LuaAPI.lua_type(L, 7) == LuaTypes.LUA_TSTRING)) 
                {
                    string _cmd = LuaAPI.lua_tostring(L, 2);
                    byte[] _data = LuaAPI.lua_tobytes(L, 3);
                    DelegateBytes _successCallback = translator.GetDelegate<DelegateBytes>(L, 4);
                    object _customerData = translator.GetObject(L, 5, typeof(object));
                    bool _cache = LuaAPI.lua_toboolean(L, 6);
                    string _version = LuaAPI.lua_tostring(L, 7);
                    
                        XLua.LuaHttp gen_ret = gen_to_be_invoked.SendLuaHttp( _cmd, _data, _successCallback, _customerData, _cache, _version );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 6&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DelegateBytes>(L, 4)&& translator.Assignable<object>(L, 5)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 6)) 
                {
                    string _cmd = LuaAPI.lua_tostring(L, 2);
                    byte[] _data = LuaAPI.lua_tobytes(L, 3);
                    DelegateBytes _successCallback = translator.GetDelegate<DelegateBytes>(L, 4);
                    object _customerData = translator.GetObject(L, 5, typeof(object));
                    bool _cache = LuaAPI.lua_toboolean(L, 6);
                    
                        XLua.LuaHttp gen_ret = gen_to_be_invoked.SendLuaHttp( _cmd, _data, _successCallback, _customerData, _cache );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 5&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING)&& translator.Assignable<DelegateBytes>(L, 4)&& translator.Assignable<object>(L, 5)) 
                {
                    string _cmd = LuaAPI.lua_tostring(L, 2);
                    byte[] _data = LuaAPI.lua_tobytes(L, 3);
                    DelegateBytes _successCallback = translator.GetDelegate<DelegateBytes>(L, 4);
                    object _customerData = translator.GetObject(L, 5, typeof(object));
                    
                        XLua.LuaHttp gen_ret = gen_to_be_invoked.SendLuaHttp( _cmd, _data, _successCallback, _customerData );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to XLuaUtil.SendLuaHttp!");
            
        }
        
        
        
        
        
        
		
		
		
		
    }
}
