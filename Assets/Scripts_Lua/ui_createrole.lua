
local ui = UI.new('CreateRole')
ui.cache = false			--是否缓存

function ui:onNew()
	self.job = 1
	self.gender = 2
	self.name = ''
	
	local txt = self:getCom('BtnJob1/Text','Text')
	txt.text = _T'职业1'
	local txt = self:getCom('BtnJob2/Text','Text')
	txt.text = _T'职业2'
	local txt = self:getCom('BtnJob3/Text','Text')
	txt.text = _T'职业3'
	local txt = self:getCom('BtnMale/Text','Text')
	txt.text = _T'男'
	local txt = self:getCom('BtnFemale/Text','Text')
	txt.text = _T'女'
	local txt = self:getCom('BtnCreate/Text','Text')
	txt.text = _T'创建'
	local txt = self:getCom('BtnRandom/Text','Text')
	txt.text = '随'
	local txt = self:getCom('BtnEsc/Text','Text')
	txt.text = '返回'

	local btn = self:getCom('BtnJob1','Button')
	btn.onClick:AddListener(function()
		self.job = 1
	end)
	local btn = self:getCom('BtnJob2','Button')
	btn.onClick:AddListener(function()
		self.job = 2
	end)
	local btn = self:getCom('BtnJob3','Button')
	btn.onClick:AddListener(function()
		self.job = 3
	end)
	
	local btn = self:getCom('BtnMale','Button')
	btn.onClick:AddListener(function()
		self.gender = 1
	end)
	local btn = self:getCom('BtnFemale','Button')
	btn.onClick:AddListener(function()
		self.gender = 2
	end)
	
	local input = self:getCom('InputField','InputField')
	input.onValueChanged:AddListener(function(val)
		print('onValueChanged',val)
	end)
	-- input.onEndEdit:AddListener(function(val)
		-- self.name = val
	-- end)
	
	local btn = self:getCom('BtnCreate','Button')
	btn.onClick:AddListener(function()
		local input = self:getCom('InputField','InputField')
		if input.text=='' then
			input:ActivateInputField()
			return
		end
		CS.CreateRole{Job=self.job,Gender=self.gender,
			Name=input.text,Face=0,Hair=0,Body=0}
	end)
	local btn = self:getCom('BtnRandom','Button')
	btn.onClick:AddListener(function()
		self:randomName()
	end)
	local btn = self:getCom('BtnEsc','Button')
	btn.onClick:AddListener(function()
		UI.showSelectRole(true)
	end)
	
end

function ui:onShow(old,rolelist)
	UI.showSelectRole(false)
	local input = self:getCom('InputField','InputField')
	input:ActivateInputField()
	self:randomName()
	local btn = self:getCom('BtnEsc')
	btn:SetActive(rolelist and #rolelist>0 or false)
end

function ui:randomName(...)
	local input = self:getCom('InputField','InputField')
	input.text = 'player'..math.random(9999)
end
