--_Application Interface
--Input,KetCode,Event,EventType
local Event = Event
local Input = Input
local KeyCode = KeyCode
local EventType = EventType
--Screen.width, Screen.height

--[[KeyCode = {
  Joystick6Button13 = 463;
  Joystick4Button12 = 422;
  F5 = 286;
  F11 = 292;
  Joystick1Button19 = 369;
  LeftApple = 310;
  U = 117;
  Mouse1 = 324;
  Joystick6Button19 = 469;
  Ampersand = 38;
  Joystick7Button15 = 485;
  JoystickButton17 = 347;
  AltGr = 313;
  Joystick4Button14 = 424;
  Asterisk = 42;
  Alpha2 = 50;
  Y = 121;
  Joystick2Button3 = 373;
  F9 = 290;
  Joystick2Button11 = 381;
  ScrollLock = 302;
  Alpha0 = 48;
  JoystickButton8 = 338;
  Mouse3 = 326;
  Joystick5Button2 = 432;
  Joystick4Button10 = 420;
  Joystick4Button8 = 418;
  Joystick2Button19 = 389;
  Joystick3Button15 = 405;
  Keypad6 = 262;
  F14 = 295;
  BackQuote = 96;
  RightApple = 309;
  Joystick6Button6 = 456;
  B = 98;
  Joystick2Button5 = 375;
  Joystick6Button0 = 450;
  F15 = 296;
  Joystick2Button7 = 377;
  Joystick8Button9 = 499;
  Joystick2Button17 = 387;
  At = 64;
  CapsLock = 301;
  Joystick1Button0 = 350;
  Slash = 47;
  Alpha8 = 56;
  Caret = 94;
  F = 102;
  Joystick4Button6 = 416;
  Joystick3Button1 = 391;
  Joystick4Button0 = 410;
  Tab = 9;
  Joystick3Button3 = 393;
  JoystickButton6 = 336;
  Joystick2Button13 = 383;
  Joystick6Button18 = 468;
  RightBracket = 93;
  Joystick5Button16 = 446;
  Joystick2Button2 = 372;
  F2 = 283;
  JoystickButton4 = 334;
  Menu = 319;
  Joystick4Button7 = 417;
  R = 114;
  PageUp = 280;
  Joystick3Button5 = 395;
  F6 = 287;
  Joystick5Button12 = 442;
  Joystick3Button7 = 397;
  Joystick2Button18 = 388;
  KeypadPlus = 270;
  KeypadPeriod = 266;
  Joystick7Button17 = 487;
  Exclaim = 33;
  KeypadEnter = 271;
  Joystick5Button14 = 444;
  V = 118;
  Alpha6 = 54;
  Joystick8Button7 = 497;
  Z = 122;
  Joystick2Button9 = 379;
  Joystick5Button8 = 438;
  DoubleQuote = 34;
  Alpha4 = 52;
  Joystick7Button3 = 473;
  Joystick5Button10 = 440;
  Joystick7Button19 = 489;
  Joystick3Button17 = 407;
  RightWindows = 312;
  Joystick3Button2 = 392;
  C = 99;
  LeftShift = 304;
  Delete = 127;
  Joystick6Button9 = 459;
  Joystick3Button11 = 401;
  Joystick1Button16 = 366;
  G = 103;
  Joystick3Button13 = 403;
  Joystick6Button16 = 466;
  JoystickButton10 = 340;
  Joystick7Button5 = 475;
  Joystick5Button6 = 436;
  Joystick1Button9 = 359;
  Joystick3Button19 = 409;
  Joystick5Button0 = 430;
  KeypadEquals = 272;
  PageDown = 281;
  K = 107;
  Joystick4Button9 = 419;
  Keypad5 = 261;
  Mouse4 = 327;
  Quote = 39;
  Joystick8Button11 = 501;
  Insert = 277;
  JoystickButton12 = 342;
  Joystick8Button2 = 492;
  Escape = 27;
  Pause = 19;
  Joystick1Button14 = 364;
  KeypadMinus = 269;
  Joystick5Button7 = 437;
  S = 115;
  Period = 46;
  Joystick4Button19 = 429;
  Mouse6 = 329;
  Joystick7Button2 = 472;
  JoystickButton16 = 346;
  F7 = 288;
  Joystick7Button18 = 488;
  Mouse0 = 323;
  W = 119;
  Joystick3Button18 = 408;
  Joystick6Button10 = 460;
  RightShift = 303;
  Clear = 12;
  Joystick2Button6 = 376;
  Joystick2Button14 = 384;
  Underscore = 95;
  JoystickButton9 = 339;
  Mouse2 = 325;
  Alpha3 = 51;
  RightControl = 305;
  Joystick2Button16 = 386;
  Joystick6Button1 = 451;
  Help = 315;
  KeypadDivide = 267;
  Break = 318;
  Space = 32;
  Print = 316;
  Joystick1Button1 = 351;
  Joystick2Button4 = 374;
  Joystick1Button3 = 353;
  Keypad9 = 265;
  Home = 278;
  H = 104;
  Joystick4Button1 = 411;
  Joystick4Button3 = 413;
  Joystick8Button19 = 509;
  Joystick4Button15 = 425;
  Joystick8Button18 = 508;
  Joystick1Button15 = 365;
  Joystick8Button17 = 507;
  Joystick6Button5 = 455;
  Joystick5Button9 = 439;
  Joystick8Button16 = 506;
  Numlock = 300;
  Joystick6Button7 = 457;
  L = 108;
  Joystick8Button15 = 505;
  Joystick3Button0 = 390;
  LeftArrow = 276;
  Joystick8Button14 = 504;
  Joystick6Button4 = 454;
  Joystick8Button13 = 503;
  JoystickButton14 = 344;
  Keypad0 = 256;
  Joystick8Button10 = 500;
  Joystick8Button6 = 496;
  Joystick8Button8 = 498;
  Joystick1Button5 = 355;
  JoystickButton18 = 348;
  Keypad3 = 259;
  Backslash = 92;
  Joystick1Button7 = 357;
  P = 112;
  Joystick8Button4 = 494;
  JoystickButton7 = 337;
  Joystick5Button11 = 441;
  JoystickButton2 = 332;
  LeftParen = 40;
  Joystick8Button3 = 493;
  Joystick4Button5 = 415;
  Joystick4Button18 = 428;
  JoystickButton19 = 349;
  Joystick8Button1 = 491;
  Joystick6Button11 = 461;
  Joystick7Button16 = 486;
  Joystick7Button14 = 484;
  JoystickButton5 = 335;
  Joystick7Button13 = 483;
  Colon = 58;
  Joystick4Button4 = 414;
  Joystick2Button15 = 385;
  J = 106;
  Greater = 62;
  Joystick7Button11 = 481;
  RightArrow = 275;
  LeftBracket = 91;
  Joystick7Button9 = 479;
  Joystick3Button4 = 394;
  DownArrow = 274;
  Joystick7Button8 = 478;
  Joystick7Button7 = 477;
  Joystick7Button12 = 482;
  Joystick7Button4 = 474;
  Joystick6Button2 = 452;
  Minus = 45;
  Joystick7Button1 = 471;
  X = 120;
  Joystick7Button0 = 470;
  Joystick1Button18 = 368;
  Joystick6Button15 = 465;
  F8 = 289;
  End = 279;
  Joystick6Button14 = 464;
  Joystick6Button12 = 462;
  Joystick8Button0 = 490;
  LeftCommand = 310;
  Joystick6Button8 = 458;
  Joystick6Button3 = 453;
  LeftAlt = 308;
  Joystick5Button17 = 447;
  Joystick5Button19 = 449;
  KeypadMultiply = 268;
  Joystick1Button2 = 352;
  Joystick5Button18 = 448;
  Joystick1Button10 = 360;
  Alpha9 = 57;
  F3 = 284;
  Joystick3Button14 = 404;
  Joystick4Button17 = 427;
  RightParen = 41;
  Q = 113;
  F4 = 285;
  F10 = 291;
  Joystick7Button6 = 476;
  Backspace = 8;
  A = 97;
  O = 111;
  Alpha7 = 55;
  Keypad4 = 260;
  RightAlt = 307;
  Joystick2Button1 = 371;
  Joystick2Button0 = 370;
  F13 = 294;
  Joystick5Button13 = 443;
  Joystick4Button2 = 412;
  Joystick7Button10 = 480;
  Joystick1Button12 = 362;
  Joystick3Button16 = 406;
  D = 100;
  Mouse5 = 328;
  Joystick2Button8 = 378;
  Joystick3Button10 = 400;
  E = 101;
  Keypad7 = 263;
  None = 0;
  Alpha5 = 53;
  JoystickButton3 = 333;
  Joystick8Button12 = 502;
  Dollar = 36;
  Keypad2 = 258;
  JoystickButton0 = 330;
  SysReq = 317;
  Joystick5Button4 = 434;
  Joystick5Button1 = 431;
  Joystick2Button10 = 380;
  Comma = 44;
  Equals = 61;
  Joystick5Button3 = 433;
  F12 = 293;
  I = 105;
  JoystickButton1 = 331;
  Joystick3Button12 = 402;
  Less = 60;
  LeftControl = 306;
  Semicolon = 59;
  Joystick4Button13 = 423;
  Joystick8Button5 = 495;
  Joystick1Button4 = 354;
  Joystick1Button6 = 356;
  Joystick1Button8 = 358;
  Joystick5Button15 = 445;
  Joystick1Button17 = 367;
  Joystick2Button12 = 382;
  Keypad8 = 264;
  LeftWindows = 311;
  Joystick6Button17 = 467;
  M = 109;
  Joystick4Button16 = 426;
  Return = 13;
  RightCommand = 309;
  Plus = 43;
  Joystick3Button9 = 399;
  JoystickButton11 = 341;
  N = 110;
  T = 116;
  Keypad1 = 257;
  Alpha1 = 49;
  UpArrow = 273;
  Joystick3Button6 = 396;
  Joystick3Button8 = 398;
  F1 = 282;
  Joystick1Button11 = 361;
  Joystick4Button11 = 421;
  Joystick5Button5 = 435;
  JoystickButton13 = 343;
  Joystick1Button13 = 363;
  Hash = 35;
  Question = 63;
  JoystickButton15 = 345;
};
--]]
---------------------------------------------------------------
--主帧
function _OnUpdate(e)
	onUpdate{e=e}
	
	local hero = _G.MeRole
	if hero then
		local pos = hero.pos
		local px,pz = pos.x,pos.z
		local tx,tz = px,pz
		local move
		if Input.GetKey(KeyCode.W) then
			tz = tz + 0.1
		end
		if Input.GetKey(KeyCode.A) then
			tx = tx - 0.1
		end
		if Input.GetKey(KeyCode.S) then
			tz = tz - 0.1
		end
		if Input.GetKey(KeyCode.D) then
			tx = tx + 0.1
		end
		tx = math.max(0.1,tx)
		tz = math.max(0.1,tz)
		if tx~=px or tz~=pz then
			GS.Run{X=tx, Z=tz}
			hero:runTo(tx,nil,tz) --前端先行
		end
	end
	if MeRole then
		local cameraMat = Camera.main.transform
		local rpos = MeRole.pos
		local cpos = cameraMat.position
		cpos.x = rpos.x
		cpos.y = rpos.y + 30
		cpos.z = rpos.z
		cameraMat.position = cpos
		-- cameraMat:LookAt(MeRole.avatar.transform)

	end
