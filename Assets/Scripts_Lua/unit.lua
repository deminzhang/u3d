
_G.Unit = {}

local _meta = {__index=Unit}
local new = table.new
local acos = math.acos
local sqrt = math.sqrt
local insert = table.insert
local ARRIVE_DIS = 0.05	--位移差小于此值则到达

function Unit.new(tb)
	tb = tb or {}
	setmetatable(tb, _meta)
	return tb
end
_meta.__call = Unit.new

local tmpVec3_1 = _Vector3.new()
local tmpVec3_2 = _Vector3.new()
local tmpVec3_3 = _Vector3.new()
local function updatePos(self,e) --TODO 暂只算二维移动
	local tpos = self.toPos
	if not tpos then return end
	if self.noMove then self:stop() return end
	local old,dir,new = tmpVec3_1,tmpVec3_2,tmpVec3_3
	local pos = self.pos
	old.x = pos.x
	old.y = pos.y
	old.z = pos.z
	dir.x = tpos.x - old.x
	dir.y = tpos.y - old.y
	dir.z = tpos.z - old.z
	if dir.x==0 and dir.z==0 then --ignore NAN
		self:stop()
		return
	end
	local dis = dir:magnitude()
	local move = self.speed*e
	local r = acos(dir.x/sqrt(dir.x^2+dir.z^2))
	if r ~= r then return end -- rotation is NAN
	if self.type == 'role' then
		--onRoleMove{role=self}
	end
	if dis<move or dis<ARRIVE_DIS then
		self:setPos(tpos.x, tpos.y, tpos.z, r)
		self:stop()
	else
		dir:normalized()
		_Vector3.mul(dir, move, dir)
		_Vector3.add(old, dir, dir)
		self:setPos(dir.x, dir.y, dir.z, r)
	end
	assert(tpos.y,'X6')
end
Unit.updatePos = updatePos
function Unit:update(e, now)
	updatePos(self, e)
	
end
function Unit:runTo(x,y,z,speed)
	local pos = self.pos
	self.toPos = {x=x or pos.x,y=y or pos.y,z=z or pos.z}
	self.speed = speed or self.speed
end
function Unit:stop()
	self.toPos = nil
end
function Unit:setPos(x,y,z,r)
	--print('setPos',x,y,z)
	local pos = self.pos
	if x then pos.x = x end
	if y then pos.y = y end
	if z then pos.z = z end
	--if r then pos.r = r end
	local ava = self.avatar
	if not ava then return end
	ava.transform.position = pos
end
