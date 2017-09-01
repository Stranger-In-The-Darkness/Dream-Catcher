using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;

namespace DreamCatcher
{
    public class Symbol
    {

        #region Variables

        Texture2D symbol;
        Vector2 position;
        float symbolOpacity = 0.0f;
        public int millisecondsTillDematerialize = 7000;

        #endregion

        public Symbol(Texture2D texture, Vector2 position)
        {
            symbol = texture;
            this.position = position;
        }

        #region Methods

        public void Update(GameTime gameTime)
        {
            millisecondsTillDematerialize -= gameTime.ElapsedGameTime.Milliseconds;
            if(millisecondsTillDematerialize >= 3500)
            {
                symbolOpacity += 0.02f;
                if(symbolOpacity >= 0.75f)
                {
                    symbolOpacity = 0.75f;
                }
            }
            else
            {
                symbolOpacity -= 0.02f;
                if(symbolOpacity <= 0.0f)
                {
                    symbolOpacity = 0.0f;
                }
            }
        }

        public void Draw(GameTime gameTime ,SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Draw(symbol, position, null, Color.White * symbolOpacity, 0.0f, Vector2.Zero, 0.3f, SpriteEffects.None, 1.0f);
            spriteBatch.End();
        }

        #endregion

        #region Properties
        public Rectangle CollisionRect
        {
            get
            {
                return symbol.Bounds;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public Vector2 GetCenter
        {
            get
            {
                return new Vector2(Position.X + CollisionRect.Width / 2, Position.Y + CollisionRect.Height / 2);
            }
        }
        #endregion
    }
}
