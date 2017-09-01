--Base file for item prefab
Item = {}

function Item:new(itemID, 
	                 itemValue, 
	                 itemTexture, 
	                 itemEffect, 
	                 itemDescription)
    self.__index = self
    local item = {}
    setmetatable(item, Item)
        item.id = itemID
        item.value = itemValue
        item.texture = itemTexture
        item.effect = itemEffect
        item.description = itemDescription
    return item
end

function Item:GetID()
    return self.id
end

function Item:GetValue()
    return self.value
end

function Item:GetTexture()
    return self.texture
end

function Item:GetEffect()
    return self.effect
end

function Item:GetDescription()
    return self.description
end