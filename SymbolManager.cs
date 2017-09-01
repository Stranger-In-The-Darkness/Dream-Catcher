using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace DreamCatcher
{
    public partial class SymbolManager : DrawableGameComponent
    {
        #region Variables
        InputManager inputManager = new InputManager();

        SpriteBatch spriteBatch;

        Texture2D[] symbolVariants = new Texture2D[5]; 
        List<Symbol> symbolList = new List<Symbol>();

        Rectangle exclude = new Rectangle(0, 0, 0, 0);

        Random rnd = new Random();
        #endregion

        public SymbolManager(Game game) : base(game)
        {
            
        }

        public SymbolManager(Game game, Rectangle exclude) : base(game)
        {
            this.exclude = exclude;
        }

        #region Methods

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            for(int i = 1; i<6; i++)
            {
                symbolVariants[i - 1] = MainClass.Load<Texture2D>(@"Images\Symbol" + i);
            }
            Symbol s = new Symbol(symbolVariants[rnd.Next(0, 5)], new Vector2(rnd.Next(0, 800 - symbolVariants[0].Bounds.Width), rnd.Next(0, 600 - symbolVariants[0].Bounds.Height)));
            if (s.CollisionRect.Intersects(exclude))
            {
                if (Math.Abs(s.GetCenter.X - (exclude.X + exclude.Width)/2) > Math.Abs(s.GetCenter.Y - (exclude.Y + exclude.Height)/2))
                {
                    Vector2 v = s.Position;
                    v.X = v.X + (s.GetCenter.X - ((exclude.X + exclude.Width) / 2) > 0 ? s.GetCenter.X - ((exclude.X + exclude.Width) / 2) : -s.GetCenter.X - ((exclude.X + exclude.Width) / 2));
                    s.Position = v;
                }
                else if (Math.Abs(s.GetCenter.X - (exclude.X + exclude.Width) / 2) == Math.Abs(s.GetCenter.Y - (exclude.Y + exclude.Height) / 2))
                {
                    Vector2 v = s.Position;
                    v.X = v.X + (s.GetCenter.X - ((exclude.X + exclude.Width) / 2) > 0 ? s.GetCenter.X - ((exclude.X + exclude.Width) / 2) : -s.GetCenter.X - ((exclude.X + exclude.Width) / 2));
                    v.Y = v.Y + (s.GetCenter.Y - ((exclude.Y + exclude.Height) / 2) > 0 ? s.GetCenter.Y - ((exclude.Y + exclude.Height) / 2) : -s.GetCenter.Y - ((exclude.Y + exclude.Height) / 2));
                    s.Position = v;
                }
                else
                {
                    Vector2 v = s.Position;
                    v.Y = v.Y + (s.GetCenter.Y - ((exclude.Y + exclude.Height) / 2) > 0 ? s.GetCenter.Y - ((exclude.Y + exclude.Height) / 2) : -s.GetCenter.Y - ((exclude.Y + exclude.Height) / 2));
                    s.Position = v;
                }
            }
            symbolList.Add(s);
            //switch(rnd.Next(0, 4))
            //{
            //    case 0:
            //        symbolList.Add(new Symbol(symbolVariants[rnd.Next(0, 5)], new Vector2(rnd.Next(0, 236), rnd.Next(0, 174))));
            //        break;
            //    case 1:
            //        symbolList.Add(new Symbol(symbolVariants[rnd.Next(0, 5)], new Vector2(rnd.Next(468, 704), rnd.Next(0, 174))));
            //        break;
            //    case 2:
            //        symbolList.Add(new Symbol(symbolVariants[rnd.Next(0, 5)], new Vector2(rnd.Next(0, 236), rnd.Next(240, 474))));
            //        break;
            //    case 3:
            //        symbolList.Add(new Symbol(symbolVariants[rnd.Next(0, 5)], new Vector2(rnd.Next(468, 704), rnd.Next(240, 474))));
            //        break;
            //}
        }

        public override void Update(GameTime gameTime)
        {
            symbolList[0].Update(gameTime);
            if(symbolList.Count > 1)
            {
                symbolList[1].Update(gameTime);
            }
            if(symbolList[0].millisecondsTillDematerialize < 1400)
            {
                if(symbolList.Count < 2)
                {
                    Symbol s = new Symbol(symbolVariants[rnd.Next(0, 5)], new Vector2(rnd.Next(0, 800 - symbolVariants[0].Bounds.Width), rnd.Next(0, 600 - symbolVariants[0].Bounds.Height)));
                    if (s.CollisionRect.Intersects(exclude))
                    {
                        if (Math.Abs(s.GetCenter.X - (exclude.X + exclude.Width) / 2) > Math.Abs(s.GetCenter.Y - (exclude.Y + exclude.Height) / 2))
                        {
                            Vector2 v = s.Position;
                            v.X = v.X + (s.GetCenter.X - ((exclude.X + exclude.Width) / 2) > 0 ? s.GetCenter.X - ((exclude.X + exclude.Width) / 2) : -s.GetCenter.X - ((exclude.X + exclude.Width) / 2));
                            s.Position = v;
                        }
                        else if (Math.Abs(s.GetCenter.X - (exclude.X + exclude.Width) / 2) == Math.Abs(s.GetCenter.Y - (exclude.Y + exclude.Height) / 2))
                        {
                            Vector2 v = s.Position;
                            v.X = v.X + (s.GetCenter.X - ((exclude.X + exclude.Width) / 2) > 0 ? s.GetCenter.X - ((exclude.X + exclude.Width) / 2) : -s.GetCenter.X - ((exclude.X + exclude.Width) / 2));
                            v.Y = v.Y + (s.GetCenter.Y - ((exclude.Y + exclude.Height) / 2) > 0 ? s.GetCenter.Y - ((exclude.Y + exclude.Height) / 2) : -s.GetCenter.Y - ((exclude.Y + exclude.Height) / 2));
                            s.Position = v;
                        }
                        else
                        {
                            Vector2 v = s.Position;
                            v.Y = v.Y + (s.GetCenter.Y - ((exclude.Y + exclude.Height) / 2) > 0 ? s.GetCenter.Y - ((exclude.Y + exclude.Height) / 2) : -s.GetCenter.Y - ((exclude.Y + exclude.Height) / 2));
                            s.Position = v;
                        }
                    }
                    symbolList.Add(s);
                    //switch (rnd.Next(0, 4))
                    //{
                    //    case 0:
                    //        symbolList.Add(new Symbol(symbolVariants[rnd.Next(0, 5)], new Vector2(rnd.Next(0, 236), rnd.Next(0, 174))));
                    //        break;
                    //    case 1:
                    //        symbolList.Add(new Symbol(symbolVariants[rnd.Next(0, 5)], new Vector2(rnd.Next(468, 704), rnd.Next(0, 174))));
                    //        break;
                    //    case 2:
                    //        symbolList.Add(new Symbol(symbolVariants[rnd.Next(0, 5)], new Vector2(rnd.Next(0, 236), rnd.Next(240, 474))));
                    //        break;
                    //    case 3:
                    //        symbolList.Add(new Symbol(symbolVariants[rnd.Next(0, 5)], new Vector2(rnd.Next(468, 704), rnd.Next(240, 474))));
                    //        break;
                    //}
                }
            }
            if(symbolList[0].millisecondsTillDematerialize < 0)
            {
                symbolList.RemoveAt(0);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            symbolList[0].Draw(gameTime, spriteBatch);
            if (symbolList.Count > 1)
            {
                symbolList[1].Draw(gameTime, spriteBatch);
            }
            base.Draw(gameTime);
        }

        #endregion
    }
}
