using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DreamCatcher
{
    public class Bullet
    {
        #region Variables
        Texture2D textureImage;
        Point frameSize;
        Point currentFrame;
        Point sheetSize;
        int collisionOffset;
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame;
        const int defaultMillisecorndsPerFrame = 16;
        Vector2 speed;
        Vector2 position;
        public bool explode = false;
        #endregion

        #region Constructors
        public Bullet(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
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
        public Bullet(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed)
            : this(textureImage, position,frameSize, collisionOffset, currentFrame, sheetSize, speed, defaultMillisecorndsPerFrame)
        {

        }
        #endregion

        #region Methods
        public void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position.X += speed.X * gameTime.ElapsedGameTime.Milliseconds;
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
                        explode = true;
                        currentFrame.Y = 0;
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position, new Rectangle((currentFrame.X * frameSize.X),
                (currentFrame.Y * frameSize.Y), frameSize.X,
                frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public void hit(Rectangle target)
        {
            if(target.Intersects(new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X - (2 * collisionOffset),
                    frameSize.Y - (2 * collisionOffset))))
            {
                explode = true;
            }
        }
        #endregion
    }
}
