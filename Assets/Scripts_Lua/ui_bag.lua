
local ui = UI.new('Bag')
ui.cache = true			--是否缓存
ui.cacheTime = 5000		--缓存时间

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
