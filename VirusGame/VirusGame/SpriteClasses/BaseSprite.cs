using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VirusGame.SpriteClasses
{

    public abstract class BaseSprite
    {

        #region Declaration

        protected bool isVisible = true;
        protected Texture2D texture;
        public Vector2 position;
        public String type;

        public Vector2 origin = Vector2.Zero;
        public float rotation = 0f;


        #endregion

        #region get/set

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
            }
        }

        #endregion

    }
    
}
