Map = {}
function Map:new(groundStart, 
				 groundBlock, 
				 groundCoordinates, 
				 groundRectangle, 
				 platformsStart, 
				 platformsBlock, 
				 platfrormsCoordinates, 
				 lanternsStart, 
				 lanternsBlock, 
				 lanternsCoordinates)

self.__index = self
local map = {}
setmetatable(map, Map)

map.ground = { start = {x = groundStart[1],
							   y = groundStart[2]},
			   block = {x = groundBlock[1],
							   y = groundBlock[2]}
			  }
for i,v in ipairs(groundCoordinates) 
	do map.ground.coordinates.i = groundCoordinates[v] 
end
map.ground.rectangle = { x = groundRectangle[1],
						 y = groundRectangle[2],
						 width = groundRectangle[3],
						 heigth = groundRectangle[4]}

map.platform = { start = { x = platformsStart[1],
						   y = platformsStart[2]},
				 block = { x = platformsBlock[1],
						   y = platformsBlock[2]}
			    }
for i,v in ipairs(platformCoordinates) 
	do map.platform.coordinates.i = { texture = platformCoordinates[v][1],
								  coordinates = { x = platformCoordinates[v][2],
												  y = platformCoordinates[v][3]},
								  materiality = platformCoordinates[v][4]} 
end

map.lantern = { start = { x = lanternsStart[1],
						  y = lanternsStart[2]}
				block = { x = lanternsBlock[1],
						  y = lanternsBlock[2]}
				}
for i,v in ipairs(lanternsCoordinates) 
	do map.lantern.coordinates.i = lanternsCoordinates[v] 
end

return map
end

function Map:Get(type, arg)
	if type == "ground"
		then 	
			return Map:GetGround(arg)
	elseif type == "platform"
		then
			return Map:GetPlatforms(arg)
	elseif type == "lantern"
		then
			return Map:GetLantern(arg)
	else
		return self
	end
end

function Map:GetGround(arg)
	if arg == "start" 
		then return self.ground.start
	elseif arg == "block"
		then return self.ground.block
	elseif arg == "coordinates"
		then return self.ground.coordinates
	elseif arg == "rectangle"
		then return self.ground.rectangle
	else 
		return self.ground;
	end
end

function Map:GetPlatforms(arg)
	if arg == "start" 
		then return self.platform.start
	elseif arg == "block"
		then return self.platform.block
	elseif arg == "coordinates"
		then return self.platform.coordinates
	else 
		return self.platform;
	end
end

function Map:GetLantern(arg)
	if arg == "start" 
		then return self.lantern.start
	elseif arg == "block"
		then return self.lantern.block
	elseif arg == "coordinates"
		then return self.lantern.coordinates
	else 
		return self.lantern;
	end
end