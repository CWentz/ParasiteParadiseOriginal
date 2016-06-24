using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace VirusGame.SpriteClasses.Menu
{
    public class BubblingSprite : MovingSprite
    {

        public bool active = false;
        public bool permanentlyDisable = false;

        public BubblingSprite(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
        : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            position = new Vector2(512, 384);
            animation.Scale = 1f;
            animation.Depth = .00f;
            aniM.FramesPerSecond = 6;
            animation.IsLooping = true;
            aniM.AddAnimation("active", 1, 5, animation.Copy());
            aniM.Animation = "active";
            Type = "loadscreen";
            rotates = false;
            body.CollidesWith = ~Category.All;
            active = true;
            IsVisible = true;
            body.Dispose();
            
        }

        public Vector2 setPos
        {
            set 
            { 
                position.X = value.X - (aniM.Width / 2f);
                position.Y = value.Y - (aniM.Height / 2f);
            }
        }

        public override void Update(GameTime gameTime)
        {
            aniM.Update(gameTime);
        }

    }
}
