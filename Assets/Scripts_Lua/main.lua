import "UnityEngine"
assert(UnityEngine.GameObject,"Click Make/All to generate lua wrap file")
function info()
	if jit then
		print('lua version=',_VERSION, jit.version, jit.os, jit.arch)
	else
		print('lua version=',_VERSION)
	end
	print('__UNITY_VERSION=',Application.unityVersion)
	print('__ANDROID=',__ANDROID)
	print('__IOS=',__IOS)
	print('__EDITOR=',__EDITOR)
	print('__DEVELOPMENT_BUILD=',__DEVELOPMENT_BUILD)
	print("Application.persistentDataPath=",Application.persistentDataPath)
	print("Application.dataPath=",Application.dataPath)
end

function fix()
	math.randomseed(tostring(os.time()):reverse():sub(1, 6))
	-- slua的LuaValueType.cs 类表不需要元表是, New出来的对象才要元表
	--这里清掉不动slua
	setmetatable(Vector2,nil)
	setmetatable(Vector3,nil)
	setmetatable(Color,nil)
	setmetatable(Vector4,nil)
	setmetatable(Quaternion,nil)
	Vector3.up = Vector3.New(0,1,0)
	print("Input",UnityEngine.Input)
end
dofile'table'
dofile'debug'
dofile'base'
dofile'math'
dofile'string'
dofile'utils'
dofile'event'
--dofile'codec'
--dofile'net'
dofile'define'
dofile'timer' 
dofile'vector2' 
dofile'vector3' 
dofile'app'
-- dofile'scene'
dofile'ui'
dofile'ui_loading'
dofile'ui_login'
dofile'ui_msgbox'
dofile'ui_createrole'
dofile'ui_selectrole'
dofile'ui_chat'


function main()
	info()
	fix()
	print("lua.main")
	UI.showLoading(true,'UI/loading'):loadScene('login',function()
		UI.showLogin()
		UI.showLoading(false)
	end)
end

