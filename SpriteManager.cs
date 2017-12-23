using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace DreamCatcher
{
    public partial class SpriteManager : DrawableGameComponent
    {
        #region Variables
        InputManager inputManager = new InputManager();

        SpriteBatch spriteBatch;
        Player player;
        SoundEffect walk;
        List<Sprite> enemyList = new List<Sprite>();
        List<Sprite> objectList = new List<Sprite>();
        #endregion

        public SpriteManager(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            walk = Game.Content.Load<SoundEffect>(@"SoundEffects\Shagy");
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            
        }
    }
}
