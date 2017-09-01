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
        SpriteFont arial;
        SpriteFont curlz;
        SpriteFont sans;
        SpriteFont papyrus;

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
            arial = MainClass.Load<SpriteFont>(@"Fonts\Arial");
            curlz = MainClass.Load<SpriteFont>(@"Fonts\Curlz");
            papyrus = MainClass.Load<SpriteFont>(@"Fonts\Papyrus");
            sans = MainClass.Load<SpriteFont>(@"Fonts\ComicSans");
            textFrame = MainClass.Load<Texture2D>(@"Images\Frame");
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
                    spriteBatch.DrawString(arial, text, position, color);
                    break;
                case Fonts.CurlzMT:
                    spriteBatch.DrawString(curlz, text, position, color);
                    break;
                case Fonts.Papyrus:
                    spriteBatch.DrawString(papyrus, text, position, color);
                    break;
                case Fonts.Sans:
                    spriteBatch.DrawString(sans, text, position, color);
                    break;
            }
            spriteBatch.End();
        }

        public void DialogDraw(GameTime gameTime, Fonts font, string text, Color color)
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Draw(textFrame, new Rectangle(80, 380, 640, 160), Color.White);
            switch (font)
            {
                case Fonts.Arial:
                    spriteBatch.DrawString(arial, text, new Vector2(142, 424), color);
                    break;
                case Fonts.CurlzMT:
                    spriteBatch.DrawString(curlz, text, new Vector2(142, 424), color);
                    break;
                case Fonts.Papyrus:
                    spriteBatch.DrawString(papyrus, text, new Vector2(142, 424), color);
                    break;
                case Fonts.Sans:
                    spriteBatch.DrawString(sans, text, new Vector2(142, 424), color);
                    break;
            }
            spriteBatch.End();
        }
    }
}
