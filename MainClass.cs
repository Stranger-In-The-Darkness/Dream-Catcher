using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using NLua;

namespace DreamCatcher
{
    static class MainClass
    {
        #region Variabes
        public static Point currentFrameSize = new Point(800, 600);

        //"Центр" отрисовываемого фона
        static Point center = new Point(0, 0);

        public static bool firstRun = true;

        public static int succedCount = 0;

        public static int money = 0;

        public static Region region = Region.Dream;
        public static int level = 0;

        static int[,] Map = new int[12, 48];

        static DreamCatcherGame game = null;

        #endregion

        #region Settings
        public static float SFXLevel = 1.0f; //Звуковые эффекты
        public static float MusicLevel = 1.0f; //Музыка
        public static bool helpEnabled = true; //"Помощь"
        public static Point resolution = new Point(800, 600);
        #endregion

        #region Properties
        //Текущая "сцена"
        public static GameScreen CurrentScreen
        {
            get;

            private set;
        }

        public static DreamCatcherGame Game
        {
            get
            {
                return game;
            }
            set { if(game == null
                    ) game = value; }
        }

        public static int Level
        {
            get
            {
                return level;
            }
            private set
            {
                level = value;
            }
        }

        public static Point ViewCenter
        {
            get
            {
                return center;
            }
            set
            {
                if (value.X - 320 < 0)
                {
                    value.X = 320;
                }
                if (value.X + 320 > currentFrameSize.X)
                {
                    value.X = currentFrameSize.X - 320;
                }
                if (value.Y - 320 < 0)
                {
                    value.Y = 320;
                }
                if (value.Y + 320 > currentFrameSize.Y)
                {
                    value.Y = currentFrameSize.Y - 320;
                }
                center = value;
            }
        }
        #endregion

        #region Methods
        //Метод для загрузки текстур, и всякого такого добра
        public static T Load<T>(string adress)
        {
            return game.Content.Load<T>(adress);
        }

        public static void ChangeSFXLevel(int level)
        {
            SFXLevel = level > 1 ? 1 : (level < 0 ? 0 : level);
        }

        public static void ChangeMusicLevel(int level)
        {
            MusicLevel = level > 1 ? 1 : (level < 0 ? 0 : level);
        }

        //Меняем сценку
        public static void SetScreen(GameScreen screen)
        {
            if(CurrentScreen != screen)
            {
                CurrentScreen = screen;
            }
        }

        //Конец Игры
        public static void GameOver()
        {
            game.GameOver();
        }

        public static void Win()
        {
            game.LevelWin();
        }

        public static void WriteLog(string s)
        {
            object o = new object();
            lock (o)
            {
                System.IO.StreamWriter log = new System.IO.StreamWriter("dcg.log", true);
                log.WriteLineAsync(s);
                log.Close();
            }
        }
        #endregion
    }
}
