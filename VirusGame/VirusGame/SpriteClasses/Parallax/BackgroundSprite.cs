using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VirusGame.SpriteClasses.Parallax
{
    public abstract class BackgroundSprite : MovingSprite
    {

        #region Declaration
        //protected Texture2D texture;
        protected Vector2 center = new Vector2(0, 0);
       

        
        //for changing direction of the scrolling on the X or Y axis.
        protected Vector2 xDirection = new Vector2(-1, 0);
        protected Vector2 yDirection = new Vector2(0, -1);

        protected Vector2 changedX;
        protected Vector2 changedY;

        protected Vector2 maxPosition;
        protected Vector2 minPosition;

        protected Vector2 limit;
        //scroll speed variables for the background
        protected float speed;
        protected float speedConstant;

        #endregion
        //public float Depth
        //{
        //    set { depth = value; }
        //}

        public BackgroundSprite(Texture2D _texture, float _speed) : base(_texture, _speed)
        {
            
        }
    }
}
