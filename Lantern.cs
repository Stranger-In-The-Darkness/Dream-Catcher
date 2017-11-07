using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DreamCatcher
{
    public class Lantern : Object
    {
        #region Variables
        static List<Lantern> lanternList = new List<Lantern>();

        bool isActive = false; //Работает фонарик, или нет
        bool activation = false; //Переменная отвечает за "включение/выключение" фонариков
        int health = 3;
        #endregion

        #region Constructors
        public Lantern(Texture2D spriteSheet, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, string ID = "lantern"):
            base(ID, spriteSheet, position, frameSize, collisionOffset, currentFrame, sheetSize)
        {
            lanternList.Add(this);
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (activation)
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
                            currentFrame.Y = sheetSize.Y/2 - 1;
                            currentFrame.X = sheetSize.X/2 - 1;
                            activation = false;
                        }
                    }
                }
            }
            else if(!isActive && currentFrame != Point.Zero)
            {
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

                if (timeSinceLastFrame > millisecondsPerFrame)
                {
                    timeSinceLastFrame -= millisecondsPerFrame;
                    --currentFrame.X;

                    if (currentFrame.X < 0)
                    {
                        currentFrame.X = sheetSize.X - 1;
                        --currentFrame.Y;

                        if (currentFrame.Y < 0)
                        {
                            currentFrame.Y = 0;
                            currentFrame.X = 0;
                        }
                    }
                }
            }
        }

        public void Draw(GameTime time, Vector2 drawFramePosition, SpriteBatch spriteBatch)
        {
            try
            {
                spriteBatch.Draw(spriteSheet, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y), new Rectangle((currentFrame.X * frameSize.X),
                    (currentFrame.Y * frameSize.Y), frameSize.X,
                    frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
            catch
            {
                spriteBatch.Begin();
                spriteBatch.Draw(spriteSheet, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y), new Rectangle((currentFrame.X * frameSize.X),
                    (currentFrame.Y * frameSize.Y), frameSize.X,
                    frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.End();
            }
        }

        /// <summary>
        /// Deletes all of the current lanterns. Use wisely!
        /// </summary>
        public static void Clear()
        {
            lanternList.Clear();
        }

        public void Activate()
        {
            if (!Active)
            {
                Active = true;
                activation = true;
            }
        }

        public bool Attack()
        {
            if (health > 0)
            {
                health--;
                return false;
            }
            currentFrame.X = 0;
            currentFrame.Y = 0;
            isActive = false;
            return true;
        }

        public void Charge()
        {
            health++;
            if (health > 3)
            {
                health = 3;
            }
        }

        public override string ToString()
        {
            string s = position.X + "," + position.Y + ";" + isActive;
            return s;
        }

        public bool CollisionCheck (Rectangle rectangle)
        {
            if (CollisionRect.Intersects(rectangle))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Properties
        public bool Active
        {
            get { return isActive; }
            private set { isActive = value; }
        }

        public static List<Lantern> GetLanterns
        {
            get
            {
                return lanternList;
            }
        }
        #endregion
    }
}
