print('>ui.lua')
assert(UI.new==nil, 'UI.new~=nil')
assert(UI.show==nil, 'UI.show~=nil')
assert(UI.isShow==nil, 'UI.isShow~=nil')
assert(UI.dirty==nil, 'UI.dirty~=nil')
assert(UI.dirtyAll==nil, 'UI.dirtyAll~=nil')
----------------------------------------------------------------
--tolocal
local clock = os.clock
local format = string.format
----------------------------------------------------------------
--config var
local DEFAULT_CACHE_TIME = 5000
local PREFAB_PATH = 'UI/'
----------------------------------------------------------------
--local var
local ui_base = {}
local ui_meta = {__index=ui_base}
local uis = {} 		--name:ui
local ui_onShow = {}--name:ui
local ui_cache = {}	--ui:timeTo
--local cache
local canvas = GameObject.Find("Canvas")
local uicache = GameObject.Find("Canvas/UICache")
if not uicache then
	uicache = GameObject()
	uicache.transform:SetParent(canvas.transform)
	uicache.name = "UICache";
end
----------------------------------------------------------------
--local functions
function ui_base:show(...)
	local oldShow = self._isShow
	if oldShow then
		if self.onShow then self:onShow(true,...) end
	else
		ui_onShow[self.name] = self
		self._isShow = true
		if self._ui then
			self._ui.transform:SetParent(canvas.transform)
			self._ui:SetActive(true)
		else
			local o = GameObject.Find('Canvas/'..self._name)
			if not o then
				local prefab = Resources.Load(PREFAB_PATH..self._name);
				--local prefab = Resources.Load(self.prefab);
				if prefab then
					o = GameObject.Instantiate(prefab,canvas.transform,false)
					o.name = self._name
				end
			end
			self._ui = o
			if self.onNew then self:onNew() end
		end
		if self.onShow then self:onShow(false,...) end
	end
end

function ui_base:hide(...)
	if not self._isShow then
		return
	end
	self._isShow = false
	ui_onShow[self.name] = nil
	if self.onHide then
		self:onHide()
	end
	if self._ui then
		self._ui:SetActive(false)
		if self.cache then
			self._ui.transform:SetParent(uicache.transform)
			ui_cache[self] = os.clock() + (self.cacheTime or DEFAULT_CACHE_TIME)/1000
		else
			GameObject.Destroy(self._ui)
			self._ui = nil
		end
	end
end

function ui_base:update()
	if self.onUpdate then
		self._dirty = false
		self:onUpdate()
	end
end

function ui_base:switch(show, ...)
	print('switch',self.name,show,...)
	local sw = type(show)
	assert(sw=='boolean' or sw=='nil', 'UI.show #2 require boolean or nil, got '..sw)
	if show==true then --just show
		self:show(...)
	elseif show==false then --just hide
		self:hide(...)
	else --if show then hide else show end
		if self._isShow then
			self:hide(...)
		else
			self:show(...)
		end
	end
	return self
end

function ui_base:getCom(childName,component)
	local o = self._ui.transform:Find(childName)
	if component then
		return o:GetComponent(component)
	end
	return o
end
function ui_base:getChild(name)
	return self._ui.transform:Find(name)
end

----------------------------------------------------------------
--functions
function UI.new(name)
	assert(not uis[name], name..' duplicated reg UI')
	local ui = {
		_isShow = false,
		_dirty = false,
		name = name,
		_name = 'UI'..name, 		--GameObject.Find("Canvas/'..ui._name)
		cache = false,
		cacheTime = DEFAULT_CACHE_TIME,
		-- onNew(self,...)
		-- onShow(self,oldShow,...)
		-- onHide(self,...)
		-- onUpdate(self)
	}
	uis[name] = setmetatable(ui,ui_meta)
	UI['show'..name] = function(show, ...)
		local ui = uis[name]
		assert(ui, 'invalid ui:'..tostring(name) )
		return ui:switch(show, ...)
	end
	return ui
end

function UI.isShow(name)
	return ui_onShow[name] and true or false
end

function UI.get(name)
	return uis[name]
end

function UI.update(name)
	local ui = ui_onShow[name]
	ui:update()
end

function UI.dirty(name)
	local ui = ui_onShow[name]
	if ui.onUpdate then ui._dirty = true end
end

function UI.dirtyAll()
	for _, ui in next, ui_onShow do
		if ui.onUpdate then ui._dirty = true end
	end
end

--commonUI
function UI.msgBoxSys(...)
	return UI.showMsgBox(true,...)
end

function UI.msgBoxOp(...)
	return UI.showMsgBox(true,...)
end

function UI.msgBoxForce(...)
	return UI.showMsgBox(true,...)
end
----------------------------------------------------------------
--events
local function ui_updatePerFrame()
	for _, ui in next, ui_onShow do
		if ui._dirty then
			ui:update()
			break --updateOneDirtyUIPerFrame
		end
	end
	local now = clock()
	for ui, t in next, ui_cache do
		if not ui._isShow and t < now then
			GameObject.Destroy(ui._ui)
			ui._ui = nil
			ui_cache[ui] = nil
		end
	end
end
function event.onUpdate()
	ui_updatePerFrame()
end
