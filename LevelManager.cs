using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DreamCatcher
{
    /// <summary>
    /// Manager type to control levels
    /// </summary>
    public class LevelManager : DrawableGameComponent
    {
        enum Stage { Region, Level }
        #region Variables
        Stage stage = Stage.Region;
        Region region = Region.Dream;
        int _Level = 0;

        InputManager inputManager = new InputManager();

        DreamCatcherGame game;
        SpriteBatch spriteBatch;

        private bool isDisposed = false;

        #region Level Variables
        Level level;

            Player player;
            List<Enemy> enemyList;

        List<Lantern> lanternsList;
        List<Object> objects;

        List<Texture2D> background;
        Texture2D ground;

        List<Texture2D> foreground;

        Rectangle frameSize; 
        Rectangle viewFrame; 
        Rectangle backgroundViewFrame;
        //Фон - 1512 710
        //Уровень - 2220 800'
        #endregion

        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game">Object of base game type</param>
        /// <param name="player">Object of base player type</param>
        public LevelManager(DreamCatcherGame game, Player player) : base(game)
        {
            this.game = game;
            this.player = player;
        }
        #endregion

        #region Methods
        public override void Initialize()
        {
            enemyList = new List<Enemy>();
            lanternsList = new List<Lantern>();
            objects = new List<Object>();
            background = new List<Texture2D>();
            foreground = new List<Texture2D>();

            frameSize = new Rectangle(0, 0, 2016, 947);
            viewFrame = new Rectangle(0, 347, 800, 600);
            backgroundViewFrame = new Rectangle(0, 110, 800, 600);
            //foreach(Texture2D t in background)
            //{
            //    t.Dispose();
            //}
            //ground.Dispose();
            //if (foreground.Count != 0)
            //{
            //    foreach (Texture2D t in foreground)
            //    {
            //        t.Dispose();
            //    }
            //    foreground.Clear();
            //}
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            Info.Game.IsLoading = true;
            Task t = new Task(LoadLevel);
            t.Start();
        }

        private void LoadLevel()
        {
            NLua.LuaTable table = null;
            switch (region)
            {
                //f, v, c, r
                case Region.Dream:
                default:
                    table = (NLua.LuaTable)FileManager.ParseLuaFile($"Content\\Files\\Level\\d_{Level}.lua")[0];
                    break;
            }
            NLua.LuaTable tabl = (NLua.LuaTable) table["background"];
            for(int i = 0; i<tabl.Values.Count; i++)
            {
                background.Add(Game.Content.Load<Texture2D>(tabl[i].ToString()));
            }
            //background[0] = Game.Content.Load<Texture2D>(@"Images\Backgrounds\Background" + Level);
            ground = Game.Content.Load<Texture2D>(table["ground"].ToString());
            Texture2D lantern = Game.Content.Load<Texture2D>(@"Images\Objects\Lanterns\LanternAnimation");
            try
            {
                for(int i = 0; i < ((NLua.LuaTable)table["enemy"]).Keys.Count; i++)
                {
                    tabl = (NLua.LuaTable)((NLua.LuaTable)((NLua.LuaTable)table["enemy"])[i]);
                    enemyList.Add(new Enemy(
                        tabl["id"].ToString() == "shadow_hunter" ? EnemiesID.Shadow_Hunter : EnemiesID.Shadow_Hunter, 
                        tabl["class"].ToString() == "basic" ? EnemyClass.Basic : EnemyClass.Basic, 
                        new Vector2(
                            Convert.ToInt32(((NLua.LuaTable)tabl["position"])["x"]), 
                            Convert.ToInt32(((NLua.LuaTable)tabl["position"])["y"]))
                            )
                            );
                }
            }
            catch (Exception e)
            {

            }

            try
            {
                for (int i = 0; i < ((NLua.LuaTable)table["object"]).Keys.Count; i++)
                {
                    tabl = (NLua.LuaTable)((NLua.LuaTable)((NLua.LuaTable)table["object"])[$"{i + 1}"]);
                    objects.Add(new Object(
                        tabl["id"].ToString() == "stoneberry" ? ObjectsID.Stoneberry : ObjectsID.Stoneberry,
                        new Vector2(
                            Convert.ToInt32(((NLua.LuaTable)tabl["position"])["x"]),
                            Convert.ToInt32(((NLua.LuaTable)tabl["position"])["y"]))
                            )
                            );
                }
            }
            catch (Exception e)
            {

            }
            level = new Level(game, spriteBatch, player, lantern,
                new GameScreen(ScreenType.Movable, background, background[0].Bounds.Size, Point.Zero, new Point(1, 1), 0, 0),
                ground, objects, region, 0);
            Info.Game.IsLoading = false;
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            level.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            level.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            level.Dispose();
            level = null;

            if (this.background.Count != 0)
            {
                foreach(Texture2D t in background)
                {
                    if (t.IsDisposed) continue;
                    t.Dispose();
                }
                background = null;
            }

            if (this.foreground.Count != 0)
            {
                foreach(Texture2D t in foreground)
                {
                    if (t.IsDisposed) continue;
                    t.Dispose();
                }
                foreground = null;
            }

            if (!this.ground.IsDisposed) this.ground.Dispose();
            this.ground = null;

            if (this.lanternsList.Count != 0)
            {
                foreach(Lantern l in lanternsList)
                {
                    l.Dispose();
                }
                lanternsList = null;
            }
            isDisposed = true;
            base.UnloadContent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns level number, or sets another and causes Load proccess
        /// </summary>
        public int Level
        {
            get
            {
                return _Level;
            }
            set
            {
                if (value >= 0 && value != _Level)
                {
                    _Level = value;
                    Info.Game.IsLoading = true;
                    Thread loadLevel = new Thread(new ThreadStart(LoadLevel))
                    {
                        IsBackground = true
                    };
                    loadLevel.Start();
                }
            }
        }

        public bool IsDisposed { get => isDisposed; set => isDisposed = value; }

        #endregion
    }
}
