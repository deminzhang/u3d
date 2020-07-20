
local ui = UI.new('Skill')
ui.cache = false			--是否缓存

function ui:onNew()
	print(self.name,'onNew')
end

function ui:onShow()
	print(self.name,'onShow')
end

function ui:onHide(...)
	print(self.name,'onHide')
end

function ui:onUpdate(...)
	print(self.name,'onUpdate')
end