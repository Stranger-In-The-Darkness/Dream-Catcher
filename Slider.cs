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
    /// <summary>
    /// Settings slider element
    /// </summary>
    public class Slider
    {
        Texture2D frame;
        Texture2D slider;
        Rectangle frameRect;
        Rectangle sliderRect;
        Vector2 framePosition;
        Vector2 sliderPrevPosition;
        Vector2 sliderPosition;
        int move;
        bool enabled = true;
        MouseState prevMouseState = Mouse.GetState();
        InputManager inputManager = new InputManager();

        Activity<int> activity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame">Slider frame texture</param>
        /// <param name="slider">Slider texture</param>
        /// <param name="frameRectangle">Frame "working" rectangle</param>
        /// <param name="sliderRectangle">Slider rectangle</param>
        /// <param name="framePosition">Frame position</param>
        /// <param name="sliderPosition">Slider position</param>
        public Slider(Texture2D frame, Texture2D slider, Rectangle frameRectangle, Rectangle sliderRectangle, Vector2 framePosition, Vector2 sliderPosition)
        {
            this.frame = frame;
            this.slider = slider;
            this.frameRect = frameRectangle;
            this.sliderRect = sliderRectangle;
            this.framePosition = framePosition;
            this.sliderPosition = sliderPosition;
            sliderPrevPosition = sliderPosition;
            move = frameRect.Width / 100;
            if (move < 0)
            {
                move = 1;
            }
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (sliderRect.Contains(mouseState.Position) && 
                mouseState.LeftButton == ButtonState.Pressed && 
                prevMouseState.LeftButton == ButtonState.Pressed)
            {
                sliderPosition.X += Math.Abs(prevMouseState.Position.X - mouseState.Position.X);
            }

            if(sliderPosition.X + sliderRect.Width > frameRect.Right)
            {
                sliderPosition.X = frameRect.Right - sliderRect.Width;
            }
            else if(sliderPosition.X < frameRect.Left)
            {
                sliderPosition.X = frameRect.Left;
            }

            activity.Invoke((mouseState.X - prevMouseState.X) / move);
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            try
            {
                spriteBatch.Draw(frame, framePosition, Color.White);
                spriteBatch.Draw(slider, sliderPosition, Color.White);
            }
            catch
            {
                spriteBatch.Begin();
                spriteBatch.Draw(frame, framePosition, Color.White);
                spriteBatch.Draw(slider, sliderPosition, Color.White);
                spriteBatch.End();
            }
        }

        /// <summary>
        /// Changes current activity to another not null activity
        /// </summary>
        /// <param name="activity">New activity</param>
        public void ChangeActivity(Activity<int> activity)
        {
            if (activity != null)
            {
                this.activity = activity;
            }
        }

        /// <summary>
        /// Slider current activity
        /// </summary>
        public Activity<int> Activity
        {
            get
            {
                return activity;
            }
            set
            {
                if (activity == null)
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
