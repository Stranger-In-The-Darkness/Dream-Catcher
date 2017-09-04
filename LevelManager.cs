using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        #region Level Variables
            Level level;

            Player player;
            List<Enemy> enemyList = new List<Enemy>();

            List<Lantern> lanternsList = new List<Lantern>();
            List<Object> objects = new List<Object>();

            Texture2D background;
            Texture2D ground;

            List<Texture2D> foreground = new List<Texture2D>();

            Rectangle frameSize = new Rectangle(0, 0, 2016, 947); //Вообще это должно задаваться в зависимости от размеров заднего фона... но я не придумал как... пока
            Rectangle viewFrame = new Rectangle(0, 347, 800, 600); //Рамка, что отрисовывать
            Rectangle backgroundViewFrame = new Rectangle(0, 110, 800, 600);

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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            MainClass.Game.IsLoading = true;
            Task t = new Task(LoadLevel);
            t.Start();
        }

        private void LoadLevel()
        {
            Thread.Sleep(1000);
            background = Game.Content.Load<Texture2D>(@"Images\Backgrounds\Background" + Level);
            ground = Game.Content.Load<Texture2D>((@"Images\Backgrounds\Ground" + Level).ToString());
            Texture2D lantern = Game.Content.Load<Texture2D>(@"Images\Objects\Lanterns\LanternAnimation");
            //enemyList.Add(new Enemy(EnemyClass.Basic, MainClass.Load<Texture2D>(@"Images\Enemies\EnemyAnimation1V2"), new Vector2(1800, 869 - 128),
            //    new Point(101, 130), 0, new Point(0, 0), new Point(8, 4), new Vector2(1.7f, 1.5f), 80, 140, 50, new Rectangle(20, 0, 60, 130)));
            enemyList.Add(new Enemy(EnemiesID.Shadow_Hunter, EnemyClass.Basic, new Vector2(1800, 869 - 128)));
            level = new Level(game, spriteBatch, player, enemyList, lantern,
                new GameScreen(ScreenType.Movable, new Texture2D[] { background }, background.Bounds.Size, Point.Zero, new Point(1, 1), 0, 0),
                ground, objects, region, 0);
            Thread.Sleep(1000);
            MainClass.Game.IsLoading = false;
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
                    MainClass.Game.IsLoading = true;
                    Thread loadLevel = new Thread(new ThreadStart(LoadLevel))
                    {
                        IsBackground = true
                    };
                    loadLevel.Start();
                }
            }
        }

        #endregion
    }
}
