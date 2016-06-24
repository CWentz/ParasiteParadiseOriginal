using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VirusGame.SpriteClasses.Player
{
    public class PlayerHeal : MovingSprite
    {
        private int animationTimer = 0;
        private bool healed = false;

        public PlayerHeal(World _level, Texture2D _texture, Vector2 _position, Vector2 _velocity, int _frames, int _animations)
            : base(_level, _texture, _position, _velocity, _frames, _animations)
        {
            animation.Scale = .5f;//.4375f;
            animation.Depth = 0.059f;
            aniM.FramesPerSecond = 14;
            aniM.AddAnimation("heal", 1, _frames, animation.Copy());
            aniM.Animation = "heal";

            Type = "heal";
            rotates = false;
            body.Dispose();
            IsVisible = false;
        }

        public bool Healed
        {
            set { healed = value; }
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
                    aniM.Animation = "heal";
                    healed = false;
                }

                
            }
            if (healed)
            {
                animationTimer = 0;
                IsVisible = true;
            }

            if (animationTimer >= 20)
            {
                healed = false;
                animationTimer = 0;
                IsVisible = false;
            }

            
            aniM.Update(gameTime);

        }

    }
}
