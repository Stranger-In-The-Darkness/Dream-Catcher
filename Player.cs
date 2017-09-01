using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamCatcher
{
    /// <summary>
    /// Main type for player. 
    /// </summary>
    public class Player : UserControlledSprite
    {
        #region Variables
        //Тексрура:
        //0 - стоит
        //1 - идёт
        //2 - прыгает
        //3 - атакует

        //Что бы не перегружать кучей переменных, звуковые эффекты хранятся в одной переменной
        SoundEffect currentEffect = null;

        //Состояние
        State state = State.Stay;

        InputManager inputManager = new InputManager();

        Dir dir = Dir.Right;

        //Выстрелы
        List<Bullet> bulletsList = new List<Bullet>();

        int health;
        int mana;
        int shield;
        int lives = 3; //Value will change!!!
        int attackPoints;

        Point respawnPoint = Point.Zero;

        int bagCapacity = 10;

        bool gravityOn = false;
        bool isGrounded = true;

        bool isAttacking = false;
        bool isInventoryOpen = false;

        float yBound = 0;

        Inventory inventory;

        Rectangle backFrame = new Rectangle(0, 0, 800, 600);
        Rectangle viewFrame = new Rectangle(0, 0, 800, 600);

        Nullable<Rectangle> collision = null;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textureImage">Sprite sheet of player character</param>
        /// <param name="position">Position of player</param>
        /// <param name="frameSize">Frame size of player character animation</param>
        /// <param name="collisionOffset">Collision offset of player</param>
        /// <param name="currentFrame">Current frame of player character animation</param>
        /// <param name="sheetSize">Sprite sheet size [in frames]</param>
        /// <param name="speed">Initial speed of player</param>
        /// <param name="millisecondsPerFrame">Milliseconds per frame of player character animation</param>
        public Player(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
            inventory = new Inventory(bagCapacity);
            respawnPoint = position.ToPoint();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textureImage">Sprite sheet of player character</param>
        /// <param name="position">Position of player</param>
        /// <param name="frameSize">Frame size of player character animation</param>
        /// <param name="collisionOffset">Collision offset of player</param>
        /// <param name="currentFrame">Current frame of player character animation</param>
        /// <param name="sheetSize">Sprite sheet size [in frames]</param>
        /// <param name="speed">Initial speed of player</param>
        public Player(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
            inventory = new Inventory(bagCapacity);
            respawnPoint = position.ToPoint();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textureImage">Sprite sheet of player character</param>
        /// <param name="position">Position of player</param>
        /// <param name="frameSize">Frame size of player character animation</param>
        /// <param name="collisionOffset">Collision offset of player</param>
        /// <param name="currentFrame">Current frame of player character animation</param>
        /// <param name="sheetSize">Sprite sheet size [in frames]</param>
        /// <param name="speed">Initial speed of player</param>
        /// <param name="millisecondsPerFrame">Milliseconds per frame of player character animation</param>
        /// <param name="collisionRectangle">Custom player collision rectangle</param>
        public Player(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, Rectangle collisionRectangle)
    : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
            collision = collisionRectangle;
            inventory = new Inventory(bagCapacity);
            yBound = position.Y + 3;
            respawnPoint = position.ToPoint();
        }

        #endregion

        #region Methods
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            #region Bounds_Logic
            if (position.X < 0) position.X = 0;
            if (position.X > clientBounds.Width / 0.75 - frameSize.X) position.X = clientBounds.Width / 0.75f - frameSize.X;
            if (position.Y > yBound + 20) { position.Y = yBound + 20; state = State.Stay; }
            #endregion

            inputManager.Update();

            position += Direction;

            #region View_Logic
            backFrame.X = (int)(0.75 * (GetCenter.X - 400));
            backFrame.Y = (int)(0.75 * (GetCenter.Y - 300));

            backFrame.X = backFrame.X < 0 ?
                0 : backFrame.X > clientBounds.Width - 800 ?
                clientBounds.Width - 800 : backFrame.X;
            backFrame.Y = backFrame.Y < 0 ?
                0 : backFrame.Y > clientBounds.Height - 600 ?
                clientBounds.Height - 600 : backFrame.Y;

            viewFrame.X = (int)(GetCenter.X - 400);
            viewFrame.Y = (int)(GetCenter.Y - 300);

            viewFrame.X = viewFrame.X < 0 ?
                0 : viewFrame.X > clientBounds.Width / 0.75 - 800 ?
                (int)(clientBounds.Width / 0.75) - 800 : viewFrame.X;
            viewFrame.Y = viewFrame.Y < 0 ?
                0 : viewFrame.Y > clientBounds.Height / 0.75 - 600 ?
                (int)(clientBounds.Height / 0.75) - 600 : viewFrame.Y;
            #endregion

            switch (state)
            {
                #region Stay
                case State.Stay:
                        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

                        if (timeSinceLastFrame > millisecondsPerFrame)
                        {
                            timeSinceLastFrame -= millisecondsPerFrame;
                            ++currentFrame.X;

                            if (currentFrame.X >= sheetSize.X)
                            {
                                currentFrame.X = 0;
                                currentFrame.Y++;
                                if (currentFrame.Y >= sheetSize.Y/3)
                                {
                                    currentFrame.Y = 0;
                                }
                            }
                        }
                    break;
                #endregion

                #region Walk
                case State.Walk:
                        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

                        if (timeSinceLastFrame > millisecondsPerFrame)
                        {
                            timeSinceLastFrame -= millisecondsPerFrame;
                            ++currentFrame.X;

                            if (currentFrame.X >= sheetSize.X)
                            {
                                currentFrame.X = 0;
                                currentFrame.Y++;
                                if (currentFrame.Y >= (sheetSize.Y * 2)/3)
                                {
                                    currentFrame.Y = sheetSize.Y/3;
                                }
                            }
                        }
                        if (!Keyboard.GetState().IsKeyDown(Keys.Left) &&
                            !Keyboard.GetState().IsKeyDown(Keys.A) &&
                            !Keyboard.GetState().IsKeyDown(Keys.Right) &&
                            !Keyboard.GetState().IsKeyDown(Keys.D))
                        {
                            state = State.Stay;
                            currentFrame.X = 0;
                            currentFrame.Y = 0;
                        }
                    break;
                #endregion

                #region Jump
                case State.Jump:
                    bool enabled = true;

                    timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

                    if (timeSinceLastFrame > millisecondsPerFrame)
                    {
                        timeSinceLastFrame -= millisecondsPerFrame;
                        ++currentFrame.X;
                        if (Direction.Y <= 0)
                        {
                            if (currentFrame.X >= sheetSize.X && currentFrame.Y >= (sheetSize.Y * 2) / 3)
                            {
                                currentFrame.X = sheetSize.X - 1;
                                currentFrame.Y = (sheetSize.Y * 2) / 3;
                                enabled = false;
                            }
                        }
                        else
                        {
                            if (currentFrame.X >= 4 && currentFrame.Y >= sheetSize.Y)
                            {
                                currentFrame.X = sheetSize.X / 2 - 1;
                                currentFrame.Y = (sheetSize.Y * 2) / 3 + 1;
                                enabled = false;
                            }
                        }
                        if (enabled)
                        {
                            if (currentFrame.X >= sheetSize.X)
                            {
                                currentFrame.X = 0;
                                currentFrame.Y++;
                            }
                        }
                    }
                    break;
                #endregion

                #region Landed
                case State.Landed:
                    {
                        bool enabled2 = true;
                        dir = dir == Dir.Left_Down || dir == Dir.Left_Up ? Dir.Left : dir == Dir.Right_Down || dir == Dir.Right_Up ? Dir.Right : dir;

                        timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

                        if (timeSinceLastFrame > millisecondsPerFrame)
                        {
                            timeSinceLastFrame -= millisecondsPerFrame;
                            ++currentFrame.X;

                            if (currentFrame.X >= sheetSize.X && currentFrame.Y >= sheetSize.Y)
                            {
                                state = State.Stay;
                                currentFrame.X = 0;
                                currentFrame.Y = 0;
                                enabled2 = false;
                            }

                            if (enabled2)
                            {
                                if (currentFrame.X >= sheetSize.X)
                                {
                                    currentFrame.X = 0;
                                    currentFrame.Y++;

                                    if (currentFrame.Y >= sheetSize.Y)
                                    {
                                        currentFrame.Y = (sheetSize.Y * 2) / 3;
                                    }
                                }
                            }
                        }
                    }
                    break;
                    #endregion
            }

            #region Gravity_Logic
            if (gravityOn || !isGrounded)
            {
                if (speed.Y < 0)
                {
                    state = State.Jump;
                    currentFrame.X = 0;
                    currentFrame.Y = (sheetSize.Y * 2) / 3;
                    dir = (dir == Dir.Left || dir == Dir.Left_Up) ? dir = Dir.Left_Down : (dir == Dir.Right || dir == Dir.Right_Up) ? Dir.Right_Down : dir;
                }
                speed.Y -= 0.06f;
                speed.Y = speed.Y < -25 ? -25 : speed.Y;
            }
            #endregion
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            switch (state)
            {
                #region Attack
                //case State.Attack:
                //    spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White);
                //    break;
                #endregion

                #region Stay
                case State.Stay:
                    if (dir == Dir.Right) spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White);
                    else if (dir == Dir.Left) spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                    if (isInventoryOpen)
                    {
                        inventory.Draw(gameTime, spriteBatch);
                    }
                    break;
                #endregion

                #region Walk
                case State.Walk:
                    if (dir == Dir.Right) spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White);
                    else if (dir == Dir.Left) spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                    if (isInventoryOpen)
                    {
                        inventory.Draw(gameTime, spriteBatch);
                    }
                    break;
                #endregion

                #region Jump
                case State.Jump:
                    if (dir == Dir.Right_Up || dir == Dir.Right_Down || dir == Dir.Right) spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White);
                    else if (dir == Dir.Left_Up || dir == Dir.Left_Down || dir == Dir.Left) spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                    break;
                #endregion

                #region Landed
                case State.Landed:
                    if (dir == Dir.Right_Up || dir == Dir.Right_Down || dir == Dir.Right) spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White);
                    else if (dir == Dir.Left_Up || dir == Dir.Left_Down || dir == Dir.Left) spriteBatch.Draw(textureImage, position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                    break;
                    #endregion
            }
        }

        /// <summary>
        /// Overload of Draw method, depending on draw frame position
        /// </summary>
        /// <param name="gameTime">In-game time</param>
        /// <param name="drawFramePosition">Position of draw frame</param>
        new public void Draw(GameTime gameTime, Vector2 drawFramePosition, SpriteBatch spriteBatch)
        {
            switch (state)
            {
                #region Attack
                //case State.Attack:
                //    spriteBatch.Draw(textureImage, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y),
                //        new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White);
                //    break;
                #endregion

                #region Stay
                case State.Stay:
                    if (dir == Dir.Right) spriteBatch.Draw(textureImage, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y),
                         new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White);
                    else if (dir == Dir.Left) spriteBatch.Draw(textureImage, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y),
     new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);

                    if (isInventoryOpen)
                    {
                        inventory.Draw(gameTime, spriteBatch);
                    }
                    break;
                #endregion

                #region Walk
                case State.Walk:
                    if (dir == Dir.Right) spriteBatch.Draw(textureImage, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y),
                         new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White);
                    else if (dir == Dir.Left) spriteBatch.Draw(textureImage, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y),
                         new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);

                    if (isInventoryOpen)
                    {
                        inventory.Draw(gameTime, spriteBatch);
                    }
                    break;
                #endregion

                #region Jump
                case State.Jump:
                    if (dir == Dir.Right_Up || dir == Dir.Right_Down || dir == Dir.Right) spriteBatch.Draw(textureImage, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y),
                         new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White);
                    else if (dir == Dir.Left_Up || dir == Dir.Left_Down || dir == Dir.Left) spriteBatch.Draw(textureImage, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y),
     new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                    break;
                #endregion

                #region Landed
                case State.Landed:
                    if (dir == Dir.Right_Up || dir == Dir.Right_Down || dir == Dir.Right) spriteBatch.Draw(textureImage, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y),
     new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White);
                    else if (dir == Dir.Left_Up || dir == Dir.Left_Down || dir == Dir.Left) spriteBatch.Draw(textureImage, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y),
     new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                    break;
                    #endregion
            }
        }

        public void DrawUserInterface(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Health, mana, shield and lives interface
        }

        /// <summary>
        /// Method to check collision of player and platforms
        /// </summary>
        /// <param name="p">List of platforms</param>
        public void CollisionCheck(List<Platform> p)
        {
            isGrounded = false;
            foreach (Platform pl in p)
            {
                if (CollisionRect.Intersects(pl.CollisionRect))
                {
                    if (dir == Dir.Left_Down || dir == Dir.Right_Down)
                    {
                        //Если игрок над платформой, то он останавливается на ней
                        if (CollisionRect.Bottom - pl.CollisionRect.Top > 0 
                            && CollisionRect.Bottom - pl.CollisionRect.Top < 10)
                        {
                            if (state == State.Jump)
                            {
                                state = State.Landed;
                                currentFrame.X = 4;
                                currentFrame.Y = 5;
                            }
                            gravityOn = false;
                            speed.Y = 0;
                            isGrounded = true;
                            dir = dir == Dir.Left_Down ? Dir.Left : dir == Dir.Right_Down ? Dir.Right : dir;
                        }
                    }
                    else if (state != State.Jump)
                    {
                        if (!(dir == Dir.Left && (pl.CollisionRect.Right - CollisionRect.Right > 30)) 
                            || !(dir == Dir.Right && (pl.CollisionRect.Left - CollisionRect.Left < -30)))
                        {
                            if (CollisionRect.Bottom - pl.CollisionRect.Top > 0 
                                && CollisionRect.Bottom - pl.CollisionRect.Top < 10)
                            {
                                speed.Y = 0;
                                isGrounded = true;
                                if(state != State.Stay && state != State.Walk)
                                {
                                    state = State.Stay;
                                }
                            }
                        }
                        else
                        {
                            isGrounded = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Overload of method to check collision of player and ground
        /// </summary>
        /// <param name="r">Ground rectangle</param>
        public void CollisionCheck(Rectangle r)
        {
            isGrounded = false;
            if (CollisionRect.Intersects(r))
            {
                if (dir == Dir.Left_Down || dir == Dir.Right_Down)
                {
                    //Если игрок над платформой, то он останавливается на ней
                    if (CollisionRect.Bottom - r.Top > 0
                        && CollisionRect.Bottom - r.Top < 10)
                    {
                        if (state == State.Jump)
                        {
                            state = State.Landed;
                            currentFrame.X = 4;
                            currentFrame.Y = 5;
                        }
                        gravityOn = false;
                        speed.Y = 0;
                        isGrounded = true;
                        dir = dir == Dir.Left_Down ? Dir.Left : dir == Dir.Right_Down ? Dir.Right : dir;
                    }
                }
                else if (state != State.Jump)
                {
                    if (!(dir == Dir.Left && (r.Right - CollisionRect.Right > 30))
                        || !(dir == Dir.Right && (r.Left - CollisionRect.Left < -30)))
                    {
                        if (CollisionRect.Bottom - r.Top > 0
                            && CollisionRect.Bottom - r.Top < 10)
                        {
                            speed.Y = 0;
                            isGrounded = true;
                            if (state != State.Stay && state != State.Walk)
                            {
                                state = State.Stay;
                            }
                        }
                    }
                    else
                    {
                        isGrounded = false;
                    }
                }
            }
        }

        public int Attack()
        {
            isAttacking = true;
            state = State.Attack;
            return attackPoints;
        }

        public void IsAttacked(int points)
        {
            if(points> shield)
            { 
                health -= (points - shield);
                if (health <= 0)
                {
                    MainClass.GameOver();
                }
            }
        }

        public void RespawnPointChange(List<Lantern> list)
        {
            if(list.Count != 0) foreach(Lantern lantern in list)
                {
                    if(Math.Abs((int)position.X - lantern.CollisionRect.X) < Math.Abs((int)position.X - respawnPoint.X))
                    {
                        respawnPoint = lantern.CollisionRect.Location;
                    }
                }
        }

        public void Respawn()
        {
            position = respawnPoint.ToVector2();
            lives--;
            if(lives <= 0)
            {
                MainClass.Game.GameOver();
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Direction of player
        /// </summary>
        public override Vector2 Direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;
                KeyboardState keyboard = Keyboard.GetState();
                if (keyboard.IsKeyDown(Keys.Left) || keyboard.IsKeyDown(Keys.A) )
                {
                    if (state != State.Jump && state != State.Walk)
                    {
                        state = State.Walk;
                        currentFrame.X = 0;
                        currentFrame.Y = sheetSize.Y / 3;

                    }
                    inputDirection.X -= 1;
                    dir = Dir.Left;
                    //currentEffect = MainClass.Load<SoundEffect>(@"SoundEffects\PlayerWalk");
                }
                else if (keyboard.IsKeyDown(Keys.Right) || keyboard.IsKeyDown(Keys.D))
                {
                    if (state != State.Jump && state != State.Walk)
                    {
                        state = State.Walk;
                            currentFrame.X = 0;
                            currentFrame.Y = sheetSize.Y/ 3;
                    }
                    inputDirection.X += 1;
                    dir = Dir.Right;
                    //currentEffect = MainClass.Load<SoundEffect>(@"SoundEffects\PlayerWalk");
                }
                if (gravityOn || !isGrounded)
                {
                    inputDirection.Y -= 2;
                }
                if (inputManager.KeyPressed(Keys.Up, Keys.W) && !gravityOn)
                {
                    state = State.Jump;
                    if (currentFrame.Y < sheetSize.Y - 2 || currentFrame.Y >= sheetSize.Y)
                    {
                        currentFrame.X = 0;
                        currentFrame.Y = (sheetSize.Y * 2) / 3;
                    }
                    inputDirection.Y -= 1;
                    speed.Y = 3.3f;
                    gravityOn = true;
                    if (dir == Dir.Left) dir = Dir.Left_Up;
                    else if (dir == Dir.Right) dir = Dir.Right_Up;
                    //currentEffect = MainClass.Load<SoundEffect>(@"SoundEffects\PlayerJump");
                }
                return inputDirection * speed;
            }
        }

        public List<Bullet> GetBullets
        {
            get
            {
                return bulletsList;
            }
        }

        /// <summary>
        /// Collision rectangle. Return initial, or custom value
        /// </summary>
        public override Rectangle CollisionRect
        {
            get { if (!collision.HasValue) return new Rectangle((int)position.X + collisionOffset, (int)position.Y + collisionOffset, frameSize.X - collisionOffset, frameSize.Y - collisionOffset);
                else return new Rectangle((int)position.X + collision.Value.X, (int)position.Y + collision.Value.Y, collision.Value.Width - collisionOffset, collision.Value.Height - collisionOffset);
            }
        }

        public Vector2 GetPosition
        {
            get
            {
                return position;
            }
        }

        /// <summary>
        /// Return view frame for background drawing
        /// </summary>
        public Rectangle GetBackViewFrame
        {
            get { return backFrame; }
        }

        /// <summary>
        /// Return view frame for base drawing 
        /// </summary>
        public Rectangle GetViewFrame
        {
            get { return viewFrame; }
        }

        public int Health
        {
            get
            {
                return health;
            }
            set
            {
                if(value > 0)
                {
                    health = value;
                }
            }
        }

        public int Mana
        {
            get
            {
                return mana;
            }
            set
            {
                if (value > 0)
                {
                    mana = value;
                }
            }
        }

        public int Shield
        {
            get
            {
                return shield;
            }
            set
            {
                if (value >= 0)
                {
                    shield = value;
                }
            }
        }

        public int BagCapacity
        {
            get
            {
                return bagCapacity;
            }

            set
            {
                if(value >= inventory.Count)
                {
                    bagCapacity = value;
                    inventory.Capacity = value;
                }
            }
        }
        #endregion
    }
}
