using UnityEngine;  
using System.Net.Sockets;  
using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class TcpNet : UnityEngine.Object
{
    enum States { CONN, RECV, CLOSE };
    private TcpClient net = new TcpClient();
    //private int len = 0;
    private long timeout = 0;
    private States state = States.CONN;
    public bool decode;     //recv push to lua
    private int lisnconnref = 0;//Listener在没close前/异步connect回调前 防回收的强引用
    public string[] seps = new string[4];
    public int userdatapk;
    public TcpClient Net {
        get { return net; }
        //set { net = value; }
    }
    //public int WaitLen {
    //    get { return len; }
    //    set { len = value; }
    //}
    public long TimeOut {
        get { return timeout; }
        set { timeout = value; }
    }
    public int ConnRef  {
        get { return lisnconnref; }
        set { lisnconnref = value; }
    }
    public void Close()  {
        state = States.CLOSE;
        net.Close();
    }
    public bool Closed() {
        return state == States.CLOSE;
    }
}

public class NetMgr : MonoBehaviour
{
    static List<TcpNet> nets = new List<TcpNet>(3);
    const int BUFFSIZE = 4 * 1024 * 1024;
    static byte[] buffer = new byte[BUFFSIZE];
    private static string META_NAME = "[NET]";
    private static string META_NAME_U = "[NET-U]";
    void Update()
    {
    }
    void FixedUpdate()
    {
        foreach (TcpNet net in nets)
        {
            if (!net.Net.Connected && net.TimeOut > DateTime.Now.Ticks)
            {
                //timeout

            }
        }
    }
    //userdefine//////////////////////////////////////////////////////////////////////////
    public static NetMgr GetNetMgr()
    {
        GameObject obj = GameObject.Find("Manager");
        NetMgr netmgr = obj.GetComponent<NetMgr>();
        if (netmgr == null)
            netmgr = obj.AddComponent<NetMgr>();
        return netmgr;
    }
    //statics//////////////////////////////////////////////////////////////////////////
    private static void onHead(IAsyncResult ar)
    {
        IntPtr L = LuaMgr.LuaState;
        try
        {
            TcpClient net = (TcpClient)ar.AsyncState;
            int bytesRead;
            bytesRead = net.GetStream().EndRead(ar);
            if (bytesRead < 1)
            {
                Debug.Log("onHead error len：" + bytesRead);
                return;
            }
            else
            {
                Debug.Log("onHead:" + bytesRead + "<<" + System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead) + ">>");
                string ss = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                byte[] bt = Encoding.UTF8.GetBytes(ss);
                Debug.Log("onHeadtoInt:" + System.BitConverter.ToInt32(bt, 0));


                net.GetStream().BeginRead(buffer, 0, System.BitConverter.ToInt32(bt, 0), onBody, net);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("catch onHead error：" + ex.Message);
        }
    }
    private static void onBody(IAsyncResult ar)
    {
        IntPtr L = LuaMgr.LuaState;
        try
        {
            TcpClient net = (TcpClient)ar.AsyncState;
            int bytesRead;
            bytesRead = net.GetStream().EndRead(ar);
            if (bytesRead < 1)
            {
                Debug.Log("onBody error len：" + bytesRead);
                return;
            }
            else
            {
                Debug.Log("onBody:" + bytesRead + "<<" + System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead) + ">>");

                //PushCallInToQueue

            }
            net.GetStream().BeginRead(buffer, 0, 4, onHead, net);
        }
        catch (Exception ex)
        {
            Debug.Log("服务端产生异常3：" + ex.Message);
        }
    }
    private static void onConnnect(IAsyncResult ar) //callback not in main thread
    {
        IntPtr L = LuaMgr.LuaState;
        TcpNet net = (TcpNet)ar.AsyncState;
        TcpClient clnt = net.Net;
        if (net.ConnRef != 0) {
            LuaMgr.lua_unref(L, net.ConnRef); //del forceref
            net.ConnRef = 0;
        }
        //Debug.Log("onConn sockname = " + clnt.Client.LocalEndPoint + ". peername = " + clnt.Client.RemoteEndPoint);
        //clnt.GetStream().BeginRead(buffer, 0, 4, onHead, clnt);
        LuaMgr.lua_GetWeakV(L);
        LuaMgr.lua_pushnumber(L, net.userdatapk);
        LuaMgr.lua_rawget(L, -2);
        if (!LuaMgr.lua_istable(L, -1))
        {
            Debug.Log("not table net_onconn");
            return;
        }
        LuaMgr.lua_getfield(L, -1, "__onConnect");
        if (!LuaMgr.lua_isfunction(L, -1))
        {
            Debug.Log("__onConnect not func");
            return;
        }
        LuaMgr.lua_pushvalue(L, -2);
        string[] array = clnt.Client.RemoteEndPoint.ToString().Split(':');
        string ip = array[0];
        int port = int.Parse(array[1]);
        LuaMgr.lua_pushstring(L, ip);
        LuaMgr.lua_pushnumber(L, port);
        array = clnt.Client.LocalEndPoint.ToString().Split(':');
        ip = array[0];
        port = int.Parse(array[1]);
        LuaMgr.lua_pushstring(L, ip);
        LuaMgr.lua_pushnumber(L, port);
        ////beforecall
        if (LuaMgr.lua_pcall(L, 5, 0, 0) != 0) {
            //errorCall
            Debug.Log("onConnectErr" + LuaMgr.lua_tostring(L, -1));
        }
        ////aftercall
    }
    private static void onReceive(IAsyncResult ar)
    {
        IntPtr L = LuaMgr.LuaState;
        TcpNet net = (TcpNet)ar.AsyncState;
        TcpClient clnt = net.Net;
        string recv;
        int bytesRead = clnt.GetStream().EndRead(ar);
        if (bytesRead < 1)
        {
            Debug.Log("onBody error len：" + bytesRead);
            return;
        }
        else
        {
            //Debug.Log("onBody:" + bytesRead + "<<" + Encoding.UTF8.GetString(buffer, 0, bytesRead) + ">>");
            recv = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }

        LuaMgr.lua_GetWeakV(L);
        LuaMgr.lua_pushnumber(L, net.userdatapk);
        LuaMgr.lua_rawget(L, -2);
        if (!LuaMgr.lua_istable(L, -1))
        {
            Debug.Log("not table onReceive");
            return;
        }
        LuaMgr.lua_getfield(L, -1, "__onReceive");
        if (!LuaMgr.lua_isfunction(L, -1))
        {
            Debug.Log("__onReceive not func");
            return;
        }
        LuaMgr.lua_pushvalue(L, -2);
        LuaMgr.lua_pushstring(L, recv);
        ////beforecall
        if (LuaMgr.lua_pcall(L, 2, 0, 0) != 0)
        {
            //errorCall
            Debug.Log("onReceiveErr" + LuaMgr.lua_tostring(L, -1));
        }
        ////aftercall
    }
    public static int luanet_gc(IntPtr L)
    {
        IntPtr u = LuaMgr.lua_touserdata(L, 1);
        GCHandle gch = GCHandle.FromIntPtr(new IntPtr(Marshal.ReadInt32(u)));
        TcpNet net = (TcpNet)gch.Target;

        if (net.ConnRef != 0)  {
            LuaMgr.lua_unref(L, net.ConnRef); //del forceref
            net.ConnRef = 0;
        }
        net.Net.Close();
        nets.Remove(net);
        gch.Free();

        int p = LuaMgr.lua_topointer(L, 1).ToInt32();
        LuaMgr.lua_GetWeakV(L);
        LuaMgr.lua_pushnumber(L, p);
        LuaMgr.lua_rawget(L, -2);
        if (LuaMgr.lua_istable(L, -1))
        {
            LuaMgr.lua_getfield(L, -1, "__onClose");
            if (LuaMgr.lua_isfunction(L, -1))
            { //onClose(net, resaon, errcode)
                LuaMgr.lua_pushvalue(L, -2);
                LuaMgr.lua_pushstring(L, "collect");
                //LuaMgr.lua_pushnumber(L, code);
                //beforecall
                if (LuaMgr.lua_pcall(L, 2, 0, 0) != 0)
                {
                    //errorCall
                    LuaMgr.luaL_where(L, 1);
                    LuaMgr.lua_concat(L, 2);
                    Debug.Log( "err:" + LuaMgr.lua_tostring(L, -1));
                    //lua_errorEx(L, "%s\n", lua_tostring(L, -1)); //崩
                }
                //aftercall
            }
            else
                LuaMgr.lua_errorEx(L, "onClose missing");

        }
        //if (net->state == NETCLOSE) return 0;
        //net_close(L, net, "collect", 0);
        //if (net->state == NETLISN || net->state == NETSHARE)
        //    fprintf(stderr, "listener was __gc without closed");

        return 0;
    }
    public static int luanet_close(IntPtr L)
    {
        LuaMgr.lua_GetWeakK(L);
        LuaMgr.lua_pushvalue(L, - 1); //tb
        LuaMgr.lua_rawget(L, -2);
        IntPtr u = LuaMgr.luaL_checkudata(L, -1, META_NAME_U);
        GCHandle gch = GCHandle.FromIntPtr(new IntPtr(Marshal.ReadInt32(u)));
        TcpNet net = (TcpNet)gch.Target;
        net.Close();
        return 1;
    }
    public static int luanet_closed(IntPtr L)
    {
        LuaMgr.lua_GetWeakK(L);
        LuaMgr.lua_pushvalue(L, -1); //tb
        LuaMgr.lua_rawget(L, -2);
        IntPtr u = LuaMgr.luaL_checkudata(L, -1, META_NAME_U);
        GCHandle gch = GCHandle.FromIntPtr(new IntPtr(Marshal.ReadInt32(u)));
        TcpNet net = (TcpNet)gch.Target;
        LuaMgr.lua_pushboolean(L, net.Closed());
        return 1;
    }
    public static int luanet_setreceive(IntPtr L)
    {
        //string [4] seps = new string[4];
        int i = 2;
        for (i = 2; i < 6; i++)
        {
            if (LuaMgr.lua_isstr(L, i))
            {
                //seps[i - 2] = LuaMgr.lua_tolstring(L, i, out n); //TODO暂不支持带\0字符
            }
            else
            {
                //seps[i - 2] = NULL;
                break;
            }
        }
        if (LuaMgr.lua_isstr(L, i))
        {
            LuaMgr.lua_errorEx(L, "receive:too many separators");
            return 0;
        }
        int len = LuaMgr.lua_tointeger(L, i);
        if (len < 1)
        {
            LuaMgr.lua_errorEx(L, "#%d must be 1<integer<1048576", i);
            return 0;
        }
        int funcidx;
        if (!LuaMgr.lua_isfunction(L, i + 1))
        {
            LuaMgr.lua_errorEx(L, "#%d must be a function onReceive", i + 1);
            return 0;
        }
        else funcidx = i + 1;
        int tm = (int)LuaMgr.lua_tonumber(L, i + 2);
        bool tostr = LuaMgr.lua_tobool(L, i + 3);

        LuaMgr.lua_GetWeakK(L);
        LuaMgr.lua_pushvalue(L, 1); //tb
        LuaMgr.lua_rawget(L, -2);
        IntPtr u = LuaMgr.luaL_checkudata(L, -1, META_NAME_U);
        GCHandle gch = GCHandle.FromIntPtr(new IntPtr(Marshal.ReadInt32(u)));
        TcpNet net = (TcpNet)gch.Target;
        if (net.Closed()) return 0;

        //for (i = 0; i < 4; i++)
        //    if (net->seps[i] != seps[i])
        //        if (net->seps[i])
        //        { //old
        //            LuaMgr.lua_unref(L, net->sepsref[i]);
        //            net->sepsref[i] = 0;
        //            if (seps[i])
        //            { //new
        //                net->seps[i] = seps[i];
        //                net->sepsref[i] = LuaMgr.lua_refi(L, i + 2);
        //            }
        //            else
        //                net->seps[i] = NULL;
        //        }
        //        else
        //            if (seps[i])
        //        {
        //            net->seps[i] = seps[i];
        //            net->sepsref[i] = LuaMgr.lua_refi(L, i + 2);
        //        }
        
        net.TimeOut = DateTime.Now.Ticks + (tm < 1 ? 1 : tm > 86400 ? 86400 : tm);
        net.decode = tostr;
        LuaMgr.lua_pushvalue(L, 1);
        LuaMgr.lua_pushvalue(L, funcidx);
        LuaMgr.lua_setfield(L, -2, "__onReceive");
        net.Net.GetStream().BeginRead(buffer, 0, len, onReceive, net);
        
        return 0;
    }
    public static int luanet_send(IntPtr L)
    {
        string msg = LuaMgr.lua_tostring(L, 2);
        LuaMgr.lua_GetWeakK(L);
        LuaMgr.lua_pushvalue(L, -1); //tb
        LuaMgr.lua_rawget(L, -2);
        IntPtr u = LuaMgr.luaL_checkudata(L, -1, META_NAME_U);
        GCHandle gch = GCHandle.FromIntPtr(new IntPtr(Marshal.ReadInt32(u)));
        TcpNet net = (TcpNet)gch.Target;
        try
        {
            NetworkStream stream = net.Net.GetStream();//获得客户端的流  
            byte[] buffer = Encoding.UTF8.GetBytes(msg);//将字符串转化为二进制  //Unicode
            byte[] len = BitConverter.GetBytes(buffer.Length);
            stream.Write(len, 0, 4);//将转换好的二进制数据写入流中并发送  
            stream.Write(buffer, 0, buffer.Length);//将转换好的二进制数据写入流中并发送
            //stream.Flush();
            Debug.Log("send：" + buffer.Length + "|" + msg.Length);
            return 0;
        }
        catch (Exception ex)
        {
            Debug.Log("catch Send error：" + ex.Message);
            return 1;
        }
    }
    public static int luanet_setnodelay(IntPtr L)
    {
        int b = LuaMgr.lua_toboolean(L, 2);
        LuaMgr.lua_GetWeakK(L);
        LuaMgr.lua_pushvalue(L, -1); //tb
        LuaMgr.lua_rawget(L, -2);
        IntPtr u = LuaMgr.luaL_checkudata(L, -1, META_NAME_U);
        GCHandle gch = GCHandle.FromIntPtr(new IntPtr(Marshal.ReadInt32(u)));
        TcpNet net = (TcpNet)gch.Target;
        net.Net.NoDelay = b == 1;
        return 0;
    }
    public static int lua_connect(IntPtr L) {
        string addr = LuaMgr.lua_tostring(L, 1);
        string[] array = addr.Split(':');
        if (array.Length != 2)
        {
            Debug.Log("error hostaddr:port：" + addr);
            return 0;
        }
        string hostname = array[0];
        int port = int.Parse(array[1]);
        if (!LuaMgr.lua_isfunction(L, 2))
        {
            Debug.Assert(false, "#2 must be a function onConnect");
            return 0;
        }
        if (!LuaMgr.lua_isfunction(L, 3))
        {
            Debug.Assert(false, "#3 must be a function onClose");
            return 0;
        }
        int tm = (int)LuaMgr.lua_tonumber(L, 4);
        
        TcpNet net = new TcpNet();
        net.Net.ReceiveBufferSize = BUFFSIZE;
        nets.Add(net);
        net.TimeOut = DateTime.Now.Ticks + (tm < 1 ? 1 : tm > 86400 ? 86400 : tm);
        IntPtr u = LuaMgr.lua_newuserdata(L, sizeof(int));
        Marshal.WriteInt32(u, GCHandle.ToIntPtr(GCHandle.Alloc(net)).ToInt32());
        
        LuaMgr.lua_HideUserdata(L, META_NAME, META_NAME_U);
        LuaMgr.lua_pushvalue(L, 2);
        LuaMgr.lua_setfield(L, -2, "__onConnect");
        LuaMgr.lua_pushvalue(L, 3);
        LuaMgr.lua_setfield(L, -2, "__onClose");
        LuaMgr.lua_pushvalue(L, -1);
        net.ConnRef = LuaMgr.lua_ref(L, -1); //force ref while connecting
        
        net.userdatapk = u.ToInt32();
        try
        {
            //IAsyncResult ar = 
                net.Net.BeginConnect(hostname, port, onConnnect, net); //异步
        }
        catch (Exception ex)
        {
            Debug.Log("BeginConnect Fail：" + ex.Message);
            //onclose
            return 0;
        }

        return 1;
    }

    public static void luaopen_net(IntPtr L)
    {
        LuaMgr.luaL_Reg[] methods0 = {
            new LuaMgr.luaL_Reg("__gc", luanet_gc),
        };
        LuaMgr.lua_RegMetaU(L, META_NAME_U, methods0);
        LuaMgr.luaL_Reg[] methods = {
            new LuaMgr.luaL_Reg("close", luanet_close),
            new LuaMgr.luaL_Reg("closed", luanet_closed),
            new LuaMgr.luaL_Reg("receive", luanet_setreceive),
            new LuaMgr.luaL_Reg("send", luanet_send),
            new LuaMgr.luaL_Reg("nagle", luanet_setnodelay),
        };
        LuaMgr.lua_RegClass(L, META_NAME, methods);

        LuaMgr.lua_register(L, "_connect", lua_connect);

    }
}  