﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    texture = Info.Load<Texture2D>("");
                    break;
                case ObstaclesID.Rock_Pile:
                case ObstaclesID.Stump:
                case ObstaclesID.Bush:
                case ObstaclesID.Tree:
                    texture = Info.Load<Texture2D>("");
                    break;
            }
        }

        public void Dispose()
        {
            if (!this.texture.IsDisposed) this.texture.Dispose();
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
        //List<Enemy> enemyList = new List<Enemy>();
        //List<Lantern> lanternsList = new List<Lantern>();
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
            Info.Load<Texture2D>(@"Images\Platforms\PlatformSpriteSheetRE"),
            Info.Load<Texture2D>(@"Images\Platforms\SymbolPlatforms\SymbolPlatformSpriteSheet1RE"),
            Info.Load<Texture2D>(@"Images\Platforms\SymbolPlatforms\SymbolPlatformSpriteSheet2RE"),
            Info.Load<Texture2D>(@"Images\Platforms\SymbolPlatforms\SymbolPlatformSpriteSheet3RE"),
            Info.Load<Texture2D>(@"Images\Platforms\SymbolPlatforms\SymbolPlatformSpriteSheet4RE"),
            Info.Load<Texture2D>(@"Images\Platforms\SymbolPlatforms\SymbolPlatformSpriteSheet5RE")
        };

        //Список платформ
        List<Platform[]> platformsList = new List<Platform[]>();

        Rectangle viewFrame = new Rectangle(0, 347, 800, 600); //Рамка, что отрисовывать

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
        public Level(DreamCatcherGame game, SpriteBatch spriteBatch, Player player,
            Texture2D lantern, GameScreen background, Texture2D ground, List<Object> objects, Region region, int level)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.player = player;
            this.lantern = lantern;
            this.background = background;
            this.ground = ground;
            this.objects = objects;
            this.region = region;

            FileManager.LoadMap(@"Content\Files\Maps\" + region.ToString() + "\\map" + level.ToString() + ".dcgm", platformTextureList, lantern, ref groundRect, ref levelGround, ref platformsList);
        }

        public Level(DreamCatcherGame game, SpriteBatch spriteBatch, Player player, GameScreen background, Texture2D ground, List<Platform[]> platformsList, List<Object> objects)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.player = player;
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
            //background.Update(gameTime, player.Direction);

            #region Player Logic
            player.Update(gameTime, new Rectangle(0, 0, background.FrameSize.X, background.FrameSize.Y - background.FrameSize.Y % 10));
            player.CollisionCheck((from pl in platformsList
                                   from p in pl
                                   select p).ToList());
            #endregion

            #region Enemies Logic

            if (Enemy.GetEnemies.Count != 0) for (int i = 0; i < Enemy.GetEnemies.Count; i++)
                {
                    List<Lantern> l = (from lan in Lantern.GetLanterns
                                       where lan.Active
                                       select lan).ToList();
                    List<Platform> p = (from plat in platformsList
                                        from pl in plat
                                        where pl.IsMaterial
                                        select pl).ToList();

                    Enemy.GetEnemies[i].Update(gameTime, new Rectangle(0, 0, background.FrameSize.X, background.FrameSize.Y), p, l);
                    }
            #endregion

            #region Lanterns Logic
            if (Lantern.GetLanterns.Count != 0) for (int i = 0; i < Lantern.GetLanterns.Count; i++)
                {
                    Lantern.GetLanterns[i].Update(gameTime, new Rectangle(0, 0, background.FrameSize.X, background.FrameSize.Y));
                }
            #endregion

            #region Collision Logic
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
            if (Enemy.GetEnemies.Count != 0) for (int i = 0; i<Enemy.GetEnemies.Count; i++)
                {
                    Enemy.GetEnemies[i].CollisionCheck(groundRect);
                }
            #endregion

            #region Obstacles
            if (obstacles.Count != 0) for (int i = 0; i<obstacles.Count; i++)
                {
                    player.CollisionCheck(obstacles[i].collisionRectangle);
                }
            #endregion
            #region Lantern
            if (Lantern.GetLanterns.Count != 0)
                for (int i = 0; i< Lantern.GetLanterns.Count; i++)
                {
                    //Lantern.GetLanterns[i].CollisionCheck(player);
                    player.CollisionCheck(Lantern.GetLanterns[i]);
                }
            #endregion
            #endregion

            #region Win Logic
            bool win = true;
            for (int i = 0; i < Lantern.GetLanterns.Count; i++)
            {
                if (!Lantern.GetLanterns[i].Active)
                {
                    win = false;
                    break;
                }
            }
            if (region!= Region.Dream && Enemy.GetEnemies.Count != 0) win = false;
            if (win)
            {
                Enemy.Clear();
                //Info.Win();
            }
            #endregion
        }

        public void Draw(GameTime gameTime)
        {
            try
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                background.Draw(spriteBatch, gameTime, player.GetBackViewFrame);
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

                if (Lantern.GetLanterns.Count != 0)
                    for (int i = 0; i< Lantern.GetLanterns.Count; i++)
                    {
                        Lantern l = Lantern.GetLanterns[i];
                        l.Draw(gameTime, new Vector2(player.GetViewFrame.X, player.GetViewFrame.Y), spriteBatch);
                    }

                if (Enemy.GetEnemies.Count != 0)
                    for (int i = 0; i < Enemy.GetEnemies.Count; i++)
                    {
                        Enemy e = Enemy.GetEnemies[i];
                        e.Draw(gameTime, new Vector2(player.GetViewFrame.X, player.GetViewFrame.Y), spriteBatch);
                    }

                player.Draw(gameTime, player.GetViewFrame.Location.ToVector2(), spriteBatch);
                spriteBatch.End();
            }
            catch
            {
                background.Draw(spriteBatch, gameTime, player.GetBackViewFrame);
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
                if (Lantern.GetLanterns.Count != 0) for (int i = 0; i < Lantern.GetLanterns.Count; i++)
                    {
                        Lantern l = Lantern.GetLanterns[i];
                        l.Draw(gameTime, new Vector2(viewFrame.X, viewFrame.Y), spriteBatch);
                    }
                if (obstacles.Count != 0) for (int i = 0; i < obstacles.Count; i++)
                    {
                        Obstacle o = obstacles[i];
                        spriteBatch.Draw(o.texture, new Vector2(o.position.X - viewFrame.X, o.position.Y - viewFrame.Y), Color.White);
                    }

                if (Enemy.GetEnemies.Count != 0) for (int i = 0; i < Enemy.GetEnemies.Count; i++)
                    {
                        Enemy e = Enemy.GetEnemies[i];
                        e.Draw(gameTime, new Vector2(viewFrame.X, viewFrame.Y), spriteBatch);
                    }

                player.Draw(gameTime, new Vector2(viewFrame.X, viewFrame.Y), spriteBatch);
            }
        }

        public void Dispose()
        {
            this.background.Dispose();
            if (this.foreground.Count != 0)
            {
                foreach (Texture2D t in foreground)
                {
                    if (t.IsDisposed)
                    {
                        continue;
                    }
                    t.Dispose();
                }
                foreground = null;
            }
            if (!this.lantern.IsDisposed) this.lantern.Dispose();
            if (this.objects.Count != 0)
            {
                foreach (Object o in objects)
                {
                    o.Dispose();
                }
                objects = null;
            }
            if (this.obstacles.Count != 0)
            {
                foreach(Obstacle o in obstacles)
                {
                    o.Dispose();
                }
                obstacles = null;
            }

            if(this.platformTextureList.Count != 0)
            {
                foreach(Texture2D t in platformTextureList)
                {
                    if (t.IsDisposed) continue;
                    t.Dispose();
                }
                platformTextureList = null;
            }

            if (this.platformsList.Count != 0)
            {
                platformsList.ForEach((p) => { foreach (Platform pl in p) { pl.Dispose(); } });
                platformsList = null;
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
                                new Lantern(lantern, (start + new Point(block.X * int.Parse(s2.Split(',')[0]), block.Y * int.Parse(s2.Split(',')[1]))).ToVector2(),
                                    new Point(30, 40), 0, new Point(0, 0), new Point(4, 2));
                            }
                        }
                        break;
                }
            }
        }
        #endregion
    }
}
