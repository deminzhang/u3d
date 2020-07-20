
local ui = UI.new('SelectRole')
ui.cache = false			--是否缓存
ui.select = 1
ui.roleList = {}
local ROLE_MAX = 3

function ui:onNew()
	print(self.name,'onNew')
	ui.select = 1
	for i=1, ROLE_MAX do
		local button = GameObject.Find("Canvas/UISelectRole/BtnRole"..i)
		if button then
			local btn = button:GetComponent('Button')
			btn.onClick:AddListener(function()
				local role = ui.roleList[i]
				if role then
					ui.select = i
					local txt = self:getCom('BtnDelete/Text','Text')
					if role.delflag then
						txt.text = _T'恢复角色'
					else
						txt.text = _T'删除角色'
					end
				else
					UI.showCreateRole(true,ui.roleList)
				end
			end)
		end
	end
	
	
	local txt = self:getCom('BtnEnter/Text','Text')
	txt.text = _T'进入游戏'
	local btn = self:getCom('BtnEnter','Button')
	btn.onClick:AddListener(function()
		local role = ui.roleList[ui.select]
		CS.TryEnter{Pid=role.pid}
	end)
	local txt = self:getCom('BtnDelete/Text','Text')
	txt.text = _T'删除角色'
	local btn = self:getCom('BtnDelete','Button')
	btn.onClick:AddListener(function()
		UI.showMsgBox(true,'warn','do u wanna to delete role?!','delete','cancel',
			function()
				local role = ui.roleList[ui.select]
				if role.delflag then
					CS.RecoverRole{Pid=role.pid}
				else
					CS.DeleteRole{Pid=role.pid}
				end
			end)
	end)
end

function ui:onShow(oldShow,Data)
	UI.showCreateRole(false)
	ui.roleList = Data or ui.roleList or {}
	dump(ui.roleList)
	if not ui.roleList[ui.select] then
		ui.select = 1
	end
	for i=1, ROLE_MAX do
		local txt = self:getCom(string.format('BtnRole%d/Text',i),'Text')
		local role = ui.roleList[i]
		if role then
			txt.text = role.name
		else
			txt.text = _T'创建角色'
		end
	end
end

function ui:onUpdate(...)
	print(self.name,'onUpdate')
end

define.RoleNew{Data={}}
function event.RoleNew(Data)
	table.insert(ui.roleList, Data)
	UI.showSelectRole(true,ui.roleList)
end

define.RoleDel{Pid=0}
function event.RoleDel(Pid)
	for i,v in pairs(ui.roleList) do
		if v.pid == Pid then
			table.remove(ui.roleList,i)
			if ui.roleList[1] then
				UI.showSelectRole(true,ui.roleList)
			else
				UI.showCreateRole(true,ui.roleList)
			end
			break
		end
	end
end

local enterTrys = 0
define.OnLogin{ Res = {} }
function event.OnLogin(Res)
	print('OnLogin')
	dump(Res)
	--notice(_T'正在进入')
	print(Res.addr,Res.token)
	GS.connect(Res.addr,Res.token)
	enterTrys = 0
end

define.EnterWait{}
function event.EnterWait()
	enterTrys = enterTrys + 1
	print('EnterWait',enterTrys)
	if enterTrys<5 then
		Timer.add(2000,function()
			GS.Enter{Token=GS.token}
		end)
	else
		local role = ui.roleList[ui.select]
		CS.TryEnter{Pid=role.pid}
	end
end
