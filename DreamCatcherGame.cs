using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System;
using System.IO;
using System.Threading;

namespace DreamCatcher
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 

    public class DreamCatcherGame : Game
    {
        //Variables list
        #region Variables

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Manager's
        SymbolManager symbolManager;

        LevelManager levelManager;

        //TextManager textManager;

        InputManager inputManager = new InputManager();
        #endregion

        GameState prevState = GameState.Menu;
        GameState state = GameState.Menu;

        #region Audio
        bool muteAudio = false;

        Song forestMusic;

        SoundEffect buttonClick;
        #endregion

        #region Pointer
        Texture2D Pointer;
        Vector2 pointerPosition = Vector2.Zero;
        Point pointerFrameSize = new Point(47, 64);
        int pointerOffset = 5;
        #endregion

        #region Buttons
        Texture2D playButton;
        Vector2 playButtonPosition = new Vector2();
        Point playButtonFrameSize = new Point(136, 106);
        int playButtonOffset = 10;
        float playButtonOpacity = 0.0f;
        bool playButtonIsPressed = false;

        Texture2D optionsButton;
        Vector2 optionsButtonPosition = new Vector2();
        Point optionsButtonFrameSize = new Point(136, 106);
        int optionsButtonOffset = 10;
        float optionsButtonOpacity = 0.0f;
        bool optionsButtonIsPressed = false;

        Texture2D exitButton;
        Vector2 exitButtonPosition = new Vector2();
        Point exitButtonFrameSize = new Point(136, 106);
        int exitButtonOffset = 10;
        float exitButtonOpacity = 0.0f;
        bool exitButtonIsPressed = false;

        Texture2D applyButton;
        Vector2 applyButtonPosition = new Vector2();
        Point applyButtonFrameSize = new Point(136, 106);
        int applyButtonOffset = 10;
        float applyButtonOpacity = 0.0f;
        bool applyButtonIsPressed = false;

        Texture2D resumeButton;
        Vector2 resumeButtonPosition = new Vector2();
        Point resumeButtonFrameSize = new Point(136, 106);
        int resumeButtonOffset = 10;
        float resumeButtonOpacity = 0.0f;
        bool resumeButtonIsPressed = false;

        Texture2D quitButton;
        Vector2 quitButtonPosition = new Vector2();
        Point quitButtonFrameSize = new Point(136, 106);
        int quitButtonOffset = 10;
        float quitButtonOpacity = 0.0f;
        bool quitButtonIsPressed = false;
        #endregion

        GameScreen loading;
        GameScreen options;
        GameScreen gameOver;

        MouseState prevMouseState;

        Player player;

        float targetOpacity = 0.0f;
        #endregion

        #region Constructors
        public DreamCatcherGame()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.HiDef,

                PreferredBackBufferWidth = 800, // ширина приложения
                PreferredBackBufferHeight = 600, // высота приложения
                IsFullScreen = false // флаг полноэкранного приложения
            };
            graphics.ApplyChanges(); // применяем параметры

            Content.RootDirectory = "Content";
        }
        #endregion

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            MainClass.Game = this;

            player = new Player(Content.Load<Texture2D>(@"Images\Player\OwlAnimation"), new Vector2(0, 739), new Point(101, 130), 10, new Point(0, 0), new Point(8, 6), new Vector2(1.6f, 0), 70, new Rectangle(20, 0, 60, 130));

            #region Managers
            symbolManager = new SymbolManager(this, new Rectangle((int)playButtonPosition.X, (int)playButtonPosition.Y, (int)quitButtonPosition.X + quitButtonFrameSize.X, (int)quitButtonPosition.Y + quitButtonFrameSize.Y));
            levelManager = new LevelManager(this, player);
            //textManager = new TextManager(this);

            Components.Add(symbolManager);
            //Components.Add(textManager);
            base.Initialize();
            #endregion

            #region Buttons
            playButtonPosition = new Vector2((Window.ClientBounds.Width - playButtonFrameSize.X) / 2, Window.ClientBounds.Height / 2 - playButtonFrameSize.Y - 50);
            optionsButtonPosition = new Vector2((Window.ClientBounds.Width - optionsButtonFrameSize.X) / 2, (Window.ClientBounds.Height - optionsButtonFrameSize.Y) / 2);
            exitButtonPosition = new Vector2((Window.ClientBounds.Width - exitButtonFrameSize.X) / 2, (Window.ClientBounds.Height + optionsButtonFrameSize.Y) / 2);
            resumeButtonPosition = new Vector2((Window.ClientBounds.Width - resumeButtonFrameSize.X) / 2, Window.ClientBounds.Height / 2 - resumeButtonFrameSize.Y - 50);
            optionsButtonPosition = new Vector2((Window.ClientBounds.Width - optionsButtonFrameSize.X) / 2, (Window.ClientBounds.Height - optionsButtonFrameSize.Y) / 2);
            quitButtonPosition = new Vector2((Window.ClientBounds.Width - quitButtonFrameSize.X) / 2, (Window.ClientBounds.Height + optionsButtonFrameSize.Y) / 2);
            #endregion


            loading = new GameScreen(ScreenType.Static, new Texture2D[] { Content.Load<Texture2D>(@"Images\Screens\LoadingScreen") }, new Vector2(700, 500), new Point(6, 0), new Point(300, 300), new Point(6, 0), new Point(3, 6), 0, 60);

            gameOver = new GameScreen(ScreenType.Static, new Texture2D[] { Content.Load<Texture2D>(@"Images\Screens\GameOverScreen") }, new Point(640, 480), new Point(2, 0), new Point(4, 2), 0, 100);
            gameOver.AddSpecial(Content.Load<Texture2D>(@"Images\Screens\GameOverScreenSpecial"), new Vector2(80, 60), new Point(640, 480), new Point(430, 295), 30);

            if (!MediaPlayer.GameHasControl)
            {
                MediaPlayer.Stop();
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Thread loadSettings = new Thread(new ThreadStart(FileManager.LoadSetting))
            {
                IsBackground = true
            };
            loadSettings.Start();

            // TODO: use this.Content to load your game content here
            Pointer = Content.Load<Texture2D>(@"Images\PointerV3");
            playButton = Content.Load<Texture2D>(@"Images\PlayButtonV2");
            optionsButton = Content.Load<Texture2D>(@"Images\OptionsButtonV2");
            exitButton = Content.Load<Texture2D>(@"Images\ExitButtonV2");
            applyButton = Content.Load<Texture2D>(@"Images\ApplyButton");
            resumeButton = Content.Load<Texture2D>(@"Images\ResumeButton");
            quitButton = Content.Load<Texture2D>(@"Images\QuitButton");

            //forestMusic = Content.Load<Song>(@"Audio\Music\NightForest");
            //MediaPlayer.Play(forestMusic);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            inputManager.Update();

            targetOpacity += 0.17f;
            if (targetOpacity > 1) targetOpacity = 1;

            switch (state)
            {
                #region MainMenu
                case GameState.Menu:
                    {
                        MouseState mouseState = Mouse.GetState();
                        #region Buttons_Opacity
                        if (System.Math.Abs(mouseState.X - (playButtonPosition.X + playButtonFrameSize.X / 2)) <= 150 && System.Math.Abs(mouseState.Y - (playButtonPosition.Y + playButtonFrameSize.Y / 2)) <= 150)
                        {
                            if (System.Math.Sqrt(System.Math.Pow(mouseState.X - (playButtonPosition.X + playButtonFrameSize.X / 2), 2) + System.Math.Pow(mouseState.Y - (playButtonPosition.Y + playButtonFrameSize.Y / 2), 2)) < System.Math.Sqrt(System.Math.Pow(prevMouseState.X - (playButtonPosition.X + playButtonFrameSize.X / 2), 2) + System.Math.Pow(prevMouseState.Y - (playButtonPosition.Y + playButtonFrameSize.Y / 2), 2)))
                            {
                                playButtonOpacity += 0.05f;
                                if (playButtonOpacity > 1.0f)
                                {
                                    playButtonOpacity = 1.0f;
                                }
                            }
                            else if (System.Math.Sqrt(System.Math.Pow(mouseState.X - (playButtonPosition.X + playButtonFrameSize.X / 2), 2) + System.Math.Pow(mouseState.Y - (playButtonPosition.Y + playButtonFrameSize.Y / 2), 2)) > System.Math.Sqrt(System.Math.Pow(prevMouseState.X - (playButtonPosition.X + playButtonFrameSize.X / 2), 2) + System.Math.Pow(prevMouseState.Y - (playButtonPosition.Y + playButtonFrameSize.Y / 2), 2)))
                            {
                                playButtonOpacity -= 0.05f;
                                if (playButtonOpacity < 0.0f)
                                {
                                    playButtonOpacity = 0.0f;
                                }
                            }
                        }
                        else
                        {
                            playButtonOpacity -= 0.05f;
                            if (playButtonOpacity < 0.0f)
                            {
                                playButtonOpacity = 0.0f;
                                playButtonIsPressed = false;
                            }
                        }
                        if (System.Math.Abs(mouseState.X - (optionsButtonPosition.X + optionsButtonFrameSize.X / 2)) <= 150 && System.Math.Abs(mouseState.Y - (optionsButtonPosition.Y + optionsButtonFrameSize.Y / 2)) <= 150)
                        {
                            if (System.Math.Sqrt(System.Math.Pow(mouseState.X - (optionsButtonPosition.X + optionsButtonFrameSize.X / 2), 2) + System.Math.Pow(mouseState.Y - (optionsButtonPosition.Y + optionsButtonFrameSize.Y / 2), 2)) < System.Math.Sqrt(System.Math.Pow(prevMouseState.X - (optionsButtonPosition.X + optionsButtonFrameSize.X / 2), 2) + System.Math.Pow(prevMouseState.Y - (optionsButtonPosition.Y + optionsButtonFrameSize.Y / 2), 2)))
                            {
                                optionsButtonOpacity += 0.05f;
                                if (optionsButtonOpacity > 1.0f)
                                {
                                    optionsButtonOpacity = 1.0f;
                                }
                            }
                            else if (System.Math.Sqrt(System.Math.Pow(mouseState.X - (optionsButtonPosition.X + optionsButtonFrameSize.X / 2), 2) + System.Math.Pow(mouseState.Y - (optionsButtonPosition.Y + optionsButtonFrameSize.Y / 2), 2)) > System.Math.Sqrt(System.Math.Pow(prevMouseState.X - (optionsButtonPosition.X + optionsButtonFrameSize.X / 2), 2) + System.Math.Pow(prevMouseState.Y - (optionsButtonPosition.Y + optionsButtonFrameSize.Y / 2), 2)))
                            {
                                optionsButtonOpacity -= 0.05f;
                                if (optionsButtonOpacity < 0.0f)
                                {
                                    optionsButtonOpacity = 0.0f;
                                }
                            }
                        }
                        else
                        {
                            optionsButtonOpacity -= 0.05f;
                            if (optionsButtonOpacity < 0.0f)
                            {
                                optionsButtonOpacity = 0.0f;
                                optionsButtonIsPressed = false;
                            }
                        }
                        if (System.Math.Abs(mouseState.X - (exitButtonPosition.X + exitButtonFrameSize.X / 2)) <= 150 && System.Math.Abs(mouseState.Y - (exitButtonPosition.Y + exitButtonFrameSize.Y / 2)) <= 150)
                        {
                            if (System.Math.Sqrt(System.Math.Pow(mouseState.X - (exitButtonPosition.X + exitButtonFrameSize.X / 2), 2) + System.Math.Pow(mouseState.Y - (exitButtonPosition.Y + exitButtonFrameSize.Y / 2), 2)) < System.Math.Sqrt(System.Math.Pow(prevMouseState.X - (exitButtonPosition.X + exitButtonFrameSize.X / 2), 2) + System.Math.Pow(prevMouseState.Y - (exitButtonPosition.Y + exitButtonFrameSize.Y / 2), 2)))
                            {
                                exitButtonOpacity += 0.05f;
                                if (exitButtonOpacity > 1.0f)
                                {
                                    exitButtonOpacity = 1.0f;
                                }
                            }
                            else if (System.Math.Sqrt(System.Math.Pow(mouseState.X - (exitButtonPosition.X + exitButtonFrameSize.X / 2), 2) + System.Math.Pow(mouseState.Y - (exitButtonPosition.Y + exitButtonFrameSize.Y / 2), 2)) > System.Math.Sqrt(System.Math.Pow(prevMouseState.X - (exitButtonPosition.X + exitButtonFrameSize.X / 2), 2) + System.Math.Pow(prevMouseState.Y - (exitButtonPosition.Y + exitButtonFrameSize.Y / 2), 2)))
                            {
                                exitButtonOpacity -= 0.1f;
                                if (exitButtonOpacity < 0.0f)
                                {
                                    exitButtonOpacity = 0.0f;
                                }
                            }
                        }
                        else
                        {
                            exitButtonOpacity -= 0.05f;
                            if (exitButtonOpacity < 0.0f)
                            {
                                exitButtonOpacity = 0.0f;
                                exitButtonIsPressed = false;
                            }
                        }
                        #endregion

                        #region Button_Clicks
                        if (Collide(new Rectangle((int)pointerPosition.X, (int)pointerPosition.Y, pointerFrameSize.X - pointerOffset, pointerFrameSize.Y - pointerOffset),
     new Rectangle((int)playButtonPosition.X + playButtonOffset, (int)playButtonPosition.Y + playButtonOffset, playButtonFrameSize.X - playButtonOffset, playButtonFrameSize.Y - playButtonOffset)))
                        {
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                            {
                                playButtonIsPressed = true;
                            }
                            if(Mouse.GetState().LeftButton == ButtonState.Released)
                            {
                                if (playButtonIsPressed)
                                {
                                    PlayButtonIsClicked();
                                }
                            }
                        }

                        else if (Collide(new Rectangle((int)pointerPosition.X, (int)pointerPosition.Y, pointerFrameSize.X - pointerOffset, pointerFrameSize.Y - pointerOffset),
                            new Rectangle((int)optionsButtonPosition.X + optionsButtonOffset, (int)optionsButtonPosition.Y + optionsButtonOffset, optionsButtonFrameSize.X - optionsButtonOffset, optionsButtonFrameSize.Y - optionsButtonOffset)))
                        {
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                            {
                                optionsButtonIsPressed = true;
                            }
                            if (Mouse.GetState().LeftButton == ButtonState.Released)
                            {
                                if (optionsButtonIsPressed)
                                {
                                    OptionsButtonIsClicked();
                                }
                            }
                        }

                        else if (Collide(new Rectangle((int)pointerPosition.X, (int)pointerPosition.Y, pointerFrameSize.X - pointerOffset, pointerFrameSize.Y - pointerOffset),
                            new Rectangle((int)exitButtonPosition.X + exitButtonOffset, (int)exitButtonPosition.Y + exitButtonOffset, exitButtonFrameSize.X - exitButtonOffset, exitButtonFrameSize.Y - exitButtonOffset)))
                        {
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                            {
                                exitButtonIsPressed = true;
                            }
                            if (Mouse.GetState().LeftButton == ButtonState.Released)
                            {
                                if (exitButtonIsPressed)
                                {
                                    ExitButtonIsClicked();
                                }
                            }
                        }
                        #endregion

                        if (mouseState.X != prevMouseState.X || mouseState.Y != prevMouseState.Y)
                        {
                            pointerPosition = new Vector2(mouseState.X, mouseState.Y);
                            prevMouseState = mouseState;
                        }
                        symbolManager.Update(gameTime);
                    }
                    break;
                #endregion
                #region Help
                case GameState.Help:
                    {
                        MouseState mouseState = Mouse.GetState();
                        if (mouseState.X != prevMouseState.X || mouseState.Y != prevMouseState.Y)
                        {
                            pointerPosition = new Vector2(mouseState.X, mouseState.Y);
                            prevMouseState = mouseState;
                        }
                        if (Keyboard.GetState().GetPressedKeys().Length != 0)
                        {
                            prevState = state;
                            state = GameState.Gameplay;
                            targetOpacity = 0;
                            break;
                        }
                    }
                   
                    break;
                #endregion
                #region Options
                case GameState.Options:
                    {
                        MouseState mouseState = Mouse.GetState();
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        {
                            state = prevState;
                            targetOpacity = 0;
                        }

                        //Закрыть окно ну и так далее
                        //if (Collide(new Rectangle((int)pointerPosition.X, (int)pointerPosition.Y, pointerFrameSize.X - pointerOffset, pointerFrameSize.Y - pointerOffset),
                        //some control ...)
                        //{
                        //    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        //    {
                        //        do something...Draw(GameTime gameTime);
                        //    }
                        //}
                        if (mouseState.X != prevMouseState.X || mouseState.Y != prevMouseState.Y)
                        {
                            pointerPosition = new Vector2(mouseState.X, mouseState.Y);
                            prevMouseState = mouseState;
                        }
                    }
                    break;
                #endregion
                #region Pause
                case GameState.Pause:
                    {
                        MouseState mouseState = Mouse.GetState();
                        //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        //    state = prevState;

                        symbolManager.Update(gameTime);

                        #region Buttons_Opacity
                        if (Math.Abs(mouseState.X - (resumeButtonPosition.X + resumeButtonFrameSize.X / 2)) <= 150 && Math.Abs(mouseState.Y - (resumeButtonPosition.Y + resumeButtonFrameSize.Y / 2)) <= 150)
                        {
                            if (Math.Sqrt(Math.Pow(mouseState.X - (resumeButtonPosition.X + resumeButtonFrameSize.X / 2), 2) + Math.Pow(mouseState.Y - (resumeButtonPosition.Y + resumeButtonFrameSize.Y / 2), 2)) < Math.Sqrt(Math.Pow(prevMouseState.X - (resumeButtonPosition.X + resumeButtonFrameSize.X / 2), 2) + Math.Pow(prevMouseState.Y - (resumeButtonPosition.Y + resumeButtonFrameSize.Y / 2), 2)))
                            {
                                resumeButtonOpacity += 0.05f;
                                if (resumeButtonOpacity > 1.0f)
                                {
                                    resumeButtonOpacity = 1.0f;
                                }
                            }
                            else if (Math.Sqrt(Math.Pow(mouseState.X - (resumeButtonPosition.X + resumeButtonFrameSize.X / 2), 2) + Math.Pow(mouseState.Y - (resumeButtonPosition.Y + resumeButtonFrameSize.Y / 2), 2)) > Math.Sqrt(Math.Pow(prevMouseState.X - (resumeButtonPosition.X + resumeButtonFrameSize.X / 2), 2) + Math.Pow(prevMouseState.Y - (resumeButtonPosition.Y + resumeButtonFrameSize.Y / 2), 2)))
                            {
                                resumeButtonOpacity -= 0.05f;
                                if (resumeButtonOpacity < 0.0f)
                                {
                                    resumeButtonOpacity = 0.0f;
                                }
                            }
                        }
                        else
                        {
                            resumeButtonOpacity -= 0.05f;
                            if (resumeButtonOpacity < 0.0f)
                            {
                                resumeButtonOpacity = 0.0f;
                                resumeButtonIsPressed = false;
                            }
                        }
                        if (Math.Abs(mouseState.X - (optionsButtonPosition.X + optionsButtonFrameSize.X / 2)) <= 150 && Math.Abs(mouseState.Y - (optionsButtonPosition.Y + optionsButtonFrameSize.Y / 2)) <= 150)
                        {
                            if (Math.Sqrt(Math.Pow(mouseState.X - (optionsButtonPosition.X + optionsButtonFrameSize.X / 2), 2) + Math.Pow(mouseState.Y - (optionsButtonPosition.Y + optionsButtonFrameSize.Y / 2), 2)) < Math.Sqrt(Math.Pow(prevMouseState.X - (optionsButtonPosition.X + optionsButtonFrameSize.X / 2), 2) + Math.Pow(prevMouseState.Y - (optionsButtonPosition.Y + optionsButtonFrameSize.Y / 2), 2)))
                            {
                                optionsButtonOpacity += 0.05f;
                                if (optionsButtonOpacity > 1.0f)
                                {
                                    optionsButtonOpacity = 1.0f;
                                }
                            }
                            else if (Math.Sqrt(Math.Pow(mouseState.X - (optionsButtonPosition.X + optionsButtonFrameSize.X / 2), 2) + Math.Pow(mouseState.Y - (optionsButtonPosition.Y + optionsButtonFrameSize.Y / 2), 2)) > Math.Sqrt(Math.Pow(prevMouseState.X - (optionsButtonPosition.X + optionsButtonFrameSize.X / 2), 2) + Math.Pow(prevMouseState.Y - (optionsButtonPosition.Y + optionsButtonFrameSize.Y / 2), 2)))
                            {
                                optionsButtonOpacity -= 0.05f;
                                if (optionsButtonOpacity < 0.0f)
                                {
                                    optionsButtonOpacity = 0.0f;
                                }
                            }
                        }
                        else
                        {
                            optionsButtonOpacity -= 0.05f;
                            if (optionsButtonOpacity < 0.0f)
                            {
                                optionsButtonOpacity = 0.0f;
                                optionsButtonIsPressed = false;
                            }
                        }
                        if (System.Math.Abs(mouseState.X - (quitButtonPosition.X + quitButtonFrameSize.X / 2)) <= 150 && System.Math.Abs(mouseState.Y - (quitButtonPosition.Y + quitButtonFrameSize.Y / 2)) <= 150)
                        {
                            if (System.Math.Sqrt(System.Math.Pow(mouseState.X - (quitButtonPosition.X + quitButtonFrameSize.X / 2), 2) + System.Math.Pow(mouseState.Y - (quitButtonPosition.Y + quitButtonFrameSize.Y / 2), 2)) < System.Math.Sqrt(System.Math.Pow(prevMouseState.X - (quitButtonPosition.X + quitButtonFrameSize.X / 2), 2) + System.Math.Pow(prevMouseState.Y - (quitButtonPosition.Y + quitButtonFrameSize.Y / 2), 2)))
                            {
                                quitButtonOpacity += 0.05f;
                                if (quitButtonOpacity > 1.0f)
                                {
                                    quitButtonOpacity = 1.0f;
                                }
                            }
                            else if (System.Math.Sqrt(System.Math.Pow(mouseState.X - (quitButtonPosition.X + quitButtonFrameSize.X / 2), 2) + System.Math.Pow(mouseState.Y - (quitButtonPosition.Y + quitButtonFrameSize.Y / 2), 2)) > System.Math.Sqrt(System.Math.Pow(prevMouseState.X - (quitButtonPosition.X + quitButtonFrameSize.X / 2), 2) + System.Math.Pow(prevMouseState.Y - (quitButtonPosition.Y + quitButtonFrameSize.Y / 2), 2)))
                            {
                                quitButtonOpacity -= 0.1f;
                                if (quitButtonOpacity < 0.0f)
                                {
                                    quitButtonOpacity = 0.0f;
                                }
                            }
                        }
                        else
                        {
                            quitButtonOpacity -= 0.05f;
                            if (quitButtonOpacity < 0.0f)
                            {
                                quitButtonOpacity = 0.0f;
                                quitButtonIsPressed = false;
                            }
                        }
                        #endregion 


                        if (Collide(new Rectangle((int)pointerPosition.X, (int)pointerPosition.Y, pointerFrameSize.X - pointerOffset, pointerFrameSize.Y - pointerOffset),
     new Rectangle((int)resumeButtonPosition.X + resumeButtonOffset, (int)resumeButtonPosition.Y + resumeButtonOffset, resumeButtonFrameSize.X - resumeButtonOffset, resumeButtonFrameSize.Y - resumeButtonOffset)))
                        {
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                            {
                                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                                {
                                    resumeButtonIsPressed = true;
                                }
                                if (Mouse.GetState().LeftButton == ButtonState.Released)
                                {
                                    if (resumeButtonIsPressed)
                                    {
                                        ResumeButtonIsClicked();
                                    }
                                }
                            }
                        }

                        else if (Collide(new Rectangle((int)pointerPosition.X, (int)pointerPosition.Y, pointerFrameSize.X - pointerOffset, pointerFrameSize.Y - pointerOffset),
                            new Rectangle((int)optionsButtonPosition.X + optionsButtonOffset, (int)optionsButtonPosition.Y + optionsButtonOffset, optionsButtonFrameSize.X - optionsButtonOffset, optionsButtonFrameSize.Y - optionsButtonOffset)))
                        {
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                            {
                                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                                {
                                    optionsButtonIsPressed = true;
                                }
                                if (Mouse.GetState().LeftButton == ButtonState.Released)
                                {
                                    if (optionsButtonIsPressed)
                                    {
                                        OptionsButtonIsClicked();
                                    }
                                }
                            }
                        }

                        else if (Collide(new Rectangle((int)pointerPosition.X, (int)pointerPosition.Y, pointerFrameSize.X - pointerOffset, pointerFrameSize.Y - pointerOffset),
                            new Rectangle((int)quitButtonPosition.X + quitButtonOffset, (int)quitButtonPosition.Y + quitButtonOffset, quitButtonFrameSize.X - quitButtonOffset, quitButtonFrameSize.Y - quitButtonOffset)))
                        {
                            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                            {
                                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                                {
                                    quitButtonIsPressed = true;
                                }
                                if (Mouse.GetState().LeftButton == ButtonState.Released)
                                {
                                    if (quitButtonIsPressed)
                                    {
                                        QuitButtonIsClicked();
                                    }
                                }
                            }
                        }
                        if (mouseState.X != prevMouseState.X || mouseState.Y != prevMouseState.Y)
                        {
                            pointerPosition = new Vector2(mouseState.X, mouseState.Y);
                            prevMouseState = mouseState;
                        }
                    }
                    break;
                #endregion
                #region Gameplay
                case GameState.Gameplay:
                    {
                        MouseState mouseState = Mouse.GetState();
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                            state = GameState.Pause;
                        if (mouseState.X != prevMouseState.X || mouseState.Y != prevMouseState.Y)
                        {
                            pointerPosition = new Vector2(mouseState.X, mouseState.Y);
                            prevMouseState = mouseState;
                        }
                            levelManager.Update(gameTime);
                    }
                    break;
                #endregion
                #region LevelWin
                case GameState.LevelWin:
                    {
                        MouseState mouseState = Mouse.GetState();
                        if (mouseState.X != prevMouseState.X || mouseState.Y != prevMouseState.Y)
                        {
                            pointerPosition = new Vector2(mouseState.X, mouseState.Y);
                            prevMouseState = mouseState;
                        }
                        //fadingAnimation(GraphicsDevice);
                    }
                    break;
                    #endregion
                #region GameOver
                case GameState.GameOver:
                    {
                        MouseState mouseState = Mouse.GetState();

                        gameOver.Update(gameTime, Vector2.Zero);
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            prevState = state;
                            state = GameState.Menu;
                            targetOpacity = 0;
                            Initialize();
                        }

                        if (mouseState.X != prevMouseState.X || mouseState.Y != prevMouseState.Y)
                        {
                            pointerPosition = new Vector2(mouseState.X, mouseState.Y);
                            prevMouseState = mouseState;
                        }
                    }
                    break;
                #endregion
                #region Loading
                case GameState.Loading:
                    {
                        MouseState mouseState = Mouse.GetState();
                        if (mouseState.X != prevMouseState.X || mouseState.Y != prevMouseState.Y)
                        {
                            pointerPosition = new Vector2(mouseState.X, mouseState.Y);
                            prevMouseState = mouseState;
                        }

                        loading.Update(gameTime, new Vector2(0));
                    }
                    break;
                    #endregion
            }

            #region Screenshot
            if (Keyboard.GetState().IsKeyDown(Keys.F11))
            {
                RenderTarget2D screenshot = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
                GraphicsDevice.SetRenderTarget(screenshot);
                Draw(gameTime);
                GraphicsDevice.SetRenderTarget(null);
                Stream stream = File.OpenWrite(DateTime.Now + ".png");
                screenshot.SaveAsPng(stream, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
                stream.Dispose();
                stream.Close();
                screenshot.Dispose();
            }
            #endregion
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            RenderTarget2D target = new RenderTarget2D(GraphicsDevice, Window.ClientBounds.Width, Window.ClientBounds.Height);
            spriteBatch.GraphicsDevice.SetRenderTarget(target);

            switch (state)
            {
                #region Menu
                case GameState.Menu:
                    {
                        GraphicsDevice.Clear(Color.Black);
                        symbolManager.Draw(gameTime);
                        spriteBatch.Draw(playButton, playButtonPosition, Color.White * playButtonOpacity);
                        spriteBatch.Draw(optionsButton, optionsButtonPosition, Color.White * optionsButtonOpacity);
                        spriteBatch.Draw(exitButton, exitButtonPosition, Color.White * exitButtonOpacity);
                    }
                    break;
                #endregion
                #region Help
                case GameState.Help:
                    GraphicsDevice.Clear(Color.Black);
                    //textManager.Draw(gameTime, Fonts.CurlzMT, "[A], [D] - to move\n[W] - to jump\n[I] - to open inventory\n[Q] - to attack\n[E] - to act\n\nPress any key, to continue...", new Vector2(120, 60), Color.White);
                    break;
                #endregion
                #region Options
                case GameState.Options:
                    {
                        GraphicsDevice.Clear(Color.Black);
                    }
                    break;
                #endregion
                #region Pause
                case GameState.Pause:
                    {
                        GraphicsDevice.Clear(Color.Black);
                        levelManager.Draw(gameTime);
                        symbolManager.Draw(gameTime);
                        spriteBatch.Draw(resumeButton, resumeButtonPosition, Color.White * resumeButtonOpacity);
                        spriteBatch.Draw(optionsButton, optionsButtonPosition, Color.White * optionsButtonOpacity);
                        spriteBatch.Draw(quitButton, quitButtonPosition, Color.White * quitButtonOpacity);
                    }
                    break;
                #endregion
                #region Gameplay
                case GameState.Gameplay:
                    {
                        GraphicsDevice.Clear(Color.Black);
                        levelManager.Draw(gameTime);
                    }
                    break;
                #endregion
                #region LevelWin
                case GameState.LevelWin:
                        GraphicsDevice.Clear(Color.Black);
                    break;
                #endregion
                #region GameOver
                case GameState.GameOver:
                    {
                        GraphicsDevice.Clear(Color.Black);
                        gameOver.Draw(spriteBatch, gameTime);
                        //textManager.Draw(gameTime, Fonts.CurlzMT, "Press [Enter]...", new Vector2(80, 420), new Color(152, 255, 71));

                        break;
                    }
                #endregion
                #region Loading
                case GameState.Loading:
                    {
                        GraphicsDevice.Clear(Color.Black);
                        loading.Draw(spriteBatch, gameTime, 0.25f);
                    }
                    break;
                    #endregion
            }
            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Draw(target, new Rectangle(0, 0, 800, 600), Color.White * targetOpacity);
            spriteBatch.Draw(Pointer, pointerPosition, Color.White);
            spriteBatch.End();
        }

        bool Collide(Rectangle r1, Rectangle r2)
        {
            return r1.Intersects(r2);
        }

        //Buttons click events list
        #region ButtonsClickMethods
        private void PlayButtonIsClicked()
        {
            if (MainClass.helpEnabled)
            {
                state = GameState.Help;
            }
            else
            {
                state = GameState.Gameplay;
            }
            prevState = state;
            GraphicsDevice.Clear(new Color(9, 3, 0));
            MediaPlayer.Stop();
            Components.Add(levelManager);
            targetOpacity = 0;
        }
        private void OptionsButtonIsClicked()
        {
            prevState = state;
            state = GameState.GameOver;
            targetOpacity = 0;
        }
        private void ExitButtonIsClicked()
        {
            Exit();
        }
        private void ApplyButtonIsClicked()
        {
            //...
            targetOpacity = 0;
        }
        private void CancelButtonIsClicked()
        {
            GameState st = prevState;
            prevState = state;
            state = st;
            targetOpacity = 0;
        }
        private void ResumeButtonIsClicked()
        {
            prevState = state;
            state = GameState.Gameplay;
            targetOpacity = 0;
        }
        private void QuitButtonIsClicked()
        {
            //Save();
            prevState = state;
            state = GameState.Menu;
            targetOpacity = 0;
        }
        #endregion

        public void GameOver()
        {
            state = GameState.GameOver;
        }

        public void LevelWin()
        {
            state = GameState.LevelWin;
        }

        public void GoToMainMenu()
        {
            state = GameState.Menu;
        }

        public bool IsLoading
        {
            set
            {
                if (value)
                {
                    prevState = state;
                    state = GameState.Loading;
                }
                else
                {
                    state = prevState;
                } 
            }
        }
    }
}