end
---------------------------------------------------------------
--键盘的快捷一般放在_OnKeyDown
--鼠标的快捷一般放在_OnMouseUp
---------------------------------------------------------------
--key 没有独立的shift事件
local _downkeys = {} --已按下的键
function _OnKeyDown(key,ctrl,alt,shift)
	if not _downkeys[key] then
		_downkeys[key] = true
		print('_OnKeyDown',key,ctrl,alt,shift)
		--按下只触发一次快捷
		--KeyCode.Alpha1
		--KeyCode.Alpha2
		--KeyCode.Alpha3
		onKeyDown{key=key,ctrl=ctrl,alt=alt,shift=shift}
	end
	print('_OnKeyKeepDown',key,ctrl,alt,shift)
	onKeyKeepDown{key=key,ctrl=ctrl,alt=alt,shift=shift}
	--按住类快捷
	--KeyCode.W
	--KeyCode.A
	--KeyCode.S
	--KeyCode.D
end
function _OnKeyUp(key,ctrl,alt,shift)
	_downkeys[key] = false
	print('_OnKeyUp',key,ctrl,alt,shift)
	onKeyUp{key=key,ctrl=ctrl,alt=alt,shift=shift}
end
---------------------------------------------------------------
--mouse
function _OnMouseDown(key,x,y)
	print('_OnMouseDown',key,x,y)
	--onMouseDown{key=key,x=x,y=y}
	--Terrain
