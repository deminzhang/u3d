print('test.lua')

--test
--http://www.xuanyusong.com/archives/4331 //HeadUI


local canvas = GameObject.Find("Canvas")
local o = GameObject.Find('Canvas/BtnTest')
if not o then
	local prefab = Resources.Load('UI/BtnTest');
	if prefab then
		o = GameObject.Instantiate(prefab,canvas.transform,false)
		o.name = 'BtnTest'
	end
end
local btnTest = GameObject.Find("Canvas/BtnTest"):GetComponent('Button')
btnTest.onClick:AddListener(function()
	dofile'hot'
	--UI.showLogin()
	
	-- print('XXXXXXXX')
	-- local cube = GameObject.CreatePrimitive(PrimitiveType.Cube)
	-- cube.transform.position = Vector3.New(1,1,1);
	--cube.transform.position:Set(1,1,1);
	
	
	do return end
	
	
	local o = GameObject.Find('role0')
	if not o then
		local prefab = Resources.Load('daozei_body2_1');
		print('prefab',prefab)
		if prefab then
			o = GameObject.Instantiate(prefab)
			--o.transform.position.Set(0,0,0);
			o.name = 'role0'
		end
	end
	local ani = o:GetComponent('Animation')
	--ani:Stop()
	ani:Play('atk1_2');
	ani.wrapMode = WrapMode.Default
	-- ani.wrapMode = WrapMode.Once --一次后转别的
	-- ani.wrapMode = WrapMode.Loop 
	-- ani.wrapMode = WrapMode.ClampForever --一次后停在本动画最后
	
	-- ani:Play("animation");//将开始播放名称为animation的动画
	-- ani:PlayQueued("animation");//排队播放名称为animation的动画
	-- ani:Play("animation", PlayMode.StopAll);// 播放animation动画 - 停止其他动画。
	-- ani:CrossFade("animation",1.5f);//在一定时间内淡入名称为animation的动画并且淡出其他动画
	-- ani:Stop();//停止所有动画。
	-- ani.wrapMode = WrapMode.Loop;//使用动画循环模式。
	
	RenderSettings.skybox = Resources.Load("Scene/SkyBox/sky_temp0", Material);
	--print('StartCoroutine',GameObject.StartCoroutine)
	--UI.showLoading(true,'desk')
	UI.showLoading(true,'UI/loading')
end)
