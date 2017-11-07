using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DreamCatcher
{
    public delegate void Activity<T>(Nullable<T> t) where T: struct;

    /// <summary>
    /// Check box settings element
    /// </summary>
    public class CheckBox
    {
        Texture2D texture;
        Rectangle rectangle;
        Vector2 position;
        bool @checked = false;

        Activity<bool> activity = null;

        MouseState prevMouseState = Mouse.GetState();

        InputManager manager = new InputManager();

        bool playAnimation = false;
        Point sheetSize;
        Point currentFrame;
        Point frameSize;
        int millisecondsPerFrame;
        int timeSinceLastFrame = 0;
        const int defaultMillisecondsPerFrame = 16;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture">Spritesheet</param>
        /// <param name="rectangle">"Active" rectangle</param>
        /// <param name="position">Position</param>
        /// <param name="sheetSize">Sheet size</param>
        /// <param name="currentFrame">Current frame</param>
        /// <param name="frameSize">Frame size</param>
        /// <param name="check">State of the check box. Default: false</param>
        /// <param name="millisecondsPerFrame">Milliseconds per each animation frame</param>
        public CheckBox(Texture2D texture, Rectangle rectangle, Vector2 position, Point sheetSize, Point currentFrame, Point frameSize, bool check = false, int millisecondsPerFrame = defaultMillisecondsPerFrame)
        {
            this.texture = texture;
            this.rectangle = rectangle;
            this.position = position;
            this.@checked = check;
            this.sheetSize = sheetSize;
            this.currentFrame = currentFrame;
            this.frameSize = frameSize;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }
        
        public void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState();
            if(rectangle.Contains(state.Position) &&
                rectangle.Contains(prevMouseState.Position) &&
                state.LeftButton == ButtonState.Released &&
                state.LeftButton == ButtonState.Pressed)
            {
                Checked = !Checked;
                playAnimation = true;
            }
            if (playAnimation)
            {
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if(timeSinceLastFrame >= millisecondsPerFrame)
                {
                    if (Checked)
                    {
                        currentFrame.X++;
                        if (currentFrame.X >= sheetSize.X)
                        {
                            currentFrame.X = 0;
                            currentFrame.Y++;
                            if (currentFrame.Y >= sheetSize.Y)
                            {
                                currentFrame.X = sheetSize.X - 1;
                                currentFrame.Y = sheetSize.Y - 1;
                                playAnimation = false;
                            }
                        }
                    }
                    else
                    {
                        currentFrame.X--;
                        if (currentFrame.X < 0)
                        {
                            currentFrame.X = sheetSize.X - 1;
                            currentFrame.Y--;
                            if (currentFrame.Y < 0) 
                            {
                                currentFrame.X = 0;
                                currentFrame.Y = 0;
                                playAnimation = false;
                            }
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            try
            {
                spriteBatch.Draw(texture, position, new Rectangle(frameSize.X * currentFrame.X, frameSize.Y * currentFrame.Y, frameSize.X, frameSize.Y), Color.White);
            }
            catch
            {
                spriteBatch.Begin();
                spriteBatch.Draw(texture, position, new Rectangle(frameSize.X * currentFrame.X, frameSize.Y * currentFrame.Y, frameSize.X, frameSize.Y), Color.White);
                spriteBatch.End();
            }
        }

        /// <summary>
        /// Changes current activity to another not null activity
        /// </summary>
        /// <param name="activity">New activity</param>
        public void ChangeActivity(Activity<bool> activity)
        {
            if (activity != null)
            {
                this.activity = activity;
            }
        }

        /// <summary>
        /// State of check box
        /// </summary>
        public bool Checked
        {
            get
            {
                return @checked;
            }
            set
            {
                if(value != @checked)
                {
                    @checked = value;
                    activity.Invoke(@checked);
                }
            }
        }

        /// <summary>
        /// Slider current activity
        /// </summary>
        public Activity<bool> Activity
        {
            get
            {
                return activity;
            }
            set
            {
                if(activity == null)
                {
                    activity = value;
                }
                else
                {
                    throw new Exception("Current object activity is already settled.");
                }
            }
        }
    }
}
