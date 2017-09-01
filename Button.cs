using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamCatcher
{
    delegate void Action(string s); 

    public class Button
    {
        #region Variabes
        Texture2D buttonTexture;
        Vector2 buttonPosition = new Vector2();
        Point buttonFrameSize = new Point(136, 106);
        int buttonOffset = 10;
        float buttonOpacity = 1.0f;
        bool buttonIsPressed = false;
        bool enabled;
        List<string> Tags = new List<string>();

        Action a;
        #endregion

        #region Constructors
        public Button(Texture2D texture, Vector2 position,
            Point frameSize, int offset, float opacity, bool enabled = true)
        {
            buttonTexture = texture;
            buttonPosition = position;
            buttonFrameSize = frameSize;
            buttonOffset = offset;
            buttonOpacity = opacity;
            this.enabled = enabled;
        }

        public Button(Texture2D texture, Vector2 position,
    Point frameSize, int offset, float opacity, bool enabled = true, params string[] Tag)
        {
            buttonTexture = texture;
            buttonPosition = position;
            buttonFrameSize = frameSize;
            buttonOffset = offset;
            buttonOpacity = opacity;
            if (Tag.Length != 0) foreach (string s in Tag)
                {
                    Tags.Add(s);
                }
            this.enabled = enabled;
        }
        #endregion

        #region Methods
        public void Update(GameTime gameTime)
        {
            if (enabled)
            {
                if (new Rectangle((int)buttonPosition.X, (int)buttonPosition.Y, buttonFrameSize.X, buttonFrameSize.Y).
                    Intersects(new Rectangle(Mouse.GetState().X - 5, Mouse.GetState().Y - 5, 35, 35)))
                {
                    buttonOpacity = 1;
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        buttonIsPressed = true;
                        buttonOpacity = 1;
                    }
                    else if (Mouse.GetState().LeftButton == ButtonState.Released && buttonIsPressed)
                    {
                        string s = "";
                        if (Tags.Count != 0)
                        {
                            foreach (string tag in Tags)
                            {
                                s += tag + ",";
                            }
                        }
                        a.Invoke(s);
                        buttonOpacity = 0.9f;
                    }
                }
                else
                {
                    buttonIsPressed = false;
                    buttonOpacity = 0.9f;
                }
            }
            else
            {
                buttonOpacity = 0.5f;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(buttonTexture, buttonPosition, Color.White * Opacity);
            spriteBatch.End();
        }
        #endregion

        #region Properties
        public float Opacity
        {
            get
            {
                return buttonOpacity;
            }
            set
            {
                if(value > 1)
                {
                    buttonOpacity = 1;
                }
                else if(value < 0)
                {
                    buttonOpacity = 0;
                }
            }
        }

        public void AddAction(Action<string> a)
        {
            this.a = new Action(a);
        }

        public List<string> Tag
        {
            get
            {
                return Tags;
            }
            set
            {
                Tags = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                if (enabled ^ value)
                {
                    enabled = value;
                }
            }
        }
        #endregion
    }
}
