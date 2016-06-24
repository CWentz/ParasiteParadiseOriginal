using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VirusGame.SpriteClasses.Player
{
    public class PlayerCollect : MovingSprite
    {
        private int animationTimer = 0;
        private bool collect = false;

        public PlayerCollect(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            animation.Scale = .4375f;
            animation.Depth = 0.059f;
            aniM.FramesPerSecond = 8;
            aniM.AddAnimation("collect", 1, _frames, animation.Copy());
            aniM.Animation = "collect";

            Type = "collect";
            rotates = false;
            body.Dispose();
            IsVisible = false;
        }

        public bool Collect
        {
            set { collect = value; }
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
                    aniM.Animation = "collect";
                    collect = false;
                }


            }
            if (collect && animationTimer == 0)
            {
                animationTimer = 0;
                IsVisible = true;
            }

            if (animationTimer >= 36)
            {
                collect = false;
                animationTimer = 0;
                IsVisible = false;
            }


            aniM.Update(gameTime);

        }

    }
}
