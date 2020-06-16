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
    public class SystemReflectionMethodInfoWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(System.Reflection.MethodInfo);
			Utils.BeginObjectRegister(type, L, translator, 1, 7, 4, 0);
			Utils.RegisterFunc(L, Utils.OBJ_META_IDX, "__eq", __EqMeta);
            
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Equals", _m_Equals);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetHashCode", _m_GetHashCode);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetBaseDefinition", _m_GetBaseDefinition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetGenericArguments", _m_GetGenericArguments);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetGenericMethodDefinition", _m_GetGenericMethodDefinition);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "MakeGenericMethod", _m_MakeGenericMethod);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CreateDelegate", _m_CreateDelegate);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "MemberType", _g_get_MemberType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ReturnType", _g_get_ReturnType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ReturnParameter", _g_get_ReturnParameter);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "ReturnTypeCustomAttributes", _g_get_ReturnTypeCustomAttributes);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "System.Reflection.MethodInfo does not have a constructor!");
        }
        
		
        
		
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __EqMeta(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
			
				if (translator.Assignable<System.Reflection.MethodInfo>(L, 1) && translator.Assignable<System.Reflection.MethodInfo>(L, 2))
				{
					System.Reflection.MethodInfo leftside = (System.Reflection.MethodInfo)translator.GetObject(L, 1, typeof(System.Reflection.MethodInfo));
					System.Reflection.MethodInfo rightside = (System.Reflection.MethodInfo)translator.GetObject(L, 2, typeof(System.Reflection.MethodInfo));
					
					LuaAPI.lua_pushboolean(L, leftside == rightside);
					
					return 1;
				}
            
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to right hand of == operator, need System.Reflection.MethodInfo!");
            
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Equals(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Reflection.MethodInfo gen_to_be_invoked = (System.Reflection.MethodInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    object _obj = translator.GetObject(L, 2, typeof(object));
                    
                        bool gen_ret = gen_to_be_invoked.Equals( _obj );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetHashCode(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Reflection.MethodInfo gen_to_be_invoked = (System.Reflection.MethodInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        int gen_ret = gen_to_be_invoked.GetHashCode(  );
                        LuaAPI.xlua_pushinteger(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetBaseDefinition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Reflection.MethodInfo gen_to_be_invoked = (System.Reflection.MethodInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.Reflection.MethodInfo gen_ret = gen_to_be_invoked.GetBaseDefinition(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGenericArguments(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Reflection.MethodInfo gen_to_be_invoked = (System.Reflection.MethodInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.Type[] gen_ret = gen_to_be_invoked.GetGenericArguments(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetGenericMethodDefinition(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Reflection.MethodInfo gen_to_be_invoked = (System.Reflection.MethodInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.Reflection.MethodInfo gen_ret = gen_to_be_invoked.GetGenericMethodDefinition(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_MakeGenericMethod(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Reflection.MethodInfo gen_to_be_invoked = (System.Reflection.MethodInfo)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Type[] _typeArguments = translator.GetParams<System.Type>(L, 2);
                    
                        System.Reflection.MethodInfo gen_ret = gen_to_be_invoked.MakeGenericMethod( _typeArguments );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CreateDelegate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                System.Reflection.MethodInfo gen_to_be_invoked = (System.Reflection.MethodInfo)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& translator.Assignable<System.Type>(L, 2)) 
                {
                    System.Type _delegateType = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    
                        System.Delegate gen_ret = gen_to_be_invoked.CreateDelegate( _delegateType );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& translator.Assignable<System.Type>(L, 2)&& translator.Assignable<object>(L, 3)) 
                {
                    System.Type _delegateType = (System.Type)translator.GetObject(L, 2, typeof(System.Type));
                    object _target = translator.GetObject(L, 3, typeof(object));
                    
                        System.Delegate gen_ret = gen_to_be_invoked.CreateDelegate( _delegateType, _target );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to System.Reflection.MethodInfo.CreateDelegate!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_MemberType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                System.Reflection.MethodInfo gen_to_be_invoked = (System.Reflection.MethodInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.MemberType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ReturnType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                System.Reflection.MethodInfo gen_to_be_invoked = (System.Reflection.MethodInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ReturnType);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ReturnParameter(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                System.Reflection.MethodInfo gen_to_be_invoked = (System.Reflection.MethodInfo)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.ReturnParameter);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_ReturnTypeCustomAttributes(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                System.Reflection.MethodInfo gen_to_be_invoked = (System.Reflection.MethodInfo)translator.FastGetCSObj(L, 1);
                translator.PushAny(L, gen_to_be_invoked.ReturnTypeCustomAttributes);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
