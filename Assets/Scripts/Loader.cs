using UnityEngine;
using System;
using System.Collections;
using System.IO;
using SLua;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets; //Window->Package Manager 搜addressables install
using UnityEngine.ResourceManagement;

public class Loader : MonoBehaviour
{
    static LuaSvr l;
    public Text logText;
    public String luaPath = "/Scripts_Lua/";

    int progress = 0;

#if UNITY_IPHONE && !UNITY_EDITOR
	const string LUADLL = "__Internal";
#else
    const string LUADLL = "slua";
#endif
    static public LuaSvr luaSvr
    {
        get { return l; }
    }

    static LuaFunction
        _OnUpdate,
        _OnTouchBegin,
        _OnTouchMove,
        _OnTouchEnd,
        _OnTouchCancel,
        _OnKeyDown,
        _OnKeyUp,
        _OnMouseDown,
        _OnMouseUp,
        _OnMouseDrag,
        _OnMouseWheel;
    // Use this for initialization
    void Awake()
    {
        //准备日志文件
        //SetFontDirty(); //处理UI字体花屏

        //初始化对象保留
        GameObject[] InitGameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject go in InitGameObjects)
        {
            if (go.transform.parent == null)
                GameObject.DontDestroyOnLoad(go.transform.root);
        }
        //设置手机上不要自动锁屏
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 10000;
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.logMessageReceived += this.log;

