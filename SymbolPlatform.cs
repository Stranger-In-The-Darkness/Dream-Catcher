using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace DreamCatcher
{
    public class SymbolPlatform: Platform
    {
        Texture2D spriteSheet;
        Vector2 position;
        Point sheetSize;
        Point frameSize;
        Point currentFrame;
        int symbolPlatformOffset = 1;
        int millisecondsPerFrame = 15;
        int timeSinceLastFrame = 0;

        bool isActivated = false;

        public SymbolPlatform(Texture2D spriteSheet, Vector2 position, Point sheetSize, Point frameSize, Point currentFrame) :
            base(spriteSheet, position, sheetSize, frameSize, currentFrame)
        {
            this.spriteSheet = spriteSheet;
            this.position = position;
            this.sheetSize = sheetSize;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
        }

        new public void Update(GameTime time)
        {
            if (isActivated)
            {
                timeSinceLastFrame += time.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame >= millisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    currentFrame.X++;
                    if (currentFrame.X > sheetSize.X)
                    {
                        currentFrame.X = sheetSize.X;
                    }
                }
            }
        }

        new public void Draw(GameTime time, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(spriteSheet, position, new Rectangle((currentFrame.X * frameSize.X),
                            (currentFrame.Y * frameSize.Y), frameSize.X,
                            frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        new public Rectangle CollisionRect
        {
            get
            {
                return new Rectangle((int)position.X + symbolPlatformOffset, (int)position.Y + symbolPlatformOffset, frameSize.X, frameSize.Y);
            }
        }

        new public Vector2 GetCenter
        {
            get { return new Vector2(position.X + frameSize.X / 2, position.Y + frameSize.Y / 2); }
        }

        new public void Activate()
        {
            isActivated = true;
        }

        public bool IsActive
        {
            get
            {
                if (isActivated)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
