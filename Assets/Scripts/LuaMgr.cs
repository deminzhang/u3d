using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class LuaMgr : MonoBehaviour {
    const string LUADLL = "lua51";

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int LuaCSFunction(IntPtr luaState);
#else
	public delegate int LuaCSFunction(IntPtr luaState);
#endif
    //public delegate int LuaFunctionCallback(IntPtr luaState);
    //===============================================================
    //lua.h
    /* ** pseudo-indices  */
    public const int
        LUA_REGISTRYINDEX = -10000,
        LUA_ENVIRONINDEX = -10001,
        LUA_GLOBALSINDEX = -10002;
    /* ** basic types    */
    public const int
        LUA_TNONE = -1,
        LUA_TNIL = 0,
        LUA_TBOOLEAN = 1,
        LUA_TLIGHTUSERDATA = 2,
        LUA_TNUMBER = 3,
        LUA_TSTRING = 4,
        LUA_TTABLE = 5,
        LUA_TFUNCTION = 6,
        LUA_TUSERDATA = 7,
        LUA_TTHREAD = 8;
    /* thread status; 0 is OK */
    public const int
        LUA_YIELD = 1,
        LUA_ERRRUN = 2,
        LUA_ERRSYNTAX = 3,
        LUA_ERRMEM = 4,
        LUA_ERRERR = 5;
    public const int
        LUA_GCSTOP = 0,
        LUA_GCRESTART = 1,
        LUA_GCCOLLECT = 2,
        LUA_GCCOUNT = 3,
        LUA_GCCOUNTB = 4,
        LUA_GCSTEP = 5,
        LUA_GCSETPAUSE = 6,
        LUA_GCSETSTEPMUL = 7;
    public struct luaL_Reg
    {
        public string name;
        public LuaCSFunction func;
        public luaL_Reg(string _name, LuaCSFunction _func)
        {
            name = _name;
            func = _func;
        }
    }
    public static int lua_upvalueindex(int i)
    {
        return LUA_GLOBALSINDEX - i;
    }
    /*    ** state manipulation
    LUA_API lua_State *(lua_newstate) (lua_Alloc f, void* ud);
    LUA_API void       (lua_close) (lua_State* L);
    LUA_API lua_State *(lua_newthread) (lua_State* L);
    LUA_API lua_CFunction(lua_atpanic) (lua_State* L, lua_CFunction panicf);    */
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_close(IntPtr luaState);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern IntPtr lua_newthread(IntPtr luaState);
    
    /* ** basic stack manipulation   
    LUA_API int   (lua_gettop) (lua_State* L);
    LUA_API void  (lua_settop) (lua_State* L, int idx);
    LUA_API void  (lua_pushvalue) (lua_State* L, int idx);
    LUA_API void  (lua_remove) (lua_State* L, int idx);
    LUA_API void  (lua_insert) (lua_State* L, int idx);
    LUA_API void  (lua_replace) (lua_State* L, int idx);
    LUA_API int   (lua_checkstack) (lua_State* L, int sz);
    LUA_API void  (lua_xmove) (lua_State* from, lua_State *to, int n);     */
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_gettop(IntPtr luaState);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_settop(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_pushvalue(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_remove(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_insert(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_replace(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_checkstack(IntPtr luaState, int sz);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_xmove(IntPtr fromluaState, IntPtr toluaState, int n);
    /*  ** access functions (stack -> C)
    LUA_API int             (lua_isnumber) (lua_State* L, int idx);
    LUA_API int             (lua_isstring) (lua_State* L, int idx);
    LUA_API int             (lua_iscfunction) (lua_State* L, int idx);
    LUA_API int             (lua_isuserdata) (lua_State* L, int idx);
    LUA_API int             (lua_type) (lua_State* L, int idx);
    LUA_API const char*(lua_typename)(lua_State * L, int tp);
    LUA_API int            (lua_equal) (lua_State* L, int idx1, int idx2);
    LUA_API int            (lua_rawequal) (lua_State* L, int idx1, int idx2);
    LUA_API int            (lua_lessthan) (lua_State* L, int idx1, int idx2);
    LUA_API lua_Number(lua_tonumber) (lua_State* L, int idx);
    LUA_API lua_Integer(lua_tointeger) (lua_State* L, int idx);
    LUA_API int             (lua_toboolean) (lua_State* L, int idx);
    LUA_API const char*(lua_tolstring)(lua_State * L, int idx, size_t * len);
    LUA_API size_t(lua_objlen) (lua_State* L, int idx);
    LUA_API lua_CFunction(lua_tocfunction) (lua_State* L, int idx);
    LUA_API void*(lua_touserdata) (lua_State* L, int idx);
    LUA_API lua_State      *(lua_tothread) (lua_State* L, int idx);
    LUA_API const void*(lua_topointer)(lua_State * L, int idx);    */
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_isnumber(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_isstring(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_iscfunction(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_isuserdata(IntPtr luaState, int idx);
    public static bool lua_isnum(IntPtr luaState, int type)
    {
        return lua_isnumber(luaState, type) != 0;
    }
    public static bool lua_isstr(IntPtr luaState, int type)
    {
        return lua_isstring(luaState, type) != 0;
    }
    public static bool lua_isfunc(IntPtr luaState, int type)
    {
        return lua_iscfunction(luaState, type) != 0;
    }
    public static bool lua_isudata(IntPtr luaState, int type)
    {
        return lua_isuserdata(luaState, type) != 0;
    }
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_type(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern IntPtr lua_typename(IntPtr luaState, int type);
    public static string lua_typenameCS(IntPtr luaState, int type)
    {
        IntPtr p = lua_typename(luaState, type);
        return Marshal.PtrToStringAnsi(p);
    }
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern IntPtr lua_equal(IntPtr luaState, int idx1, int idx2);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern IntPtr lua_rawequal(IntPtr luaState, int idx1, int idx2);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern IntPtr lua_lessthan(IntPtr luaState, int idx1, int idx2);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern double lua_tonumber(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_tointeger(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_toboolean(IntPtr luaState, int idx);
    public static bool lua_tobool(IntPtr luaState, int idx)
    {
        return lua_toboolean(luaState, idx)==1;
    }
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern IntPtr lua_tolstring(IntPtr luaState, int idx, out int len);
    public static string lua_tostring(IntPtr luaState, int idx)
    {
        int len;
        IntPtr str = lua_tolstring(luaState, idx, out len);
        if (str != IntPtr.Zero)
        {
            return Marshal.PtrToStringAnsi(str, len);
        }
        return null;
    }
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern IntPtr lua_objlen(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern IntPtr lua_tocfunction(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern IntPtr lua_touserdata(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern IntPtr lua_tothread(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern IntPtr lua_topointer(IntPtr luaState, int idx);
    public static int lua_topointerInt32(IntPtr luaState, int idx)
    {
        IntPtr p = lua_topointer(luaState, idx);
        return p.ToInt32();
    }
    /*    ** push functions (C -> stack)
    LUA_API void  (lua_pushnil) (lua_State* L);
    LUA_API void  (lua_pushnumber) (lua_State* L, lua_Number n);
    LUA_API void  (lua_pushinteger) (lua_State* L, lua_Integer n);
    LUA_API void  (lua_pushlstring) (lua_State* L, const char* s, size_t l);
    LUA_API void  (lua_pushstring) (lua_State* L, const char* s);
    LUA_API const char*(lua_pushvfstring)(lua_State * L, const char* fmt, va_list argp);
    LUA_API const char*(lua_pushfstring)(lua_State * L, const char* fmt, ...);
    LUA_API void  (lua_pushcclosure) (lua_State* L, lua_CFunction fn, int n);
    LUA_API void  (lua_pushboolean) (lua_State* L, int b);
    LUA_API void  (lua_pushlightuserdata) (lua_State* L, void* p);
    LUA_API int   (lua_pushthread) (lua_State* L);    */
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_pushnil(IntPtr luaState);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_pushnumber(IntPtr luaState, double number);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_pushinteger(IntPtr luaState, int i);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_pushstring(IntPtr luaState, byte[] str);
    public static void lua_pushstring(IntPtr luaState, string str)
    {
        lua_pushstring(luaState, System.Text.Encoding.Default.GetBytes(str));
    }
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_pushlstring(IntPtr luaState, byte[] str, uint size);
    public static void lua_pushlstring(IntPtr luaState, byte[] str, int size)
    {
        lua_pushlstring(luaState, str, (uint)size);
    }
    public static void lua_pushlstring(IntPtr luaState, string str, int size)
    {
        lua_pushlstring(luaState, System.Text.Encoding.Default.GetBytes(str), size);
    }

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_pushcclosure(IntPtr l, IntPtr f, int nup);
    public static void lua_pushcclosure(IntPtr l, LuaCSFunction f, int nup)
    {
        GCHandle.Alloc(f); //TODO what time to Free?
        IntPtr fn = Marshal.GetFunctionPointerForDelegate(f);
        lua_pushcclosure(l, fn, nup);
    }
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_pushboolean(IntPtr luaState, int value);
    public static void lua_pushboolean(IntPtr luaState, bool value)
    {
        lua_pushboolean(luaState, value ? 1 : 0);
    }
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_pushlightuserdata(IntPtr luaState, IntPtr udata);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_pushthread(IntPtr luaState);
    /*    ** get functions (Lua -> stack)
    LUA_API void  (lua_gettable) (lua_State* L, int idx);
    LUA_API void  (lua_getfield) (lua_State* L, int idx, const char* k);
    LUA_API void  (lua_rawget) (lua_State* L, int idx);
    LUA_API void  (lua_rawgeti) (lua_State* L, int idx, int n);
    LUA_API void  (lua_createtable) (lua_State* L, int narr, int nrec);
    LUA_API void*(lua_newuserdata) (lua_State* L, size_t sz);
    LUA_API int   (lua_getmetatable) (lua_State* L, int objindex);
    LUA_API void  (lua_getfenv) (lua_State* L, int idx);    */
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_gettable(IntPtr luaState, int index);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_getfield(IntPtr luaState, int idx, string meta);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_rawget(IntPtr luaState, int index);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_rawgeti(IntPtr luaState, int tableIndex, int index);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_createtable(IntPtr luaState, int narr, int nrec);
    //lua_newuserdata TODO need wrap
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern IntPtr lua_newuserdata(IntPtr luaState, uint size);

    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_getmetatable(IntPtr luaState, int objIndex);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_getfenv(IntPtr luaState, int idx);
    /*    ** set functions (stack -> Lua)
    LUA_API void  (lua_settable) (lua_State* L, int idx);
    LUA_API void  (lua_setfield) (lua_State* L, int idx, const char* k);
    LUA_API void  (lua_rawset) (lua_State* L, int idx);
    LUA_API void  (lua_rawseti) (lua_State* L, int idx, int n);
    LUA_API int   (lua_setmetatable) (lua_State* L, int objindex);
    LUA_API int   (lua_setfenv) (lua_State* L, int idx);    */
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_settable(IntPtr luaState, int index);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_setfield(IntPtr luaState, int idx, string name);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_rawset(IntPtr luaState, int index);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_rawseti(IntPtr luaState, int idx, int n);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_setmetatable(IntPtr luaState, int objIndex);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_setfenv(IntPtr luaState, int idx);

    /*  ** `load' and `call' functions (load and run Lua code)
    LUA_API void  (lua_call) (lua_State* L, int nargs, int nresults);
    LUA_API int   (lua_pcall) (lua_State* L, int nargs, int nresults, int errfunc);
    LUA_API int   (lua_cpcall) (lua_State* L, lua_CFunction func, void* ud);
    LUA_API int   (lua_load) (lua_State* L, lua_Reader reader, void* dt,  const char* chunkname);
    LUA_API int (lua_dump) (lua_State* L, lua_Writer writer, void* data);    */
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_call(IntPtr luaState, int nArgs, int nResults);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_pcall(IntPtr luaState, int nArgs, int nResults, int errfunc);

    /*   ** coroutine functions
    LUA_API int  (lua_yield) (lua_State* L, int nresults);
    LUA_API int  (lua_resume) (lua_State* L, int narg);
    LUA_API int  (lua_status) (lua_State* L);    */
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_yield(IntPtr luaState, int nresultss);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_resume(IntPtr luaState, int narg);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_status(IntPtr luaState);
    /*    ** garbage-collection function and options
    LUA_API int (lua_gc) (lua_State* L, int what, int data);    */
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_gc(IntPtr luaState, int what, int data);

    /*   ** miscellaneous functions
    LUA_API int   (lua_error) (lua_State* L);
    LUA_API int   (lua_next) (lua_State* L, int idx);
    LUA_API void  (lua_concat) (lua_State* L, int n);
    LUA_API lua_Alloc(lua_getallocf) (lua_State* L, void** ud);
    LUA_API void lua_setallocf(lua_State* L, lua_Alloc f, void* ud);    */
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_error(IntPtr luaState);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int lua_next(IntPtr luaState, int idx);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_concat(IntPtr luaState, int n);

    /*    ** ===============================================================
    ** some useful macros
    #define lua_pop(L,n)		lua_settop(L, -(n)-1)
    #define lua_newtable(L)		lua_createtable(L, 0, 0)
    #define lua_register(L,n,f) (lua_pushcfunction(L, (f)), lua_setglobal(L, (n)))
    #define lua_pushcfunction(L,f)	lua_pushcclosure(L, (f), 0)
    #define lua_strlen(L,i)		lua_objlen(L, (i))
    #define lua_isfunction(L,n)	(lua_type(L, (n)) == LUA_TFUNCTION)
    #define lua_istable(L,n)	(lua_type(L, (n)) == LUA_TTABLE)
    #define lua_islightuserdata(L,n)	(lua_type(L, (n)) == LUA_TLIGHTUSERDATA)
    #define lua_isnil(L,n)		(lua_type(L, (n)) == LUA_TNIL)
    #define lua_isboolean(L,n)	(lua_type(L, (n)) == LUA_TBOOLEAN)
    #define lua_isthread(L,n)	(lua_type(L, (n)) == LUA_TTHREAD)
    #define lua_isnone(L,n)		(lua_type(L, (n)) == LUA_TNONE)
    #define lua_isnoneornil(L, n)	(lua_type(L, (n)) <= 0)
    #define lua_pushliteral(L, s)   lua_pushlstring(L, "" s, (sizeof(s)/sizeof(char))-1)
    #define lua_setglobal(L,s)	lua_setfield(L, LUA_GLOBALSINDEX, (s))
    #define lua_getglobal(L,s)	lua_getfield(L, LUA_GLOBALSINDEX, (s))
    #define lua_tostring(L,i)	lua_tolstring(L, (i), NULL)    */
    public static void lua_pop(IntPtr luaState, int n)
    {
        lua_settop(luaState, -(n) - 1);
    }
    public static void lua_newtable(IntPtr luaState)
    {
        lua_createtable(luaState, 0, 0);
    }
    public static void lua_register(IntPtr luaState, string n, LuaCSFunction f)
    {
        lua_pushcclosure(luaState, f, 0);   //lua_pushcsfunction(luaState, (f));
        lua_setglobal(luaState, (n));
    }
    public static void lua_pushcsfunction(IntPtr luaState, LuaCSFunction f) //not lua_pushcfunction
    {
        lua_pushcclosure(luaState, f, 0);
    }
    public static int lua_strlen(IntPtr luaState, int idx)
    {
        return (int)lua_objlen(luaState, idx);
    }
    public static bool lua_isfunction(IntPtr luaState, int idx)
    {
        return lua_type(luaState, (idx)) == LUA_TFUNCTION;
    }
    public static bool lua_istable(IntPtr luaState, int idx)
    {
        return lua_type(luaState, (idx)) == LUA_TTABLE;
    }
    public static bool lua_islightuserdata(IntPtr luaState, int idx)
    {
        return lua_type(luaState, (idx)) == LUA_TLIGHTUSERDATA;
    }
    public static bool lua_isnil(IntPtr luaState, int idx)
    {
        return lua_type(luaState, (idx)) == LUA_TNIL;
    }
    public static bool lua_isboolean(IntPtr luaState, int idx)
    {
        return lua_type(luaState, (idx)) == LUA_TBOOLEAN;
    }
    public static bool lua_isthread(IntPtr luaState, int idx)
    {
        return lua_type(luaState, (idx)) == LUA_TTHREAD;
    }
    public static bool lua_isnone(IntPtr luaState, int idx)
    {
        return lua_type(luaState, (idx)) == LUA_TNONE;
    }
    public static bool lua_isnoneornil(IntPtr luaState, int idx)
    {
        return lua_type(luaState, (idx)) < 0;
    }
    public static void lua_pushliteral(IntPtr luaState, string s)
    {
        lua_pushstring(luaState, s);
    }
    public static void lua_setglobal(IntPtr luaState, string name)
    {
        lua_pushstring(luaState, name);
        lua_insert(luaState, -2);
        lua_settable(luaState, LUA_GLOBALSINDEX);
    }
    public static void lua_getglobal(IntPtr luaState, string name)
    {
        lua_pushstring(luaState, name);
        lua_gettable(luaState, LUA_GLOBALSINDEX);
    }
    /*    ** compatibility macros and functions
    #define lua_open()	luaL_newstate()
    #define lua_getregistry(L)	lua_pushvalue(L, LUA_REGISTRYINDEX)
    #define lua_getgccount(L)	lua_gc(L, LUA_GCCOUNT, 0)
    #define lua_Chunkreader		lua_Reader
    #define lua_Chunkwriter		lua_Writer    */
    public static IntPtr lua_open()
    {
        return luaL_newstate();
    }
    public static void lua_getregistry(IntPtr luaState)
    {
        lua_pushvalue(luaState, LUA_REGISTRYINDEX);
    }
    public static int lua_getgccount(IntPtr luaState)
    {
        return lua_gc(luaState, LUA_GCCOUNT, 0);
    }
    /* hack 
    LUA_API void lua_setlevel(lua_State* from, lua_State* to);    */
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void lua_setlevel(IntPtr fromluaState, IntPtr toluaState);

    //===============================================================
    //lauxlib.h
    //---------------------------------------------------------------
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern IntPtr luaL_newstate();
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void luaL_openlibs(IntPtr luaState);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void luaL_where(IntPtr luaState, int level);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int luaL_loadfile(IntPtr luaState, string fn);
    public static int luaL_dofile(IntPtr luaState, string fn)
    {
        int result = luaL_loadfile(luaState, fn);
        if (result != 0)
            return result;
        return lua_pcall(luaState, 0, -1, 0);
    }
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int luaL_loadstring(IntPtr luaState, string chunk); 
    public static int luaL_dostring(IntPtr luaState, string chunk)
    {
        int result = luaL_loadstring(luaState, chunk);
        if (result != 0)
            return result;
        return lua_pcall(luaState, 0, -1, 0);
    }
    public static int lua_dostring(IntPtr luaState, string chunk)
    {
        return luaL_dostring(luaState, chunk);
    }
    public static void luaL_getmetatable(IntPtr luaState, string meta)
    {
        lua_getfield(luaState, LUA_REGISTRYINDEX, meta);
    }
    //LUALIB_API int   (luaL_newmetatable) (lua_State* L, const char* tname);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int luaL_newmetatable(IntPtr luaState, string tname);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int luaL_getmetafield(IntPtr luaState, int stackPos, string field);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern int luaL_ref(IntPtr luaState, int t);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void luaL_unref(IntPtr luaState, int t, int refn);
    //#define lua_ref(L,lock) ((lock) ? luaL_ref(L, LUA_REGISTRYINDEX) :  (lua_pushstring(L, "unlocked references are obsolete"), lua_error(L), 0))
    //#define lua_unref(L,ref)        luaL_unref(L, LUA_REGISTRYINDEX, (ref))
    //#define lua_getref(L,ref)       lua_rawgeti(L, LUA_REGISTRYINDEX, (ref))
    //LUALIB_API void (luaL_checkstack) (lua_State* L, int sz, const char* msg);
    //LUALIB_API void *(luaL_checkudata) (lua_State *L, int ud, const char *tname);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern void luaL_checkstack(IntPtr luaState, int sz, string msg);
    [DllImport(LUADLL, CallingConvention = CallingConvention.Cdecl)]    public static extern IntPtr luaL_checkudata(IntPtr luaState, int ud, string tname);
    public static int lua_ref(IntPtr luaState, int idx)
    {
        if (idx!=0)
            return luaL_ref(luaState, LUA_REGISTRYINDEX);
        else {
            lua_pushstring(luaState, "unlocked references are obsolete");
            lua_error(luaState);
            return 0;
        }
    }
	public static int lua_refi(IntPtr L, int idx)
	{
		if (idx == -1)
			return lua_ref(L, -1);
		lua_pushvalue(L, idx);
		int n = lua_ref(L, -1);
		lua_pop(L, 1);
		return n;
	}
    public static void lua_unref(IntPtr luaState, int refn)
    {
        luaL_unref(luaState, LUA_REGISTRYINDEX, refn);
    }
    public static void lua_getref(IntPtr luaState, int refn)
    {
        lua_rawgeti(luaState, LUA_REGISTRYINDEX, refn);
    }
    
    public static void luaL_setfuncs(IntPtr L, luaL_Reg[] l, int nup) {
        luaL_checkstack(L, nup + 1, "too many upvalues");
        for (int e = 0; e < l.Length; e++)
        {
            lua_pushstring(L, l[e].name);
            int i;
            for (i = 0; i < nup; i++)   /* copy upvalues to the top */
                lua_pushvalue(L, -(nup + 1));
            lua_pushcclosure(L, l[e].func, nup);  /* closure with those upvalues */
            lua_settable(L, -(nup + 3));
        }
        lua_pop(L, nup);	/* remove upvalues */
    }
    //==standard method }===================================================================

    //======================================================================================
    //reg
    private static IntPtr _luaState = IntPtr.Zero;
    static int mHideTable, mWeakKTable, mWeakVTable, mWeakVOid2Tb;
    static void lua_init(IntPtr L)
    {
        //ref hidetable
        lua_createtable(L, 0, 0);
        mHideTable = luaL_ref(L, LUA_REGISTRYINDEX);
        //ref weakk table
        lua_createtable(L, 0, 0); //weakk
        lua_createtable(L, 0, 1);
        lua_pushliteral(L, "__mode");
        lua_pushliteral(L, "k");
        lua_rawset(L, -3);
        lua_setmetatable(L, -2);
        mWeakKTable = luaL_ref(L, LUA_REGISTRYINDEX);
        //ref weakv table
        lua_createtable(L, 0, 0); //weakv
        lua_createtable(L, 0, 1);
        lua_pushliteral(L, "__mode");
        lua_pushliteral(L, "v");
        lua_rawset(L, -3);
        lua_setmetatable(L, -2);
        mWeakVTable = luaL_ref(L, LUA_REGISTRYINDEX);

        lua_createtable(L, 0, 0); //weakv
        lua_createtable(L, 0, 1);
        lua_pushliteral(L, "__mode");
        lua_pushliteral(L, "v");
        lua_rawset(L, -3);
        lua_setmetatable(L, -2);
        mWeakVOid2Tb = luaL_ref(L, LUA_REGISTRYINDEX);
    }
    public static IntPtr LuaState
    {
        get { return _luaState; }
    }
    public static void lua_GetWeakK(IntPtr L)
    {
        lua_getref(L, mWeakKTable);
    }
    public static void lua_GetWeakV(IntPtr L)
    {
        lua_getref(L, mWeakVTable);
    }
    public static void lua_GetHideT(IntPtr L)
    {
        lua_getref(L, mHideTable);
    }
    public static void lua_GetObjWV(IntPtr L)
    {
        lua_getref(L, mWeakVOid2Tb);
    }

    public static void lua_errorEx(IntPtr L, string format, params object[] list) {
        //for (int x = 1; (luaL_where(L, x), !lua_tostring(L, -1)[0] && x < 10); x++);
        //va_list va; va_start(va, format);
        //lua_pushvfstring(L, format, ...);
        //va_end(va);
        //lua_concat(L, 2);
        Debug.Assert(false, format);
        lua_error(L);
    }
    public static void lua_HideUserdata(IntPtr L, string type, string utype)
    {
        if (lua_type(L, -1) != LUA_TUSERDATA) {
            lua_errorEx(L, "need userdata");
            return;
        }
        luaL_getmetatable(L, utype); //ud use __gc
        lua_setmetatable(L, -2); //setmetatable(ud,mt) top=ud
        lua_createtable(L, 0, 0);//o
        luaL_getmetatable(L, type); //o use others
        lua_setmetatable(L, -2); //setmetatable(ud,mt) top=o
        lua_getref(L, mWeakKTable);
        lua_pushvalue(L, -2); //k=o
        lua_pushvalue(L, -4); //v=ud
        lua_rawset(L, -3);  //weakk[k]=v
        lua_pop(L, 1); //pop weakk top=o
        lua_getref(L, mWeakVTable);
        int pk = lua_topointer(L, -3).ToInt32();
        lua_pushnumber(L, pk);  //k=p
        lua_pushvalue(L, -3); //v=o
        lua_rawset(L, -3);  //weakv[k]=o
        lua_pop(L, 1); //pop weakv top=o
    }
    IntPtr lua_GetUserdata(IntPtr L, int idx, string utype) //tb >> ud
    {
        lua_getref(L, mWeakKTable);
        lua_pushvalue(L, idx>0 ? idx : idx - 1);	//k=o
        lua_rawget(L, -2);  //u = weakk[k]
        IntPtr p = luaL_checkudata(L, -1, utype);
        //lua_replace(L, -2);
        lua_pop(L, 2);
	    return p;
    }
    public static void lua_GetObj(IntPtr L, IntPtr p)
    {
        lua_getref(L, mWeakVTable);//wv
        int pk = p.ToInt32();//int pk = (int)p;
        lua_pushnumber(L, pk);
        lua_rawget(L, -2);//wv o
        lua_replace(L, -2);//o
    }

    public static void lua_RegMetaU(IntPtr L, string type, luaL_Reg[] methods)
    {
        luaL_newmetatable(L, type);
        lua_pushstring(L, type);
        lua_setfield(L, -2, "_TYPE");
        luaL_setfuncs(L, methods, 0);
        lua_pop(L, 1);
    }
    public static void lua_RegClass(IntPtr L, string type, luaL_Reg[] methods)
    {
        luaL_newmetatable(L, type);
        lua_pushstring(L, type);
        lua_setfield(L, -2, "_TYPE");
        luaL_setfuncs(L, methods, 0);
        lua_pushvalue(L, -1);
        lua_setfield(L, -2, "__index");
        lua_pop(L, 1);
    }
    
    private static int lua_print(IntPtr L)
    {
        int top = lua_gettop(L);
        string s = "";
        for (int i = 1; i<= top; i++)
        {
            int t = lua_type(L, i);
            if (t == LUA_TNONE)  break;
            lua_getglobal(L, "tostring");
            lua_pushvalue(L, i);
            lua_pcall(L, 1, 1, 0);
            if(i>1) s = s + '\t';
            s = s + lua_tostring(L, -1);
        }
        print(s);
        return 0;
    }
    //--------------------------------------------------------------------------------------
    static void RegObj(IntPtr L, string type)
    {
        luaL_Reg[] methods = new luaL_Reg[2];
        methods[1].name = "__gc";
        methods[1].func = lua_ObjGC;

        luaL_newmetatable(L, type);
        lua_pushstring(L, type);
        lua_setfield(L, -2, "_TYPE");
        
        if (methods != null)
            luaL_setfuncs(L, methods, 0);
        lua_pushvalue(L, -1);
        lua_setfield(L, -2, "__index");
        lua_pop(L, 1);
    }
    static int lua_getObj(IntPtr L, GameObject obj)//ing
    {
        int oid = obj.GetInstanceID();
        lua_GetWeakV(L);
        lua_pushnumber(L, oid);
        lua_rawget(L, -2);
        IntPtr u;
        if (lua_isudata(L, -1)) //if wv[id] return wv[id]
        {
            u = lua_touserdata(L, -1);
            lua_GetWeakV(L);
            lua_pushnumber(L, u.ToInt32());
            lua_rawget(L, -2); //tb
            return 1;
        }


        return 1;
    }
    static int lua_pushObj(IntPtr L, GameObject obj)//ing
    {
        int oid = obj.GetInstanceID();
        lua_GetWeakV(L);
        lua_pushnumber(L, oid);
        lua_rawget(L, -2);
        IntPtr u;
        if (lua_isudata(L, -1)) //if wv[id] return wv[id]
        {
            u = lua_touserdata(L, -1);
            lua_GetWeakV(L);
            lua_pushnumber(L, u.ToInt32());
            lua_rawget(L, -2); //tb
            return 1;
        }


        return 1;
    }
    static int lua_FindObjByName(IntPtr L)//ing
    {
        string name = lua_tostring(L, 1);
        GameObject obj = GameObject.Find(name);
        if (obj == null) return 0;
        //lua_getObj(L, obj);
        int oid = obj.GetInstanceID();
        lua_GetWeakV(L);
        lua_pushnumber(L, oid);
        lua_rawget(L, -2);
        IntPtr u;
        if (lua_isudata(L, -1)) //if wv[oid] return wv[oid]
        {
            u = lua_touserdata(L, -1);
            lua_GetWeakV(L);
            lua_pushnumber(L, u.ToInt32() );
            lua_rawget(L, -2); //tb
            return 1;
        }
        //lua_pushOjbect(L, ojb);
        u = lua_newuserdata(L, sizeof(int));
        //int pk = lua_topointer(L, -1).ToInt32();

        //ud.gch = gch
        int gchid = GCHandle.ToIntPtr(GCHandle.Alloc(obj)).ToInt32();
        Marshal.WriteInt32(u, gchid); 
        //wv[oid] = ud
        lua_GetWeakV(L);
        lua_pushnumber(L, oid);
        lua_pushvalue(L, -3);
        lua_rawset(L, -3);
        lua_pop(L, 1);
        //setmetatable(u,U);
        luaL_getmetatable(L, "M-U");
        lua_setmetatable(L, -2);
        //new setmetatable(tb,M);
        lua_createtable(L, 0, 0);
        luaL_getmetatable(L, "M");
        lua_setmetatable(L, -2);
        //wk[tb] = ud
        lua_GetWeakK(L);
        lua_pushvalue(L, -2); //k=tb
        lua_pushvalue(L, -4); //v=ud
        lua_rawset(L, -3);  //weakk[k]=v
        lua_pop(L, 1); //pop weakk top=tb
        //wv[u.ToInt32()] = tb
        lua_GetWeakV(L);
        lua_pushnumber(L, u.ToInt32()); //k=p
        lua_pushvalue(L, -3); //v=tb
        lua_rawset(L, -3);  //weakv[p]=tb
        lua_pop(L, 1); //pop weakv top=tb
        
        return 1;
    }
    static int lua_FindObjByTag(IntPtr L)
    {
        //string tag = lua_tostring(L, 1);
        //GameObject obj = GameObject.FindGameObjectWithTag(tag);
        //lua_getObj(L, obj);


        return 1;
    }
    static int lua_ObjGC(IntPtr L)
    {
        IntPtr u = lua_touserdata(L, 1);
        int i = Marshal.ReadInt32(u);
        IntPtr p = new IntPtr(i);
        GCHandle gch = GCHandle.FromIntPtr(p);
        gch.Free();
        return 0;
    }
    static int lua_SetOjbPos(IntPtr L)
    {
        IntPtr u = lua_touserdata(L, 1);
        int i = Marshal.ReadInt32(u);
        IntPtr p = new IntPtr(i);
        GCHandle gch = GCHandle.FromIntPtr(p);
        GameObject ojb = (GameObject)gch.Target;
        ojb.transform.position = new Vector3(0, 0, 0);
        return 0;
    }
    //--------------------------------------------------------------------------------------
    //test======================================================================================
    static int lua_test(IntPtr L)
    {
        //lua_pushinteger(L, 123);
        //lua_pushnumber(L, 123.343);
        //IntPtr s = (IntPtr)lua_newuserdata(L, sizeof(int));
        //Marshal.WriteInt32(s, 1234);
        //lua_pushnumber(L, Marshal.ReadInt32(s));

        string name = lua_tostring(L, 1);
        GameObject obj = GameObject.Find(name);
        if (obj == null) return 0;
        int oid = obj.GetInstanceID();
        //lua_pushinteger(L, oid);
        //lua_pushstring(L, obj.ToString());

        //GameObject prototype = Resources.Load("Capsule") as GameObject;

        //GameObject obj2 = Instantiate(prototype);
        //obj2.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y + 2, obj.transform.position.z);
        //Resources.UnloadUnusedAssets();
        lua_getglobal(L, "tostring");
        lua_createtable(L, 0, 0);
        lua_pushinteger(L, oid);
        GameObject obj1 = new GameObject();
        lua_pushinteger(L, obj1.GetInstanceID());
        TcpNet net1 = new TcpNet();
        lua_pushinteger(L, net1.GetInstanceID());
        UnityEngine.Object b = new UnityEngine.Object();
        lua_pushinteger(L, b.GetInstanceID());
        //lua_pcall(L, 1, 1, 0);
        //print(lua_gettop(L));
        //string s = lua_tostring(L, -1);
        //print(s);
        //lua_pushstring(L, s);
        return 4;
    }
    //======================================================================================
    // Use this for initialization
    void Start ()
    {
        print("LuaMgr.Start");
        if(_luaState==IntPtr.Zero)
            _luaState = luaL_newstate();
        IntPtr L = _luaState;
        luaL_openlibs(L);
        lua_init(L);
        NetMgr.luaopen_net(L);
        //
        lua_register(L, "print", lua_print);
        //object
        lua_createtable(L, 0, 3);
        lua_pushcsfunction(L, lua_FindObjByName);
        lua_setfield(L, -2, "Find");
        lua_pushcsfunction(L, lua_FindObjByTag);
        lua_setfield(L, -2, "FindByTag");
        lua_pushcsfunction(L, lua_SetOjbPos);
        lua_setfield(L, -2, "SetPos");
        lua_setglobal(L, "Object");
        //test
        lua_register(L, "_test", lua_test);
    
        //checktop
        int topn = lua_gettop(L);
        if (topn!=0)
        {
            Debug.LogError("[C#]Warning:lua_gettop()="+ topn + ". Maybe some init defective\n");
            lua_close(L);
            return;
        }
        int ret = luaL_dofile(L, "Assets/lua/launch.lua");
        if (ret != 0)  {
            print("[L]error:" + lua_tostring(L, -1));
            lua_close(L);
        }
    }
    // Update is called once per frame
    void Update ()
    {
        //print("LuaMgr.Update");
    }
    void OnApplicationQuit()
    {
        lua_close(_luaState);
        _luaState = IntPtr.Zero;
    }
}
