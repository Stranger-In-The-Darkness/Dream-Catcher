using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace DreamCatcher
{
    public class UserControlledSprite : Sprite
    {
        bool isJumping = false;

        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {

        }

        public UserControlledSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {

        }

        override public Vector2 Direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;
                KeyboardState keyboard = Keyboard.GetState();
                if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A))
                {
                    inputDirection.X -= 1;
                }
                if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
                {
                    inputDirection.X += 1;
                }
                if (keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W))
                {
                    inputDirection.Y += 1;
                }
                return inputDirection * speed;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += Direction;

            if (position.X < 0)
            {
                position.X = 0;
            }

            if (position.Y < 0)
            {
                position.Y = 0;
            }

            if (position.X > clientBounds.Width - frameSize.X)
            {
                position.X = clientBounds.Width - frameSize.X;
            }

            if (position.Y > clientBounds.Height - frameSize.Y)
            {
                position.X = clientBounds.Height - frameSize.Y;
            }
            base.Update(gameTime, clientBounds);
        }

        public Vector2 GetCenter
        {
            get
            {
                return new Vector2(position.X + frameSize.X/2, position.Y + frameSize.Y/2);
            }
        }
    }
}
