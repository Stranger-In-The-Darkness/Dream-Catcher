require('Content\\Files\\Enemies\\enemy')

local enemy = Enemy:new("b_shadow_hunter",
					"Images\\Enemies\\EnemyAnimation1V2",
					{101,130},
					{0,0},
					{8,4},
					0,
					{1.7, 1.5},
					1,
					0,
					0,
					200,
					50,
					140,
					80,
					{20,0,60,130}
					)
return enemy
--Enemy:GetID(enemy)
--Enemy:GetTexture(enemy)
--Enemy:GetFrameSize(enemy)
--Enemy:GetCurrentFrame(enemy)
--Enemy:GetSheetSize(enemy)
--Enemy:GetCollisionOffset(enemy)
--Enemy:GetSpeed(enemy)
--Enemy:GetMillisecondsPerFrame(enemy)
--Enemy:GetVisionRadius(enemy)
--Enemy:GetAttackRadius(enemy)
--Enemy:GetJumpRadius(enemy)
--Enemy:GetCollisionRectangle(enemy)
--Enemy:GetHealth(enemy)
--Enemy:GetDefence(enemy)
--Enemy:GetAttack(enemy)