end
local lastx,lasty
function _OnMouseUp(key,x,y)
	-- local e = Event.current
	-- e.control,e.alt,e.shift
	-- print('_OnMouseUp',key,x,y)
	-- onMouseUp{key=key,x=x,y=y}
	if key==0 then
		print('onMouseLClick{x=x,y=y}',x,y)
		onMouseLClick{x=x,y=y}
	elseif key==1 then
		print('onMouseRClick{x=x,y=y}',x,y)
		onMouseRClick{x=x,y=y}
	elseif key==2 then
		print('onMouseMClick{x=x,y=y}',x,y)
		onMouseMClick{x=x,y=y}
	end
	lastx,lasty = nil,nil
end
function _OnMouseDrag(key,x,y)
	-- local e = Event.current
	-- e.control,e.alt,e.shift
	lastx,lasty = lastx or x,lasty or y
	local dx,dy = x-lastx,y-lasty
	lastx,lasty = x,y
	print('_OnMouseDrag',key,x,y)
	onMouseDrag{key=key,x=x,y=y}
	
	if MeRole then
		local pos = MeRole.avatar.transform.position
		local mat = Camera.main.transform
		print('X',pos)
		mat:RotateAround(pos,Vector3.up,dx>0 and 1 or -1);
		mat:RotateAround(pos,mat.right,dy>0 and 1 or -1);
	end
