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
    //Первый - задний фон движется относительно движения персонажа
    //Второй - остаётся на месте. Пример - "Конец Игры"

    /// <summary>
    /// Special element that represents "easter eggs"
    /// </summary>
    class GameScreenSpecial
    {
        #region Variables
        public Texture2D screen;
        public Vector2 position;
        public Point frameSize;
        public float opacity = 0;
        public Point secretPoint;
        public int pointOffset;
        #endregion
    }

    /// <summary>
    /// Contains texture and draw rectangle
    /// </summary>
    struct ScreenPart
    {
        Texture2D texture;
        Vector2 position;
        Rectangle view;
        public ScreenPart (Texture2D texture, Rectangle view, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
            this.view = view;
        }

        public Texture2D @Texture
        {
            get
            {
                return texture;
            }
        }

        public Rectangle View
        {
            get
            {
                return view;
            }
            set
            {
                if (value.X < 0) value.X = 0;
                if (value.Y < 0) value.Y = 0;
                view = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }
    }

    /// <summary>
    /// Base type for game screen
    /// </summary>
    public class GameScreen
    {
        #region Variables
        InputManager inputManager = new InputManager();

        ScreenType type;
        ScreenPart[] screen;
        Point frameSize = new Point(0, 0);
        Point currentFrame = new Point(0, 0);
        Point sheetSize = new Point(0, 0);
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame;

        Nullable<Point> textureStart; 

        int millisecondsTillInvisible = 3000;
        int milliseconds = 0;
        int delay = 1500;

        //Часть картинки, которая отрисовывается

        MouseState mouseState;
        MouseState prevMouseState;

        List<GameScreen> additionalScreens = new List<GameScreen>();

        //Опять пасхалки и секретки ^^
        List<GameScreenSpecial> screenSpecialsList = new List<GameScreenSpecial>();
        #endregion

        #region Constructors
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Type of screen. Static or movable</param>
        /// <param name="screen">Massive of textures</param>
        /// <param name="frameSize">Frame size of animation</param>
        /// <param name="currentFrame">Current frame of animation</param>
        /// <param name="sheetSize">Sheet size of animation [in frames]</param>
        /// <param name="timeSinceLastFrame">Time since last animation frame</param>
        /// <param name="millisecondsPerFrame">Milliseconds per animation frames</param>
        public GameScreen(ScreenType type, Texture2D[] screen, Point frameSize, Point currentFrame,
            Point sheetSize, int timeSinceLastFrame, int millisecondsPerFrame)
        {
            this.type = type;
            this.screen = new ScreenPart[screen.Length];
            for (int i = 0; i<screen.Length; i++)
            {
                this.screen[i] = new ScreenPart(screen[i], new Rectangle(0, screen[i].Height - 600, 800, 600), new Vector2(80, 60));
            }
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.timeSinceLastFrame = timeSinceLastFrame;
            this.millisecondsPerFrame = millisecondsPerFrame;
            mouseState = Mouse.GetState();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">Type of screen. Static or movable</param>
        /// <param name="screen">Massive of textures</param>
        /// <param name="textureStart">Variable for multy-texture sheets. Declares the start position of texture in sheet. Null, if we use the hole texture.</param>
        /// <param name="frameSize">Frame size of animation</param>
        /// <param name="currentFrame">Current frame of animation</param>
        /// <param name="sheetSize">Sheet size of animation [in frames]</param>
        /// <param name="timeSinceLastFrame">Time since last animation frame</param>
        /// <param name="millisecondsPerFrame">Milliseconds per animation frames</param>
        public GameScreen(ScreenType type, Texture2D[] screen, Vector2 position, Point textureStart, Point frameSize, Point currentFrame,
    Point sheetSize, int timeSinceLastFrame, int millisecondsPerFrame)
        {
            this.type = type;
            this.screen = new ScreenPart[screen.Length];
            for (int i = 0; i < screen.Length; i++)
            {
                this.screen[i] = new ScreenPart(screen[i], new Rectangle(0, screen[i].Height - 600, 800, 600), position);
            }
            this.textureStart = textureStart;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.timeSinceLastFrame = timeSinceLastFrame;
            this.millisecondsPerFrame = millisecondsPerFrame;
            mouseState = Mouse.GetState();
        }

        #endregion

        #region Methods
        public void Update(GameTime gameTime, Vector2 direction)
        {
            delay -= gameTime.ElapsedGameTime.Milliseconds;
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            if (delay < 0)
            {
                switch (type)
                {
                    #region Static
                    case ScreenType.Static:
                        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

                        if (timeSinceLastFrame > millisecondsPerFrame)
                        {
                            timeSinceLastFrame -= millisecondsPerFrame;
                            ++currentFrame.X;
                            if (textureStart.HasValue)
                            {
                                if(currentFrame.X >= sheetSize.X)
                                {
                                    currentFrame.X = textureStart.Value.X;
                                    ++currentFrame.Y;

                                    if(currentFrame.Y >= sheetSize.Y)
                                    {
                                        currentFrame.Y = textureStart.Value.Y;
                                    }
                                }
                            }
                            else
                            {
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

                        //Логика для секреток
                        foreach (GameScreenSpecial special in screenSpecialsList)
                        {
                            if (Math.Abs(mouseState.X - special.secretPoint.X) <= special.pointOffset && Math.Abs(mouseState.Y - special.secretPoint.Y) <= special.pointOffset)
                            {
                                if (Math.Sqrt(Math.Pow(mouseState.X - special.secretPoint.X, 2) + Math.Pow(mouseState.Y - special.secretPoint.Y, 2)) < Math.Sqrt(Math.Pow(prevMouseState.X - special.secretPoint.X, 2) + Math.Pow(prevMouseState.Y - special.secretPoint.Y, 2)))
                                {
                                    special.opacity += 0.05f;
                                    if (special.opacity > 1.0f)
                                    {
                                        special.opacity = 1.0f;
                                    }
                                }

                                else if (Math.Sqrt(Math.Pow(mouseState.X - special.secretPoint.X, 2) + Math.Pow(mouseState.Y - special.secretPoint.Y, 2)) > Math.Sqrt(Math.Pow(prevMouseState.X - special.secretPoint.X, 2) + Math.Pow(prevMouseState.Y - special.secretPoint.Y, 2)))
                                {
                                    special.opacity -= 0.05f;
                                    if (special.opacity < 0.0f)
                                    {
                                        special.opacity = 0.0f;
                                    }
                                }
                                else
                                {
                                    milliseconds += gameTime.ElapsedGameTime.Milliseconds;
                                    if (milliseconds >= millisecondsTillInvisible)
                                    {
                                        special.opacity -= 0.05f;
                                        if (special.opacity < 0.0f)
                                        {
                                            special.opacity = 0.0f;
                                            milliseconds = 0;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                special.opacity -= 0.05f;
                                if (special.opacity < 0.0f)
                                {
                                    special.opacity = 0.0f;
                                    milliseconds = 0;
                                }
                            }
                        }
                        break;
                    #endregion

                    #region Movable
                    case ScreenType.Movable:
                        for (int i = 0; i< screen.Length; i++)
                        {
                            Vector2 dir = direction / (i+1);
                            int x = screen[i].View.X + (int)direction.X;
                            int y = screen[i].View.Y + (int)direction.Y;

                            if (x >= frameSize.X - 640)
                            {
                                x = frameSize.X - 640;
                            }
                            else if (x <= 0)
                            {
                                x = 0;
                            }
                            if (y >= frameSize.Y - 480)
                            {
                                y = frameSize.Y - 480;
                            }
                            else if (y <= 0)
                            {
                                y = 0;
                            }

                            screen[i].View = new Rectangle(x, y, screen[i].View.Width, screen[i].View.Height);
                        }
                        break;
                    #endregion
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            switch (type)
            {
                #region Static
                case ScreenType.Static:
                        foreach (ScreenPart screen in screen)
                        {
                            spriteBatch.Draw(screen.Texture, screen.Position,
                        new Rectangle((currentFrame.X * frameSize.X),
                       (currentFrame.Y * frameSize.Y), frameSize.X, frameSize.Y),
                        Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.0f);
                        }
                        if(additionalScreens.Count != 0) foreach(GameScreen screen in additionalScreens)
                        {
                            screen.Draw(spriteBatch, gameTime);
                        }
                        foreach (GameScreenSpecial spec in screenSpecialsList)
                        {
                            spriteBatch.Draw(spec.screen, spec.position, Color.White * spec.opacity);
                        }
                    break;
                #endregion

                #region Movable
                case ScreenType.Movable:
                    foreach (ScreenPart screen in screen)
                    {
                        spriteBatch.Draw(screen.Texture, Vector2.Zero, screen.View, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.0f);
                    }
                    if (additionalScreens.Count != 0) foreach (GameScreen screen in additionalScreens)
                        {
                            screen.Draw(spriteBatch, gameTime);
                        }
                    break;
                    #endregion
            }

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime, float scale)
        {
            switch (type)
            {
                #region Static
                case ScreenType.Static:
                    foreach (ScreenPart screen in screen)
                    {
                        spriteBatch.Draw(screen.Texture, screen.Position,
                    new Rectangle((currentFrame.X * frameSize.X),
                   (currentFrame.Y * frameSize.Y), frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
                    }
                    if (additionalScreens.Count != 0) foreach (GameScreen screen in additionalScreens)
                        {
                            screen.Draw(spriteBatch, gameTime);
                        }
                    foreach (GameScreenSpecial spec in screenSpecialsList)
                    {
                        spriteBatch.Draw(spec.screen, spec.position, Color.White * spec.opacity);
                    }
                    break;
                #endregion

                #region Movable
                case ScreenType.Movable:
                    foreach (ScreenPart screen in screen)
                    {
                        spriteBatch.Draw(screen.Texture, Vector2.Zero, screen.View, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
                    }
                    if (additionalScreens.Count != 0) foreach (GameScreen screen in additionalScreens)
                        {
                            screen.Draw(spriteBatch, gameTime);
                        }
                    break;
                    #endregion
            }
        }

        /// <summary>
        /// Adds additional screens to current screen 
        /// </summary>
        /// <param name="gameScreen">Additional game screne to be added</param>
        /// <param name="isFirst">Determines wheter this screen should be drawn exactly after main screen, or in order with other. Drawn in order by default</param>
        public void Add(GameScreen gameScreen, bool isFirst = false)
        {
            if (isFirst)
            {
                additionalScreens.Insert(0, gameScreen);
            }
            else
            {
                additionalScreens.Add(gameScreen);
            }
        }
        
        /// <summary>
        /// Adds specials to game screen
        /// </summary>
        /// <param name="screen">Game special animation texture</param>
        /// <param name="position">Position of specila</param>
        /// <param name="frameSize">Frame size of special animation</param>
        /// <param name="secretPoint">Trigger point of special</param>
        /// <param name="offset">Offset of trigger point</param>
        public void AddSpecial(Texture2D screen, Vector2 position, Point frameSize, Point secretPoint, int offset)
        {
            GameScreenSpecial gSS = new GameScreenSpecial();
            gSS.screen = screen;
            gSS.position = position;
            gSS.frameSize = frameSize;
            gSS.secretPoint = secretPoint;
            gSS.pointOffset = offset;
            gSS.opacity = 0.0f;
            screenSpecialsList.Add(gSS);
        }
        #endregion

        #region Properties
        public Point FrameSize
        {
            get
            {
                return frameSize;
            }
        }
        #endregion
    }
}
