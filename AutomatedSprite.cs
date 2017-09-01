using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace DreamCatcher
{
    public class AutomatedSprite : Sprite
    {
        new readonly Vector2 speed;
        #region Constructors
        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed) 
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed)
        {
            this.speed = speed;
        }

        public AutomatedSprite(Texture2D textureImage, Vector2 position, Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {
            this.speed = speed;
        }
        #endregion

        #region Properties
        public override Vector2 Direction
        {
            get
            {
                return speed;
            }
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += Direction;
            base.Update(gameTime, clientBounds);
        }
        #endregion
    }
}
