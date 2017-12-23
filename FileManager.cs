using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using NLua;

namespace DreamCatcher
{
    public static class FileManager
    {
        #region Variables
        static StreamWriter settingsSave;
        static StreamWriter mainSave;

        static StreamReader settingsRead;
        static StreamReader mainRead;

        static Lua lua = new Lua();
        #endregion

        #region Methods
        public static void Save(Region region, int level, int playerHealth, int playerShield, int playerMana,
            int coins, int playerBagCapacity, List<string> objects)
        {
            mainSave = new StreamWriter(File.Open(@"Saves\gs0.dcsv", FileMode.OpenOrCreate, FileAccess.ReadWrite));
            string[] text = mainRead.ReadToEnd().Replace("/*", "").Split('/');
            text[0] = region.ToString();
            text[1] = level.ToString();
            text[2] = playerHealth.ToString();
            text[3] = playerShield.ToString();
            text[4] = playerMana.ToString();
            text[5] = coins.ToString();
            text[6] = playerBagCapacity.ToString();
            foreach (string s in objects)
            {
                text[7] += s + ",";
            }
            foreach (string s in text)
            {
                mainSave.Write(s);
                mainSave.Write("/");
            }
            mainSave.Write("*");
            mainSave.Dispose();
            mainSave.Close();
        }

        public static void Save(bool firstRun, Region region, int level, int succedCount, bool[] secretFound, params object[] stats)
        {
            mainSave = new StreamWriter(File.Open(@"Saves\gs0.dcsv", FileMode.OpenOrCreate, FileAccess.Write));
        }

        public static void Load(string path)
        {
            mainRead = new StreamReader(File.Open(@"Saves\gs0.dcsv", FileMode.Open, FileAccess.Read));

            string @string = "";
            while (!mainRead.EndOfStream)
            {
                string str = mainRead.ReadLine();
                if (str.IndexOf("//") != -1)
                {
                    str = str.Remove(str.IndexOf("//"));
                }
                @string += str;
            }

            for (int i = 0; i < 3; i++)
            {
                string s = @string.Split('.')[i];
                switch (i)
                {
                    #region First Run
                    case 0:
                        {
                            bool firstRun = bool.Parse(s);
                            if (!firstRun)
                            {
                                Info.firstRun = false;
                            }
                        }
                        #endregion
                        break;
                    #region Current Save
                    case 1:
                        {
                            s.Replace("[", "").Replace("]", "");
                            for (int i2 = 0; i2 < 8; i2++)
                            {
                                string s2 = s.Split(';')[i2];
                                switch (i2)
                                {
                                    case 0:
                                        switch (s2)
                                        {
                                            case "Forest":
                                                Info.region = Region.Forest;
                                                break;
                                            case "City":
                                                Info.region = Region.City;
                                                break;
                                            case "Cave":
                                                Info.region = Region.Cave;
                                                break;
                                            case "Ruins":
                                                Info.region = Region.Ruins;
                                                break;
                                        }
                                        break;
                                    case 1:
                                        if (!int.TryParse(s2, out Info.level))
                                        {
                                            Info.level = 0;
                                        }
                                        break;
                                    case 2:
                                        if (!int.TryParse(s2, out Info.money))
                                        {
                                            Info.money = 0;
                                        }
                                        break;
                                    case 3:
                                        //Размер нвентаря
                                        break;
                                    case 4:
                                        //Сам инвентарь
                                        break;
                                    case 5:
                                        //Здоровье
                                        break;
                                    case 6:
                                        //Защита
                                        break;
                                    case 7:
                                        //Мана
                                        break;
                                }
                            }
                        }
                        #endregion
                        break;
                    #region Special
                    case 2:
                        {
                            string s2 = s.Replace("[", "").Replace("]", "");
                            if (!int.TryParse(s2.Split(';')[0], out Info.succedCount))
                            {
                                Info.succedCount = 0;
                            }
                            for (int i2 = 0; i2 < s2.Split(';')[1].Split(',').Length; i2++)
                            {
                                //Логика для секреток. Предположительно... Info: bool <ID> = value;
                            }
                        }
                        #endregion
                        break;
                }
            }
            mainRead.Dispose();
            mainRead.Close();
        }

        #region Settings
        public static void SaveSettings(int musicVolume = -1, int SFXVolume = -1, bool helpEnabled = true)
        {
            settingsSave = new StreamWriter(File.Open("gst.dcset", FileMode.OpenOrCreate, FileAccess.ReadWrite));
            if (musicVolume != -1)
            {
                if (musicVolume > 100)
                {
                    musicVolume = 100;
                }
                settingsSave.WriteLine(musicVolume.ToString());
            }
            else
            {
                settingsSave.WriteLine(Info.MusicLevel.ToString());
            }

            if (SFXVolume != -1)
            {
                if (SFXVolume > 100)
                {
                    SFXVolume = 100;
                }
                settingsSave.WriteLine(SFXVolume.ToString());
            }
            else
            {
                settingsSave.WriteLine(Info.SFXLevel.ToString());
            }

            if (helpEnabled)
            {
                settingsSave.WriteLine("true");
            }
            else
            {
                settingsSave.WriteLine("false");
            }

            settingsSave.Dispose();
            settingsSave.Close();
        }

