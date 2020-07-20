_G.const = {}
local tb = {}
setmetatable(const,{__index=tb,__newindex=function(t,k,v)
	assert(tb[k]==nil, k..' const can not be changed')
	tb[k] = v
end})
