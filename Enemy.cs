using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DreamCatcher
{
    /// <summary>
    /// Base type for enemies
    /// </summary>
    public class Enemy : AutomatedSprite
    {
        #region Variables
        EnemyClass eClass;
        EnemiesID ID;

        State state = State.Stay;
        Dir dir = Dir.Left;

        int health;
        int attack;
        int defence;

        bool isAttacking = false;
        //bool gravityOn = false;

        bool isGrounded = true;
        bool jump = false;
        bool downWeGo = false;

        int jumpRadius = 100;
        int attackRadius = 50;
        int visionRadius = 180;

        const int delay = 2000;
        readonly int jumpDelay = 500;
        int currentDelay = 0;

        int counter = 0; //Костыль редкостный. Считает сколько раз Тень "отбивается" от края платформы. Если > 2 - прыгаем

        new Vector2 direction = new Vector2(-1, 0);
        Lantern currentTarget = null;
        Platform currentPlatform = null;
        Nullable<Rectangle> collisionRectangle;
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteSheet">Enemy sprite sheet</param>
        /// <param name="position">Enemy position</param>
        /// <param name="frameSize">Enemy animation frame size</param>
        /// <param name="collisionOffset">Collision offset of enemy</param>
        /// <param name="currentFrame">Current frame of enemy animation</param>
        /// <param name="sheetSize">Animation sheet size [in frames]</param>
        /// <param name="speed">Enemy speed</param>
        public Enemy(Texture2D spriteSheet, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed) :
            base(spriteSheet, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="class">Enemy class</param>
        /// <param name="spriteSheet">Enemy sprite sheet</param>
        /// <param name="position">Enemy position</param>
        /// <param name="frameSize">Enemy animation frame size</param>
        /// <param name="collisionOffset">Collision offset of enemy</param>
        /// <param name="currentFrame">Current frame of enemy animation</param>
        /// <param name="sheetSize">Animation sheet size [in frames]</param>
        /// <param name="speed">Enemy speed</param>
        public Enemy(EnemyClass @class, Texture2D spriteSheet, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed) :
            base(spriteSheet, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
            eClass = @class;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteSheet">Enemy sprite sheet</param>
        /// <param name="position">Enemy position</param>
        /// <param name="frameSize">Enemy animation frame size</param>
        /// <param name="collisionOffset">Collision offset of enemy</param>
        /// <param name="currentFrame">Current frame of enemy animation</param>
        /// <param name="sheetSize">Animation sheet size [in frames]</param>
        /// <param name="speed">Enemy speed</param>
        /// <param name="millisecondsPerFrame">Milliseconda per animation frame</param>
        public Enemy(Texture2D spriteSheet, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame) :
            base(spriteSheet, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="class">Enemy class</param>
        /// <param name="spriteSheet">Enemy sprite sheet</param>
        /// <param name="position">Enemy position</param>
        /// <param name="frameSize">Enemy animation frame size</param>
        /// <param name="collisionOffset">Collision offset of enemy</param>
        /// <param name="currentFrame">Current frame of enemy animation</param>
        /// <param name="sheetSize">Animation sheet size [in frames]</param>
        /// <param name="speed">Enemy speed</param>
        /// <param name="millisecondsPerFrame">Milliseconda per animation frame</param>
        public Enemy(EnemyClass @class, Texture2D spriteSheet, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame) :
            base(spriteSheet, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
            eClass = @class;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteSheet">Enemy sprite sheet</param>
        /// <param name="position">Enemy position</param>
        /// <param name="frameSize">Enemy animation frame size</param>
        /// <param name="collisionOffset">Collision offset of enemy</param>
        /// <param name="currentFrame">Current frame of enemy animation</param>
        /// <param name="sheetSize">Animation sheet size [in frames]</param>
        /// <param name="speed">Enemy speed</param>
        /// <param name="jumpRadius">Distance of enemy jump</param>
        /// <param name="attackRadius">Distance of enemy attack</param>
        /// <param name="visionRadius">Distance of enemy vision</param>
        public Enemy(Texture2D spriteSheet, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int jumpRadius, int attackRadius, int visionRadius = 200) :
            base(spriteSheet, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
            this.jumpRadius = jumpRadius;
            this.attackRadius = attackRadius;
            this.visionRadius = visionRadius;
            this.jumpDelay = (int)(jumpRadius / speed.Y);
        }

        /// <summary>
        /// 
        /// </summary> 
        /// <param name="class">Enemy class</param>
        /// <param name="spriteSheet">Enemy sprite sheet</param>
        /// <param name="position">Enemy position</param>
        /// <param name="frameSize">Enemy animation frame size</param>
        /// <param name="collisionOffset">Collision offset of enemy</param>
        /// <param name="currentFrame">Current frame of enemy animation</param>
        /// <param name="sheetSize">Animation sheet size [in frames]</param>
        /// <param name="speed">Enemy speed</param>
        /// <param name="millisecondsPerFrame">Milliseconda per animation frame</param>
        /// <param name="jumpRadius">Distance of enemy jump</param>
        /// <param name="attackRadius">Distance of enemy attack</param>
        /// <param name="visionRadius">Distance of enemy vision</param>
        public Enemy(EnemyClass @class, Texture2D spriteSheet, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, int jumpRadius, int attackRadius, int visionRadius = 200) :
    base(spriteSheet, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
            eClass = @class;
            this.jumpRadius = jumpRadius;
            this.attackRadius = attackRadius;
            this.visionRadius = visionRadius;
            this.jumpDelay = (int)(jumpRadius / speed.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="class">Enemy class</param>
        /// <param name="spriteSheet">Enemy sprite sheet</param>
        /// <param name="position">Enemy position</param>
        /// <param name="frameSize">Enemy animation frame size</param>
        /// <param name="collisionOffset">Collision offset of enemy</param>
        /// <param name="currentFrame">Current frame of enemy animation</param>
        /// <param name="sheetSize">Animation sheet size [in frames]</param>
        /// <param name="speed">Enemy speed</param>
        /// <param name="millisecondsPerFrame">Milliseconda per animation frame</param>
        /// <param name="jumpRadius">Distance of enemy jump</param>
        /// <param name="attackRadius">Distance of enemy attack</param>
        /// <param name="collisionRectangle">Custom collision rectangle</param>
        /// <param name="visionRadius">Distance of enemy vision</param>
        public Enemy(EnemyClass @class, Texture2D spriteSheet, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, int jumpRadius, int attackRadius, Rectangle collisionRectangle, int visionRadius = 200) :
base(spriteSheet, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
            eClass = @class;
            this.jumpRadius = jumpRadius;
            this.attackRadius = attackRadius;
            this.visionRadius = visionRadius;
            this.collisionRectangle = collisionRectangle;
            this.jumpDelay = (int)(jumpRadius / speed.Y);
        }

        public Enemy(EnemyClass @class, Texture2D spriteSheet, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame, int jumpRadius, int attackRadius, Rectangle collisionRectangle, int visionRadius = 200, int health = 1, int attack = 0, int defence = 0) :
base(spriteSheet, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
            eClass = @class;
            this.jumpRadius = jumpRadius;
            this.attackRadius = attackRadius;
            this.visionRadius = visionRadius;
            this.collisionRectangle = collisionRectangle;
            this.jumpDelay = (int)(jumpRadius / speed.Y);
            this.health = health;
            this.attack = attack;
            this.defence = defence;
        }

        public Enemy(EnemiesID ID, EnemyClass @class, Vector2 position)
            :base(null, Vector2.Zero, Point.Zero, 0, Point.Zero, Point.Zero, Vector2.Zero)
        {
            this.ID = ID;
            eClass = @class;
            //Получаем таблицу. Обращаемся по ключам. Еееей)
            object[] result = new object[0];
            switch (eClass)
            {
                case EnemyClass.Basic:
                    result = FileManager.ParseLuaFile($"Content\\Files\\Enemies\\b_{ID.ToString().ToLower()}.lua");
                    break;
            }
            NLua.LuaTable table = (NLua.LuaTable)result[0];
            textureImage = MainClass.Load<Texture2D>(table["texture"].ToString());
            this.position = position;

            NLua.LuaTable tabl = (NLua.LuaTable)table["frame_size"];
            frameSize = new Point(Convert.ToInt32(tabl["x"]), Convert.ToInt32(tabl["y"]));

            tabl = (NLua.LuaTable)table["current_frame"];
            currentFrame = new Point(Convert.ToInt32(tabl["x"]), Convert.ToInt32(tabl["y"]));

            tabl = (NLua.LuaTable)table["sheet_size"];
            sheetSize = new Point(Convert.ToInt32(tabl["x"]), Convert.ToInt32(tabl["y"]));

            collisionOffset = Convert.ToInt32(table["collision_offset"]);

            tabl = (NLua.LuaTable)table["speed"];
            speed = new Vector2(Convert.ToSingle(tabl["x"]), Convert.ToSingle(tabl["y"]));
            millisecondsPerFrame = Convert.ToInt32(table["milliseconds_per_frame"]);
            visionRadius = Convert.ToInt32(table["vision_rad"]);
            attackRadius = Convert.ToInt32(table["attack_rad"]);
            jumpRadius = Convert.ToInt32(table["jump_rad"]);

            tabl = (NLua.LuaTable)table["collision_rectangle"];
            collisionRectangle = new Rectangle(Convert.ToInt32(tabl["x"]), Convert.ToInt32(tabl["y"]), Convert.ToInt32(tabl["width"]), Convert.ToInt32(tabl["height"]));

            health = Convert.ToInt32(table["hit_points"]);
            attack = Convert.ToInt32(table["attack_points"]);
            defence = Convert.ToInt32(table["defence_points"]);
            jumpDelay = (int)(jumpRadius / speed.Y);
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            switch (eClass)
            {
                case EnemyClass.Basic:
                    {
                        switch (state)
                        {
                            case State.Attack:
                                break;
                            case State.Jump:
                                break;
                            case State.Stay:
                                if (dir == Dir.Left) { }
                                else if (dir == Dir.Right) { }
                                break;
                            case State.Walk:
                                if (dir == Dir.Left) { }
                                else if (dir == Dir.Right) { }
                                break;
                        }
                    }
                    break;
                case EnemyClass.Special:
                    {
                        switch (state)
                        {
                            case State.Attack:
                                break;
                            case State.Jump:
                                break;
                            case State.Stay:
                                if (dir == Dir.Left) { }
                                else if (dir == Dir.Right) { }
                                break;
                            case State.Walk:
                                if (dir == Dir.Left) { }
                                else if (dir == Dir.Right) { }
                                break;
                        }
                    }
                    break;
                case EnemyClass.Boss:
                    {
                        switch (state)
                        {
                            case State.Attack:
                                break;
                            case State.Jump:
                                break;
                            case State.Stay:
                                if (dir == Dir.Left) { }
                                else if (dir == Dir.Right) { }
                                break;
                            case State.Walk:
                                if (dir == Dir.Left) { }
                                else if (dir == Dir.Right) { }
                                break;
                        }
                    }
                    break;
            }
        }

        public void Update(GameTime gameTime, Rectangle clientBounds, List<Platform> platformList, List<Lantern> lanternList)
        {
            switch (eClass)
            {
                case EnemyClass.Basic:
                    {
                        ChooseTarget(lanternList); //Выбор цели
                        if (currentTarget != null && currentTarget.CollisionRect.Intersects(CollisionRect)) //Если достиг цели - действуй
                        {
                            currentTarget.Attack();
                            currentTarget = null;
                            jump = true;
                            downWeGo = false;
                            counter = 0;
                        }
                        //Атака цели

                        switch (state)
                        {

                            #region Jump
                            case State.Jump:
                                if (currentPlatform != null) //Если запрыгнул на платформу - выжди положенный срок
                                {
                                    currentDelay += gameTime.ElapsedGameTime.Milliseconds;
                                    if (currentDelay > jumpDelay)
                                    {
                                        state = State.Stay;
                                        currentDelay = 0;
                                    }
                                }
                                if (!isGrounded) //Свободное (или не очень) падение
                                {
                                    position.Y += speed.Y;
                                }
                                break;
                            #endregion

                            #region Stay
                            case State.Stay:
                                {
                                    currentDelay += gameTime.ElapsedGameTime.Milliseconds;

                                    #region Animation
                                    timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

                                    if (timeSinceLastFrame > millisecondsPerFrame)
                                    {
                                        timeSinceLastFrame -= millisecondsPerFrame;
                                        ++currentFrame.X;

                                        if (currentFrame.X >= sheetSize.X)
                                        {
                                            currentFrame.X = 0;
                                            currentFrame.Y++;
                                            if (currentFrame.Y >= 2)
                                            {
                                                currentFrame.Y = 0;
                                            }
                                        }
                                    }
                                    #endregion

                                    //Костыли
                                    if (dir == Dir.Left) direction.X = -1; 
                                    else if (dir == Dir.Right) direction.X = 1;

                                    #region Not_Null_Target
                                    if (currentTarget != null)
                                    {
                                        #region Null_platform
                                        if (currentPlatform == null)
                                        {
                                            direction.X = currentTarget.CollisionRect.X - CollisionRect.X >= 0 && currentTarget.CollisionRect.X - CollisionRect.X <= 10 ? 0 : currentTarget.CollisionRect.X < CollisionRect.X ? -1 : 1;
                                            if (direction.X != 0)
                                            {
                                                state = State.Walk;
                                                currentFrame.Y = 2;
                                                currentFrame.X = 0;
                                                if (direction.X == -1) dir = Dir.Left;
                                                else dir = Dir.Right;
                                            }
                                            else
                                            {
                                                state = State.Stay;
                                                direction.Y = currentTarget.CollisionRect.Intersects(CollisionRect) ? 0 : currentTarget.CollisionRect.Y > CollisionRect.Y ? -1 : 1;
                                                if (direction.Y == 0) state = State.Stay;
                                                else if (direction.Y != 0)
                                                {
                                                    List<Platform> p = (from pl in platformList
                                                                        where pl.CollisionRect.Intersects(JumpRect)
                                                                        select pl).ToList();
                                                    if (p.Count != 0)
                                                    {
                                                        if (direction.Y == -1)
                                                        {
                                                            if (direction.X == -1)
                                                            {

                                                            }
                                                            else if (direction.X == 0)
                                                            {
                                                                foreach (Platform pl in p)
                                                                {
                                                                    if (pl.CollisionRect.Y < CollisionRect.Y
                                                                        && pl.CollisionRect.X >= currentTarget.CollisionRect.X
                                                                        && pl.CollisionRect.X <= currentTarget.CollisionRect.X)
                                                                    {
                                                                        Jump(pl);
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {

                                                            }
                                                        }
                                                        else if (direction.Y == 1)
                                                        {
                                                            if (direction.X == -1)
                                                            {

                                                            }
                                                            else if (direction.X == 0)
                                                            {
                                                                foreach (Platform pl in p)
                                                                {
                                                                    if (pl.CollisionRect.Y > CollisionRect.Y
                                                                        && pl.CollisionRect.X >= currentTarget.CollisionRect.X
                                                                        && pl.CollisionRect.X <= currentTarget.CollisionRect.X)
                                                                    {
                                                                        Jump(pl);
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        #region Not_null_platform
                                        else
                                        {
                                            direction.X = currentTarget.CollisionRect.X - CollisionRect.X >= 0 && currentTarget.CollisionRect.X - CollisionRect.X <= 10 ? 0 : currentTarget.CollisionRect.X < CollisionRect.X ? -1 : 1;
                                            if (currentTarget.CollisionRect.Y >= CollisionRect.Y && currentTarget.CollisionRect.Bottom <= CollisionRect.Bottom) direction.Y = 0;
                                            else if (currentTarget.CollisionRect.Y < CollisionRect.Y) direction.Y = -1;
                                            else if (currentTarget.CollisionRect.Bottom > CollisionRect.Bottom) direction.Y = 1;
                                            dir = direction.X == -1 ? Dir.Left : direction.X == 1 ? Dir.Right : dir;

                                            if (direction.Y != 0)
                                            {
                                                List<Platform> p2 = (from pl in platformList
                                                                     where pl.CollisionRect.Intersects(JumpRect) &&
                                                                     pl != currentPlatform
                                                                     select pl).ToList();
                                                foreach (Platform p in p2)
                                                {
                                                    if (direction.Y == -1)
                                                    {
                                                        if (direction.X == -1)
                                                        {
                                                            if (p.CollisionRect.X < CollisionRect.X
                                                            && p.CollisionRect.Y < CollisionRect.Y)
                                                            {
                                                                Jump(p);
                                                                break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (p.CollisionRect.X > CollisionRect.X
                                                            && p.CollisionRect.Y < CollisionRect.Y)
                                                            {
                                                                Jump(p);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    else if (direction.Y == 1)
                                                    {
                                                        if (direction.X == -1)
                                                        {
                                                            if (p.CollisionRect.X < CollisionRect.X
                                                            && p.CollisionRect.Y < CollisionRect.Y)
                                                            {
                                                                Jump(p);
                                                                break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (p.CollisionRect.X > CollisionRect.X
                                                            && p.CollisionRect.Y < CollisionRect.Y)
                                                            {
                                                                Jump(p);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                List<Platform> p2 = (from pl in platformList
                                                                     where pl.CollisionRect.Intersects(currentPlatform.CollisionRect)
                                                                     && pl != currentPlatform
                                                                     select pl).ToList();
                                                if (direction.X != 0)
                                                {
                                                    if (p2.Count != 0)
                                                    {
                                                        foreach (Platform p in p2)
                                                        {
                                                            if (direction.X == -1)
                                                            {
                                                                if(p.CollisionRect.X < currentPlatform.CollisionRect.X)
                                                                {
                                                                    state = State.Walk;
                                                                    break;
                                                                }
                                                            }
                                                            else if (direction.X == 1)
                                                            {
                                                                if(p.CollisionRect.X > currentPlatform.CollisionRect.X)
                                                                {
                                                                    state = State.Walk;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        List<Platform> p3 = (from pl in platformList
                                                                             where pl.CollisionRect.Intersects(JumpRect)
                                                                             && pl != currentPlatform
                                                                             && pl.CollisionRect.Y == currentPlatform.CollisionRect.Y
                                                                             select pl).ToList();
                                                        if(p3.Count != 0)
                                                        {
                                                            foreach(Platform p in p3)
                                                            {
                                                                if (direction.X == -1)
                                                                {
                                                                    if(p.CollisionRect.X < currentPlatform.CollisionRect.X)
                                                                    {
                                                                        Jump(p);
                                                                        break;
                                                                    }
                                                                }
                                                                else if(direction.X == 1)
                                                                {
                                                                    if (p.CollisionRect.X > currentPlatform.CollisionRect.X)
                                                                    {
                                                                        Jump(p);
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            #region Вроде_Херня
                                            //if (downWeGo)
                                            //{
                                            //    if (p2.Count != 0)
                                            //    {
                                            //        Platform control = currentPlatform;
                                            //        foreach (Platform pl in p2)
                                            //        {
                                            //            if (dir == Dir.Left && pl.collisionRect.X < currentPlatform.collisionRect.X
                                            //                || dir == Dir.Right && pl.collisionRect.X > currentPlatform.collisionRect.X)
                                            //            {
                                            //                Jump(pl);
                                            //            }
                                            //        }
                                            //        if (currentPlatform == control) Jump(p2[0]);
                                            //    }
                                            //}
                                            #endregion
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region Null_Target
                                    else
                                    {
                                        speed.X = 1.5f;
                                        #region Null_current_platform
                                        if (currentPlatform == null) //Если просто идём, то всё окей
                                        {
                                            if (currentDelay > delay)
                                            {
                                                state = State.Walk;
                                                currentFrame.Y = 2;
                                                currentFrame.X = 0; ;
                                                currentDelay -= delay;
                                            }
                                        }
                                        #endregion

                                        #region Not_null_current_platform
                                        else //Если стоим на платформе
                                        {
                                            if (currentDelay > delay)
                                            {
                                                //Выбираем платформы, которые "состыкованы"  текущей. Если такие есть
                                                List<Platform> p2 = (from pl in platformList
                                                                     where pl.CollisionRect.Intersects(currentPlatform.CollisionRect)
                                                                     && pl != currentPlatform
                                                                     select pl).ToList();
                                                if (p2.Count != 0)
                                                {
                                                    foreach (Platform pl in p2)
                                                    {
                                                        //Для каждой платформы, проверяем, можем ли мы по ней пройтись
                                                        if (direction.X == -1)
                                                        {
                                                            if (pl.GetCenter.X < currentPlatform.GetCenter.X)
                                                            {
                                                                state = State.Walk;
                                                                currentFrame.Y = 2;
                                                                currentFrame.X = 0;
                                                                currentDelay = 0;
                                                                break;
                                                            }
                                                        }
                                                        else if (direction.X == 1)
                                                        {
                                                            if (pl.GetCenter.X > currentPlatform.GetCenter.X)
                                                            {
                                                                state = State.Walk;
                                                                currentFrame.Y = 2;
                                                                currentFrame.X = 0;
                                                                currentDelay = 0;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (state != State.Walk) //Если мы таки никуда не двинулись
                                                    {
                                                        //Меняем направление, и добавляем к счётчику поворотов 1
                                                        dir = dir == Dir.Left ? Dir.Right : Dir.Left;
                                                        direction.X = -direction.X;
                                                        counter++;

                                                        if (counter >= 1) //Если поернули один или больше раз - думаем
                                                        {
                                                            if (!downWeGo) //Если не достигали верху ине падаем вниз
                                                            {
                                                                if (!jump) //И если не прыгаем, то стоит об этом подумать
                                                                {
                                                                    jump = true;
                                                                    counter = 0;
                                                                }
                                                                else //Иначе есть смысл вообще спуститься вниз
                                                                {
                                                                    jump = false;
                                                                    downWeGo = true;
                                                                    counter = 0;
                                                                }
                                                            }
                                                            else //Если мы вс таки ползём вниз
                                                            {
                                                                if (jump) //И будем опять спрыгивать, то обнулем счётчик
                                                                {
                                                                    counter = 0;
                                                                }
                                                                else //Иначе всё равно его обнуляем, но всё таки прыгаем
                                                                {
                                                                    jump = true;
                                                                    counter = 0;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else //Если нам таки некуда больше топать по платформам
                                                {
                                                    //Проверяем "соседей"
                                                    List<Platform> p3 = (from pl in platformList
                                                                         where pl.CollisionRect.Intersects(JumpRect)
                                                                         && pl != currentPlatform
                                                                         && pl.CollisionRect.Y == currentPlatform.CollisionRect.Y
                                                                         select pl).ToList();
                                                    if (p3.Count != 0) //Если соседи есть, прыгаем к ним
                                                    {
                                                        MainClass.WriteLog("Inline jump: " + p3.Count.ToString());
                                                        Jump(dir == Dir.Left ? p3[p3.Count -1] : p3[0]);
                                                        break;
                                                    }
                                                    else //Иначе опять думаем
                                                    {
                                                        if (!downWeGo) //Если не падаем...
                                                        {
                                                            if (!jump) //И не прыгаем...
                                                            {
                                                                //Проверяем, можно-ли куда повыше забраться
                                                                List<Platform> p4 = (from pl in platformList
                                                                                     where pl.CollisionRect.Intersects(JumpRect) &&
                                                                                     pl != currentPlatform &&
                                                                                     pl.CollisionRect.Y < currentPlatform.CollisionRect.Y
                                                                                     select pl).ToList();
                                                                if (p4.Count == 0) //Если нет, то хрен с вами... не очень-то и хотелось...
                                                                {
                                                                    downWeGo = true;
                                                                }
                                                                else //Если да, то всё отлично
                                                                {
                                                                    jump = true;
                                                                }
                                                            }
                                                            else //Если таки будем прыгать
                                                            {
                                                                List<Platform> p4 = (from pl in platformList
                                                                                     where pl.CollisionRect.Intersects(JumpRect) &&
                                                                                     pl != currentPlatform &&
                                                                                     pl.CollisionRect.Y < currentPlatform.CollisionRect.Y
                                                                                     select pl).ToList();
                                                                if(p4.Count != 0) //И если есть куда, то скатертью дорога
                                                                {
                                                                    MainClass.WriteLog("Up-line jump: " + p4.Count.ToString());
                                                                    Jump(dir == Dir.Left ? p4[0] : p4[p4.Count - 1]);
                                                                    break;
                                                                }
                                                                else //Иначе грустно ползём вниз
                                                                {
                                                                    downWeGo = true;
                                                                }
                                                            }
                                                        }
                                                        else //Если же всё таки ползём вниз...
                                                        {
                                                            if (jump) //И прыгаем...
                                                            {
                                                                //Проверяем "соседей" снизу
                                                                List<Platform> p4 = (from pl in platformList
                                                                                     where pl.CollisionRect.Intersects(JumpRect) &&
                                                                                     pl != currentPlatform &&
                                                                                     pl.CollisionRect.Y > currentPlatform.CollisionRect.Y
                                                                                     select pl).ToList();
                                                                if(p4.Count != 0) //Если есть - пошли в гости
                                                                {
                                                                    MainClass.WriteLog("Down-line jump: " + p3.Count.ToString());
                                                                    Jump(dir == Dir.Left ? p4[0] : p4[p4.Count - 1]);
                                                                    break;
                                                                }
                                                                else //Иначе грустно спрыгиваем на землю вообще
                                                                {
                                                                    state = State.Jump;
                                                                    jump = false;
                                                                    position.Y += 145;
                                                                    isGrounded = false;
                                                                    currentPlatform = null;
                                                                }
                                                            }
                                                            else //Если не прыгаем, то будем
                                                            {
                                                                jump = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }
                                }
                                break;
                            #endregion

                            #region Walk
                            case State.Walk:
                                {
                                    position.X += direction.X * speed.X;

                                    timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                                    if (timeSinceLastFrame > millisecondsPerFrame)
                                    {
                                        timeSinceLastFrame -= millisecondsPerFrame;
                                        ++currentFrame.X;

                                        if (currentFrame.X >= sheetSize.X)
                                        {
                                            currentFrame.X = 0;
                                            currentFrame.Y++;
                                            if (currentFrame.Y >= sheetSize.Y)
                                            {
                                                currentFrame.Y = 2;
                                            }
                                        }
                                    }

                                    #region Rotation
                                    if (currentPlatform != null) //Если стоим на платформе
                                    {
                                        List<Platform> p = (from pl in platformList
                                                            where pl.CollisionRect.Intersects(currentPlatform.CollisionRect)
                                                            && pl != currentPlatform
                                                            select pl).ToList();
                                        if (p.Count != 0) //И есть куда топать
                                        {
                                            bool canGo = false;
                                            for (int i = 0; i<p.Count; i++)
                                            {
                                                Platform pl = p[i];
                                                //То проверяем, прокатит, или нет
                                                if ((direction.X == 1 &&
                                                    pl.GetCenter.X > currentPlatform.GetCenter.X) ||
                                                    (direction.X == -1 &&
                                                    pl.GetCenter.X < currentPlatform.GetCenter.X))
                                                {
                                                    canGo = true;
                                                }
                                            }
                                            if (!canGo) //Если не прокатило - топаем обратно
                                            {
                                                counter++;
                                                direction.X = -direction.X;
                                                dir = dir == Dir.Left ? Dir.Right : Dir.Left;
                                                state = State.Stay;
                                                currentFrame = Point.Zero;
                                            }
                                            else //Если прокатило, проверяем, не перешли ли мы на следующую платформу
                                            {
                                                for (int i = 0; i < p.Count; i++)
                                                {
                                                    Platform pl = p[i];
                                                    if (Math.Abs(pl.CollisionRect.X - CollisionRect.X) < 10)
                                                    {
                                                        currentPlatform = pl;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion

                                    #region Not_Null_Target
                                    if (currentTarget != null)
                                    {
                                        #region Null_Current_Platform
                                        if (currentPlatform == null)
                                        {
                                            if (currentTarget.CollisionRect.Y > position.Y)
                                            {
                                                direction.Y = 0;
                                                jump = false;
                                                downWeGo = false;
                                                if (currentTarget.CollisionRect.Intersects(CollisionRect))
                                                {
                                                    state = State.Attack;
                                                    direction.X = 0;
                                                }
                                                else
                                                {
                                                    direction.X = CollisionRect.X > currentTarget.CollisionRect.X ? -1 : 1;
                                                    dir = direction.X == -1 ? Dir.Left : direction.X == 1 ? Dir.Right : dir;          
                                                }
                                            }
                                            else
                                            {
                                                if (jump)
                                                {
                                                    List<Platform> p = (from pl in platformList
                                                                        where pl.CollisionRect.Intersects(JumpRect)
                                                                        select pl).ToList();
                                                    if(p.Count!= 0)
                                                    {
                                                        foreach(Platform pl in p)
                                                        {
                                                            if (direction.X == -1 && pl.CollisionRect.X <= position.X)
                                                            {
                                                                Jump(pl); break;
                                                            }
                                                            else if(direction.X == 1 && pl.CollisionRect.X >= position.X)
                                                            {
                                                                Jump(pl); break;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    jump = true;
                                                }
                                                direction.X = CollisionRect.X > currentTarget.CollisionRect.X ? -1 : 1;
                                                dir = direction.X == -1 ? Dir.Left : direction.X == 1 ? Dir.Right : dir;
                                            }
                                        }
                                        #endregion

                                        #region Not_Null_Current_Platform
                                        else
                                        {
                                            if (currentTarget.CollisionRect.Y >= CollisionRect.Y && currentTarget.CollisionRect.Y <= CollisionRect.Bottom)
                                            {
                                                direction.Y = 0;
                                                if (currentTarget.CollisionRect.X >= CollisionRect.X && currentTarget.CollisionRect.X <= CollisionRect.Right) direction.X = 0;
                                                else if (currentTarget.CollisionRect.X < CollisionRect.X)
                                                {
                                                    direction.X = -1;
                                                }
                                                else if (currentTarget.CollisionRect.X > CollisionRect.X)
                                                {
                                                    direction.X = 1;
                                                }
                                            }
                                            else if (currentTarget.CollisionRect.Y < CollisionRect.Y)
                                            {
                                                direction.Y = 1;
                                            }
                                            else if (currentTarget.CollisionRect.Y > CollisionRect.Y)
                                            {
                                                direction.Y = -1;
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion

                                    #region Null_Target
                                    else
                                    {
                                        speed.X = 1.5f;
                                        #region Jump
                                        if (jump)
                                        {
                                            #region Null_platfrm
                                            if (currentPlatform == null)
                                            {
                                                List<Platform> p2 = (from pl in platformList
                                                                     where pl.CollisionRect.Intersects(JumpRect)
                                                                     select pl).ToList();
                                                if (p2.Count != 0)
                                                {
                                                    for (int i = 0; i < p2.Count; i++)
                                                    {
                                                        Platform pl = p2[i];
                                                        if (dir == Dir.Left)
                                                        {
                                                            if (pl.CollisionRect.X - position.X <= 0)
                                                            {
                                                                MainClass.WriteLog("Walk");
                                                                Jump(pl);
                                                                break;
                                                            }
                                                        }
                                                        else if (dir == Dir.Right)
                                                        {
                                                            if (pl.CollisionRect.X - position.X >= 0)
                                                            {
                                                                MainClass.WriteLog("Walk");
                                                                Jump(pl);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    if (jump)
                                                    {
                                                        MainClass.WriteLog("Walk");
                                                        Jump(dir == Dir.Left ? p2[0] : p2[p2.Count - 1]);
                                                        break;
                                                    }
                                                }
                                            }
                                            #endregion

                                            #region Not_null_platform
                                            else
                                            {
                                                #region Down
                                                if (downWeGo)
                                                {
                                                    List<Platform> p2 = (from pl in platformList
                                                                         where pl.CollisionRect.Intersects(JumpRect) &&
                                                                         pl != currentPlatform &&
                                                                         pl.CollisionRect.Y > currentPlatform.CollisionRect.Y
                                                                         select pl).ToList();
                                                    if (p2.Count != 0)
                                                    {
                                                        MainClass.WriteLog("Down-line jump: " + p2.Count.ToString());
                                                        bool @do = false;
                                                        foreach (Platform pl in p2)
                                                        {
                                                            if (dir == Dir.Left && pl.CollisionRect.X < currentPlatform.CollisionRect.X
                                                                || dir == Dir.Right && pl.CollisionRect.X > currentPlatform.CollisionRect.X)
                                                            {
                                                                Jump(pl);
                                                                @do = true;
                                                                break;
                                                            }
                                                        }
                                                        if (!@do)
                                                        {
                                                            Jump(dir == Dir.Left ? p2[0] : p2[p2.Count - 1]);
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (counter >= 2)
                                                        {
                                                            state = State.Jump;
                                                            jump = false;
                                                            position.Y += 150;
                                                            isGrounded = false;
                                                            currentPlatform = null;
                                                            counter = 0;
                                                        }
                                                    }
                                                }
                                                #endregion

                                                #region Not_Down
                                                else
                                                {
                                                    bool @do = false;
                                                    List<Platform> p2 = (from pl in platformList
                                                                         where pl.CollisionRect.Intersects(JumpRect) &&
                                                                         pl.CollisionRect.Y == currentPlatform.CollisionRect.Y &&
                                                                         pl != currentPlatform
                                                                         select pl).ToList();
                                                    if (p2.Count != 0)
                                                    {
                                                        MainClass.WriteLog("Inline jump: " + p2.Count.ToString());
                                                        if (dir == Dir.Left && p2[p2.Count - 1].CollisionRect.X > currentPlatform.CollisionRect.X)
                                                        {
                                                            Jump(p2[p2.Count - 1]);
                                                            @do = true;
                                                            break;
                                                        }
                                                        else if (dir == Dir.Right && p2[0].CollisionRect.X < currentPlatform.CollisionRect.X)
                                                        {
                                                            Jump(p2[0]);
                                                            @do = true;
                                                            break;
                                                        }
                                                    }
                                                    if (!@do)
                                                    {
                                                        List<Platform> p3 = (from pl in platformList
                                                                             where pl.CollisionRect.Intersects(JumpRect)
                                                                             && pl != currentPlatform &&
                                                                             pl.CollisionRect.Y < currentPlatform.CollisionRect.Y
                                                                             select pl).ToList();
                                                        if (p3.Count != 0)
                                                        {
                                                            MainClass.WriteLog("Up-line jump: " + p3.Count.ToString());
                                                            foreach (Platform pl in p3)
                                                            {
                                                                if (dir == Dir.Left)
                                                                {
                                                                    if (pl.CollisionRect.X - position.X <= 0)
                                                                    {
                                                                        Jump(pl);
                                                                        @do = true;
                                                                        break;
                                                                    }
                                                                }
                                                                else if (dir == Dir.Right)
                                                                {
                                                                    if (pl.CollisionRect.X - position.X >= 0)
                                                                    {
                                                                        Jump(pl);
                                                                        @do = true;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                            if (!@do)
                                                            {
                                                                foreach (Platform pl in p3)
                                                                {
                                                                    if (pl.GetCenter.Y < currentPlatform.GetCenter.Y)
                                                                    {
                                                                        if (pl.GetCenter.X == currentPlatform.GetCenter.X)
                                                                        {
                                                                            Jump(pl);
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            jump = false;
                                                            downWeGo = true;
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                            #endregion
                                        }
                                        #endregion

                                        #region Not_jump
                                        else if (!jump)
                                        {
                                            #region Null_current_platform
                                            if (currentPlatform == null)
                                            {
                                                if (position.X + frameSize.X > clientBounds.Width / 0.75)
                                                {
                                                    position.X = clientBounds.Width / 0.75f - frameSize.X;
                                                    dir = dir == Dir.Left ? Dir.Right : dir == Dir.Right ? Dir.Left : dir;
                                                    state = State.Stay;
                                                    currentFrame = Point.Zero;
                                                    direction.X = -direction.X;
                                                    jump = true;
                                                    downWeGo = false;
                                                    counter = 0;
                                                }
                                                else if (position.X < 0)
                                                {
                                                    position.X = 0;
                                                    dir = dir == Dir.Left ? Dir.Right : dir == Dir.Right ? Dir.Left : dir;
                                                    state = State.Stay;
                                                    currentFrame = Point.Zero;
                                                    direction.X = -direction.X;
                                                    jump = true;
                                                    downWeGo = false;
                                                    counter = 0;
                                                }
                                            }
                                            #endregion

                                            #region Not_null_current_platform
                                            else
                                            {
                                                if (downWeGo)
                                                {
                                                    if (counter >= 1)
                                                    {
                                                        jump = true;
                                                        counter = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    if (counter >= 1)
                                                    {
                                                        jump = true;
                                                        downWeGo = false;
                                                        counter = 0;
                                                    }
                                                }
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                break;
                                #endregion
                        }
                    }
                    break;
                case EnemyClass.Special:
                    {
                        switch (state)
                        {
                            case State.Attack:
                                break;
                            case State.Jump:
                                break;
                            case State.Stay:
                                break;
                            case State.Walk:
                                break;
                        }
                    }
                    break;
                case EnemyClass.Boss:
                    {
                        switch (state)
                        {
                            case State.Attack:
                                break;
                            case State.Jump:
                                break;
                            case State.Stay:
                                if (dir == Dir.Left) { }
                                else if (dir == Dir.Right) { }
                                break;
                            case State.Walk:
                                if (dir == Dir.Left) { }
                                else if (dir == Dir.Right) { }
                                break;
                        }
                    }
                    break;
            }
        }

        public override void Draw(GameTime time, Vector2 drawFramePosition, SpriteBatch spriteBatch)
        {
            try
            {
                if (dir == Dir.Left_Up || dir == Dir.Left || dir == Dir.Left_Down)
                {
                    spriteBatch.Draw(textureImage, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y), new Rectangle((currentFrame.X * frameSize.X),
                    (currentFrame.Y * frameSize.Y), frameSize.X,
                    frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.Draw(textureImage, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y), new Rectangle((currentFrame.X * frameSize.X),
                    (currentFrame.Y * frameSize.Y), frameSize.X,
                    frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                }
            }
            catch
            {
                spriteBatch.Begin();
                if (dir == Dir.Left_Up || dir == Dir.Left || dir == Dir.Left_Down) spriteBatch.Draw(textureImage, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y), new Rectangle((currentFrame.X * frameSize.X),
        (currentFrame.Y * frameSize.Y), frameSize.X,
        frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                else spriteBatch.Draw(textureImage, new Vector2(position.X - drawFramePosition.X, position.Y - drawFramePosition.Y), new Rectangle((currentFrame.X * frameSize.X),
                    (currentFrame.Y * frameSize.Y), frameSize.X,
                    frameSize.Y), Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                spriteBatch.End();
            }
        }

        public int Attack()
        {
            state = State.Attack;
            return attack;
        }

        public bool IsAttacked(int attackPoints)
        {
            if (attackPoints > defence)
            {
                health--;
                if (health <= 0)
                {
                    return true;
                }
                return false;
            }
            else
            {
                defence -= attackPoints;
                return false;
            }
        }

        /// <summary>
        /// Method to transport enemy to distinct platform
        /// </summary>
        /// <param name="p">Distinct platform</param>
        public void Jump(Platform p)
        {
            currentDelay = 0;
            currentPlatform = p;
            jump = false;
            currentFrame.Y = 2;
            currentFrame.X = 0;
            state = State.Jump;
            position.Y = p.CollisionRect.Y - CollisionRect.Height;
            position.X = p.GetCenter.X - frameSize.X / 2;
            isGrounded = false;
        }

        /// <summary>
        /// Checking collision 
        /// </summary>
        /// <param name="r">Collision rectangle of object</param>
        /// <param name="p">Nullable platform. Determines whether enemy stand on platform or on ground</param>
        /// <returns>True, if object intersects with ground or other platform</returns>
        public bool CollisionCheck(Rectangle r, Platform p = null)
        {
            if (CollisionRect.Intersects(r))
            {
                //Если игрок над платформой, то он останавливается на ней
                if (CollisionRect.Bottom - r.Top > 0 && CollisionRect.Bottom - r.Top < 10)
                {
                    //gravityOn = false;
                    isGrounded = true;
                    if (p == null)
                    {
                        downWeGo = false;
                    }
                    dir = dir == Dir.Left_Down ? Dir.Left : dir == Dir.Right_Down ? Dir.Right : dir;
                    currentPlatform = p;
                    if (state == State.Jump) state = State.Stay;
                    return true;
                }
            }
            isGrounded = false;
            return false;
        }

        public override string ToString()
        {
            string s = eClass.ToString() + ";" + position.X + "," + position.Y + ";" + dir.ToString() + ";" + health + ";" + attack;
            return s;
        }

        /// <summary>
        /// Method to choose target of enemy
        /// </summary>
        /// <param name="list">List of lanterns to choose target from</param>
        private void ChooseTarget(List<Lantern> list)
        {
            List<Lantern> l = (from lan in list
                               where lan != null 
                               && lan.CollisionRect.Intersects(VisionRect)
                               select lan).ToList();
            double distance = visionRadius;
            foreach (Lantern ln in l)
            {
                double d = Math.Sqrt(Math.Pow(ln.CollisionRect.X - position.X, 2) + Math.Pow(ln.CollisionRect.Y - position.Y, 2));
                if (d < distance)
                {
                    distance = d;
                    speed.X = 2;
                    currentTarget = ln;
                }
            }
        }
        #endregion

        #region Collision_Rectangles
        /// <summary>
        /// Collision rectangle of enemy
        /// </summary>
        public override Rectangle CollisionRect
        {
            get
            {
                if (collisionRectangle.HasValue) return new Rectangle((int)position.X + collisionRectangle.Value.X, (int)position.Y + collisionRectangle.Value.Y, collisionRectangle.Value.Width - collisionOffset, collisionRectangle.Value.Height - collisionOffset);
                else
                {
                    return new Rectangle(
                        (int)position.X + collisionOffset,
                        (int)position.Y + collisionOffset,
                        frameSize.X - (2 * collisionOffset),
                        frameSize.Y - (2 * collisionOffset));

                }
            }
        }

        /// <summary>
        /// Jump rectangle
        /// </summary>
        private Rectangle JumpRect
        {
            get
            {
                return new Rectangle((int)position.X - jumpRadius / 2, (int)position.Y - jumpRadius, CollisionRect.Width + jumpRadius, frameSize.Y + (2 * jumpRadius) + 30);
            }
        }

        /// <summary>
        /// Vision rectangle
        /// </summary>
        private Rectangle VisionRect
        {
            get
            {
                return new Rectangle((int)position.X - visionRadius, (int)position.Y - visionRadius, CollisionRect.Width + (2 * visionRadius), frameSize.Y + (2 * visionRadius));
            }
        }

        /// <summary>
        /// Attack rectangle
        /// </summary>
        private Rectangle AttackRect
        {
            get
            {
                return new Rectangle((int)position.X - attackRadius, (int)position.Y, CollisionRect.Width + (2 * attackRadius), CollisionRect.Y);
            }
        }
        #endregion

        /// <summary>
        /// Returns center of enemy
        /// </summary>
        private Point GetCenter
        {
            get
            {
                return new Point(CollisionRect.X + CollisionRect.Width / 2, CollisionRect.Y + CollisionRect.Height / 2);
            }
        }
    }
}
