using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace VirusGame.SpriteClasses.Menu
{
    public class FinishSprite : MovingSprite
    {

        public bool active = false;
        public bool permanentlyDisable = false;

        public FinishSprite(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            position = _position;
            //position.Y += aniM.Height / 2f;
            animation.Scale = 2.4f;
            animation.Depth = .1f;
            aniM.FramesPerSecond = 10;
            animation.IsLooping = true;
            //rotation = (float)Math.PI / 2f;
            aniM.AddAnimation("active", 1, _frames, animation.Copy());
            aniM.Animation = "active";
            Type = "loadscreen";
            rotates = false;
            body.CollidesWith = ~Category.All;
            active = true;
            IsVisible = true;
            body.Dispose();


        }


        public override void Update(GameTime gameTime)
        {
            aniM.Update(gameTime);
        }

    }
}
