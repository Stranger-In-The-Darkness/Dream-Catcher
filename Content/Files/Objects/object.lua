--Base file for object prefab
Object = {}
defaultMillisecondsPerFrame = 16

function Object:new(objectID, 
	                objectValue, 
	                objectTexture, 
	                objectPosition, 
	                objectFrameSize, 
	                objectCurrentFrame, 
	                objectSheetSize, 
	                objectCollisionOffset)
    self.__index = self
    local obj = {}
    setmetatable(obj, Object)
        obj.name = objectID
        obj.value = objectValue
        obj.texture = objectTexture
        obj.position = {
        	   x = objectPosition[1],
        	   y = objectPosition[2]
        }
        obj.frame_size = {
            x = objectFrameSize[1],
            y = objectFrameSize[2]
        }
        obj.current_frame = {
        	   x = objectCurrentFrame[1],
        	   y = objectCurrentFrame[2]
        }
        obj.sheet_size = {
        	   x = objectSheetSize[1],
        	   y = objectSheetSize[2]
        }
        obj.collision_offset = objectCollisionOffset
        obj.milliseconds_per_frame = defaultMillisecondsPerFrame
    return obj
end

function Object:new(objectID, 
	                   objectValue, 
	                   objectTexture, 
	                   objectPosition, 
	                   objectFrameSize, 
	                   objectCurrentFrame, 
	                   objectSheetSize, 
	                   objectCollisionOffset, 
	                   objectMillisecondsPerFrame)
    self.__index = self
    local obj = {}
    setmetatable(obj, Object)
        obj.name = objectID
        obj.value = objectValue
        obj.texture = objectTexture
        obj.position = {
        	   x = objectPosition[1],
        	   y = objectPosition[2]
        }
        obj.frame_size = {
            x = objectFrameSize[1],
            y = objectFrameSize[2]
        }
        obj.current_frame = {
        	   x = objectCurrentFrame[1],
        	   y = objectCurrentFrame[2]
        }
        obj.sheet_size = {
        	   x = objectSheetSize[1],
        	   y = objectSheetSize[2]
        }
        obj.collision_offset = objectCollisionOffset
        obj.milliseconds_per_frame = objectMillisecondsPerFrame
    return obj
end

function Object:GetID() 
    return self.name 
end

function Object:GetValue()
    return self.value
end

function Object:GetTexture()
    return self.texture
end

function Object:GetPosition()
    return self.position
end

function Object:GetFrameSize()
    return self.frame_size
end

function Object:GetCurrentFrame()
    return self.current_frame
end

function Object:GetSheetSize()
    return self.sheet_size
end

function Object:GetCollisionOffset()
    return self.collision_offset
end

function Object:GetMillisecondsPerFrame()
    return self.millisecond_per_frame
end
