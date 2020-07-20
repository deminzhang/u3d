
_G.CurSen = nil	--主场景
_G.Me = nil 	--主角数据
_G.MeRole = nil --主角对象
----------------------------------------------------------------
--tolocal
local pairs = pairs
local _decode = _decode
local strRep = string.rep
----------------------------------------------------------------
--config temp
_G.cfg_zone = {
	[1] = {id=1, type='normal', sen='sen2'	},
	[2] = {id=2, type='normal', sen='sen3'	},
	[3] = {id=3, type='private',sen='sen2'	},
	[4] = {id=4, type='dungeon', sen='sen3'	},
	[5] = {id=5, type='battle', sen='sen2'	},
}
----------------------------------------------------------------
--local
local AllUnitswk = table.weakv(0,200)
local _obj_pool = table.weakv(0,200) --TODO对象池以后再说
local function freeAvatar(unit)
	--_obj_pool[unit.guid] = unit.avatar 
	GameObject.Destroy(unit.avatar)
	unit.avatar = nil
end
local _pool_hit = 0
local function getAvatar(unit)
	local uobj = _obj_pool[unit.guid]
	-- if uobj then
		-- _pool_hit = _pool_hit + 1
		-- print('_pool_hit', _pool_hit)
	-- else
		uobj = GameObject.CreatePrimitive(PrimitiveType.Capsule)
	--end
	return uobj
end
----------------------------------------------------------------
_G.AllUnits = {}
_G.UnitNum = 0
function FreeAllUnit()
	for guid, u in pairs(AllUnits) do
		AllUnits[guid] = nil
		freeAvatar(u)
	end
	_G.UnitNum = 0
end
----------------------------------------------------------------
--event
function event.onUpdate(e)
	-- print('D',Input.GetKeyDown(KeyCode.W))
	-- print('P',Input.GetKey(KeyCode.W))
	for guid, u in pairs(AllUnits)do
		u:update(e)
	end
	-- print('T',GameObject.Find("Terrain"))
end

function event.onSecond()
	if _G.MeRole then
		local o = _G.MeRole.avatar.transform:Find('NameBoard/TitleFont')
		local o = o.gameObject
		o:GetComponent('TextMesh').text = _G.UnitNum
	end
end
----------------------------------------------------------------
--RPC
define.EnterZone{Id=0,Time=0,Role={},Units={},Zone={}}
define.AddUnit{T={},L=''}
define.DelUnit{Guid=0,L=''}
define.AddUnits{List={},L=''}
define.DelUnits{List={},L=''}
define.DelZoneUnits{Zid=0}
define.Run{Guid=0,X=0,Y=0,FX=0,FY=0,Speed=0}
define.Stop{Guid=0, X=0, Y=0}
define.JumpTo{Guid=0, X=0, Y=0,Z=0, R=0}

function event.EnterZone(Id,Time,Role,Units,Zone)
	print('>>EnterZone',Id)
	UI.showSelectRole(false)
	local onloaded = function(sceneName,sen)
		print('onLoadScene', sceneName)
		--dump(Units)
		--clearUpOldZone
		for guid, u in pairs(AllUnits) do
			AllUnits[guid] = nil
			freeAvatar(u)
		end
		_G.UnitNum = 0
		--setZone
		_G.CurSenID = Id
		_G.CurSen = sen
		--createRole
		_G.Me = Role
		--createUnits
		AddUnits{List=Units}
		
		UI.showLoading(false)
		UI.showChat(true)
		GS.EnterDone{}
	end
	
	local sceneName = cfg_zone[Id].sen
	if CurSenID then
		local oldsen = cfg_zone[CurSenID].sen
		if sceneName == oldsen then
			onloaded(sceneName, CurSen)
			return
		end
	end
	UI.showLoading(true,'UI/loading',sceneName,onloaded)
end

local root = GameObject.Find("root").transform
local nameboard = Resources.Load("Avatar/NameBoard")
function event.AddUnit(T)
	local u = T
	for gk,gv in pairs(u) do
		--后端接变化频率分组编码的数据,前端重组展开
		if type(gv)=='userdata' then
			u[gk] = nil
			for k,v in pairs(_decode(gv)) do
				u[k] = v
			end
		end
	end
	--dump(u)
	Unit.new(u)
	local guid = u.guid
	--print('AddUnit',guid)
	if AllUnits[guid] then
		print('ERR:repeat AllUnits',guid)
		return
	end
	local pos = u.pos
	local uobj = getAvatar(u)
	--uobj:AddComponent(Rigidbody)
	local mat = uobj.transform
	uobj.name = 'unit'..guid
	mat:SetParent(root)
	--hpBar
	local titleMat = GameObject.Instantiate(nameboard).transform
	titleMat:SetParent(mat)
	titleMat.name = 'NameBoard'
	local o = titleMat:Find('NameFont').gameObject
	o:GetComponent('TextMesh').text = u.name
	local o = titleMat:Find('TitleFont').gameObject
	o:GetComponent('TextMesh').text = u.type..'|'..guid
	local o = titleMat:Find('GuildFont').gameObject

	local per = math.round(u.hp*10/u.maxHp)
	o:GetComponent('TextMesh').text = strRep('=',per)..strRep('-',10-per)
	
	local h = 0.8 --terrain.getHeight(pos.x,pos.z)
	if u.toPos then u.toPos.y = h end
	local pos = Vector3.New(pos.x,h,pos.z)
	u.pos = pos
	mat.position = pos
	u.avatar = uobj
	AllUnits[guid] = u
	AllUnitswk[guid] = u
	if Me.guid == guid then
		_G.MeRole = u
	end
	_G.UnitNum = _G.UnitNum +1
end
function event.AddUnits(List,L)
	--print('AddUnits',L)
	--dump(List)
	for _, u in pairs(List) do --_为方便server不一定是guid, guid从u中取
		AddUnit{T=u}
	end
end

function event.DelUnit(Guid)
	local u = AllUnits[Guid]
	--print('DelUnit',guid,u)
	if u then
		AllUnits[Guid] = nil
		freeAvatar(u)
		_G.UnitNum = _G.UnitNum -1
	end
end
function event.DelUnits(List,L)
	-- print('DelUnits',L)
	-- dump(List)
	for _, guid in pairs(List) do
		DelUnits{Guid=guid}
	end
end

function event.DelZoneUnits(Zid)
	if CurSenID~=Zid then return end
	for guid, u in pairs(AllUnits) do
		if Me.guid~=guid then
			DelUnits{Guid=guid}
		end
	end
end

function event.Run(Guid,X,Y,Z,FX,FY,FZ,Speed)
	--print('Run',Guid,X,Y,Z,FX,Fy,FZ,Speed)
	if Me.guid == Guid then
		-- if 当然位置过差于(FX,FZ) then
			-- 拉回()
		-- end
		print('RunMe')
		return
	end
	local u = AllUnits[Guid]
	if not u then return end
	-- u.toPos = {x=X,y=u.pos.y,z=Z}
	-- u.speed = Speed
	u:runTo(X,u.pos.y,Z,Speed)
end

function event.Stop(Guid,X,Y,Z)
	local u = AllUnits[Guid]
	--print('Stop',Guid,X,Y,Z,u)
	if not u then return end
	local pos = u.pos
	u:stop()
	u:setPos(X,pos.y,Z,r)
end

function event.JumpTo(Guid,X,Y,Z,R)
	local u = AllUnits[Guid]
	if not u then return end
	local pos = u.pos
	u:setPos(X,pos.y,Z,r)
end
