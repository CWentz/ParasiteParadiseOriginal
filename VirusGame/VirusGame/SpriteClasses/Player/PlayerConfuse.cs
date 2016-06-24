using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VirusGame.SpriteClasses.Player
{
    public class PlayerConfuse : MovingSprite
    {
        private int animationTimer = 0;
        private bool confused = false;

        public PlayerConfuse(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            animation.Scale = .5f;//.4375f;
            animation.Depth = 0.059f;
            aniM.FramesPerSecond = 8;
            aniM.AddAnimation("confused", 1, _frames, animation.Copy());
            aniM.Animation = "confused";

            Type = "confused";
            rotates = false;
            body.Dispose();
            IsVisible = false;
        }

        public bool Confused
        {
            set { confused = value; }
        }

        public override void Update(GameTime gameTime)
        {

            //body.Position = Globals.getWorldPosition(position);
            //body.Rotation = rotation;

            if (IsVisible)
            {

                animationTimer++;
                if (animationTimer == 1)
                {
                    aniM.Animation = "confused";
                    confused = false;
                }

                
            }
            if (confused && animationTimer == 0)
            {
                animationTimer = 0;
                IsVisible = true;
            }

            if (animationTimer >= 36)
            {
                confused = false;
                animationTimer = 0;
                IsVisible = false;
            }

            
            aniM.Update(gameTime);

        }

    }
}
