
local ui = UI.new('Chat')
ui.cache = true			--是否缓存
ui.cacheTime = 180000	--缓存时间

function ui:onNew()
	local btn = GameObject.Find("Canvas/UIChat/BtnSend"):GetComponent('Button')
	btn.onClick:AddListener(function()
		local msg = GameObject.Find("Canvas/UIChat/InputChat"):GetComponent('InputField')
		local txt = msg.text
		print(txt)
		if txt=='' then return end
		if txt:lead'#' then --local dostring
			print('#',txt:sub(2,-1))
			local f = load(txt:sub(2,-1))
			if f then f() end
		elseif txt:lead'/' then --test command
			print('/',txt:sub(2,-1))
		else	--normal chat
			print('chat',txt)
		end
	end)
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