end
function _OnMouseWheel(deltaX,deltaY)
	-- local e = Event.current
	-- e.control,e.alt,e.shift
	print('_OnMouseWheel',deltaY)
	onMouseWheel{deltaX=deltaX,deltaY=deltaY}
	
	local camera = Camera.main
	if deltaY<0 then
		if(camera.fieldOfView<=100) then
			camera.fieldOfView = camera.fieldOfView+2;
		end
		if(camera.orthographicSize<=20) then
			camera.orthographicSize = camera.orthographicSize + 0.5;
		end
	elseif deltaY>0 then
		if(camera.fieldOfView>2) then
			camera.fieldOfView = camera.fieldOfView-2;
		end
		if(camera.orthographicSize>=1) then
			camera.orthographicSize = camera.orthographicSize - 0.5;
		end
	end
	
end
function _OnMouseDbclick(key,x,y)
	-- U3D木有,必要时自己实现 游戏中很少用双击了
	print('_OnMouseDbclick',key,x,y)
end
function _OnMouseMove(key,x,y)
	-- U3D木有 仅__EDITOR有 PC一般用MouseDrag,phone一般用TouchMove,GUI的有
	-- GUIElement或ColliderC#有OnMouse*方法
	print('_OnMouseMove',key,x,y)
end
---------------------------------------------------------------
--if Input.touchSupported 
--x,y当前触摸的位置；
--count当前屏幕上触摸点的个数；
--fingerId触发当前回调的手指id；
--force当前触摸的平均力度(0-1?)。if not Input.touchPressureSupported then return 1.0f end
local TOUCH_SIZE_X = 0.2
local TOUCH_SIZE_Y = 0.2
function _OnTouchBegin(x, y, count, fingerId, force, maxForce)
	print('onTouchBegin', x, y, count, fingerId, force, maxForce)
	onTouchBegin{x=x, y=y, count=count, fingerId=fingerId, force=force, maxForce=maxForce}
end
--Move/Stationary
function _OnTouchMove(x, y, count, fingerId, force, maxForce)
	print('onTouchMove', x, y, count, fingerId, force, maxForce)
	onTouchMove{x=x, y=y, count=count, fingerId=fingerId, force=force, maxForce=maxForce}
	local nx,ny = x/Screen.width, y/Screen.height
	if (nx < TOUCH_SIZE_X) then
		if (ny < TOUCH_SIZE_Y) then
			--Move(0, 1, Time.deltaTime);
		elseif (npos.y < TOUCH_SIZE_Y * 2) then
			--Move(0, -1, Time.deltaTime);
		end
	elseif (npos.x > 1 - TOUCH_SIZE_X) then
		if (npos.y < TOUCH_SIZE_Y) then
			--TurnCamera(0, 1, Time.deltaTime);
		elseif (npos.y < TOUCH_SIZE_Y * 2) then
			--TurnCamera(1, 0, Time.deltaTime);
		end
	end
end

function _OnTouchEnd(x, y, count, fingerId, force, maxForce)
	print('onTouchEnd', x, y, count, fingerId, force, maxForce)
	onTouchEnd{x=x, y=y, count=count, fingerId=fingerId, force=force, maxForce=maxForce}
end
--The system cancelled tracking for the touch.
function _OnTouchCancel(x, y, count, fingerId, force, maxForce)
	print('onTouchCancel', x, y, count, fingerId, force, maxForce)
	onTouchCancel{x=x, y=y, count=count, fingerId=fingerId, force=force, maxForce=maxForce}
end
---------------------------------------------------------------

local coroutines = {} --TODO 弱对象依赖
function _Coroutine(start)
	coroutines[coroutine.create(start)] = true
end
function event.onUpdate(e)
	for co, _ in pairs(coroutines) do
		if coroutine.status(co)=='suspended' then
			coroutine.resume(co)
		else
			coroutines[co] = nil
		end
	end
end
