using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace DreamCatcher
{
    /// <summary>
    /// State of obstacle
    /// </summary>
    enum objectState { Passable, Impassable, Breakable }

    /// <summary>
    /// In-game obstacle (rock, stump, etc.)
    /// </summary>
    class Obstacle
    {
        public Texture2D texture;
        public Vector2 position;
        public Rectangle collisionRectangle;
        public objectState state;

        public Obstacle(Texture2D texture, Vector2 position, Rectangle collisionRectangle, objectState state)
        {
            this.texture = texture;
            this.position = position;
            this.collisionRectangle = collisionRectangle;
            this.state = state;
        }

        public Obstacle(DreamCatcher.ObstaclesID ID, Vector2 position, objectState state)
        {
            this.state = state;
            this.position = position;
            switch (ID)
            {
                case ObstaclesID.Leaf_Pile:
                    texture = MainClass.Load<Texture2D>("");
                    break;
                case ObstaclesID.Rock_Pile:
                case ObstaclesID.Stump:
                case ObstaclesID.Bush:
                case ObstaclesID.Tree:
                    texture = MainClass.Load<Texture2D>("");
                    break;
            }
        }
    }

    /// <summary>
    /// Main type for levels
    /// </summary>
    public class Level
    {
        #region Variables
        DreamCatcherGame game;
        SpriteBatch spriteBatch;
        InputManager inputManager = new InputManager();

        Random rnd = new Random();

        Player player;
        List<Enemy> enemyList = new List<Enemy>();
        List<Lantern> lanternsList = new List<Lantern>();
        List<Object> objects = new List<Object>();

        Region region;

        GameScreen background;
        Texture2D ground;
        Rectangle groundRect = new Rectangle(0, 869, 2016, 947);
        List<Point> levelGround = new List<Point>();
        Point groundSize = new Point(400, 100);

        Texture2D lantern;

        List<Texture2D> foreground = new List<Texture2D>();

        //Все варианты платформ
        List<Texture2D> platformTextureList = new List<Texture2D>
        {
            MainClass.Load<Texture2D>(@"Images\Platforms\PlatformSpriteSheet"),
            MainClass.Load<Texture2D>(@"Images\Platforms\SymbolPlatforms\SymbolPlatformSpriteSheet1V3"),
            MainClass.Load<Texture2D>(@"Images\Platforms\SymbolPlatforms\SymbolPlatformSpriteSheet2V3"),
            MainClass.Load<Texture2D>(@"Images\Platforms\SymbolPlatforms\SymbolPlatformSpriteSheet3"),
            MainClass.Load<Texture2D>(@"Images\Platforms\SymbolPlatforms\SymbolPlatformSpriteSheet4V3"),
            MainClass.Load<Texture2D>(@"Images\Platforms\SymbolPlatforms\SymbolPlatformSpriteSheet5V3")
        };

        //Список платформ
        List<Platform[]> platformsList = new List<Platform[]>();

        Rectangle frameSize = new Rectangle(0, 0, 2016, 947); //Вообще это должно задаваться в зависимости от размеров заднего фона... но я не придумал как... пока
        Rectangle viewFrame = new Rectangle(0, 347, 800, 600); //Рамка, что отрисовывать
        Rectangle backgroundViewFrame = new Rectangle(0, 110, 800, 600);

        List<Obstacle> obstacles = new List<Obstacle>();

        bool levelSucced = false;
        Button endLevel;
        Button goToTheCity;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game">Object of base type of the game</param>
        /// <param name="spriteBatch">Sprite batch</param>
        /// <param name="player">Object of player type</param>
        /// <param name="enemyList">List of enemies</param>
        /// <param name="lantern">Lantern sprite sheet texture</param>
        /// <param name="background">Background game screen</param>
        /// <param name="ground">Ground texture</param>
        /// <param name="objects">List of different in-game objects</param>
        /// <param name="region">In-game region</param>
        /// <param name="level">Level number</param>
        public Level(DreamCatcherGame game, SpriteBatch spriteBatch, Player player, List<Enemy> enemyList,
            Texture2D lantern, GameScreen background, Texture2D ground, List<Object> objects, Region region, int level)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.player = player;
            this.enemyList = enemyList;
            this.lantern = lantern;
            this.background = background;
            this.ground = ground;
            this.objects = objects;
            this.region = region;

            //levelGround.AddRange(new Point[] { new Point(-600, 847), new Point(-200, 847), new Point(200, 847), new Point(600, 847), new Point(1000, 847), new Point(1400, 847), new Point(1800, 847), new Point(2200, 847) });

            FileManager.LoadMap(@"Content\Files\Maps\" + region.ToString() + "\\map" + level.ToString() + ".dcgm", platformTextureList, lantern, ref groundRect, ref levelGround, ref platformsList, ref lanternsList);
        }

        public Level(DreamCatcherGame game, SpriteBatch spriteBatch, Player player, List<Enemy> enemyList,
            List<Lantern> lanternsList, GameScreen background, Texture2D ground, List<Platform[]> platformsList, List<Object> objects)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.player = player;
            this.enemyList = enemyList;
            this.lanternsList = lanternsList;
            this.background = background;
            this.ground = ground;
            this.objects = objects;
            this.platformsList = platformsList;

            levelGround.AddRange(new Point[] { new Point(-600, 847), new Point(-200, 847), new Point(200, 847), new Point(600, 847), new Point(1000, 847), new Point(1400, 847), new Point(1800, 847), new Point(2200, 847) });
        }
        #endregion

        #region Methods
        public void Update(GameTime gameTime)
        {
            inputManager.Update();

            #region Player_Logic
            player.Update(gameTime, new Rectangle(0, 0, background.FrameSize.X, background.FrameSize.Y));
            player.CollisionCheck((from pl in platformsList
                                   from p in pl
                                   select p).ToList());
            #endregion

            #region Enemies_Logic
            if (enemyList.Count != 0) for (int i = 0; i < enemyList.Count; i++)
                {
                    List<Lantern> l = (from lan in lanternsList
                                       where lan.Active == true
                                       select lan).ToList();
                    List<Platform> p = (from plat in platformsList
                                        from pl in plat
                                        where pl.IsMaterial == true
                                        select pl).ToList();
                    enemyList[i].Update(gameTime, new Rectangle(0, 0, background.FrameSize.X, background.FrameSize.Y), p, l);
                    if (player.CollisionRect.Intersects(enemyList[i].CollisionRect)) player.Respawn();
                }
            #endregion

            #region Lanterns_Logic
            if (lanternsList.Count != 0) for (int i = 0; i < lanternsList.Count; i++)
                {
                    lanternsList[i].Update(gameTime, new Rectangle(0, 0, background.FrameSize.X, background.FrameSize.Y));
                }
            #endregion

            #region Collision_Logic
            #region Platforms
            for (int i = 0; i<platformsList.Count; i++)
            {
                foreach (Platform p2 in platformsList[i])
                {
                    p2.Update(gameTime);
                }

                //Включение платформ... Если игрок приблизился на достаочное расстояние, то запускается цепная реакция
                #region PlatformActivation
                if (platformsList[i][0].CollisionRect.Intersects(player.CollisionRect))
                {
                    platformsList[i][0].Activate();
                    platformsList[i][platformsList[i].Length-1].Activate();
                    for (int i2 = 1; i2 < platformsList[i].Length - 1; i2++)
                    {
                        platformsList[i][i2].Activate();
                    }
                }
                else if (platformsList[i][platformsList[i].Length - 1].CollisionRect.Intersects(player.CollisionRect))
                {
                    platformsList[i][platformsList[i].Length - 1].Activate();
                    platformsList[i][0].Activate();
                    for (int i2 = platformsList[i].Length - 1; i2 > 0; i2--)
                    {
                        platformsList[i][i2].Activate();
                    }
                }
                #endregion
            }
            #endregion

            #region Ground
                player.CollisionCheck(groundRect);
            if (enemyList.Count != 0) for (int i = 0; i<enemyList.Count; i++)
                {
                    enemyList[i].CollisionCheck(groundRect);
                }
            #endregion

            #region Obstacles
            if (obstacles.Count != 0) for (int i = 0; i<obstacles.Count; i++)
                {
                    player.CollisionCheck(obstacles[i].collisionRectangle);
                }
            if (lanternsList.Count != 0) for (int i = 0; i<lanternsList.Count; i++)
                {
                    if (lanternsList[i].CollisionCheck(player.CollisionRect)) lanternsList[i].Activate();
                }
            #endregion
            #endregion

            #region Win_Logic
            bool win = true;
            for (int i = 0; i < lanternsList.Count; i++)
            {
                if (!lanternsList[i].Active)
                {
                    win = false;
                    break;
                }
            }
            if (region!= Region.Dream && enemyList.Count != 0) win = false;
            if (win)
            {
                while(enemyList.Count != 0)
                {
                    enemyList.RemoveAt(0);
                }
                //MainClass.Win();
            }
            #endregion
        }

        public void Draw(GameTime gameTime)
        {
            try
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                background.Draw(spriteBatch, gameTime);
                for(int i = 0; i< levelGround.Count; i++)
                {
                    Point r = levelGround[i];
                    spriteBatch.Draw(ground, new Vector2(r.X - player.GetViewFrame.X, r.Y - player.GetViewFrame.Y), Color.White);
                }

                for (int i = 0; i< platformsList.Count; i++)
                {
                    Platform[] p1 = platformsList[i];
                    foreach (Platform p2 in p1)
                    {
                        p2.Draw(gameTime, new Vector2(player.GetViewFrame.X, player.GetViewFrame.Y), spriteBatch);
                    }
                }

                if (obstacles.Count != 0)
                    for (int i = 0; i<obstacles.Count; i++)
                    {
                        Obstacle o = obstacles[i];
                        spriteBatch.Draw(o.texture, new Vector2(o.position.X - player.GetViewFrame.X, o.position.Y - player.GetViewFrame.Y), Color.White);
                    }

                if (lanternsList.Count != 0)
                    for (int i = 0; i<lanternsList.Count; i++)
                    {
                        Lantern l = lanternsList[i];
                        l.Draw(gameTime, new Vector2(player.GetViewFrame.X, player.GetViewFrame.Y), spriteBatch);
                    }

                if (enemyList.Count != 0)
                    for (int i = 0; i < enemyList.Count; i++)
                    {
                        Enemy e = enemyList[i];
                        e.Draw(gameTime, new Vector2(player.GetViewFrame.X, player.GetViewFrame.Y), spriteBatch);
                    }

                player.Draw(gameTime, player.GetViewFrame.Location.ToVector2(), spriteBatch);
                spriteBatch.End();
            }
            catch
            {
                background.Draw(spriteBatch, gameTime);
                for (int i = 0; i < levelGround.Count; i++)
                {
                    Point r = levelGround[i];
                    spriteBatch.Draw(ground, new Vector2(r.X - viewFrame.X, r.Y - viewFrame.Y), Color.White);
                }
                for (int i = 0; i < platformsList.Count; i++)
                {
                    Platform[] p1 = platformsList[i];
                    foreach (Platform p2 in p1)
                    {
                        p2.Draw(gameTime, new Vector2(viewFrame.X, viewFrame.Y), spriteBatch);
                    }
                }
                if (lanternsList.Count != 0) for (int i = 0; i < lanternsList.Count; i++)
                    {
                        Lantern l = lanternsList[i];
                        l.Draw(gameTime, new Vector2(viewFrame.X, viewFrame.Y), spriteBatch);
                    }
                if (obstacles.Count != 0) for (int i = 0; i < obstacles.Count; i++)
                    {
                        Obstacle o = obstacles[i];
                        spriteBatch.Draw(o.texture, new Vector2(o.position.X - viewFrame.X, o.position.Y - viewFrame.Y), Color.White);
                    }

                if (enemyList.Count != 0) for (int i = 0; i < enemyList.Count; i++)
                    {
                        Enemy e = enemyList[i];
                        e.Draw(gameTime, new Vector2(viewFrame.X, viewFrame.Y), spriteBatch);
                    }

                player.Draw(gameTime, new Vector2(viewFrame.X, viewFrame.Y), spriteBatch);
            }
        }

        /// <summary>
        /// Loads game components, such as platforms and lanterns
        /// </summary>
        /// <param name="stage">Current level region</param>
        /// <param name="level">Current level number</param>
        private void LoadMap(Region stage, int level)
        {
            StreamReader r = new StreamReader(@"Content\Files\Maps\" + stage.ToString() + "\\map" + level.ToString() + ".dcgm");
            string @string = "";
            while (!r.EndOfStream)
            {
                string str = r.ReadLine();
                if (str.IndexOf("//") != -1)
                {
                    str = str.Remove(str.IndexOf("//"));
                }
                @string += str;
            }
            for (int i = 0; i < 2; i++)
            {
                string info = @string.Split('.')[i];
                switch (i)
                {
                    case 0:
                        {
                            string str = info.Split(';')[0];
                            str = str.Substring(str.IndexOf('{') + 1, str.Length - 2);
                            Point start = new Point(int.Parse(str.Split(',')[0]), int.Parse(str.Split(',')[1]));

                            str = info.Split(';')[1];
                            str = str.Substring(str.IndexOf('(') + 1, str.Length - 2);
                            Point block = new Point(int.Parse(str.Split(',')[0]), int.Parse(str.Split(',')[1]));

                            str = info.Substring(info.IndexOf(info.Split(';')[2]));
                            foreach (string s in str.Split('|'))
                            {
                                string s2 = s.Substring(s.IndexOf('[') + 1, s.Length - 2);
                                Platform[] p = new Platform[s2.Split(';').Length];
                                for (int count = 0; count < p.Length; count++)
                                {
                                    string s3 = s2.Split(';')[count];
                                    s3 = s3.Substring(s3.IndexOf('[') + 1, s3.Length - 2);
                                    p[count] = new Platform(platformTextureList[int.Parse(s3.Split(',')[0])],
                                        (start + new Point(block.X * int.Parse(s3.Split(',')[1]),
                                        block.Y * int.Parse(s3.Split(',')[2]))).ToVector2(), new Point(3, 3),
                                        new Point(50, 37), new Point(0, 0), int.Parse(s3.Split(',')[3]) == 1 ? true : false);
                                }
                                platformsList.Add(p);
                            }
                        }
                        break;
                    case 1:
                        {
                            string str = info.Split(';')[0];
                            str = str.Substring(str.IndexOf('{') + 1, str.Length - 2);
                            Point start = new Point(int.Parse(str.Split(',')[0]), int.Parse(str.Split(',')[1]));

                            str = info.Split(';')[1];
                            str = str.Substring(str.IndexOf('(') + 1, str.Length - 2);
                            Point block = new Point(int.Parse(str.Split(',')[0]), int.Parse(str.Split(',')[1]));

                            str = info.Split(';')[2];
                            foreach (string s in str.Split('|'))
                            {
                                string s2 = s.Substring(s.IndexOf('<') + 1, s.Length - 2).Replace(">","");
                                lanternsList.Add(new Lantern(lantern, (start + new Point(block.X * int.Parse(s2.Split(',')[0]), block.Y * int.Parse(s2.Split(',')[1]))).ToVector2(),
                                    new Point(30, 40), 0, new Point(0, 0), new Point(4, 2)));
                            }
                        }
                        break;
                }
            }
        }
        #endregion
    }
}
