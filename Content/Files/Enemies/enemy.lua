Enemy = {}
defaultMillisecondsPerFrame = 16

function Enemy:new(enemyID, 
	                   enemyTexture, 
	                   enemyFrameSize, 
	                   enemyCurrentFrame, 
	                   enemySheetSize, 
	                   enemyCollisionOffset,
					   enemySpeed,
	                   enemyHealth,
	                   enemyDefence,
	                   enemyAttack)
    self.__index = self
    local obj = {}
    setmetatable(obj, Enemy)
        obj.name = enemyID
        obj.texture = enemyTexture
        obj.frame_size = {
            x = enemyFrameSize[1],
            y = enemyFrameSize[2]
        }
        obj.current_frame = {
        	   x = enemyCurrentFrame[1],
        	   y = enemyCurrentFrame[2]
        }
        obj.sheet_size = {
        	   x = enemySheetSize[1],
        	   y = enemySheetSize[2]
        }
        obj.collision_offset = enemyCollisionOffset
		obj.speed = enemySpeed
        obj.milliseconds_per_frame = defaultMillisecondsPerFrame
        obj.hit_points = enemyHealth
        obj.defence_points = enemyDefence
        obj.attack_points = enemyAttack
    return obj
end

function Enemy:new(enemyID, 
	                   enemyTexture, 
	                   enemyFrameSize, 
	                   enemyCurrentFrame, 
	                   enemySheetSize, 
	                   enemyCollisionOffset,
					   enemySpeed,
	                   enemyHealth,
	                   enemyDefence,
	                   enemyAttack,
	                   enemyMillisecondsPerFrame)
    self.__index = self
    local obj = {}
    setmetatable(obj, Object)
        obj.name = enemyID
        obj.texture = enemyTexture
        obj.frame_size = {
            x = enemyFrameSize[1],
            y = enemyFrameSize[2]
        }
        obj.current_frame = {
        	   x = enemyCurrentFrame[1],
        	   y = enemyCurrentFrame[2]
        }
        obj.sheet_size = {
        	   x = enemySheetSize[1],
        	   y = enemySheetSize[2]
        }
        obj.collision_offset = enemyCollisionOffset
		obj.speed = enemySpeed
        obj.milliseconds_per_frame = enemyMillisecondsPerFrame
        obj.hit_points = enemyHealth
        obj.defence_points = enemyDefence
        obj.attack_points = enemyAttack
    return obj
end

function Enemy:new(enemyID, 
	                   enemyTexture, 
	                   enemyFrameSize, 
	                   enemyCurrentFrame, 
	                   enemySheetSize, 
	                   enemyCollisionOffset,
					   enemySpeed,
	                   enemyHealth,
	                   enemyDefence,
	                   enemyAttack,
					   visionRadius,
					   attackRadius,
					   jumpRadius)
    self.__index = self
    local obj = {}
    setmetatable(obj, Object)
        obj.name = enemyID
        obj.texture = enemyTexture
        obj.frame_size = {
            x = enemyFrameSize[1],
            y = enemyFrameSize[2]
        }
        obj.current_frame = {
        	   x = enemyCurrentFrame[1],
        	   y = enemyCurrentFrame[2]
        }
        obj.sheet_size = {
        	   x = enemySheetSize[1],
        	   y = enemySheetSize[2]
        }
        obj.collision_offset = enemyCollisionOffset
		obj.speed = enemySpeed
        obj.milliseconds_per_frame = defaultMillisecondsPerFrame
        obj.hit_points = enemyHealth
        obj.defence_points = enemyDefence
        obj.attack_points = enemyAttack
		obj.vision_rad = visionRadius
		obj.attack_rad = attackRadius
		obj.jump_rad = jumpRadius
    return obj
end

