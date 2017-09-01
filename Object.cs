using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamCatcher
{
    public class Object
    {
        #region Variables
        InputManager inputManager = new InputManager();

        protected string ID;
        protected Texture2D spriteSheet;
        protected Texture2D iconSprite;
        protected Point frameSize;
        protected Point currentFrame;
        protected Point sheetSize;
        protected int collisionOffset;
        protected int timeSinceLastFrame = 0;
        protected int millisecondsPerFrame;
        protected const int defaultMillisecorndsPerFrame = 16;
        protected Vector2 position;
        #endregion

        #region Constructors
        public Object(string ID, Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize)
            : this(ID, textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, defaultMillisecorndsPerFrame)
        {

        }

        public Object(string ID, Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, int millisecondsPerFrame)
        {
            spriteSheet = textureImage;
            //iconSprite = MainClass.Load<Texture2D>((@"Images\Icons\" + ID).ToString());
            this.ID = ID;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public Object(ObjectsID ID, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, int millisecondsPerFrame)
        {
            switch (ID)
            {
                case ObjectsID.Stoneberry:
                    break;
                case ObjectsID.Green_Potion:
                    spriteSheet = MainClass.Load<Texture2D>(@"Images\Objects\Potions\Green Bottle");
                    break;
                case ObjectsID.Blue_Potion:
                    spriteSheet = MainClass.Load<Texture2D>(@"Images\Objects\Potions\Blue Bottle");
                    break;
                case ObjectsID.Crystal:
                    spriteSheet = MainClass.Load<Texture2D>("");
                    break;
                case ObjectsID.Lantern:
                    spriteSheet = MainClass.Load<Texture2D>(@"Images\Objects\Lanterns\LanternAnimationV2");
                    break;
                case ObjectsID.Broken_Lantern:
                    spriteSheet = MainClass.Load<Texture2D>("");
                    break;
            }
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public Object(ObjectsID ID, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize) :
            this(ID, position, frameSize, collisionOffset, currentFrame, sheetSize, defaultMillisecorndsPerFrame)
        {

        }

        public Object(SpecialObjectsID ID, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, int millisecondsPerFrame)
        {
            switch (ID)
            {
                case SpecialObjectsID.Shadow_Hunter_Mask:
                case SpecialObjectsID.Shadow_Spider_Mask:
                case SpecialObjectsID.Shadow_Bull_Mask:
                case SpecialObjectsID.Shadow_Boss_Mask:
                    break;
            }
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public Object(SpecialObjectsID ID, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize) :
            this(ID, position, frameSize, collisionOffset, currentFrame, sheetSize, defaultMillisecorndsPerFrame)
        {

        }

        public Object()
        {

        }
        #endregion

        #region Methods
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
            try
            {
                spriteBatch.Draw(spriteSheet, position, new Rectangle((currentFrame.X * frameSize.X),
                    (currentFrame.Y * frameSize.Y), frameSize.X,
                    frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            }
            catch
            {
                spriteBatch.Begin();
                spriteBatch.Draw(spriteSheet, position, new Rectangle((currentFrame.X * frameSize.X),
                    (currentFrame.Y * frameSize.Y), frameSize.X,
                    frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                spriteBatch.End();
            }
        }

        public virtual void DrawIcon(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(iconSprite, position, new Rectangle((currentFrame.X * frameSize.X),
                (currentFrame.Y * frameSize.Y), frameSize.X,
                frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }

        public override string ToString()
        {
            string s = ID + ";" + position.X + "," + position.Y;
            return s;
        }

        //Дичайшая ересь. Я даже не знаю, будет ли работать...
	//<Подтверждаю. Ересь ещё та. Будет заменено на заготовки Lua>
        public static Object Parse(string info)
        {
            string s = "";
            System.IO.StreamReader reader = new System.IO.StreamReader(System.IO.File.OpenRead(@"Content\Files\Objects\" + info.Split(',')[0] + ".dcobj"));
            s = reader.ReadLine();
            reader.Dispose();
            reader.Close();
            Object o = new Object(info.Split(';')[0], MainClass.Load<Texture2D>(s.Split(';')[0]), new Vector2(float.Parse(info.Split(';')[1].Split(',')[0]), float.Parse(info.Split(';')[1].Split(',')[1])), new Point(int.Parse(s.Split(';')[1]), int.Parse(s.Split(';')[2])), int.Parse(s.Split(';')[3]), new Point(int.Parse(s.Split(';')[4]), int.Parse(s.Split(';')[5])), new Point(int.Parse(s.Split(';')[6]), int.Parse(s.Split(';')[7])));
            return o;
        }
        #endregion

        #region Properties
        public Rectangle CollisionRect
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

        public string GetID
        {
            get
            {
                return ID;
            }
        }

        public string GetInfo
        {
            get { return "*nothing"; }
        }
        #endregion
    }
}
