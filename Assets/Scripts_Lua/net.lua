
--[[in C===================================================================
_net.listen(addr,onListen,onClose)
	onListen(net, listener, cip, cport, sip, sport, share)
	net:close()
	net:closed()
	net:receive( len, func_onReceive(net, data), timeout, bool_tostr)
	net:send(string)
	net:nagle(bool) --setnodelay
	net:share(...)
_net.connect(addr,port,onConnect,onClose,timeOut,laddress,lport)
	onConnect(net, listener, cip,cport, sip,sport, share)
	onClose(net, res, errcode) --res=reason or where
_net.hostips(addr) --DNS return IPs

function _callin(from, data)
	local fn, args = _decode(data)
	local func = event[fn]
	assert(func, fn..'invaild RPC')
	_enqueue(0, from, fn, args)
end
--]]
--lua===================================================================
_G.Net = {}
---------------------------------------------------------------
--config/const
local NET_LOG_ON = true			--日志开关
local NET_RECV0_TIMEOUT = 10	--首次连接超时s
local NET_RECV_TIMEOUT = 300	--接收超时s
local NET_CONN_TIMEOUT = 10		--连接起时s
local NET_HEAD_LEN = 4			--包头长度
local NET_RECV_MAX_LEN = 0x100000--最大接收大小
Net.HEART 			= 0

Net.NET_HEAD_LEN = NET_HEAD_LEN
----------------------------------------------------------------
--to local
local format = string.format
local char = string.char
local from32l = string.from32l
local to32l = string.to32l
local find = string.find
local toJson = table.toJson
local push = table.push
local _listen = _net.listen
local _connect = _net.connect
local _hostips = _net.hostips
----------------------------------------------------------------
--local
local netnum = 0
local nets = {}				--main refer
local weaks = table.weakk()	--weakk refer
local newconn = table.weakk()
local sendcache = nil
----------------------------------------------------------------
--normal count of nets
function Net.count()
	return netnum,nets
end
--weak count of nets
function Net.weakCount()
	return table.count(weaks),weaks
end
--new count of nets
function Net.newCount()
	return table.count(newconn),newconn
end
--cleanup newconn without valid data timeout
function Net.cleanUp()
	local now = os.time()
	for net, t in pairs(newconn) do
		if now - t > 60 then
			newconn[net] = nil
			net:close('newnettimeout')
		end
	end
end

function Net.sendEx(net, data)
	if sendcache then
		push( sendcache, net, data )
	else
		return net:send( data )
	end
end
----------------------------------------------------------------
define._netSecond{}
when{} function _netSecond()
	_netSecond{_delay=1000}
	Net.cleanUp()
end
_netSecond{_delay=2000}
----------------------------------------------------------------
--common 
local onListen0, _onClose0, onHead, onBody, onHeart
function onHead(net, data)
	local len = data:to32l()
	if len==542393671 then --if data:lead'GET ' then
		Http.onHttpGet(net,data)
	elseif len==1414745936 then --if data:lead'POST' then
		Http.onHttpPost(net,data)
	elseif len==0 then		--hearbeat
		net:receive(NET_HEAD_LEN, onHeart, NET_RECV_TIMEOUT)
	elseif len < 0 or len > NET_RECV_MAX_LEN then --1~int4
		net:close('dataerror')
	else --bodylength
		net:receive(len, onBody, NET_RECV_TIMEOUT)
	end
end
function onBody(net, data)
	--print( net, 'onBody>>', data )
	newconn[net] = nil
	_callin( net, data )
	net:receive(NET_HEAD_LEN, onHead, NET_RECV_TIMEOUT)
end 
function onHeart(net, data)
	local code = data:to32l()
	print('>>onHeartCode:', code)
	net:receive(NET_HEAD_LEN, onHead, NET_RECV_TIMEOUT)
end 
----------------------------------------------------------------
local function onConnect0(net, ip, port, myip, myport)
	print('>>onConnect',net, ip, port)
	Net.callout( net )
	local pipe = net.onPipe
	if pipe and pipe~='' then
		net:send('PIPE')
		net:send(char(#pipe))
		net:send(pipe)
	end
	net:receive(NET_HEAD_LEN, onHead, NET_RECV_TIMEOUT)
	local onConnect = net.onConnect
	if onConnect then
		onConnect(net, ip, port, myip, myport)
	end
end
local function onClose0(net, err, code)
	print('>>connect.onClose',net, err, code)
	local onClose = net.onClose
	if onClose then
		onClose(net, err, code)
	end
end
function Net.connect(addr,onConnect,onClose,timeout)
	local _,_,host,port,pipe = addr:find("[%[]*([^%]]*)[%]]*:(%d+)[%@]*([^%@]*)")
	if not host then
		error('Net.listen invalid addr:',addr)
	end
	local ip = _hostips(host)
	print('>>connect:',host,port,pipe,ip)
	if not ip then
		error( 'no host:'..host )
		return
	end
	local net = _connect(ip, toint(port), onConnect0, onClose0, timeout or NET_CONN_TIMEOUT)
	net.onConnect = onConnect
	net.onClose = onClose
	net.onPipe = pipe
	return net
end
----------------------------------------------------------------Http
-- 解析xxx1=xxxa&xxx2=xxxb的字符串为表
--Net.parseParam('xxx1=xxxa&xxx2=xxxb')return{xxx1='xxxa',xxx2='xxxb'}
local tmp
local deurl = function(k,v)
	tmp[k:deurl()] = v:deurl()
end
function Net.parseParam(s,out)
	tmp = out or {}
	s:gsub('(%w+)=(%w+)',deurl)
	out = tmp; tmp = nil
	return out
end
--解析xxx.com/api?xx=xx&cc=cc
--为{xx=xx,cc=cc}, 'api', xx=xx&cc=cc
function Net.parseUrl(url)
	local _, _, cf, params  = find(url, '%/([%a%_%/]-%.*[%a%_%/]*)%?(.*)')
	if cf==nil then
		_,_,cf = find(url, '%/(.*)')
		if cf==nil then
			return;
		end
		return nil, cf;
	end
	-- 解析参数
	local ps --paramtable
	if params then
		local _, _, args, _  = find(params, '(.*)%s(.*)')
		if args then
			ps = Net.parseParam(args)
		else
			ps = Net.parseParam(params)
		end
	end
	return ps, cf, params
end

----------------------------------------------------------------
function Net.callout( net )
	if not _callout( net ) then
		_callout( net, function( net, rpc, args, data, len )
			--local data,len = _encode(rpc,args)
			net:send(from32l(len))
			net:send(data,len)
		end )
	end
end

function Net.send(net, data, len)
	if not len then len = #data end
	net:send(from32l(len))
	net:send(data, len)
end

function Net.RPC(net, name, args, data)
	local data, len = _encode(name, args)
	assert(net,'net is nil')
	net:send(from32l(len))
	net:send(data, len)
end

function Net.sendText(net, text)
	local s = format( {"HTTP/1.1 200 OK\r\nContent-Type: text/html;charset=utf-8\r\nConnection: close\r\nContent-Length: %d\r\n\r\n%s",#text,text} )
	-- sendcount = sendcount + #s
	Net.sendEx(net, s)
end

function Net.sendJson(net, tb)
	local content = toJson(tb)
	local s = format( "HTTP/1.1 200 OK\r\nContent-Type: application/json;charset=utf-8\r\nConnection: close\r\nContent-Length: %d\r\n\r\n%s", #content, content )
	--sendcount = sendcount + #s
	Net.sendEx(net, s)
end
