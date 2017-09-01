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
    public class Platform
    {
        #region Variables
        Texture2D platformSpriteSheet;
        Point platformSheetSize;
        Point platformFrameSize;
        Point platformCurrentFrame;
        Vector2 platformPosition;
        //int platformOffset = 1;
        int millisecondsPerFrame = 16;
        int timeSinceLastFrame = 0;

        bool isMaterialized = false;
        bool isActivated = false;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture">Platform sprite sheet</param>
        /// <param name="position">Platform position</param>
        /// <param name="sheetSize">Platform animation sheet size [in frames]</param>
        /// <param name="frameSize">Platform animation frame size</param>
        /// <param name="currentFrame">Platform animation current frame</param>
        public Platform(Texture2D texture, Vector2 position, Point sheetSize, Point frameSize,
            Point currentFrame)
        {
            platformSpriteSheet = texture;
            platformPosition = position;
            platformSheetSize = sheetSize;
            platformFrameSize = frameSize;
            platformCurrentFrame = currentFrame;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture">Platform sprite sheet</param>
        /// <param name="position">Platform position</param>
        /// <param name="sheetSize">Platform animation sheet size [in frames]</param>
        /// <param name="frameSize">Platform animation frame size</param>
        /// <param name="currentFrame">Platform animation current frame</param>
        /// <param name="millisecondsPerFrame">Milliseconda per animation frame</param>
        public Platform(Texture2D texture, Vector2 position, Point sheetSize, Point frameSize,
    Point currentFrame, int millisecondsPerFrame)
        {
            platformSpriteSheet = texture;
            platformPosition = position;
            platformSheetSize = sheetSize;
            platformFrameSize = frameSize;
            platformCurrentFrame = currentFrame;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture">Platform sprite sheet</param>
        /// <param name="position">Platform position</param>
        /// <param name="sheetSize">Platform animation sheet size [in frames]</param>
        /// <param name="frameSize">Platform animation frame size</param>
        /// <param name="currentFrame">Platform animation current frame</param>
        /// <param name="isMaterialized">Determnes whether platform is materialized? and could be interacted with, or not</param>
        public Platform(Texture2D texture, Vector2 position, Point sheetSize, Point frameSize,
Point currentFrame, bool isMaterialized)
        {
            platformSpriteSheet = texture;
            platformPosition = position;
            platformSheetSize = sheetSize;
            platformFrameSize = frameSize;
            platformCurrentFrame = currentFrame;
            this.isMaterialized = isMaterialized;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="p">Platform object</param>
        public Platform(Platform p)
        {
            platformPosition = new Point(p.CollisionRect.X, p.CollisionRect.Y).ToVector2();
            platformFrameSize = new Point(p.CollisionRect.Width, p.CollisionRect.Height);
            isMaterialized = true;
        }
        #endregion

        #region Methods
        public void Update(GameTime time)
        {
            if (isActivated)
            {
                isMaterialized = true;
                timeSinceLastFrame += time.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame >= millisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    platformCurrentFrame.X++;
                    if (platformCurrentFrame.X >= platformSheetSize.X)
                    {
                        platformCurrentFrame.X = 0;
                        platformCurrentFrame.Y++;
                        if (platformCurrentFrame.Y >= platformSheetSize.Y)
                        {
                            platformCurrentFrame.X = platformSheetSize.X - 1;
                            platformCurrentFrame.Y = platformSheetSize.Y - 1;
                            isActivated = false;
                        }
                    }
                }
            }
        }

        public void Draw(GameTime time, SpriteBatch spriteBatch)
        {
            try
            {
                spriteBatch.Draw(platformSpriteSheet, platformPosition, new Rectangle((platformCurrentFrame.X * platformFrameSize.X),
                    (platformCurrentFrame.Y * platformFrameSize.Y), platformFrameSize.X,
                    platformFrameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            catch
            {
                spriteBatch.Begin();
                spriteBatch.Draw(platformSpriteSheet, platformPosition, new Rectangle((platformCurrentFrame.X * platformFrameSize.X),
    (platformCurrentFrame.Y * platformFrameSize.Y), platformFrameSize.X,
    platformFrameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.End();
            }
        }

        public void Draw(GameTime time, Vector2 drawFramePosition, SpriteBatch spriteBatch)
        {
            try
            {
                spriteBatch.Draw(platformSpriteSheet, new Vector2(platformPosition.X - drawFramePosition.X, platformPosition.Y - drawFramePosition.Y), new Rectangle((platformCurrentFrame.X * platformFrameSize.X),
                    (platformCurrentFrame.Y * platformFrameSize.Y), platformFrameSize.X,
                    platformFrameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            catch
            {
                spriteBatch.Begin();
                spriteBatch.Draw(platformSpriteSheet, new Vector2(platformPosition.X - drawFramePosition.X, platformPosition.Y - drawFramePosition.Y), new Rectangle((platformCurrentFrame.X * platformFrameSize.X),
    (platformCurrentFrame.Y * platformFrameSize.Y), platformFrameSize.X,
    platformFrameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

                spriteBatch.End();
            }
        }

        public void Activate()
        {
            isActivated = true;
        }

        public override string ToString()
        {
            return platformPosition.X + " " + platformPosition.Y;
        }
        #endregion

        #region Properties
        public Rectangle CollisionRect
        {
            get
            {
                if (isMaterialized) return new Rectangle((int)platformPosition.X - 1, (int)platformPosition.Y, platformFrameSize.X + 1, platformFrameSize.Y);
                else return new Rectangle(0, 0, 0, 0);
            }
        }

        public Vector2 GetCenter
        {
            get
            {
                return new Vector2(platformPosition.X + platformSheetSize.X / 2, platformPosition.Y + platformSheetSize.Y / 2);
            }
        }

        public bool IsMaterial
        {
            get
            {
                return isMaterialized;
            }
        }
        #endregion
    }
}
