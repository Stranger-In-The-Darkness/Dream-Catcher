using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;

namespace DreamCatcher
{
    public abstract class Sprite : IDisposable
    {
        protected Texture2D textureImage;
        protected Point frameSize;
        protected Point currentFrame;
        protected Point sheetSize;
        protected int collisionOffset;
        protected int timeSinceLastFrame = 0;
        protected int millisecondsPerFrame;
        protected const int defaultMillisecorndsPerFrame = 16;
        protected Vector2 speed;
        protected Vector2 position;

        public abstract Vector2 Direction
        {
            get;
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, defaultMillisecorndsPerFrame)
        {

        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame -= millisecondsPerFrame;
                ++currentFrame.X;

                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;

                    if (currentFrame.Y >= sheetSize.Y)
                    {
                        currentFrame.Y = 0;
                    }
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position, new Rectangle((currentFrame.X * frameSize.X),
                (currentFrame.Y * frameSize.Y), frameSize.X,
                frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public virtual void Draw(GameTime gameTime, Vector2 drawFramePosition, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y), new Rectangle((currentFrame.X * frameSize.X),
                (currentFrame.Y * frameSize.Y), frameSize.X,
                frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public void Dispose()
        {
            if(!this.textureImage.IsDisposed) this.textureImage.Dispose();
        }

        public virtual Rectangle CollisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X - (2 * collisionOffset),
                    frameSize.Y - (2 * collisionOffset));
            }
        }
    }
}
