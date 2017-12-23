using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace DreamCatcher
{
    public class EnemiesManager : DrawableGameComponent
    {
    #region Variables
        const int maxPower = 5; //Maximm sum of all enemies powers, where: basic enemy - 1, special enemy - 2, boss - 3-4 
        int power; //Sum of all enemies powers

        List<AutomatedSprite> enemyList = new List<AutomatedSprite>(); 

        int respawnDelay; //Delay between spawn of enemies in milliseconds
        Point respawnPoint; //One respawn point for every basic enemy. Special enemies and bosses won't respawn after death
        bool respawnEnabled = true; //Determines whether enemies could respawn? or not

        bool isMaterial; //Determines whether respawn point is represented by an material in-game object
        int health; //Health of material variation of resp-point

        SpriteBatch spriteBatch;
        DreamCatcherGame game;
        #endregion

        #region Methods
        public EnemiesManager(DreamCatcherGame game) : base(game)
        {
            this.game = game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void UnloadContent()
        {
            if (enemyList.Count != 0)
            {
                foreach(AutomatedSprite e in enemyList)
                {
                    e.Dispose();
                }
                enemyList = null;
            }
            base.UnloadContent();
        }

        #endregion

        #region Properties
        public List<AutomatedSprite> Enemy
        {
            get
            {
                return enemyList;
            }
            set
            {
                if(enemyList.Count == 0)
                {
                    enemyList = value;
                }
            }
        }
        #endregion

        #region Indexers
        public AutomatedSprite this[int index]
        {
            get
            {
                if(index >= 0 && index < enemyList.Count)
                {
                    return enemyList[index];
                }
                else
                {
                    throw new IndexOutOfRangeException($"Index {index} was out of list keys range");
                }
            }
        }
        #endregion
    }
}
