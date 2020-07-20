
local ui = UI.new('MsgBox')
ui.cache = true			--是否缓存
ui.cacheTime = 5000		--缓存时间

function ui:onNew()
	local btnYes = GameObject.Find("Canvas/UIMsgBox/BtnYes"):GetComponent('Button')
	btnYes.onClick:AddListener(function()
		if self.onYes then
			self.onYes()
		end
		self:hide()
	end)
	local btnNo = GameObject.Find("Canvas/UIMsgBox/BtnNo"):GetComponent('Button')
	btnNo.onClick:AddListener(function()
		self:hide()
		if self.onNo then
			self.onNo()
		end
	end)
end

function ui:onShow(oldShow,title,text,labYes,labNo,onYes,onNo)
	local txt = GameObject.Find("Canvas/UIMsgBox/Title"):GetComponent('Text')
	txt.text = title or ''
	local txt = GameObject.Find("Canvas/UIMsgBox/Text"):GetComponent('Text')
	txt.text = text or ''
	local lab1 = GameObject.Find("Canvas/UIMsgBox/BtnYes/Text"):GetComponent('Text')
	lab1.text = labYes or 'Yes'
	local lab2 = GameObject.Find("Canvas/UIMsgBox/BtnNo/Text"):GetComponent('Text')
	lab2.text = labNo or 'No'
	self.onYes = onYes	
	self.onNo = onNo
end

function ui:onHide(...)
	print(self.name,'onHide')
end

function ui:onUpdate(...)
	print(self.name,'onUpdate')
end
