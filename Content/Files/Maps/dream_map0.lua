require('map')
local texture = { 0 = "Images\\Platforms\\PlatformSpriteSheet",
				  1 = "Images\\Platforms\\SymbolPaltforms\\SymbolPlatformSpriteSheet1V3",
				  2 = "Images\\Platforms\\SymbolPaltforms\\SymbolPlatformSpriteSheet2V3"
				  3 = "Images\\Platforms\\SymbolPaltforms\\SymbolPlatformSpriteSheet3V3"
				  4 = "Images\\Platforms\\SymbolPaltforms\\SymbolPlatformSpriteSheet4V3"
				  5 = "Images\\Platforms\\SymbolPaltforms\\SymbolPlatformSpriteSheet5V3"}
local map = Map:new({-600, 847},
					{400, 100},
					{{0,0}, {1,0}, {2,0}, {3,0}, {4,0}, {5,0}, {6,0}, {7,0}},
					{0, 869, 2016, 947},
					{0, 250},
					{50, 150},
					{{texture[4], 2, 0, true}})