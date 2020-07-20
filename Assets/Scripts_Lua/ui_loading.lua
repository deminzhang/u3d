
local ui = UI.new('Loading')
ui.cache = true			--是否缓存
ui.cacheTime = math.huge--缓存时间

function ui:onNew()
	print(self.name,'onNew')
	local canvas = self._ui.transform:GetComponent('Canvas')
	canvas.overrideSorting = true;
	canvas.sortingOrder = 999;
	self:setProgress(0)
end

function ui:onShow(old, backGround)
	local _ui = self._ui
	print(self.name,'onShow')
	if backGround then
		self:setBackGround(backGround)
	end
	self:setProgress(1)
end

function ui:setBackGround(value)
	self:getCom('BackGround','Image').sprite = Resources.Load(value, Sprite)
end

function ui:setProgress(value,txt)
	self:getCom('Slider','Slider').value = value;
	if txt then
		self:getCom('Text','Text').text = txt
	else
		self:getCom('Text','Text').text = string.format('Loading %s%%', value);
	end
end

function ui:loadScene(senName,callBack)
	-- _Coroutine(function()
		-- local SceneManager = SceneManagement.SceneManager
		-- SceneManager.LoadScene("loading");
		-- local async = SceneManager.LoadSceneAsync(senName);
		-- async.allowSceneActivation = false
		-- self:setProgress(async.progress*100)
		-- while not async.isDone do
			-- if async.progress>0.89999 then
				-- async.allowSceneActivation = true
			-- end
			-- self:setProgress(async.progress*100)
			-- coroutine.yield()
		-- end
		-- self:setProgress(100)
		-- if callBack then callBack() end
	-- end)
	GameObject.Find('Main Camera'):GetComponent('loader'):LoadSceneAsync(senName,function(e)
		if e>=0.89999 then
			self:setProgress(100,'any key to continue')
		else 
			self:setProgress(e*100)
		end
	end,callBack)
end
