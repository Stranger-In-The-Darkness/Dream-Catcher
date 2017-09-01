using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DreamCatcher
{
    public class InputManager
    {
        KeyboardState prevKeyboardState, keyboardState = Keyboard.GetState();

        MouseState prevMouseState, mouseState = Mouse.GetState();

        public KeyboardState PrevKeyboardState
        {
            get { return prevKeyboardState; }
            set { prevKeyboardState = value; }
        }

        public KeyboardState KeyboardState
        {
            get { return keyboardState; }
            set { keyboardState = value; }
        }

        public void Update()
        {
            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
        }

        public bool KeyPressed(Keys key)
        {
            if(keyboardState.IsKeyDown(key) && prevKeyboardState.IsKeyUp(key))
            {
                return true;
            }
            return false;
        } 

        public bool KeyPressed(params Keys[] k)
        {
            foreach(Keys key in k)
            {
                if (keyboardState.IsKeyDown(key) && prevKeyboardState.IsKeyUp(key))
                {
                    return true;
                }
            }
            return false;
        }

        public bool KeyReleased(Keys key)
        {
            if (keyboardState.IsKeyUp(key) && prevKeyboardState.IsKeyDown(key))
            {
                return true;
            }
            return false;
        }

        public bool KeyReleased(params Keys[] k)
        {
            foreach (Keys key in k)
            {
                if (keyboardState.IsKeyUp(key) && prevKeyboardState.IsKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }

        public bool KeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public bool KeyDown(params Keys[] k)
        {
            foreach(Keys key in k)
            {
                if (keyboardState.IsKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }

        public Vector2 MouseDirection()
        {
            return new Vector2(mouseState.X - prevMouseState.X, mouseState.Y - prevMouseState.Y);
        }
    }
}