        public static void LoadSetting()
        {
            settingsRead = new StreamReader(File.Open("settings.dcset", FileMode.OpenOrCreate, FileAccess.ReadWrite));
            string[] settings = settingsRead.ReadToEnd().Split('/');
            Info.SFXLevel = float.Parse(settings[0]);
            Info.MusicLevel = float.Parse(settings[1]);
            Info.helpEnabled = bool.Parse(settings[2]);

            settingsRead.Dispose();
            settingsRead.Close();
        }

        public static void ExportSetting(string path)
        {
            settingsRead = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read));
            string[] settings = settingsRead.ReadToEnd().Split('/');
            Info.SFXLevel = float.Parse(settings[0]);
            Info.MusicLevel = float.Parse(settings[1]);
            Info.helpEnabled = bool.Parse(settings[2]);

            settingsRead.Dispose();
            settingsRead.Close();
        }
        #endregion

        #region Level
        public static void SaveLevel(Region r, int level, bool passed, Vector2 playerPosition, List<bool> lanternsActive, List<Enemy> enemys)
        {

        }

        public static void SaveLevel(Region r, int level, bool passed, Vector2 playerPosition, List<bool> lanternsActive, List<Enemy> enemys, List<bool> secretsFound)
        {

        }

        public static void LoadRegion(Region region, out int levelCount, out bool[] levelDone, out string[] levelsInfo)
        {
            levelCount = 4;
            levelDone = new bool[4];
            levelsInfo = new string[4]; 
            StreamReader r = new StreamReader(@"Content\Files\Regions\" + region.ToString() + ".dcrs");
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
            int lc = 0;
            for (int i = 0; i < 2; i++)
            {
                switch (i)
                {
                    case 0:
                        {
                            if (!int.TryParse(@string.Split('.')[0], out levelCount))
                            {
                                switch (region)
                                {
                                    case Region.Forest:
                                        levelCount = 4;
                                        break;
                                    case Region.City:
                                        levelCount = 4;
                                        break;
                                    case Region.Cave:
                                        levelCount = 4; break;
                                    case Region.Ruins:
                                        levelCount = 4;
                                        break;
                                }
                            }
                            lc = levelCount;
                        }
                        break;
                    case 1:
                        {
                            levelDone = new bool[lc];
                            levelsInfo = new string[lc];
                            for (int i2 = 1; i2<=lc; i2++)
                            {
                                string s = @string.Split('.')[1].Replace("[", "").Replace("]", "").Split(';')[i2];
                                levelDone[lc] = bool.Parse(s.Split(',')[0]);
                                levelsInfo[lc] = s.Substring(s.IndexOf(',') + 1);
                            }
                        }
                        break;
                }
            }
        }
        #endregion

        public static void LoadMap(string path, List<Microsoft.Xna.Framework.Graphics.Texture2D> platformTextureList, Microsoft.Xna.Framework.Graphics.Texture2D lantern, ref Rectangle groundRect, ref List<Point> levelGround, ref List<Platform[]> platformsList)
        {
            StreamReader r = new StreamReader(path);
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
            for (int i = 0; i < 4; i++)
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
                                string data = s.Replace("[", "").Replace("]", "");
                                levelGround.Add(start + new Point(int.Parse(data.Split(',')[0]) * block.X, int.Parse(data.Split(',')[1]) * block.Y));
                            }
                        }
                        break;
                    case 1:
                        {
                            string str = info.Substring(1, info.Length -2);
                            groundRect = new Rectangle(
                                 int.Parse(str.Split(',')[0]), int.Parse(str.Split(',')[1]),
                                 int.Parse(str.Split(',')[2]), int.Parse(str.Split(',')[3]));
                        }
                        break;
                    case 2:
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
                    case 3:
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
                                string s2 = s.Substring(s.IndexOf('<') + 1, s.Length - 2).Replace(">", "");
                                new Lantern(lantern, (start + new Point(block.X * int.Parse(s2.Split(',')[0]), block.Y * int.Parse(s2.Split(',')[1]))).ToVector2(),
                                    new Point(40, 40), 0, new Point(0, 0), new Point(4, 4));
                            }
                        }
                        break;
                }
            }
        }

        public static object[] ParseLuaFile(string luaFilePath)
        {
            return lua.DoFile(luaFilePath);
        }

        public static bool RegisterLuaVariable(string path, object o)
        {
            try
            {
                lua[path] = o;
                return true;
            }
            catch (NLua.Exceptions.LuaException e)
            {
                return false;
            }
        }

        public static object GetLuaVariable(string path)
        {
            return lua[path];
        }

        public static bool RegisterLuaFunction(string path, object target, System.Reflection.MethodBase method)
        {
            try
            {
                lua.RegisterFunction(path, target, method);
                return true;
            }
            catch (NLua.Exceptions.LuaException)
            {
                return false;
            }
        }

        public static LuaFunction GetLuaFunction(string path)
        {
            return lua.GetFunction(path);
        }

        //Вызываем уже в самом конце, что бы закрыть потоки
        public static void Close()
        {
            settingsSave.Dispose();
            settingsSave.Close();
            mainSave.Dispose();
            mainSave.Close();
        }
        #endregion
    }
}
