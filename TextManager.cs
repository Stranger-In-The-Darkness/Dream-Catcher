using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamCatcher
{
    public class TextManager : DrawableGameComponent
    {
        SpriteFont arial16;
        SpriteFont curlz24;
        SpriteFont chiller16;
        SpriteFont sans16;
        SpriteFont papyrus16;
        SpriteFont gigi16;

        Texture2D textFrame;
        SpriteBatch spriteBatch;

        public TextManager(Game game) : base(game)
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            arial16 = Info.Load<SpriteFont>(@"Fonts\Arial16");
            curlz24 = Info.Load<SpriteFont>(@"Fonts\Curlz24");
            papyrus16 = Info.Load<SpriteFont>(@"Fonts\Papyrus16");
            sans16 = Info.Load<SpriteFont>(@"Fonts\ComicSans16");
            chiller16 = Info.Load<SpriteFont>(@"Fonts\Chiller16");
            gigi16 = Info.Load<SpriteFont>(@"Fonts\Gigi16");
            textFrame = Info.Load<Texture2D>(@"Images\Frame");
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public void Draw(GameTime gameTime, Fonts font, string text, Vector2 position, Color color)
        {
            spriteBatch.Begin();
            switch (font)
            {
                case Fonts.Arial:
                    spriteBatch.DrawString(arial16, text, position, color);
                    break;
                case Fonts.Chiller:
                    spriteBatch.DrawString(chiller16, text, position, color);
                    break;
                case Fonts.CurlzMT:
                    spriteBatch.DrawString(curlz24, text, position, color);
                    break;
                case Fonts.Papyrus:
                    spriteBatch.DrawString(papyrus16, text, position, color);
                    break;
                case Fonts.Sans:
                    spriteBatch.DrawString(sans16, text, position, color);
                    break;
            }
            spriteBatch.End();
        }

        public void DialogDraw(GameTime gameTime, Fonts font, string text, Color color)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Draw(textFrame, new Rectangle(70, 400, 640, 160), Color.White);
            switch (font)
            {
                case Fonts.Arial:
                    spriteBatch.DrawString(arial16, text, new Vector2(142, 444), color);
                    break;
                case Fonts.Chiller:
                    spriteBatch.DrawString(chiller16, text, new Vector2(142, 444), color);
                    break;
                case Fonts.CurlzMT:
                    spriteBatch.DrawString(curlz24, text, new Vector2(142, 444), color);
                    break;
                case Fonts.Papyrus:
                    spriteBatch.DrawString(papyrus16, text, new Vector2(142, 444), color);
                    break;
                case Fonts.Sans:
                    spriteBatch.DrawString(sans16, text, new Vector2(142, 444), color);
                    break;
            }
            spriteBatch.End();
        }
    }
}