        l = new LuaSvr();
        l.init(tick, complete, LuaSvrFlag.LSF_BASIC | LuaSvrFlag.LSF_EXTLIB);
    }

    // Update is called once per frame
    void Update()
    {
        if (_OnUpdate != null) 
         _OnUpdate.call(Time.deltaTime);
    }

    void OnGUI()
    {
        if (progress != 100)
        {
            //GUI.Label(new Rect(0, 15, 150, 50), string.Format("Load Scripts... {0}%", progress));
            GUI.Label(new Rect(Screen.width/2-100, Screen.height-30, 200, 50), string.Format("Load Scripts... {0}%", progress));
        }

        //LuaFunction func;
        //touch
        if (Input.touchSupported && Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                var t = Input.GetTouch(i);
                var pos = t.position;
                switch (t.phase)
                {
                    case TouchPhase.Began:
                        //_OnTouchBegin = (LuaFunction)l.luaState["_OnTouchBegin"];
                        if (_OnTouchBegin != null)
                            _OnTouchBegin.call(pos.x, pos.y, Input.touchCount, t.fingerId, t.pressure, t.maximumPossiblePressure);
                        break;
                    case TouchPhase.Stationary:
                    case TouchPhase.Moved:
                        //_OnTouchMove = (LuaFunction)l.luaState["_OnTouchMove"];
                        if (_OnTouchMove != null)
                            _OnTouchMove.call(pos.x, pos.y, Input.touchCount, t.fingerId, t.pressure, t.maximumPossiblePressure);
                        break;
                    case TouchPhase.Ended:
                        //_OnTouchEnd = (LuaFunction)l.luaState["_OnTouchEnd"];
                        if (_OnTouchEnd != null)
                            _OnTouchEnd.call(pos.x, pos.y, Input.touchCount, t.fingerId, t.pressure, t.maximumPossiblePressure);
                        break;
                    case TouchPhase.Canceled:
                        //_OnTouchCancel = (LuaFunction)l.luaState["_OnTouchCancel"];
                        if (_OnTouchCancel != null)
                            _OnTouchCancel.call(pos.x, pos.y, Input.touchCount, t.fingerId, t.pressure, t.maximumPossiblePressure);
                        break;
                }
            }
        }
        //mouse & key
        Event e = Event.current;
        switch (e.type)
        {
            case EventType.KeyDown:
                //_OnKeyDown = (LuaFunction)l.luaState["_OnKeyDown"];
                if (_OnKeyDown != null)
                    _OnKeyDown.call(e.keyCode, e.control, e.alt, e.shift);
                break;
            case EventType.KeyUp:
                //_OnKeyUp = (LuaFunction)l.luaState["_OnKeyUp"];
                if (_OnKeyUp != null)
                    _OnKeyUp.call(e.keyCode, e.control, e.alt, e.shift);
                break;
            case EventType.MouseDown:
                //_OnMouseDown = (LuaFunction)l.luaState["_OnMouseDown"];
                if (_OnMouseDown != null)
                {
                    Vector2 pos = e.mousePosition;
                    _OnMouseDown.call(e.button, pos.x, pos.y);
                }
                break;
            case EventType.MouseUp:
                //_OnMouseUp = (LuaFunction)l.luaState["_OnMouseUp"];
                if (_OnMouseUp != null)
                {
                    Vector2 pos = e.mousePosition;
                    _OnMouseUp.call(e.button, pos.x, pos.y);
                }
                break;
            case EventType.MouseDrag:
                //_OnMouseDrag = (LuaFunction)l.luaState["_OnMouseDrag"];
                if (_OnMouseDrag != null)
                {
                    Vector2 pos = e.mousePosition;
                    _OnMouseDrag.call(e.button, pos.x, pos.y);
                }
                break;
            case EventType.ScrollWheel:
                //_OnMouseWheel = (LuaFunction)l.luaState["_OnMouseWheel"];
                if (_OnMouseWheel != null)
                {
                    Vector2 pos = e.mousePosition;
                    _OnMouseWheel.call(e.delta.x, e.delta.y);
                }
                break;
            default:
                break;
        }
    }


    void log(string cond, string trace, LogType lt)
    {
        if (logText == null) return;
        string txt = logText.text + (cond + "\n");
        if (txt.Length > 1000)
            txt = txt.Substring(txt.Length - 1000);
        logText.text = txt;
    }
    void tick(int p)
    {
        progress = p;
        //UILoading.SetProgress(p);
        //加载脚本进度
    }
    void complete()
    {
        //宏 http://blog.csdn.net/yjh4866/article/details/25550739
        LuaState luaState = LuaSvr.mainState;
        //C->lua 给lua的一些全局变量
#if UNITY_ANDROID
        luaState["__ANDROID"] = true;
#else
        luaState["__ANDROID"] = false;
#endif
#if (UNITY_IOS || UNITY_IPHONE)
        luaState["__IOS"] = true;
#else
        luaState["__IOS"] = false;
#endif
#if UNITY_EDITOR
        luaState["__EDITOR"] = true;
#else
        luaState["__EDITOR"] = false;
#endif
#if !UNITY_EDITOR && DEVELOPMENT_BUILD
        luaState["__DEVELOPMENT_BUILD"] = true;
#else
        luaState["__DEVELOPMENT_BUILD"] = false;
#endif
        //使用本地lua 加LuaPath
        luaState.loaderDelegate += LuaLoader;
        //startlua main
        l.start("main");
        ////lua->C 给C调的一些接口
        _OnUpdate = (LuaFunction)luaState["_OnUpdate"];
        _OnTouchBegin = (LuaFunction)luaState["_OnTouchBegin"];
        _OnTouchMove = (LuaFunction)luaState["_OnTouchMove"];
        _OnTouchEnd = (LuaFunction)luaState["_OnTouchEnd"];
        _OnTouchCancel = (LuaFunction)luaState["_OnTouchCancel"];
        _OnKeyDown = (LuaFunction)luaState["_OnKeyDown"];
        _OnKeyUp = (LuaFunction)luaState["_OnKeyUp"];
        _OnMouseDown = (LuaFunction)luaState["_OnMouseDown"];
        _OnMouseUp = (LuaFunction)luaState["_OnMouseUp"];
        _OnMouseDrag = (LuaFunction)luaState["_OnMouseDrag"];
        _OnMouseWheel = (LuaFunction)luaState["_OnMouseWheel"];

        Debug.Log("scripts load complete");
    }

    private byte[] LuaLoader(string fn, ref string absoluteFn)
    {
        string newFn = fn.Replace(".", "/");
        byte[] data = null;
        string path = Application.persistentDataPath + luaPath + newFn + ".lua";
        if (File.Exists(path))
        {
            data = File.ReadAllBytes(path);
        }
        if (data == null)
        {
            path = Application.dataPath + luaPath + newFn + ".lua";
            if (File.Exists(path))
            {
                data = File.ReadAllBytes(path);
            }
        }
        if (data == null)
        {
            TextAsset txt = Resources.Load<TextAsset>(newFn);
        }
            
        return data;
    }

    /****************************************************************************/
    void LoadSceneAsync(string name, LuaFunction onTick, LuaFunction onComplete)
    {
        StartCoroutine(I_loadSceneAsync(name, onTick, onComplete));
    }
    IEnumerator I_loadSceneAsync(string name, LuaFunction onTick, LuaFunction onComplete)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(name);
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            if (async.progress >= 0.9f) //记得要f
            {
                if (Input.anyKeyDown) //最后10% 按任意键继续
                    async.allowSceneActivation = true;
            }
            onTick.call(async.progress);
            yield return null;
        }
        onComplete.call();
    }

}
