
_G.CS = {}
_G.GS = {}
local gsnet
local csnet
----------------------------------------------------------------
function CS.isConnect()
	return csnet and not csnet:closed()
end
function CS.close()
	if CS.isConnect() then csnet:close() end
end
function CS.connect(host,user,pass,kick)
	print('CS.connect',host,user,pass)
	--Net.connect('localhost:9000', function(net)
	Net.connect(host, function(net)
		print('>>CS.onConnect',net, net:macaddr())
		csnet = net
		CS.Login{Uid=user,Pass=pass,Token='1',Mac=csnet:macaddr(),Kick=kick}
	end, function(net, err, code)
		print('>>CS.onClose',net, err, code)
		csnet = nil
		if __EDITOR and err == 'collect' then return end
		UI.showMsgBox(true,'','connect server fail!','Retry','Exit',
			function()
				UI.showLoading(true,'UI/loading','login',function()
					_G.CurSenID = nil
					_G.CurSen = nil
					_G.Me = nil
					UI.showLogin()
					UI.showLoading(false)
				end)
			end,
			function()
				if not __EDITOR then
					os.exit(0)
				end
			end)
	end, 5)
end
_callout(CS, function( CS, rpc, args, data, len )
	if CS.isConnect() then
		csnet:send(string.from32l(len))
		csnet:send(data,len)
	else
		print('>>CS not connected')
	end
end )
----------------------------------------------------------------
function GS.isConnect()
	return gsnet and not gsnet:closed()
end
function GS.close()
	if GS.isConnect() then gsnet:close() end
end
function GS.connect(addr, token)
	GS.addr = addr
	GS.token = token
	if GS.isConnect() then gsnet:close('reconn') end
	Net.connect(addr, function(net)
		print('>>GS.onConnect',net, net:macaddr())
		gsnet = net
		GS.Enter{Token=token}
	end, function(net, err, code)
		print('>>GS.onClose',net, err, code)
		if GS.addr ~= addr then return end --换gs了
		gsnet = nil
		if __EDITOR and err == 'collect' then return end
		if not CS.isConnect() then return end --CS也断了就等CS先重连
		UI.showMsgBox(true,'','connect gs fail!','Retry','Exit',
			function()
				GS.connect(addr, token)
			end,
			function()
				if not __EDITOR then
					os.exit(0)
				end
			end)
	end)
end
_callout(GS, function( GS, rpc, args, data, len )
	if GS.isConnect() then
		gsnet:send(string.from32l(len))
		gsnet:send(data,len)
	else
		print('>>GS not connected')
	end
end )
----------------------------------------------------------------
function event.onSecond()
	if CS.isConnect() then
		--Net.RPC(csnet, 'Hello',{T="2stCS:Hi!CS"})
		-- CS.Hello{T="3stCS:Hi!CS from client"}
		--CS.csnet.Hello{T="4stCS:Hi!CS"}
	end
	if GS.isConnect() then
		-- GS.Hello{T="3stCS:Hi!GS from client"}
	end
end
