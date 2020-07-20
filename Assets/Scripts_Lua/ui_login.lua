
local ui = UI.new('Login')
ui.cache = false			--是否缓存

local _host,_user,_pass
function ui:onNew()
	local host = GameObject.Find("Canvas/UILogin/InputHost"):GetComponent('InputField')
	host.text = 'localhost:8341'
	local btn = GameObject.Find("Canvas/UILogin/Button"):GetComponent('Button')
	btn.onClick:AddListener(function()
		local user = GameObject.Find("Canvas/UILogin/InputUser"):GetComponent('InputField')
		local pass = GameObject.Find("Canvas/UILogin/InputPass"):GetComponent('InputField')
		if user.text=='' then
			user:ActivateInputField();
		elseif pass.text=='' then
			pass:ActivateInputField();
		else
			_user,_pass = user.text,pass.text
			CS.connect(host.text, _user,_pass)
		end
	end)
end

function ui:onShow()
	local user = GameObject.Find("Canvas/UILogin/InputUser"):GetComponent('InputField')
	if user.text=='' then
		user:ActivateInputField()
	end
end

define.RoleList{Data={}}
function event.RoleList(Data)
	UI.showLogin(false)
	if #Data==0 then
		UI.showCreateRole(true)
	else
		UI.showSelectRole(true,Data)
	end
end

define.LoginDuplicate{}
function event.LoginDuplicate()
	UI.msgBoxSys('','帐号在游戏中,是否强制踢出?','是','否',
		function()
			CS.connect(_host,_user,_pass,true)
		end,
		function()
		end)
end

define.LoginKick{}
function event.LoginKick()
	UI.msgBoxSys('','帐号已从别处登陆','退出','重登',
		function()
			UI.showLogin()
		end,
		function()
			UI.showLogin()
		end)
end