function Enemy:new(enemyID, 
	                   enemyTexture, 
	                   enemyFrameSize, 
	                   enemyCurrentFrame, 
	                   enemySheetSize, 
	                   enemyCollisionOffset,
					   enemySpeed,
	                   enemyHealth,
	                   enemyDefence,
	                   enemyAttack,
					   visionRadius,
					   attackRadius,
					   jumpRadius,
	                   enemyMillisecondsPerFrame)
    self.__index = self
    local obj = {}
    setmetatable(obj, Object)
        obj.name = enemyID
        obj.texture = enemyTexture
        obj.frame_size = {
            x = enemyFrameSize[1],
            y = enemyFrameSize[2]
        }
        obj.current_frame = {
        	   x = enemyCurrentFrame[1],
        	   y = enemyCurrentFrame[2]
        }
        obj.sheet_size = {
        	   x = enemySheetSize[1],
        	   y = enemySheetSize[2]
        }
        obj.collision_offset = enemyCollisionOffset
		obj.speed = enemySpeed
        obj.milliseconds_per_frame = enemyMillisecondsPerFrame
        obj.hit_points = enemyHealth
        obj.defence_points = enemyDefence
        obj.attack_points = enemyAttack
				obj.vision_rad = visionRadius
		obj.attack_rad = attackRadius
		obj.jump_rad = jumpRadius
    return obj
end

function Enemy:new(enemyID, 
	                   enemyTexture, 
	                   enemyFrameSize, 
	                   enemyCurrentFrame, 
	                   enemySheetSize, 
	                   enemyCollisionOffset,
					   enemySpeed,
	                   enemyHealth,
	                   enemyDefence,
	                   enemyAttack,
					   visionRadius,
					   attackRadius,
					   jumpRadius,
	                   enemyMillisecondsPerFrame,
					   enemyCollisionRectangle)
    self.__index = self
    local obj = {}
    setmetatable(obj, Object)
        obj.name = enemyID
        obj.texture = enemyTexture
        obj.frame_size = {
            x = enemyFrameSize[1],
            y = enemyFrameSize[2]
        }
        obj.current_frame = {
        	   x = enemyCurrentFrame[1],
        	   y = enemyCurrentFrame[2]
        }
        obj.sheet_size = {
        	   x = enemySheetSize[1],
        	   y = enemySheetSize[2]
        }
        obj.collision_offset = enemyCollisionOffset
		obj.speed = enemySpeed
        obj.milliseconds_per_frame = enemyMillisecondsPerFrame
        obj.hit_points = enemyHealth
        obj.defence_points = enemyDefence
        obj.attack_points = enemyAttack
		obj.vision_rad = visionRadius
		obj.attack_rad = attackRadius
		obj.jump_rad = jumpRadius
		obj.collision_rectangle = {x = enemyCollisionRectangle[1],
								   y = enemyCollisionRectangle[2],
								   width = enemyCollisionRectangle[3],
								   height = enemyCollisionRectangle[4]}
    return obj
end

function Enemy:GetID(self) 
    return self.name 
end

function Enemy:GetTexture(self)
    return self.texture
end

function Enemy:GetFrameSize(self)
    return self.frame_size
end

function Enemy:GetCurrentFrame(self)
    return self.current_frame
end

function Enemy:GetSheetSize(self)
    return self.sheet_size
end

function Enemy:GetCollisionOffset(self)
    return self.collision_offset
end

function Enemy:GetSpeed(self)
	return self.speed
end

function Enemy:GetMillisecondsPerFrame(self)
    return self.millisecond_per_frame
end

function Enemy:GetHealth(self)
    return self.hit_points
end

function Enemy:GetDefence(self)
    return self.defence_points
end

function Enemy:GetAttack(self)
    return self.attack_points
end

function Enemy:GetVisionRadius(self)
	return self.vision_rad
end

function Enemy:GetAttackRadius(self)
	return self.attack_rad
end

function Enemy:GetJumpRadius(self)
	return self.jump_rad
end

function Enemy:GetCollisionRectangle(self)
	return self.collision_rectangle
end


