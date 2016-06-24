using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VirusGame.SpriteClasses
{
    /// <summary>
    /// The Animation class holds all variables for a draw method.
    /// Animation.Copy() will return a copy of the animation.
    /// </summary>
    public class Animation
    {
        #region Declarations

        public Rectangle[] Rectangles;          
        public Color Color = Color.White;
        public Vector2 Origin;
        public float Rotation = 0f;
        private float scale = 1f * Globals.GlobalScale;
        public SpriteEffects SpriteEffect;
        public bool IsLooping = true;
        public int Frames;
        public float Depth = 0f;

        #endregion

        public float Scale
        {
            set { scale = value * Globals.GlobalScale; }
            get {return scale;}

        }

        /// <summary>
        /// Copy returns a copy of the animation.
        /// </summary>
        /// <returns></returns>
        public Animation Copy()
        {
            Animation anim = new Animation();
            anim.Rectangles = Rectangles;
            anim.Color = Color;
            anim.Origin = Origin;
            anim.Rotation = Rotation;
            anim.Scale = Scale;
            anim.SpriteEffect = SpriteEffect;
            anim.IsLooping = IsLooping;
            anim.Frames = Frames;
            anim.Depth = Depth;
            return anim;
        }
    }
}
