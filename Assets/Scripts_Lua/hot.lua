print('hot.lua')

--CS.TryCrossZoneTest{Zid=CurSenID==1 and 2 or 1} --跨服传
--CS.TryChangeZoneTest{Zid=CurSenID==1 and 2 or 1} --跨线传
--GS.TestTrans{Zid=CurSenID==1 and 3 or 1} --同线传

--GS.AskTransTo{Zid=CurSenID==1 and 3 or 1, Point=}

GS.HotTest{T=[[
	local _R = debug.getregistry()
	--local queues = _R[5]
	print('BytesLen', table.count(_R[3]))
	-- dump(queues,2)
	-- debug.profiler(30, 'profiler.log')
]]}